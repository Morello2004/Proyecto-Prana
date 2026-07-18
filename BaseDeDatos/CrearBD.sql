-- Script inicial PranaElectronica

-- Crear base de datos owo
CREATE DATABASE PranaElectronica;
GO
USE PranaElectronica;
GO
-- Ahora las tablas jejejejejeje
CREATE TABLE Categoria(
 id_categoria INT IDENTITY(1,1) PRIMARY KEY,
 nombre_categoria NVARCHAR(100) NOT NULL
);

CREATE TABLE Marca(
 id_marca INT IDENTITY(1,1) PRIMARY KEY,
 nombre_marca NVARCHAR(100) NOT NULL
);

CREATE TABLE Localidad(
 codigo_postal_localidad INT PRIMARY KEY,
 nombre_localidad NVARCHAR(30) NOT NULL
);

CREATE TABLE Rol(
 id_rol_usuario INT IDENTITY(1,1) PRIMARY KEY,
 nombre_rol_usuario NVARCHAR(20) NOT NULL
);

CREATE TABLE Usuario(
 id_usuario INT IDENTITY(1,1) PRIMARY KEY,
 nombre_usuario NVARCHAR(100) NOT NULL,
 email NVARCHAR(150) NOT NULL UNIQUE,
 hash_contrasena NVARCHAR(255) NOT NULL,
 id_rol_usuario INT NOT NULL,
 activo BIT NOT NULL DEFAULT 1,
 CONSTRAINT FK_Usuario_Rol FOREIGN KEY(id_rol_usuario) REFERENCES Rol(id_rol_usuario)
);

CREATE TABLE Producto(
 id_producto INT IDENTITY(1,1) PRIMARY KEY,
 nombre_producto NVARCHAR(150) NOT NULL,
 id_marca INT NOT NULL,
 descripcion NVARCHAR(500) NULL,
 id_categoria INT NOT NULL,
 CONSTRAINT FK_Producto_Marca FOREIGN KEY(id_marca) REFERENCES Marca(id_marca),
 CONSTRAINT FK_Producto_Categoria FOREIGN KEY(id_categoria) REFERENCES Categoria(id_categoria)
);

CREATE TABLE Variante_Producto(
    id_variante INT IDENTITY(1,1) PRIMARY KEY,
    color NVARCHAR(50) NOT NULL,
    condicion_producto NVARCHAR(10) NOT NULL,
    precio DECIMAL(10,2) NOT NULL CHECK (precio >= 0),
    stock_disponible INT NOT NULL CHECK (stock_disponible >= 0),
    id_producto INT NOT NULL,
    CONSTRAINT FK_Variante_Producto
        FOREIGN KEY (id_producto)
        REFERENCES Producto(id_producto)
);

CREATE TABLE Cliente(
    id_cliente INT IDENTITY(1,1) PRIMARY KEY,
    nombre_cliente NVARCHAR(100) NOT NULL,
    apellido_cliente NVARCHAR(100) NOT NULL,
    dni NVARCHAR(11) NOT NULL UNIQUE,
    telefono NVARCHAR(20) NOT NULL,
    email NVARCHAR(150) NULL,
    direccion NVARCHAR(200) NULL,
    codigo_postal_localidad INT NOT NULL,
    fecha_registro DATE NOT NULL,
    saldo_a_favor DECIMAL(10,2) NULL DEFAULT 0 CHECK (saldo_a_favor >= 0),
    CONSTRAINT FK_Cliente_Localidad
        FOREIGN KEY (codigo_postal_localidad)
        REFERENCES Localidad(codigo_postal_localidad)
);

CREATE TABLE Modalidad_Entrega(
    id_modalidad_entrega INT IDENTITY(1,1) PRIMARY KEY,
    nombre_modalidad_entrega NVARCHAR(100) NOT NULL
);

CREATE TABLE Estado_Variante_Producto(
    id_estado_variante_producto INT IDENTITY(1,1) PRIMARY KEY,
    nombre_estado_variante_producto NVARCHAR(100) NOT NULL
);

CREATE TABLE Estado_Venta(
    id_estado_venta INT IDENTITY(1,1) PRIMARY KEY,
    nombre_estado_venta NVARCHAR(100) NOT NULL
);

