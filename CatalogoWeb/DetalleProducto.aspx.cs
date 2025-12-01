using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Dominio;

namespace CatalogoWeb
{
	public partial class DetalleProducto : System.Web.UI.Page
	{
        private ArticuloDatos articuloDatos = new ArticuloDatos();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProducto();
            }
        }

        private void CargarProducto()
        {
            string idProducto = Request.QueryString["id"];
            if (string.IsNullOrEmpty(idProducto))
            {
                Response.Redirect("Default.aspx");
                return;
            }

            Articulo articulo = articuloDatos.ObtenerPorId(Convert.ToInt32(idProducto));
            if (articulo != null)
            {
                lblNombre.Text = articulo.Nombre;
                lblCodigo.Text = articulo.Codigo;
                // Descripción con saltos de línea
                if (!string.IsNullOrEmpty(articulo.Descripcion))
                {
                    // Dividir por líneas
                    var lineas = articulo.Descripcion
                        .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    // Convertir cada línea en <li> "Puntos de lista"
                    string listaHtml = "<ul class='mb-3'>";
                    foreach (var linea in lineas)
                    {
                        listaHtml += $"<li>{linea}</li>";
                    }
                    listaHtml += "</ul>";

                    lblDescripcion.Text = listaHtml;
                }
                else
                {
                    lblDescripcion.Text = "";
                }
                lblMarca.Text = articulo.Marca.Descripcion;
                lblCategoria.Text = articulo.Categoria.Descripcion;
                lblPrecio.Text = articulo.Precio.ToString("C");
                imgProducto.ImageUrl = articulo.ImagenUrl;
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
}