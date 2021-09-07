using System;
using System.ComponentModel.DataAnnotations;

namespace WS_AcessoDB.ApiModel.Models
{
    public class AutenticarModel
    {
        /// <summary>
        /// Login de Acesso da Api
        /// </summary>
        [Required]
        public string Login { get; set; }
        /// <summary>
        /// Senha de Acesso da Api
        /// </summary>
        [Required]
        public string Senha { get; set; }
    }

    public class RetornoAutenticar
    {
        /// <summary>
        /// Usuario autenticado com exito?
        /// </summary>
        public bool Autenticado { get; set; }
        /// <summary>
        /// Data de Criação do Token
        /// </summary>
        public DateTime Criado { get; set; }
        /// <summary>
        /// Data de Expiração do Token
        /// </summary>
        public DateTime Expira { get; set; }
        /// <summary>
        /// Token de Acesso
        /// </summary>
        public string TokenAcesso { get; set; }
        /// <summary>
        /// Mensagem do Processo
        /// </summary>
        public string Mensagem { get; set; }
    }
}
