using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;

namespace CatalogoWeb
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Cierra Forms Authentication
            FormsAuthentication.SignOut();

            // Limpia la sesión
            Session.Clear();
            Session.Abandon();

            // Redirige al inicio
            Response.Redirect("~/Default.aspx");
        }
    }
}
