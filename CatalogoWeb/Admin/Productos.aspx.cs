using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace CatalogoWeb.Admin
{
    public partial class Productos : System.Web.UI.Page
    {
        private string Cnx => ConfigurationManager.ConnectionStrings["CATALOGO_WEB_DB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1) Debe estar autenticado por Forms (si no, al Login con ReturnUrl correcto)
            if (!Request.IsAuthenticated)
            {
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                return;
            }

            // 2) Debe ser admin (acepta cualquiera de los nombres por si tienes "Admin" o "IsAdmin")
            bool isAdmin =
                (Session["IsAdmin"] is bool b && b) ||
                (Session["Admin"] is bool c && c);

            if (!isAdmin)
            {
                // Autenticado pero NO admin -> fuera del /Admin
                Response.Redirect("~/Default.aspx");
                return;
            }

            // Necesario para <asp:FileUpload>
            Page.Form.Enctype = "multipart/form-data";

            if (!IsPostBack)
            {
                CargarProductos();

                // --- NUEVO: mostrar aviso si viene ?msg= ---
                var msg = Request.QueryString["msg"];
                lblAviso.Visible = false;

                if (!string.IsNullOrEmpty(msg))
                {
                    lblAviso.Visible = true;
                    switch (msg.ToLowerInvariant())
                    {
                        case "creado":
                            lblAviso.CssClass = "alert alert-success d-block";
                            lblAviso.Text = "✅ Producto creado correctamente.";
                            break;

                        case "actualizado":
                            lblAviso.CssClass = "alert alert-info d-block";
                            lblAviso.Text = "ℹ️ Producto actualizado.";
                            break;

                        case "eliminado":
                            lblAviso.CssClass = "alert alert-warning d-block";
                            lblAviso.Text = "⚠️ Producto eliminado.";
                            break;

                        default:
                            // Mensaje desconocido: no mostramos nada
                            lblAviso.Visible = false;
                            break;
                    }
                }
            }
        }

        private void CargarProductos()
        {
            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand(@"
        SELECT  a.Codigo,
                a.Nombre,
                a.Descripcion,
                a.IdMarca               AS MarcaId,          -- para el DropDown
                m.Descripcion           AS MarcaNombre,      -- ASPX puede Eval('MarcaNombre')
                a.IdCategoria           AS CategoriaId,      -- para el DropDown
                c.Descripcion           AS CategoriaNombre,  -- ASPX puede Eval('CategoriaNombre')
                a.Precio,
                a.ImagenUrl
        FROM dbo.ARTICULOS a
        INNER JOIN dbo.MARCAS     m ON a.IdMarca     = m.Id
        INNER JOIN dbo.CATEGORIAS c ON a.IdCategoria = c.Id
        ORDER BY a.Codigo;", cn))
            {
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    gvAdminProductos.DataSource = dr;
                    gvAdminProductos.DataBind();
                }
            }
        }

        private DataTable GetMarcas()
        {
            using (var cn = new SqlConnection(Cnx))
            using (var da = new SqlDataAdapter(
                "SELECT Id AS MarcaId, Descripcion AS Nombre FROM dbo.MARCAS ORDER BY Descripcion;", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private DataTable GetCategorias()
        {
            using (var cn = new SqlConnection(Cnx))
            using (var da = new SqlDataAdapter(
                "SELECT Id AS CategoriaId, Descripcion AS Nombre FROM dbo.CATEGORIAS ORDER BY Descripcion;", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        protected void gvAdminProductos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvAdminProductos.EditIndex = e.NewEditIndex;
            CargarProductos();
        }

        protected void gvAdminProductos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAdminProductos.EditIndex = -1;
            CargarProductos();
        }

        protected void gvAdminProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            // ¿La fila está en modo edición?
            bool esEdicion = (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit
                             || (e.Row.RowState & DataControlRowState.Alternate) == DataControlRowState.Alternate
                                && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit;
            if (!esEdicion) return;

            // --- Marca ---
            var ddlMarca = (DropDownList)e.Row.FindControl("ddlMarca");
            var hdMarcaIdActual = (HiddenField)e.Row.FindControl("hdMarcaIdActual");
            if (ddlMarca != null)
            {
                ddlMarca.DataSource = GetMarcas();
                ddlMarca.DataTextField = "Nombre";
                ddlMarca.DataValueField = "MarcaId";
                ddlMarca.DataBind();

                if (int.TryParse(hdMarcaIdActual?.Value, out int marcaActual))
                {
                    // Si el valor existe en la lista, lo selecciona
                    var item = ddlMarca.Items.FindByValue(marcaActual.ToString());
                    if (item != null) ddlMarca.SelectedValue = item.Value;
                }
            }

            // --- Categoría ---
            var ddlCategoria = (DropDownList)e.Row.FindControl("ddlCategoria");
            var hdCategoriaIdActual = (HiddenField)e.Row.FindControl("hdCategoriaIdActual");
            if (ddlCategoria != null)
            {
                ddlCategoria.DataSource = GetCategorias();
                ddlCategoria.DataTextField = "Nombre";
                ddlCategoria.DataValueField = "CategoriaId";
                ddlCategoria.DataBind();

                if (int.TryParse(hdCategoriaIdActual?.Value, out int catActual))
                {
                    var item = ddlCategoria.Items.FindByValue(catActual.ToString());
                    if (item != null) ddlCategoria.SelectedValue = item.Value;
                }
            }
        }

        protected void gvAdminProductos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            lblMensaje.Text = string.Empty;

            string codigo = gvAdminProductos.DataKeys[e.RowIndex].Value.ToString();

            var fila = gvAdminProductos.Rows[e.RowIndex];
            var txtNombre = (TextBox)fila.FindControl("txtNombre");
            var txtDescripcion = (TextBox)fila.FindControl("txtDescripcion");
            var ddlMarca = (DropDownList)fila.FindControl("ddlMarca");
            var ddlCategoria = (DropDownList)fila.FindControl("ddlCategoria");
            var txtPrecio = (TextBox)fila.FindControl("txtPrecio");
            var fuImagen = (FileUpload)fila.FindControl("fuImagen");
            var hdImagenActual = (HiddenField)fila.FindControl("hdImagenActual");
            var txtImagenUrl = (TextBox)fila.FindControl("txtImagenUrl");

            string nombre = txtNombre?.Text?.Trim() ?? "";
            string descripcion = txtDescripcion?.Text?.Trim() ?? "";

            if (!int.TryParse(ddlMarca?.SelectedValue, out int idMarca)) { lblMensaje.Text = "Selecciona una marca válida."; return; }
            if (!int.TryParse(ddlCategoria?.SelectedValue, out int idCategoria)) { lblMensaje.Text = "Selecciona una categoría válida."; return; }
            if (!TryParsePrecio(txtPrecio?.Text, out decimal precio)) { lblMensaje.Text = "Precio inválido. Usa 1234,56 o 1234.56"; return; }

            // --- IMAGEN: archivo > URL escrita > actual ---
            string imagenUrl = hdImagenActual?.Value ?? "";

            if (fuImagen != null && fuImagen.HasFile)
            {
                string ext = Path.GetExtension(fuImagen.FileName)?.ToLowerInvariant() ?? "";
                string[] ok = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
                if (!ok.Contains(ext)) { lblMensaje.Text = "Formato no permitido (JPG/PNG/WEBP/GIF/BMP)."; return; }

                string folderVirtual = "~/Imagenes"; // o "~/Uploads"
                string folderFisico = Server.MapPath(folderVirtual);
                if (!Directory.Exists(folderFisico)) Directory.CreateDirectory(folderFisico);

                string baseName = Path.GetFileNameWithoutExtension(fuImagen.FileName);
                if (string.IsNullOrWhiteSpace(baseName)) baseName = "img";
                string safeBaseName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars()));

                string nombreArchivo = $"{safeBaseName}_{codigo}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                string rutaFisica = Path.Combine(folderFisico, nombreArchivo);

                fuImagen.SaveAs(rutaFisica);
                imagenUrl = $"{folderVirtual}/{nombreArchivo}";
            }
            else if (!string.IsNullOrWhiteSpace(txtImagenUrl?.Text))
            {
                imagenUrl = txtImagenUrl.Text.Trim();
            }

            // NORMALIZAR: si es ruta local, forzar "~/" para que luego ResolveUrl funcione
            if (!string.IsNullOrWhiteSpace(imagenUrl) &&
                !imagenUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !imagenUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase) &&
                !imagenUrl.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                if (!imagenUrl.StartsWith("~/"))
                    imagenUrl = imagenUrl.StartsWith("/") ? "~" + imagenUrl : "~/" + imagenUrl;
            }

            using (var cn = new SqlConnection(Cnx))
            using (var cmd = new SqlCommand(@"
        UPDATE dbo.ARTICULOS
           SET Nombre      = @Nombre,
               Descripcion = @Descripcion,
               IdMarca     = @IdMarca,
               IdCategoria = @IdCategoria,
               Precio      = @Precio,
               ImagenUrl   = @ImagenUrl
         WHERE Codigo      = @Codigo;", cn))
            {
                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 200).Value = nombre;
                cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, -1).Value = (object)descripcion ?? DBNull.Value;
                cmd.Parameters.Add("@IdMarca", SqlDbType.Int).Value = idMarca;
                cmd.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = idCategoria;

                var p = cmd.Parameters.Add("@Precio", SqlDbType.Decimal);
                p.Precision = 18; p.Scale = 2; p.Value = precio;

                cmd.Parameters.Add("@ImagenUrl", SqlDbType.NVarChar, 300).Value = (object)imagenUrl ?? DBNull.Value;
                cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar, 50).Value = codigo;

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            // Importante: redirigir para evitar reenvío y mostrar el aviso
            Response.Redirect("~/Admin/Productos.aspx?msg=actualizado", endResponse: false);
            return;
        }


        protected void gvAdminProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                string codigo = e.CommandArgument?.ToString();
                if (string.IsNullOrWhiteSpace(codigo)) return;

                using (var cn = new SqlConnection(Cnx))
                using (var cmd = new SqlCommand("DELETE FROM dbo.ARTICULOS WHERE Codigo = @Codigo;", cn))
                {
                    cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar, 50).Value = codigo;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Redirige para mostrar aviso y evitar reenvío
                Response.Redirect("~/Admin/Productos.aspx?msg=eliminado", endResponse: false);
                return;
            }
        }

        private bool TryParsePrecio(string input, out decimal value)
        {
            value = 0m;
            if (string.IsNullOrWhiteSpace(input)) return false;

            input = input.Trim();
            var esCO = new System.Globalization.CultureInfo("es-CO");
            var enUS = new System.Globalization.CultureInfo("en-US");

            if (decimal.TryParse(input, System.Globalization.NumberStyles.Number, esCO, out value)) return true;
            if (decimal.TryParse(input, System.Globalization.NumberStyles.Number, enUS, out value)) return true;

            string alt = input.Replace(',', '.');
            return decimal.TryParse(alt, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out value);
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/NuevoProducto.aspx");
        }
    }
 }

