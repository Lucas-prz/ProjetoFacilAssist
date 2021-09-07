using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace WA_PortalCliente.Auxiliar
{
    public class AuxHttp
    {
        /// <summary>
        /// Requisição do tipo GET para APIs baseadas em JSON.
        /// </summary>
        /// <typeparam name="T">Tipo do retorno da requisição.</typeparam>
        /// <param name="url">URL da API.</param>
        /// <param name="mensagem">Mensagem de retorno da requisição.</param>
        /// <param name="authToken">Token para autenticação.</param>
        /// <returns>Resposta da requisição baseada no tipo de entrada</returns>
        public static T GetJSON<T>(string url, out string mensagem, string token)
        {
            mensagem = string.Empty;

            T obj = default(T);

            string jsonResponse = string.Empty;

            try
            {
                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                webRequest.Method = WebRequestMethods.Http.Get;

                if (!string.IsNullOrEmpty(token))
                {
                    webRequest.Accept = "application/json";
                    webRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Bearer {0}", token);
                }

                HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse;

                if (response.StatusCode.ToString() != "OK")
                {
                    mensagem = response.StatusCode.ToString() + ": " + response.StatusDescription;
                    return default(T);
                }

                StreamReader reader = new StreamReader(response.GetResponseStream());
                string temp = null;

                while ((temp = reader.ReadLine()) != null)
                {
                    jsonResponse += temp;
                }

                obj = new JavaScriptSerializer().Deserialize<T>(jsonResponse);
            }
            catch (WebException w)
            {
                mensagem = w.Message;
                if (w.InnerException != null && !w.Message.Equals(w.InnerException.Message))
                    mensagem += " :: " + w.InnerException.Message;
                return obj;
            }
            catch (Exception e)
            {
                mensagem = e.Message;
                if (e.InnerException != null && !e.Message.Equals(e.InnerException.Message))
                    mensagem += " :: " + e.InnerException.Message;
                return obj;
            }
            return obj;
        }

        /// <summary>
        /// Requisição do tipo POST para APIs baseadas em JSON.
        /// </summary>
        /// <typeparam name="T">Tipo do retorno da requisição.</typeparam>
        /// <param name="url">URL da API.</param>
        /// <param name="objeto">Objeto para ser enviado via POST.</param>
        /// <param name="mensagem">Mensagem de retorno da requisição.</param>
        /// <param name="authToken">Token para autenticação.</param>
        /// <returns>Resposta da requisição baseada no tipo de entrada</returns>
        public static T PostJSON<T>(string url, dynamic objeto, out string mensagem, string authToken = null)
        {
            mensagem = string.Empty;

            T obj = default(T);

            try
            {
                Encoding encoding = new UTF8Encoding();

                string json = JsonConvert.SerializeObject(objeto, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                byte[] data = encoding.GetBytes(json);

                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                webRequest.Method = WebRequestMethods.Http.Post;
                webRequest.ContentType = "application/json;charset=UTF-8";
                webRequest.AutomaticDecompression = DecompressionMethods.GZip;
                webRequest.ProtocolVersion = HttpVersion.Version11;
                webRequest.ContentLength = data.Length;

                if (!string.IsNullOrEmpty(authToken))
                {
                    webRequest.Accept = "application/json";
                    webRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Bearer {0}", authToken);
                }

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Stream sw = webRequest.GetRequestStream();
                sw.Write(data, 0, data.Length);
                sw.Close();

                var response = (HttpWebResponse)webRequest.GetResponse();

                if (response != null)
                {
                    var incomingStreamReader = new StreamReader(response.GetResponseStream());

                    var temp = string.Empty;
                    var jsonResponse = string.Empty;

                    while ((temp = incomingStreamReader.ReadLine()) != null)
                    {
                        jsonResponse += temp;
                    }

                    incomingStreamReader.Close();
                    response.GetResponseStream().Close();

                    obj = new JavaScriptSerializer().Deserialize<T>(jsonResponse);
                }
            }
            catch (WebException w)
            {
                mensagem = w.Message;
                if (w.InnerException != null && !w.Message.Equals(w.InnerException.Message))
                    mensagem += " :: " + w.InnerException.Message;
                return obj;
            }
            catch (Exception e)
            {
                mensagem = e.Message;
                if (e.InnerException != null && !e.Message.Equals(e.InnerException.Message))
                    mensagem += " :: " + e.InnerException.Message;
                return obj;
            }
            return obj;
        }

        /// <summary>
        /// Requisição do tipo POST para APIs baseadas em JSON.
        /// </summary>
        /// <typeparam name="T">Tipo do retorno da requisição.</typeparam>
        /// <param name="url">URL da API.</param>
        /// <param name="objeto">Objeto para ser enviado via POST.</param>
        /// <param name="mensagem">Mensagem de retorno da requisição.</param>
        /// <param name="authToken">Token para autenticação.</param>
        /// <returns>Resposta da requisição baseada no tipo de entrada</returns>
        public static T DeleteJSON<T>(string url, out string mensagem, string authToken = null)
        {
            mensagem = string.Empty;

            T obj = default(T);

            string jsonResponse = string.Empty;
            try
            {
                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                webRequest.Method = "DELETE";
                webRequest.ContentType = "application/json;charset=UTF-8";
                webRequest.AutomaticDecompression = DecompressionMethods.GZip;
                webRequest.ProtocolVersion = HttpVersion.Version11;

                if (!string.IsNullOrEmpty(authToken))
                {
                    webRequest.Accept = "application/json";
                    webRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Bearer {0}", authToken);
                }

                var response = (HttpWebResponse)webRequest.GetResponse();
                if (response.StatusCode.ToString() != "OK")
                {
                    mensagem = response.StatusCode.ToString() + ": " + response.StatusDescription;
                    return default(T);
                }

                StreamReader reader = new StreamReader(response.GetResponseStream());
                string temp = null;

                while ((temp = reader.ReadLine()) != null)
                {
                    jsonResponse += temp;
                }

                obj = new JavaScriptSerializer().Deserialize<T>(jsonResponse);
            }
            catch (WebException w)
            {
                mensagem = w.Message;
                if (w.InnerException != null && !w.Message.Equals(w.InnerException.Message))
                    mensagem += " :: " + w.InnerException.Message;
                return obj;
            }
            catch (Exception e)
            {
                mensagem = e.Message;
                if (e.InnerException != null && !e.Message.Equals(e.InnerException.Message))
                    mensagem += " :: " + e.InnerException.Message;
                return obj;
            }
            return obj;
        }
    }
}