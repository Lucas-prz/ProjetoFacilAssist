using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using WA_PortalCliente.Auxiliar;
using WA_PortalCliente.Interface;

namespace WA_PortalCliente
{
    public partial class Default : System.Web.UI.Page
    {
        private string login;
        private string senha;
        private string urlApi;

        protected void Page_Load(object sender, EventArgs e)
        {
            login = WebConfigurationManager.AppSettings["login"].ToString();
            senha = WebConfigurationManager.AppSettings["senha"].ToString();
            urlApi = WebConfigurationManager.AppSettings["urlApi"].ToString();

            if (!Page.IsPostBack)
            {
                CarregaTela();
            }
        }

        #region Métodos padrão
        protected void CarregaTela()
        {
            lblAviso.Text = string.Empty;
            var erro = string.Empty;
            var token = string.Empty;

            if (Request.Cookies["Token"] != null)
            {
                if ((Request.Cookies["Token"].Expires - DateTime.Now).TotalSeconds > 20)
                {
                    token = Request.Cookies["Token"].Value;
                }
                else
                {
                    var aut = new AutenticarApi();
                    aut.login = login;
                    aut.senha = senha;

                    var respToken = AuxHttp.PostJSON<RetornoAutenticar>(urlApi + "/api/Autenticar", aut, out erro);
                    if (respToken == null)
                    {
                        lblAviso.Text = "Erro ao na requisição :: " + erro;
                        lblAviso.ForeColor = Color.Red;
                        return;
                    }
                    if (!respToken.autenticado)
                    {
                        lblAviso.Text = respToken.mensagem;
                        if (!string.IsNullOrEmpty(erro))
                            lblAviso.Text += " :: " + erro;
                        lblAviso.ForeColor = Color.Red;
                        return;
                    }

                    Request.Cookies["Token"].Value = respToken.tokenAcesso;
                    Request.Cookies["Token"].Expires = respToken.expira;

                    token = respToken.tokenAcesso;
                }
            }
            else
            {
                var aut = new AutenticarApi();
                aut.login = login;
                aut.senha = senha;

                var respToken = AuxHttp.PostJSON<RetornoAutenticar>(urlApi + "/api/Autenticar", aut, out erro);
                if (respToken == null)
                {
                    lblAviso.Text = "Erro ao na requisição :: " + erro;
                    lblAviso.ForeColor = Color.Red;
                    return;
                }
                if (!respToken.autenticado)
                {
                    lblAviso.Text = respToken.mensagem;
                    if (!string.IsNullOrEmpty(erro))
                        lblAviso.Text += " :: " + erro;
                    lblAviso.ForeColor = Color.Red;
                    return;
                }

                var cookieToken = new HttpCookie("Token", respToken.tokenAcesso);
                cookieToken.Expires = respToken.expira;
                Request.Cookies.Add(cookieToken);

                token = respToken.tokenAcesso;
            }

            var listaStatus = AuxHttp.GetJSON<RetornoStatusLista>(urlApi + "/api/Clientes/ListarStatusDeClientes", out erro, token);
            if (listaStatus == null)
            {
                lblAviso.Text = "Erro ao na requisição :: " + erro;
                lblAviso.ForeColor = Color.Red;
                return;
            }
            if (!listaStatus.sucesso)
            {
                lblAviso.Text = listaStatus.mensagem;
                if (!string.IsNullOrEmpty(erro))
                    lblAviso.Text += " :: " + erro;
                lblAviso.ForeColor = Color.Red;
                return;
            }

            ddlStatus.Items.Clear();
            foreach (Status stats in listaStatus.status)
            {
                ddlStatus.Items.Add(new ListItem(stats.descricao, stats.idStatus.ToString()));
            }

            var listaTipo = AuxHttp.GetJSON<RetornoTiposLista>(urlApi + "/api/Clientes/ListarTipoDeClientes", out erro, token);
            if (listaTipo == null)
            {
                lblAviso.Text = "Erro ao na requisição :: " + erro;
                lblAviso.ForeColor = Color.Red;
                return;
            }
            if (!listaTipo.sucesso)
            {
                lblAviso.Text = listaTipo.mensagem;
                if (!string.IsNullOrEmpty(erro))
                    lblAviso.Text += " :: " + erro;
                lblAviso.ForeColor = Color.Red;
                return;
            }

            ddlTipo.Items.Clear();
            foreach (Tipos tipo in listaTipo.tipos)
            {
                ddlTipo.Items.Add(new ListItem(tipo.descricao, tipo.idTipo.ToString()));
            }

            var listaClientes = AuxHttp.GetJSON<RetornoClientesLista>(urlApi + "/api/Clientes/ListarClientes", out erro, token);
            if (listaClientes == null)
            {
                lblAviso.Text = "Erro ao na requisição :: " + erro;
                lblAviso.ForeColor = Color.Red;
                return;
            }
            if (!listaClientes.sucesso)
            {
                lblAviso.Text = listaClientes.mensagem;
                if (!string.IsNullOrEmpty(erro))
                    lblAviso.Text += " :: " + erro;
                lblAviso.ForeColor = Color.Red;
                return;
            }

            var dt = new DataTable();

            dt.Columns.Add("CPF", typeof(string));
            dt.Columns.Add("NOME", typeof(string));
            dt.Columns.Add("SEXO", typeof(string));
            dt.Columns.Add("TIPO", typeof(string));
            dt.Columns.Add("STATUS", typeof(string));

            foreach (Cliente cli in listaClientes.clientes)
            {
                var row = dt.NewRow();
                row["CPF"] = cli.cpf;
                row["NOME"] = cli.nome;
                row["SEXO"] = cli.sexo == "M" ? "Masculino" : "Feminino";
                row["TIPO"] = cli.tipo + "-" + cli.descTipo;
                row["STATUS"] = cli.status + "-" + cli.descStatus;
                dt.Rows.Add(row);
            }

            gvClientesCadastrados.DataSource = dt;
            this.ViewState["gvClientesCadastrados"] = dt;
            gvClientesCadastrados.PageIndex = 0;
            gvClientesCadastrados.DataBind();
            gvClientesCadastrados.SelectedIndex = -1;
        }

