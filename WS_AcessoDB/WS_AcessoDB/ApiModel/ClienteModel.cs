using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WS_AcessoDB.ApiModel
{
    public class ClienteModel
    {
        /// <summary>
        /// CPF do Cliente (Apenas Numeros)
        /// </summary>
        [Required]
        [MaxLength(11)]
        public string CPF { get; set; }
        /// <summary>
        /// Nome do Cliente (Apenas Numeros)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }
        /// <summary>
        /// Sexo do Cliente (M ou F)
        /// </summary>
        [Required]
        public string Sexo { get; set; }
        /// <summary>
        /// Status do Cadastro
        /// </summary>
        [Required]
        public int Status { get; set; }
        /// <summary>
        /// Descrição do Status
        /// </summary>
        public string DescStatus { get; set; }
        /// <summary>
        /// Tipo de Cliente
        /// </summary>
        [Required]
        public int Tipo { get; set; }
        /// <summary>
        /// Descrição do Tipo
        /// </summary>
        public string DescTipo { get; set; }
    }

    public class StatusModel
    {
        /// <summary>
        /// Codigo de Status do Cadastro
        /// </summary>
        [Required]
        public int IdStatus { get; set; }
        /// <summary>
        /// Descrição do Status
        /// </summary>
        [Required]
        public string Descricao { get; set; }
    }

    public class TipoModel
    {
        /// <summary>
        /// Codigo de Tipo do Cliente
        /// </summary>
        [Required]
        public int IdTipo { get; set; }
        /// <summary>
        /// Descrição do Tipo
        /// </summary>
        [Required]
        public string Descricao { get; set; }
    }

    public class RetornoGeral
    {
        /// <summary>
        /// A transação foi bem sucedida?
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem do Processo
        /// </summary>
        public string Mensagem { get; set; }
    }

    public class RetornoCliente
    {
        /// <summary>
        /// A transação foi bem sucedida?
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem do Processo
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Dados do Cliente
        /// </summary>
        public ClienteModel Cliente { get; set; }
    }

    public class RetornoListaClientes
    {
        /// <summary>
        /// A transação foi bem sucedida?
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem do Processo
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Lista de Dados do Cliente
        /// </summary>
        public List<ClienteModel> Clientes { get; set; }
    }

    public class RetornoStatus
    {
        /// <summary>
        /// A transação foi bem sucedida?
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem do Processo
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Status do Cliente
        /// </summary>
        public StatusModel Status { get; set; }
    }

    public class RetornoListaStatus
    {
        /// <summary>
        /// A transação foi bem sucedida?
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem do Processo
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Lista de Status do Cliente
        /// </summary>
        public List<StatusModel> Status { get; set; }
    }

    public class RetornoTipo
    {
        /// <summary>
        /// A transação foi bem sucedida?
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem do Processo
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Tipo de Cliente
        /// </summary>
        public TipoModel Tipo { get; set; }
    }

    public class RetornoListaTipos
    {
        /// <summary>
        /// A transação foi bem sucedida?
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem do Processo
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Lista Tipos de Cliente
        /// </summary>
        public List<TipoModel> Tipos { get; set; }
    }
}
