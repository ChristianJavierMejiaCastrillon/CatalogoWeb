using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogoWeb
{
	public partial class Login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                using (SqlConnection conexion = new SqlConnection("server=POWER\\SQLEXPRESS; database=CATALOGO_WEB_DB; integrated security=true"))
                {
                    conexion.Open();
                    string query = "SELECT Id, Nombre, Apellido, Admin FROM USERS WHERE Email = @email AND Pass = @password";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Guardar datos en sesión
                                Session["IdUser"] = reader["Id"];
                                Session["Nombre"] = reader["Nombre"];
                                Session["Apellido"] = reader["Apellido"];
                                Session["Admin"] = reader["Admin"];

                                // Redirigir al admin
                                if ((bool)reader["Admin"])
                                    Response.Redirect("Admin/Productos.aspx");
                                else
                                    Response.Redirect("Default.aspx");
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
    }
}