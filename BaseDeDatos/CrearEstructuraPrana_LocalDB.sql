-- script de creacion de la estructura local de prana electronica este archivo se ejecuta solamente sobre una base sin tablas de la aplicacion crea las tablas, las claves, las relaciones y las reglas de validacion al final incorpora el rol administrador y el usuario admin inicial no contiene clientes, productos, ventas ni otros datos de trabajo la instruccion go separa el script en lotes que sql server ejecuta en orden se utiliza la base de la conexion actual no cambia el destino con use tabla: categoria guarda las categorias utilizadas para clasificar los productos aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON
-- fin del lote el inicializador divide el archivo aqui; go no es una consulta enviada por sqlcommand
GO
-- permite utilizar identificadores delimitados y reserva las comillas simples para textos
SET QUOTED_IDENTIFIER ON

GO
-- comienza la creacion fisica de la tabla y de sus columnas
CREATE TABLE [dbo].[Categoria](
-- define id_categoria como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_categoria] [int] IDENTITY(1,1) NOT NULL,
-- define nombre_categoria como texto de hasta 100 caracteres, obligatorio
	[nombre_categoria] [nvarchar](100) NOT NULL,
-- define la clave primaria que identifica de manera unica cada registro de la tabla
PRIMARY KEY CLUSTERED
(
	[id_categoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: cliente guarda los datos personales, fiscales y de contacto de los clientes aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Cliente](
-- define id_cliente como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_cliente] [int] IDENTITY(1,1) NOT NULL,
-- define nombre_cliente como texto de hasta 100 caracteres, obligatorio
	[nombre_cliente] [nvarchar](100) NOT NULL,
-- define apellido_cliente como texto de hasta 100 caracteres, obligatorio
	[apellido_cliente] [nvarchar](100) NOT NULL,
-- define dni como texto de hasta 11 caracteres, obligatorio
	[dni] [nvarchar](11) NOT NULL,
    
    [tipo_documento] [nvarchar](4) NOT NULL CONSTRAINT DF_Cliente_TipoDocumento DEFAULT (N'DNI') CONSTRAINT CK_Cliente_TipoDocumento CHECK (tipo_documento IN (N'DNI', N'CUIT')),
-- define telefono como texto de hasta 20 caracteres, obligatorio
	[telefono] [nvarchar](20) NOT NULL,
-- define direccion como texto de hasta 200 caracteres, opcional
	[direccion] [nvarchar](200) NULL,
-- define fecha_registro como fecha y hora, obligatorio
	[fecha_registro] [datetime2](7) NOT NULL,
	
	[activo] [bit] NOT NULL CONSTRAINT [DF_Cliente_Activo] DEFAULT ((1)),
	
	[eliminado] [bit] NOT NULL CONSTRAINT [DF_Cliente_Eliminado] DEFAULT ((0)),
-- define saldo_a_favor como numero decimal, opcional
	[saldo_a_favor] [decimal](10, 2) NULL,
-- define id_localidad como numero entero, opcional
	[id_localidad] [int] NULL,

PRIMARY KEY CLUSTERED
(
	[id_cliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
-- impide que se repita el valor indicado en diferentes registros impide repetir la combinacion de columnas definida por esta restriccion
UNIQUE NONCLUSTERED
(
	[dni] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: detalle_ingreso guarda cada variante y cantidad incluida en un ingreso de stock aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Detalle_Ingreso](
-- define id_detalle_ingreso como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_detalle_ingreso] [int] IDENTITY(1,1) NOT NULL,
-- define id_ingreso como numero entero, obligatorio
	[id_ingreso] [int] NOT NULL,
-- define id_variante como numero entero, obligatorio
	[id_variante] [int] NOT NULL,
-- define cantidad como numero entero, obligatorio
	[cantidad] [int] NOT NULL,
-- define costo_unitario como numero decimal, obligatorio
	[costo_unitario] [decimal](10, 2) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_detalle_ingreso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: detalle_remito guarda los productos y cantidades incluidos en cada remito aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Detalle_Remito](
-- define id_detalle_remito como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_detalle_remito] [int] IDENTITY(1,1) NOT NULL,
-- define id_remito como numero entero, obligatorio
	[id_remito] [int] NOT NULL,

	[id_variante] [int] NOT NULL,

	[cantidad] [int] NOT NULL,
-- define descripcion como texto de hasta 300 caracteres, obligatorio
	[descripcion] [nvarchar](300) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_detalle_remito] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: detalle_venta guarda los productos, cantidades y precios que componen una venta aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Detalle_Venta](
-- define id_detalle_venta como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_detalle_venta] [int] IDENTITY(1,1) NOT NULL,
-- define id_venta como numero entero, obligatorio
	[id_venta] [int] NOT NULL,

	[id_variante] [int] NOT NULL,

	[cantidad] [int] NOT NULL,
