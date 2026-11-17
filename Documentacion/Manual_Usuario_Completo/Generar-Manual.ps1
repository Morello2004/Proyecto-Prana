param([switch]$SoloHtml)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem
Add-Type -AssemblyName System.IO.Compression
$root = $PSScriptRoot
$docx = Join-Path $root 'Manual_de_Usuario_Completo_Prana.docx'
$zip = [IO.Compression.ZipFile]::Open($docx, [IO.Compression.ZipArchiveMode]::Update)
try {
    $entry = $zip.GetEntry('word/document.xml')
    $reader = [IO.StreamReader]::new($entry.Open())
    try { $xml = $reader.ReadToEnd() } finally { $reader.Dispose() }
    $xml = $xml.Replace('Para descartar la carga, utilizá Cancelar.', 'Si no vas a crear el cliente, borrá los datos ingresados sin presionar Crear cliente.')
    $xml = $xml.Replace(', localidad y código postal', ', localidad').Replace(', Localidad y Código postal', ' y Localidad').Replace('El lista de productos', 'La lista de productos')
    $entry.Delete()
    $writer = [IO.StreamWriter]::new($zip.CreateEntry('word/document.xml').Open(), [Text.UTF8Encoding]::new($false))
    try { $writer.Write($xml) } finally { $writer.Dispose() }
    [xml]$document = $xml
    $ns = [Xml.XmlNamespaceManager]::new($document.NameTable)
    $ns.AddNamespace('w','http://schemas.openxmlformats.org/wordprocessingml/2006/main')
    $ns.AddNamespace('a','http://schemas.openxmlformats.org/drawingml/2006/main')
    $ns.AddNamespace('r','http://schemas.openxmlformats.org/officeDocument/2006/relationships')
    $reader = [IO.StreamReader]::new($zip.GetEntry('word/_rels/document.xml.rels').Open())
    try { [xml]$relationships = $reader.ReadToEnd() } finally { $reader.Dispose() }
    $images = @{}
    foreach ($rel in $relationships.DocumentElement.ChildNodes) {
        if ($rel.Type -like '*/image') {
            $memory = [IO.MemoryStream]::new()
            $stream = $zip.GetEntry('word/'+$rel.Target).Open()
            try { $stream.CopyTo($memory); $images[$rel.Id] = [Convert]::ToBase64String($memory.ToArray()) } finally { $stream.Dispose(); $memory.Dispose() }
        }
    }
    $script:heading = 0
    function Block($node) {
        if ($node.LocalName -eq 'tbl') {
            $table = '<table>'
            foreach ($row in $node.SelectNodes('w:tr',$ns)) {
                $table += '<tr>'
                foreach ($cell in $row.SelectNodes('w:tc',$ns)) {
                    $table += '<td>'
                    foreach ($part in $cell.ChildNodes) { $table += Block $part }
                    $table += '</td>'
                }
                $table += '</tr>'
            }
            return $table+'</table>'
        }
        if ($node.LocalName -ne 'p') { return '' }
        $picture = $node.SelectSingleNode('.//a:blip',$ns)
        if ($picture) {
            $id = $picture.GetAttribute('embed','http://schemas.openxmlformats.org/officeDocument/2006/relationships')
            return '<figure><img alt="Captura de pantalla de Prana" src="data:image/png;base64,'+$images[$id]+'"></figure>'
        }
        $text = ($node.SelectNodes('.//w:t',$ns) | ForEach-Object InnerText) -join ''
        if (!$text) { return '' }
        $text = [Net.WebUtility]::HtmlEncode($text)
        $style = $node.SelectSingleNode('w:pPr/w:pStyle',$ns)
        $name = if ($style) { $style.GetAttribute('val','http://schemas.openxmlformats.org/wordprocessingml/2006/main') } else { '' }
        if ($script:heading -eq 1 -and $name -ne 'Heading1' -and $text -match '^(\d{2})\. ') {
            return '<p><a href="#tema-'+([int]$Matches[1]+1)+'">'+$text+'</a></p>'
        }
        switch ($name) {
            'Title' { return '<header><p>PRANA ELECTRÓNICA</p><h1 class="title">'+$text+'</h1></header>' }
            'Subtitle' { return '<p class="subtitle">'+$text+'</p>' }
            'Heading1' { $script:heading++; return '<h1 id="tema-'+$script:heading+'">'+$text+'</h1>' }
            'Heading2' { return '<h2>'+$text+'</h2>' }
            'Caption' { return '<p class="caption">'+$text+'</p>' }
            'Advice' { return '<aside>'+$text+'</aside>' }
            default { return '<p>'+$text+'</p>' }
        }
    }
    $html = [Text.StringBuilder]::new()
    [void]$html.Append('<!doctype html><html lang="es"><head><meta charset="utf-8"><title>Prana · Manual de usuario · 17/11/2026</title><style>@page{size:A4;margin:12mm 13mm 16mm}*{box-sizing:border-box}body{font:10.5pt/1.25 Arial,sans-serif;color:#24344a;margin:0;background:#e8eef3}main{max-width:180mm;margin:auto;background:white;padding:10mm}h1{font-size:17pt;color:#14344a;break-after:avoid;margin-top:6mm}h2{font-size:12pt;color:#087d8e;break-after:avoid}p{margin:1.8mm 0;orphans:3;widows:3}aside{background:#eaf6f6;border-left:4px solid #087d8e;padding:2mm;margin:3mm 0}table{width:100%;border-collapse:collapse;font-size:9pt;margin:3mm 0}td{border:1px solid #ccd9e2;padding:1.8mm;vertical-align:top}tr{break-inside:avoid}tr:first-child{background:#14344a;color:white}figure{margin:3mm 0;break-inside:avoid}img{display:block;max-width:100%;max-height:68mm;object-fit:contain;margin:auto}.caption{font-size:9pt;color:#52677b}.title{font-size:34pt;margin-top:30mm}.subtitle{font-size:16pt;color:#087d8e}header{padding-top:10mm}@media print{body{background:white}main{padding:0;max-width:none}}@media screen{main{box-shadow:0 1px 12px #aab8c6}}</style></head><body><main>')
    foreach ($node in $document.SelectSingleNode('//w:body',$ns).ChildNodes) { [void]$html.Append((Block $node)) }
    [void]$html.Append('</main></body></html>')
    $htmlPath = Join-Path $root 'Manual_de_Usuario_Completo_Prana.html'
    [IO.File]::WriteAllText($htmlPath,$html.ToString(),[Text.UTF8Encoding]::new($false))
} finally { $zip.Dispose() }
if (!$SoloHtml) {
    $edge = 'C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe'
    $pdfPath = Join-Path $root 'Manual_de_Usuario_Completo_Prana.pdf'
    $profile = Join-Path ([IO.Path]::GetTempPath()) ('PranaManual-'+[guid]::NewGuid().ToString('N'))
    $process = Start-Process $edge -WindowStyle Hidden -PassThru -RedirectStandardError (Join-Path $root 'exportacion_pdf.log') -ArgumentList @('--headless=new','--disable-gpu','--in-process-gpu','--no-sandbox','--no-first-run','--no-pdf-header-footer',('--user-data-dir="'+$profile+'"'),('--print-to-pdf="'+$pdfPath+'"'),([Uri]::new($htmlPath).AbsoluteUri))
    if (!$process.WaitForExit(45000)) { Stop-Process -Id $process.Id; throw 'Se agotó el plazo de exportación a PDF.' }
    if (!(Test-Path $pdfPath) -or (Get-Item $pdfPath).LastWriteTime -lt (Get-Item $htmlPath).LastWriteTime) { throw 'No se actualizó el PDF.' }
}
Write-Output 'Manual actualizado desde Word: HTML y PDF generados.'
