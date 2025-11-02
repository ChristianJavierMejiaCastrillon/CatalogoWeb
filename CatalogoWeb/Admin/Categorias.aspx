<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categorias.aspx.cs" Inherits="CatalogoWeb.Admin.Categorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Administración de Categorías</h2>

        <asp:PlaceHolder ID="phAlert" runat="server" />

        <div class="card my-3">
            <div class="card-body">
                <div class="row g-2">
                    <div class="col-md-6">
                        <asp:TextBox ID="txtNuevaCategoria" runat="server" CssClass="form-control"
                            MaxLength="100" placeholder="Nombre de la categoría..." />
                    </div>
                    <div class="col-auto">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar"
                            CssClass="btn btn-primary" OnClick="btnAgregar_Click" />
                    </div>
                </div>
            </div>
        </div>

        <asp:GridView ID="gvCategorias" runat="server" AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped" DataKeyNames="Id"
            OnRowCommand="gvCategorias_RowCommand">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />

                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar"
                            CssClass="btn btn-sm btn-outline-danger"
                            CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'
                            OnClientClick="return confirm('¿Deseas eliminar esta categoría?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
