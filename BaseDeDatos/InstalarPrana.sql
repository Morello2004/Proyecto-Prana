-- Instalacion limpia de Prana. Ejecutar solo en una base vacia.
-- Generado desde la secuencia completa de creacion del esquema vigente.

-- ===== CrearEstructuraPrana_LocalDB.sql =====

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
-- localidad en texto heredada; se reemplaza por id_localidad al completar la instalacion
	[localidad] [nvarchar](25) NULL,

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
-- indica si todas las unidades de este renglón se entregaron
	[entregado] [bit] NOT NULL CONSTRAINT [DF_DetalleVenta_Entregado] DEFAULT ((0)),

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
	[direccion] [nvarchar](200) NULL,
	[localidad] [nvarchar](25) NULL,
	[entregado] [bit] NOT NULL CONSTRAINT [DF_Envio_Entregado] DEFAULT ((0)),
-- define fecha_entrega como fecha y hora, opcional
	[fecha_entrega] [datetime2](7) NULL,

PRIMARY KEY CLUSTERED
(
	[id_envio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

-- tabla: estado_venta
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

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

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
    [observacion_pago] [nvarchar](500) NULL,
    [condicion_pago] [nvarchar](30) NOT NULL CONSTRAINT DF_Venta_CondicionPago DEFAULT(N''),
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
        domicilio nvarchar(200) NULL, localidad nvarchar(25) NULL,
        fecha_estimada datetime2 NULL,
        horario_sugerido nvarchar(50) NULL,
        observaciones nvarchar(500) NULL,
        tipo_logistica nvarchar(50) NULL,
        costo_envio decimal(10,2) NOT NULL,
        
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


-- ===== ActualizarEstructuraPrana_LocalDB.sql =====

-- actualizacion acumulativa e idempotente se ejecuta sobre la conexion abierta por el inicializador, sin cambiar la base con use incluye altas, adaptaciones y eliminacion de columnas o tablas antiguas sin uso respaldar antes de actualizar comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.Usuario',N'U') IS NULL OR OBJECT_ID(N'dbo.Venta',N'U') IS NULL
    OR OBJECT_ID(N'dbo.Medio_Pago',N'U') IS NULL OR OBJECT_ID(N'dbo.Rol',N'U') IS NULL
    OR OBJECT_ID(N'dbo.Cliente',N'U') IS NULL
    -- detiene el flujo con un error explicativo en lugar de continuar con una estructura incompatible
    THROW 50001, N'La estructura base está incompleta. No se puede actualizar automáticamente.', 1;
-- fin del lote el inicializador divide el archivo aqui; go no es una consulta enviada por sqlcommand
GO

-- estado de actividad y baja logica de los clientes comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Cliente',N'activo') IS NULL
    -- modifica una tabla existente add incorpora estructura; alter cambia su tipo; drop elimina el objeto indicado
    ALTER TABLE dbo.Cliente ADD activo bit NOT NULL CONSTRAINT DF_Cliente_Activo DEFAULT(1) WITH VALUES;
-- comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Cliente',N'eliminado') IS NULL
    
    ALTER TABLE dbo.Cliente ADD eliminado bit NOT NULL CONSTRAINT DF_Cliente_Eliminado DEFAULT(0) WITH VALUES;

GO

-- recargos por cuotas y baja logica de los medios de pago comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Medio_Pago',N'recargo_2') IS NULL
    
    ALTER TABLE dbo.Medio_Pago ADD recargo_2 decimal(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo2 DEFAULT(0) WITH VALUES;

GO

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

-- agrega los campos necesarios para limitar los intentos de inicio de sesion comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Usuario',N'intentos_login_fallidos') IS NULL
    
    ALTER TABLE dbo.Usuario ADD intentos_login_fallidos int NOT NULL CONSTRAINT DF_Usuario_IntentosLogin DEFAULT(0) WITH VALUES;

IF COL_LENGTH(N'dbo.Usuario',N'bloqueado_hasta') IS NULL
    
    ALTER TABLE dbo.Usuario ADD bloqueado_hasta datetime2(7) NULL;

GO

-- amplia el descuento monetario antiguo sin reducir columnas de mayor capacidad comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Venta')
    AND name=N'descuento' AND precision<10 AND scale=2 AND TYPE_NAME(system_type_id) IN (N'decimal',N'numeric'))
-- inicio del grupo de instrucciones correspondiente a la condicion o al flujo anterior
BEGIN
    
    IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Venta') AND name=N'descuento' AND is_nullable=1)
        
        ALTER TABLE dbo.Venta ALTER COLUMN descuento decimal(10,2) NULL;
    ELSE
        
        ALTER TABLE dbo.Venta ALTER COLUMN descuento decimal(10,2) NOT NULL;
-- fin del bloque agrupado; las instrucciones siguientes quedan fuera de ese grupo
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
        -- da nombre o define una regla de integridad para poder comprobarla o administrarla despues
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
        
        CONSTRAINT FK_VentaEntrega_Venta FOREIGN KEY(id_venta) REFERENCES dbo.Venta(id_venta)
    );

END;

GO

-- La normalización de envíos se realiza en NormalizarEnviosYPagos.sql.
-- el remito se genera siempre; la columna solo almacenaba un indicador redundante comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Venta_Entrega',N'generar_remito') IS NOT NULL
    
    ALTER TABLE dbo.Venta_Entrega DROP COLUMN generar_remito;

GO

-- quita el catalogo de formatos, que no selecciona ni almacena plantillas reales comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.Formato_Documento',N'U') IS NOT NULL

BEGIN
    
    DECLARE @quitarFormatoDocumento nvarchar(max) = N'';
    
    SELECT @quitarFormatoDocumento = @quitarFormatoDocumento +
        N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(fk.parent_object_id)) + N'.' +
        QUOTENAME(OBJECT_NAME(fk.parent_object_id)) + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N';'
    
    FROM sys.foreign_keys fk
    
    WHERE fk.referenced_object_id = OBJECT_ID(N'dbo.Formato_Documento');
    
    EXEC sys.sp_executesql @quitarFormatoDocumento;

    
    IF COL_LENGTH(N'dbo.Remito',N'id_formato_documento') IS NOT NULL
        
        ALTER TABLE dbo.Remito DROP COLUMN id_formato_documento;

    
    DROP TABLE dbo.Formato_Documento;

END;

GO

-- el esquema de devoluciones no tiene uso operativo en la aplicacion comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.Devolucion',N'U') IS NOT NULL
    
    DROP TABLE dbo.Devolucion;

IF OBJECT_ID(N'dbo.Tipo_Resolucion',N'U') IS NOT NULL
    
    DROP TABLE dbo.Tipo_Resolucion;

GO

-- elimina el correo sin uso y sus restricciones en instalaciones anteriores busca la restriccion por columna: su nombre cambia entre instalaciones declara una variable local del lote; no es una columna permanente de una tabla
DECLARE @quitarUnicidadEmail nvarchar(max) = N'';

SELECT @quitarUnicidadEmail = @quitarUnicidadEmail +
    N'ALTER TABLE dbo.Usuario DROP CONSTRAINT ' + QUOTENAME(k.name) + N';'

FROM sys.key_constraints k

WHERE k.parent_object_id = OBJECT_ID(N'dbo.Usuario') AND k.type = 'UQ'
  AND (SELECT COUNT(*) FROM sys.index_columns ic
       
       WHERE ic.object_id=k.parent_object_id AND ic.index_id=k.unique_index_id AND ic.key_ordinal>0)=1
  AND EXISTS (SELECT 1 FROM sys.index_columns ic JOIN sys.columns c
      ON c.object_id=ic.object_id AND c.column_id=ic.column_id
      
      WHERE ic.object_id=k.parent_object_id AND ic.index_id=k.unique_index_id
        AND ic.key_ordinal=1 AND c.name=N'email');

EXEC sys.sp_executesql @quitarUnicidadEmail;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Usuario') AND name=N'UX_Usuario_EmailInformado')
    -- retira el indice indicado, sin borrar por si mismo las filas de la tabla
    DROP INDEX UX_Usuario_EmailInformado ON dbo.Usuario;

IF COL_LENGTH(N'dbo.Usuario', N'email') IS NOT NULL
    
    ALTER TABLE dbo.Usuario DROP COLUMN email;

IF COL_LENGTH(N'dbo.Cliente', N'email') IS NOT NULL
    
    ALTER TABLE dbo.Cliente DROP COLUMN email;

IF COL_LENGTH(N'dbo.Proveedor', N'email') IS NOT NULL
    
    ALTER TABLE dbo.Proveedor DROP COLUMN email;

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

-- unifica la condicion inicial de las ventas anteriores sin modificar sus importes comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Venta_Entrega',N'condicion_pago') IS NOT NULL
    
    EXEC(N'UPDATE dbo.Venta_Entrega SET condicion_pago=N''Pago parcial'' WHERE condicion_pago=N''Seña para reserva''');

GO

-- convierte el codigo postal numerico de localidad a texto sin cambiar sus valores comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF EXISTS (
    
    SELECT 1
    
    FROM sys.columns
    
    WHERE object_id = OBJECT_ID(N'dbo.Localidad')
      AND name = N'codigo_postal'
      AND TYPE_NAME(system_type_id) = N'int'
)

BEGIN
    
    ALTER TABLE dbo.Localidad ALTER COLUMN codigo_postal nvarchar(12) NULL;

END;

GO

-- unifica las condiciones reconocidas sin modificar otras condiciones existentes modifica las filas que cumplan el filtro; no agrega una nueva operacion por si sola
UPDATE dbo.Variante_Producto

SET condicion_producto = UPPER(LTRIM(RTRIM(condicion_producto)))

WHERE UPPER(LTRIM(RTRIM(condicion_producto))) IN (N'PRIMERA', N'OUTLET')
  AND condicion_producto COLLATE Latin1_General_100_BIN2
      <> UPPER(LTRIM(RTRIM(condicion_producto))) COLLATE Latin1_General_100_BIN2;

GO

-- iguala el costo de variante al precio sin recortar valores existentes comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF EXISTS (
    
    SELECT 1 FROM sys.columns
    
    WHERE object_id = OBJECT_ID(N'dbo.Variante_Producto')
      AND name = N'costo'
      AND TYPE_NAME(system_type_id) IN (N'decimal', N'numeric')
      AND precision = 18 AND scale = 2
)

