using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WEBAPI_VOPAK.Models
{
    public class DocumentoViewModel2
    {
        public int Id { get; set; }
        public int PessoaId { get; set; }
        public string Numero { get; set; }
        public TipoDocumentoPessoa Tipo { get; set; }
        public virtual PessoaViewModel2 Pessoa { get; set; }
        public DateTime Cadastro { get; set; }
        public bool IsDeleted { get; set; }

    }

    [Serializable()]
    public enum TipoDocumentoPessoa
    {
        [Description("Registro Geral - RG")]
        RG,
        [Description("Registro Nacional de Estrangeiros - RNE")]
        RNE,
        [Description("Cadastro de Pessoas Físicas - CPF")]
        CPF,
        [Description("Carteira Nacional de Habilitação - CNH")]
        CNH,
        [Description("Carteira de Trabalho e Previdência Social - CTPS")]
        CTPS,
        [Description("Passaporte")]
        Passaporte,
        [Description("Cadastro Nacional de Pessoa Jurídica - CNPJ")]
        CNPJ,
        [Description("Indefinido")]
        NDA,
    }


public class PessoaViewModel2
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public virtual List<DocumentoViewModel2> Documentos { get; set; }
        public virtual List<EmpresaViewModel2> Empresas { get; set; }
        public virtual List<VeiculoViewModel2> Veiculos { get; set; }
        public virtual List<FotoViewModel2> Fotos { get; set; }
        public DateTime Cadastro { get; set; }
        public bool IsDeleted { get; set; }
    }


    public class EquipamentoViewModel2
    {
        public int Id { get; set; }
        public int? MasterId { get; set; }
        public int SetorId { get; set; }
        public virtual SetorViewModel2 Setor { get; set; }
        public string Descricao { get; set; }
        public string Configuracao { get; set; }
        public bool DuploAcesso { get; set; } = false;
        public bool AcessoPrimario { get; set; }
        public DateTime Cadastro { get; set; }
        public bool IsDeleted { get; set; }
        public virtual List<PermissaoViewModel2> Permissoes { get; set; }
        public virtual List<FotoViewModel2> Fotos { get; set; }
    }

    public class SetorViewModel2
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public TipoSetor TipoSetor { get; set; }
        public virtual List<EquipamentoViewModel2> Equipamentos { get; set; }
        public DateTime Cadastro { get; set; }
        public bool IsDeleted { get; set; }
    }

    public enum TipoSetor
    {
        [Description("Nenhum")]
        NDA,
        [Description("Pátio")]
        PATIO,
        [Description("Área de Controle Integrado")]
        ACI
    }

    public class EmpresaViewModel2
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        public DateTime Cadastro { get; set; }
        public bool IsDeleted { get; set; }
        public virtual List<PessoaViewModel2> Pessoas { get; set; }
    }

    public class VeiculoViewModel2
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public virtual List<CrachaViewModel2> Crachas { get; set; }
        public DateTime Cadastro { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class CrachaViewModel2
    {
        public int Id { get; set; }
        public int PessoaId { get; set; }
        public TipoCredencial TipoCredencial { get; set; }
        public virtual string Codigo { get; set; }
        public virtual PessoaViewModel2 Pessoa { get; set; }
        public DateTime Cadastro { get; set; }
        public bool IsDeleted { get; set; }

    }

    public enum TipoCredencial
    {
        [Description("Nenhum")]
        NDA,
        [Description("Funcionário")]
        Funcionario,
        [Description("Motorista")]
        Motorista,
        [Description("Prestador de Serviço")]
        Prestador,
        [Description("Visitante")]
        Visitante
    }

    public class FotoViewModel2
    {
        public int Id { get; set; }
        public DateTime Cadastro { get; set; }
        public bool IsDeleted { get; set; }
        public string Caminho { get; set; }
        public TipoImagem TipoImagem { get; set; }
        public byte[] Bytes { get; set; } 
        public int? PessoaId { get; set; }
        public int? EquipamantoId { get; set; }
        public int? VeiculoId { get; set; }
    }
    public enum TipoImagem
    {
        [Description("Nenhum")]
        NDA,
        [Description("OCR")]
        OCR,
        [Description("LBF")]
        LBF,
        [Description("Credencial")]
        CREDENCIAL,
        [Description("Digital")]
        DIGITAL,
        [Description("Autenticação")]
        AUTENTICACAO
    }

    public class AgendaViewModel2 
    {
        public int Id { get; set; }
        public int? PessoaId { get; set; }
        public int? EmpresaId { get; set; }
        public int? CrachaId { get; set; }
        public int? VeiculoId { get; set; }
        public DateTime Chegada { get; set; }
        public DateTime Saida { get; set; }
        public int? OrdemServico { get; set; }
        public string Credencial { get; set; }
        public string Placa { get; set; }
        public virtual List<PermissaoViewModel2> Permissoes { get; set; }
        public virtual CrachaViewModel2 Cracha { get; set; }
        public virtual PessoaViewModel2 Pessoa { get; set; }
        public virtual EmpresaViewModel2 Empresa { get; set; }
        public virtual VeiculoViewModel2 Veiculo { get; set; }
        public List<int> EquipamentosId { get; set; }
        public DateTime Cadastro { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class PermissaoViewModel2
    {     
        public int? AgendaId { get; set; }
        public int? EquipamentoId { get; set; }
    }


    public class ApiClient : IDisposable
    {
        #region Instância, uma vez que esta classe não é estática.
        /// <summary>
        /// Manutenção das chamadas ao destuidor.
        /// </summary>
        private bool disposedValue;
        /// <summary>
        /// Destruidor efetivo.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) { CleanUp(); }
                disposedValue = true;
            }
        }
        /// <summary>
        /// Finalize.
        /// </summary>
        ~ApiClient() { Dispose(disposing: false); }
        /// <summary>
        /// Destruidor padrão.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Limpeza de memória.
        /// </summary>
        private void CleanUp() { }
        #endregion

        public async Task<ActionResult<ContextBoundObject>> Use<T>(HttpMethod http, T obj, string metodo) where T : class
        {
            try
            {
                HttpResponseMessage response;
                string url = string.Format("https://{0}{1}", ApiHelpers.UrlBase, metodo);
                string output = JsonConvert.SerializeObject(obj);
                StringContent content = new StringContent(output, Encoding.UTF8, "application/json");
                response = await GetResponse(http.Method, url, content);
                string result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ActionResult<ContextBoundObject>>(result);
            }
            catch (Exception)
            {
                return new ActionResult<ContextBoundObject>();
            }
        }


        public async Task<string> Lista(HttpMethod http, object obj, string metodo)
        {
            try
            {
                HttpResponseMessage response;
                string url = string.Format("https://{0}{1}", ApiHelpers.UrlBase, metodo);
                string output = JsonConvert.SerializeObject(obj);
                StringContent content = new StringContent(output, Encoding.UTF8, "application/json");
                response = await GetResponse(http.Method, url, content);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            { return ex.Message; }
        }

        internal async Task<HttpResponseMessage> GetResponse(string method = "GET", string url = "", StringContent content = null, bool authenticate = false, string token = "", string conta = "", string senha = "")
        {
            HttpResponseMessage ret = null;
            // Opa, não consigo mais conectar ao serviço. O que fazer?
            if (!ApiHelpers.IsAlive)
            {
                ret = new HttpResponseMessage() { StatusCode = HttpStatusCode.ServiceUnavailable, Content = null, ReasonPhrase = "Servidor indisponível", RequestMessage = null };
            }
            else
            {
                // Opa, o endereço estava nulo ou em branco. O que fazer?
                if (string.IsNullOrEmpty(url))
                {
                    ret = new HttpResponseMessage() { StatusCode = HttpStatusCode.NotAcceptable, Content = null, ReasonPhrase = "Endereço do servidor ausente", RequestMessage = null };
                }
                else
                {
                    AuthenticationHeaderValue adv = null;
                    ApiMethod metodo = method.SwitchMethod();
                    if (authenticate)
                    {
                        if (!string.IsNullOrEmpty(conta) && !string.IsNullOrEmpty(senha))
                        {
                            string uid = "";
                            uid = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{conta}:{senha}"));
                            adv = new AuthenticationHeaderValue("Basic", uid);
                        }
                        else if (!string.IsNullOrEmpty(token))
                        {
                            adv = new AuthenticationHeaderValue("Bearer", token);
                        }
                        else
                        {
                            string uid = "";
                            uid = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"Anonymous:\"\""));
                            adv = new AuthenticationHeaderValue("Basic", uid);
                        }
                    }
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            if (authenticate) client.DefaultRequestHeaders.Authorization = adv;
                            switch (metodo)
                            {
                                default:
                                case ApiMethod.Get: return await client.GetAsync(url);
                                case ApiMethod.Post: return await client.PostAsync(url, content);
                                case ApiMethod.Put: return await client.PutAsync(url, content);
                                case ApiMethod.Delete: return await client.DeleteAsync(url);
                            }
                        }
                    }
                    catch (ArgumentNullException ex) { ret = new HttpResponseMessage() { StatusCode = HttpStatusCode.ServiceUnavailable, Content = null, ReasonPhrase = ex.Message, RequestMessage = null }; }
                    catch (WebException ex) { ret = new HttpResponseMessage() { StatusCode = HttpStatusCode.ServiceUnavailable, Content = null, ReasonPhrase = ex.Message, RequestMessage = null }; }
                    catch (NotSupportedException ex) { ret = new HttpResponseMessage() { StatusCode = HttpStatusCode.ServiceUnavailable, Content = null, ReasonPhrase = ex.Message, RequestMessage = null }; }
                    catch (InvalidOperationException ex) { ret = new HttpResponseMessage() { StatusCode = HttpStatusCode.ServiceUnavailable, Content = null, ReasonPhrase = ex.Message, RequestMessage = null }; }
                    catch (HttpRequestException ex) { ret = new HttpResponseMessage() { StatusCode = HttpStatusCode.ServiceUnavailable, Content = null, ReasonPhrase = ex.Message, RequestMessage = null }; }
                    catch (TaskCanceledException ex) { ret = new HttpResponseMessage() { StatusCode = HttpStatusCode.ServiceUnavailable, Content = null, ReasonPhrase = ex.Message, RequestMessage = null }; }
                }
            }
            return ret;
        }
        public enum ApiMethod
        {
            [Description("Incluir")]
            Put,
            [Description("Alterar")]
            Post,
            [Description("Excluir")]
            Delete,
            [Description("Selecionar/listar")]
            Get
        }
    }

}