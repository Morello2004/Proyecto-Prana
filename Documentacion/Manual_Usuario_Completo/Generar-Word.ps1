$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'Preparar-Contenido-Manual.ps1')

# escapa los caracteres reservados para que el texto no se interprete como instrucciones del documento
function Esc([string]$v) { [System.Security.SecurityElement]::Escape($v) }

# prepara c le an con los datos recibidos y devuelve el resultado al generador
function Clean([string]$html) {
 
 $html = [regex]::Replace($html, '<aside>[^<]*(?:<[^>]+>[^<]*)*?En Stock, la aplicación revisada.*?</aside>', '', 'Singleline')
 
 $html = [regex]::Replace($html, '<p>[^<]*Fuente revisada:.*?</p>', '', 'Singleline')
 
 $html = $html.Replace('La interfaz revisada', 'Prana').Replace('La versión revisada','Prana').Replace('la aplicación revisada','Prana').Replace('según permisos definidos','con autorización del administrador').Replace('controlados por negocio','').Replace('El límite técnico','El máximo permitido').Replace('Conserva el registro histórico','Conserva el historial').Replace('La consulta de deuda usa el identificador del cliente, lo que permite distinguir personas con el mismo nombre.', 'Si hay personas con el mismo nombre, verificá el documento antes de elegir al cliente.').Replace('La instalación local usa archivos de datos asociados a la cuenta de Windows.', '')
 
 return $html
}

$pkg = Join-Path $root ('word-temporal-' + [guid]::NewGuid().ToString('N'))

foreach($d in @('_rels','word','word/_rels','word/media')) { [void][IO.Directory]::CreateDirectory((Join-Path $pkg $d)) }

$utf = [Text.UTF8Encoding]::new($false)

# prepara w ri te pa rt con los datos recibidos y devuelve el resultado al generador
function WritePart($path,$text) { [IO.File]::WriteAllText((Join-Path $pkg $path),$text,$utf) }

$body = [Text.StringBuilder]::new()

# prepara p ar a con los datos recibidos y devuelve el resultado al generador
function Para($text,$style='Normal') {
 
 if([string]::IsNullOrWhiteSpace($text)){return}
 
 [void]$body.Append('<w:p><w:pPr><w:pStyle w:val="'+$style+'"/></w:pPr><w:r><w:t xml:space="preserve">'+(Esc $text)+'</w:t></w:r></w:p>')
}

# prepara t ex to h tm l con los datos recibidos y devuelve el resultado al generador
function Texto-Html([string]$html) {
    return [regex]::Replace([Net.WebUtility]::HtmlDecode([regex]::Replace($html, '<[^>]+>', ' ')), '\s+', ' ').Trim()
}

# convierte filas y encabezados en una tabla del documento conservando sus valores
function Tabla-Word([string]$html) {
    [void]$body.Append('<w:tbl><w:tblPr><w:tblW w:w="0" w:type="auto"/><w:tblBorders><w:top w:val="single" w:sz="4" w:color="CEDAE2"/><w:left w:val="single" w:sz="4" w:color="CEDAE2"/><w:bottom w:val="single" w:sz="4" w:color="CEDAE2"/><w:right w:val="single" w:sz="4" w:color="CEDAE2"/><w:insideH w:val="single" w:sz="4" w:color="CEDAE2"/><w:insideV w:val="single" w:sz="4" w:color="CEDAE2"/></w:tblBorders></w:tblPr>')
    foreach ($fila in [regex]::Matches($html, '<tr\b[^>]*>(.*?)</tr>', 'Singleline')) {
        $encabezado = $fila.Groups[1].Value -match '<th\b'
        [void]$body.Append('<w:tr><w:trPr><w:cantSplit/>')
        if ($encabezado) { [void]$body.Append('<w:tblHeader/>') }
        [void]$body.Append('</w:trPr>')
        foreach ($celda in [regex]::Matches($fila.Groups[1].Value, '<t[dh]\b[^>]*>(.*?)</t[dh]>', 'Singleline')) {
            [void]$body.Append('<w:tc><w:tcPr><w:tcW w:w="0" w:type="auto"/><w:tcMar><w:top w:w="55" w:type="dxa"/><w:left w:w="55" w:type="dxa"/><w:bottom w:w="55" w:type="dxa"/><w:right w:w="55" w:type="dxa"/></w:tcMar>')
            if ($encabezado) { [void]$body.Append('<w:shd w:fill="14344A"/>') }
            [void]$body.Append('</w:tcPr><w:p><w:r><w:rPr><w:sz w:val="19"/>')
            if ($encabezado) { [void]$body.Append('<w:b/><w:color w:val="FFFFFF"/>') }
            [void]$body.Append('</w:rPr><w:t xml:space="preserve">'+(Esc (Texto-Html $celda.Groups[1].Value))+'</w:t></w:r></w:p></w:tc>')
        }
        [void]$body.Append('</w:tr>')
    }
    [void]$body.Append('</w:tbl><w:p/>')
}