-- define precio_unitario como numero decimal, obligatorio
	[precio_unitario] [decimal](10, 2) NOT NULL,
-- define subtotal como numero decimal, obligatorio
	[subtotal] [decimal](10, 2) NOT NULL,
-- define id_estado_variante_producto como numero entero, obligatorio
	[id_estado_variante_producto] [int] NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_detalle_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: envio registra los datos necesarios para realizar y controlar un envio aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Envio](
-- define id_envio como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_envio] [int] IDENTITY(1,1) NOT NULL,

	[id_remito] [int] NOT NULL,
-- define direccion como texto de hasta 200 caracteres, obligatorio
	[direccion] [nvarchar](200) NOT NULL,
-- define horario_sugerido como texto de hasta 50 caracteres, opcional
	[horario_sugerido] [nvarchar](50) NULL,
-- define el tipo operativo del envio con uno de los dos valores permitidos
	[tipo_envio] [nvarchar](30) NOT NULL,
-- define costo como numero decimal, obligatorio
	[costo] [decimal](10, 2) NOT NULL,
-- define id_estado_envio como numero entero, obligatorio
	[id_estado_envio] [int] NOT NULL,
-- define fecha_entrega como fecha y hora, opcional
	[fecha_entrega] [datetime2](7) NULL,

PRIMARY KEY CLUSTERED
(
	[id_envio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
-- da nombre o define una regla de integridad para poder comprobarla o administrarla despues
CONSTRAINT [CK_Envio_TipoEnvio] CHECK ([tipo_envio] IN (N'Envío a domicilio', N'Retiro en local'))
) ON [PRIMARY]

GO

-- tabla: estado_envio contiene los posibles estados por los que puede pasar un envio aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Estado_Envio](
-- define id_estado_envio como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_estado_envio] [int] IDENTITY(1,1) NOT NULL,
-- define nombre como texto de hasta 30 caracteres, obligatorio
	[nombre] [nvarchar](30) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_estado_envio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: estado_variante_producto contiene los estados aplicables a una variante dentro de una venta aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Estado_Variante_Producto](
-- define id_estado_variante_producto como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_estado_variante_producto] [int] IDENTITY(1,1) NOT NULL,
-- define nombre_estado_variante_producto como texto de hasta 100 caracteres, obligatorio
	[nombre_estado_variante_producto] [nvarchar](100) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_estado_variante_producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: estado_venta contiene los posibles estados de una venta aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Estado_Venta](
-- define id_estado_venta como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_estado_venta] [int] IDENTITY(1,1) NOT NULL,
-- define nombre_estado_venta como texto de hasta 100 caracteres, obligatorio
	[nombre_estado_venta] [nvarchar](100) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_estado_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: ingreso_stock registra el encabezado de cada ingreso de mercaderia aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Ingreso_Stock](
-- define id_ingreso como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_ingreso] [int] IDENTITY(1,1) NOT NULL,
-- define id_proveedor como numero entero, obligatorio
	[id_proveedor] [int] NOT NULL,
-- define remito_proveedor como texto de hasta 50 caracteres, obligatorio
	[remito_proveedor] [nvarchar](50) NOT NULL,
-- define fecha_ingreso como fecha y hora, obligatorio
	[fecha_ingreso] [datetime2](7) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_ingreso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: localidad guarda las localidades asociadas a clientes y otras entidades aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Localidad](
-- define id_localidad como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_localidad] [int] IDENTITY(1,1) NOT NULL,
-- define nombre_localidad como texto de hasta 100 caracteres, obligatorio
	[nombre_localidad] [nvarchar](100) NOT NULL,
-- guarda el codigo postal como texto de hasta doce caracteres
	[codigo_postal] [nvarchar](12) NULL,
 
 CONSTRAINT [PK_Localidad] PRIMARY KEY CLUSTERED
