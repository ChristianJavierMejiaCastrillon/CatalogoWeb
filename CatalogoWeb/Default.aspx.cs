using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Datos;
using Dominio;
//using Negocio; // (La agregaremos más adelante cuando hagamos la capa de negocio)

namespace CatalogoWeb
{
    public partial class Default : System.Web.UI.Page
    {
        private ArticuloDatos articuloDatos = new ArticuloDatos(); // <-- Declaramos el objeto aquí
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarFiltros();
                CargarProductos();
            }
        }

        private void CargarFiltros()
        {
            // Limpiar listas antes de cargar datos
            ddlFiltroMarca.Items.Clear();
            ddlFiltroCategoria.Items.Clear();

            // Agregar opción "Todas" al inicio
            ddlFiltroMarca.Items.Add(new ListItem("Todas las Marcas", ""));
            ddlFiltroCategoria.Items.Add(new ListItem("Todas las Categorías", ""));

            try
            {
                using (SqlConnection conexion = new SqlConnection("server=POWER\\SQLEXPRESS; database=CATALOGO_WEB_DB; integrated security=true"))
                {
                    conexion.Open();

                    // Cargar Marcas
                    using (SqlCommand cmd = new SqlCommand("SELECT Descripcion FROM MARCAS", conexion))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ddlFiltroMarca.Items.Add(new ListItem(reader["Descripcion"].ToString(), reader["Descripcion"].ToString()));
                        }
                    }

                    // Cargar Categorías
                    using (SqlCommand cmd = new SqlCommand("SELECT Descripcion FROM CATEGORIAS", conexion))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ddlFiltroCategoria.Items.Add(new ListItem(reader["Descripcion"].ToString(), reader["Descripcion"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar los filtros: " + ex.Message);
            }
        }

        private void CargarProductos()
        {
            List<Articulo> listaProductos = articuloDatos.Listar();

            gvProductos.DataSource = listaProductos;
            gvProductos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombre = txtFiltroNombre.Text.Trim();
            string marca = ddlFiltroMarca.SelectedValue;
            string categoria = ddlFiltroCategoria.SelectedValue;

            List<Articulo> listaProductos = articuloDatos.Listar(nombre, marca, categoria);

            gvProductos.DataSource = listaProductos;
            gvProductos.DataBind();
        }
    }
}