# inserta un salto de pagina en el documento word
function PageBreak { [void]$body.Append('<w:p><w:r><w:br w:type="page"/></w:r></w:p>') }

$rels = [Text.StringBuilder]::new()

[void]$rels.Append('<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="styles" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/><Relationship Id="footer" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/footer" Target="footer1.xml"/>')

Para 'PRANA ELECTRÓNICA' 'Subtitle'

Para 'Manual de capacitación y operación' 'Title'

Para 'Instalación · Ventas · Cobros · Stock · Entregas · Caja' 'Subtitle'

Para 'Guía paso a paso con capturas e indicaciones numeradas.'

Para 'Edición: 12 de noviembre de 2026'

Para 'Para vendedores y administradores del comercio. Los importes y documentos de ejemplo se utilizan únicamente para capacitación.'

PageBreak

Para 'Índice de consulta' 'Heading1'

for($i=0;$i -lt $chapters.Count;$i++){ Para (('{0:00}. ' -f ($i+1)) + $chapters[$i].title) }

$figure = 0
$figurasManual = New-Object 'System.Collections.Generic.List[object]'
$carpetaFiguras = Join-Path $root 'imagenes_edicion'
[void][IO.Directory]::CreateDirectory($carpetaFiguras)

for($i=0;$i -lt $chapters.Count;$i++) {
 
 $ch = $chapters[$i]
 
 if ($i -eq 0) { PageBreak }
 
 Para (('{0:00}. ' -f ($i+1))+$ch.title) 'Heading1'
 
 if($ch.img) {
  
  $path = Join-Path $root ('imagenes/'+$ch.img+'.png')
  
  if(Test-Path $path) {
   
   $figure++
   
   $source = [Drawing.Image]::FromFile($path)
   
   $rect = [Drawing.Rectangle]::new(0,0,$source.Width,$source.Height)
   
   if($ch.crop){$rect=[Drawing.Rectangle]::new([int]$ch.crop[0],[int]$ch.crop[1],[int]$ch.crop[2],[int]$ch.crop[3])}
   
   $bmp = [Drawing.Bitmap]::new($rect.Width,$rect.Height)
   
   $g=[Drawing.Graphics]::FromImage($bmp)
   
   $g.DrawImage($source,[Drawing.Rectangle]::new(0,0,$rect.Width,$rect.Height),$rect,[Drawing.GraphicsUnit]::Pixel)
   
   $number=0
   
   foreach($mark in $ch.marks){
    
    $number++; $x=[int]$mark[0]-$rect.X; $y=[int]$mark[1]-$rect.Y
    
    if($x -ge 0 -and $y -ge 0 -and $x -lt $rect.Width -and $y -lt $rect.Height){
     
     $cx=[Math]::Min($rect.Width-22,[Math]::Max(22,$x+55));$cy=[Math]::Max(22,$y-40)
     
     $pen=[Drawing.Pen]::new([Drawing.Color]::Orange,4);$g.DrawLine($pen,$cx,$cy,$x,$y);$pen.Dispose()
     
     $g.FillEllipse([Drawing.Brushes]::DarkOrange,$cx-17,$cy-17,34,34)
     
     $font=[Drawing.Font]::new('Arial',16,[Drawing.FontStyle]::Bold)
     
     $g.DrawString([string]$number,$font,[Drawing.Brushes]::White,$cx-10,$cy-13);$font.Dispose()
    }
   }
   
   $dest=Join-Path $pkg ('word/media/fig'+$figure+'.png');$bmp.Save($dest,[Drawing.Imaging.ImageFormat]::Png)
   $bmp.Save((Join-Path $carpetaFiguras ('fig'+$figure+'.png')),[Drawing.Imaging.ImageFormat]::Png)
   $figurasManual.Add([pscustomobject]@{capitulo=$i;numero=$figure;archivo=('imagenes_edicion/fig'+$figure+'.png');ancho=$rect.Width;alto=$rect.Height})
   
   $width=6096000; $height=[long]($width*$rect.Height/$rect.Width)
   if ($height -gt 2448000) { $width=[long]($width*2448000/$height); $height=2448000 }
   
   if($height -gt 3657600){$width=[long]($width*3657600/$height);$height=3657600}
   
   $g.Dispose();$bmp.Dispose();$source.Dispose()
   
   [void]$rels.Append('<Relationship Id="img'+$figure+'" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/image" Target="media/fig'+$figure+'.png"/>')
   
   [void]$body.Append('<w:p><w:pPr><w:jc w:val="center"/><w:keepNext/></w:pPr><w:r><w:drawing><wp:inline distT="0" distB="0" distL="0" distR="0"><wp:extent cx="'+$width+'" cy="'+$height+'"/><wp:docPr id="'+$figure+'" name="Figura '+$figure+'" descr="'+(Esc $ch.title)+'"/><a:graphic xmlns:a="http://schemas.openxmlformats.org/drawingml/2006/main"><a:graphicData uri="http://schemas.openxmlformats.org/drawingml/2006/picture"><pic:pic xmlns:pic="http://schemas.openxmlformats.org/drawingml/2006/picture"><pic:nvPicPr><pic:cNvPr id="0" name="Captura"/><pic:cNvPicPr/></pic:nvPicPr><pic:blipFill><a:blip r:embed="img'+$figure+'"/><a:stretch><a:fillRect/></a:stretch></pic:blipFill><pic:spPr><a:xfrm><a:off x="0" y="0"/><a:ext cx="'+$width+'" cy="'+$height+'"/></a:xfrm><a:prstGeom prst="rect"><a:avLst/></a:prstGeom></pic:spPr></pic:pic></a:graphicData></a:graphic></wp:inline></w:drawing></w:r></w:p>')
   
   Para ('Figura '+$figure+'. '+$ch.title+'.') 'Caption'
   
   $n=0;foreach($mark in $ch.marks){$n++;Para ($n.ToString()+'. '+[string]$mark[2]) 'Caption'}
  }
 }
 
 $html = Clean $ch.body
 
 $html = $html.Replace('Unidades reservadas al preparar entregas.', 'Unidades vendidas que todavía están pendientes de preparación.').Replace('Stock reservado para entregas preparadas.', 'Unidades vendidas pendientes de preparación.').Replace('Stock físico menos comprometido.', 'Unidades que pueden asignarse a la preparación; comprobá la disponibilidad antes de confirmar.').Replace('Informá el texto, hora y módulo al responsable técnico. Verificará motor e instalación.', 'Informá el texto, hora y sección al responsable para revisar la instalación.')
 
 $blocks = [regex]::Matches($html,'<(table|ol|ul|p|h2|h3|aside|div)\b[^>]*>(.*?)</\1>','Singleline')
 
 $step=0
 
 foreach($block in $blocks){
  
  $tag=$block.Groups[1].Value
  if ($tag -eq 'table') { Tabla-Word $block.Value; continue }
  if ($tag -eq 'ol' -or $tag -eq 'ul') {
      $numeroPaso = 0
      foreach ($item in [regex]::Matches($block.Groups[2].Value,'<li\b[^>]*>(.*?)</li>','Singleline')) {
          $numeroPaso++
          $prefijo = '• '
          if ($tag -eq 'ol') { $prefijo = $numeroPaso.ToString()+'. ' }
          Para ($prefijo + (Texto-Html $item.Groups[1].Value))
      }
      continue
  }
  
  $text=[Net.WebUtility]::HtmlDecode([regex]::Replace($block.Groups[2].Value,'<[^>]+>',' '))
  
  $text=[regex]::Replace($text,'\s+',' ').Trim().TrimEnd('|').Trim()
  
  if($text -match 'código de la|catálogo de permisos|repositorio|\bIDE\b|renderiz|validación cuenta las filas|aplica de forma uniforme|fuente revisada'){continue}
  
  $style='Normal'
  
  if($tag -eq 'h2' -or $tag -eq 'h3'){$style='Heading2'}
  
  if($tag -eq 'aside'){$style='Advice';$text='Recordá: '+$text}
  
  if($tag -eq 'li'){$step++;$text=$step.ToString()+'. '+$text}
  
  Para $text $style
 }
}