(
	[id_localidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: marca guarda las marcas de los productos aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Marca](
-- define id_marca como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_marca] [int] IDENTITY(1,1) NOT NULL,
-- define nombre_marca como texto de hasta 100 caracteres, obligatorio
	[nombre_marca] [nvarchar](100) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_marca] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: medio_pago guarda los medios de pago y el recargo configurado para cada uno aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Medio_Pago](
-- define id_medio_pago como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_medio_pago] [int] IDENTITY(1,1) NOT NULL,
-- define nombre_medio_pago como texto de hasta 32 caracteres, obligatorio
	[nombre_medio_pago] [nvarchar](32) NOT NULL,
-- define recargo_medio_pago como numero decimal, opcional
	[recargo_medio_pago] [decimal](5, 2) NULL,

PRIMARY KEY CLUSTERED
(
	[id_medio_pago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: modalidad_entrega contiene las modalidades disponibles para entregar una venta aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Modalidad_Entrega](
-- define id_modalidad_entrega como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_modalidad_entrega] [int] IDENTITY(1,1) NOT NULL,
-- define nombre_modalidad_entrega como texto de hasta 100 caracteres, obligatorio
	[nombre_modalidad_entrega] [nvarchar](100) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_modalidad_entrega] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: pago registra los pagos realizados y el medio utilizado aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Pago](
-- define id_pago como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_pago] [int] IDENTITY(1,1) NOT NULL,

	[id_venta] [int] NOT NULL,
-- define id_usuario como numero entero, obligatorio
	[id_usuario] [int] NOT NULL,
-- define id_medio_pago como numero entero, obligatorio
	[id_medio_pago] [int] NOT NULL,
-- define cuotas como numero entero, opcional
	[cuotas] [int] NULL,
-- define monto como numero decimal, obligatorio
	[monto] [decimal](10, 2) NOT NULL,
-- define monto_final como numero decimal, obligatorio
	[monto_final] [decimal](10, 2) NOT NULL,
-- define fecha_pago como fecha y hora, obligatorio
	[fecha_pago] [datetime2](7) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_pago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: producto guarda la informacion general de los productos comercializados aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Producto](
-- define id_producto como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_producto] [int] IDENTITY(1,1) NOT NULL,
-- define nombre_producto como texto de hasta 150 caracteres, obligatorio
	[nombre_producto] [nvarchar](150) NOT NULL,
-- define id_marca como numero entero, obligatorio
	[id_marca] [int] NOT NULL,
-- define descripcion como texto de hasta 500 caracteres, opcional
	[descripcion] [nvarchar](500) NULL,
-- define id_categoria como numero entero, obligatorio
	[id_categoria] [int] NOT NULL,
-- define modelo como texto de hasta 15 caracteres, opcional
	[modelo] [varchar](15) NULL,

PRIMARY KEY CLUSTERED
(
	[id_producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: proveedor guarda los proveedores utilizados en los ingresos de stock aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Proveedor](
-- define id_proveedor como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_proveedor] [int] IDENTITY(1,1) NOT NULL,
-- define nombre_proveedor como texto de hasta 150 caracteres, obligatorio
	[nombre_proveedor] [nvarchar](150) NOT NULL,
-- define telefono como texto de hasta 20 caracteres, opcional
	[telefono] [nvarchar](20) NULL,

PRIMARY KEY CLUSTERED
(
	[id_proveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: recibo registra los recibos emitidos por los pagos aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Recibo](
-- define id_recibo como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_recibo] [int] IDENTITY(1,1) NOT NULL,
-- define id_pago como numero entero, obligatorio
	[id_pago] [int] NOT NULL,
-- define fecha_emision como fecha y hora, obligatorio
	[fecha_emision] [datetime2](7) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_recibo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: remito registra los remitos asociados a las ventas y entregas aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Remito](
-- define id_remito como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_remito] [int] IDENTITY(1,1) NOT NULL,

	[id_venta] [int] NOT NULL,

	[fecha_emision] [datetime2](7) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_remito] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: rol guarda los roles que determinan los permisos de los usuarios aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Rol](
-- define id_rol_usuario como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_rol_usuario] [int] IDENTITY(1,1) NOT NULL,
-- define nombre_rol_usuario como texto de hasta 20 caracteres, obligatorio
	[nombre_rol_usuario] [nvarchar](20) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_rol_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: usuario guarda los usuarios que pueden iniciar sesion en prana aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Usuario](
-- define id_usuario como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_usuario] [int] IDENTITY(1,1) NOT NULL,
-- define nombre_usuario como texto de hasta 100 caracteres, obligatorio
	[nombre_usuario] [nvarchar](100) NOT NULL,
