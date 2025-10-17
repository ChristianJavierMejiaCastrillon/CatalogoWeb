<%@ Page Title="Nuevo Producto" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="NuevoProducto.aspx.cs"
    Inherits="CatalogoWeb.Admin.NuevoProducto" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Nuevo Producto</h2>
        <hr />

        <asp:ValidationSummary ID="vs1" runat="server" CssClass="alert alert-danger" HeaderText="Por favor corrige:" />

        <!-- INICIO DE LA FILA DE CAMPOS -->
        <div class="row g-3">
            <!-- Código -->
            <div class="col-md-4">
                <label class="form-label">Código</label>
                <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCodigo"
                    ErrorMessage="El código es obligatorio" CssClass="text-danger" />
            </div>

            <!-- Nombre -->
            <div class="col-md-8">
                <label class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
                    ErrorMessage="El nombre es obligatorio" CssClass="text-danger" />
            </div>

            <!-- Precio -->
            <div class="col-md-4">
                <label class="form-label">Precio</label>
                <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPrecio"
                    ErrorMessage="El precio es obligatorio" CssClass="text-danger" />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPrecio"
                    ValidationExpression="^\s*(\d{1,3}([.,]\d{3})+([.,]\d{1,2})?|\d+([.,]\d{1,2})?)\s*$"
                    ErrorMessage="Formato numérico (ej: 12.345,67 o 12345.67)" CssClass="text-danger" />
            </div>

            <!-- Categoría -->
            <div class="col-md-4">
                <label class="form-label">Categoría</label>
                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                    <asp:ListItem Text="-- Seleccione --" Value="" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCategoria" InitialValue=""
                    ErrorMessage="Seleccione una categoría" CssClass="text-danger" />
            </div>

            <!-- Marca -->
            <div class="col-md-4">
                <label class="form-label">Marca</label>
                <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                    <asp:ListItem Text="-- Seleccione --" Value="" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlMarca" InitialValue=""
                    ErrorMessage="Seleccione una marca" CssClass="text-danger" />
            </div>

            <!-- Descripción -->
            <div class="col-12">
                <label class="form-label">Descripción</label>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
            </div>

            <!-- Imagen (archivo y URL) más juntas -->
            <div class="row g-1 align-items-start">
                <%-- g-2 reduce el espacio entre columnas --%>
                <div class="col-md-4">
                    <label class="form-label">Imagen (archivo)</label>
                    <asp:FileUpload ID="fuImagen" runat="server" CssClass="form-control" />
                    <small class="text-muted d-block mt-1">Opcional. También puedes usar una URL.</small>
                </div>

                <div class="col-md-5">
                    <label class="form-label">Imagen (URL)</label>
                    <asp:TextBox ID="txtImagenUrl" runat="server" CssClass="form-control" />
                </div>
                <!-- Quedan 3 columnas libres al final (5 + 4 = 9), lo que acerca la segunda columna a la primera -->
            </div>
        </div>
            <!-- FIN DE LA FILA DE CAMPOS -->

            <hr />
            <asp:Label ID="lblMsg" runat="server" CssClass="d-block mt-2"></asp:Label>

            <div class="mt-3">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success"
                    OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary ms-2"
                    OnClick="btnCancelar_Click" CausesValidation="false" />
            </div>
        </div>
</asp:Content>

