param([switch]$SoloHtml)
$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
# genera primero el word y reutiliza el mismo contenido y las mismas figuras para el html y el pdf
& (Join-Path $root 'Generar-Word.ps1')
$capitulos = Get-Content (Join-Path $root 'contenido_edicion.json') -Raw -Encoding UTF8 | ConvertFrom-Json
$figuras = Get-Content (Join-Path $root 'figuras_edicion.json') -Raw -Encoding UTF8 | ConvertFrom-Json
$html = New-Object Text.StringBuilder
# centraliza tipografia margenes tablas y tamanos de imagen para mantener compacta la edicion impresa
$estilos = @'
@page{size:A4;margin:12mm 13mm 16mm}*{box-sizing:border-box}body{font:10.5pt/1.25 Arial,sans-serif;color:#24344a;margin:0;background:#e8eef3}main{max-width:180mm;margin:auto;background:white;padding:10mm}h1{font-size:17pt;color:#14344a;line-height:1.15;break-after:avoid}h2{font-size:12pt;color:#087d8e;break-after:avoid;margin-top:4mm}h3{font-size:12pt;color:#087d8e;break-after:avoid}p{margin:1.8mm 0;orphans:3;widows:3}li{margin:1mm 0}aside{background:#eaf6f6;border-left:4px solid #087d8e;padding:1.8mm;margin:2.5mm 0}table{width:100%;border-collapse:collapse;font-size:9pt;margin:3mm 0}th{background:#14344a;color:white;text-align:left}th,td{border:1px solid #ccd9e2;padding:1.8mm;vertical-align:top}tr{break-inside:avoid}thead{display:table-header-group}tr:nth-child(even){background:#f4f8fa}figure{margin:2.5mm 0;break-inside:avoid}figure img{display:block;max-width:100%;max-height:68mm;object-fit:contain;margin:auto}figcaption,.referencias{font-size:9pt;color:#52677b}.referencias{display:flex;gap:2mm 5mm;flex-wrap:wrap;margin:2mm 0 3mm}.referencias span{display:block}.capitulo{padding-top:2mm}.portada{min-height:240mm;padding:25mm 5mm;color:#14344a}.portada h1{font-size:40pt;margin-top:25mm}.portada .subtitulo{font-size:20pt;color:#087d8e}.indice{break-before:page}.indice ol{columns:2;column-gap:8mm;padding-left:6mm}.indice li{font-size:10pt;break-inside:avoid;margin-bottom:3mm}a{color:#14344a;text-decoration:none}.volver{font-size:9pt;color:#087d8e;margin-top:4mm}h1,h2,h3{scroll-margin-top:10mm}@media print{body{background:white}main{padding:0;max-width:none}.volver{display:none}}@media screen{main{box-shadow:0 1px 12px #aab8c6}section{margin-bottom:14mm}}
'@
[void]$html.Append('<!doctype html><html lang="es"><head><meta charset="utf-8"><title>PRANA · Manual completo de empleados</title><style>'+$estilos+'</style></head><body><main>')
[void]$html.Append('<section class="portada"><p>PRANA ELECTRÓNICA</p><h1>Manual de capacitación y operación</h1><p class="subtitulo">Instalación · Ventas · Cobros · Stock · Entregas · Caja</p><p>Edición: 12 de noviembre de 2026</p><p>Para vendedores y administradores. Instrucciones por pantalla, capturas actuales, ejemplos y resolución de problemas.</p><p>Los importes, clientes y documentos de ejemplo son ficticios y se utilizan solamente para aprender.</p></section><section class="indice" id="indice"><h1>Índice de consulta</h1><ol>')
for ($i=0;$i -lt $capitulos.Count;$i++) { [void]$html.Append('<li><a href="#capitulo-'+$i+'">'+[Net.WebUtility]::HtmlEncode($capitulos[$i].title)+'</a></li>') }
[void]$html.Append('</ol></section>')
for ($i=0;$i -lt $capitulos.Count;$i++) {
    # incorpora cada seccion con su captura sus referencias numeradas y su explicacion
    $capitulo = $capitulos[$i]
    [void]$html.Append('<section class="capitulo" id="capitulo-'+$i+'"><h1>'+('{0:00}. ' -f ($i+1))+[Net.WebUtility]::HtmlEncode($capitulo.title)+'</h1>')
    $figura = $figuras | Where-Object capitulo -eq $i | Select-Object -First 1
    if ($figura) {
        $datosImagen = [Convert]::ToBase64String([IO.File]::ReadAllBytes((Join-Path $root $figura.archivo)))
        [void]$html.Append('<figure><img alt="'+[Net.WebUtility]::HtmlEncode($capitulo.title)+'" src="data:image/png;base64,'+$datosImagen+'"><figcaption>Figura '+$figura.numero+'. '+[Net.WebUtility]::HtmlEncode($capitulo.title)+'.</figcaption></figure><div class="referencias">')
        $numero=0
        foreach ($marca in $capitulo.marks) { $numero++; [void]$html.Append('<span><b>'+$numero+'.</b> '+[Net.WebUtility]::HtmlEncode([string]$marca[2])+'</span>') }
        [void]$html.Append('</div>')
    }
    [void]$html.Append($capitulo.body+'<p class="volver"><a href="#indice">Volver al índice</a></p></section>')
}
[void]$html.Append('</main></body></html>')
$rutaHtml = Join-Path $root 'Manual_de_Usuario_Completo_Prana.html'
# guarda un html autonomo con las imagenes incorporadas y lo imprime con el navegador si se solicita pdf
[IO.File]::WriteAllText($rutaHtml,$html.ToString(),[Text.UTF8Encoding]::new($false))
Write-Output "HTML generado: $rutaHtml"
if (!$SoloHtml) { & (Join-Path $root 'Exportar-Browser.ps1') }
