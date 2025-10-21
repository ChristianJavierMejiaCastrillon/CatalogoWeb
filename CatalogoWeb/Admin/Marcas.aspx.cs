using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogoWeb.Admin
{
    public partial class Marcas : System.Web.UI.Page
    {
        private readonly AccesoMarcas _repo = new AccesoMarcas();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) CargarMarcas();
        }
        private void CargarMarcas()
        {
            gvMarcas.DataSource = _repo.Listar();
            gvMarcas.DataBind();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            var nombre = txtNuevaMarca.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarAlerta("El nombre de la marca es obligatorio.", "danger");
                return;
            }

            try
            {
                int id = _repo.AgregarOReactivar(nombre);
                MostrarAlerta("Marca guardada correctamente.", "success");
                txtNuevaMarca.Text = "";
                CargarMarcas();
            }
            catch (InvalidOperationException)
            {
                // Ya existe activa
                MostrarAlerta("Ya existe una marca con ese nombre.", "warning");
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error al guardar la marca: " + ex.Message, "danger");
            }
        }


        protected void gvMarcas_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int id = int.Parse(e.CommandArgument.ToString());

                // Verificar si tiene productos activos asociados
                if (_repo.TieneArticulos(id))
                {
                    MostrarAlerta("No se puede eliminar la marca porque tiene productos asociados.", "danger");
                    return;
                }

                try
                {
                    _repo.Eliminar(id);
                    MostrarAlerta("Marca eliminada correctamente (soft delete).", "success");
                    CargarMarcas();
                }
                catch (Exception ex)
                {
                    MostrarAlerta("Error al eliminar la marca: " + ex.Message, "danger");
                }
            }
        }

        // Método auxiliar para mostrar mensajes bootstrap
        private void MostrarAlerta(string mensaje, string tipo)
        {
            string script = $@"
        <div class='alert alert-{tipo} alert-dismissible fade show' role='alert'>
            {mensaje}
            <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
        </div>";
            Response.Write(script);
        }

    }
}