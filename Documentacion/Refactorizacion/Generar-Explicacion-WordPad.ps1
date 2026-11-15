param(
    [string]$Carpeta = $PSScriptRoot
)

$ErrorActionPreference = 'Stop'
$origen = Join-Path $Carpeta 'Explicacion_Tecnica_Formularios.md'
$destino = Join-Path $Carpeta 'Explicacion_Tecnica_Formularios_WordPad.rtf'
$contenido = [System.IO.File]::ReadAllLines($origen, [System.Text.Encoding]::UTF8)

# protege los caracteres especiales de rtf y conserva letras con acento mediante unicode
function Convertir-TextoRtf([string]$Texto) {
    $resultado = New-Object System.Text.StringBuilder
    foreach ($caracter in $Texto.ToCharArray()) {
        $codigo = [int]$caracter
        if ($caracter -eq '\' -or $caracter -eq '{' -or $caracter -eq '}') {
            [void]$resultado.Append('\').Append($caracter)
        }
        elseif ($codigo -gt 127) {
            if ($codigo -gt 32767) { $codigo -= 65536 }
            [void]$resultado.Append('\u').Append($codigo).Append('?')
        }
        else {
            [void]$resultado.Append($caracter)
        }
    }
    return $resultado.ToString()
}

$documento = New-Object System.Text.StringBuilder
[void]$documento.AppendLine('{\rtf1\ansi\ansicpg1252\deff0\uc1{\fonttbl{\f0 Arial;}{\f1 Consolas;}}{\colortbl;\red20\green52\blue74;}')
[void]$documento.AppendLine('\paperw11907\paperh16840\margl1134\margr1134\margt1134\margb1134\viewkind4 ')
foreach ($linea in $contenido) {
    if ([string]::IsNullOrWhiteSpace($linea)) { continue }
    $texto = $linea
    $tamano = 22
    $negrita = '\b0 '
    $espacioAntes = 0
    $color = '\cf0 '
    $sangria = 0
    $mantener = ''
    if ($linea -match '^(#{1,4})\s+(.+)$') {
        $nivel = $Matches[1].Length
        $texto = $Matches[2]
        $negrita = '\b '
        $espacioAntes = 220
        $color = '\cf1 '
        $mantener = '\keepn '
        if ($nivel -eq 1) { $tamano = 36 }
        elseif ($nivel -eq 2) { $tamano = 32; $espacioAntes = 420 }
        elseif ($nivel -eq 3) { $tamano = 26 }
        else { $tamano = 23; $sangria = 180 }
    }
    [void]$documento.Append('\pard\f0\fs').Append($tamano).Append('\li').Append($sangria).Append('\sb').Append($espacioAntes).Append('\sa140\sl276\slmult1 ').Append($negrita).Append($color).Append($mantener)
    foreach ($parte in [regex]::Split($texto, '(`[^`]+`|\*\*[^*]+\*\*)')) {
        if ($parte.StartsWith('`') -and $parte.EndsWith('`') -and $parte.Length -ge 2) {
            [void]$documento.Append('{\f1 ').Append((Convertir-TextoRtf $parte.Substring(1, $parte.Length - 2))).Append('}')
        }
        elseif ($parte.StartsWith('**') -and $parte.EndsWith('**') -and $parte.Length -ge 4) {
            [void]$documento.Append('{\b ').Append((Convertir-TextoRtf $parte.Substring(2, $parte.Length - 4))).Append('}')
        }
        else {
            [void]$documento.Append((Convertir-TextoRtf $parte))
        }
    }
    [void]$documento.AppendLine('\par')
}
[void]$documento.AppendLine('}')
[System.IO.File]::WriteAllText($destino, $documento.ToString(), [System.Text.Encoding]::ASCII)

# abre el archivo con el lector rtf de windows forms y compara todo el texto con el original
Add-Type -AssemblyName System.Windows.Forms
$lector = New-Object System.Windows.Forms.RichTextBox
try {
    $lector.LoadFile($destino, [System.Windows.Forms.RichTextBoxStreamType]::RichText)
    $esperado = ($contenido | Where-Object { -not [string]::IsNullOrWhiteSpace($_) } | ForEach-Object {
        (($_ -replace '^#{1,4}\s+', '') -replace '`', '') -replace '\*\*', ''
    }) -join "`n"
    if ($lector.Text.TrimEnd() -cne $esperado.TrimEnd()) {
        throw 'El texto del RTF no coincide con la explicacion original'
    }
    Write-Output "RTF verificado con texto completo: $destino"
}
finally {
    $lector.Dispose()
}
