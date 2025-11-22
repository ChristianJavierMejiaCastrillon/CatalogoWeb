using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogoWeb.Admin
{
    public partial class NuevoProducto : System.Web.UI.Page
    {
        // --- Conexión desde web.config ---
        private readonly string cnx = ConfigurationManager.ConnectionStrings["CATALOGO_WEB_DB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1) Debe estar autenticado
            if (!Request.IsAuthenticated)
            {
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                return;
            }

            // 2) Debe ser admin
            bool isAdmin =
                (Session["IsAdmin"] is bool b && b) ||
                (Session["Admin"] is bool c && c);

            if (!isAdmin)
            {
                // Si no es admin, lo mandamos al inicio
                Response.Redirect("~/Default.aspx");
                return;
            }

            // 3) Solo la primera vez se cargan combos
            if (!IsPostBack)
            {
                CargarCategorias();
                CargarMarcas();
            }
        }

        // --- Cargar Categorías ---
        private void CargarCategorias()
        {
            using (SqlConnection con = new SqlConnection(cnx))
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT Id, Descripcion FROM CATEGORIAS ORDER BY Descripcion", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlCategoria.DataSource = dt;
                ddlCategoria.DataTextField = "Descripcion";
                ddlCategoria.DataValueField = "Id";
                ddlCategoria.DataBind();
                ddlCategoria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione --", ""));
            }
        }

        // --- Cargar Marcas ---
        private void CargarMarcas()
        {
            using (SqlConnection con = new SqlConnection(cnx))
            using (SqlDataAdapter da = new SqlDataAdapter(
                "SELECT Id, Descripcion FROM MARCAS WHERE Activo = 1 ORDER BY Descripcion", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlMarca.DataSource = dt;
                ddlMarca.DataTextField = "Descripcion";
                ddlMarca.DataValueField = "Id";
                ddlMarca.DataBind();
                ddlMarca.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione --", ""));
            }
        }

        // --- Botones ---
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            // Limpiar estado visual de errores antes de validar
            LimpiarErroresCampos();
            lblMsg.Text = "";
            lblMsg.CssClass = "d-block mt-2";

            // 1) Validaciones mínimas de servidor (por si desactivan JS)

            //validacion codigo
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MarcarCodigoInvalido("El código es obligatorio.");
                ShowModal("Error", "El código es obligatorio.");
                return;
            }
            //validacion nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MarcarNombreInvalido("El nombre es obligatorio.");
                ShowModal("Error", "El nombre es obligatorio.");
                return;
            }
            //validacion categoria
            if (string.IsNullOrEmpty(ddlCategoria.SelectedValue))
            {
                MarcarCategoriaInvalida("Seleccione una categoría.");
                ShowModal("Error", "Seleccione una categoría.");
                return;
            }
            //validacion marca
            if (string.IsNullOrEmpty(ddlMarca.SelectedValue))
            {
                MarcarMarcaInvalida("Seleccione una marca.");
                ShowModal("Error", "Seleccione una marca.");
                return;
            }

            // 2) Parseo de precio (acepta , o .)
            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MarcarPrecioInvalido("El precio es obligatorio.");
                ShowModal("Error", "El precio es obligatorio.");
                return;
            }

            if (!TryParsePrecio(txtPrecio.Text, out decimal precio))
            {
                MarcarPrecioInvalido("Precio inválido. Use 1234,56 o 1234.56");
                ShowModal("Error", "Precio inválido. Use 1234,56 o 1234.56");
                return;
            }

            // 3) Validar duplicado por Código
            if (ExisteCodigo(txtCodigo.Text.Trim()))
            {
                MarcarCodigoInvalido("El código ya existe. Ingrese uno diferente.");
                ShowModal("Error", "El código ya existe. Ingrese uno diferente.");
                return;
            }

            // 4) Determinar ruta de imagen (archivo subido o URL)
            string imagenUrl = null;

            // a) Archivo subido
            if (fuImagen.HasFile)
            {
                try
                {
                    // Carpeta de destino (puedes cambiar a ~/Content/Uploads o similar)
                    string folderVirtual = "~/Uploads";
                    string folderFisico = Server.MapPath(folderVirtual);

                    if (!Directory.Exists(folderFisico))
                        Directory.CreateDirectory(folderFisico);

                    string ext = Path.GetExtension(fuImagen.FileName);
                    // Nombre único para evitar colisiones
                    string fileName = $"prod_{DateTime.Now:yyyyMMdd_HHmmssfff}{ext}";
                    string rutaFisica = Path.Combine(folderFisico, fileName);

                    fuImagen.SaveAs(rutaFisica);

                    imagenUrl = ResolveUrl($"{folderVirtual}/{fileName}"); // guardamos la ruta virtual
                }
                catch (Exception ex)
                {
                    ShowMsg("No se pudo guardar la imagen del producto. " + ex.Message, isError: true);
                    return;
                }
            }
            // b) URL escrita
            else if (!string.IsNullOrWhiteSpace(txtImagenUrl.Text))
            {
                imagenUrl = txtImagenUrl.Text.Trim();
            }

            // 5) INSERT a la base de datos
            try
            {
                using (var con = new SqlConnection(cnx))
                using (var cmd = new SqlCommand(@"
                    INSERT INTO dbo.ARTICULOS
                    (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)
                    VALUES
                    (@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @ImagenUrl, @Precio);
                ", con))
                {
                    cmd.Parameters.AddWithValue("@Codigo", txtCodigo.Text.Trim());
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Descripcion", (object)(txtDescripcion.Text?.Trim() ?? "") ?? DBNull.Value);

                    // Muy importante: enviar INT para las FK
                    cmd.Parameters.Add("@IdMarca", System.Data.SqlDbType.Int).Value = Convert.ToInt32(ddlMarca.SelectedValue);
                    cmd.Parameters.Add("@IdCategoria", System.Data.SqlDbType.Int).Value = Convert.ToInt32(ddlCategoria.SelectedValue);

                    cmd.Parameters.AddWithValue("@ImagenUrl", (object)imagenUrl ?? DBNull.Value);

                    // Precio al final (da igual el orden de parámetros, pero lo dejamos claro)
                    cmd.Parameters.AddWithValue("@Precio", precio);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        // Antes usábamos Redirect; ahora mostramos modal de éxito
                        ShowModal("Éxito", "Producto guardado correctamente ✅");

                        // Limpiar el formulario para que quede listo para otro producto
                        txtCodigo.Text = "";
                        txtNombre.Text = "";
                        txtDescripcion.Text = "";
                        txtPrecio.Text = "";
                        txtImagenUrl.Text = "";
                        ddlCategoria.SelectedIndex = 0;
                        ddlMarca.SelectedIndex = 0;
                    }
                    else
                    {
                        ShowMsg("No se pudo guardar el producto. Intenta de nuevo.", isError: true);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg("Error al guardar en la base de datos: " + ex.Message, isError: true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Productos.aspx");
        }

        private void ShowMsg(string texto, bool isError)
        {
            lblMsg.CssClass = isError ? "text-danger d-block mt-2" : "text-success d-block mt-2";
            lblMsg.Text = texto;
        }

        private bool TryParsePrecio(string input, out decimal value)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                value = 0m;
                return false;
            }

            // Reemplaza coma por punto y usa InvariantCulture
            string normalized = input.Trim().Replace(',', '.');
            return decimal.TryParse(
                normalized,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
                CultureInfo.InvariantCulture,
                out value);
        }
        private void LimpiarErroresCampos()
        {
            // Limpia estado visual 
            // Código
            if (txtCodigo != null)
            {
                txtCodigo.CssClass = txtCodigo.CssClass
                    .Replace(" is-invalid", "")
                    .Replace(" is-valid", "");
                litCodigoError.Text = string.Empty;
            }

            // Nombre
            if (txtNombre != null)
            {
                txtNombre.CssClass = txtNombre.CssClass
                    .Replace(" is-invalid", "")
                    .Replace(" is-valid", "");
                litNombreError.Text = string.Empty;
            }

            // Precio
            if (txtPrecio != null)
            {
                txtPrecio.CssClass = txtPrecio.CssClass
                    .Replace(" is-invalid", "")
                    .Replace(" is-valid", "");
                litPrecioError.Text = string.Empty;
            }
            // Categoría
            if (ddlCategoria != null)
            {
                ddlCategoria.CssClass = ddlCategoria.CssClass
                    .Replace(" is-invalid", "")
                    .Replace(" is-valid", "");
                litCategoriaError.Text = string.Empty;
            }

            // Marca
            if (ddlMarca != null)
            {
                ddlMarca.CssClass = ddlMarca.CssClass
                    .Replace(" is-invalid", "")
                    .Replace(" is-valid", "");
                litMarcaError.Text = string.Empty;
            }
        }

        private void MarcarCodigoInvalido(string mensaje)
        {
            if (!txtCodigo.CssClass.Contains("is-invalid"))
                txtCodigo.CssClass += " is-invalid";

            // Mensaje pequeño debajo del campo
            litCodigoError.Text = $"<div class='invalid-feedback d-block'>{HttpUtility.HtmlEncode(mensaje)}</div>";
        }
        private void MarcarNombreInvalido(string mensaje)
        {
            if (!txtNombre.CssClass.Contains("is-invalid"))
                txtNombre.CssClass += " is-invalid";

            litNombreError.Text =
                $"<div class='invalid-feedback d-block'>{HttpUtility.HtmlEncode(mensaje)}</div>";
        }
        private void MarcarPrecioInvalido(string mensaje)
        {
            if (!txtPrecio.CssClass.Contains("is-invalid"))
                txtPrecio.CssClass += " is-invalid";

            litPrecioError.Text =
                $"<div class='invalid-feedback d-block'>{HttpUtility.HtmlEncode(mensaje)}</div>";
        }
        private void MarcarCategoriaInvalida(string mensaje)
        {
            if (!ddlCategoria.CssClass.Contains("is-invalid"))
                ddlCategoria.CssClass += " is-invalid";

            litCategoriaError.Text =
                $"<div class='invalid-feedback d-block'>{HttpUtility.HtmlEncode(mensaje)}</div>";
        }

        private void MarcarMarcaInvalida(string mensaje)
        {
            if (!ddlMarca.CssClass.Contains("is-invalid"))
                ddlMarca.CssClass += " is-invalid";

            litMarcaError.Text =
                $"<div class='invalid-feedback d-block'>{HttpUtility.HtmlEncode(mensaje)}</div>";
        }

        private bool ExisteCodigo(string codigo)
        {
            try
            {
                using (var con = new SqlConnection(cnx))
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.ARTICULOS WHERE Codigo = @Codigo", con))
                {
                    cmd.Parameters.AddWithValue("@Codigo", codigo);
                    con.Open();

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0; // Devuelve true solo si ya existe
                }
            }
            catch (Exception ex)
            {
                ShowMsg("Error al validar código: " + ex.Message, isError: true);
                return false; // Si hay error, no bloquear el guardado
            }
        }
        private void ShowModal(string titulo, string mensaje)
        {
            string safeTitle = HttpUtility.JavaScriptStringEncode(titulo ?? "Mensaje");
            string safeBody = HttpUtility.JavaScriptStringEncode(mensaje ?? "");

            string js = $"setTimeout(function(){{ showAppMessage('{safeTitle}','{safeBody}'); }}, 150);";

            ScriptManager.RegisterStartupScript(
                Page,                          // página actual
                Page.GetType(),                // tipo
                Guid.NewGuid().ToString(),     // key única
                js,                            // script a ejecutar
                true                           // que agregue <script> ... </script>
            );
        }
    }

}