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
                lblDescripcion.Text = articulo.Descripcion;
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