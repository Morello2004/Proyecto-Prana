using Microsoft.Data.SqlClient;
using Prana.Datos;
using Prana.Entidades;
using Prana.negocio;
using Prana.Presentacion;
using System.Text.RegularExpressions;
using System.Text.Json;
class Pruebas {
const string Base="PranaCajaPruebas_20260923";
static void Ok(bool valor,string mensaje){if(!valor)throw new Exception(mensaje);Console.WriteLine("OK: "+mensaje);}
static void Rechaza(Action accion,string mensaje){try{accion();}catch(Exception e)when(e is InvalidOperationException || e is ArgumentException || e is SqlException){Console.WriteLine("OK: rechaza "+mensaje);return;}throw new Exception("No rechazó "+mensaje);}
static object? Sql(string texto){using var cn=new SqlConnection(Conexion.ObtenerCadena());cn.Open();using var cmd=new SqlCommand(texto,cn);return cmd.ExecuteScalar();}
static void Script(string archivo){using var cn=new SqlConnection(Conexion.ObtenerCadena());cn.Open();string texto=File.ReadAllText(archivo);Ok(!Regex.IsMatch(texto,@"(?im)^\s*(USE|CREATE DATABASE)\b"),"Script aislado: "+Path.GetFileName(archivo));foreach(string lote in Regex.Split(texto,@"(?im)^\s*GO\s*$")){if(string.IsNullOrWhiteSpace(lote))continue;using var cmd=new SqlCommand(lote,cn);cmd.ExecuteNonQuery();}}
static NuevaVenta Solicitud(decimal pago=400) => new(){IdCliente=1,IdUsuario=1,FechaPago=DateTime.Today,FechaEntrega=DateTime.Today,HorarioSugerido="14 a 18",ObservacionesEntrega="Llamar antes",ObservacionPago="Pago parcial registrado",TipoLogistica="Flete",Total=1000,CondicionPago="Pago parcial",ModalidadEntrega="Retiro en el local",Detalles=new(){new(){IdVariante=1,Cantidad=1,PrecioUnitario=1000}},Pagos=pago>0?new(){new(){IdMedioPago=1,Monto=pago,Cuotas=1}}:new()};
[STAThread]static void Main(string[] args){
if(args.Contains("--presentacion")){PruebasPresentacion.Ejecutar();return;}
var b=new SqlConnectionStringBuilder(Conexion.ObtenerCadena());Ok(b.InitialCatalog==Base,"Conexión exclusiva a base de pruebas");b.InitialCatalog="master";
using var master=new SqlConnection(b.ConnectionString);master.Open();bool creada=false;
try{
using(var cmd=new SqlCommand($"IF DB_ID('{Base}') IS NOT NULL THROW 50000,'La base de prueba ya existe; no se modifica.',1; CREATE DATABASE [{Base}]",master)){cmd.ExecuteNonQuery();creada=true;}
Script("BaseDeDatos/CrearEstructuraPrana_LocalDB.sql");Script("BaseDeDatos/ActualizarEstructuraPrana_LocalDB.sql");Script("BaseDeDatos/CajaYComprobantes.sql");Script("BaseDeDatos/CajaYComprobantes.sql");
Sql(@"INSERT Categoria(nombre_categoria) VALUES(N'Prueba'); INSERT Marca(nombre_marca) VALUES(N'Prueba');
INSERT Producto(nombre_producto,id_marca,id_categoria,modelo) VALUES(N'Heladera <Prueba>',1,1,'TEST');
INSERT Variante_Producto(color,condicion_producto,precio,stock_disponible,id_producto,activo,costo) VALUES(N'Blanco',N'Nuevo',1000,20,1,1,50);
INSERT Cliente(nombre_cliente,apellido_cliente,dni,telefono,direccion,fecha_registro,saldo_a_favor) VALUES(N'Juana',N'Pérez','12345678','2323000000','Calle & Prueba 123',SYSDATETIME(),0);
INSERT Medio_Pago(nombre_medio_pago,recargo_medio_pago,recargo_3,recargo_6,recargo_9,recargo_12,activo,tipo_caja) VALUES(N'Efectivo',0,0,0,0,0,1,N'Efectivo'),(N'Transferencia',0,0,0,0,0,1,N'Transferencia'),(N'Tarjeta',10,20,30,40,50,1,N'Crédito');");
Script("BaseDeDatos/ActualizarEstructuraPrana_LocalDB.sql");
new CD_Usuario().CrearUsuario("usuario_prueba_uno", "hash_prueba", 2);
new CD_Usuario().CrearUsuario("usuario_prueba_dos", "hash_prueba", 2);
Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM Usuario WHERE nombre_usuario LIKE 'usuario_prueba_%' AND email IS NULL"))==2,"Varios usuarios sin correo después de migrar dos veces");
Sql("UPDATE Usuario SET email='prueba@example.com' WHERE nombre_usuario='usuario_prueba_uno'");
Rechaza(()=>Sql("UPDATE Usuario SET email='prueba@example.com' WHERE nombre_usuario='usuario_prueba_dos'"),"correo informado duplicado");
SesionActual.IniciarSesion(new usuario{idUsuario=1,IdRolUsuario=1});
var clientes=new CN_Cliente();
var clientePostal=clientes.buscarCliente("12345678").Single();
clientePostal.Localidad.NombreLocalidad="Lujan"; clientePostal.CodigoPostal="B6700ABC";
clientePostal.Direccion="";
Ok(clientes.NuevoCliente(new Cliente {Nombre="Ana",Apellido="Prueba",FechaRegistro=DateTime.Today,DNI="87654321",Telefono="2323000000",Direccion="",Localidad=new Localidad {NombreLocalidad="Lujan"}}),"Crear cliente sin dirección");
Ok(clientes.ModificarCliente(clientePostal),"Guardar ciudad y codigo postal alfanumerico");
Ok(clientes.buscarCliente("12345678").Single().CodigoPostal=="B6700ABC","Codigo postal persistido");
clientePostal.Localidad.NombreLocalidad="";
Rechaza(()=>clientes.ModificarCliente(clientePostal),"ciudad vacia");
clientePostal.Localidad.NombreLocalidad="Lujan"; clientePostal.CodigoPostal="";
Ok(clientes.ModificarCliente(clientePostal) && clientes.buscarCliente("12345678").Single().CodigoPostal=="","Codigo postal opcional y se puede borrar");
clientePostal.CodigoPostal="6700"; clientes.ModificarCliente(clientePostal);
var ventas=new CN_RegistroVenta();var caja=new CN_Caja();var pagos=new CN_Pago();
Rechaza(()=>ventas.Registrar(Solicitud(0)),"venta parcial sin pago inicial");
int primera=ventas.Registrar(Solicitud());Ok(primera==2500,"Primera venta = 2500 en base vacía, migración repetida");
var conDescuento=Solicitud(1); conDescuento.Descuento=100; conDescuento.Total=900;
int segunda=ventas.Registrar(conDescuento);Ok(segunda==2501,"Segunda venta = 2501");
Script("BaseDeDatos/CajaYComprobantes.sql");int tercera=ventas.Registrar(Solicitud(1));Ok(tercera==2502,"Reiniciar no retrocede ni repite números");
Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM LogActividad WHERE accion='REGISTRAR_VENTA'"))==3,"Historial registra cada venta sin duplicados");
Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM LogActividad WHERE accion='APLICAR_DESCUENTO'"))==1,"Historial registra solo descuentos aplicados");
Sql($"DELETE FROM LogActividad WHERE id_entidad={segunda} AND entidad='Venta'");
Script("BaseDeDatos/CajaYComprobantes.sql");Script("BaseDeDatos/CajaYComprobantes.sql");
Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM LogActividad WHERE accion='REGISTRAR_VENTA'"))==3 && Convert.ToInt32(Sql("SELECT COUNT(*) FROM LogActividad WHERE accion='APLICAR_DESCUENTO'"))==1,"Recuperacion historica idempotente de ventas y descuentos");
Ok(Sql("SELECT OBJECT_ID('dbo.Venta_Comprobante','U')") is DBNull,"La tabla eliminada no se recrea");
var doc=new CN_Comprobante().Obtener(primera);Ok(doc.Numero==2500 && doc.Pagado==400 && doc.Saldo==600 && doc.Productos.Count==1,"Comprobantes reconstruidos con número común y saldo real");
Ok(doc.CodigoPostal=="6700" && doc.Localidad=="Lujan","Remito conserva ciudad y codigo postal");
Ok(doc.DatosEntregaCompletos && doc.HorarioSugerido=="14 a 18" && doc.ObservacionPago=="Pago parcial registrado" && doc.Observaciones=="Llamar antes","Comprobante consulta observaciones y horario");
var recuperado=new CN_Comprobante().Obtener(primera);
Ok(recuperado.HorarioSugerido=="14 a 18" && recuperado.Total==doc.Total && recuperado.Pagado==doc.Pagado,"Comprobante consulta datos de entrega e importes");
Sql("UPDATE Cliente SET nombre_cliente=N'Cambiado' WHERE id_cliente=1");Ok(new CN_Comprobante().Obtener(primera).Cliente=="Cambiado Pérez","Reimpresión consulta los datos actuales del cliente");
string carpeta=DocumentosPrana.GuardarVenta(doc,"obj/CajaComprobantes/muestras");string garantia=File.ReadAllText(Path.Combine(carpeta,"Garantia-2500.html"));string remito=File.ReadAllText(Path.Combine(carpeta,"Remito-2500.html"));
Ok(garantia.Contains("GARANTÍA") && garantia.Contains("2500") && remito.Contains("2500") && garantia.Contains("Heladera &lt;Prueba&gt;"),"HTML generado, títulos, números y escape seguro");
var medios=new CN_MedioPago().ObtenerTodos();var efectivo=medios.Single(m=>m.TipoCaja=="Efectivo");var transferencia=medios.Single(m=>m.TipoCaja=="Transferencia");
pagos.Registrar(primera,transferencia,1,600,"Lemon");
Sql($"UPDATE Venta SET fecha_venta=DATEADD(day,-1,fecha_venta) WHERE id_venta={primera}");
caja.Registrar(new MovimientoCaja{Concepto="Flete",Importe=100,Tipo="Gasto"});
caja.Registrar(new MovimientoCaja{Concepto="Café",Importe=50,Tipo="Café"});
caja.Registrar(new MovimientoCaja{Concepto="Ingreso USD",Importe=20,Tipo="Ingreso extra",Moneda="USD"});
caja.Registrar(new MovimientoCaja{Concepto="Gasto USD",Importe=5,Tipo="Gasto",Moneda="USD"});
caja.Registrar(new MovimientoCaja{Concepto="Error",Importe=999,Tipo="Gasto"});
var consulta=caja.Consultar(DateTime.Today);caja.Anular(consulta.Movimientos.Single(m=>m.Concepto=="Error").Id,"Error de carga");
consulta=caja.Consultar(DateTime.Today);consulta.SaldoInicial=200;consulta.SaldoInicialUsd=10;
consulta.Billetes=new(){new(){Moneda="ARS",Denominacion=100,Cantidad=5},new(){Moneda="ARS",Denominacion=50,Cantidad=1},new(){Moneda="ARS",Denominacion=1,Cantidad=2},new(){Moneda="USD",Denominacion=5,Cantidad=5}};
Ok(consulta.Ingresos("ARS")==1052 && consulta.Gastos("ARS")==100 && consulta.Esperado("ARS")==552 && consulta.BalanceArs==1152 && consulta.Diferencia("ARS")==0,"Caja: cobros de ventas anteriores, gastos, café, apertura y diferencia");
Ok(consulta.Esperado("USD")==25 && consulta.Diferencia("USD")==0,"Dólares separados, sin mezclarse con pesos");
Ok(consulta.Movimientos.Any(m=>m.Cuenta=="Lemon"),"Cuenta destino de transferencias en cierre");
var obsoleto=caja.Consultar(DateTime.Today);caja.Registrar(new MovimientoCaja{Concepto="Nuevo",Importe=1,Tipo="Ingreso extra"});Rechaza(()=>caja.Cerrar(obsoleto),"cierre con movimientos desactualizados");
caja.Anular(caja.Consultar(DateTime.Today).Movimientos.Single(m=>m.Concepto=="Nuevo").Id,"Prueba");
consulta.Movimientos=caja.Consultar(DateTime.Today).Movimientos;consulta.Inicio=DateTime.Today;
var cerrado=caja.Cerrar(consulta);Ok(cerrado.Numero>0,"Cierre guardado");
File.WriteAllText("obj/CajaComprobantes/muestras/Cierre.html",DocumentosPrana.CierreHtml(cerrado));
Rechaza(()=>caja.Cerrar(consulta),"segundo cierre del mismo día");Rechaza(()=>caja.Registrar(new MovimientoCaja{Concepto="Tarde",Importe=1}),"gasto posterior al cierre");
Rechaza(()=>pagos.Registrar(segunda,efectivo,1,10),"cobro posterior al cierre");
Rechaza(()=>ventas.Registrar(Solicitud()),"venta con cobro posterior al cierre");
Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM Venta"))==3,"Fallo no deja venta parcial");
Sql("UPDATE Medio_Pago SET tipo_caja=N'Otros' WHERE id_medio_pago=1");var guardado=caja.Consultar(DateTime.Today);
Ok(guardado.Esperado("ARS")==550 && guardado.Contado("USD")==25,"Cierre conserva snapshot, billetes y totales");
using(var formulario=new frmCaja()) {
    typeof(frmCaja).GetMethod("Cargar",System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance)!.Invoke(formulario,null);
    foreach(int ancho in new[]{840,1100,780,840}) {
        formulario.ClientSize=new Size(ancho,690); formulario.PerformLayout();
        var pagina=(Panel)formulario.Controls["paginaAdaptable"]!;
        formulario.Controls.Remove(pagina); pagina.Dock=DockStyle.None; pagina.Size=new Size(ancho,690); pagina.PerformLayout();
        foreach(Control c in pagina.Controls) Ok(c.Right<=pagina.ClientSize.Width,"Caja: sección dentro del ancho "+ancho);
        using var imagen=new Bitmap(ancho,690); pagina.DrawToBitmap(imagen,new Rectangle(0,0,ancho,690));
        if(ancho==840)imagen.Save("obj/CajaComprobantes/caja-ui.png");
        pagina.ScrollControlIntoView(pagina.Controls[pagina.Controls.Count-1]);
        Ok(pagina.Controls[pagina.Controls.Count-1].Bottom<=pagina.ClientSize.Height+1,"Caja: botón de cierre accesible al desplazar");
        pagina.AutoScrollPosition=Point.Empty; formulario.Controls.Add(pagina); pagina.Dock=DockStyle.Fill;
    }
}
var stockNegocio = new Prana.Negocio.CN_Producto();
var detalleStock = new List<(int idVariante, int cantidad, decimal costoUnitario)>{(1,2,50m)};
Ok(stockNegocio.RegistrarIngreso("  Proveedor nuevo prueba  ",DateTime.Today,"STOCK-1",detalleStock),"Ingreso crea proveedor nuevo");
Ok(stockNegocio.RegistrarIngreso("proveedor NUEVO prueba",DateTime.Today,"STOCK-2",detalleStock),"Ingreso reutiliza nombre escrito sin distinguir mayúsculas");
Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM Proveedor WHERE nombre_proveedor='Proveedor nuevo prueba'"))==1,"No duplica proveedor");
Ok(Convert.ToInt32(Sql("SELECT COUNT(DISTINCT id_proveedor) FROM Ingreso_Stock WHERE remito_proveedor IN ('STOCK-1','STOCK-2')"))==1,"Ambos ingresos usan el proveedor correcto");
Rechaza(()=>stockNegocio.RegistrarIngreso("Proveedor rollback",DateTime.Today,"STOCK-ERROR",new(){(int.MaxValue,1,10m)}),"Ingreso inválido informa la causa");
Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM Proveedor WHERE nombre_proveedor='Proveedor rollback'"))==0,"Ingreso fallido revierte proveedor nuevo");
Rechaza(()=>stockNegocio.RegistrarIngreso("Proveedor inválido",DateTime.Today,"PRUEBA",new(){(1,0,10m)}),"ingreso sin cantidad positiva");
try {
    stockNegocio.RegistrarIngreso("Proveedor sin documento",DateTime.Today,"   ",detalleStock);
    throw new Exception("Aceptó ingreso sin documento");
} catch (InvalidOperationException ex) {
    Ok(ex.Message=="Ingrese el número o código del documento del proveedor (remito).","Documento vacío informa exactamente qué falta");
}
Rechaza(()=>stockNegocio.RegistrarIngreso("Proveedor inválido",DateTime.Today,new string('X',51),detalleStock),"documento supera longitud de la base");
using(var stockForm = new frmStock())
{
    object CampoStock(string nombre) => typeof(frmStock).GetField(nombre,System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic)!.GetValue(stockForm)!;
    var proveedorControl=(ComboBox)CampoStock("comboBoxSeleccionaProveedor");
    Ok(proveedorControl.DropDownStyle==ComboBoxStyle.DropDown && proveedorControl.AutoCompleteSource==AutoCompleteSource.ListItems,"Un proveedor editable con sugerencias");
    proveedorControl.Text="Proveedor nuevo prueba";
    ((TextBox)CampoStock("textBoxRtoProveedor")).Text="STOCK-1";
    ((DateTimePicker)CampoStock("dtpFechaIngresoStock")).Value=DateTime.Today.AddDays(-1);
    ((Label)CampoStock("LblTotalIngresoDinero")).Text="85000";
    ((Label)CampoStock("labelCantidadIngresada")).Text="5";
    typeof(frmStock).GetMethod("LimpiarIngreso",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic)!.Invoke(stockForm,null);
    Ok(proveedorControl.SelectedIndex==-1 && proveedorControl.Text=="" && ((TextBox)CampoStock("textBoxRtoProveedor")).Text=="", "Ingreso limpia proveedor y remito");
    Ok(((Label)CampoStock("labelCantidadIngresada")).Text=="0" && ((Label)CampoStock("LblTotalIngresoDinero")).Text==0m.ToString("C2") && ((DateTimePicker)CampoStock("dtpFechaIngresoStock")).Value.Date==DateTime.Today,"Ingreso reinicia totales y fecha");
}
SesionActual.IniciarSesion(new usuario{idUsuario=1,IdRolUsuario=2});Rechaza(()=>caja.Consultar(DateTime.Today),"caja sin permiso de administrador");
Console.WriteLine("TODAS LAS PRUEBAS PASARON");
}finally{if(creada){SqlConnection.ClearAllPools();using var cmd=new SqlCommand($"ALTER DATABASE [{Base}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{Base}]",master);cmd.ExecuteNonQuery();}}
}
}
