using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Data;

namespace Prana.Datos;

// administra las opciones existentes sin borrar productos ni ingresos de mercaderia
public class CD_OpcionStock
{
    // los identificadores sql salen de esta lista interna y nunca del texto escrito por el usuario
    private static (string Tabla, string Id, string Nombre, string Relacion) Estructura(TipoOpcionStock tipo)
    {
        switch (tipo)
        {
            case TipoOpcionStock.Proveedor: return ("Proveedor", "id_proveedor", "nombre_proveedor", "Ingreso_Stock");
            case TipoOpcionStock.Categoria: return ("Categoria", "id_categoria", "nombre_categoria", "Producto");
            case TipoOpcionStock.Marca: return ("Marca", "id_marca", "nombre_marca", "Producto");
            case TipoOpcionStock.Color: return ("Variante_Producto", "id_variante", "color", "Variante_Producto");
            default: throw new ArgumentException("Seleccione un tipo de opción válido.");
        }
    }

    // cuenta productos o ingresos relacionados y agrupa colores que se guardan como texto en las variantes
    public List<OpcionStock> Obtener(TipoOpcionStock tipo)
    {
        (string tabla, string id, string nombre, string relacion) = Estructura(tipo);
        string sql;
        if (tipo == TipoOpcionStock.Color)
            sql = "SELECT 0, UPPER(LTRIM(RTRIM(color))), COUNT(*) FROM dbo.Variante_Producto WHERE NULLIF(LTRIM(RTRIM(color)),N'') IS NOT NULL GROUP BY UPPER(LTRIM(RTRIM(color))) ORDER BY 2";
        else
            sql = $"SELECT o.{id}, o.{nombre}, (SELECT COUNT(*) FROM dbo.{relacion} r WHERE r.{id}=o.{id}) FROM dbo.{tabla} o ORDER BY o.{nombre},o.{id}";
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        using SqlCommand comando = new SqlCommand(sql, conexion);
        conexion.Open();
        using SqlDataReader lector = comando.ExecuteReader();
        List<OpcionStock> opciones = new List<OpcionStock>();
        while (lector.Read()) opciones.Add(new OpcionStock { Id = lector.GetInt32(0), Nombre = lector.GetString(1), Usos = lector.GetInt32(2) });
        return opciones;
    }

    // confirma la correccion o unificacion junto con su actividad de historial
    public void Guardar(TipoOpcionStock tipo, OpcionStock opcion, string nuevoNombre, int? destino, bool eliminar, int usuario)
    {
        (string tabla, string id, string nombre, string relacion) = Estructura(tipo);
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        try
        {
            // bloquea las opciones mientras comprueba sus relaciones y aplica el cambio
            using SqlCommand bloqueo = new SqlCommand($"SELECT COUNT(*) FROM dbo.{tabla} WITH (TABLOCKX,HOLDLOCK)", conexion, transaccion);
            bloqueo.ExecuteScalar();
            using SqlCommand comando = new SqlCommand("", conexion, transaccion);
            comando.Parameters.Add("@id", SqlDbType.Int).Value = opcion.Id;
            comando.Parameters.Add("@anterior", SqlDbType.NVarChar, 100).Value = opcion.Nombre;
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nuevoNombre;
            comando.Parameters.Add("@destino", SqlDbType.Int).Value = (object?)destino ?? DBNull.Value;
            string filtro = $"{id}=@id AND {nombre}=@anterior";
            if (tipo == TipoOpcionStock.Color) filtro = "UPPER(LTRIM(RTRIM(color)))=UPPER(@anterior)";
            comando.CommandText = $"SELECT COUNT(*) FROM dbo.{tabla} WHERE {filtro}";
            if (Convert.ToInt32(comando.ExecuteScalar()) == 0) throw new InvalidOperationException("La opción cambió o ya no existe. Actualice la lista.");
            string accion = "MODIFICAR_OPCION_STOCK";
            if (eliminar)
            {
                // un color existe porque alguna variante lo usa por eso se corrige o unifica en lugar de borrar esa variante
                if (tipo == TipoOpcionStock.Color) throw new InvalidOperationException("Este color está utilizado en variantes. Corríjalo o unifíquelo con otro color.");
                comando.CommandText = $"IF EXISTS(SELECT 1 FROM dbo.{relacion} WITH (UPDLOCK,HOLDLOCK) WHERE {id}=@id) THROW 50001,N'La opción está en uso. Corrija su nombre o unifíquela con otra; no se puede eliminar.',1; DELETE dbo.{tabla} WHERE {filtro};";
                accion = "ELIMINAR_OPCION_STOCK";
            }
            else if (destino.HasValue)
            {
                comando.CommandText = $"SELECT {nombre} FROM dbo.{tabla} WHERE {id}=@destino AND {id}<>@id";
                object? valor = comando.ExecuteScalar();
                if (valor == null) throw new InvalidOperationException("Seleccione otra opción existente como destino.");
                nuevoNombre = Convert.ToString(valor)!;
                comando.CommandText = $"UPDATE dbo.{relacion} SET {id}=@destino WHERE {id}=@id; DELETE dbo.{tabla} WHERE {filtro};";
                accion = "UNIFICAR_OPCION_STOCK";
            }
            else
            {
                if (tipo != TipoOpcionStock.Color)
                {
                    comando.CommandText = $"SELECT COUNT(*) FROM dbo.{tabla} WHERE UPPER(LTRIM(RTRIM({nombre})))=UPPER(@nombre) AND {id}<>@id";
                    if (Convert.ToInt32(comando.ExecuteScalar()) > 0) throw new InvalidOperationException("Ya existe ese nombre. Use Unificar para conservar una sola opción.");
                }
                // en colores el cambio actualiza las variantes y permite corregir varios textos hacia un mismo color
                comando.CommandText = $"UPDATE dbo.{tabla} SET {nombre}=@nombre WHERE {filtro}";
            }
            comando.ExecuteNonQuery();
            using SqlCommand historial = new SqlCommand("INSERT dbo.LogActividad(id_usuario,accion,entidad,id_entidad,detalle) VALUES(@usuario,@accion,@entidad,@id,@detalle)", conexion, transaccion);
            historial.Parameters.AddWithValue("@usuario", usuario);
            historial.Parameters.AddWithValue("@accion", accion);
            historial.Parameters.AddWithValue("@entidad", tipo.ToString());
            historial.Parameters.AddWithValue("@id", opcion.Id);
            historial.Parameters.AddWithValue("@detalle", "Nombre anterior: " + opcion.Nombre + " | Nombre destino: " + nuevoNombre);
            historial.ExecuteNonQuery();
            transaccion.Commit();
        }
        catch { transaccion.Rollback(); throw; }
    }
}
