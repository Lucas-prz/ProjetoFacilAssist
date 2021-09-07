using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WA_PortalCliente.Interface
{
    public class RetornoCliente
    {
        public bool sucesso { get; set; }
        public string mensagem { get; set; }
        public Cliente cliente { get; set; }
    }
    public class RetornoClientesLista
    {
        public bool sucesso { get; set; }
        public string mensagem { get; set; }
        public Cliente[] clientes { get; set; }
    }

    public class Cliente
    {
        public string cpf { get; set; }
        public string nome { get; set; }
        public string sexo { get; set; }
        public int status { get; set; }
        public string descStatus { get; set; }
        public int tipo { get; set; }
        public string descTipo { get; set; }
    }

    public class AutenticarApi
    {
        public string login { get; set; }
        public string senha { get; set; }
    }

    public class RetornoAutenticar
    {
        public bool autenticado { get; set; }
        public DateTime criado { get; set; }
        public DateTime expira { get; set; }
        public string tokenAcesso { get; set; }
        public string mensagem { get; set; }
    }

    public class RetornoTipo
    {
        public bool sucesso { get; set; }
        public string mensagem { get; set; }
        public Tipos tipo { get; set; }
    }
    public class RetornoTiposLista
    {
        public bool sucesso { get; set; }
        public string mensagem { get; set; }
        public Tipos[] tipos { get; set; }
    }

    public class Tipos
    {
        public int idTipo { get; set; }
        public string descricao { get; set; }
    }

    public class RetornoStatus
    {
        public bool sucesso { get; set; }
        public string mensagem { get; set; }
        public Status status { get; set; }
    }

    public class RetornoStatusLista
    {
        public bool sucesso { get; set; }
        public string mensagem { get; set; }
        public Status[] status { get; set; }
    }

    public class Status
    {
        public int idStatus { get; set; }
        public string descricao { get; set; }
    }


    public class RetornoGenerico
    {
        public bool sucesso { get; set; }
        public string mensagem { get; set; }
    }

}