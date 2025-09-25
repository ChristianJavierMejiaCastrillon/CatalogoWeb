using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogoWeb.Admin
{
	public partial class Productos : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            // 🔐 Paso 1: Verificar si el usuario está autenticado y es admin
            if (Session["IdUser"] == null || Session["Admin"] == null || !(bool)Session["Admin"])
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            // ✅ Paso 2: Ejecutar solo la primera vez que se carga la página
            if (!IsPostBack)
            {
                CargarProductos();
            }

        }
        
            private void CargarProductos()
        {
            ArticuloDatos datos = new ArticuloDatos();
            gvAdminProductos.DataSource = datos.Listar();
            gvAdminProductos.DataBind();
        }

        protected void gvAdminProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                // Obtener el índice de la fila
                int index = Convert.ToInt32(e.CommandArgument);

                // Obtener el código del producto desde la fila seleccionada
                string codigo = gvAdminProductos.DataKeys[index].Value.ToString();

                // Llamar al método para eliminar
                ArticuloDatos datos = new ArticuloDatos();
                datos.Eliminar(codigo);

                // Recargar el GridView
                CargarProductos();
            }
        }
    }
}