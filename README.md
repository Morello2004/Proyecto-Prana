# Sistema de Gestión Comercial – Prana Electrónica

Aplicación de escritorio desarrollada como proyecto de **Prácticas Profesionalizantes de la Tecnicatura Superior en Análisis de Sistemas**, orientada a la gestión integral de un comercio de electrodomésticos.

El sistema busca centralizar procesos que actualmente involucran ventas, clientes, productos, stock, proveedores, pagos y entregas, aplicando una arquitectura en capas y una base de datos relacional.

## Tecnologías utilizadas

* **C#**
* **.NET**
* **Windows Forms**
* **SQL Server**
* **Microsoft.Data.SqlClient**
* **Git / GitHub**
* **Visual Studio**

## Arquitectura

El proyecto utiliza una arquitectura multicapa para separar responsabilidades y facilitar el mantenimiento del sistema.

```text
Prana.Presentacion
        │
        ▼
Prana.Negocio
        │
        ▼
Prana.Datos
        │
        ▼
   SQL Server

Prana.Entidades
     ▲   ▲   ▲
     └───┴───┘
```

### Prana.Presentacion

Contiene la interfaz gráfica desarrollada con Windows Forms y la interacción con el usuario.

### Prana.Negocio

Contiene las reglas de negocio, validaciones y comunicación entre la interfaz y la capa de acceso a datos.

### Prana.Datos

Responsable del acceso a SQL Server y de la ejecución de consultas parametrizadas.

### Prana.Entidades

Contiene las entidades y modelos utilizados para representar la información manejada por el sistema.

## Funcionalidades

Actualmente el proyecto cuenta con módulos para:

* Inicio de sesión y validación de usuarios
* Panel principal
* Gestión de clientes
* Gestión de productos y variantes
* Búsqueda y selección de productos
* Consulta y administración de stock
* Registro de ingresos de mercadería
* Gestión de proveedores
* Registro y gestión de ventas
* Gestión de pagos
* Gestión de entregas
* Gestión de usuarios
* Alertas

El sistema continúa en desarrollo y se incorporan nuevas funcionalidades y reglas de negocio de forma progresiva.

## Base de datos

La aplicación utiliza **SQL Server** como sistema gestor de base de datos.

El diseño contempla relaciones entre las diferentes entidades del negocio, utilizando:

* Claves primarias y foráneas
* Restricciones e integridad referencial
* Relaciones entre tablas
* Consultas con múltiples tablas y `JOIN`
* Consultas parametrizadas desde C#
* Operaciones de lectura y modificación de información

El repositorio incluye un script dentro de:

```text
BaseDeDatos/CrearBD.sql
```

para documentar la estructura utilizada por el sistema.

## Estructura del repositorio

```text
Proyecto-Prana/
│
├── Prana.Datos/
├── Prana.Entidades/
├── Prana.negocio/
├── BaseDeDatos/
│   └── CrearBD.sql
│
├── Prana.Presentacion.csproj
├── PranaElectronica.slnx
├── App.config
├── Program.cs
└── ...
```

## Capturas

### Inicio de sesión

> Captura próximamente.

### Panel principal

> Captura próximamente.

### Gestión de productos y stock

> Captura próximamente.

### Registro de ventas

> Captura próximamente.

## Contexto del proyecto

El sistema surge a partir del análisis de procesos reales de **Prana Electrónica**, un comercio de electrodomésticos.

Durante el desarrollo se trabaja sobre el relevamiento de requerimientos, modelado de procesos, diseño de base de datos, definición de reglas de negocio e implementación de las distintas funcionalidades del sistema.

Esto permite aplicar de manera integrada conocimientos de:

* Programación Orientada a Objetos
* Análisis y diseño de sistemas
* Bases de datos relacionales
* Arquitectura de software
* Desarrollo de interfaces
* Control de versiones

## Estado

**En desarrollo.**

Proyecto realizado con fines académicos y de formación profesional.
<img width="1536" height="1024" alt="collages interfaces" src="https://github.com/user-attachments/assets/ddb97a8d-0543-4a59-a3a0-7df9008a489a" />
