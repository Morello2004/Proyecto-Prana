using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Prana.negocio
{
    public class CN_Cliente
    {
        private readonly CD_Cliente datosCliente = new CD_Cliente();

        // valida los datos y solicita el registro del cliente
        // valida los datos y registra un nuevo cliente
        public bool NuevoCliente(Cliente cliente)
        {
            // comprueba que se haya recibido un cliente
            if (cliente == null)
            {
                throw new InvalidOperationException("No se recibieron los datos del cliente");
            }

            // comprueba que el nombre este completo
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                throw new InvalidOperationException("Debe ingresar el nombre");
            }

            // comprueba que el nombre solo contenga letras
            if (!Regex.IsMatch(cliente.Nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
            {
                throw new InvalidOperationException("El nombre solamente puede contener letras");
            }

            // comprueba que el apellido este completo
            if (string.IsNullOrWhiteSpace(cliente.Apellido))
            {
                throw new InvalidOperationException("Debe ingresar el apellido");
            }

            // comprueba que el apellido solo contenga letras
            if (!Regex.IsMatch(cliente.Apellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
            {
                throw new InvalidOperationException("El apellido solamente puede contener letras");
            }

            // comprueba que el dni tenga siete u ocho numeros
            if (!Regex.IsMatch(cliente.DNI, @"^\d{7,8}$"))
            {
                throw new InvalidOperationException("El DNI debe tener entre 7 y 8 números");
            }

            // comprueba que el telefono tenga un formato valido
            if (!Regex.IsMatch(cliente.Telefono, @"^[0-9+\-\s()]{8,20}$"))
            {
                throw new InvalidOperationException("El teléfono ingresado no es válido");
            }

            // comprueba que la direccion este completa
            if (string.IsNullOrWhiteSpace(cliente.Direccion))
            {
                throw new InvalidOperationException("Debe ingresar la dirección");
            }

            // comprueba que la localidad este completa
            if (cliente.Localidad == null || string.IsNullOrWhiteSpace(cliente.Localidad.NombreLocalidad))
            {
                throw new InvalidOperationException("Debe ingresar la localidad");
            }


            // comprueba si el dni ya se encuentra registrado
            if (datosCliente.ExisteClientePorDNI(cliente.DNI, 0))
            {
                throw new InvalidOperationException("Ya existe un cliente registrado con ese DNI");
            }

            // envia el cliente a la capa de datos
            return datosCliente.NuevoCliente(cliente);
        }

        // solicita la busqueda de clientes
        public List<Cliente> buscarCliente(string? busqueda)
        {
            // envia la busqueda a la capa de datos
            return datosCliente.buscarCliente(busqueda) ?? new List<Cliente>();
        }


        // valida los datos y modifica el cliente
        public bool ModificarCliente(Cliente cliente)
        {
            // comprueba que el cliente tenga un identificador valido
            if (cliente == null || cliente.IdCliente <= 0)
            {
                throw new InvalidOperationException("No se pudo identificar al cliente");
            }

            // comprueba que el nombre este completo
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                throw new InvalidOperationException("Debe ingresar el nombre");
            }

            // comprueba que el nombre solo contenga letras
            if (!Regex.IsMatch(cliente.Nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
            {
                throw new InvalidOperationException("El nombre solamente puede contener letras");
            }

            // comprueba que el apellido este completo
            if (string.IsNullOrWhiteSpace(cliente.Apellido))
            {
                throw new InvalidOperationException("Debe ingresar el apellido");
            }

            // comprueba que el apellido solo contenga letras
            if (!Regex.IsMatch(cliente.Apellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
            {
                throw new InvalidOperationException("El apellido solamente puede contener letras");
            }

            // comprueba que el dni tenga siete u ocho numeros
            if (!Regex.IsMatch(cliente.DNI, @"^\d{7,8}$"))
            {
                throw new InvalidOperationException("El DNI debe tener entre 7 y 8 números");
            }

            // comprueba que el telefono tenga un formato valido
            if (!Regex.IsMatch(cliente.Telefono, @"^[0-9+\-\s()]{8,20}$"))
            {
                throw new InvalidOperationException("El teléfono ingresado no es válido");
            }

            // comprueba que la direccion este completa
            if (string.IsNullOrWhiteSpace(cliente.Direccion))
            {
                throw new InvalidOperationException("Debe ingresar la dirección");
            }

            // comprueba que la localidad este completa
            if (cliente.Localidad == null || string.IsNullOrWhiteSpace(cliente.Localidad.NombreLocalidad))
            {
                throw new InvalidOperationException("Debe ingresar la localidad");
            }

            // comprueba que el dni no pertenezca a otro cliente
            if (datosCliente.ExisteClientePorDNI(cliente.DNI, cliente.IdCliente))
            {
                throw new InvalidOperationException("Ya existe otro cliente registrado con ese DNI");
            }

            // envia el cliente a la capa de datos
            return datosCliente.ModificarCliente(cliente);
        }
    }
}
