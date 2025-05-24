<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CatalogoWeb.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <h2 class="text-center">Iniciar Sesión</h2>
        <div class="row justify-content-center">
            <div class="col-md-4">
                <asp:Label runat="server" Text="Email" />
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                <br />
                <asp:Label runat="server" Text="Contraseña" />
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                <br />
                <asp:Button ID="btnLogin" runat="server" Text="Ingresar" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click" />
                <br />
                <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" />
            </div>
        </div>
    </div>
</asp:Content>
