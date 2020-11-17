using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApiBusiness.App_Data;
using WebApiBusiness.Business;

namespace WebApiBusiness.Unisolution
{
    public class UnisolutionService
    {
        public static string GetToken(string apiUsername, string apiPassword, string urlToken)
        {
            try
            {
                string str1 = apiUsername;
                string str2 = apiPassword;
                string str3 = urlToken;
                FormUrlEncodedContent urlEncodedContent = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>()
        {
          {
            "grant_type",
            "password"
          },
          {
            "username",
            str1
          },
          {
            "password",
            str2
          }
        });
                Uri baseUri = new Uri(string.Format("http://{0}", (object)str3));
                HttpRequestMessage request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(baseUri, "token"),
                    Content = (HttpContent)new StringContent("grant_type=password")
                };
                HttpContentHeaders headers = request.Content.Headers;
                MediaTypeWithQualityHeaderValue qualityHeaderValue = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded");
                qualityHeaderValue.CharSet = "UTF-8";
                headers.ContentType = (MediaTypeHeaderValue)qualityHeaderValue;
                request.Content = (HttpContent)urlEncodedContent;
                HttpResponseMessage result1 = new HttpClient().SendAsync(request).Result;
                string result2 = result1.Content.ReadAsStringAsync().Result;
                string result3 = result1.Content.ReadAsStringAsync().Result;
                if (result1.IsSuccessStatusCode)
                    return JObject.Parse(result3)["access_token"].ToString();
                return JObject.Parse(result3)["error_description"].ToString();
            }
            catch (Exception ex)
            {
                return (string)null;
            }
        }

        public static UnisolutionMotorista ValidaEntradaPatio(
          string placa,
          string token,
          string urlValidaEntradaPation,
          Local local,
          string filial,
          string sessao = "0")
        {
            string str = string.Format("http://{0}?placa={1}&local={2}", (object)urlValidaEntradaPation, (object)placa, (object)(int)local);
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = "endereco : " + str,
                idSessao = sessao
            });
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(str);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", (object)token));
                    LogBusiness.InserirLogApi(new LogApi()
                    {
                        texto = "token : " + token,
                        idSessao = sessao
                    });
                    httpClient.DefaultRequestHeaders.Add("FILIAL", filial);
                    LogBusiness.InserirLogApi(new LogApi()
                    {
                        texto = "filial : " + filial,
                        idSessao = sessao
                    });
                    HttpResponseMessage result1 = httpClient.GetAsync(str).Result;
                    if (result1.IsSuccessStatusCode)
                    {
                        string result2 = result1.Content.ReadAsStringAsync().Result;
                        Result result3 = JsonConvert.DeserializeObject<RootObject>(result2).result;
                        LogBusiness.InserirLogApi(new LogApi()
                        {
                            texto = "resultado : " + result1.Content.ReadAsStringAsync().Result,
                            idSessao = sessao
                        });
                        RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(result2);
                        LogApi log = new LogApi();
                        log.texto = "rootObject : " + (object)rootObject;
                        log.idSessao = sessao;
                        LogBusiness.InserirLogApi(log);
                        LogBusiness.InserirLogApi(log);
                        log.idSessao = sessao;
                        UnisolutionMotorista unisolutionMotorista = new UnisolutionMotorista()
                        {
                            statusCode = rootObject.statusCode,
                            result = result3,
                            statusMessage = rootObject.statusMessages.First<string>()
                        };
                        LogBusiness.InserirLogApi(new LogApi()
                        {
                            texto = "retSuccess : " + (object)unisolutionMotorista,
                            idSessao = sessao
                        });
                        return unisolutionMotorista;
                    }
                    return new UnisolutionMotorista()
                    {
                        statusCode = 1,
                        statusMessage = "Motorista não autorizado"
                    };
                }
            }
            catch (HttpRequestException)
            {
                LogBusiness.InserirLogApi(new LogApi()
                {
                    texto = "retSuccess : Erro",
                    idSessao = sessao
                });
                return new UnisolutionMotorista()
                {
                    statusCode = 1,
                    statusMessage = "Motorista não autorizado"
                };
            }
        }

        public static async Task<UnisolutionMotorista> ValidaEntradaPatioV1(
          string placa,
          string token,
          string urlValidaEntradaPation,
          Local local,
          string filial)
        {
            string str = string.Format("http://{0}?placa={1}", (object)urlValidaEntradaPation, (object)placa);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", (object)token));
                    client.DefaultRequestHeaders.Add("FILIAL", filial);
                    ResultAntigo resultAntigo = JsonConvert.DeserializeObject<ResultAntigo>(await (await client.GetAsync(str)).Content.ReadAsStringAsync());
                    return new UnisolutionMotorista()
                    {
                        statusCode = resultAntigo.statusCode,
                        result = new Result()
                        {
                            numOs = resultAntigo.result
                        },
                        statusMessage = resultAntigo.statusMessages.First<string>()
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                return new UnisolutionMotorista()
                {
                    statusCode = 1,
                    statusMessage = "Motorista não autorizado"
                };
            }
        }
    }
}