CREATE TABLE Venta(
    id_venta INT IDENTITY(1,1) PRIMARY KEY,
    id_cliente INT NOT NULL,
    id_usuario INT NOT NULL,
    fecha_venta DATETIME2 NOT NULL,
    id_estado_venta INT NOT NULL,
    total DECIMAL(10,2) NOT NULL CHECK (total >= 0),
    id_modalidad_entrega INT NOT NULL,
    descuento DECIMAL(5,2) NULL DEFAULT 0 CHECK (descuento >= 0),

    CONSTRAINT FK_Venta_Cliente
        FOREIGN KEY (id_cliente)
        REFERENCES Cliente(id_cliente),

    CONSTRAINT FK_Venta_Usuario
        FOREIGN KEY (id_usuario)
        REFERENCES Usuario(id_usuario),

    CONSTRAINT FK_Venta_Estado
        FOREIGN KEY (id_estado_venta)
        REFERENCES Estado_Venta(id_estado_venta),

    CONSTRAINT FK_Venta_Modalidad
        FOREIGN KEY (id_modalidad_entrega)
        REFERENCES Modalidad_Entrega(id_modalidad_entrega)
);

CREATE TABLE Detalle_Venta(
    id_detalle_venta INT IDENTITY(1,1) PRIMARY KEY,
    id_venta INT NOT NULL,
    id_variante INT NOT NULL,
    cantidad INT NOT NULL CHECK (cantidad > 0),
    precio_unitario DECIMAL(10,2) NOT NULL CHECK (precio_unitario >= 0),
    subtotal DECIMAL(10,2) NOT NULL CHECK (subtotal >= 0),
    id_estado_variante_producto INT NOT NULL,

    CONSTRAINT FK_DetalleVenta_EstadoVariante   
        FOREIGN KEY (id_estado_variante_producto)
        REFERENCES Estado_Variante_Producto(id_estado_variante_producto),

    CONSTRAINT FK_DetalleVenta_Venta
        FOREIGN KEY (id_venta)
        REFERENCES Venta(id_venta),

    CONSTRAINT FK_DetalleVenta_Variante
        FOREIGN KEY (id_variante)
        REFERENCES Variante_Producto(id_variante)
);

CREATE TABLE Medio_Pago(
    id_medio_pago INT IDENTITY(1,1) PRIMARY KEY,
    nombre_medio_pago NVARCHAR(32) NOT NULL,
    recargo_medio_pago DECIMAL(5,2) NULL DEFAULT 0
);

CREATE TABLE Pago(
    id_pago INT IDENTITY(1,1) PRIMARY KEY,
    id_venta INT NOT NULL,
    id_usuario INT NOT NULL,
    id_medio_pago INT NOT NULL,
    cuotas INT NULL,
    monto DECIMAL(10,2) NOT NULL CHECK (monto >= 0),
    monto_final DECIMAL(10,2) NOT NULL CHECK (monto_final >= 0),
    fecha_pago DATETIME2 NOT NULL,

    CONSTRAINT FK_Pago_Venta
        FOREIGN KEY (id_venta)
        REFERENCES Venta(id_venta),

    CONSTRAINT FK_Pago_Usuario
        FOREIGN KEY (id_usuario)
        REFERENCES Usuario(id_usuario),

    CONSTRAINT FK_Pago_Medio
        FOREIGN KEY (id_medio_pago)
        REFERENCES Medio_Pago(id_medio_pago)
);

CREATE TABLE Formato_Documento(
    id_formato_documento INT IDENTITY(1,1) PRIMARY KEY,
    nombre_formato_documento NVARCHAR(30) NOT NULL
);

CREATE TABLE Recibo(
    id_recibo INT IDENTITY(1,1) PRIMARY KEY,
    id_pago INT NOT NULL,
    fecha_emision DATETIME2 NOT NULL,
    id_formato_documento INT NOT NULL,

    CONSTRAINT FK_Recibo_Pago
        FOREIGN KEY (id_pago)
        REFERENCES Pago(id_pago),

    CONSTRAINT FK_Recibo_Formato
        FOREIGN KEY (id_formato_documento)
        REFERENCES Formato_Documento(id_formato_documento)
);

CREATE TABLE Remito(
    id_remito INT IDENTITY(1,1) PRIMARY KEY,
    id_venta INT NOT NULL,
    fecha_emision DATETIME2 NOT NULL,
    id_formato_documento INT NOT NULL,

    CONSTRAINT FK_Remito_Venta
        FOREIGN KEY (id_venta)
        REFERENCES Venta(id_venta),

    CONSTRAINT FK_Remito_Formato
        FOREIGN KEY (id_formato_documento)
        REFERENCES Formato_Documento(id_formato_documento)
);

