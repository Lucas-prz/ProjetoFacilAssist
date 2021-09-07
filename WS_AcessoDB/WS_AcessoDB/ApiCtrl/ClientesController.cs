using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using WS_AcessoDB.ApiModel;
using WS_AcessoDB.ApiService;

namespace WS_AcessoDB.ApiCtrl
{
    [Authorize("Bearer")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {

        private string strConn = null;

        public ClientesController(IConfiguration configuration)
        {
            strConn = configuration.GetConnectionString("DefaultConn");
        }

        /// <summary>
        /// Carrega clietne pelo CPF
        /// </summary>
        /// <param name="CPF">CPF do cliente (somente numeros)</param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet]
        public RetornoCliente CarregaCliente(string CPF)
        {
            string erro = string.Empty;

            var cli = new ClienteModel();

            if (string.IsNullOrEmpty(CPF))
            {
                return new RetornoCliente
                {
                    Sucesso = false,
                    Mensagem = "Erro ao carregar cliente :: CPF informado esta em branco",
                    Cliente = cli
                };
            }


            var dbServ = new DbServices();

            var listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter("@Cpf", CPF));

            var dt = dbServ.CarregaRegistros(strConn, "CarregaCliente", listParam, out erro);
            if (dt == null)
            {
                return new RetornoCliente
                {
                    Sucesso = false,
                    Mensagem = erro,
                    Cliente = cli
                };
            }

            if (dt.Rows.Count == 0)
            {
                return new RetornoCliente
                {
                    Sucesso = false,
                    Mensagem = "Cliente não encontrado",
                    Cliente = cli
                };
            }

            cli.CPF = CPF;
            cli.Nome = dt.Rows[0]["NOME"] != null ? dt.Rows[0]["NOME"].ToString() : string.Empty;
            cli.Sexo = dt.Rows[0]["SEXO"] != null ? dt.Rows[0]["NOME"].ToString() : string.Empty;
            cli.Tipo = dt.Rows[0]["TIPO"] != null ? int.Parse(dt.Rows[0]["TIPO"].ToString()) : 0;
            cli.DescTipo = dt.Rows[0]["DESCTIPO"] != null ? dt.Rows[0]["DESCTIPO"].ToString() : string.Empty;
            cli.Status = dt.Rows[0]["STATUS"] != null ? int.Parse(dt.Rows[0]["STATUS"].ToString()) : 0;
            cli.DescStatus = dt.Rows[0]["DESCSTATUS"] != null ? dt.Rows[0]["DESCSTATUS"].ToString() : string.Empty;

            return new RetornoCliente
            {
                Sucesso = true,
                Mensagem = "OK",
                Cliente = cli
            };
        }

        /// <summary>
        /// Lista todos os clientes cadastrados 
        /// </summary>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet]
        public RetornoListaClientes ListarClientes()
        {
            string erro;
            var listParam = new List<SqlParameter>();
            var ListCli = new List<ClienteModel>();

            var dbServ = new DbServices();
            var dt = dbServ.CarregaRegistros(strConn, "ListaClientes", listParam, out erro);

            if (dt == null)
            {
                return new RetornoListaClientes
                {
                    Sucesso = false,
                    Mensagem = erro,
                    Clientes = ListCli
                };
            }

            foreach (DataRow dr in dt.Rows)
            {
                var cli = new ClienteModel();
                cli.CPF = dr["CPF"] != null ? dr["CPF"].ToString() : string.Empty;
                cli.Nome = dr["NOME"] != null ? dr["NOME"].ToString() : string.Empty;
                cli.Sexo = dr["SEXO"] != null ? dr["SEXO"].ToString() : string.Empty;
                cli.Tipo = dr["TIPO"] != null ? int.Parse(dr["TIPO"].ToString()) : 0;
                cli.DescTipo = dr["DESCTIPO"] != null ? dr["DESCTIPO"].ToString() : string.Empty;
                cli.Status = dr["STATUS"] != null ? int.Parse(dr["STATUS"].ToString()) : 0;
                cli.DescStatus = dr["DESCSTATUS"] != null ? dr["DESCSTATUS"].ToString() : string.Empty;
                ListCli.Add(cli);
            }

            return new RetornoListaClientes
            {
                Sucesso = true,
                Mensagem = "OK",
                Clientes = ListCli
            };
        }

        /// <summary>
        /// Gravar/Alterar Cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPost]
        public RetornoGeral GravarCliente([FromBody] ClienteModel cliente)
        {
            if (string.IsNullOrEmpty(cliente.CPF))
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao gravar cliente :: CPF informado esta em branco"
                };
            }
            if (string.IsNullOrEmpty(cliente.Nome))
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao gravar cliente :: Nome do cliente informado esta em branco"
                };
            }
            if (string.IsNullOrEmpty(cliente.Sexo) || cliente.Sexo != "M" && cliente.Sexo != "F")
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao gravar cliente :: Sexo do cliente informado esta em branco, ou esta diferente de 'M' ou 'F'"
                };
            }
            if (cliente.CPF.Length != 11)
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao gravar cliente :: CPF deve ter 11 digitos e conter apenas numeros"
                };
            }
            long i;
            if (!long.TryParse(cliente.CPF, out i))
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao gravar cliente :: CPF deve conter apenas numeros"
                };
            }

            var dbServ = new DbServices();
            string erro = string.Empty;

            var listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter("@Status", cliente.Status));

            var dt = dbServ.CarregaRegistros(strConn, "CarregaStatusCliente", listParam, out erro);
            if (dt == null)
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao gravar cliente :: Erro ao validar status: " + erro
                };
            }
            if (dt.Rows.Count == 0)
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao gravar cliente :: Status Informado não esta cadastrado"
                };
            }

            listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter("@Tipo", cliente.Tipo));

            dt = dbServ.CarregaRegistros(strConn, "CarregaTipoCliente", listParam, out erro);
            if (dt == null)
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao gravar cliente :: Erro ao validar tipo de cliente: " + erro
                };
            }
            if (dt.Rows.Count == 0)
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao gravar cliente :: Tipo de cliente Informado não esta cadastrado"
                };
            }

            listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter("@Cpf", cliente.CPF));
            listParam.Add(new SqlParameter("@Nome", cliente.Nome));
            listParam.Add(new SqlParameter("@Sexo", cliente.Sexo));
            listParam.Add(new SqlParameter("@Tipo", cliente.Tipo));
            listParam.Add(new SqlParameter("@Status", cliente.Status));

            var res = dbServ.ExecutarProc(strConn, "GravaCliente", listParam, out erro);
            if (!res)
            {
                return new RetornoGeral
                {
                    Sucesso = true,
                    Mensagem = "Erro ao gravar cliente :: " + erro
                };
            }

            return new RetornoGeral
            {
                Sucesso = true,
                Mensagem = "Cliente Gravado com Sucesso!"
            };
        }

        /// <summary>
        /// Deleta o cliente identificado pelo CPF 
        /// </summary>
        /// <param name="CPF">CPF do cliente a excluir (somente numeros)</param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpDelete]
        public RetornoGeral ExcluiCliente(string CPF)
        {
            if (string.IsNullOrEmpty(CPF))
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao gravar cliente :: CPF informado esta em branco"
                };
            }
            if (CPF.Length != 11)
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao gravar cliente :: CPF deve ter 11 digitos e conter apenas numeros"
                };
            }
            if (!Regex.IsMatch(CPF, "[0-9]+"))
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao gravar cliente :: CPF deve conter apenas numeros"
                };
            }

            var dbServ = new DbServices();
            string erro = string.Empty;
            var listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter("@Cpf", CPF));
            var res = dbServ.ExecutarProc(strConn, "ExcluirCliente", listParam, out erro);

            if (!res)
            {
                return new RetornoGeral
                {
                    Sucesso = false,
                    Mensagem = "Erro ao excluir cliente :: " + erro
                };
            }

            return new RetornoGeral
            {
                Sucesso = true,
                Mensagem = "Cliente Excluido com Sucesso!"
            };
        }

        /// <summary>
        /// Carrega status de cliente através do código
        /// </summary>
        /// <param name="IdStatus">Código do Status</param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet]
        public RetornoStatus CarregaStatus(int IdStatus)
        {
            var status = new StatusModel();
            string erro = string.Empty;

            var dbServ = new DbServices();
            var listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter("@Status", IdStatus));

            var dt = dbServ.CarregaRegistros(strConn, "CarregaStatusCliente", listParam, out erro);
            if (dt == null)
            {
                return new RetornoStatus
                {
                    Sucesso = false,
                    Mensagem = erro,
                    Status = status
                };
            }

            status.IdStatus = int.Parse(dt.Rows[0]["STATUS"].ToString());
            status.Descricao = dt.Rows[0]["DESCRICAO"].ToString();

            return new RetornoStatus
            {
                Sucesso = true,
                Mensagem = "OK",
                Status = status
            };
        }

        /// <summary>
        /// Carrega lista com todos os satatus de clientes cadastrados
        /// </summary>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet]
        public RetornoListaStatus ListarStatusDeClientes()
        {
            string erro;
            var listParam = new List<SqlParameter>();
            var ListStatus = new List<StatusModel>();

            var dbServ = new DbServices();
            var dt = dbServ.CarregaRegistros(strConn, "ListaStatusDeClientes", listParam, out erro);
            if (dt == null)
            {
                return new RetornoListaStatus
                {
                    Sucesso = false,
                    Mensagem = erro,
                    Status = ListStatus
                };
            }

            foreach (DataRow dr in dt.Rows)
            {
                var status = new StatusModel();
                status.IdStatus = int.Parse(dr["STATUS"].ToString());
                status.Descricao = dr["DESCRICAO"].ToString();
                ListStatus.Add(status);
            }

            return new RetornoListaStatus
            {
                Sucesso = true,
                Mensagem = "OK",
                Status = ListStatus
            };
        }

        /// <summary>
        /// Carrega tipo de cliente através do código
        /// </summary>
        /// <param name="IdTipo">Código do Tipo</param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet]
        public RetornoTipo CarregaTipo(int IdTipo)
        {
            string erro = string.Empty;

            var tipo = new TipoModel();
            var dbServ = new DbServices();
            var listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter("@Tipo", IdTipo));

            var dt = dbServ.CarregaRegistros(strConn, "CarregaTipoCliente", listParam, out erro);
            if (dt == null)
            {
                return new RetornoTipo
                {
                    Sucesso = false,
                    Mensagem = erro,
                    Tipo = tipo
                };
            }

            tipo.IdTipo = int.Parse(dt.Rows[0]["STATUS"].ToString());
            tipo.Descricao = dt.Rows[0]["DESCRICAO"].ToString();

            return new RetornoTipo
            {
                Sucesso = true,
                Mensagem = "OK",
                Tipo = tipo
            };
        }

        /// <summary>
        /// Carrega lista com todos os tipos de clientes cadastrados
        /// </summary>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet]
        public RetornoListaTipos ListarTipoDeClientes()
        {
            string erro;

            var listParam = new List<SqlParameter>();
            var ListTipos = new List<TipoModel>();

            var dbServ = new DbServices();
            var dt = dbServ.CarregaRegistros(strConn, "ListaTiposDeClientes", listParam, out erro);
            if (dt == null)
            {
                return new RetornoListaTipos
                {
                    Sucesso = false,
                    Mensagem = erro,
                    Tipos = ListTipos
                };
            }

            foreach (DataRow dr in dt.Rows)
            {
                var tipo = new TipoModel();
                tipo.IdTipo = int.Parse(dr["TIPO"].ToString());
                tipo.Descricao = dr["DESCRICAO"].ToString();
                ListTipos.Add(tipo);
            }

            return new RetornoListaTipos
            {
                Sucesso = true,
                Mensagem = "OK",
                Tipos = ListTipos
            };
        }
    }
}