-- define hash_contrasena como texto de hasta 255 caracteres, obligatorio
	[hash_contrasena] [nvarchar](255) NOT NULL,
-- define id_rol_usuario como numero entero, obligatorio
	[id_rol_usuario] [int] NOT NULL,
-- define activo como valor logico: 1 representa verdadero y 0 representa falso, obligatorio
	[activo] [bit] NOT NULL,
-- conserva los intentos de inicio de sesion fallidos consecutivos
	[intentos_login_fallidos] [int] NOT NULL CONSTRAINT [DF_Usuario_IntentosLogin] DEFAULT ((0)),
-- conserva hasta cuando se bloquea temporalmente una cuenta
	[bloqueado_hasta] [datetime2](7) NULL,

PRIMARY KEY CLUSTERED
(
	[id_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: variante_producto guarda cada variante comercial de un producto, junto con precio y stock aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Variante_Producto](
-- define id_variante como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_variante] [int] IDENTITY(1,1) NOT NULL,
-- define color como texto de hasta 50 caracteres, obligatorio
	[color] [nvarchar](50) NOT NULL,
-- define condicion_producto como texto de hasta 10 caracteres, obligatorio
	[condicion_producto] [nvarchar](10) NOT NULL,
-- define precio como numero decimal, obligatorio
	[precio] [decimal](10, 2) NOT NULL,
-- define stock_disponible como numero entero, obligatorio
	[stock_disponible] [int] NOT NULL,
-- define id_producto como numero entero, obligatorio
	[id_producto] [int] NOT NULL,

	[activo] [bit] NOT NULL,

	[costo] [decimal](10, 2) NOT NULL,

PRIMARY KEY CLUSTERED
(
	[id_variante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: venta registra el encabezado, importes, cliente, usuario y estado de cada venta aplica el comportamiento estandar de sql server para las comparaciones con valores null
SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Venta](
-- define id_venta como numero entero, generado automaticamente de uno en uno, obligatorio
	[id_venta] [int] IDENTITY(1,1) NOT NULL,
-- define id_cliente como numero entero, obligatorio
	[id_cliente] [int] NOT NULL,

	[id_usuario] [int] NOT NULL,
-- define fecha_venta como fecha y hora, obligatorio
	[fecha_venta] [datetime2](7) NOT NULL,
-- define id_estado_venta como numero entero, obligatorio
	[id_estado_venta] [int] NOT NULL,
-- define total como numero decimal, obligatorio
	[total] [decimal](10, 2) NOT NULL,
-- define id_modalidad_entrega como numero entero, obligatorio
	[id_modalidad_entrega] [int] NOT NULL,
-- define descuento como numero decimal, opcional
	[descuento] [decimal](10, 2) NULL,

PRIMARY KEY CLUSTERED
(
	[id_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
-- establece el valor que se asignara automaticamente cuando no se informe esta columna
ALTER TABLE [dbo].[Cliente] ADD  DEFAULT ((0)) FOR [saldo_a_favor]

GO

ALTER TABLE [dbo].[Envio] ADD  DEFAULT ((0)) FOR [costo]

GO

ALTER TABLE [dbo].[Medio_Pago] ADD  DEFAULT ((0)) FOR [recargo_medio_pago]

GO

ALTER TABLE [dbo].[Usuario] ADD  DEFAULT ((1)) FOR [activo]

GO

ALTER TABLE [dbo].[Variante_Producto] ADD  CONSTRAINT [DF_VarianteProducto_Activo]  DEFAULT ((1)) FOR [activo]

GO

ALTER TABLE [dbo].[Variante_Producto] ADD  CONSTRAINT [DF_VarianteProducto_Costo]  DEFAULT ((0)) FOR [costo]

GO

ALTER TABLE [dbo].[Venta] ADD  DEFAULT ((0)) FOR [descuento]

GO
-- crea una clave externa para relacionar esta tabla con otra tabla del sistema
ALTER TABLE [dbo].[Cliente]  WITH CHECK ADD  CONSTRAINT [FK_Cliente_Localidad] FOREIGN KEY([id_localidad])
-- indica la tabla y la columna que debe existir para mantener la integridad de la relacion indica la clave de la tabla relacionada que debe existir para aceptar esta referencia
REFERENCES [dbo].[Localidad] ([id_localidad])

GO
-- activa y valida la relacion definida en la instruccion anterior
ALTER TABLE [dbo].[Cliente] CHECK CONSTRAINT [FK_Cliente_Localidad]

GO

ALTER TABLE [dbo].[Detalle_Ingreso]  WITH CHECK ADD  CONSTRAINT [FK_DetalleIngreso_Ingreso] FOREIGN KEY([id_ingreso])

REFERENCES [dbo].[Ingreso_Stock] ([id_ingreso])

GO

ALTER TABLE [dbo].[Detalle_Ingreso] CHECK CONSTRAINT [FK_DetalleIngreso_Ingreso]

GO

ALTER TABLE [dbo].[Detalle_Ingreso]  WITH CHECK ADD  CONSTRAINT [FK_DetalleIngreso_Variante] FOREIGN KEY([id_variante])

REFERENCES [dbo].[Variante_Producto] ([id_variante])

GO

ALTER TABLE [dbo].[Detalle_Ingreso] CHECK CONSTRAINT [FK_DetalleIngreso_Variante]

GO

ALTER TABLE [dbo].[Detalle_Remito]  WITH CHECK ADD  CONSTRAINT [FK_DetalleRemito_Remito] FOREIGN KEY([id_remito])

REFERENCES [dbo].[Remito] ([id_remito])

GO

ALTER TABLE [dbo].[Detalle_Remito] CHECK CONSTRAINT [FK_DetalleRemito_Remito]

GO

ALTER TABLE [dbo].[Detalle_Remito]  WITH CHECK ADD  CONSTRAINT [FK_DetalleRemito_Variante] FOREIGN KEY([id_variante])

REFERENCES [dbo].[Variante_Producto] ([id_variante])

GO

ALTER TABLE [dbo].[Detalle_Remito] CHECK CONSTRAINT [FK_DetalleRemito_Variante]

GO

ALTER TABLE [dbo].[Detalle_Venta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleVenta_EstadoVariante] FOREIGN KEY([id_estado_variante_producto])

REFERENCES [dbo].[Estado_Variante_Producto] ([id_estado_variante_producto])

GO

ALTER TABLE [dbo].[Detalle_Venta] CHECK CONSTRAINT [FK_DetalleVenta_EstadoVariante]

GO

ALTER TABLE [dbo].[Detalle_Venta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleVenta_Variante] FOREIGN KEY([id_variante])

REFERENCES [dbo].[Variante_Producto] ([id_variante])

GO

ALTER TABLE [dbo].[Detalle_Venta] CHECK CONSTRAINT [FK_DetalleVenta_Variante]

GO

ALTER TABLE [dbo].[Detalle_Venta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleVenta_Venta] FOREIGN KEY([id_venta])

REFERENCES [dbo].[Venta] ([id_venta])

GO

ALTER TABLE [dbo].[Detalle_Venta] CHECK CONSTRAINT [FK_DetalleVenta_Venta]

GO

ALTER TABLE [dbo].[Envio]  WITH CHECK ADD  CONSTRAINT [FK_Envio_Estado] FOREIGN KEY([id_estado_envio])

REFERENCES [dbo].[Estado_Envio] ([id_estado_envio])

GO

ALTER TABLE [dbo].[Envio] CHECK CONSTRAINT [FK_Envio_Estado]

GO

ALTER TABLE [dbo].[Envio]  WITH CHECK ADD  CONSTRAINT [FK_Envio_Remito] FOREIGN KEY([id_remito])

REFERENCES [dbo].[Remito] ([id_remito])

GO

ALTER TABLE [dbo].[Envio] CHECK CONSTRAINT [FK_Envio_Remito]

GO
-- crea una clave externa para relacionar esta tabla con otra tabla del sistema
ALTER TABLE [dbo].[Ingreso_Stock]  WITH CHECK ADD  CONSTRAINT [FK_IngresoStock_Proveedor] FOREIGN KEY([id_proveedor])

REFERENCES [dbo].[Proveedor] ([id_proveedor])

GO

ALTER TABLE [dbo].[Ingreso_Stock] CHECK CONSTRAINT [FK_IngresoStock_Proveedor]

GO

ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [FK_Pago_Medio] FOREIGN KEY([id_medio_pago])

REFERENCES [dbo].[Medio_Pago] ([id_medio_pago])

GO

ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [FK_Pago_Medio]

GO

ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [FK_Pago_Usuario] FOREIGN KEY([id_usuario])

REFERENCES [dbo].[Usuario] ([id_usuario])

GO

ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [FK_Pago_Usuario]

GO

ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [FK_Pago_Venta] FOREIGN KEY([id_venta])

REFERENCES [dbo].[Venta] ([id_venta])

GO

ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [FK_Pago_Venta]

GO

ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [FK_Producto_Categoria] FOREIGN KEY([id_categoria])

REFERENCES [dbo].[Categoria] ([id_categoria])

GO

ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_Categoria]

GO

ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [FK_Producto_Marca] FOREIGN KEY([id_marca])

REFERENCES [dbo].[Marca] ([id_marca])

GO

ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_Marca]

GO

ALTER TABLE [dbo].[Recibo]  WITH CHECK ADD  CONSTRAINT [FK_Recibo_Pago] FOREIGN KEY([id_pago])

REFERENCES [dbo].[Pago] ([id_pago])

GO

ALTER TABLE [dbo].[Recibo] CHECK CONSTRAINT [FK_Recibo_Pago]

GO

ALTER TABLE [dbo].[Remito]  WITH CHECK ADD  CONSTRAINT [FK_Remito_Venta] FOREIGN KEY([id_venta])

REFERENCES [dbo].[Venta] ([id_venta])

GO

ALTER TABLE [dbo].[Remito] CHECK CONSTRAINT [FK_Remito_Venta]

GO

ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([id_rol_usuario])

REFERENCES [dbo].[Rol] ([id_rol_usuario])

GO

ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]

GO

ALTER TABLE [dbo].[Variante_Producto]  WITH CHECK ADD  CONSTRAINT [FK_Variante_Producto] FOREIGN KEY([id_producto])

REFERENCES [dbo].[Producto] ([id_producto])

GO

ALTER TABLE [dbo].[Variante_Producto] CHECK CONSTRAINT [FK_Variante_Producto]

GO

ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Cliente] FOREIGN KEY([id_cliente])

REFERENCES [dbo].[Cliente] ([id_cliente])

GO

ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Cliente]

