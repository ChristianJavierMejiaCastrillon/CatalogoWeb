using Datos;
using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;
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
                using (SqlConnection conexion = new SqlConnection(@"Data Source=DESKTOP-BMT25TC\SQLEXPRESS;Initial Catalog=CATALOGO_WEB_DB;Integrated Security=True;Encrypt=False;MultipleActiveResultSets=True"))

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

        protected void gvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductos.PageIndex = e.NewPageIndex;
            CargarProductos(); // Recarga los productos con la nueva página seleccionada
        }
        protected string FormatearDescripcion(object descripcionObj)
        {
            if (descripcionObj == null)
                return string.Empty;

            string desc = descripcionObj.ToString();
            if (string.IsNullOrWhiteSpace(desc))
                return string.Empty;

            var lineas = desc.Split(
                new[] { "\r\n", "\n" },
                StringSplitOptions.RemoveEmptyEntries
            );

            if (lineas.Length == 0)
                return string.Empty;

            var sb = new StringBuilder();
            sb.Append("<ul class='mb-0 ps-3'>");

            foreach (var linea in lineas)
            {
                sb.Append("<li>");
                // Por seguridad, codificamos el texto
                sb.Append(Server.HtmlEncode(linea));
                sb.Append("</li>");
            }

            sb.Append("</ul>");
            return sb.ToString();
        }
    }
}
