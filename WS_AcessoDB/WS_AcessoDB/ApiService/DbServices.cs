using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WS_AcessoDB.ApiService
{
    public class DbServices
    {
        [NonAction]
        public DataTable CarregaRegistros(string strConn, string procedure, List<SqlParameter> cmdParam, out string Erro)
        {
            DataTable dt = new DataTable();
            Erro = string.Empty;

            try
            {
                using (SqlConnection dbConn = new SqlConnection(strConn))
                {
                    using (SqlCommand SqlCmd = new SqlCommand(procedure, dbConn))
                    {
                        SqlCmd.CommandType = CommandType.StoredProcedure;

                        if (cmdParam != null && cmdParam.Count > 0)
                        {
                            foreach (SqlParameter param in cmdParam)
                            {
                                SqlCmd.Parameters.Add(param);
                            }
                        }

                        SqlDataAdapter SqlDa = new SqlDataAdapter(SqlCmd);
                        SqlDa.Fill(dt);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                dt = null;
                Erro = sqlEx.Message;
                if (sqlEx.InnerException != null && !sqlEx.Message.Equals(sqlEx.InnerException.Message))
                    Erro += " :: " + sqlEx.InnerException.Message;
            }
            catch (Exception ex)
            {
                dt = null;
                Erro = ex.Message;
                if (ex.InnerException != null && !ex.Message.Equals(ex.InnerException.Message))
                    Erro += " :: " + ex.InnerException.Message;
            }

            return dt;
        }

        [NonAction]
        public bool ExecutarProc(string strConn, string procedure, List<SqlParameter> cmdParam, out string Erro)
        {
            Erro = string.Empty;
            try
            {
                using (SqlConnection dbConn = new SqlConnection(strConn))
                {
                    using (SqlCommand SqlCmd = new SqlCommand(procedure, dbConn))
                    {
                        SqlCmd.Connection.Open();
                        SqlCmd.CommandType = CommandType.StoredProcedure;

                        if (cmdParam != null && cmdParam.Count > 0)
                        {
                            foreach (SqlParameter param in cmdParam)
                            {
                                SqlCmd.Parameters.Add(param);
                            }
                        }

                        int res = SqlCmd.ExecuteNonQuery();
                        if (SqlCmd.Parameters.Contains("@Retorno"))
                            SqlCmd.Connection.Close();
                    }
                }

                return true;
            }
            catch (SqlException sqlEx)
            {
                Erro = sqlEx.Message;
                if (sqlEx.InnerException != null && !sqlEx.Message.Equals(sqlEx.InnerException.Message))
                    Erro += " :: " + sqlEx.InnerException.Message;
                return false;
            }
            catch (Exception ex)
            {
                Erro = ex.Message;
                if (ex.InnerException != null && !ex.Message.Equals(ex.InnerException.Message))
                    Erro += " :: " + ex.InnerException.Message;
                return false;
            }
        }

        [NonAction]
        public bool ValidaAcesso(string strConn, List<SqlParameter> cmdParam, out string Erro)
        {
            Erro = string.Empty;
            try
            {
                using (SqlConnection dbConn = new SqlConnection(strConn))
                {
                    using (SqlCommand SqlCmd = new SqlCommand("ValidaAcesso", dbConn))
                    {
                        SqlCmd.Connection.Open();
                        SqlCmd.CommandType = CommandType.StoredProcedure;

                        foreach (SqlParameter param in cmdParam)
                        {
                            SqlCmd.Parameters.Add(param);
                        }

                        SqlCmd.Parameters.Add(new SqlParameter("@Retorno", SqlDbType.Int));
                        SqlCmd.Parameters["@Retorno"].Direction = ParameterDirection.Output;
                        SqlCmd.ExecuteNonQuery();
                        int res = int.Parse(SqlCmd.Parameters["@Retorno"].Value.ToString());
                        SqlCmd.Connection.Close();

                        if (res > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Erro = sqlEx.Message;
                if (sqlEx.InnerException != null && !sqlEx.Message.Equals(sqlEx.InnerException.Message))
                    Erro += " :: " + sqlEx.InnerException.Message;
                return false;
            }
            catch (Exception ex)
            {
                Erro = ex.Message;
                if (ex.InnerException != null && !ex.Message.Equals(ex.InnerException.Message))
                    Erro += " :: " + ex.InnerException.Message;
                return false;
            }
        }
    }
}
