using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Dominio;

namespace Datos
{
    public class ArticuloDatos
    {
        private readonly string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=CATALOGO_WEB_DB;Integrated Security=True;Encrypt=False;MultipleActiveResultSets=True";


        public List<Articulo> Listar(string nombre = null, string marca = null, string categoria = null)
        {
            List<Articulo> lista = new List<Articulo>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();

                    using (SqlCommand comando = new SqlCommand("sp_ListarArticulos", conexion))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        // Agregar parámetros solo si tienen valor
                        if (!string.IsNullOrEmpty(nombre))
                            comando.Parameters.AddWithValue("@nombre", nombre);
                        else
                            comando.Parameters.AddWithValue("@nombre", DBNull.Value);

                        if (!string.IsNullOrEmpty(marca))
                            comando.Parameters.AddWithValue("@marca", marca);
                        else
                            comando.Parameters.AddWithValue("@marca", DBNull.Value);

                        if (!string.IsNullOrEmpty(categoria))
                            comando.Parameters.AddWithValue("@categoria", categoria);
                        else
                            comando.Parameters.AddWithValue("@categoria", DBNull.Value);

                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                Articulo articulo = new Articulo
                                {
                                    Id = Convert.ToInt32(lector["Id"]),
                                    Codigo = lector["Codigo"].ToString(),
                                    Nombre = lector["Nombre"].ToString(),
                                    Descripcion = lector["Descripcion"].ToString(),
                                    Marca = new Marca { Descripcion = lector["Marca"].ToString() },
                                    Categoria = new Categoria { Descripcion = lector["Categoria"].ToString() },
                                    ImagenUrl = lector["ImagenUrl"].ToString(),
                                    Precio = Convert.ToDecimal(lector["Precio"])
                                };

                                lista.Add(articulo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los productos: " + ex.Message);
            }
            return lista;
        }

        public Articulo ObtenerPorId(int id)
        {
            Articulo articulo = null;

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand("SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Descripcion AS Marca, C.Descripcion AS Categoria, A.ImagenUrl, A.Precio " +
                                                               "FROM ARTICULOS A " +
                                                               "INNER JOIN MARCAS M ON A.IdMarca = M.Id " +
                                                               "INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id " +
                                                               "WHERE A.Id = @id", conexion))
                    {
                        comando.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                articulo = new Articulo
                                {
                                    Id = Convert.ToInt32(lector["Id"]),
                                    Codigo = lector["Codigo"].ToString(),
                                    Nombre = lector["Nombre"].ToString(),
                                    Descripcion = lector["Descripcion"].ToString(),
                                    Marca = new Marca { Descripcion = lector["Marca"].ToString() },
                                    Categoria = new Categoria { Descripcion = lector["Categoria"].ToString() },
                                    ImagenUrl = lector["ImagenUrl"].ToString(),
                                    Precio = Convert.ToDecimal(lector["Precio"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el producto: " + ex.Message);
            }

            return articulo;
        }

        public void Eliminar(string codigo)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand("DELETE FROM ARTICULOS WHERE Codigo = @codigo", conexion))
                    {
                        comando.Parameters.AddWithValue("@codigo", codigo);
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el producto: " + ex.Message);
            }
        }

    }
}
