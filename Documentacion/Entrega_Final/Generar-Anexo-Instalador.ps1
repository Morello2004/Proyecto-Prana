param([string]$Nombre = '06_Instalacion_y_Distribucion_Prana')

$ErrorActionPreference='Stop'

Add-Type -AssemblyName System.IO.Compression.FileSystem

Add-Type -AssemblyName System.IO.Compression

$root=$PSScriptRoot

$name=$Nombre

$lines=Get-Content (Join-Path $root ($name+'.md')) -Encoding UTF8

$builder=[Text.StringBuilder]::new()

$inCode=$false

foreach($line in $lines){
 
 if($line.StartsWith('```')){$inCode=!$inCode;continue}
 
 if([string]::IsNullOrWhiteSpace($line)){continue}
 
 $style='Normal';$text=$line
 
 if($line.StartsWith('# ')){$style='Title';$text=$line.Substring(2)}
 
 elseif($line.StartsWith('## ')){$style='Heading1';$text=$line.Substring(3)}
 
 elseif($line.StartsWith('- ')){$text='• '+$line.Substring(2)}
 
 if($inCode){$style='Code'}
 
 $escaped=[Security.SecurityElement]::Escape($text)
 
 [void]$builder.Append('<w:p><w:pPr><w:pStyle w:val="'+$style+'"/></w:pPr><w:r><w:t xml:space="preserve">'+$escaped+'</w:t></w:r></w:p>')
}

$parts=@{}

$parts['[Content_Types].xml']='<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types"><Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/><Default Extension="xml" ContentType="application/xml"/><Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/><Override PartName="/word/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml"/><Override PartName="/word/footer.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.footer+xml"/></Types>'

$parts['_rels/.rels']='<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="doc" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/></Relationships>'

$parts['word/_rels/document.xml.rels']='<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="styles" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/><Relationship Id="footer" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/footer" Target="footer.xml"/></Relationships>'

$parts['word/document.xml']='<w:document xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships"><w:body>'+$builder.ToString()+'<w:sectPr><w:footerReference w:type="default" r:id="footer"/><w:pgSz w:w="11906" w:h="16838"/><w:pgMar w:top="1134" w:right="1134" w:bottom="1134" w:left="1134" w:footer="500"/></w:sectPr></w:body></w:document>'

$parts['word/styles.xml']='<w:styles xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main"><w:docDefaults><w:rPrDefault><w:rPr><w:rFonts w:ascii="Calibri" w:hAnsi="Calibri"/><w:sz w:val="22"/><w:lang w:val="es-AR"/></w:rPr></w:rPrDefault><w:pPrDefault><w:pPr><w:spacing w:after="150" w:line="276" w:lineRule="auto"/></w:pPr></w:pPrDefault></w:docDefaults><w:style w:type="paragraph" w:default="1" w:styleId="Normal"><w:name w:val="Normal"/></w:style><w:style w:type="paragraph" w:styleId="Title"><w:name w:val="Title"/><w:rPr><w:b/><w:sz w:val="42"/><w:color w:val="14344A"/></w:rPr></w:style><w:style w:type="paragraph" w:styleId="Heading1"><w:name w:val="heading 1"/><w:pPr><w:keepNext/><w:outlineLvl w:val="0"/><w:spacing w:before="280" w:after="180"/></w:pPr><w:rPr><w:b/><w:sz w:val="28"/><w:color w:val="087D8E"/></w:rPr></w:style><w:style w:type="paragraph" w:styleId="Code"><w:name w:val="Comando"/><w:pPr><w:shd w:fill="EDF2F5"/></w:pPr><w:rPr><w:rFonts w:ascii="Consolas" w:hAnsi="Consolas"/><w:sz w:val="18"/></w:rPr></w:style></w:styles>'

$parts['word/footer.xml']='<w:ftr xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main"><w:p><w:pPr><w:jc w:val="right"/></w:pPr><w:r><w:t>Prana · Prácticas Profesionalizantes III · </w:t></w:r><w:fldSimple w:instr="PAGE"/></w:p></w:ftr>'

$out=Join-Path $root ($name+'.docx')

if(Test-Path -LiteralPath $out){[IO.File]::Delete($out)}

$archive=[IO.Compression.ZipFile]::Open($out,[IO.Compression.ZipArchiveMode]::Create)

try{foreach($key in $parts.Keys){[void]([xml]$parts[$key]);$entry=$archive.CreateEntry($key);$writer=[IO.StreamWriter]::new($entry.Open(),[Text.UTF8Encoding]::new($false));$writer.Write($parts[$key]);$writer.Dispose()}}finally{$archive.Dispose()}

Write-Output $out