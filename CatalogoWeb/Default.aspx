<%@ Page Title="Catálogo de Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CatalogoWeb.Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-center">Catálogo de Productos</h2>

    <!-- Filtros -->
    <div class="row mb-3">
        <div class="col-md-4">
            <asp:TextBox ID="txtFiltroNombre" CssClass="form-control" runat="server" Placeholder="Buscar por nombre..." />
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlFiltroMarca" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlFiltroCategoria" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnBuscar" CssClass="btn btn-primary" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
        </div>
    </div>

    <!-- GridView para mostrar los productos -->
    <asp:GridView ID="gvProductos" CssClass="table table-bordered table-striped" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="Codigo" HeaderText="Código" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
            <asp:BoundField DataField="Marca" HeaderText="Marca" />
            <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
            <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />
            <asp:TemplateField HeaderText="Imagen">
                <ItemTemplate>
                    <img src='<%# Eval("ImagenUrl") %>' alt="Imagen" style="width: 100px; height: auto;" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>

