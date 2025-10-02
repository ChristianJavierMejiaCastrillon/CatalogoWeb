<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="CatalogoWeb.Admin.Productos" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Panel de Administración - Productos</h2>
        <hr />

        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" />

        <asp:GridView ID="gvAdminProductos" runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped"
            DataKeyNames="Codigo"
            OnRowEditing="gvAdminProductos_RowEditing"
            OnRowCancelingEdit="gvAdminProductos_RowCancelingEdit"
            OnRowUpdating="gvAdminProductos_RowUpdating"
            OnRowDataBound="gvAdminProductos_RowDataBound"
            OnRowCommand="gvAdminProductos_RowCommand">
            <Columns>
                <%-- Clave --%>
                <asp:BoundField DataField="Codigo" HeaderText="Código" ReadOnly="True" />

                <%-- Nombre --%>
                <asp:TemplateField HeaderText="Nombre">
                    <ItemTemplate><%# Eval("Nombre") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" Text='<%# Bind("Nombre") %>' />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
                            ErrorMessage="Nombre requerido" Display="Dynamic" CssClass="text-danger" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <%-- Descripcion --%>
                <asp:TemplateField HeaderText="Descripción">
                    <ItemTemplate><%# Eval("Descripcion") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"
                            Text='<%# Bind("Descripcion") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <%-- Marca --%>
                <asp:TemplateField HeaderText="Marca">
                    <ItemTemplate><%# Eval("MarcaNombre") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select"></asp:DropDownList>
                        <asp:HiddenField ID="hdMarcaIdActual" runat="server" Value='<%# Eval("MarcaId") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <%-- Categoria --%>
                <asp:TemplateField HeaderText="Categoría">
                    <ItemTemplate><%# Eval("CategoriaNombre") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select"></asp:DropDownList>
                        <asp:HiddenField ID="hdCategoriaIdActual" runat="server" Value='<%# Eval("CategoriaId") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <%-- Precio --%>
                <asp:TemplateField HeaderText="Precio">
                    <ItemTemplate><%# String.Format("{0:N2}", Eval("Precio")) %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control"
                            Text='<%# Bind("Precio","{0:N2}") %>' />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPrecio"
                            ErrorMessage="Precio requerido" Display="Dynamic" CssClass="text-danger"
                            ValidationGroup="GVEdit" />
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPrecio"
                            ValidationExpression="^\s*(\d{1,3}([.,]\d{3})+([.,]\d{1,2})?|\d+([.,]\d{1,2})?)\s*$"
                            ErrorMessage="Formato numérico (ej: 12.345,67 o 12345.67)"
                            Display="Dynamic" CssClass="text-danger"
                            ValidationGroup="GVEdit" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <%-- Imagen --%>
                <asp:TemplateField HeaderText="Imagen">
                    <ItemTemplate>
                        <asp:Image ID="imgP" runat="server"
                            ImageUrl='<%# ResolveUrl(Convert.ToString(Eval("ImagenUrl") ?? "")) %>'
                            Width="60" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <div class="mb-1">
                            <asp:FileUpload ID="fuImagen" runat="server" />
                        </div>
                        <div class="input-group input-group-sm">
                            <span class="input-group-text">URL</span>
                            <asp:TextBox ID="txtImagenUrl" runat="server" CssClass="form-control"
                                Text='<%# Bind("ImagenUrl") %>' />
                        </div>
                        <!-- Mantener ruta actual por si no se sube archivo ni se escribe URL -->
                        <asp:HiddenField ID="hdImagenActual" runat="server" Value='<%# Eval("ImagenUrl") %>' />
                        <small class="text-muted d-block">Sube un archivo o pega una URL.</small>
                    </EditItemTemplate>
                </asp:TemplateField>

                <%-- Acciones --%>

                <asp:TemplateField HeaderText="Acciones">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />

                    <ItemTemplate>
                        <div class="d-flex justify-content-center">
                            <asp:Button ID="btnEditar" runat="server" Text="Editar"
                                CommandName="Edit"
                                CssClass="btn btn-sm btn-primary me-2"
                                CausesValidation="false" />
                            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar"
                                CommandName="Eliminar"
                                CommandArgument='<%# Eval("Codigo") %>'
                                CssClass="btn btn-sm btn-danger"
                                CausesValidation="false" />
                        </div>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <div class="d-flex justify-content-center">
                            <asp:Button ID="btnActualizar" runat="server" Text="Actualizar"
                                CommandName="Update"
                                CssClass="btn btn-sm btn-success me-2"
                                ValidationGroup="GVEdit" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                                CommandName="Cancel"
                                CssClass="btn btn-sm btn-secondary"
                                CausesValidation="false" />
                        </div>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
