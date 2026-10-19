/*
    SCRIPT DE CREACIÓN DE LA ESTRUCTURA LOCAL DE PRANA ELECTRÓNICA

    Este archivo se ejecuta solamente sobre una base sin tablas de la aplicación.
    Crea las tablas, las claves, las relaciones y las reglas de validación.
    Al final incorpora el rol Administrador y el usuario admin inicial.
    No contiene clientes, productos, ventas ni otros datos de trabajo.

    La instrucción GO separa el script en lotes que SQL Server ejecuta en orden.
*/

-- Se utiliza la base de la conexión actual. No cambia el destino con USE.

/* =========================================================
   TABLA: Categoria
   Guarda las categorías utilizadas para clasificar los productos.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Categoria](
-- Define id_categoria como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_categoria] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre_categoria como texto de hasta 100 caracteres, obligatorio.
	[nombre_categoria] [nvarchar](100) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_categoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Cliente
   Guarda los datos personales, fiscales y de contacto de los clientes.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Cliente](
-- Define id_cliente como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_cliente] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre_cliente como texto de hasta 100 caracteres, obligatorio.
	[nombre_cliente] [nvarchar](100) NOT NULL,
-- Define apellido_cliente como texto de hasta 100 caracteres, obligatorio.
	[apellido_cliente] [nvarchar](100) NOT NULL,
-- Define dni como texto de hasta 11 caracteres, obligatorio.
	[dni] [nvarchar](11) NOT NULL,
-- Define telefono como texto de hasta 20 caracteres, obligatorio.
	[telefono] [nvarchar](20) NOT NULL,
-- Define email como texto de hasta 150 caracteres, opcional.
	[email] [nvarchar](150) NULL,
-- Define direccion como texto de hasta 200 caracteres, opcional.
	[direccion] [nvarchar](200) NULL,
-- Define fecha_registro como fecha y hora, obligatorio.
	[fecha_registro] [datetime2](7) NOT NULL,
-- Define saldo_a_favor como número decimal, opcional.
	[saldo_a_favor] [decimal](10, 2) NULL,
-- Define id_localidad como número entero, opcional.
	[id_localidad] [int] NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_cliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
-- Impide que se repita el valor indicado en diferentes registros.
UNIQUE NONCLUSTERED
(
	[dni] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Detalle_Ingreso
   Guarda cada variante y cantidad incluida en un ingreso de stock.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Detalle_Ingreso](
-- Define id_detalle_ingreso como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_detalle_ingreso] [int] IDENTITY(1,1) NOT NULL,
-- Define id_ingreso como número entero, obligatorio.
	[id_ingreso] [int] NOT NULL,
-- Define id_variante como número entero, obligatorio.
	[id_variante] [int] NOT NULL,
-- Define cantidad como número entero, obligatorio.
	[cantidad] [int] NOT NULL,
-- Define costo_unitario como número decimal, obligatorio.
	[costo_unitario] [decimal](10, 2) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_detalle_ingreso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Detalle_Remito
   Guarda los productos y cantidades incluidos en cada remito.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Detalle_Remito](
-- Define id_detalle_remito como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_detalle_remito] [int] IDENTITY(1,1) NOT NULL,
-- Define id_remito como número entero, obligatorio.
	[id_remito] [int] NOT NULL,
-- Define id_variante como número entero, obligatorio.
	[id_variante] [int] NOT NULL,
-- Define cantidad como número entero, obligatorio.
	[cantidad] [int] NOT NULL,
-- Define descripcion como texto de hasta 300 caracteres, obligatorio.
	[descripcion] [nvarchar](300) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_detalle_remito] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Detalle_Venta
   Guarda los productos, cantidades y precios que componen una venta.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Detalle_Venta](
-- Define id_detalle_venta como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_detalle_venta] [int] IDENTITY(1,1) NOT NULL,
-- Define id_venta como número entero, obligatorio.
	[id_venta] [int] NOT NULL,
-- Define id_variante como número entero, obligatorio.
	[id_variante] [int] NOT NULL,
-- Define cantidad como número entero, obligatorio.
	[cantidad] [int] NOT NULL,
-- Define precio_unitario como número decimal, obligatorio.
	[precio_unitario] [decimal](10, 2) NOT NULL,
-- Define subtotal como número decimal, obligatorio.
	[subtotal] [decimal](10, 2) NOT NULL,
-- Define id_estado_variante_producto como número entero, obligatorio.
	[id_estado_variante_producto] [int] NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_detalle_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Envio
   Registra los datos necesarios para realizar y controlar un envío.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Envio](
-- Define id_envio como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_envio] [int] IDENTITY(1,1) NOT NULL,
-- Define id_remito como número entero, obligatorio.
	[id_remito] [int] NOT NULL,
-- Define direccion como texto de hasta 200 caracteres, obligatorio.
	[direccion] [nvarchar](200) NOT NULL,
-- Define horario_sugerido como texto de hasta 50 caracteres, opcional.
	[horario_sugerido] [nvarchar](50) NULL,
-- Define el tipo operativo del envío con uno de los dos valores permitidos.
	[tipo_envio] [nvarchar](30) NOT NULL,
-- Define costo como número decimal, obligatorio.
	[costo] [decimal](10, 2) NOT NULL,
-- Define id_estado_envio como número entero, obligatorio.
	[id_estado_envio] [int] NOT NULL,
-- Define fecha_entrega como fecha y hora, opcional.
	[fecha_entrega] [datetime2](7) NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_envio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
CONSTRAINT [CK_Envio_TipoEnvio] CHECK ([tipo_envio] IN (N'Envío a domicilio', N'Retiro en local'))
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Estado_Envio
   Contiene los posibles estados por los que puede pasar un envío.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Estado_Envio](
-- Define id_estado_envio como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_estado_envio] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre como texto de hasta 30 caracteres, obligatorio.
	[nombre] [nvarchar](30) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_estado_envio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Estado_Variante_Producto
   Contiene los estados aplicables a una variante dentro de una venta.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Estado_Variante_Producto](
-- Define id_estado_variante_producto como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_estado_variante_producto] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre_estado_variante_producto como texto de hasta 100 caracteres, obligatorio.
	[nombre_estado_variante_producto] [nvarchar](100) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_estado_variante_producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Estado_Venta
   Contiene los posibles estados de una venta.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Estado_Venta](
-- Define id_estado_venta como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_estado_venta] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre_estado_venta como texto de hasta 100 caracteres, obligatorio.
	[nombre_estado_venta] [nvarchar](100) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_estado_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Ingreso_Stock
   Registra el encabezado de cada ingreso de mercadería.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Ingreso_Stock](
-- Define id_ingreso como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_ingreso] [int] IDENTITY(1,1) NOT NULL,
-- Define id_proveedor como número entero, obligatorio.
	[id_proveedor] [int] NOT NULL,
-- Define remito_proveedor como texto de hasta 50 caracteres, obligatorio.
	[remito_proveedor] [nvarchar](50) NOT NULL,
-- Define fecha_ingreso como fecha y hora, obligatorio.
	[fecha_ingreso] [datetime2](7) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_ingreso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Localidad
   Guarda las localidades asociadas a clientes y otras entidades.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Localidad](
-- Define id_localidad como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_localidad] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre_localidad como texto de hasta 100 caracteres, obligatorio.
	[nombre_localidad] [nvarchar](100) NOT NULL,
-- Define codigo_postal como número entero, opcional.
	[codigo_postal] [int] NULL,
 CONSTRAINT [PK_Localidad] PRIMARY KEY CLUSTERED
(
	[id_localidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Marca
   Guarda las marcas de los productos.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Marca](
-- Define id_marca como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_marca] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre_marca como texto de hasta 100 caracteres, obligatorio.
	[nombre_marca] [nvarchar](100) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_marca] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Medio_Pago
   Guarda los medios de pago y el recargo configurado para cada uno.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Medio_Pago](
-- Define id_medio_pago como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_medio_pago] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre_medio_pago como texto de hasta 32 caracteres, obligatorio.
	[nombre_medio_pago] [nvarchar](32) NOT NULL,
-- Define recargo_medio_pago como número decimal, opcional.
	[recargo_medio_pago] [decimal](5, 2) NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_medio_pago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Modalidad_Entrega
   Contiene las modalidades disponibles para entregar una venta.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Modalidad_Entrega](
-- Define id_modalidad_entrega como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_modalidad_entrega] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre_modalidad_entrega como texto de hasta 100 caracteres, obligatorio.
	[nombre_modalidad_entrega] [nvarchar](100) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_modalidad_entrega] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Pago
   Registra los pagos realizados y el medio utilizado.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Pago](
-- Define id_pago como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_pago] [int] IDENTITY(1,1) NOT NULL,
-- Define id_venta como número entero, obligatorio.
	[id_venta] [int] NOT NULL,
-- Define id_usuario como número entero, obligatorio.
	[id_usuario] [int] NOT NULL,
-- Define id_medio_pago como número entero, obligatorio.
	[id_medio_pago] [int] NOT NULL,
-- Define cuotas como número entero, opcional.
	[cuotas] [int] NULL,
-- Define monto como número decimal, obligatorio.
	[monto] [decimal](10, 2) NOT NULL,
-- Define monto_final como número decimal, obligatorio.
	[monto_final] [decimal](10, 2) NOT NULL,
-- Define fecha_pago como fecha y hora, obligatorio.
	[fecha_pago] [datetime2](7) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_pago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Producto
   Guarda la información general de los productos comercializados.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Producto](
-- Define id_producto como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_producto] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre_producto como texto de hasta 150 caracteres, obligatorio.
	[nombre_producto] [nvarchar](150) NOT NULL,
-- Define id_marca como número entero, obligatorio.
	[id_marca] [int] NOT NULL,
-- Define descripcion como texto de hasta 500 caracteres, opcional.
	[descripcion] [nvarchar](500) NULL,
-- Define id_categoria como número entero, obligatorio.
	[id_categoria] [int] NOT NULL,
-- Define modelo como texto de hasta 15 caracteres, opcional.
	[modelo] [varchar](15) NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Proveedor
   Guarda los proveedores utilizados en los ingresos de stock.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Proveedor](
-- Define id_proveedor como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_proveedor] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre_proveedor como texto de hasta 150 caracteres, obligatorio.
	[nombre_proveedor] [nvarchar](150) NOT NULL,
-- Define telefono como texto de hasta 20 caracteres, opcional.
	[telefono] [nvarchar](20) NULL,
-- Define email como texto de hasta 150 caracteres, opcional.
	[email] [nvarchar](150) NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_proveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Recibo
   Registra los recibos emitidos por los pagos.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Recibo](
-- Define id_recibo como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_recibo] [int] IDENTITY(1,1) NOT NULL,
-- Define id_pago como número entero, obligatorio.
	[id_pago] [int] NOT NULL,
-- Define fecha_emision como fecha y hora, obligatorio.
	[fecha_emision] [datetime2](7) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_recibo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Remito
   Registra los remitos asociados a las ventas y entregas.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Remito](
-- Define id_remito como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_remito] [int] IDENTITY(1,1) NOT NULL,
-- Define id_venta como número entero, obligatorio.
	[id_venta] [int] NOT NULL,
-- Define fecha_emision como fecha y hora, obligatorio.
	[fecha_emision] [datetime2](7) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_remito] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Rol
   Guarda los roles que determinan los permisos de los usuarios.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Rol](
-- Define id_rol_usuario como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_rol_usuario] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre_rol_usuario como texto de hasta 20 caracteres, obligatorio.
	[nombre_rol_usuario] [nvarchar](20) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_rol_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Usuario
   Guarda los usuarios que pueden iniciar sesión en Prana.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Usuario](
-- Define id_usuario como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_usuario] [int] IDENTITY(1,1) NOT NULL,
-- Define nombre_usuario como texto de hasta 100 caracteres, obligatorio.
	[nombre_usuario] [nvarchar](100) NOT NULL,
-- Define email como texto de hasta 150 caracteres, opcional.
	[email] [nvarchar](150) NULL,
-- Define hash_contrasena como texto de hasta 255 caracteres, obligatorio.
	[hash_contrasena] [nvarchar](255) NOT NULL,
-- Define id_rol_usuario como número entero, obligatorio.
	[id_rol_usuario] [int] NOT NULL,
-- Define activo como valor lógico: 1 representa verdadero y 0 representa falso, obligatorio.
	[activo] [bit] NOT NULL,
-- Conserva los intentos de inicio de sesion fallidos consecutivos.
	[intentos_login_fallidos] [int] NOT NULL CONSTRAINT [DF_Usuario_IntentosLogin] DEFAULT ((0)),
-- Conserva hasta cuando se bloquea temporalmente una cuenta.
	[bloqueado_hasta] [datetime2](7) NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
-- Impide que se repita el valor indicado en diferentes registros.
UNIQUE NONCLUSTERED
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Variante_Producto
   Guarda cada variante comercial de un producto, junto con precio y stock.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Variante_Producto](
-- Define id_variante como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_variante] [int] IDENTITY(1,1) NOT NULL,
-- Define color como texto de hasta 50 caracteres, obligatorio.
	[color] [nvarchar](50) NOT NULL,
-- Define condicion_producto como texto de hasta 10 caracteres, obligatorio.
	[condicion_producto] [nvarchar](10) NOT NULL,
-- Define precio como número decimal, obligatorio.
	[precio] [decimal](10, 2) NOT NULL,
-- Define stock_disponible como número entero, obligatorio.
	[stock_disponible] [int] NOT NULL,
-- Define id_producto como número entero, obligatorio.
	[id_producto] [int] NOT NULL,
-- Define activo como valor lógico: 1 representa verdadero y 0 representa falso, obligatorio.
	[activo] [bit] NOT NULL,
-- Define costo como número decimal, obligatorio.
	[costo] [decimal](18, 2) NOT NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_variante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* =========================================================
   TABLA: Venta
   Registra el encabezado, importes, cliente, usuario y estado de cada venta.
   ========================================================= */
