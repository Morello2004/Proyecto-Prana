using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Prana.negocio
{
    // agrupa los datos y operaciones de cn cliente
    public class CN_Cliente
    {
        public const int MaximoNombreApellido = 100;

        private static void ValidarLongitudNombreApellido(Cliente cliente)
        {
            if (cliente.Nombre.Length > MaximoNombreApellido)
                throw new InvalidOperationException("El nombre admite hasta 100 caracteres; no se guardó ningún cambio.");
            if (cliente.Apellido.Length > MaximoNombreApellido)
                throw new InvalidOperationException("El apellido admite hasta 100 caracteres; no se guardó ningún cambio.");
        }
        // Solo comprueba dígitos y longitud; no interpreta prefijos ni operadores.
        public static void ValidarTelefono(string? telefono)
        {
            if (telefono == null || !Regex.IsMatch(telefono, @"\A[0-9]{10}\z"))
            {
                throw new InvalidOperationException("El teléfono debe contener 10 dígitos entre área y número, sin letras ni separadores.");
            }
        }
        public static string UnirTelefono(string? area, string? numero, bool opcional = false)
        {
            area = (area ?? string.Empty).Trim();
            numero = (numero ?? string.Empty).Trim();
            if (opcional && area.Length == 0 && numero.Length == 0) return string.Empty;
            if (!Regex.IsMatch(area, @"\A[0-9]{2,4}\z") || !Regex.IsMatch(numero, @"\A[0-9]{6,8}\z"))
                throw new InvalidOperationException("Ingrese un área de 2 a 4 dígitos y un número de 6 a 8 dígitos; entre ambos deben sumar 10 dígitos.");
            string telefono = area + numero;
            ValidarTelefono(telefono);
            return telefono;
        }
        // valida el documento segun el tipo elegido antes de consultar la base comprueba validar documento; si no se cumple la regla interrumpe con un mensaje antes de continuar
        private static void ValidarDocumento(Cliente cliente)
        {
            cliente.TipoDocumento = (cliente.TipoDocumento ?? "").Trim().ToUpperInvariant();
            cliente.DNI = (cliente.DNI ?? "").Trim();
            if (cliente.TipoDocumento == "DNI")
            {
                if (!Regex.IsMatch(cliente.DNI, @"\A[0-9]{7,8}\z"))
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("El DNI debe tener 7 u 8 numeros sin puntos ni espacios.");
                }

                return;
            }

            if (cliente.TipoDocumento != "CUIT")
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Seleccione DNI o CUIT como tipo de documento.");
            }

            if (!Regex.IsMatch(cliente.DNI, @"\A[0-9]{11}\z"))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El CUIT debe tener 11 numeros sin guiones ni espacios.");
            }

            // calcula el digito verificador con los primeros diez numeros
            int[] pesos =
            {
                5,
                4,
                3,
                2,
                7,
                6,
                5,
                4,
                3,
                2
            };
            int suma = 0;
            for (int i = 0; i < pesos.Length; i++)
            {
                suma += (cliente.DNI[i] - '0') * pesos[i];
            }

            int digito = 11 - suma % 11;
            if (digito == 11)
            {
                digito = 0;
            }

            if (digito == 10 || digito != cliente.DNI[10] - '0' || cliente.DNI == "00000000000")
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El CUIT tiene un digito verificador invalido.");
            }
        }

        // conserva datos cliente entre las operaciones de esta clase
        private readonly CD_Cliente datosCliente = new CD_Cliente();
        // valida los datos y solicita el registro del cliente valida los datos y registra un nuevo cliente
        public bool NuevoCliente(Cliente cliente)
        {
            // comprueba que se haya recibido un cliente
            if (cliente == null)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("No se recibieron los datos del cliente");
            }

            // aplica el mismo formato al crear y modificar un cliente
            cliente.Nombre = NormalizarTexto.Mayusculas(cliente.Nombre);
            cliente.Apellido = NormalizarTexto.Mayusculas(cliente.Apellido);
            ValidarLongitudNombreApellido(cliente);
            cliente.Direccion = NormalizarTexto.Mayusculas(cliente.Direccion);
            cliente.NombreLocalidad = Direcciones.NormalizarLocalidad(cliente.NombreLocalidad);

            // comprueba que el nombre este completo
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Debe ingresar el nombre");
            }

            // comprueba que el nombre solo contenga letras
            if (!Regex.IsMatch(cliente.Nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El nombre solamente puede contener letras");
            }

            // comprueba que el apellido este completo
            if (string.IsNullOrWhiteSpace(cliente.Apellido))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Debe ingresar el apellido");
            }

            // comprueba que el apellido solo contenga letras
            if (!Regex.IsMatch(cliente.Apellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El apellido solamente puede contener letras");
            }

            ValidarDocumento(cliente);
            ValidarTelefono(cliente.Telefono);

            cliente.Direccion = (cliente.Direccion ?? "").Trim();
            cliente.Direccion = Direcciones.ValidarCalleNumero(cliente.Direccion, false);
            // comprueba si el dni ya se encuentra registrado
            if (datosCliente.ExisteClientePorDNI(cliente.DNI, 0))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ya existe un cliente registrado con ese documento");
            }

            // envia el cliente a la capa de datos
            return datosCliente.NuevoCliente(cliente);
        }

        // la busqueda simple mantiene el comportamiento habitual: devuelve clientes activos
        public List<Cliente> buscarCliente(string? busqueda) => buscarCliente(busqueda, 0);
        // filtro decide que estados traer; buscarporprefijo permite que "40" coincida con dni/telefono que empiezan asi
        public List<Cliente> buscarCliente(string? busqueda, int filtro, bool buscarPorPrefijo = false)
        {
            // normaliza espacios para que nombres escritos con espacios repetidos se busquen de forma consistente
            if (string.IsNullOrWhiteSpace(busqueda))
            {
                busqueda = null;
            }
            else
            {
                busqueda = Regex.Replace(busqueda.Trim(), @"\s+", " ");
            }
            return datosCliente.buscarCliente(busqueda, filtro, buscarPorPrefijo);
        }

        // resuelve dar de baja cliente con los datos recibidos y entrega el resultado al codigo que lo llama
        public void DarDeBajaCliente(int idCliente)
        {
            Permisos.Exigir(Permisos.DarDeBajaClientes);
            datosCliente.DarDeBajaCliente(idCliente, UsuarioActualId());
        }

        // resuelve activar cliente con los datos recibidos y entrega el resultado al codigo que lo llama
        public void ActivarCliente(int idCliente) => datosCliente.ActivarCliente(idCliente, UsuarioActualId());
        // resuelve restaurar cliente con los datos recibidos y entrega el resultado al codigo que lo llama
        public void RestaurarCliente(int idCliente) => datosCliente.RestaurarCliente(idCliente, UsuarioActualId());
        // resuelve usuario actual id con los datos recibidos y entrega el resultado al codigo que lo llama
        private static int UsuarioActualId() => SesionActual.Usuario?.idUsuario ?? throw new InvalidOperationException("Se necesita una sesión iniciada para cambiar el estado del cliente.");
        // valida los datos y modifica el cliente
        public bool ModificarCliente(Cliente cliente)
        {
            // comprueba que el cliente tenga un identificador valido
            if (cliente == null || cliente.IdCliente <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("No se pudo identificar al cliente");
            }

            // aplica el mismo formato al crear y modificar un cliente
            cliente.Nombre = NormalizarTexto.Mayusculas(cliente.Nombre);
            cliente.Apellido = NormalizarTexto.Mayusculas(cliente.Apellido);
            ValidarLongitudNombreApellido(cliente);
            cliente.Direccion = NormalizarTexto.Mayusculas(cliente.Direccion);
            cliente.NombreLocalidad = Direcciones.NormalizarLocalidad(cliente.NombreLocalidad);

            // comprueba que el nombre este completo
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Debe ingresar el nombre");
            }

            // comprueba que el nombre solo contenga letras
            if (!Regex.IsMatch(cliente.Nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El nombre solamente puede contener letras");
            }

            // comprueba que el apellido este completo
            if (string.IsNullOrWhiteSpace(cliente.Apellido))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Debe ingresar el apellido");
            }

            // comprueba que el apellido solo contenga letras
            if (!Regex.IsMatch(cliente.Apellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El apellido solamente puede contener letras");
            }

            ValidarDocumento(cliente);
            ValidarTelefono(cliente.Telefono);

            cliente.Direccion = (cliente.Direccion ?? "").Trim();
            cliente.Direccion = Direcciones.ValidarCalleNumero(cliente.Direccion, false);
            // comprueba que el dni no pertenezca a otro cliente
            if (datosCliente.ExisteClientePorDNI(cliente.DNI, cliente.IdCliente))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ya existe otro cliente registrado con ese documento");
            }

            // envia el cliente a la capa de datos
            return datosCliente.ModificarCliente(cliente);
        }
    }
}
