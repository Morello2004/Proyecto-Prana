param([switch]$Ejemplos)
$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
# utiliza un perfil independiente para exportar sin modificar las ventanas habituales del navegador
$edge = 'C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe'
if (!(Test-Path $edge)) { throw 'No se encontro Microsoft Edge para exportar los documentos' }
$perfil = Join-Path (Split-Path (Split-Path $root)) ('obj/ManualEdge-' + [guid]::NewGuid().ToString('N'))
$puerto = 19228
# inicia edge oculto y habilita el canal local por el que se enviaran las ordenes de impresion
$navegador = Start-Process -FilePath $edge -ArgumentList @('--headless=new','--disable-gpu','--in-process-gpu','--no-sandbox','--no-first-run',"--remote-debugging-port=$puerto",('--user-data-dir="'+$perfil+'"'),'about:blank') -WindowStyle Hidden -PassThru -RedirectStandardError (Join-Path $root 'exportacion_edge.log')
$socket = New-Object System.Net.WebSockets.ClientWebSocket
$script:numeroMensaje = 0

# envia una orden al navegador y espera la respuesta asociada a esa solicitud
function Comando-Browser([string]$Metodo, $Parametros = @{}) {
    $script:numeroMensaje++
    $id = $script:numeroMensaje
    # identifica cada solicitud para no confundir su respuesta con otros eventos del navegador
    $mensaje = @{id=$id;method=$Metodo;params=$Parametros} | ConvertTo-Json -Depth 15 -Compress
    $bytes = [Text.Encoding]::UTF8.GetBytes($mensaje)
    $limite = New-Object Threading.CancellationTokenSource
    $limite.CancelAfter(45000)
    try {
        # envia la orden como json y reune los fragmentos de respuesta recibidos por websocket
        [void]$socket.SendAsync([ArraySegment[byte]]::new($bytes),[Net.WebSockets.WebSocketMessageType]::Text,$true,$limite.Token).GetAwaiter().GetResult()
        while ($true) {
            $memoria = New-Object IO.MemoryStream
            try {
                do {
                    $buffer = New-Object byte[] 65536
                    $recibido = $socket.ReceiveAsync([ArraySegment[byte]]::new($buffer),$limite.Token).GetAwaiter().GetResult()
                    $memoria.Write($buffer,0,$recibido.Count)
                } while (!$recibido.EndOfMessage)
                $respuesta = [Text.Encoding]::UTF8.GetString($memoria.ToArray()) | ConvertFrom-Json
            } finally { $memoria.Dispose() }
            if ($respuesta.id -eq $id) {
                if ($respuesta.error) { throw ($respuesta.error | ConvertTo-Json -Compress) }
                return $respuesta.result
            }
        }
    } finally { $limite.Dispose() }
}

# prepara a br ir p ag in a con los datos recibidos y devuelve el resultado al generador
function Abrir-Pagina([string]$Ruta) {
    $uri = [Uri]::new([IO.Path]::GetFullPath($Ruta)).AbsoluteUri
    [void](Comando-Browser 'Page.navigate' @{url=$uri})
    $reloj = [Diagnostics.Stopwatch]::StartNew()
    do {
        Start-Sleep -Milliseconds 100
        $estado = Comando-Browser 'Runtime.evaluate' @{expression='document.readyState === "complete" && Array.from(document.images).every(i => i.complete)';returnByValue=$true}
        if ($reloj.Elapsed.TotalSeconds -gt 20) { throw 'La pagina no termino de cargar sus imagenes' }
    } while (!$estado.result.value)
}

try {
    $pestana = $null
    $http = New-Object Net.WebClient
    $http.Proxy = $null
    for ($intento = 0; $intento -lt 40; $intento++) {
        try { $paginas = $http.DownloadString("http://127.0.0.1:$puerto/json/list") | ConvertFrom-Json; $pestana = @($paginas) | Where-Object type -eq 'page' | Select-Object -First 1; if ($pestana) { break } } catch { $ultimoError = $_.Exception.Message; Start-Sleep -Milliseconds 150 }
    }
    $http.Dispose()
    if (!$pestana) { throw "No se pudo iniciar la exportacion con Edge: $ultimoError" }
    [void]$socket.ConnectAsync([Uri]$pestana.webSocketDebuggerUrl,[Threading.CancellationToken]::None).GetAwaiter().GetResult()
    [void](Comando-Browser 'Page.enable')
    [void](Comando-Browser 'Emulation.setDeviceMetricsOverride' @{width=1100;height=1400;deviceScaleFactor=1;mobile=$false})
    if ($Ejemplos) {
        foreach ($nombre in @('ejemplo_garantia','ejemplo_remito','ejemplo_cierre')) {
            Abrir-Pagina (Join-Path $root ('imagenes/'+$nombre+'.html'))
            $dimension = Comando-Browser 'Runtime.evaluate' @{expression='JSON.stringify((()=>{const r=document.querySelector("main").getBoundingClientRect();return {x:r.left,y:r.top,width:r.width,height:r.height,scale:1}})())';returnByValue=$true}
            $clip = $dimension.result.value | ConvertFrom-Json
            $captura = Comando-Browser 'Page.captureScreenshot' @{format='png';captureBeyondViewport=$true;clip=$clip}
            [IO.File]::WriteAllBytes((Join-Path $root ('imagenes/'+$nombre+'.png')),[Convert]::FromBase64String($captura.data))
            Write-Output "Ejemplo actualizado: $nombre"
        }
    } else {
        Abrir-Pagina (Join-Path $root 'Manual_de_Usuario_Completo_Prana.html')
        $impresion = Comando-Browser 'Page.printToPDF' @{printBackground=$true;preferCSSPageSize=$true;displayHeaderFooter=$true;headerTemplate='<span></span>';footerTemplate='<div style="font-size:8px;width:100%;text-align:center;color:#617588">PRANA ELECTRÓNICA · Manual de empleados · <span class="pageNumber"></span> / <span class="totalPages"></span></div>'}
        $rutaPdf = Join-Path $root 'Manual_de_Usuario_Completo_Prana.pdf'
        [IO.File]::WriteAllBytes($rutaPdf,[Convert]::FromBase64String($impresion.data))
        $visual = Comando-Browser 'Page.captureScreenshot' @{format='png';captureBeyondViewport=$false}
        [IO.File]::WriteAllBytes((Join-Path $root 'vista_previa_manual.png'),[Convert]::FromBase64String($visual.data))
        Write-Output "PDF generado: $rutaPdf"
    }
} finally {
    $socket.Dispose()
    if (!$navegador.HasExited) { Stop-Process -Id $navegador.Id }
}
