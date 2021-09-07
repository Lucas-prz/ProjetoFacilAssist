<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WA_PortalCliente.Default" MasterPageFile="~/Site.Master" Title="FA -Casdastro de Clientes"%>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="MainUpPanel" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server">
                <table width="100%">
                    <tr>
                        <td align="center" class="caixaTitulo" colspan="4">
                            <asp:Label runat="server" ID="lblTitulo" Font-Bold="true" Font-Size="Medium">Cadastrar Clientes</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlForm" CssClass="painelForm" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td align="center" width="100%">
                                            <asp:Label runat="server" ID="lblAviso" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <h />
                                <table width="100%">
                                    <tr>
                                        <td width="90px">
                                            <asp:Label ID="lblCliente" runat="server">Cliente</asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:TextBox ID="txtCpf" runat="server" placeholder="CPF" Columns="10" MaxLength="11" OnTextChanged="txtCpf_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:TextBox ID="txtNome" runat="server" placeholder="Nome" Columns="30" MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td width="90px">
                                            <asp:Label ID="lblSexo" runat="server">Sexo</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSexo" runat="server" Columns="10">
                                                <asp:ListItem Value="M">Masculino</asp:ListItem>
                                                <asp:ListItem Value="F">Feminino</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTipo" runat="server">Tipo Cliente</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipo" runat="server" Columns="10">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStatus" runat="server">Status Cliente</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatus" runat="server" Columns="10">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="8">
                                            <asp:Button ID="btnGravar" runat="server" OnClick="btnGravar_Click" Text="Gravar" Width="80px" />
                                            &nbsp;
                                            <asp:Button ID="btnCancela" runat="server" OnClick="btnCancela_Click" Text="Cancelar" Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlGrid" CssClass="painelForm" Height="300px" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td width="100%">
                                            <asp:GridView ID="gvClientesCadastrados" runat="server" Width="98%" DataKeyNames="CPF"
                                                OnSelectedIndexChanged="gvClientesCadastrados_SelectedIndexChanged"
                                                OnRowDeleting="gvClientesCadastrados_RowDeleting" AutoGenerateColumns="false"
                                                AllowPaging="true" PageSize="5" OnPageIndexChanging="gvClientesCadastrados_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField DataTextField="CPF" HeaderText="CPF" CommandName="Select"/>
                                                    <asp:BoundField DataField="NOME" HeaderText="Nome"/>
                                                    <asp:BoundField DataField="SEXO" HeaderText="Sexo" />
                                                    <asp:BoundField DataField="TIPO" HeaderText="Tipo Cliente" />
                                                    <asp:BoundField DataField="STATUS" HeaderText="Situação" />
                                                    <asp:TemplateField HeaderText="Excluir" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="iBtmExcluir" ImageUrl="~/imagens/icon-delete.png" runat="server" CommandName="Delete"
                                                                ToolTip="Excluir" OnClientClick="return confirm('Tem certeza que deseja excluir o cliente?);" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <SelectedRowStyle BackColor="#FFFF99" />
                                                <HeaderStyle BackColor="SteelBlue" ForeColor="White" />
                                                <EmptyDataTemplate>
                                                    <asp:Label runat="server">Não há clientes cadastrados</asp:Label>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="MainUpPRog" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="OutPanel" CssClass="overlay" runat="server">
                <asp:Panel ID="InPanel" CssClass="loader" runat="server">
                    <span class="upPanelText">
                        <asp:Image ID="ImgLoading" runat="server" ImageUrl="~/imagens/loader.gif" Width="50px"/>
                        <span style="padding-left: 6px;">Processando...</span>
                    </span>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
