using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class AccesoCategorias
    {
        private string Cnx => ConfigurationManager.ConnectionStrings["CATALOGO_WEB_DB"].ConnectionString;

        // 1) Listar solo CATEGORÍAS activas
        public DataTable Listar()
        {
            using (var cn = new SqlConnection(Cnx))
            using (var da = new SqlDataAdapter(
                "SELECT Id, Descripcion AS Nombre FROM CATEGORIAS WHERE Activo = 1 ORDER BY Descripcion;", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // 2) Saber si una categoría tiene artículos asociados (activos)
        public bool TieneArticulos(int idCategoria)
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand(
                "SELECT TOP 1 1 FROM ARTICULOS WHERE IdCategoria = @id AND (IdCategoria IS NOT NULL)", cn))
            {
                cmd.Parameters.AddWithValue("@id", idCategoria);
                cn.Open();
                return cmd.ExecuteScalar() != null;
            }
        }

        // 3) Soft delete: desactivar
        public void Eliminar(int id)
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand("UPDATE CATEGORIAS SET Activo = 0 WHERE Id = @Id;", cn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // 4) Alta inteligente (crear o reactivar)

        // ¿Existe una categoría ACTIVA con ese nombre?
        public bool ExisteActiva(string nombre)
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand(
                "SELECT 1 FROM CATEGORIAS WHERE Descripcion = @n AND Activo = 1", cn))
            {
                cmd.Parameters.AddWithValue("@n", nombre.Trim());
                cn.Open();
                return cmd.ExecuteScalar() != null;
            }
        }

        // Buscar Id por nombre (activa o inactiva)
        public int? BuscarIdPorNombre(string nombre)
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand(
                "SELECT Id FROM CATEGORIAS WHERE Descripcion = @n", cn))
            {
                cmd.Parameters.AddWithValue("@n", nombre.Trim());
                cn.Open();
                var r = cmd.ExecuteScalar();
                return r == null ? (int?)null : Convert.ToInt32(r);
            }
        }

        // Reactivar inactiva
        public void Reactivar(int id)
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand(
                "UPDATE CATEGORIAS SET Activo = 1 WHERE Id = @id", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Crear nueva y devolver Id
        public int Agregar(string nombre)
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand(
                "INSERT INTO CATEGORIAS(Descripcion, Activo) OUTPUT INSERTED.Id VALUES(@n, 1)", cn))
            {
                cmd.Parameters.AddWithValue("@n", nombre.Trim());
                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        // Crear o reactivar según corresponda
        public int AgregarOReactivar(string nombre)
        {
            if (ExisteActiva(nombre))
                throw new InvalidOperationException("Ya existe una categoría activa con ese nombre.");

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
    }
}
