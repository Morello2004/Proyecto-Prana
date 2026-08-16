using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.Datos
{
    public class CD_Cliente
    {
        //metodo que inserta un nuevo cliente en la base de datos
        // registra un cliente junto con su localidad
        public bool NuevoCliente(Cliente cliente)
        {
            // crea la conexion con la base de datos
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

            // abre la conexion con la base de datos
            conexion.Open();

            // inicia una transaccion para guardar todo junto
            using SqlTransaction transaccion = conexion.BeginTransaction();

            try
            {
                // crea el objeto que permite trabajar con las localidades
                CD_Localidad datosLocalidad = new CD_Localidad();

                // busca la localidad o la crea y obtiene su identificador
                int idLocalidad = datosLocalidad.ObtenerOCrearLocalidad(cliente.Localidad.NombreLocalidad, conexion, transaccion);

                // prepara la consulta para registrar el cliente
                string consulta = "INSERT INTO Cliente (nombre_cliente, apellido_cliente, dni, telefono, fecha_registro, direccion, saldo_a_favor, id_localidad) VALUES (@Nombre, @Apellido, @DNI, @Telefono, @FechaRegistro, @Direccion, @SaldoFavor, @IdLocalidad)";

                // crea el comando que registrara el cliente
                using SqlCommand comando = new SqlCommand(consulta, conexion, transaccion);

                // envia el nombre del cliente a la consulta
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = cliente.Nombre;

                // envia el apellido del cliente a la consulta
                comando.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = cliente.Apellido;

                // envia el dni del cliente a la consulta
                comando.Parameters.Add("@DNI", SqlDbType.NVarChar, 20).Value = cliente.DNI;

                // envia el telefono o un valor nulo si esta vacio
                comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(cliente.Telefono) ? DBNull.Value : cliente.Telefono;

                // envia la fecha de registro del cliente
                comando.Parameters.Add("@FechaRegistro", SqlDbType.DateTime).Value = cliente.FechaRegistro;

                // envia la direccion o un valor nulo si esta vacia
                comando.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(cliente.Direccion) ? DBNull.Value : cliente.Direccion;

                // envia el saldo a favor del cliente
                comando.Parameters.Add("@SaldoFavor", SqlDbType.Decimal).Value = cliente.SaldoFavor;

                // envia el identificador de la localidad
                comando.Parameters.Add("@IdLocalidad", SqlDbType.Int).Value = idLocalidad;

                // ejecuta la consulta y guarda la cantidad de filas modificadas
                int filasAfectadas = comando.ExecuteNonQuery();

                // confirma todos los cambios realizados
                transaccion.Commit();

                // devuelve verdadero si se registro el cliente
                return filasAfectadas > 0;
            }
            catch
            {
                // deshace los cambios si ocurre un error
                transaccion.Rollback();

                // envia el error a la capa que llamo al metodo
                throw;
            }
        }

        //metodo que busca al cliente por nombre, apellido y telefono, si no se ingresa ningun parametro devuelve todos los clientes
        // busca clientes por nombre apellido o telefono
        // busca clientes por nombre apellido dni o telefono
        public List<Cliente> buscarCliente(string? busqueda)
        {
            // crea la lista de resultados
            List<Cliente> clientesEncontrados = new List<Cliente>();

            // busca el texto en los datos principales del cliente
            string consulta = @"SELECT c.id_cliente, c.nombre_cliente, c.apellido_cliente, c.dni, c.telefono, c.fecha_registro, c.direccion, c.saldo_a_favor, c.id_localidad, l.nombre_localidad, l.codigo_postal FROM Cliente c LEFT JOIN Localidad l ON c.id_localidad = l.id_localidad WHERE (@busqueda IS NULL OR c.nombre_cliente LIKE '%' + @busqueda + '%' OR c.apellido_cliente LIKE '%' + @busqueda + '%' OR c.dni LIKE '%' + @busqueda + '%' OR c.telefono LIKE '%' + @busqueda + '%')";

            // crea la conexion
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

            // crea el comando
            using SqlCommand comando = new SqlCommand(consulta, conexion);

            // envia la busqueda o null si esta vacia
            comando.Parameters.Add("@busqueda", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(busqueda) ? DBNull.Value : busqueda;

            // abre la conexion
            conexion.Open();

            // ejecuta la consulta
            using SqlDataReader lector = comando.ExecuteReader();

            // recorre los clientes encontrados
            while (lector.Read())
            {
                // crea el cliente obtenido
                Cliente cliente = new Cliente
                {
                    IdCliente = lector.GetInt32(lector.GetOrdinal("id_cliente")),
                    Nombre = lector["nombre_cliente"]?.ToString() ?? string.Empty,
                    Apellido = lector["apellido_cliente"]?.ToString() ?? string.Empty,
                    DNI = lector["dni"]?.ToString() ?? string.Empty,
                    Telefono = lector["telefono"]?.ToString() ?? string.Empty,
                    FechaRegistro = lector.GetDateTime(lector.GetOrdinal("fecha_registro")),
                    Direccion = lector["direccion"]?.ToString() ?? string.Empty,
                    SaldoFavor = lector.GetDecimal(lector.GetOrdinal("saldo_a_favor")),
                    IdLocalidad = lector["id_localidad"] == DBNull.Value ? 0 : Convert.ToInt32(lector["id_localidad"]),

                    // carga la localidad del cliente
                    Localidad = new Localidad
                    {
                        IdLocalidad = lector["id_localidad"] == DBNull.Value ? 0 : Convert.ToInt32(lector["id_localidad"]),
                        NombreLocalidad = lector["nombre_localidad"]?.ToString() ?? string.Empty,
                        CodigoPostal = lector["codigo_postal"]?.ToString() ?? string.Empty
                    }
                };

                // agrega el cliente a los resultados
                clientesEncontrados.Add(cliente);
            }

            // devuelve los clientes encontrados
            return clientesEncontrados;
        }


        // modifica los datos de un cliente y su localidad
        public bool ModificarCliente(Cliente cliente)
        {
            // crea la conexion con la base de datos
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

            // abre la conexion con la base de datos
            conexion.Open();

            // inicia una transaccion para guardar todos los cambios juntos
            using SqlTransaction transaccion = conexion.BeginTransaction();

            try
            {
                // crea el objeto que permite trabajar con localidades
                CD_Localidad datosLocalidad = new CD_Localidad();

                // busca la localidad o la crea y obtiene su identificador
                int idLocalidad = datosLocalidad.ObtenerOCrearLocalidad(cliente.Localidad.NombreLocalidad, conexion, transaccion);

                // prepara la consulta para modificar el cliente
                string consulta = "UPDATE Cliente SET nombre_cliente = @Nombre, apellido_cliente = @Apellido, dni = @DNI, telefono = @Telefono, direccion = @Direccion, id_localidad = @IdLocalidad WHERE id_cliente = @IdCliente";

                // crea el comando que ejecutara la modificacion
                using SqlCommand comando = new SqlCommand(consulta, conexion, transaccion);

                // envia el identificador del cliente
                comando.Parameters.Add("@IdCliente", SqlDbType.Int).Value = cliente.IdCliente;

                // envia el nombre del cliente
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = cliente.Nombre;

                // envia el apellido del cliente
                comando.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = cliente.Apellido;

                // envia el dni del cliente
                comando.Parameters.Add("@DNI", SqlDbType.NVarChar, 20).Value = cliente.DNI;

                // envia el telefono o un valor nulo si esta vacio
                comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(cliente.Telefono) ? DBNull.Value : cliente.Telefono;

                // envia la direccion o un valor nulo si esta vacia
                comando.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(cliente.Direccion) ? DBNull.Value : cliente.Direccion;

                // envia el identificador de la localidad
                comando.Parameters.Add("@IdLocalidad", SqlDbType.Int).Value = idLocalidad;

                // ejecuta la consulta y obtiene las filas modificadas
                int filasAfectadas = comando.ExecuteNonQuery();

                // confirma todos los cambios realizados
                transaccion.Commit();

                // devuelve verdadero si se modifico el cliente
                return filasAfectadas > 0;
            }
            catch
            {
                // deshace todos los cambios si ocurre un error
                transaccion.Rollback();

                // envia el error a la capa que llamo al metodo
                throw;
            }
        }


        // comprueba si el dni pertenece a otro cliente
        public bool ExisteClientePorDNI(string dni, int idClienteExcluir)
        {
            // busca el dni ignorando el cliente indicado
            string consulta = "SELECT COUNT(*) FROM Cliente WHERE dni = @DNI AND id_cliente <> @IdClienteExcluir";

            // crea la conexion
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

            // crea el comando
            using SqlCommand comando = new SqlCommand(consulta, conexion);

            // envia el dni
            comando.Parameters.Add("@DNI", SqlDbType.NVarChar, 20).Value = dni;

            // envia el identificador que debe ignorarse
            comando.Parameters.Add("@IdClienteExcluir", SqlDbType.Int).Value = idClienteExcluir;

            // abre la conexion
            conexion.Open();

            // obtiene la cantidad de coincidencias
            int cantidadClientes = Convert.ToInt32(comando.ExecuteScalar());

            // devuelve verdadero si encontro otro cliente
            return cantidadClientes > 0;
        }

    }
}