-- Aplica el comportamiento estándar de SQL Server para las comparaciones con valores NULL.
SET ANSI_NULLS ON
GO
-- Permite utilizar identificadores delimitados y reserva las comillas simples para textos.
SET QUOTED_IDENTIFIER ON
GO
-- Comienza la creación física de la tabla y de sus columnas.
CREATE TABLE [dbo].[Venta](
-- Define id_venta como número entero, generado automáticamente de uno en uno, obligatorio.
	[id_venta] [int] IDENTITY(1,1) NOT NULL,
-- Define id_cliente como número entero, obligatorio.
	[id_cliente] [int] NOT NULL,
-- Define id_usuario como número entero, obligatorio.
	[id_usuario] [int] NOT NULL,
-- Define fecha_venta como fecha y hora, obligatorio.
	[fecha_venta] [datetime2](7) NOT NULL,
-- Define id_estado_venta como número entero, obligatorio.
	[id_estado_venta] [int] NOT NULL,
-- Define total como número decimal, obligatorio.
	[total] [decimal](10, 2) NOT NULL,
-- Define id_modalidad_entrega como número entero, obligatorio.
	[id_modalidad_entrega] [int] NOT NULL,
-- Define descuento como número decimal, opcional.
	[descuento] [decimal](10, 2) NULL,
-- Define la clave primaria que identifica de manera única cada registro de la tabla.
PRIMARY KEY CLUSTERED
(
	[id_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-- Establece el valor que se asignará automáticamente cuando no se informe esta columna.
ALTER TABLE [dbo].[Cliente] ADD  DEFAULT ((0)) FOR [saldo_a_favor]
GO
-- Establece el valor que se asignará automáticamente cuando no se informe esta columna.
ALTER TABLE [dbo].[Envio] ADD  DEFAULT ((0)) FOR [costo]
GO
-- Establece el valor que se asignará automáticamente cuando no se informe esta columna.
ALTER TABLE [dbo].[Medio_Pago] ADD  DEFAULT ((0)) FOR [recargo_medio_pago]
GO
-- Establece el valor que se asignará automáticamente cuando no se informe esta columna.
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT ((1)) FOR [activo]
GO
-- Establece el valor que se asignará automáticamente cuando no se informe esta columna.
ALTER TABLE [dbo].[Variante_Producto] ADD  CONSTRAINT [DF_VarianteProducto_Activo]  DEFAULT ((1)) FOR [activo]
GO
-- Establece el valor que se asignará automáticamente cuando no se informe esta columna.
ALTER TABLE [dbo].[Variante_Producto] ADD  CONSTRAINT [DF_VarianteProducto_Costo]  DEFAULT ((0)) FOR [costo]
GO
-- Establece el valor que se asignará automáticamente cuando no se informe esta columna.
ALTER TABLE [dbo].[Venta] ADD  DEFAULT ((0)) FOR [descuento]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Cliente]  WITH CHECK ADD  CONSTRAINT [FK_Cliente_Localidad] FOREIGN KEY([id_localidad])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Localidad] ([id_localidad])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Cliente] CHECK CONSTRAINT [FK_Cliente_Localidad]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Detalle_Ingreso]  WITH CHECK ADD  CONSTRAINT [FK_DetalleIngreso_Ingreso] FOREIGN KEY([id_ingreso])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Ingreso_Stock] ([id_ingreso])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Detalle_Ingreso] CHECK CONSTRAINT [FK_DetalleIngreso_Ingreso]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Detalle_Ingreso]  WITH CHECK ADD  CONSTRAINT [FK_DetalleIngreso_Variante] FOREIGN KEY([id_variante])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Variante_Producto] ([id_variante])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Detalle_Ingreso] CHECK CONSTRAINT [FK_DetalleIngreso_Variante]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Detalle_Remito]  WITH CHECK ADD  CONSTRAINT [FK_DetalleRemito_Remito] FOREIGN KEY([id_remito])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Remito] ([id_remito])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Detalle_Remito] CHECK CONSTRAINT [FK_DetalleRemito_Remito]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Detalle_Remito]  WITH CHECK ADD  CONSTRAINT [FK_DetalleRemito_Variante] FOREIGN KEY([id_variante])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Variante_Producto] ([id_variante])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Detalle_Remito] CHECK CONSTRAINT [FK_DetalleRemito_Variante]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Detalle_Venta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleVenta_EstadoVariante] FOREIGN KEY([id_estado_variante_producto])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Estado_Variante_Producto] ([id_estado_variante_producto])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Detalle_Venta] CHECK CONSTRAINT [FK_DetalleVenta_EstadoVariante]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Detalle_Venta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleVenta_Variante] FOREIGN KEY([id_variante])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Variante_Producto] ([id_variante])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Detalle_Venta] CHECK CONSTRAINT [FK_DetalleVenta_Variante]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Detalle_Venta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleVenta_Venta] FOREIGN KEY([id_venta])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Venta] ([id_venta])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Detalle_Venta] CHECK CONSTRAINT [FK_DetalleVenta_Venta]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Envio]  WITH CHECK ADD  CONSTRAINT [FK_Envio_Estado] FOREIGN KEY([id_estado_envio])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Estado_Envio] ([id_estado_envio])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Envio] CHECK CONSTRAINT [FK_Envio_Estado]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Envio]  WITH CHECK ADD  CONSTRAINT [FK_Envio_Remito] FOREIGN KEY([id_remito])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Remito] ([id_remito])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Envio] CHECK CONSTRAINT [FK_Envio_Remito]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Ingreso_Stock]  WITH CHECK ADD  CONSTRAINT [FK_IngresoStock_Proveedor] FOREIGN KEY([id_proveedor])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Proveedor] ([id_proveedor])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Ingreso_Stock] CHECK CONSTRAINT [FK_IngresoStock_Proveedor]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [FK_Pago_Medio] FOREIGN KEY([id_medio_pago])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Medio_Pago] ([id_medio_pago])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [FK_Pago_Medio]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [FK_Pago_Usuario] FOREIGN KEY([id_usuario])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Usuario] ([id_usuario])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [FK_Pago_Usuario]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [FK_Pago_Venta] FOREIGN KEY([id_venta])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Venta] ([id_venta])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [FK_Pago_Venta]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [FK_Producto_Categoria] FOREIGN KEY([id_categoria])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Categoria] ([id_categoria])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_Categoria]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [FK_Producto_Marca] FOREIGN KEY([id_marca])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Marca] ([id_marca])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_Marca]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Recibo]  WITH CHECK ADD  CONSTRAINT [FK_Recibo_Pago] FOREIGN KEY([id_pago])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Pago] ([id_pago])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Recibo] CHECK CONSTRAINT [FK_Recibo_Pago]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Remito]  WITH CHECK ADD  CONSTRAINT [FK_Remito_Venta] FOREIGN KEY([id_venta])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Venta] ([id_venta])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Remito] CHECK CONSTRAINT [FK_Remito_Venta]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([id_rol_usuario])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Rol] ([id_rol_usuario])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Variante_Producto]  WITH CHECK ADD  CONSTRAINT [FK_Variante_Producto] FOREIGN KEY([id_producto])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Producto] ([id_producto])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Variante_Producto] CHECK CONSTRAINT [FK_Variante_Producto]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Cliente] FOREIGN KEY([id_cliente])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Cliente] ([id_cliente])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Cliente]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Estado] FOREIGN KEY([id_estado_venta])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Estado_Venta] ([id_estado_venta])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Estado]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Modalidad] FOREIGN KEY([id_modalidad_entrega])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Modalidad_Entrega] ([id_modalidad_entrega])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Modalidad]
GO
-- Crea una clave externa para relacionar esta tabla con otra tabla del sistema.
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Usuario] FOREIGN KEY([id_usuario])
-- Indica la tabla y la columna que debe existir para mantener la integridad de la relación.
REFERENCES [dbo].[Usuario] ([id_usuario])
GO
-- Activa y valida la relación definida en la instrucción anterior.
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Usuario]
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Cliente]  WITH CHECK ADD CHECK  (([saldo_a_favor]>=(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Detalle_Ingreso]  WITH CHECK ADD CHECK  (([cantidad]>(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Detalle_Ingreso]  WITH CHECK ADD CHECK  (([costo_unitario]>=(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Detalle_Remito]  WITH CHECK ADD CHECK  (([cantidad]>(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Detalle_Venta]  WITH CHECK ADD CHECK  (([cantidad]>(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Detalle_Venta]  WITH CHECK ADD CHECK  (([precio_unitario]>=(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Detalle_Venta]  WITH CHECK ADD CHECK  (([subtotal]>=(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Envio]  WITH CHECK ADD CHECK  (([costo]>=(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD CHECK  (([monto]>=(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD CHECK  (([monto_final]>=(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Variante_Producto]  WITH CHECK ADD CHECK  (([precio]>=(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Variante_Producto]  WITH CHECK ADD CHECK  (([stock_disponible]>=(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD CHECK  (([descuento]>=(0)))
GO
-- Agrega una regla que impide guardar valores que no cumplan la condición indicada.
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD CHECK  (([total]>=(0)))
GO

/* =========================================================
   DATOS INICIALES DE LA APLICACIÓN
   ========================================================= */

-- Permite indicar manualmente el identificador 1 del rol inicial.
SET IDENTITY_INSERT [dbo].[Rol] ON;
GO

-- Comprueba que el registro no exista para evitar insertarlo dos veces.
IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Rol]
    WHERE [id_rol_usuario] = 1
)
BEGIN
    -- Inserta el rol Administrador requerido por el usuario inicial.
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
END;
GO

-- Devuelve a SQL Server el control automático de los identificadores de Rol.
SET IDENTITY_INSERT [dbo].[Rol] OFF;
GO

-- Permite indicar manualmente el identificador 1 del usuario inicial.
SET IDENTITY_INSERT [dbo].[Usuario] ON;
GO

-- Comprueba que el registro no exista para evitar insertarlo dos veces.
IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Usuario]
    WHERE [nombre_usuario] = N'admin'
)
BEGIN
    -- Inserta el usuario admin utilizado para el primer inicio de sesión.
    INSERT INTO [dbo].[Usuario]
    (
        [id_usuario],
        [nombre_usuario],
        [email],
        [hash_contrasena],
        [id_rol_usuario],
        [activo]
    )
    VALUES
    (
        1,
        N'admin',
        NULL,
        N'$2a$11$gsz7kf9BQFdxU9.hSy8XJON16Yir5OY1FTV0Hxc6TNy8BMKoW4Yvi',
        1,
        1
    );
END;
GO

-- Devuelve a SQL Server el control automático de los identificadores de Usuario.
SET IDENTITY_INSERT [dbo].[Usuario] OFF;
GO

-- Completa la estructura actual: cuotas, historial, logística y rol Vendedor.
/* Actualización acumulativa e idempotente.
   Se ejecuta sobre la conexión abierta por el inicializador, sin USE ni borrados.
   Las columnas existentes y los datos de trabajo se conservan.
*/
IF OBJECT_ID(N'dbo.Usuario',N'U') IS NULL OR OBJECT_ID(N'dbo.Venta',N'U') IS NULL
    OR OBJECT_ID(N'dbo.Medio_Pago',N'U') IS NULL OR OBJECT_ID(N'dbo.Rol',N'U') IS NULL
    THROW 50001, N'La estructura base está incompleta. No se puede actualizar automáticamente.', 1;
GO

-- Recargos por cuotas y baja lógica de los medios de pago.
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

-- Amplía el descuento monetario antiguo sin reducir columnas de mayor capacidad.
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Venta')
    AND name=N'descuento' AND precision<10 AND scale=2 AND TYPE_NAME(system_type_id) IN (N'decimal',N'numeric'))
BEGIN
    IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Venta') AND name=N'descuento' AND is_nullable=1)
        ALTER TABLE dbo.Venta ALTER COLUMN descuento decimal(10,2) NULL;
    ELSE
        ALTER TABLE dbo.Venta ALTER COLUMN descuento decimal(10,2) NOT NULL;
END;
GO

-- Historial: la fecha se registra automáticamente en cada operación.
IF OBJECT_ID(N'dbo.LogActividad',N'U') IS NULL
BEGIN
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

-- Datos de logística que se guardan junto con una nueva venta.
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

-- El código de permisos usa el identificador 2 para Vendedor; no altera roles existentes.
IF NOT EXISTS(SELECT 1 FROM dbo.Rol WHERE id_rol_usuario=2)
BEGIN
    SET IDENTITY_INSERT dbo.Rol ON;
    INSERT INTO dbo.Rol(id_rol_usuario,nombre_rol_usuario) VALUES(2,N'Vendedor');
    SET IDENTITY_INSERT dbo.Rol OFF;
END;
GO
