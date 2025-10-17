using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
namespace CatalogoWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            try
            {
                string cnx = ConfigurationManager.ConnectionStrings["CATALOGO_WEB_DB"].ConnectionString;
                using (SqlConnection conexion = new SqlConnection(cnx))
                {
                    conexion.Open();
                    string query = @"SELECT Id, Nombre, Apellido, Admin
                             FROM USERS
                             WHERE Email = @email AND Pass = @password";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Flags de sesión
                                bool esAdmin = reader["Admin"] != DBNull.Value && (bool)reader["Admin"];

                                Session["IdUser"] = reader["Id"];
                                Session["Nombre"] = reader["Nombre"];
                                Session["Apellido"] = reader["Apellido"];
                                Session["Admin"] = esAdmin;      // si ya lo usabas
                                Session["IsAdmin"] = esAdmin;      // <-- NECESARIO para Productos.aspx

                                // Autentica en Forms (deja de ser "?" para /Admin)
                                FormsAuthentication.SetAuthCookie(email, false);

                                // Volver a la página pedida (si venías rebotado de /Admin)
                                string returnUrl = Request["ReturnUrl"] ?? Request["returnUrl"];
                                if (!string.IsNullOrEmpty(returnUrl) && UrlIsLocal(returnUrl))
                                {
                                    Response.Redirect(returnUrl, false);
                                    Context.ApplicationInstance.CompleteRequest();
                                    return;
                                }

                                // Redirección por rol
                                Response.Redirect(esAdmin ? "~/Admin/Productos.aspx" : "~/Default.aspx", false);
                                Context.ApplicationInstance.CompleteRequest();
                                return;
                            }
                            else
                            {
                                lblMensaje.Text = "Usuario o contraseña incorrectos.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al iniciar sesión: " + ex.Message;
            }
        }
        // Para evitar open-redirect; solo aceptamos rutas locales
        private bool UrlIsLocal(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            return url.StartsWith("/", StringComparison.Ordinal) ||
                   url.StartsWith("~/", StringComparison.Ordinal);
        }
      }
    }
