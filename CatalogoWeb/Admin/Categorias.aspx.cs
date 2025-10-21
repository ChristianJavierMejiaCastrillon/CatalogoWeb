using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogoWeb.Admin
{
    public partial class Categorias : System.Web.UI.Page
    {
        private readonly AccesoCategorias _repo = new AccesoCategorias();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarCategorias();
        }

        private void CargarCategorias()
        {
            gvCategorias.DataSource = _repo.Listar(); // solo activas
            gvCategorias.DataBind();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            var nombre = txtNuevaCategoria.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarAlerta("El nombre de la categoría es obligatorio.", "danger");
                return;
            }

            try
            {
                int id = _repo.AgregarOReactivar(nombre);
                MostrarAlerta("Categoría guardada correctamente.", "success");
                txtNuevaCategoria.Text = "";
                CargarCategorias();
            }
            catch (InvalidOperationException)
            {
                MostrarAlerta("Ya existe una categoría activa con ese nombre.", "warning");
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error al guardar la categoría: " + ex.Message, "danger");
            }
        }

        protected void gvCategorias_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                if (_repo.TieneArticulos(id))
                {
                    MostrarAlerta("No se puede eliminar la categoría porque tiene productos asociados.", "danger");
                    return;
                }

                try
                {
                    _repo.Eliminar(id); // soft delete
                    MostrarAlerta("Categoría eliminada correctamente.", "success");
                    CargarCategorias();
                }
                catch (Exception ex)
                {
                    MostrarAlerta("Error al eliminar la categoría: " + ex.Message, "danger");
                }
            }
        }

        // Helper para alertas Bootstrap
        private void MostrarAlerta(string mensaje, string tipo)
        {
            string html = $@"<div class='alert alert-{tipo} alert-dismissible fade show' role='alert'>
                        {Server.HtmlEncode(mensaje)}
                        <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
                     </div>";
            phAlert.Controls.Clear();
            phAlert.Controls.Add(new System.Web.UI.LiteralControl(html));
        }

    }
}