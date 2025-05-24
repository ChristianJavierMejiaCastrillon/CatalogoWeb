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
                // Aquí luego iría el código para cargar productos desde la base de datos
                // Por ahora podemos dejar un comentario temporal
                // Ejemplo: CargarListaDeProductos();
            }
        }
	}
}