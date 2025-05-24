<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs" Inherits="CatalogoWeb.DetalleProducto" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="text-center">Detalle del Producto</h2>
        <div class="row">
            <div class="col-md-6">
                <asp:Image ID="imgProducto" CssClass="img-fluid" runat="server" />
            </div>
            <div class="col-md-6">
                <h3><asp:Label ID="lblNombre" runat="server"></asp:Label></h3>
                <p><strong>Código:</strong> <asp:Label ID="lblCodigo" runat="server"></asp:Label></p>
                <p><strong>Descripción:</strong> <asp:Label ID="lblDescripcion" runat="server"></asp:Label></p>
                <p><strong>Marca:</strong> <asp:Label ID="lblMarca" runat="server"></asp:Label></p>
                <p><strong>Categoría:</strong> <asp:Label ID="lblCategoria" runat="server"></asp:Label></p>
                <p><strong>Precio:</strong> <asp:Label ID="lblPrecio" runat="server"></asp:Label></p>
                <a href="Default.aspx" class="btn btn-primary">Volver al catálogo</a>
            </div>
        </div>
    </div>
</asp:Content>