        protected void txtCpf_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCpf.Text))
            {
                lblAviso.Text = string.Empty;
                var token = string.Empty;
                var erro = string.Empty;

                if (Request.Cookies["Token"] != null)
                {
                    if ((Request.Cookies["Token"].Expires - DateTime.Now).TotalSeconds > 20)
                    {
                        token = Request.Cookies["Token"].Value;
                    }
                    else
                    {
                        var aut = new AutenticarApi();
                        aut.login = login;
                        aut.senha = senha;

                        var respToken = AuxHttp.PostJSON<RetornoAutenticar>(urlApi + "/api/Autenticar", aut, out erro);
                        if (respToken == null)
                        {
                            lblAviso.Text = "Erro ao na requisição :: " + erro;
                            lblAviso.ForeColor = Color.Red;
                            return;
                        }
                        if (!respToken.autenticado)
                        {
                            lblAviso.Text = respToken.mensagem;
                            if (!string.IsNullOrEmpty(erro))
                                lblAviso.Text += " :: " + erro;
                            lblAviso.ForeColor = Color.Red;
                            return;
                        }

                        Request.Cookies["Token"].Value = respToken.tokenAcesso;
                        Request.Cookies["Token"].Expires = respToken.expira;

                        token = respToken.tokenAcesso;
                    }
                }
                else
                {
                    var aut = new AutenticarApi();
                    aut.login = login;
                    aut.senha = senha;

                    var respToken = AuxHttp.PostJSON<RetornoAutenticar>(urlApi + "/api/Autenticar", aut, out erro);
                    if (respToken == null)
                    {
                        lblAviso.Text = "Erro ao na requisição :: " + erro;
                        lblAviso.ForeColor = Color.Red;
                        return;
                    }
                    if (!respToken.autenticado)
                    {
                        lblAviso.Text = respToken.mensagem;
                        if (!string.IsNullOrEmpty(erro))
                            lblAviso.Text += " :: " + erro;
                        lblAviso.ForeColor = Color.Red;
                        return;
                    }

                    var cookieToken = new HttpCookie("Token", respToken.tokenAcesso);
                    cookieToken.Expires = respToken.expira;
                    Request.Cookies.Add(cookieToken);

                    token = respToken.tokenAcesso;
                }

                var url = urlApi + "/api/Clientes/CarregaCliente?CPF=" + txtCpf.Text;
                var carregaCli = AuxHttp.GetJSON<RetornoCliente>(url, out erro, token);
                if (carregaCli == null)
                {
                    lblAviso.Text = "Erro ao na requisição :: " + erro;
                    lblAviso.ForeColor = Color.Red;
                    return;
                }
                if (carregaCli.sucesso)
                {
                    txtNome.Text = carregaCli.cliente.nome;
                    ddlSexo.SelectedValue = carregaCli.cliente.sexo;
                    ddlStatus.SelectedValue = carregaCli.cliente.status.ToString();
                    ddlTipo.SelectedValue = carregaCli.cliente.tipo.ToString();
                }
                else
                {
                    txtNome.Text = string.Empty;
                    ddlSexo.SelectedIndex = 0;
                    ddlStatus.SelectedIndex = 0;
                    ddlTipo.SelectedIndex = 0;
                }
            }
            else
            {
                txtNome.Text = string.Empty;
                ddlSexo.SelectedIndex = 0;
                ddlStatus.SelectedIndex = 0;
                ddlTipo.SelectedIndex = 0;
            }
        }
        #endregion

        #region Métodos Click
        protected void btnGravar_Click(object sender, EventArgs e)
        {
            lblAviso.Text = string.Empty;
            var token = string.Empty;
            var erro = string.Empty;

            if (Request.Cookies["Token"] != null)
            {
                if ((Request.Cookies["Token"].Expires - DateTime.Now).TotalSeconds > 20)
                {
                    token = Request.Cookies["Token"].Value;
                }
                else
                {
                    var aut = new AutenticarApi();
                    aut.login = login;
                    aut.senha = senha;

                    var respToken = AuxHttp.PostJSON<RetornoAutenticar>(urlApi + "/api/Autenticar", aut, out erro);
                    if (respToken == null)
                    {
                        lblAviso.Text = "Erro ao na requisição :: " + erro;
                        lblAviso.ForeColor = Color.Red;
                        return;
                    }
                    if (!respToken.autenticado)
                    {
                        lblAviso.Text = respToken.mensagem;
                        if (!string.IsNullOrEmpty(erro))
                            lblAviso.Text += " :: " + erro;
                        lblAviso.ForeColor = Color.Red;
                        return;
                    }

                    Request.Cookies["Token"].Value = respToken.tokenAcesso;
                    Request.Cookies["Token"].Expires = respToken.expira;

                    token = respToken.tokenAcesso;
                }
            }
            else
            {
                var aut = new AutenticarApi();
                aut.login = login;
                aut.senha = senha;

                var respToken = AuxHttp.PostJSON<RetornoAutenticar>(urlApi + "/api/Autenticar", aut, out erro); 
                if (respToken == null)
                {
                    lblAviso.Text = "Erro ao na requisição :: " + erro;
                    lblAviso.ForeColor = Color.Red;
                    return;
                }
                if (!respToken.autenticado)
                {
                    lblAviso.Text = respToken.mensagem;
                    if (!string.IsNullOrEmpty(erro))
                        lblAviso.Text += " :: " + erro;
                    lblAviso.ForeColor = Color.Red;
                    return;
                }

                var cookieToken = new HttpCookie("Token", respToken.tokenAcesso);
                cookieToken.Expires = respToken.expira;
                Request.Cookies.Add(cookieToken);

                token = respToken.tokenAcesso;
            }

            var cli = new Cliente();
            cli.cpf = txtCpf.Text;
            cli.nome = txtNome.Text;
            cli.sexo = ddlSexo.SelectedValue;
            cli.status = int.Parse(ddlStatus.SelectedValue);
            cli.tipo = int.Parse(ddlTipo.SelectedValue);

            var postCli = AuxHttp.PostJSON<RetornoGenerico>(urlApi + "/api/Clientes/GravarCliente", cli, out erro, token);
            if (postCli == null)
            {
                lblAviso.Text = "Erro ao na requisição :: " + erro;
                lblAviso.ForeColor = Color.Red;
                return;
            }
            if (!postCli.sucesso)
            {
                lblAviso.Text = postCli.mensagem;
                if (!string.IsNullOrEmpty(erro))
                    lblAviso.Text += " :: " + erro;
                lblAviso.ForeColor = Color.Red;
                return;
            }

            txtCpf.Text = string.Empty;
            txtNome.Text = string.Empty;
            ddlSexo.SelectedIndex = 0;

            CarregaTela();

            lblAviso.Text = postCli.mensagem;
            lblAviso.ForeColor = Color.Green;
        }

        protected void btnCancela_Click(object sender, EventArgs e)
        {
            lblAviso.Text = string.Empty;
            txtCpf.Text = string.Empty;
            txtNome.Text = string.Empty;
            ddlSexo.SelectedIndex = 0;

            CarregaTela();
        }
        #endregion

        #region Métodos de GridView
        protected void gvClientesCadastrados_SelectedIndexChanged(object sender, EventArgs e)
        {
            var gvRow = gvClientesCadastrados.SelectedRow;
            txtCpf.Text = gvClientesCadastrados.SelectedDataKey["CPF"].ToString();
            txtNome.Text = gvRow.Cells[1].Text;
            ddlSexo.SelectedValue = gvRow.Cells[2].Text.ToString() == "Masculino" ? "M" : "F";
            ddlTipo.SelectedValue = gvRow.Cells[3].Text.Split('-')[0].Trim();
            ddlStatus.SelectedValue = gvRow.Cells[4].Text.Split('-')[0].Trim(); ;
        }

        protected void gvClientesCadastrados_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            gvClientesCadastrados.SelectedIndex = e.RowIndex;
            lblAviso.Text = string.Empty;
            var token = string.Empty;
            var erro = string.Empty;

            if (Request.Cookies["Token"] != null)
            {
                if ((Request.Cookies["Token"].Expires - DateTime.Now).TotalSeconds > 20)
                {
                    token = Request.Cookies["Token"].Value;
                }
                else
                {
                    var aut = new AutenticarApi();
                    aut.login = login;
                    aut.senha = senha;

                    var respToken = AuxHttp.PostJSON<RetornoAutenticar>(urlApi + "/api/Autenticar", aut, out erro);
                    if (!respToken.autenticado)
                    {
                        lblAviso.Text = respToken.mensagem;
                        if (!string.IsNullOrEmpty(erro))
                            lblAviso.Text += " :: " + erro;
                        lblAviso.ForeColor = Color.Red;
                        return;
                    }

                    Request.Cookies["Token"].Value = respToken.tokenAcesso;
                    Request.Cookies["Token"].Expires = respToken.expira;

                    token = respToken.tokenAcesso;
                }
            }
            else
            {
                var aut = new AutenticarApi();
                aut.login = login;
                aut.senha = senha;

                var respToken = AuxHttp.PostJSON<RetornoAutenticar>(urlApi + "/api/Autenticar", aut, out erro);
                if (!respToken.autenticado)
                {
                    lblAviso.Text = respToken.mensagem;
                    if (!string.IsNullOrEmpty(erro))
                        lblAviso.Text += " :: " + erro;
                    lblAviso.ForeColor = Color.Red;
                    return;
                }

                var cookieToken = new HttpCookie("Token", respToken.tokenAcesso);
                cookieToken.Expires = respToken.expira;
                Request.Cookies.Add(cookieToken);

                token = respToken.tokenAcesso;
            }

            var url = urlApi + "/api/Clientes/ExcluiCliente?CPF=" + gvClientesCadastrados.SelectedDataKey["CPF"].ToString();
            var deleteCli = AuxHttp.DeleteJSON<RetornoGenerico>(url, out erro, token);
            if (!deleteCli.sucesso)
            {
                lblAviso.Text = deleteCli.mensagem;
                if (!string.IsNullOrEmpty(erro))
                    lblAviso.Text += " :: " + erro;
                lblAviso.ForeColor = Color.Red;
                return;
            }

            txtCpf.Text = string.Empty;
            txtNome.Text = string.Empty;
            ddlSexo.SelectedIndex = 0;

            CarregaTela();

            lblAviso.Text = deleteCli.mensagem;
            lblAviso.ForeColor = Color.Green;
        }

        protected void gvClientesCadastrados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dt = this.ViewState["gvClientesCadastrados"] as DataTable;
            gvClientesCadastrados.DataSource = dt;
            gvClientesCadastrados.PageIndex = e.NewPageIndex;
            gvClientesCadastrados.SelectedIndex = -1;
            gvClientesCadastrados.DataBind();
        }
        #endregion
    }
}