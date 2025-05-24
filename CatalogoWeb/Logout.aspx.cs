using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogoWeb
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();     // Borra todas las variables de sesión
            Session.Abandon();   // Finaliza la sesión del usuario

            Response.Redirect("Default.aspx"); // Redirige al inicio
        }
    }
}