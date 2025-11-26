<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs" Inherits="CatalogoWeb.DetalleProducto" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">

        <!-- Título general con espacio -->
        <h2 class="text-center mb-4">Detalle del Producto</h2>

        <div class="row">

            <!-- Imagen -->
            <div class="col-md-6 mb-4">
                <asp:Image ID="imgProducto" CssClass="img-fluid rounded" runat="server" />
            </div>

            <!-- Información -->
            <div class="col-md-6">

                <!-- Nombre del producto-->
                <h3 class="fw-bold mb-3">
                    <asp:Label ID="lblNombre" runat="server"></asp:Label>
                </h3>

                <p class="mb-2"><strong>Código:</strong>
                    <asp:Label ID="lblCodigo" runat="server"></asp:Label></p>

                <p class="mb-2"><strong>Descripción:</strong>
                    <asp:Label ID="lblDescripcion" runat="server"></asp:Label></p>

                <p class="mb-2"><strong>Marca:</strong>
                    <asp:Label ID="lblMarca" runat="server"></asp:Label></p>

                <p class="mb-2"><strong>Categoría:</strong>
                    <asp:Label ID="lblCategoria" runat="server"></asp:Label></p>

                <p class="mb-1 text-muted"><strong>Precio</strong></p>
                <p class="fs-4 fw-bold mb-3">
                    <asp:Label ID="lblPrecio" runat="server"></asp:Label>
                </p>

                <a href="Default.aspx" class="btn btn-primary mt-2">Volver al catálogo</a>
            </div>

        </div>
    </div>

</asp:Content>