GO

ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Estado] FOREIGN KEY([id_estado_venta])

REFERENCES [dbo].[Estado_Venta] ([id_estado_venta])

GO

ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Estado]

GO

ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Modalidad] FOREIGN KEY([id_modalidad_entrega])

REFERENCES [dbo].[Modalidad_Entrega] ([id_modalidad_entrega])

GO

ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Modalidad]

GO

ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Usuario] FOREIGN KEY([id_usuario])

REFERENCES [dbo].[Usuario] ([id_usuario])

GO

ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Usuario]

GO
-- agrega una regla que impide guardar valores que no cumplan la condicion indicada
ALTER TABLE [dbo].[Cliente]  WITH CHECK ADD CHECK  (([saldo_a_favor]>=(0)))

GO

ALTER TABLE [dbo].[Detalle_Ingreso]  WITH CHECK ADD CHECK  (([cantidad]>(0)))

GO

ALTER TABLE [dbo].[Detalle_Ingreso]  WITH CHECK ADD CHECK  (([costo_unitario]>=(0)))

GO

ALTER TABLE [dbo].[Detalle_Remito]  WITH CHECK ADD CHECK  (([cantidad]>(0)))

GO

ALTER TABLE [dbo].[Detalle_Venta]  WITH CHECK ADD CHECK  (([cantidad]>(0)))

