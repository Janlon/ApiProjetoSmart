namespace WEBAPI_VOPAK.Models
{
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Net.NetworkInformation;
    using System.Threading.Tasks;
    using static WEBAPI_VOPAK.Models.ApiClient;

    /// <summary>
    /// Métodos de extensão para <see cref="PingReply"/>. 
    /// Muito específico para ficar nos genéricos.
    /// </summary>
    internal static class ApiHelpers
    {
        #region Métodos úteis
        /// <summary>
        /// Retorna a mensagem textul de um código de status Http de final de requisição.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string StatusMessage(this HttpStatusCode code)
        {
            string ret = "";
            switch (code)
            {
                case HttpStatusCode.Accepted: break;
                case HttpStatusCode.OK: break;
                case HttpStatusCode.PartialContent: break;
                case HttpStatusCode.Continue: break;
                case HttpStatusCode.Created: break;
                case HttpStatusCode.Found: break;
                case HttpStatusCode.Ambiguous: break;
                case HttpStatusCode.Moved: break;
                case HttpStatusCode.NoContent: break;
                case HttpStatusCode.NotModified: break;
                case HttpStatusCode.RedirectKeepVerb: break;
                case HttpStatusCode.RedirectMethod: break;
                case HttpStatusCode.BadGateway: ret = "Erro de gateway"; break;
                case HttpStatusCode.Conflict: ret = "Conflito do servidor"; break;
                case HttpStatusCode.ExpectationFailed: ret = "Cabeçalho inválido"; break;
                case HttpStatusCode.Forbidden: ret = "Ação não permitida"; break;
                case HttpStatusCode.GatewayTimeout: ret = "Tempo expirado"; break;
                case HttpStatusCode.Gone: ret = "Rota inválida"; break;
                case HttpStatusCode.HttpVersionNotSupported: ret = "Versão HTTP não suportada"; break;
                case HttpStatusCode.InternalServerError: ret = "Erro interno do servidor"; break;
                case HttpStatusCode.LengthRequired: ret = "Sem conteúdo"; break;
                case HttpStatusCode.MethodNotAllowed: ret = "O método não está habilitado"; break;
                case HttpStatusCode.NonAuthoritativeInformation: ret = "É necessário reenviar o login"; break;
                case HttpStatusCode.NotAcceptable: ret = "Cabeçalhos inválidos"; break;
                case HttpStatusCode.NotFound: ret = "Página não encontrada"; break;
                case HttpStatusCode.NotImplemented: ret = "Não implementado"; break;
                case HttpStatusCode.PaymentRequired: break;
                case HttpStatusCode.PreconditionFailed: ret = "Condição não satisfeita"; break;
                case HttpStatusCode.ProxyAuthenticationRequired: ret = "É necessário reenviar o token do proxy"; break;
                case HttpStatusCode.RequestedRangeNotSatisfiable: ret = "Requisição fora da faixa"; break;
                case HttpStatusCode.RequestEntityTooLarge: ret = "Requisição muito larga"; break;
                case HttpStatusCode.RequestTimeout: break;
                case HttpStatusCode.RequestUriTooLong: break;
                case HttpStatusCode.ResetContent: break;
                case HttpStatusCode.ServiceUnavailable: break;
            }
            return ret;
        }
      
      
        /// <summary>
        /// Texto para valor da enumeração.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static ApiMethod SwitchMethod(this string method)
        {
            switch (method.Trim().ToUpper())
            {
                case "POST": return ApiMethod.Post;
                case "PUT": return ApiMethod.Put;
                case "DELETE": return ApiMethod.Delete;
                default: return ApiMethod.Get;
            }
        }
        /// <summary>
        /// Retorna a mensagem de status do comando Ping.Send.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string IPStatusMessage(this IPStatus status)
        {
            string ret = "";
            switch (status)
            {
                case IPStatus.BadDestination: ret = $"A solicitação de eco ICMP falhou porque o endereço IP de destino não pode receber solicitações de eco ICMP ou nunca deve aparecer no campo de endereço de destino de qualquer datagrama IP."; break;
                case IPStatus.BadHeader: ret = $"A solicitação de eco ICMP falhou porque o cabeçalho é inválido."; break;
                case IPStatus.BadOption: ret = $"A solicitação de eco ICMP falhou porque ela contém uma opção inválida."; break;
                case IPStatus.BadRoute: ret = $"A solicitação de eco ICMP falhou porque não há nenhuma rota válida entre os computadores de origem e de destino."; break;
                case IPStatus.DestinationHostUnreachable: ret = $"A solicitação de eco ICMP falhou porque o computador de destino não está acessível."; break;
                case IPStatus.DestinationNetworkUnreachable: ret = $"A solicitação de eco ICMP falhou porque a rede que contém o computador de destino não está acessível."; break;
                case IPStatus.DestinationPortUnreachable: ret = $"A solicitação de eco ICMP falhou porque a porta no computador de destino não está disponível."; break;
                case IPStatus.DestinationProtocolUnreachable: ret = $"A solicitação de eco ICMP falhou porque o computador de destino especificado em uma mensagem de eco ICMP está inacessível porque ele não dá suporte ao protocolo do pacote.Esse valor se aplica somente ao IPv4.Esse valor é descrito no IETF RFC 1812 como Comunicação Proibida Administrativamente."; break;
                case IPStatus.DestinationScopeMismatch: ret = $"A solicitação de eco ICMP falhou porque o endereço de origem e o endereço de destino especificados em uma mensagem de eco ICMP não estão no mesmo escopo. Isso geralmente é causado por um roteador encaminhando um pacote usando uma interface que está fora do escopo do endereço de origem.Os escopos de endereço(vínculo local, site local e escopo global) determinam onde na rede um endereço é válido."; break;
                case IPStatus.DestinationUnreachable: ret = $"A solicitação de eco ICMP falhou porque o computador de destino especificado em uma mensagem de eco ICMP está inacessível, a causa exata do problema é desconhecida."; break;
                case IPStatus.HardwareError: ret = $"A solicitação de eco ICMP falhou devido a um erro de hardware."; break;
                case IPStatus.IcmpError: ret = $"A solicitação de eco ICMP falhou devido a um erro de protocolo ICMP."; break;
                case IPStatus.NoResources: ret = $"A solicitação de eco ICMP falhou devido a recursos de rede insuficientes."; break;
                case IPStatus.PacketTooBig: ret = $"A solicitação de eco ICMP falhou porque o pacote que contém a solicitação é maior do que a MTU (Unidade Máxima de Transmissão) de um nó(roteador ou gateway) localizado entre a origem e o destino.A MTU define o tamanho máximo de um pacote transmissível."; break;
                case IPStatus.ParameterProblem: ret = $"A solicitação de eco ICMP falhou porque um nó (roteador ou gateway) encontrou problemas ao processar o cabeçalho do pacote.Este será o status se, por exemplo, o cabeçalho contiver dados de campo inválidos ou uma opção não reconhecida."; break;
                case IPStatus.SourceQuench: ret = $"A solicitação de eco ICMP falhou porque o pacote foi descartado.Isso ocorre quando a fila de saída do computador de origem tem espaço de armazenamento insuficiente ou quando os pacotes chegam ao destino rápido demais para serem processados."; break;
                default:
                case IPStatus.Success: ret = $""; break;
                case IPStatus.TimedOut: ret = $"A resposta de eco do ICMP não foi recebida dentro do tempo alocado. O tempo padrão permitido para respostas é 5 segundos.Você pode alterar esse valor usando os métodos Send ou SendAsync que aceitam um parâmetro timeout."; break;
                case IPStatus.TimeExceeded: ret = $"A solicitação de eco ICMP falhou porque seu valor de TTL (vida útil) chegou a zero, fazendo o nó de encaminhamento (roteador ou gateway) descartar o pacote."; break;
                case IPStatus.TtlExpired: ret = $"A solicitação de eco ICMP falhou porque seu valor de TTL(vida útil) chegou a zero, fazendo o nó de encaminhamento(roteador ou gateway) descartar o pacote."; break;
                case IPStatus.TtlReassemblyTimeExceeded: ret = $"A solicitação de eco ICMP falhou porque o pacote foi dividido em fragmentos para transmissão e todos os fragmentos não foram recebidos dentro do tempo alocado para a remontagem.O RFC 2460 especifica 60 segundos como o limite de tempo no qual todos os fragmentos de pacote devem ser recebidos."; break;
                case IPStatus.Unknown: ret = $"A solicitação de eco ICMP falhou por motivo desconhecido."; break;
                case IPStatus.UnrecognizedNextHeader: ret = $"A solicitação de eco ICMP falhou porque o campo Próximo Cabeçalho não contém um valor reconhecido. O campo Próximo Cabeçalho indica o tipo de cabeçalho de extensão (se houver) ou o protocolo acima da camada IP, por exemplo, TCP ou UDP."; break;
            }
            return ret;
        }
        #endregion

        #region Propriedades internas
        /// <summary>
        /// Retorna o endereço de base do serviço de API.
        /// </summary>
        public static Uri UrlBase
        {
            get
            {
                if (ConfigurationManager.AppSettings["SmartApi"] != null)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SmartApi"]))
                        return new Uri(ConfigurationManager.AppSettings["SmartApi"]);
                    else
                        throw new ConfigurationErrorsException("A configuração para \"SmartApi\" não foi encontrada.");
                }
                else
                {
                    throw new ConfigurationErrorsException("A configuração para \"SmartApi\" não foi encontrada.");
                }
            }
        }
        /// <summary>
        /// Retorna se o serviço de API está ativo, 
        /// mediante um mero "ping" no IP de base.
        /// Obs.: O IP eventualmente é um IPV4 ou
        /// um IPV6, para esta rotina, não importa.
        /// </summary>
        public static bool IsAlive
        {
            get
            {
                bool ret = false;
                Uri uri = ApiHelpers.UrlBase;
                if (uri.IsWellFormedOriginalString())
                {
                    var ip = Dns.GetHostAddresses(uri.Host)[0];
                    string host = string.Format("{0}", ip);
                    Ping p = new Ping();
                    try
                    {
                        PingReply reply = p.Send(host, 3000);
                        ret = (reply.Status == IPStatus.Success);
                    }
                    catch (Exception) { ret = false; }
                }
                else
                {
                    throw new ConfigurationErrorsException("A configuração para \"SmartApi\" não foi encontrada.");
                }
                return ret;
            }
        }
        #endregion




    }

    public class ActionResult<T> where T : class
    {
        public DateTime Creation { get; set; } = DateTime.Now;
        public CrudAction Action { get; set; }
        public TimeSpan Delay { get; set; }
        public bool Sucess { get { return (Errors.Count == 0); } }
        public int Affected { get; set; }
        public dynamic Result { get; set; }
        public string Mensagem { get; set; }
        public object Origin { get; set; }
        Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    }

    public enum CrudAction
    {
        /// <summary>
        /// Nenhuma ação.
        /// </summary>
        [Display(Name = "Nenhuma ação")]
        None,
        /// <summary>
        /// Ação de inclusão.
        /// </summary>
        [Display(Name = "Inclusão de registros")]
        Insert,
        /// <summary>
        /// Ação de alteração dos dados.
        /// </summary>
        [Display(Name = "Atualização de registros")]
        Update,
        /// <summary>
        /// Ação de listagem completa.
        /// </summary>
        [Display(Name = "Lista completa de registros")]
        List,
        /// <summary>
        /// Ação de seleção de um registro.
        /// </summary>
        [Display(Name = "Seleção de um único registro")]
        Select,
        /// <summary>
        /// Ação de exclusão física (permanente) de um registro.
        /// </summary>
        [Display(Name = "Exclusão física de um registro")]
        Delete,
        /// <summary>
        /// Ação de exclusão lógica (reversível) de um registro.
        /// </summary>
        [Display(Name = "Exclusão lógica de um registro")]
        Deactivate,
        /// <summary>
        /// Ação de reversão de exclusão lógica de um registro.
        /// </summary>
        [Display(Name = "Restauração lógica de um registro")]
        Reactivate,
        /// <summary>
        /// Ação de filtragem de registros.
        /// </summary>
        [Display(Name = "Lista filtrada de registros")]
        Filter,
        /// <summary>
        /// Ações relacionadas à DML (CREATE).
        /// </summary>
        [Display(Name = "Criação de uma entidade de persistência (tabela, indice, visualização, procedimento etc)")]
        Create,
        /// <summary>
        /// Ações relacionadas à DML (DROP).
        /// </summary>
        [Display(Name = "Exclusão de uma entidade de persistência (tabela, indice, visualização, procedimento etc)")]
        Drop,
        /// <summary>
        /// Ações relacionadas à DIL (Importação/exportação).
        /// </summary>
        [Display(Name = "Exportação dos dados de uma consulta")]
        Export,
        /// <summary>
        /// Ações relacionadas à DIL (Importação/exportação).
        /// </summary>
        [Display(Name = "Exportação de todos os dados de um catálogo")]
        FullExport,
        /// <summary>
        /// Ações relacionadas à DIL (Importação/exportação).
        /// </summary>
        [Display(Name = "Importação dos dados de um arquivo")]
        Import,
        /// <summary>
        /// Ações relacionadas à DIL (Importação/exportação).
        /// </summary>
        [Display(Name = "Geração de uma cópia de segurança")]
        Backup
    }
}
