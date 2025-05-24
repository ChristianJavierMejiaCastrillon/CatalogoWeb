<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="CatalogoWeb.Admin.Productos" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Panel de Administración - Productos</h2>
        <hr />

        <!-- Aquí más adelante pondremos el GridView para ver y administrar los productos -->
        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" />

    </div>
</asp:Content>
