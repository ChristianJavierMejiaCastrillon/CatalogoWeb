<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Marcas.aspx.cs" Inherits="CatalogoWeb.Admin.Marcas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Administración de Marcas</h2>

        <div class="card my-3">
            <div class="card-body">
                <div class="row g-2">
                    <div class="col-md-6">
                        <asp:TextBox ID="txtNuevaMarca" runat="server" CssClass="form-control"
                            MaxLength="100" placeholder="Nombre de la marca..." />
                    </div>
                    <div class="col-auto">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar"
                            CssClass="btn btn-primary" OnClick="btnAgregar_Click" />
                    </div>
                </div>
            </div>
        </div>


        <asp:GridView ID="gvMarcas" runat="server" AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped" DataKeyNames="Id"
            OnRowCommand="gvMarcas_RowCommand">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />

                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEliminar" runat="server"
                            Text="Eliminar"
                            CssClass="btn btn-sm btn-outline-danger"
                            CommandName="Eliminar"
                            CommandArgument='<%# Eval("Id") %>'
                            OnClientClick="return confirm('¿Deseas eliminar esta marca?');">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
