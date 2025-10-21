using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Datos
{
    public class AccesoMarcas
    {
        private string Cnx => ConfigurationManager.ConnectionStrings["CATALOGO_WEB_DB"].ConnectionString;

        // Listar solo marcas activas
        public DataTable Listar()
        {
            using (var cn = new SqlConnection(Cnx))
            using (var da = new SqlDataAdapter(
                "SELECT Id, Descripcion AS Nombre FROM MARCAS WHERE Activo = 1 ORDER BY Descripcion;", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        // ¿Existe una marca ACTIVA con ese nombre?
        public bool ExisteActiva(string nombre)
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand(
                "SELECT 1 FROM MARCAS WHERE Descripcion = @n AND Activo = 1", cn))
            {
                cmd.Parameters.AddWithValue("@n", nombre.Trim());
                cn.Open();
                return cmd.ExecuteScalar() != null;
            }
        }

        // Crear una marca nueva y devolver el Id
        public int Agregar(string nombre)
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand(
                "INSERT INTO MARCAS(Descripcion, Activo) OUTPUT INSERTED.Id VALUES(@n, 1)", cn))
            {
                cmd.Parameters.AddWithValue("@n", nombre.Trim());
                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void Eliminar(int id)
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand("UPDATE MARCAS SET Activo = 0 WHERE Id = @Id;", cn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool TieneArticulos(int idMarca)
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand(
                "SELECT TOP 1 1 FROM ARTICULOS WHERE IdMarca = @id", cn))
            {
                cmd.Parameters.AddWithValue("@id", idMarca);
                cn.Open();
                var r = cmd.ExecuteScalar();
                return r != null; // true = tiene al menos un artículo
            }
        }

        // Alta inteligente: si existe inactiva, la reactiva; si no existe, la crea.
        // Si ya existe ACTIVA, lanza InvalidOperationException.
        public int AgregarOReactivar(string nombre)
        {
            if (ExisteActiva(nombre))
                throw new InvalidOperationException("Ya existe una marca activa con ese nombre.");

            var idExistente = BuscarIdPorNombre(nombre);
            if (idExistente.HasValue)
            {
                Reactivar(idExistente.Value);
                return idExistente.Value;
            }
            else
            {
                return Agregar(nombre);
            }
        }

        // Reactivar una marca inactiva
        public void Reactivar(int id)
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand(
                "UPDATE MARCAS SET Activo = 1 WHERE Id = @id", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Si existe por nombre (activa o inactiva), devuelve su Id; si no, null
        public int? BuscarIdPorNombre(string nombre)
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand(
                "SELECT Id FROM MARCAS WHERE Descripcion = @n", cn))
            {
                cmd.Parameters.AddWithValue("@n", nombre.Trim());
                cn.Open();
                var r = cmd.ExecuteScalar();
                return r == null ? (int?)null : Convert.ToInt32(r);
            }
        }
    }
}