[void]$body.Append('<w:sectPr><w:footerReference w:type="default" r:id="footer"/><w:pgSz w:w="11906" w:h="16838"/><w:pgMar w:top="1020" w:right="1020" w:bottom="1020" w:left="1020" w:header="480" w:footer="480"/></w:sectPr>')

WritePart 'word/document.xml' ('<?xml version="1.0" encoding="utf-8"?><w:document xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships" xmlns:wp="http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"><w:body>'+$body.ToString()+'</w:body></w:document>')

[void]$rels.Append('</Relationships>');WritePart 'word/_rels/document.xml.rels' $rels.ToString()

WritePart '_rels/.rels' '<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="document" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/></Relationships>'

WritePart '[Content_Types].xml' '<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types"><Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/><Default Extension="xml" ContentType="application/xml"/><Default Extension="png" ContentType="image/png"/><Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/><Override PartName="/word/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml"/><Override PartName="/word/footer1.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.footer+xml"/></Types>'

WritePart 'word/styles.xml' @'
<w:styles xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
<w:docDefaults><w:rPrDefault><w:rPr><w:rFonts w:ascii="Calibri" w:hAnsi="Calibri"/><w:sz w:val="21"/><w:lang w:val="es-AR"/></w:rPr></w:rPrDefault><w:pPrDefault><w:pPr><w:spacing w:after="80" w:line="245" w:lineRule="auto"/></w:pPr></w:pPrDefault></w:docDefaults>
<w:style w:type="paragraph" w:default="1" w:styleId="Normal"><w:name w:val="Normal"/></w:style>
<w:style w:type="paragraph" w:styleId="Title"><w:name w:val="Title"/><w:pPr><w:spacing w:before="1400" w:after="500"/></w:pPr><w:rPr><w:b/><w:color w:val="14344A"/><w:sz w:val="64"/></w:rPr></w:style>
<w:style w:type="paragraph" w:styleId="Subtitle"><w:name w:val="Subtitle"/><w:rPr><w:color w:val="087D8E"/><w:sz w:val="28"/></w:rPr></w:style>
<w:style w:type="paragraph" w:styleId="Heading1"><w:name w:val="heading 1"/><w:pPr><w:keepNext/><w:outlineLvl w:val="0"/><w:spacing w:before="180" w:after="140"/></w:pPr><w:rPr><w:b/><w:color w:val="14344A"/><w:sz w:val="32"/></w:rPr></w:style>
<w:style w:type="paragraph" w:styleId="Heading2"><w:name w:val="heading 2"/><w:pPr><w:keepNext/><w:outlineLvl w:val="1"/></w:pPr><w:rPr><w:b/><w:color w:val="087D8E"/><w:sz w:val="26"/></w:rPr></w:style>
<w:style w:type="paragraph" w:styleId="Caption"><w:name w:val="Caption"/><w:rPr><w:i/><w:color w:val="52677B"/><w:sz w:val="18"/></w:rPr></w:style>
<w:style w:type="paragraph" w:styleId="Advice"><w:name w:val="Recordatorio"/><w:pPr><w:shd w:fill="EAF6F6"/><w:spacing w:before="150" w:after="200"/></w:pPr><w:rPr><w:color w:val="14344A"/></w:rPr></w:style>
</w:styles>
'@