GO

ALTER TABLE [dbo].[Detalle_Venta]  WITH CHECK ADD CHECK  (([precio_unitario]>=(0)))

GO

ALTER TABLE [dbo].[Detalle_Venta]  WITH CHECK ADD CHECK  (([subtotal]>=(0)))

GO

ALTER TABLE [dbo].[Envio]  WITH CHECK ADD CHECK  (([costo]>=(0)))

GO

ALTER TABLE [dbo].[Pago]  WITH CHECK ADD CHECK  (([monto]>=(0)))

GO

ALTER TABLE [dbo].[Pago]  WITH CHECK ADD CHECK  (([monto_final]>=(0)))

GO

ALTER TABLE [dbo].[Variante_Producto]  WITH CHECK ADD CHECK  (([precio]>=(0)))

GO

ALTER TABLE [dbo].[Variante_Producto]  WITH CHECK ADD CHECK  (([stock_disponible]>=(0)))

GO

ALTER TABLE [dbo].[Venta]  WITH CHECK ADD CHECK  (([descuento]>=(0)))

GO

ALTER TABLE [dbo].[Venta]  WITH CHECK ADD CHECK  (([total]>=(0)))

GO

-- datos iniciales de la aplicacion permite indicar manualmente el identificador 1 del rol inicial
SET IDENTITY_INSERT [dbo].[Rol] ON;

