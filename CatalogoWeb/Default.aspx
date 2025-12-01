<%@ Page Title="Catálogo de Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CatalogoWeb.Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-center">Catálogo de Productos</h2>

    <style>
        .product-link {
            color: #0d6efd;
            font-weight: 600;
            text-decoration: none;
            transition: color 0.2s ease-in-out;
        }

            .product-link:hover {
                color: #0a58ca;
                text-decoration: underline;
            }
    </style>


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
    <asp:GridView ID="gvProductos" CssClass="table table-bordered table-striped text-center" runat="server"
        AutoGenerateColumns="False" AllowPaging="True" PageSize="10"
        OnPageIndexChanging="gvProductos_PageIndexChanging">

        <%-- Centrar encabezados en pagina de inicio --%>
        <HeaderStyle CssClass="text-center align-middle" />

        <Columns>
            <asp:BoundField DataField="Codigo" HeaderText="Código" />

            <%-- Se estiliza el link del nombre del producto --%>
            <asp:TemplateField HeaderText="Nombre">
                <%-- Centramos también el header --%>
                <HeaderStyle CssClass="text-center" />
                <ItemStyle CssClass="text-center" />

                <ItemTemplate>
                    <a href='DetalleProducto.aspx?id=<%# Eval("Id") %>' class="product-link">
                        <%# Eval("Nombre") %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>


            <%-- Se ponen saltos de linea a los textos de la descripcion --%>
            <asp:TemplateField HeaderText="Descripción">
                <%-- Se alinea texto a la izquierda --%>
                <ItemStyle CssClass="text-start" />
                <ItemTemplate>
                    <div class="small">
                        <asp:Literal ID="litDescripcion" runat="server"
                            Text='<%# FormatearDescripcion(Eval("Descripcion")) %>' />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Marca" HeaderText="Marca" />
            <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
            <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" HtmlEncode="false" />

            <asp:TemplateField HeaderText="Imagen">
                <ItemTemplate>
                    <asp:Image ID="imgProducto" runat="server"
                        ImageUrl='<%# ResolveUrl(Convert.ToString(Eval("ImagenUrl") ?? "")) %>'
                        Width="100" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