CREATE TABLE Detalle_Remito(
    id_detalle_remito INT IDENTITY(1,1) PRIMARY KEY,
    id_remito INT NOT NULL,
    id_variante INT NOT NULL,
    cantidad INT NOT NULL CHECK (cantidad > 0),
    descripcion NVARCHAR(300) NOT NULL,

    CONSTRAINT FK_DetalleRemito_Remito
        FOREIGN KEY (id_remito)
        REFERENCES Remito(id_remito),

    CONSTRAINT FK_DetalleRemito_Variante
        FOREIGN KEY (id_variante)
        REFERENCES Variante_Producto(id_variante)
);

CREATE TABLE Estado_Envio(
    id_estado_envio INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(30) NOT NULL
);

CREATE TABLE Tipo_Envio(
    id_tipo_envio INT IDENTITY(1,1) PRIMARY KEY,
    nombre_tipo_envio NVARCHAR(30) NOT NULL
);

CREATE TABLE Envio(
    id_envio INT IDENTITY(1,1) PRIMARY KEY,
    id_remito INT NOT NULL,
    direccion NVARCHAR(200) NOT NULL,
    horario_sugerido NVARCHAR(50) NULL,
    id_tipo_envio INT NOT NULL,
    costo DECIMAL(10,2) NOT NULL DEFAULT 0 CHECK (costo >= 0),
    id_estado_envio INT NOT NULL,
    fecha_entrega DATETIME2 NULL,

    CONSTRAINT FK_Envio_Remito
        FOREIGN KEY (id_remito)
        REFERENCES Remito(id_remito),

    CONSTRAINT FK_Envio_Tipo
        FOREIGN KEY (id_tipo_envio)
        REFERENCES Tipo_Envio(id_tipo_envio),

    CONSTRAINT FK_Envio_Estado
        FOREIGN KEY (id_estado_envio)
        REFERENCES Estado_Envio(id_estado_envio)
);

CREATE TABLE Tipo_Resolucion(
    id_tipo_resolucion INT IDENTITY(1,1) PRIMARY KEY,
    nombre_tipo_resolucion NVARCHAR(50) NOT NULL
);

CREATE TABLE Devolucion(
    id_devolucion INT IDENTITY(1,1) PRIMARY KEY,
    id_detalle_venta INT NOT NULL,
    cantidad INT NOT NULL CHECK (cantidad > 0),
    motivo NVARCHAR(300) NOT NULL,
    id_tipo_resolucion INT NOT NULL,
    monto_diferencia DECIMAL(10,2) NULL,
    fecha_devolucion DATETIME2 NOT NULL,

    CONSTRAINT FK_Devolucion_DetalleVenta
        FOREIGN KEY (id_detalle_venta)
        REFERENCES Detalle_Venta(id_detalle_venta),

    CONSTRAINT FK_Devolucion_TipoResolucion
        FOREIGN KEY (id_tipo_resolucion)
        REFERENCES Tipo_Resolucion(id_tipo_resolucion)
);

CREATE TABLE Proveedor(
    id_proveedor INT IDENTITY(1,1) PRIMARY KEY,
    nombre_proveedor NVARCHAR(150) NOT NULL,
    telefono NVARCHAR(20) NULL,
    email NVARCHAR(150) NULL
);

CREATE TABLE Ingreso_Stock(
    id_ingreso INT IDENTITY(1,1) PRIMARY KEY,
    id_proveedor INT NOT NULL,
    remito_proveedor NVARCHAR(50) NOT NULL,
    fecha_ingreso DATETIME2 NOT NULL,

    CONSTRAINT FK_IngresoStock_Proveedor
        FOREIGN KEY (id_proveedor)
        REFERENCES Proveedor(id_proveedor)
);

CREATE TABLE Detalle_Ingreso(
    id_detalle_ingreso INT IDENTITY(1,1) PRIMARY KEY,
    id_ingreso INT NOT NULL,
    id_variante INT NOT NULL,
    cantidad INT NOT NULL CHECK (cantidad > 0),
    costo_unitario DECIMAL(10,2) NOT NULL CHECK (costo_unitario >= 0),

    CONSTRAINT FK_DetalleIngreso_Ingreso
        FOREIGN KEY (id_ingreso)
        REFERENCES Ingreso_Stock(id_ingreso),

    CONSTRAINT FK_DetalleIngreso_Variante
        FOREIGN KEY (id_variante)
        REFERENCES Variante_Producto(id_variante)
);