BEGIN
    -- conserva el bloqueo hasta que termine la transaccion del inicializador comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
    IF EXISTS (
        
        SELECT 1 FROM dbo.Variante_Producto WITH (TABLOCKX, HOLDLOCK)
        
        WHERE costo > 99999999.99 OR costo < -99999999.99
    )
        
        THROW 50005, N'No se puede convertir el costo a decimal(10,2): existen valores fuera del rango permitido.', 1;

    
    ALTER TABLE dbo.Variante_Producto ALTER COLUMN costo decimal(10,2) NOT NULL;

END;

GO

-- conserva los documentos existentes y los identifica inicialmente como dni comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Cliente', N'tipo_documento') IS NULL

BEGIN
    
    ALTER TABLE dbo.Cliente ADD tipo_documento nvarchar(4) NOT NULL
        
        CONSTRAINT DF_Cliente_TipoDocumento DEFAULT (N'DNI') WITH VALUES;

END;

GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.Cliente') AND name = N'CK_Cliente_TipoDocumento')
    
    ALTER TABLE dbo.Cliente WITH CHECK ADD CONSTRAINT CK_Cliente_TipoDocumento CHECK (tipo_documento IN (N'DNI', N'CUIT'));

GO
-- reserva el nombre para siempre, incluso cuando la cuenta esta dada de baja comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF NOT EXISTS (SELECT 1 FROM sys.indexes
    
    WHERE object_id=OBJECT_ID(N'dbo.Usuario') AND name=N'UX_Usuario_Nombre')
AND NOT EXISTS (SELECT nombre_usuario FROM dbo.Usuario GROUP BY nombre_usuario HAVING COUNT(*) > 1)

BEGIN
    -- las bases antiguas con duplicados pueden iniciar sin cambiar cuentas existentes crearusuario y modificarusuario validan igualmente todos los nombres crea un indice que rechaza claves repetidas; no crea otra tabla de datos
    CREATE UNIQUE INDEX UX_Usuario_Nombre ON dbo.Usuario(nombre_usuario);

END;

GO


-- ===== CajaYComprobantes.sql =====

-- migracion idempotente, aplicada en la transaccion del inicializador nunca renumera ventas existentes ni retrocede una numeracion superior comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COALESCE(IDENT_CURRENT(N'dbo.Venta'),0) < 2500 AND NOT EXISTS(SELECT 1 FROM dbo.Venta WHERE id_venta >= 2500)
-- inicio del grupo de instrucciones correspondiente a la condicion o al flujo anterior
BEGIN
    -- comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
    IF EXISTS(SELECT 1 FROM sys.identity_columns WHERE object_id=OBJECT_ID(N'dbo.Venta') AND last_value IS NULL)
        -- ajusta o comprueba informacion interna indicada, como el contador identity; no equivale a insertar una venta
        DBCC CHECKIDENT ('dbo.Venta', RESEED, 2500) WITH NO_INFOMSGS;
    ELSE
        
        DBCC CHECKIDENT ('dbo.Venta', RESEED, 2499) WITH NO_INFOMSGS;
-- fin del bloque agrupado; las instrucciones siguientes quedan fuera de ese grupo
END;
-- fin del lote el inicializador divide el archivo aqui; go no es una consulta enviada por sqlcommand
GO

-- conserva la clasificacion interna de los cobros aunque no se muestre como campo editable en medios de pago
IF COL_LENGTH(N'dbo.Medio_Pago',N'tipo_caja') IS NULL

