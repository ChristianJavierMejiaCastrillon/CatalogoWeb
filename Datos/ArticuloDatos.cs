using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Dominio;

namespace Datos
{
    public class ArticuloDatos
    {
        private string connectionString = "server=POWER\\SQLEXPRESS; database=CATALOGO_WEB_DB; integrated security=true";

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
    }
}
