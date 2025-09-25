<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="CatalogoWeb.Admin.Productos" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Panel de Administración - Productos</h2>
        <hr />
        <asp:GridView ID="gvAdminProductos" runat="server" AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped"
            DataKeyNames="Codigo"
            OnRowCommand="gvAdminProductos_RowCommand">
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                <asp:BoundField DataField="Marca" HeaderText="Marca" />
                <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />
                <asp:TemplateField HeaderText="Imagen">
                    <ItemTemplate>
                        <img src='<%# Eval("ImagenUrl") %>' alt="Imagen" style="width: 100px;" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField ButtonType="Button" CommandName="Editar" Text="Editar" />
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:Button runat="server" Text="Eliminar"
                            CommandName="Eliminar"
                            CommandArgument='<%# Container.DataItemIndex %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <!-- Aquí más adelante pondremos el GridView para ver y administrar los productos -->
        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" />

    </div>
</asp:Content>