GO

-- comprueba que el registro no exista para evitar insertarlo dos veces comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF NOT EXISTS
(
    -- consulta los datos o metadatos indicados; por si sola no guarda cambios en las tablas
    SELECT 1
    -- define el origen de los registros utilizados por la consulta
    FROM [dbo].[Rol]
    -- limita los registros afectados o consultados; es importante comprobarlo antes de ejecutar escrituras
    WHERE [id_rol_usuario] = 1
)
-- inicio del grupo de instrucciones correspondiente a la condicion o al flujo anterior
BEGIN
    -- inserta el rol administrador requerido por el usuario inicial
    INSERT INTO [dbo].[Rol]
    (
        [id_rol_usuario],
        [nombre_rol_usuario]
    )
    VALUES
    (
        1,
        N'Administrador'
    );
-- fin del bloque agrupado; las instrucciones siguientes quedan fuera de ese grupo
END;

GO

-- devuelve a sql server el control automatico de los identificadores de rol
SET IDENTITY_INSERT [dbo].[Rol] OFF;

GO

-- permite indicar manualmente el identificador 1 del usuario inicial
SET IDENTITY_INSERT [dbo].[Usuario] ON;

GO


IF NOT EXISTS
(
    
    SELECT 1
    
    FROM [dbo].[Usuario]
    
    WHERE [nombre_usuario] = N'admin'
)

BEGIN
    -- inserta el usuario admin utilizado para el primer inicio de sesion
    INSERT INTO [dbo].[Usuario]
    (
        [id_usuario],
        [nombre_usuario],
        [hash_contrasena],
        [id_rol_usuario],
        [activo]
    )
    VALUES
    (
        1,
        N'admin',
        N'$2a$11$gsz7kf9BQFdxU9.hSy8XJON16Yir5OY1FTV0Hxc6TNy8BMKoW4Yvi',
        1,
        1
    );

END;

GO

-- devuelve a sql server el control automatico de los identificadores de usuario
SET IDENTITY_INSERT [dbo].[Usuario] OFF;

GO

-- completa la estructura actual: cuotas, historial, logistica y rol vendedor actualizacion acumulativa e idempotente se ejecuta sobre la conexion abierta por el inicializador, sin use ni borrados las columnas existentes y los datos de trabajo se conservan comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.Usuario',N'U') IS NULL OR OBJECT_ID(N'dbo.Venta',N'U') IS NULL
    OR OBJECT_ID(N'dbo.Medio_Pago',N'U') IS NULL OR OBJECT_ID(N'dbo.Rol',N'U') IS NULL
    -- detiene el flujo con un error explicativo en lugar de continuar con una estructura incompatible
    THROW 50001, N'La estructura base está incompleta. No se puede actualizar automáticamente.', 1;

GO