WritePart 'word/footer1.xml' '<w:ftr xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main"><w:p><w:pPr><w:jc w:val="right"/></w:pPr><w:r><w:rPr><w:sz w:val="16"/><w:color w:val="52677B"/></w:rPr><w:t>PRANA ELECTRÓNICA · Manual de usuario · </w:t></w:r><w:fldSimple w:instr="PAGE"/></w:p></w:ftr>'

$out=Join-Path $root 'Manual_de_Usuario_Completo_Prana.docx'

if(Test-Path $out){[IO.File]::Delete($out)}

$archive=[IO.Compression.ZipFile]::Open($out,[IO.Compression.ZipArchiveMode]::Create)

try {
 
 foreach($file in Get-ChildItem -LiteralPath $pkg -Recurse -File) {
  
  $name=$file.FullName.Substring($pkg.Length+1).Replace('\','/')
  
  [void][IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive,$file.FullName,$name)
 }

} finally { $archive.Dispose() }
# verifica que todas las partes xml sean legibles y que las figuras esten incluidas asigna el resultado a una variable o propiedad las variables con $ conservan los datos usados en los pasos siguientes
$zip=[IO.Compression.ZipFile]::OpenRead($out)

foreach($entry in $zip.Entries){if($entry.FullName -match '\.(xml|rels)$'){ $reader=[IO.StreamReader]::new($entry.Open());try{[void]([xml]$reader.ReadToEnd())}finally{$reader.Dispose()} }}

$zip.Dispose()

Write-Output ('Word generado: '+$out)

Write-Output ('Capitulos: '+$chapters.Count+'; figuras: '+$figure)
$chapters | ConvertTo-Json -Depth 30 | Set-Content (Join-Path $root 'contenido_edicion.json') -Encoding UTF8
$figurasManual | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $root 'figuras_edicion.json') -Encoding UTF8
