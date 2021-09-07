using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WS_AcessoDB.ApiModel.Models;
using WS_AcessoDB.ApiService;

namespace WS_AcessoDB.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AutenticarController : ControllerBase
    {
        private string strConn = string.Empty;
        public AutenticarController(IConfiguration configuration)
        {
            strConn = configuration.GetConnectionString("DefaultConn");
        }

        [AllowAnonymous]
        [HttpPost]
        public RetornoAutenticar AutenticarAcesso([FromBody] AutenticarModel login, [FromServices] TokenConfig tknConfig)
        {
            var dbServ = new DbServices();
            string erro = string.Empty;
            var listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter("@Login", login.Login));
            listParam.Add(new SqlParameter("@Senha", login.Senha));

            if (dbServ.ValidaAcesso(strConn, listParam, out erro))
            {
                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao.AddSeconds(tknConfig.Segundos);

                var ChaveSeg = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tknConfig.Chave));

                var handler = new JwtSecurityTokenHandler();

                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tknConfig.Emitente,
                    Audience = tknConfig.Publico,
                    SigningCredentials = new SigningCredentials(ChaveSeg, SecurityAlgorithms.HmacSha256),
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });

                var token = handler.WriteToken(securityToken);

                return new RetornoAutenticar
                {
                    Autenticado = true,
                    Criado = dataCriacao,
                    Expira = dataExpiracao,
                    TokenAcesso = token,
                    Mensagem = "OK"
                };
            }

            return new RetornoAutenticar
            {
                Autenticado = false,
                Mensagem = "Falha ao autenticar acesso :: " + erro
            }; 
        }
    }
}