-- recargos por cuotas y baja logica de los medios de pago comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Medio_Pago',N'recargo_2') IS NULL
    -- modifica una tabla existente add incorpora estructura; alter cambia su tipo; drop elimina el objeto indicado
    ALTER TABLE dbo.Medio_Pago ADD recargo_2 decimal(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo2 DEFAULT(0) WITH VALUES;

GO
-- comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Medio_Pago',N'recargo_3') IS NULL
    
    ALTER TABLE dbo.Medio_Pago ADD recargo_3 decimal(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo3 DEFAULT(0) WITH VALUES;

GO

IF COL_LENGTH(N'dbo.Medio_Pago',N'recargo_6') IS NULL
    
    ALTER TABLE dbo.Medio_Pago ADD recargo_6 decimal(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo6 DEFAULT(0) WITH VALUES;

GO

IF COL_LENGTH(N'dbo.Medio_Pago',N'recargo_9') IS NULL
    
    ALTER TABLE dbo.Medio_Pago ADD recargo_9 decimal(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo9 DEFAULT(0) WITH VALUES;

GO

IF COL_LENGTH(N'dbo.Medio_Pago',N'recargo_12') IS NULL
    
    ALTER TABLE dbo.Medio_Pago ADD recargo_12 decimal(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo12 DEFAULT(0) WITH VALUES;

GO

IF COL_LENGTH(N'dbo.Medio_Pago',N'activo') IS NULL
    
    ALTER TABLE dbo.Medio_Pago ADD activo bit NOT NULL CONSTRAINT DF_MedioPago_Activo DEFAULT(1) WITH VALUES;

GO

-- amplia el descuento monetario antiguo sin reducir columnas de mayor capacidad comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Venta')
    AND name=N'descuento' AND precision<10 AND scale=2 AND TYPE_NAME(system_type_id) IN (N'decimal',N'numeric'))

BEGIN
    
    IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Venta') AND name=N'descuento' AND is_nullable=1)
        
        ALTER TABLE dbo.Venta ALTER COLUMN descuento decimal(10,2) NULL;
    ELSE
        
        ALTER TABLE dbo.Venta ALTER COLUMN descuento decimal(10,2) NOT NULL;

END;

GO

-- historial: la fecha se registra automaticamente en cada operacion comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.LogActividad',N'U') IS NULL

BEGIN
    -- crea la estructura de la tabla: cada columna define un dato y las restricciones protegen su integridad
    CREATE TABLE dbo.LogActividad (
        id_log int IDENTITY(1,1) NOT NULL CONSTRAINT PK_LogActividad PRIMARY KEY,
        id_usuario int NOT NULL,
        accion nvarchar(100) NOT NULL,
        entidad nvarchar(100) NOT NULL,
        id_entidad int NULL,
        detalle nvarchar(1000) NULL,
        fecha datetime2 NOT NULL CONSTRAINT DF_LogActividad_Fecha DEFAULT(SYSDATETIME()),
        
        CONSTRAINT FK_LogActividad_Usuario FOREIGN KEY(id_usuario) REFERENCES dbo.Usuario(id_usuario)
    );

END;

GO

-- datos de logistica que se guardan junto con una nueva venta comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.Venta_Entrega',N'U') IS NULL

BEGIN
    
    CREATE TABLE dbo.Venta_Entrega (
        id_venta int NOT NULL CONSTRAINT PK_Venta_Entrega PRIMARY KEY,
        domicilio nvarchar(200) NULL,
        fecha_estimada datetime2 NULL,
        horario_sugerido nvarchar(50) NULL,
        observaciones nvarchar(500) NULL,
        tipo_logistica nvarchar(50) NULL,
        costo_envio decimal(10,2) NOT NULL,
        observacion_pago nvarchar(500) NULL,
        condicion_pago nvarchar(30) NOT NULL,
        
        CONSTRAINT FK_VentaEntrega_Venta FOREIGN KEY(id_venta) REFERENCES dbo.Venta(id_venta)
    );

END;

GO

-- el codigo de permisos usa el identificador 2 para vendedor; no altera roles existentes comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF NOT EXISTS(SELECT 1 FROM dbo.Rol WHERE id_rol_usuario=2)

BEGIN
    -- configura una opcion de la sesion sql; su alcance y valor se indican en la instruccion
    SET IDENTITY_INSERT dbo.Rol ON;
    -- incorpora filas; revisar las columnas y el filtro de existencia para no duplicar configuracion o historial
    INSERT INTO dbo.Rol(id_rol_usuario,nombre_rol_usuario) VALUES(2,N'Vendedor');
    
    SET IDENTITY_INSERT dbo.Rol OFF;

END;

GO

-- unifica las condiciones reconocidas sin modificar otras condiciones existentes modifica las filas que cumplan el filtro; no agrega una nueva operacion por si sola
UPDATE dbo.Variante_Producto

SET condicion_producto = UPPER(LTRIM(RTRIM(condicion_producto)))

WHERE UPPER(LTRIM(RTRIM(condicion_producto))) IN (N'PRIMERA', N'OUTLET')
  AND condicion_producto COLLATE Latin1_General_100_BIN2
      <> UPPER(LTRIM(RTRIM(condicion_producto))) COLLATE Latin1_General_100_BIN2;

GO