BEGIN
    -- modifica una tabla existente add incorpora estructura; alter cambia su tipo; drop elimina el objeto indicado
    ALTER TABLE dbo.Medio_Pago ADD tipo_caja nvarchar(20) NOT NULL CONSTRAINT DF_Medio_TipoCaja DEFAULT(N'Otros');
    -- ejecuta la instruccion o procedimiento indicado el sql dinamico permite adaptar nombres de objetos antiguos
    EXEC(N'UPDATE dbo.Medio_Pago SET tipo_caja=CASE
        WHEN nombre_medio_pago COLLATE Latin1_General_CI_AI LIKE ''%efectivo%'' THEN N''Efectivo''
        WHEN nombre_medio_pago COLLATE Latin1_General_CI_AI LIKE ''%transf%'' THEN N''Transferencia''
        WHEN nombre_medio_pago COLLATE Latin1_General_CI_AI LIKE ''%debito%'' THEN N''Débito''
        WHEN nombre_medio_pago COLLATE Latin1_General_CI_AI LIKE ''%credito%'' OR nombre_medio_pago LIKE ''%Tarjeta%'' THEN N''Crédito'' ELSE N''Otros'' END');

END;

GO

-- copia el tipo del medio al cobro para conservar su clasificacion historica
IF COL_LENGTH(N'dbo.Pago',N'tipo_caja') IS NULL

BEGIN
    
    ALTER TABLE dbo.Pago ADD tipo_caja nvarchar(20) NULL;
    
    EXEC(N'UPDATE p SET tipo_caja=m.tipo_caja FROM dbo.Pago p JOIN dbo.Medio_Pago m ON m.id_medio_pago=p.id_medio_pago');

END;

GO

-- guarda ingresos y gastos manuales la anulacion conserva la fila y su motivo en lugar de borrarla
IF OBJECT_ID(N'dbo.Caja_Movimiento',N'U') IS NULL
    -- crea la estructura de la tabla: cada columna define un dato y las restricciones protegen su integridad
    CREATE TABLE dbo.Caja_Movimiento (
        id int IDENTITY PRIMARY KEY, fecha datetime2 NOT NULL DEFAULT(SYSDATETIME()),
        concepto nvarchar(200) NOT NULL, tipo nvarchar(20) NOT NULL CHECK(tipo IN(N'Gasto',N'Ingreso extra',N'Café')),
        medio nvarchar(20) NOT NULL, moneda char(3) NOT NULL CHECK(moneda IN('ARS','USD')),
        importe decimal(18,2) NOT NULL CHECK(importe>0), cuenta nvarchar(100) NOT NULL,
        id_usuario int NOT NULL REFERENCES dbo.Usuario(id_usuario), anulado bit NOT NULL DEFAULT(0),
        motivo_anulacion nvarchar(200) NULL);

GO

-- agrega la referencia del destino del dinero a cobros existentes sin perder sus importes
IF COL_LENGTH(N'dbo.Pago',N'cuenta_destino') IS NULL
    
    ALTER TABLE dbo.Pago ADD cuenta_destino nvarchar(100) NOT NULL CONSTRAINT DF_Pago_Cuenta DEFAULT(N'');

GO

-- una fila representa todo el dia primero abierta y luego cerrada sin eliminarla al cerrar
-- CajasPorTurno permite varias aperturas por fecha. Los movimientos se consultan por sus claves foráneas.
-- id_usuario y fecha_cierre se completan al cerrar el responsable queda nulo en cierres automaticos
IF OBJECT_ID(N'dbo.Caja',N'U') IS NULL
    CREATE TABLE dbo.Caja (
        id int IDENTITY PRIMARY KEY,
        numero_cierre int NULL,
        fecha date NOT NULL CONSTRAINT UQ_Caja_Fecha UNIQUE,
        estado nvarchar(8) NOT NULL CONSTRAINT DF_Caja_Estado DEFAULT(N'Abierta')
            CONSTRAINT CK_Caja_Estado CHECK(estado IN(N'Abierta',N'Cerrada')),
        id_usuario int NULL REFERENCES dbo.Usuario(id_usuario),
        fecha_cierre datetime2 NULL,
        observaciones nvarchar(1000) NOT NULL CONSTRAINT DF_Caja_Observaciones DEFAULT(N''),
        CONSTRAINT CK_Caja_Cierre CHECK(
            (estado=N'Abierta' AND fecha_cierre IS NULL AND id_usuario IS NULL)
            OR (estado=N'Cerrada' AND fecha_cierre IS NOT NULL)));

GO

-- Solo las versiones antiguas requieren una columna temporal para recuperar su JSON.
IF (OBJECT_ID(N'dbo.Caja_Cierre',N'U') IS NOT NULL OR OBJECT_ID(N'dbo.Caja_Abierta',N'U') IS NOT NULL)
    AND COL_LENGTH(N'dbo.Caja',N'datos') IS NULL
    ALTER TABLE dbo.Caja ADD datos nvarchar(max) NULL;
GO
-- la migracion se ejecuta en la transaccion del inicializador conserva numeros fechas y json de cierres anteriores
-- el sql dinamico permite iniciar bases nuevas donde las tablas antiguas nunca existieron
IF OBJECT_ID(N'dbo.Caja_Cierre',N'U') IS NOT NULL
    EXEC(N'
        IF EXISTS(SELECT 1 FROM dbo.Caja_Cierre a JOIN dbo.Caja c ON c.fecha=a.fecha
            WHERE c.id<>a.id OR c.estado<>N''Cerrada'' OR c.datos COLLATE Latin1_General_100_BIN2<>a.datos COLLATE Latin1_General_100_BIN2)
            THROW 50020,N''Existen cajas incompatibles con los cierres anteriores. No se modificaron sus datos.'',1;
        IF EXISTS(SELECT 1 FROM dbo.Caja_Cierre a JOIN dbo.Caja c ON c.id=a.id WHERE c.fecha<>a.fecha)
            THROW 50021,N''Un numero de caja coincide con otro cierre anterior. Revise la migracion.'',1;
        BEGIN TRY
            SET IDENTITY_INSERT dbo.Caja ON;
            INSERT dbo.Caja(id,fecha,estado,id_usuario,fecha_cierre,datos)
                SELECT a.id,a.fecha,N''Cerrada'',a.id_usuario,a.fecha_cierre,a.datos
                FROM dbo.Caja_Cierre a WHERE NOT EXISTS(SELECT 1 FROM dbo.Caja c WHERE c.fecha=a.fecha);
            SET IDENTITY_INSERT dbo.Caja OFF;
        END TRY
        BEGIN CATCH
            SET IDENTITY_INSERT dbo.Caja OFF;
            THROW;
        END CATCH;
        IF EXISTS(SELECT 1 FROM dbo.Caja_Cierre a WHERE NOT EXISTS(
            SELECT 1 FROM dbo.Caja c WHERE c.fecha=a.fecha AND c.id=a.id AND c.estado=N''Cerrada''
                AND c.datos COLLATE Latin1_General_100_BIN2=a.datos COLLATE Latin1_General_100_BIN2))
            THROW 50022,N''No se pudieron conservar todos los cierres anteriores.'',1;');

-- copia los datos de dias abiertos solo cuando no hay un cierre definitivo para esa misma fecha
IF OBJECT_ID(N'dbo.Caja_Abierta',N'U') IS NOT NULL
    EXEC(N'
        INSERT dbo.Caja(fecha,estado,datos)
            SELECT a.fecha,N''Abierta'',a.datos FROM dbo.Caja_Abierta a
            WHERE NOT EXISTS(SELECT 1 FROM dbo.Caja c WHERE c.fecha=a.fecha);
        IF EXISTS(SELECT 1 FROM dbo.Caja_Abierta a WHERE NOT EXISTS(
            SELECT 1 FROM dbo.Caja c WHERE c.fecha=a.fecha AND (c.estado=N''Cerrada''
                OR c.datos COLLATE Latin1_General_100_BIN2=a.datos COLLATE Latin1_General_100_BIN2)))
            THROW 50023,N''No se pudieron conservar todas las cajas abiertas.'',1;');

-- retira las estructuras anteriores despues de verificar sus datos la misma transaccion revierte todo si hay un error
IF OBJECT_ID(N'dbo.Caja_Abierta',N'U') IS NOT NULL
    DROP TABLE dbo.Caja_Abierta;
IF OBJECT_ID(N'dbo.Caja_Cierre',N'U') IS NOT NULL
    DROP TABLE dbo.Caja_Cierre;

GO
-- los comprobantes se construyen desde las tablas de la venta retira la copia json de instalaciones anteriores; no elimina ventas ni pagos comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.Venta_Comprobante',N'U') IS NOT NULL
    -- elimina la tabla indicada y sus filas este paso requiere respaldo y revision de sus dependencias
    DROP TABLE dbo.Venta_Comprobante;

GO




-- recupera ventas anteriores con su fecha y vendedor originales, sin duplicarlas
INSERT LogActividad(id_usuario,accion,entidad,id_entidad,detalle,fecha)
-- consulta los datos o metadatos indicados; por si sola no guarda cambios en las tablas
SELECT v.id_usuario,N'REGISTRAR_VENTA',N'Venta',v.id_venta,
 CONCAT(N'Venta / RTO ',v.id_venta,N' | Total ARS: ',FORMAT(v.total,'N2','es-AR'),
 N' | Descuento ARS: ',FORMAT(COALESCE(v.descuento,0),'N2','es-AR'),N' | Recuperada del registro de ventas'),v.fecha_venta
-- define el origen de los registros utilizados por la consulta
FROM Venta v WHERE NOT EXISTS(SELECT 1 FROM LogActividad l WHERE l.entidad=N'Venta' AND l.id_entidad=v.id_venta AND l.accion=N'REGISTRAR_VENTA');
-- incorpora filas; revisar las columnas y el filtro de existencia para no duplicar configuracion o historial
INSERT LogActividad(id_usuario,accion,entidad,id_entidad,detalle,fecha)

SELECT v.id_usuario,N'APLICAR_DESCUENTO',N'Venta',v.id_venta,
 CONCAT(N'Venta / RTO ',v.id_venta,N' | Descuento ARS: ',FORMAT(v.descuento,'N2','es-AR'),
 N' | Total final ARS: ',FORMAT(v.total,'N2','es-AR'),N' | Recuperado del registro de ventas'),v.fecha_venta

FROM Venta v WHERE v.descuento>0 AND NOT EXISTS(SELECT 1 FROM LogActividad l WHERE l.entidad=N'Venta' AND l.id_entidad=v.id_venta AND l.accion=N'APLICAR_DESCUENTO');

GO

-- corrige los cuatro medios fijos sin alterar sus recargos generales ni los de credito modifica las filas que cumplan el filtro; no agrega una nueva operacion por si sola
UPDATE mp
-- configura una opcion de la sesion sql; su alcance y valor se indican en la instruccion
SET tipo_caja = medios.tipo_caja,
    activo = 1,
    recargo_2 = CASE WHEN medios.tipo_caja = N'Crédito' THEN mp.recargo_2 ELSE 0 END,
    recargo_3 = CASE WHEN medios.tipo_caja = N'Crédito' THEN mp.recargo_3 ELSE 0 END,
    recargo_6 = CASE WHEN medios.tipo_caja = N'Crédito' THEN mp.recargo_6 ELSE 0 END,
    recargo_9 = CASE WHEN medios.tipo_caja = N'Crédito' THEN mp.recargo_9 ELSE 0 END,
    recargo_12 = CASE WHEN medios.tipo_caja = N'Crédito' THEN mp.recargo_12 ELSE 0 END

FROM dbo.Medio_Pago mp
-- conserva solo las filas que tienen coincidencia en ambos origenes
INNER JOIN (VALUES
    (N'Efectivo', N'Efectivo'),
    (N'Transferencia', N'Transferencia'),
    (N'Débito', N'Débito'),
    (N'Crédito', N'Crédito')
) AS medios(nombre, tipo_caja)
    ON mp.nombre_medio_pago COLLATE Latin1_General_100_BIN2 = medios.nombre COLLATE Latin1_General_100_BIN2;

GO

-- crea los medios fijos que todavia no existan
INSERT INTO dbo.Medio_Pago
    (nombre_medio_pago, recargo_medio_pago, recargo_2, recargo_3,
     recargo_6, recargo_9, recargo_12, activo, tipo_caja)

SELECT medios.nombre, 0, 0, 0, 0, 0, 0, 1, medios.tipo_caja

FROM (VALUES
    (N'Efectivo', N'Efectivo'),
    (N'Transferencia', N'Transferencia'),
    (N'Débito', N'Débito'),
    (N'Crédito', N'Crédito')
) AS medios(nombre, tipo_caja)
-- limita los registros afectados o consultados; es importante comprobarlo antes de ejecutar escrituras
WHERE NOT EXISTS
(
    
    SELECT 1
    
    FROM dbo.Medio_Pago mp
    
    WHERE mp.nombre_medio_pago COLLATE Latin1_General_100_BIN2 = medios.nombre COLLATE Latin1_General_100_BIN2
);

GO


-- ===== CajasPorTurno.sql =====

-- agrega los horarios propios de cada apertura sin cambiar los identificadores de cajas anteriores
IF COL_LENGTH(N'dbo.Caja',N'inicio') IS NULL
    ALTER TABLE dbo.Caja ADD inicio datetime2 NULL;
IF COL_LENGTH(N'dbo.Caja',N'fin') IS NULL
    ALTER TABLE dbo.Caja ADD fin datetime2 NULL;
IF COL_LENGTH(N'dbo.Caja',N'id_usuario_apertura') IS NULL
    ALTER TABLE dbo.Caja ADD id_usuario_apertura int NULL REFERENCES dbo.Usuario(id_usuario);
IF COL_LENGTH(N'dbo.Pago',N'id_caja') IS NULL
    ALTER TABLE dbo.Pago ADD id_caja int NULL;
IF COL_LENGTH(N'dbo.Caja_Movimiento',N'id_caja') IS NULL
    ALTER TABLE dbo.Caja_Movimiento ADD id_caja int NULL;
GO

-- las operaciones anteriores se relacionan por fecha una sola vez antes de habilitar varios turnos diarios
-- si no habia fila de caja para un dia con operaciones la crea para conservar esas operaciones
IF COL_LENGTH(N'dbo.Caja',N'datos') IS NOT NULL
    EXEC(N'INSERT dbo.Caja(fecha,estado,inicio,datos)
SELECT dias.fecha,N''Abierta'',CONVERT(datetime2,dias.fecha),
    (SELECT dias.fecha AS Fecha,CONVERT(datetime2,dias.fecha) AS Inicio FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)
FROM (SELECT CONVERT(date,fecha_pago) fecha FROM dbo.Pago WHERE id_caja IS NULL
      UNION SELECT CONVERT(date,fecha) FROM dbo.Caja_Movimiento WHERE id_caja IS NULL) dias
WHERE NOT EXISTS(SELECT 1 FROM dbo.Caja c WHERE c.fecha=dias.fecha);');
ELSE
    INSERT dbo.Caja(fecha,estado,inicio)
    SELECT dias.fecha,N'Abierta',CONVERT(datetime2,dias.fecha)
    FROM (SELECT CONVERT(date,fecha_pago) fecha FROM dbo.Pago WHERE id_caja IS NULL
          UNION SELECT CONVERT(date,fecha) FROM dbo.Caja_Movimiento WHERE id_caja IS NULL) dias
    WHERE NOT EXISTS(SELECT 1 FROM dbo.Caja c WHERE c.fecha=dias.fecha);
-- rechaza asignaciones ambiguas en lugar de repartir movimientos antiguos entre turnos arbitrarios
IF EXISTS(SELECT 1 FROM dbo.Pago p WHERE p.id_caja IS NULL AND
    (SELECT COUNT(*) FROM dbo.Caja c WHERE c.fecha=CONVERT(date,p.fecha_pago))<>1)
    THROW 50030,N'No se pudo identificar la caja de un pago anterior.',1;
IF EXISTS(SELECT 1 FROM dbo.Caja_Movimiento m LEFT JOIN dbo.Usuario u ON u.id_usuario=m.id_usuario WHERE m.id_caja IS NULL AND
    (SELECT COUNT(*) FROM dbo.Caja c WHERE c.fecha=CONVERT(date,m.fecha))<>1)
    THROW 50031,N'No se pudo identificar la caja de un movimiento anterior.',1;
UPDATE p SET id_caja=c.id FROM dbo.Pago p JOIN dbo.Caja c ON c.fecha=CONVERT(date,p.fecha_pago) WHERE p.id_caja IS NULL;
UPDATE m SET id_caja=c.id FROM dbo.Caja_Movimiento m JOIN dbo.Caja c ON c.fecha=CONVERT(date,m.fecha) WHERE m.id_caja IS NULL;

-- recupera horarios historicos desde el json y usa la fecha cuando una version anterior no los guardaba
IF COL_LENGTH(N'dbo.Caja',N'datos') IS NOT NULL
    EXEC(N'UPDATE dbo.Caja SET inicio=COALESCE(TRY_CONVERT(datetime2,JSON_VALUE(datos,''$.Inicio''),126),CONVERT(datetime2,fecha)) WHERE inicio IS NULL;');
ELSE UPDATE dbo.Caja SET inicio=CONVERT(datetime2,fecha) WHERE inicio IS NULL;
IF COL_LENGTH(N'dbo.Caja',N'datos') IS NOT NULL
    EXEC(N'UPDATE dbo.Caja SET fin=COALESCE(TRY_CONVERT(datetime2,JSON_VALUE(datos,''$.Fin''),126),fecha_cierre,DATEADD(day,1,CONVERT(datetime2,fecha))) WHERE estado=N''Cerrada'' AND fin IS NULL;');
ELSE UPDATE dbo.Caja SET fin=COALESCE(fecha_cierre,DATEADD(day,1,CONVERT(datetime2,fecha))) WHERE estado=N'Cerrada' AND fin IS NULL;
UPDATE dbo.Caja SET fin=COALESCE(fecha_cierre,inicio) WHERE estado=N'Cerrada' AND fin<inicio;
UPDATE dbo.Caja SET fin=inicio WHERE estado=N'Cerrada' AND fin<inicio;
GO

-- completa cajas abiertas de dias anteriores para poder exigir una sola apertura vigente en toda la aplicacion
-- Los movimientos permanecen en sus tablas originales y se consultan por id_caja.
UPDATE dbo.Caja SET estado=N'Cerrada',id_usuario=NULL,fecha_cierre=SYSDATETIME(),
    fin=DATEADD(day,1,CONVERT(datetime2,fecha))
WHERE estado=N'Abierta' AND fecha<CONVERT(date,SYSDATETIME());
GO

-- toda operacion queda asociada a una caja real incluso cuando hay varios cierres en la misma fecha
IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Caja') AND name=N'inicio' AND is_nullable=1)
    ALTER TABLE dbo.Caja ALTER COLUMN inicio datetime2 NOT NULL;
IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Pago') AND name=N'id_caja' AND is_nullable=1)
    ALTER TABLE dbo.Pago ALTER COLUMN id_caja int NOT NULL;
IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Caja_Movimiento') AND name=N'id_caja' AND is_nullable=1)
    ALTER TABLE dbo.Caja_Movimiento ALTER COLUMN id_caja int NOT NULL;
IF NOT EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_Pago_Caja')
    ALTER TABLE dbo.Pago ADD CONSTRAINT FK_Pago_Caja FOREIGN KEY(id_caja) REFERENCES dbo.Caja(id);
IF NOT EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_CajaMovimiento_Caja')
    ALTER TABLE dbo.Caja_Movimiento ADD CONSTRAINT FK_CajaMovimiento_Caja FOREIGN KEY(id_caja) REFERENCES dbo.Caja(id);
IF NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.Caja') AND name=N'CK_Caja_FinTurno')
    ALTER TABLE dbo.Caja ADD CONSTRAINT CK_Caja_FinTurno CHECK(
        (estado=N'Abierta' AND fin IS NULL) OR (estado=N'Cerrada' AND fin IS NOT NULL));

-- permite repetir fechas pero impide dos cajas abiertas al mismo tiempo incluso desde dos computadoras
IF EXISTS(SELECT 1 FROM sys.key_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.Caja') AND name=N'UQ_Caja_Fecha')
    ALTER TABLE dbo.Caja DROP CONSTRAINT UQ_Caja_Fecha;
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Caja') AND name=N'UX_Caja_UnicaAbierta')
    CREATE UNIQUE INDEX UX_Caja_UnicaAbierta ON dbo.Caja(estado) WHERE estado=N'Abierta';
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Pago') AND name=N'IX_Pago_Caja')
    CREATE INDEX IX_Pago_Caja ON dbo.Pago(id_caja,fecha_pago);
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Caja_Movimiento') AND name=N'IX_CajaMovimiento_Caja')
    CREATE INDEX IX_CajaMovimiento_Caja ON dbo.Caja_Movimiento(id_caja,fecha);
GO


-- ===== SimplificarEntregasYCaja.sql =====

-- Ejecutar sobre la base de Prana con la aplicación cerrada.
-- Elimina el horario de la preparación de entregas y los fondos iniciales.
-- Conserva el horario sugerido de Nueva venta, utilizado en el remito.
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    IF COL_LENGTH(N'dbo.Envio', N'horario_sugerido') IS NOT NULL
        ALTER TABLE dbo.Envio DROP COLUMN horario_sugerido;
    IF COL_LENGTH(N'dbo.Venta_Entrega', N'horario_sugerido') IS NULL
        ALTER TABLE dbo.Venta_Entrega ADD horario_sugerido nvarchar(50) NULL;

    -- NormalizarCajaSinJson.sql retira definitivamente el JSON y los fondos iniciales.
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;


-- ===== NumerarCierresDesde500.sql =====

-- Numeración visible independiente de los IDs internos. Ejecutar sobre la base de Prana.
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    DECLARE @bloqueo int;
    EXEC @bloqueo=sys.sp_getapplock @Resource=N'Prana.Caja.Activa', @LockMode='Exclusive', @LockOwner='Transaction', @LockTimeout=15000;
    IF @bloqueo<0 THROW 50003,N'La caja está ocupada. Reintente.',1;
    IF COL_LENGTH(N'dbo.Caja',N'numero_cierre') IS NULL
        ALTER TABLE dbo.Caja ADD numero_cierre int NULL;
    EXEC(N'
        DECLARE @ultimo int=(SELECT COALESCE(MAX(numero_cierre),499) FROM dbo.Caja);
        IF @ultimo<499 SET @ultimo=499;
        ;WITH pendientes AS (
            SELECT id, @ultimo+ROW_NUMBER() OVER(ORDER BY COALESCE(fin,fecha_cierre,inicio),id) numero
            FROM dbo.Caja WHERE estado=N''Cerrada'' AND numero_cierre IS NULL
        )
        UPDATE c SET numero_cierre=p.numero FROM dbo.Caja c JOIN pendientes p ON p.id=c.id;
        IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N''dbo.Caja'') AND name=N''UX_Caja_NumeroCierre'')
            CREATE UNIQUE INDEX UX_Caja_NumeroCierre ON dbo.Caja(numero_cierre) WHERE numero_cierre IS NOT NULL;
    ');
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;


-- ===== NormalizarCajaSinJson.sql =====

-- Ejecutar completo en la base de Prana, con la aplicación cerrada.
-- Extrae los datos exclusivos del JSON y elimina Caja.datos. No copia movimientos.
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    DECLARE @bloqueo int;
    EXEC @bloqueo=sys.sp_getapplock @Resource=N'Prana.Caja.Activa',@LockMode='Exclusive',@LockOwner='Transaction',@LockTimeout=15000;
    IF @bloqueo<0 THROW 50003,N'La caja está ocupada. Reintente.',1;
    IF OBJECT_ID(N'dbo.Caja',N'U') IS NULL THROW 50060,N'No existe Caja en la base seleccionada.',1;
    IF COL_LENGTH(N'dbo.Caja',N'observaciones') IS NULL
        ALTER TABLE dbo.Caja ADD observaciones nvarchar(1000) NOT NULL CONSTRAINT DF_Caja_Observaciones DEFAULT(N'');
    IF COL_LENGTH(N'dbo.Caja',N'datos') IS NOT NULL
    BEGIN
        EXEC(N'
            IF EXISTS(SELECT 1 FROM dbo.Caja WHERE ISJSON(datos)<>1 OR datos IS NULL)
                THROW 50061,N''Hay datos de caja inválidos. No se eliminó el JSON.'',1;
            IF EXISTS(SELECT 1 FROM dbo.Caja WHERE LEN(JSON_VALUE(datos,''$.Observaciones''))>1000)
                THROW 50062,N''Hay observaciones de más de 1000 caracteres. Revise antes de migrar.'',1;
            IF EXISTS(SELECT 1 FROM dbo.Caja WHERE NULLIF(observaciones,N'''') IS NOT NULL
                AND NULLIF(JSON_VALUE(datos,''$.Observaciones''),N'''') IS NOT NULL
                AND observaciones<>JSON_VALUE(datos,''$.Observaciones''))
                THROW 50063,N''Las observaciones de Caja difieren del JSON. Revise antes de migrar.'',1;

            -- Antes de quitar la copia, exige que las operaciones originales sigan existiendo.
            IF EXISTS(SELECT 1 FROM dbo.Caja c CROSS APPLY OPENJSON(c.datos,''$.Movimientos'')
                WITH(id int ''$.Id'',venta int ''$.Venta'',importe decimal(18,2) ''$.Importe'') m
                WHERE (m.venta IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.Pago p WHERE p.id_pago=m.id AND p.id_venta=m.venta AND p.id_caja=c.id))
                   OR (m.venta IS NULL AND NOT EXISTS(SELECT 1 FROM dbo.Caja_Movimiento cm WHERE cm.id=m.id AND cm.id_caja=c.id AND cm.anulado=0 AND cm.importe=m.importe)))
                THROW 50064,N''Hay operaciones del JSON sin original coincidente en Pago o Caja_Movimiento. No se eliminó ningún dato; revise esos cierres.'',1;

            UPDATE dbo.Caja SET observaciones=COALESCE(JSON_VALUE(datos,''$.Observaciones''),N'''')
                WHERE NULLIF(observaciones,N'''') IS NULL;

            -- El operador se conserva por su FK; el nombre se consulta en Usuario.
            UPDATE c SET id_usuario=u.id_usuario FROM dbo.Caja c JOIN dbo.Usuario u
                ON u.nombre_usuario=JSON_VALUE(c.datos,''$.Usuario'')
                WHERE c.estado=N''Cerrada'' AND c.id_usuario IS NULL
                  AND JSON_VALUE(c.datos,''$.Usuario'')<>N''CIERRE AUTOMÁTICO'';
            IF EXISTS(SELECT 1 FROM dbo.Caja c WHERE c.estado=N''Cerrada'' AND c.id_usuario IS NULL
                AND NULLIF(JSON_VALUE(c.datos,''$.Usuario''),N'''') IS NOT NULL
                AND JSON_VALUE(c.datos,''$.Usuario'')<>N''CIERRE AUTOMÁTICO'')
                THROW 50065,N''Hay responsables de cierre que no se pueden relacionar con Usuario. Revise antes de migrar.'',1;

        ');
        DECLARE @quitar nvarchar(max)=N'';
        SELECT @quitar=@quitar+N'ALTER TABLE dbo.Caja DROP CONSTRAINT '+QUOTENAME(cc.name)+N';'
        FROM sys.check_constraints cc WHERE cc.parent_object_id=OBJECT_ID(N'dbo.Caja') AND EXISTS(
            SELECT 1 FROM sys.sql_expression_dependencies d JOIN sys.columns c ON c.object_id=d.referenced_id AND c.column_id=d.referenced_minor_id
            WHERE d.referencing_id=cc.object_id AND c.object_id=cc.parent_object_id AND c.name=N'datos');
        SELECT @quitar=@quitar+N'ALTER TABLE dbo.Caja DROP CONSTRAINT '+QUOTENAME(dc.name)+N';'
        FROM sys.default_constraints dc JOIN sys.columns c ON c.object_id=dc.parent_object_id AND c.column_id=dc.parent_column_id
        WHERE dc.parent_object_id=OBJECT_ID(N'dbo.Caja') AND c.name=N'datos';
        IF @quitar<>N'' EXEC sys.sp_executesql @quitar;
        ALTER TABLE dbo.Caja DROP COLUMN datos;
    END;
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;


-- ===== MigrarDireccionLocalidad.sql =====

-- Ejecutar sobre la base de Prana. Conserva nombres existentes; elimina los CP.
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    IF COL_LENGTH('dbo.Cliente','localidad') IS NULL
        ALTER TABLE dbo.Cliente ADD localidad nvarchar(25) NULL;

    IF OBJECT_ID('dbo.Localidad','U') IS NOT NULL AND COL_LENGTH('dbo.Cliente','id_localidad') IS NOT NULL
    BEGIN
        EXEC(N'IF EXISTS (SELECT 1 FROM dbo.Cliente c JOIN dbo.Localidad l ON l.id_localidad=c.id_localidad WHERE LEN(LTRIM(RTRIM(l.nombre_localidad)))>25)
            THROW 50010, N''Hay localidades de más de 25 caracteres. Abrevie sus nombres antes de migrar; no se modificó la base.'', 1;
            UPDATE c SET localidad=UPPER(NULLIF(LTRIM(RTRIM(l.nombre_localidad)),N''''))
            FROM dbo.Cliente c JOIN dbo.Localidad l ON l.id_localidad=c.id_localidad;');
    END;

    IF OBJECT_ID('dbo.Venta_Entrega','U') IS NOT NULL AND COL_LENGTH('dbo.Venta_Entrega','localidad') IS NULL
    BEGIN
        ALTER TABLE dbo.Venta_Entrega ADD localidad nvarchar(25) NULL;
        -- Solo se puede inferir la localidad si coincide el domicilio registrado.
        EXEC(N'UPDATE ve SET localidad=c.localidad FROM dbo.Venta_Entrega ve
            JOIN dbo.Venta v ON v.id_venta=ve.id_venta JOIN dbo.Cliente c ON c.id_cliente=v.id_cliente
            WHERE UPPER(LTRIM(RTRIM(ve.domicilio)))=UPPER(LTRIM(RTRIM(c.direccion)));');
    END;

    IF OBJECT_ID('dbo.FK_Cliente_Localidad','F') IS NOT NULL
        ALTER TABLE dbo.Cliente DROP CONSTRAINT FK_Cliente_Localidad;
    IF COL_LENGTH('dbo.Cliente','id_localidad') IS NOT NULL
        ALTER TABLE dbo.Cliente DROP COLUMN id_localidad;
    IF COL_LENGTH('dbo.Cliente','codigo_postal') IS NOT NULL
        ALTER TABLE dbo.Cliente DROP COLUMN codigo_postal;
    IF OBJECT_ID('dbo.Localidad','U') IS NOT NULL
        DROP TABLE dbo.Localidad;

    EXEC(N'UPDATE dbo.Cliente SET direccion=UPPER(NULLIF(LTRIM(RTRIM(direccion)),N'''')), localidad=UPPER(NULLIF(LTRIM(RTRIM(localidad)),N''''));');
    IF OBJECT_ID('dbo.Venta_Entrega','U') IS NOT NULL
        EXEC(N'UPDATE dbo.Venta_Entrega SET domicilio=UPPER(NULLIF(LTRIM(RTRIM(domicilio)),N'''')), localidad=UPPER(NULLIF(LTRIM(RTRIM(localidad)),N''''));');
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;


-- ===== NormalizarEnviosYPagos.sql =====

-- Ejecutar completo en la base de Prana, con la aplicación cerrada.
-- Conserva el destino propio de Envio; los pagos generales pertenecen a Venta.
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    IF OBJECT_ID(N'dbo.Venta',N'U') IS NULL OR OBJECT_ID(N'dbo.Venta_Entrega',N'U') IS NULL OR OBJECT_ID(N'dbo.Envio',N'U') IS NULL
        THROW 50050,N'Faltan tablas de Prana en la base seleccionada.',1;

    IF COL_LENGTH(N'dbo.Venta',N'observacion_pago') IS NULL
        ALTER TABLE dbo.Venta ADD observacion_pago nvarchar(500) NULL;
    IF COL_LENGTH(N'dbo.Venta',N'condicion_pago') IS NULL
        ALTER TABLE dbo.Venta ADD condicion_pago nvarchar(30) NOT NULL CONSTRAINT DF_Venta_CondicionPago DEFAULT(N'');

    -- Copia los datos antes de retirar las columnas anteriores. Repetir el script no los sobrescribe.
    IF COL_LENGTH(N'dbo.Venta_Entrega',N'observacion_pago') IS NOT NULL
    BEGIN
        EXEC(N'IF EXISTS(SELECT 1 FROM dbo.Venta v JOIN dbo.Venta_Entrega ve ON ve.id_venta=v.id_venta
            WHERE NULLIF(v.observacion_pago,N'''') IS NOT NULL AND NULLIF(ve.observacion_pago,N'''') IS NOT NULL AND v.observacion_pago<>ve.observacion_pago)
            THROW 50051,N''Hay observaciones de pago distintas en Venta y Venta_Entrega. Revise antes de migrar.'',1;
            UPDATE v SET observacion_pago=ve.observacion_pago FROM dbo.Venta v JOIN dbo.Venta_Entrega ve ON ve.id_venta=v.id_venta
            WHERE NULLIF(v.observacion_pago,N'''') IS NULL;');
    END;
    IF COL_LENGTH(N'dbo.Venta_Entrega',N'condicion_pago') IS NOT NULL
    BEGIN
        EXEC(N'IF EXISTS(SELECT 1 FROM dbo.Venta v JOIN dbo.Venta_Entrega ve ON ve.id_venta=v.id_venta
            WHERE NULLIF(v.condicion_pago,N'''') IS NOT NULL AND NULLIF(ve.condicion_pago,N'''') IS NOT NULL AND v.condicion_pago<>ve.condicion_pago)
            THROW 50052,N''Hay condiciones de pago distintas en Venta y Venta_Entrega. Revise antes de migrar.'',1;
            UPDATE v SET condicion_pago=COALESCE(ve.condicion_pago,N'''') FROM dbo.Venta v JOIN dbo.Venta_Entrega ve ON ve.id_venta=v.id_venta
            WHERE NULLIF(v.condicion_pago,N'''') IS NULL;');
    END;

    -- Cada entrega puede tener otro destino: la dirección de Envio se conserva.
    IF COL_LENGTH(N'dbo.Envio',N'costo') IS NOT NULL
        EXEC(N'IF EXISTS(SELECT 1 FROM dbo.Envio e JOIN dbo.Remito r ON r.id_remito=e.id_remito
            LEFT JOIN dbo.Venta_Entrega ve ON ve.id_venta=r.id_venta
            WHERE e.costo<>0 AND (ve.id_venta IS NULL OR e.costo<>ve.costo_envio))
            THROW 50054,N''Hay costos de envío que difieren del costo de la venta. Revise antes de migrar. No se eliminaron datos.'',1;');
    IF COL_LENGTH(N'dbo.Envio',N'tipo_envio') IS NOT NULL
        EXEC(N'IF EXISTS(SELECT 1 FROM dbo.Envio e JOIN dbo.Remito r ON r.id_remito=e.id_remito
            JOIN dbo.Venta v ON v.id_venta=r.id_venta JOIN dbo.Modalidad_Entrega m ON m.id_modalidad_entrega=v.id_modalidad_entrega
            WHERE e.tipo_envio<>CASE WHEN m.nombre_modalidad_entrega LIKE N''Retiro%'' THEN N''Retiro en local'' ELSE N''Envío a domicilio'' END)
            THROW 50055,N''Hay modalidades de envío distintas de la venta. Revise antes de migrar.'',1;');

    IF OBJECT_ID(N'dbo.CK_Envio_TipoEnvio',N'C') IS NOT NULL
        ALTER TABLE dbo.Envio DROP CONSTRAINT CK_Envio_TipoEnvio;
    -- También admite versiones antiguas con un catálogo Tipo_Envio.
    IF COL_LENGTH(N'dbo.Envio',N'id_tipo_envio') IS NOT NULL
    BEGIN
        IF OBJECT_ID(N'dbo.Tipo_Envio',N'U') IS NOT NULL
            EXEC(N'IF EXISTS(SELECT 1 FROM dbo.Envio e JOIN dbo.Tipo_Envio t ON t.id_tipo_envio=e.id_tipo_envio
                JOIN dbo.Remito r ON r.id_remito=e.id_remito JOIN dbo.Venta v ON v.id_venta=r.id_venta
                JOIN dbo.Modalidad_Entrega m ON m.id_modalidad_entrega=v.id_modalidad_entrega
                WHERE t.nombre_tipo_envio<>CASE WHEN m.nombre_modalidad_entrega LIKE N''Retiro%'' THEN N''Retiro en local'' ELSE N''Envío a domicilio'' END)
                THROW 50055,N''Hay modalidades antiguas de envío distintas de la venta.'',1;');
        DECLARE @fk nvarchar(max)=N'';
        SELECT @fk=@fk+N'ALTER TABLE dbo.Envio DROP CONSTRAINT '+QUOTENAME(f.name)+N';'
        FROM sys.foreign_keys f JOIN sys.foreign_key_columns fc ON fc.constraint_object_id=f.object_id
        JOIN sys.columns c ON c.object_id=fc.parent_object_id AND c.column_id=fc.parent_column_id
        WHERE f.parent_object_id=OBJECT_ID(N'dbo.Envio') AND c.name=N'id_tipo_envio';
        IF @fk<>N'' EXEC sys.sp_executesql @fk;
        ALTER TABLE dbo.Envio DROP COLUMN id_tipo_envio;
    END;
    IF OBJECT_ID(N'dbo.Tipo_Envio',N'U') IS NOT NULL DROP TABLE dbo.Tipo_Envio;
    DECLARE @checks nvarchar(max)=N'';
    SELECT @checks=@checks+N'ALTER TABLE dbo.Envio DROP CONSTRAINT '+QUOTENAME(cc.name)+N';'
    FROM sys.check_constraints cc
    WHERE cc.parent_object_id=OBJECT_ID(N'dbo.Envio') AND EXISTS(
        SELECT 1 FROM sys.sql_expression_dependencies d JOIN sys.columns c
        ON c.object_id=d.referenced_id AND c.column_id=d.referenced_minor_id
        WHERE d.referencing_id=cc.object_id AND c.object_id=cc.parent_object_id
            AND c.name IN(N'costo',N'tipo_envio',N'horario_sugerido'));
    IF @checks<>N'' EXEC sys.sp_executesql @checks;
    -- Retira valores predeterminados vinculados a las columnas que se eliminan.
    DECLARE @quitar nvarchar(max)=N'';
    SELECT @quitar=@quitar+N'ALTER TABLE '+QUOTENAME(OBJECT_SCHEMA_NAME(dc.parent_object_id))+N'.'+QUOTENAME(OBJECT_NAME(dc.parent_object_id))+
        N' DROP CONSTRAINT '+QUOTENAME(dc.name)+N';'
    FROM sys.default_constraints dc JOIN sys.columns c ON c.object_id=dc.parent_object_id AND c.column_id=dc.parent_column_id
    WHERE (dc.parent_object_id=OBJECT_ID(N'dbo.Envio') AND c.name IN(N'costo',N'tipo_envio',N'horario_sugerido'))
       OR (dc.parent_object_id=OBJECT_ID(N'dbo.Venta_Entrega') AND c.name IN(N'observacion_pago',N'condicion_pago'));
    IF @quitar<>N'' EXEC sys.sp_executesql @quitar;
    IF COL_LENGTH(N'dbo.Envio',N'costo') IS NOT NULL ALTER TABLE dbo.Envio DROP COLUMN costo;
    IF COL_LENGTH(N'dbo.Envio',N'tipo_envio') IS NOT NULL ALTER TABLE dbo.Envio DROP COLUMN tipo_envio;
    IF COL_LENGTH(N'dbo.Envio',N'horario_sugerido') IS NOT NULL ALTER TABLE dbo.Envio DROP COLUMN horario_sugerido;
    IF COL_LENGTH(N'dbo.Venta_Entrega',N'observacion_pago') IS NOT NULL ALTER TABLE dbo.Venta_Entrega DROP COLUMN observacion_pago;
    IF COL_LENGTH(N'dbo.Venta_Entrega',N'condicion_pago') IS NOT NULL ALTER TABLE dbo.Venta_Entrega DROP COLUMN condicion_pago;
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;


-- ===== HabilitarDestinosPorEntrega.sql =====

-- Ejecutar completo en la base de Prana con la aplicación cerrada.
-- Cada entrega conserva su propio destino. No cambia el destino de la venta ni el remito impreso.
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    IF OBJECT_ID(N'dbo.Envio',N'U') IS NULL OR OBJECT_ID(N'dbo.Venta_Entrega',N'U') IS NULL
        THROW 50080,N'Faltan tablas de Prana en la base seleccionada.',1;
    IF COL_LENGTH(N'dbo.Envio',N'direccion') IS NULL
        ALTER TABLE dbo.Envio ADD direccion nvarchar(200) NULL;
    IF COL_LENGTH(N'dbo.Envio',N'localidad') IS NULL
        ALTER TABLE dbo.Envio ADD localidad nvarchar(25) NULL;
    EXEC(N'
        -- Solo inicializa entregas sin destino; no reemplaza una dirección existente distinta.
        UPDATE e SET direccion=CASE WHEN m.nombre_modalidad_entrega LIKE N''Retiro%'' THEN N''Retiro en local'' ELSE ve.domicilio END,
            localidad=CASE WHEN m.nombre_modalidad_entrega LIKE N''Retiro%'' THEN NULL ELSE ve.localidad END
        FROM dbo.Envio e JOIN dbo.Remito r ON r.id_remito=e.id_remito JOIN dbo.Venta v ON v.id_venta=r.id_venta
        JOIN dbo.Modalidad_Entrega m ON m.id_modalidad_entrega=v.id_modalidad_entrega
        LEFT JOIN dbo.Venta_Entrega ve ON ve.id_venta=v.id_venta
        WHERE NULLIF(LTRIM(RTRIM(e.direccion)),N'''') IS NULL;

        -- Separa calle y localidad únicamente cuando coinciden inequívocamente con la sugerencia.
        -- Si el envío tenía otro destino, conserva su texto y deja la localidad desconocida sin inventarla.
        UPDATE e SET direccion=ve.domicilio,localidad=ve.localidad
        FROM dbo.Envio e JOIN dbo.Remito r ON r.id_remito=e.id_remito
        JOIN dbo.Venta_Entrega ve ON ve.id_venta=r.id_venta JOIN dbo.Venta v ON v.id_venta=r.id_venta
        JOIN dbo.Modalidad_Entrega m ON m.id_modalidad_entrega=v.id_modalidad_entrega
        WHERE m.nombre_modalidad_entrega NOT LIKE N''Retiro%''
          AND NULLIF(LTRIM(RTRIM(e.localidad)),N'''') IS NULL AND NULLIF(ve.localidad,N'''') IS NOT NULL
          AND (UPPER(LTRIM(RTRIM(e.direccion)))=UPPER(LTRIM(RTRIM(ve.domicilio)))
            OR UPPER(LTRIM(RTRIM(e.direccion)))=UPPER(LTRIM(RTRIM(CONCAT(ve.domicilio,N'', '',ve.localidad)))));
    ');
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;


-- ===== AclararNombresCaja.sql =====

-- Ejecutar completo en la base de Prana con la aplicacion cerrada.
-- Renombra columnas sin modificar valores ni eliminar relaciones.
-- fecha_operativa: dia al que pertenece el turno.
-- fecha_hora_fin_turno: limite efectivo del turno (en cierres automaticos puede ser medianoche).
-- fecha_hora_registro_cierre: momento en que el sistema registro el cierre.
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    DECLARE @bloqueo int;
    EXEC @bloqueo=sys.sp_getapplock @Resource=N'Prana.Caja.Activa',@LockMode='Exclusive',@LockOwner='Transaction',@LockTimeout=15000;
    IF @bloqueo<0 THROW 50003,N'La caja esta ocupada. Reintente.',1;
    IF OBJECT_ID(N'dbo.Caja',N'U') IS NULL OR OBJECT_ID(N'dbo.Caja_Movimiento',N'U') IS NULL
        THROW 50090,N'Faltan las tablas de caja en la base seleccionada.',1;
    -- SQL Server impide renombrar columnas usadas por CHECK; conserva y recompone sus reglas.
    DECLARE @reglas TABLE(tabla sysname,nombre sysname,definicion nvarchar(max),deshabilitada bit,no_confiable bit);
    INSERT @reglas
        SELECT OBJECT_NAME(parent_object_id),name,definition,is_disabled,is_not_trusted
        FROM sys.check_constraints WHERE parent_object_id IN(OBJECT_ID(N'dbo.Caja'),OBJECT_ID(N'dbo.Caja_Movimiento'));
    DECLARE @tabla sysname,@nombre sysname,@definicion nvarchar(max),@deshabilitada bit,@no_confiable bit,@sql nvarchar(max);
    DECLARE reglas_quitar CURSOR LOCAL FAST_FORWARD FOR SELECT tabla,nombre FROM @reglas;
    OPEN reglas_quitar;
    FETCH NEXT FROM reglas_quitar INTO @tabla,@nombre;
    WHILE @@FETCH_STATUS=0
    BEGIN
        SET @sql=N'ALTER TABLE dbo.'+QUOTENAME(@tabla)+N' DROP CONSTRAINT '+QUOTENAME(@nombre);
        EXEC sys.sp_executesql @sql;
        FETCH NEXT FROM reglas_quitar INTO @tabla,@nombre;
    END;
    CLOSE reglas_quitar;
    DEALLOCATE reglas_quitar;
    IF COL_LENGTH(N'dbo.Caja',N'id') IS NOT NULL
    BEGIN
        IF COL_LENGTH(N'dbo.Caja',N'id_caja') IS NOT NULL
            THROW 50091,N'Existen ambas columnas: Caja.id y id_caja. No se modificaron los datos.',1;
        EXEC sys.sp_rename N'dbo.Caja.id',N'id_caja',N'COLUMN';
    END;
    IF COL_LENGTH(N'dbo.Caja',N'id_caja') IS NULL
        THROW 50092,N'Falta la columna Caja.id_caja. Actualice primero el esquema anterior.',1;
    IF COL_LENGTH(N'dbo.Caja',N'fecha') IS NOT NULL
    BEGIN
        IF COL_LENGTH(N'dbo.Caja',N'fecha_operativa') IS NOT NULL
            THROW 50091,N'Existen ambas columnas: Caja.fecha y fecha_operativa. No se modificaron los datos.',1;
        EXEC sys.sp_rename N'dbo.Caja.fecha',N'fecha_operativa',N'COLUMN';
    END;
    IF COL_LENGTH(N'dbo.Caja',N'fecha_operativa') IS NULL
        THROW 50092,N'Falta la columna Caja.fecha_operativa. Actualice primero el esquema anterior.',1;
    IF COL_LENGTH(N'dbo.Caja',N'inicio') IS NOT NULL
    BEGIN
        IF COL_LENGTH(N'dbo.Caja',N'fecha_hora_apertura') IS NOT NULL
            THROW 50091,N'Existen ambas columnas: Caja.inicio y fecha_hora_apertura. No se modificaron los datos.',1;
        EXEC sys.sp_rename N'dbo.Caja.inicio',N'fecha_hora_apertura',N'COLUMN';
    END;
    IF COL_LENGTH(N'dbo.Caja',N'fecha_hora_apertura') IS NULL
        THROW 50092,N'Falta la columna Caja.fecha_hora_apertura. Actualice primero el esquema anterior.',1;
    IF COL_LENGTH(N'dbo.Caja',N'fin') IS NOT NULL
    BEGIN
        IF COL_LENGTH(N'dbo.Caja',N'fecha_hora_fin_turno') IS NOT NULL
            THROW 50091,N'Existen ambas columnas: Caja.fin y fecha_hora_fin_turno. No se modificaron los datos.',1;
        EXEC sys.sp_rename N'dbo.Caja.fin',N'fecha_hora_fin_turno',N'COLUMN';
    END;
    IF COL_LENGTH(N'dbo.Caja',N'fecha_hora_fin_turno') IS NULL
        THROW 50092,N'Falta la columna Caja.fecha_hora_fin_turno. Actualice primero el esquema anterior.',1;
    IF COL_LENGTH(N'dbo.Caja',N'fecha_cierre') IS NOT NULL
    BEGIN
        IF COL_LENGTH(N'dbo.Caja',N'fecha_hora_registro_cierre') IS NOT NULL
            THROW 50091,N'Existen ambas columnas: Caja.fecha_cierre y fecha_hora_registro_cierre. No se modificaron los datos.',1;
        EXEC sys.sp_rename N'dbo.Caja.fecha_cierre',N'fecha_hora_registro_cierre',N'COLUMN';
    END;
    IF COL_LENGTH(N'dbo.Caja',N'fecha_hora_registro_cierre') IS NULL
        THROW 50092,N'Falta la columna Caja.fecha_hora_registro_cierre. Actualice primero el esquema anterior.',1;
    IF COL_LENGTH(N'dbo.Caja',N'id_usuario') IS NOT NULL
    BEGIN
        IF COL_LENGTH(N'dbo.Caja',N'id_usuario_cierre') IS NOT NULL
            THROW 50091,N'Existen ambas columnas: Caja.id_usuario y id_usuario_cierre. No se modificaron los datos.',1;
        EXEC sys.sp_rename N'dbo.Caja.id_usuario',N'id_usuario_cierre',N'COLUMN';
    END;
    IF COL_LENGTH(N'dbo.Caja',N'id_usuario_cierre') IS NULL
        THROW 50092,N'Falta la columna Caja.id_usuario_cierre. Actualice primero el esquema anterior.',1;
    IF COL_LENGTH(N'dbo.Caja_Movimiento',N'id') IS NOT NULL
    BEGIN
        IF COL_LENGTH(N'dbo.Caja_Movimiento',N'id_movimiento_caja') IS NOT NULL
            THROW 50091,N'Existen ambas columnas: Caja_Movimiento.id y id_movimiento_caja. No se modificaron los datos.',1;
        EXEC sys.sp_rename N'dbo.Caja_Movimiento.id',N'id_movimiento_caja',N'COLUMN';
    END;
    IF COL_LENGTH(N'dbo.Caja_Movimiento',N'id_movimiento_caja') IS NULL
        THROW 50092,N'Falta la columna Caja_Movimiento.id_movimiento_caja. Actualice primero el esquema anterior.',1;
    IF COL_LENGTH(N'dbo.Caja_Movimiento',N'fecha') IS NOT NULL
    BEGIN
        IF COL_LENGTH(N'dbo.Caja_Movimiento',N'fecha_hora_movimiento') IS NOT NULL
            THROW 50091,N'Existen ambas columnas: Caja_Movimiento.fecha y fecha_hora_movimiento. No se modificaron los datos.',1;
        EXEC sys.sp_rename N'dbo.Caja_Movimiento.fecha',N'fecha_hora_movimiento',N'COLUMN';
    END;
    IF COL_LENGTH(N'dbo.Caja_Movimiento',N'fecha_hora_movimiento') IS NULL
        THROW 50092,N'Falta la columna Caja_Movimiento.fecha_hora_movimiento. Actualice primero el esquema anterior.',1;
    UPDATE @reglas SET definicion=REPLACE(definicion,N'[id]',N'[id_caja]') WHERE tabla=N'Caja';
    UPDATE @reglas SET definicion=REPLACE(definicion,N'[fecha]',N'[fecha_operativa]') WHERE tabla=N'Caja';
    UPDATE @reglas SET definicion=REPLACE(definicion,N'[inicio]',N'[fecha_hora_apertura]') WHERE tabla=N'Caja';
    UPDATE @reglas SET definicion=REPLACE(definicion,N'[fin]',N'[fecha_hora_fin_turno]') WHERE tabla=N'Caja';
    UPDATE @reglas SET definicion=REPLACE(definicion,N'[fecha_cierre]',N'[fecha_hora_registro_cierre]') WHERE tabla=N'Caja';
    UPDATE @reglas SET definicion=REPLACE(definicion,N'[id_usuario]',N'[id_usuario_cierre]') WHERE tabla=N'Caja';
    UPDATE @reglas SET definicion=REPLACE(definicion,N'[id]',N'[id_movimiento_caja]') WHERE tabla=N'Caja_Movimiento';
    UPDATE @reglas SET definicion=REPLACE(definicion,N'[fecha]',N'[fecha_hora_movimiento]') WHERE tabla=N'Caja_Movimiento';
    DECLARE reglas_crear CURSOR LOCAL FAST_FORWARD FOR SELECT tabla,nombre,definicion,deshabilitada,no_confiable FROM @reglas;
    OPEN reglas_crear;
    FETCH NEXT FROM reglas_crear INTO @tabla,@nombre,@definicion,@deshabilitada,@no_confiable;
    WHILE @@FETCH_STATUS=0
    BEGIN
        SET @sql=N'ALTER TABLE dbo.'+QUOTENAME(@tabla)+CASE WHEN @no_confiable=1 THEN N' WITH NOCHECK' ELSE N' WITH CHECK' END
            +N' ADD CONSTRAINT '+QUOTENAME(@nombre)+N' CHECK '+@definicion;
        EXEC sys.sp_executesql @sql;
        IF @deshabilitada=1
        BEGIN
            SET @sql=N'ALTER TABLE dbo.'+QUOTENAME(@tabla)+N' NOCHECK CONSTRAINT '+QUOTENAME(@nombre);
            EXEC sys.sp_executesql @sql;
        END;
        FETCH NEXT FROM reglas_crear INTO @tabla,@nombre,@definicion,@deshabilitada,@no_confiable;
    END;
    CLOSE reglas_crear;
    DEALLOCATE reglas_crear;
    COMMIT TRANSACTION;
    PRINT N'Nombres de columnas de caja actualizados.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;


-- crea la tabla que administra las localidades del sistema
IF OBJECT_ID(N'dbo.Localidad',N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Localidad
    (
        -- identifica cada localidad con una clave estable
        id_localidad int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Localidad PRIMARY KEY,
        -- guarda el nombre normalizado y evita duplicados
        nombre_localidad nvarchar(25) NOT NULL CONSTRAINT UQ_Localidad_Nombre UNIQUE,
        -- permite ocultar localidades sin eliminar sus registros
        activo_localidad bit NOT NULL CONSTRAINT DF_Localidad_Activo DEFAULT ((1))
    );
END;
GO

-- prepara la lista temporal que reunira localidades existentes
DECLARE @localidades TABLE(nombre nvarchar(max) NOT NULL);
-- carga las localidades registradas antes de crear el combo
INSERT INTO @localidades(nombre)
SELECT localidad FROM dbo.Cliente WHERE NULLIF(LTRIM(RTRIM(localidad)),N'') IS NOT NULL
UNION ALL
SELECT localidad FROM dbo.Venta_Entrega WHERE NULLIF(LTRIM(RTRIM(localidad)),N'') IS NOT NULL
UNION ALL
SELECT localidad FROM dbo.Envio WHERE NULLIF(LTRIM(RTRIM(localidad)),N'') IS NOT NULL;
-- rechaza localidades demasiado largas para evitar truncamientos
IF EXISTS (SELECT 1 FROM @localidades WHERE LEN(LTRIM(RTRIM(nombre)))>25)
    THROW 50101,N'Hay localidades de más de 25 caracteres; abrevie esos valores antes de instalar.',1;
-- agrega cada localidad normalizada una sola vez
INSERT INTO dbo.Localidad(nombre_localidad)
SELECT DISTINCT UPPER(LTRIM(RTRIM(nombre)))
FROM @localidades l
WHERE NOT EXISTS (SELECT 1 FROM dbo.Localidad o WHERE o.nombre_localidad=UPPER(LTRIM(RTRIM(l.nombre))));
-- prepara la columna que relaciona clientes con su localidad
IF COL_LENGTH(N'dbo.Cliente',N'id_localidad') IS NULL
    ALTER TABLE dbo.Cliente ADD id_localidad int NULL;
GO
-- transfiere los nombres de cliente a las claves nuevas sin perder datos
IF COL_LENGTH(N'dbo.Cliente',N'localidad') IS NOT NULL
    EXEC sys.sp_executesql N'
        UPDATE c SET id_localidad=l.id_localidad
        FROM dbo.Cliente c JOIN dbo.Localidad l
          ON l.nombre_localidad=UPPER(LTRIM(RTRIM(c.localidad)))
        WHERE NULLIF(LTRIM(RTRIM(c.localidad)),N'''') IS NOT NULL;
        IF EXISTS (SELECT 1 FROM dbo.Cliente WHERE NULLIF(LTRIM(RTRIM(localidad)),N'''') IS NOT NULL AND id_localidad IS NULL)
            THROW 50102,N''Hay clientes cuya localidad no pudo asociarse al nuevo identificador.'',1;';
-- conecta los clientes con la tabla localidad mediante una clave foranea
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_Cliente_Localidad')
    ALTER TABLE dbo.Cliente ADD CONSTRAINT FK_Cliente_Localidad FOREIGN KEY(id_localidad) REFERENCES dbo.Localidad(id_localidad);
-- elimina la columna antigua despues de copiar y validar los nombres
IF COL_LENGTH(N'dbo.Cliente',N'localidad') IS NOT NULL
    EXEC sys.sp_executesql N'ALTER TABLE dbo.Cliente DROP COLUMN localidad;';
GO

-- ===== AclararNombresBaseDatos.sql =====

-- Ejecutar completo en la base seleccionada, con Prana cerrado y una copia de seguridad disponible.
-- Renombra las tablas y columnas aprobadas sin modificar sus datos.
-- Requiere el esquema funcional previo; admite los nombres anteriores y los actuales.
IF OBJECT_ID(N'dbo.Log_Actividad',N'U') IS NOT NULL
    AND OBJECT_ID(N'dbo.Venta_Datos_Entrega',N'U') IS NOT NULL
    AND COL_LENGTH(N'dbo.Detalle_Venta',N'entregado') IS NOT NULL
    AND NOT EXISTS (
        SELECT 1
        FROM (VALUES
            (N'Cliente',N'numero_documento'),(N'Cliente',N'fecha_hora_registro'),
            (N'Log_Actividad',N'fecha_hora_actividad'),
            (N'Pago',N'importe_base'),(N'Pago',N'importe_cobrado'),(N'Pago',N'cantidad_cuotas'),
            (N'Pago',N'clasificacion_cobro'),(N'Pago',N'fecha_hora_pago'),
            (N'Medio_Pago',N'clasificacion_cobro'),(N'Medio_Pago',N'porcentaje_recargo_base'),
            (N'Medio_Pago',N'porcentaje_recargo_2_cuotas'),(N'Medio_Pago',N'porcentaje_recargo_3_cuotas'),
            (N'Medio_Pago',N'porcentaje_recargo_6_cuotas'),(N'Medio_Pago',N'porcentaje_recargo_9_cuotas'),
            (N'Medio_Pago',N'porcentaje_recargo_12_cuotas'),(N'Variante_Producto',N'precio_venta_unitario'),
            (N'Variante_Producto',N'costo_unitario'),(N'Ingreso_Stock',N'numero_remito_proveedor'),
            (N'Ingreso_Stock',N'fecha_hora_ingreso'),(N'Venta_Datos_Entrega',N'fecha_estimada_entrega'),
            (N'Venta_Datos_Entrega',N'direccion_sugerida'),(N'Venta_Datos_Entrega',N'localidad_sugerida'),
            (N'Venta_Datos_Entrega',N'observaciones_entrega'),(N'Caja_Movimiento',N'tipo_movimiento'),
            (N'Caja_Movimiento',N'medio_movimiento'),(N'Venta',N'fecha_hora_venta'),
            (N'Remito',N'fecha_hora_emision'),
            (N'Envio',N'fecha_hora_entrega'),(N'Usuario',N'fecha_hora_fin_bloqueo'),
            (N'Detalle_Venta',N'entregado')
        ) esperadas(tabla,columna)
        WHERE COL_LENGTH(N'dbo.'+QUOTENAME(esperadas.tabla),esperadas.columna) IS NULL
    )
BEGIN
    PRINT N'Los nombres aprobados ya están aplicados.';
    RETURN;
END;
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    DECLARE @bloqueo int;
    EXEC @bloqueo=sys.sp_getapplock @Resource=N'Prana.ActualizarEsquema',@LockMode='Exclusive',@LockOwner='Transaction',@LockTimeout=15000;
    IF @bloqueo<0 THROW 50093,N'La base se esta actualizando. Reintente.',1;
    IF OBJECT_ID(N'dbo.Venta_Entrega',N'U') IS NOT NULL
    BEGIN
        IF OBJECT_ID(N'dbo.Venta_Datos_Entrega',N'U') IS NOT NULL THROW 50094,N'Existen ambas tablas Venta_Entrega y Venta_Datos_Entrega. Revise antes de continuar.',1;
        EXEC sys.sp_rename N'dbo.Venta_Entrega',N'Venta_Datos_Entrega';
    END;
    IF OBJECT_ID(N'dbo.LogActividad',N'U') IS NOT NULL
    BEGIN
        IF OBJECT_ID(N'dbo.Log_Actividad',N'U') IS NOT NULL THROW 50094,N'Existen ambas tablas LogActividad y Log_Actividad. Revise antes de continuar.',1;
        EXEC sys.sp_rename N'dbo.LogActividad',N'Log_Actividad';
    END;
    DECLARE @columnas TABLE(tabla sysname,anterior sysname,nuevo sysname);
    INSERT @columnas VALUES
        (N'Cliente',N'dni',N'numero_documento'),
        (N'Cliente',N'fecha_registro',N'fecha_hora_registro'),
        (N'Log_Actividad',N'fecha',N'fecha_hora_actividad'),
        (N'Pago',N'monto',N'importe_base'),
        (N'Pago',N'monto_final',N'importe_cobrado'),
        (N'Pago',N'cuotas',N'cantidad_cuotas'),
        (N'Pago',N'tipo_caja',N'clasificacion_cobro'),
        (N'Pago',N'fecha_pago',N'fecha_hora_pago'),
        (N'Medio_Pago',N'tipo_caja',N'clasificacion_cobro'),
        (N'Medio_Pago',N'recargo_medio_pago',N'porcentaje_recargo_base'),
        (N'Medio_Pago',N'recargo_2',N'porcentaje_recargo_2_cuotas'),
        (N'Medio_Pago',N'recargo_3',N'porcentaje_recargo_3_cuotas'),
        (N'Medio_Pago',N'recargo_6',N'porcentaje_recargo_6_cuotas'),
        (N'Medio_Pago',N'recargo_9',N'porcentaje_recargo_9_cuotas'),
        (N'Medio_Pago',N'recargo_12',N'porcentaje_recargo_12_cuotas'),
        (N'Variante_Producto',N'precio',N'precio_venta_unitario'),
        (N'Variante_Producto',N'costo',N'costo_unitario'),
        (N'Ingreso_Stock',N'remito_proveedor',N'numero_remito_proveedor'),
        (N'Ingreso_Stock',N'fecha_ingreso',N'fecha_hora_ingreso'),
        (N'Venta_Datos_Entrega',N'fecha_estimada',N'fecha_estimada_entrega'),
        (N'Venta_Datos_Entrega',N'domicilio',N'direccion_sugerida'),
        (N'Venta_Datos_Entrega',N'localidad',N'localidad_sugerida'),
        (N'Venta_Datos_Entrega',N'observaciones',N'observaciones_entrega'),
        (N'Caja_Movimiento',N'tipo',N'tipo_movimiento'),
        (N'Caja_Movimiento',N'medio',N'medio_movimiento'),
        (N'Venta',N'fecha_venta',N'fecha_hora_venta'),
        (N'Remito',N'fecha_emision',N'fecha_hora_emision'),
        (N'Envio',N'fecha_entrega',N'fecha_hora_entrega'),
        (N'Usuario',N'bloqueado_hasta',N'fecha_hora_fin_bloqueo');
    DECLARE @reglas TABLE(tabla sysname,nombre sysname,definicion nvarchar(max),deshabilitada bit,no_confiable bit);
    INSERT @reglas SELECT OBJECT_NAME(parent_object_id),name,definition,is_disabled,is_not_trusted
        FROM sys.check_constraints WHERE OBJECT_NAME(parent_object_id) IN(SELECT tabla FROM @columnas);
    DECLARE @tabla sysname,@nombre sysname,@definicion nvarchar(max),@deshabilitada bit,@no_confiable bit,@sql nvarchar(max),@anterior sysname,@nuevo sysname,@objeto nvarchar(776);
    DECLARE quitar CURSOR LOCAL FAST_FORWARD FOR SELECT tabla,nombre FROM @reglas;
    OPEN quitar;
    FETCH NEXT FROM quitar INTO @tabla,@nombre;
    WHILE @@FETCH_STATUS=0
    BEGIN
        SET @sql=N'ALTER TABLE dbo.'+QUOTENAME(@tabla)+N' DROP CONSTRAINT '+QUOTENAME(@nombre);
        EXEC sys.sp_executesql @sql;
        FETCH NEXT FROM quitar INTO @tabla,@nombre;
    END;
    CLOSE quitar;
    DEALLOCATE quitar;
    DECLARE renombrar CURSOR LOCAL FAST_FORWARD FOR SELECT tabla,anterior,nuevo FROM @columnas;
    OPEN renombrar;
    FETCH NEXT FROM renombrar INTO @tabla,@anterior,@nuevo;
    WHILE @@FETCH_STATUS=0
    BEGIN
        SET @objeto=N'dbo.'+QUOTENAME(@tabla);
        IF COL_LENGTH(@objeto,@anterior) IS NOT NULL
        BEGIN
            IF COL_LENGTH(@objeto,@nuevo) IS NOT NULL THROW 50095,N'Existen columnas anteriores y nuevas simultaneamente. Se cancelo la actualizacion.',1;
            SET @objeto=@objeto+N'.'+QUOTENAME(@anterior);
            EXEC sys.sp_rename @objeto,@nuevo,N'COLUMN';
        END;
        SET @objeto=N'dbo.'+QUOTENAME(@tabla);
        IF COL_LENGTH(@objeto,@nuevo) IS NULL THROW 50096,N'Falta una columna esperada. Se cancelo la actualizacion.',1;
        UPDATE @reglas SET definicion=REPLACE(definicion,QUOTENAME(@anterior),QUOTENAME(@nuevo)) WHERE tabla=@tabla;
        FETCH NEXT FROM renombrar INTO @tabla,@anterior,@nuevo;
    END;
    CLOSE renombrar;
    DEALLOCATE renombrar;
    DECLARE reconstruir CURSOR LOCAL FAST_FORWARD FOR SELECT tabla,nombre,definicion,deshabilitada,no_confiable FROM @reglas;
    OPEN reconstruir;
    FETCH NEXT FROM reconstruir INTO @tabla,@nombre,@definicion,@deshabilitada,@no_confiable;
    WHILE @@FETCH_STATUS=0
    BEGIN
        SET @sql=N'ALTER TABLE dbo.'+QUOTENAME(@tabla)+CASE WHEN @no_confiable=1 THEN N' WITH NOCHECK' ELSE N' WITH CHECK' END
            +N' ADD CONSTRAINT '+QUOTENAME(@nombre)+N' CHECK '+@definicion;
        EXEC sys.sp_executesql @sql;
        IF @deshabilitada=1
        BEGIN
            SET @sql=N'ALTER TABLE dbo.'+QUOTENAME(@tabla)+N' NOCHECK CONSTRAINT '+QUOTENAME(@nombre);
            EXEC sys.sp_executesql @sql;
        END;
        FETCH NEXT FROM reconstruir INTO @tabla,@nombre,@definicion,@deshabilitada,@no_confiable;
    END;
    CLOSE reconstruir;
    DEALLOCATE reconstruir;
    COMMIT TRANSACTION;
    PRINT N'Nombres de tablas y columnas actualizados sin modificar los datos.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;


