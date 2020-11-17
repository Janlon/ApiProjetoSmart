namespace WEBAPI_VOPAK
{
    using Microsoft.Owin;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;

    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using WEBAPI_VOPAK.Models;

    [CORSEnabled()]
    [EnableCors("*", "*", "*")]
    [AllowAnonymous]
    public class SmartAPIController : ApiController
    {

        /// <summary>
        /// Retorna ao encontrar algum erro de validação.
        /// </summary>
        /// <param name="ret">Objeto do tipo <see cref="Retorno"/>.</param>
        /// <returns>
        /// Mensagem de retorno (<see cref="HttpResponseMessage"/>) no padrão 
        /// contratado pela documentação do SmartAPI.
        /// </returns>
        public HttpResponseMessage SmartRetornoImediato(Retorno ret)
        {
            RetornoMensagem temp = null;
            var item = ret.Result.FirstOrDefault();
            Type t = ret.Result.GetType();
            Type tInner = item == null ? null : item.GetType();
            if ((tInner != null) && !(item is Mensagens))
                temp = new RetornoMensagem()
                {
                    Result = ret.Result
                };
            if ((tInner == null) && (item != null))
                temp = new RetornoMensagem()
                {
                    Result = ret.Result
                };
            if ((tInner != null) && (item is Mensagens))
                temp = new RetornoMensagem()
                { Result = ret.Result };
            if (temp != null)
                if (temp.Result == null)
                    temp.Result = "Registro atualizado com sucesso";

            if(temp!=null)
                return Request
                    .CreateResponse<RetornoMensagem>(temp);
            return Request
                    .CreateResponse<Retorno>(ret);
        }

        /// <summary>
        /// Retorna ao encontrar algum erro de validação.
        /// </summary>
        /// <param name="ret">Objeto do tipo <see cref="Retorno"/>.</param>
        /// <returns>
        /// Mensagem de retorno (<see cref="HttpResponseMessage"/>) no padrão 
        /// contratado pela documentação do SmartAPI.
        /// </returns>
        public HttpResponseMessage SmartRetorno(RetornoMensagem ret)
        {
            return Request
                    .CreateResponse<RetornoMensagem>(ret);
        }

        public IHttpActionResult CustomBadRequest<T>(T value)
        {
            ContentNegotiationResult negotiationResult = this.Configuration.Services.GetContentNegotiator().Negotiate(typeof(T), this.Request, (IEnumerable<MediaTypeFormatter>)this.Configuration.Formatters);
            return (IHttpActionResult)this.ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = (HttpContent)new ObjectContent<T>(value, negotiationResult.Formatter, negotiationResult.MediaType)
            });
        }

        /// <summary>
        /// Valida o tipo de operação solicitado.
        /// </summary>
        /// <param name="tipoOperacao"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public Retorno SmartValidaTipoDeOperacao(string tipoOperacao="", bool all = false)
        {
            Retorno ret = new Retorno();
            string pattern = all ? "AIDR" : "I";
            // O tipo de operação informado é válido?
            ValidLenghtResult temp = tipoOperacao.HasValidLenght(1, 1, pattern, true);
            switch (temp)
            {
                case ValidLenghtResult.IsEmpty:
                case ValidLenghtResult.IsNull:
                    ret.AddResult(ErroSmartApi.TipoOperacaoObrigatorio);
                    break;
                case ValidLenghtResult.IsGreater:
                case ValidLenghtResult.IsLower:
                case ValidLenghtResult.IsInvalid:
                    ret.AddResult(ErroSmartApi.TipoOperacaoInvalido);
                    break;
                default: break;
            }
            return ret;
        }

        /// <summary>
        /// Valida os parâmetros de empresa.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Objeto do tipo <see cref="Retorno"/>.</returns>
        public Retorno SmartValidaParametrosDaEmpresa(string ambiente, int idEmpresaIntegrador=0,string cnpj="", string razaoSocial="", string nomeFantasia="", string representante = "", string emailRepresentante = "", string logradouro="", string numero="", string bairro="", string cidade="", string uf="", string cep="")
        {
            Retorno ret = new Retorno();
            // A identificação de registro externo da empresa foi informada?
            if (idEmpresaIntegrador == 0)
                ret.AddResult(ErroSmartApi.IdEmpresaIntegradorObrigatorio);
            // O CNPJ informado é válido, e foi devidamente informado?
            if (string.IsNullOrEmpty(cnpj))
                ret.AddResult(ErroSmartApi.CNPJObrigatorio);
            if (!cnpj.IsBrCnpj())
                ret.AddResult(ErroSmartApi.CNPJInvalido);
            // A razão informada é válida e foi informada?
            if (string.IsNullOrEmpty(razaoSocial))
                ret.AddResult(ErroSmartApi.RazaoSocialObrigtorio);
            if (razaoSocial.IsBiggerThan(50, true))
                ret.AddResult(ErroSmartApi.RazaoSocialTamanhoExcedido);
            // O nome fantasia, se informado, é válido?
            if (nomeFantasia.IsBiggerThan(50))
                ret.AddResult(ErroSmartApi.NomeFantasiaTamanhoExcedido);
            // O endereço (logradouro), se informado, é válido?
            if (logradouro.IsBiggerThan(150))
                ret.AddResult(ErroSmartApi.EnderecoTamanhoExcedido);
            // O número do logradouro, se informado, é válido? 
            // (Obs.: Aqui deveria haver mais espaço disponível no campo e isso pode causar problemas.
            //  Por exemplo, se o numEndereco contém os complementos do endereço - já que esse campo não 
            //  existe, pode ser algo como: "12345, Conjunto Orquídea, Bloco C5, Sala 1234" o que daria
            //  45 caracteres, quando o campo só aceita até 10. Lembrando que São Paulo, Brasília e 
            //  Rio de Janeiro possuem muitos complementos similares ao do exemplo, até maiores.)
            if (numero.IsBiggerThan(10))
                ret.AddResult(ErroSmartApi.NumEnderecoTamanhoExcedido);
            // O bairro informado, se informado, é válido?
            if (bairro.IsBiggerThan(50))
                ret.AddResult(ErroSmartApi.BairroEnderecoTamanhoExcedido);
            // A cidade informada é válida?
            if (cidade.IsBiggerThan(50, true))
                ret.AddResult(ErroSmartApi.CidadeObrigatorio);
            // A UF informada é válida?
            if (!uf.IsBrUF())
                ret.AddResult(ErroSmartApi.UFInvalido);
            // O CEP, se informado, é válido?
            if (!cep.IsBrZIP(false, false))
                ret.AddResult(ErroSmartApi.CEPInvalido);
            // O representante é válido, se informado?
            if (representante.IsBiggerThan(100))
                ret.AddResult(ErroSmartApi.RepresentanteTamanhoExcedido);
            //// O email do representante é válido, se informado?
            //if (!string.IsNullOrEmpty(emailRepresentante))
            //    if (emailRepresentante.IsBiggerThan(100))
            //        ret.AddResult(ErroSmartApi.EmailRepresentanteTamanhoExcedido);
            //    else
            //    {
            //        if (!emailRepresentante.IsEmail())
            //            ret.AddResult(ErroSmartApi.EmailRepresentanteInvalido);
            //    }
            // O CNPJ informado foi cadastrado para outra empresa?
            if (Dados.ChecaCnpjDuplicado(ambiente, cnpj, idEmpresaIntegrador, true))
                ret.AddResult(ErroSmartApi.CNPJDuplicado);
            // A razão social foi cadastrada para outra empresa?
            //if (Dados.ChecaRazaoDuplicada(ambiente, cnpj, idEmpresaIntegrador, true))
            //    ret.AddResult(ErroSmartApi.RazaoSocialDuplicada);
            return ret;
        }


        /// <summary>
        /// Envia o objeto de requisição ao banco de dados e retorna se de fato foi recebida.
        /// </summary>
        /// <param name="requestItem">Dinâmico. Requisição.</param>
        /// <returns>Booleano.</returns>
        public Retorno SmartValidaRequisicao(dynamic requestItem)
        {
            Retorno ret = new Retorno();
            try
            {
                bool temp = (requestItem != null);
                SmartGravaRequisicao(requestItem);
                if (!temp)
                    ret.AddResult(ErroSmartApi.NenhumDadoInformado);
            }
            catch (Exception ex) { ex.Log(); ret = new Retorno(ErroSmartApi.ErroNaoEspecificado); }
            return ret;
        }

        /// <summary>
        /// Envia o objeto de requisição ao banco de dados.
        /// </summary>
        /// <param name="requestItem"></param>
        /// <returns></returns>
        private bool SmartGravaRequisicao(dynamic requestItem)
        {
            bool ret = false;
            try
            {
                string IP = "";
                if(IP == "")
                    if (Request.Properties.ContainsKey("MS_HttpContext"))                
                        IP= ((HttpContextBase)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                if (IP == "")
                    if (Request.Properties.ContainsKey(System.ServiceModel.Channels.RemoteEndpointMessageProperty.Name))
                        IP= ((System.ServiceModel.Channels.RemoteEndpointMessageProperty)Request.Properties[System.ServiceModel.Channels.RemoteEndpointMessageProperty.Name]).Address;
                if (IP == "")
                    if (Request.Properties.ContainsKey("MS_OwinContext"))
                        IP = ((OwinContext)Request.Properties["MS_OwinContext"]).Request.RemoteIpAddress;
                //HttpContextWrapper property = Request.Properties["MS_HttpContext"] as HttpContextWrapper;
                //if (property != null)
                    //IP = property.Request.UserHostAddress;
                if (!string.IsNullOrEmpty(IP))
                    ret = Dados.InsereRequisicao(requestItem.Ambiente, JsonConvert.SerializeObject(requestItem), IP);
            }
            catch (Exception ex){ ex.Log(); ret = false; }
            return ret;
        }

        /// <summary>
        /// Valida os parâmetros de documentos de um colaborador enviado por requisição.
        /// </summary>
        /// <param name="ambiente"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="cpf"></param>
        /// <param name="numDocumento"></param>
        /// <returns>Objeto do tipo <see cref="Retorno"/>.</returns>
        public Retorno SmartValidaDocumentosDoColaborador(string ambiente = "", string tipoDocumento = "", string cpf = "", string numDocumento = "")
        {
            Retorno ret = new Retorno();
            // Pegar o ID do tipo de documento enviado, para poder validar os parâmetros.
            int wIdTipoDocumento = 0;
            // CPF não informado.
            if ((string.IsNullOrEmpty(cpf)))
                ret.AddResult(ErroSmartApi.CPFObrigatorio);
            else
            {
                if ((cpf.JustNumbers() != cpf.Trim()))
                    ret.AddResult(ErroSmartApi.CpfInvalido);
            }
            // Tipo de documento excede.
            if (tipoDocumento.Length > 10)
                ret.AddResult(ErroSmartApi.TipoDocumentoTamanhoExcedido);
            else
                // Carregou o tipo. ///cpf = 1
                wIdTipoDocumento = Dados.GetIdTipoDocumento(ambiente, tipoDocumento);
            // Não tem tipo e não é um CPF.
            if ((wIdTipoDocumento == 0) && (!cpf.IsBrCpf()))
                ret.AddResult(ErroSmartApi.TipoDocumentoInvalido);
            // Tem um tipo e não tem um CPF
            if (string.IsNullOrEmpty(tipoDocumento) && string.IsNullOrEmpty(cpf))
                ret.AddResult(ErroSmartApi.TipoDocumentoObrigatorio);
            // Tem o tipo válido e o CPF foi preencido: O tipo é RG ou RGE?
            if (wIdTipoDocumento > 1 && (!string.IsNullOrEmpty(cpf)))
                if (tipoDocumento.Trim().ToUpper() != "RG" && tipoDocumento.Trim().ToUpper() != "RNE")
                    ret.AddResult(ErroSmartApi.TipoDocumentoInvalido);
            // NumDocumento obrigatório quando o CPF não é um CPF.
            if (string.IsNullOrEmpty(numDocumento) && (wIdTipoDocumento > 0) && (!cpf.IsBrCpf()))
                ret.AddResult(ErroSmartApi.TipoDocumentoNumDocumentoObrigatorios);
            else
            {
                // Tamanho do numDocumento inválido.
                if (wIdTipoDocumento > 1 && (numDocumento.Length < 2 || numDocumento.Length > 20))
                    ret.AddResult(ErroSmartApi.NumDocumentoTamanhoExcedido);
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ambiente"></param>
        /// <param name="tipoOperacao"></param>
        /// <param name="idEmpresaIntegrador"></param>
        /// <param name="idColaboradorIntegrador"></param>
        /// <param name="nome"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="numDocumento"></param>
        /// <param name="cpf"></param>
        /// <param name="cnh"></param>
        /// <param name="orgaoEmissorCNH"></param>
        /// <param name="ufEmissorCNH"></param>
        /// <param name="sexo"></param>
        /// <param name="dtNascimento"></param>
        /// <param name="tel"></param>
        /// <param name="email"></param>
        /// <param name="cracha"></param>
        /// <param name="foto"></param>
        /// <param name="endereco"></param>
        /// <param name="numero"></param>
        /// <param name="bairro"></param>
        /// <param name="cidade"></param>
        /// <param name="uf"></param>
        /// <param name="cep"></param>
        /// <returns></returns>
        public Retorno SmartValidaParametrosDoColaborador(string ambiente = "", string tipoOperacao = "", int idEmpresaIntegrador = 0, int idColaboradorIntegrador = 0, string nome="", string sexo = "", string dtNascimento = "", string tel = "", string email = "", string cracha = "", string foto = "", string tipoDocumento="", string numDocumento="", string cpf="", string cnh = "", string orgaoEmissorCNH = "", string ufEmissorCNH = "", string endereco = "", string numero = "", string bairro = "", string cidade = "", string uf = "", string cep = "")
        {
            Retorno ret = new Retorno();
            // Tipo de operação.
            ValidLenghtResult temp = tipoOperacao.HasValidLenght(1, 1, "IADR", false);
            switch (temp)
            {
                case ValidLenghtResult.IsNull:
                case ValidLenghtResult.IsEmpty:
                    ret.AddResult(ErroSmartApi.TipoOperacaoObrigatorio);
                    break;
                case ValidLenghtResult.IsGreater:
                case ValidLenghtResult.IsInvalid:
                case ValidLenghtResult.IsLower:
                    ret.AddResult(ErroSmartApi.TipoOperacaoInvalido);
                    break;
                case ValidLenghtResult.Ok:
                    break;
            }
            // Contratada
            if (idEmpresaIntegrador == 0)
                ret.AddResult(ErroSmartApi.IdEmpresaIntegradorObrigatorio);
            // Colaborador
            if (idColaboradorIntegrador == 0)
                ret.AddResult(ErroSmartApi.IdColaboradorIntegradorObrigatorio);
            // Sexo
            temp = sexo.HasValidLenght(1, 1, "FM");
            switch (temp)
            {
                case ValidLenghtResult.IsEmpty:
                case ValidLenghtResult.IsNull:
                    ret.AddResult(ErroSmartApi.SexoNaoInformado);
                    break;
                case ValidLenghtResult.IsGreater:
                case ValidLenghtResult.IsLower:
                case ValidLenghtResult.IsInvalid:
                    ret.AddResult(ErroSmartApi.SexoTamanhoExcedido);
                    break;
                default: break;
            }
            //// Nascimento
            //if (dtNascimento != null)
            //{
            //    if (dtNascimento.IsDate() == false)
            //        ret.AddResult(ErroSmartApi.DtNascimentoInvalido);
            //    else
            //        dtNascimento = DateTime.Parse(dtNascimento).ToString("yyyy-MM-dd");
            //}
            //else
            //    dtNascimento = "";
            // Endereço
            if (endereco != null)
            {
                if (endereco.Length > 150)
                    ret.AddResult(ErroSmartApi.EnderecoTamanhoExcedido);
            }
            else
                endereco = "";
            // Número do endereço
            if (numero != null)
            {
                if (numero.Length > 10)
                    ret.AddResult(ErroSmartApi.NumEnderecoTamanhoExcedido);
            }
            else
                numero = "";
            // Bairro
            if (bairro != null)
            {
                if (bairro.Length > 50)
                    ret.AddResult(ErroSmartApi.BairroEnderecoTamanhoExcedido);
            }
            else
                bairro = "";
            // Cidade
            if (string.IsNullOrEmpty(cidade) || string.IsNullOrWhiteSpace(cidade))
                ret.AddResult(ErroSmartApi.CidadeObrigatorio);
            else
            {
                if (cidade.Length > 50)
                    ret.AddResult(ErroSmartApi.CidadeTamanhoExcedido);
            }
            // UF
            if (string.IsNullOrWhiteSpace(uf) || string.IsNullOrEmpty(uf))
                ret.AddResult(ErroSmartApi.UFObrigatorio);
            else
            {
                if (uf.Length != 2)
                    ret.AddResult(ErroSmartApi.UFTamanhoExcedido);
                if (!uf.IsBrUF())
                    ret.AddResult(ErroSmartApi.UFInvalido);
            }
            // CEP
            if (!string.IsNullOrEmpty(cep))
            {
                if (cep.Length > 8 || cep.Length < 8)
                    ret.AddResult(ErroSmartApi.CEPTamanhoExcedido);
                else
                {
                    if (cep.JustNumbers() != cep.Trim())
                        ret.AddResult(ErroSmartApi.CEPSomenteNumeros);
                    if (!cep.IsBrZIP())
                        ret.AddResult(ErroSmartApi.CEPInvalido);
                }
            }
            else
                cep = "";
            // Telefone
            if (!string.IsNullOrEmpty(tel) && !string.IsNullOrWhiteSpace(tel))
            {
                if (!(tel.Trim().Length > 0 || tel.Trim().Length <= 13))
                    ret.AddResult(ErroSmartApi.TelTamanhoExcedido);
                else
                    if ((tel.JustNumbers() != tel) && (tel.JustNumbers().Length > 0))
                    ret.AddResult(ErroSmartApi.TelSomenteNumeros);
            }
            else
                tel = "";
            //// Endereço de email
            //if (!string.IsNullOrEmpty(email))
            //    if (email.Length > 100)
            //        ret.AddResult(ErroSmartApi.EmailColaboradorTamanhoExcedido);
            //    else
            //    {
            //        if (email.IsEmail() == false)
            //            ret.AddResult(ErroSmartApi.EmailColaboradorInvalido);
            //    }
            //else
            //    email = "";
            //int idCracha = 0;
            // Crachá
            if (!string.IsNullOrEmpty(cracha))
            {
                if (cracha.Length > 10)
                    ret.AddResult(ErroSmartApi.NumeroCrachaTamanhoExcedido);
                else
                {
                    if (cracha.Equals("0000000000"))
                        cracha = "FFFFFFFFFF";
                    
                    //if (tipoOperacao == "I")
                    //{

                    //    idCracha = Dados.GetIdCracha(ambiente, cracha, false);
                    //  if (idCracha > 0)
                    //    ret.AddResult(ErroSmartApi.NumeroCrachaJaExiste);
                    //}
                }
            }
            else
            {
                cracha = "FFFFFFFFFF";
            }
            // CNH
            if (!string.IsNullOrEmpty(cnh))
            {
                if (cnh.Length > 11)
                    ret.AddResult(ErroSmartApi.CNHTamanhoExcedido);
                if (cnh.JustNumbers() != cnh)
                    ret.AddResult(ErroSmartApi.CNHInvalido);
            }
            else
                ret.AddResult(ErroSmartApi.CNHObrigatorio);
            // Órgão emissor do CNH (Detran ou SSP)
            if (!string.IsNullOrEmpty(orgaoEmissorCNH))
            {
                if (orgaoEmissorCNH.Length > 6)
                    orgaoEmissorCNH = orgaoEmissorCNH.Substring(0, 6);
                // ret.AddResult(ErroSmartApi.OrgaoEmissorCNHTamanhoExcedido);
            }
            else
                orgaoEmissorCNH = "";
            // Emissor do CNH (SP ou MS ou RJ ...)
            if (!string.IsNullOrEmpty(ufEmissorCNH))
            {
                if (ufEmissorCNH.Length > 6)
                    ufEmissorCNH = ufEmissorCNH.Substring(0, 6);
                //ret.AddResult(ErroSmartApi.EmissorCNHTamanhoExcedido);
            }
            //// Foto
            //if ((foto == null) || (foto.Length == 0))
            //    ret.AddResult(ErroSmartApi.FotoObrigatorio);

            // Pegar o ID do tipo de documento enviado, para poder validar os parâmetros.
            //int wIdTipoDocumento = Dados.GetIdTipoDocumento(ambiente, tipoDocumento);

            int wIdTipoDocumento = 0;
            // Tipo de documento informado.
            if (string.IsNullOrEmpty(tipoDocumento))
                tipoDocumento = "";
            else
            {
                if (tipoDocumento.Length > 10)
                    ret.AddResult(ErroSmartApi.TipoDocumentoTamanhoExcedido);
                else
                    wIdTipoDocumento = Dados.GetIdTipoDocumento(ambiente, tipoDocumento);
            }

            if ((wIdTipoDocumento == 0) && (!cpf.IsBrCpf()))
                ret.AddResult(ErroSmartApi.CpfInvalido);
            if ((string.IsNullOrEmpty(tipoDocumento)) && (string.IsNullOrEmpty(cpf)))
                ret.AddResult(ErroSmartApi.TipoDocumentoObrigatorio);
            if ((wIdTipoDocumento > 0) && (!string.IsNullOrEmpty(cpf)))
            {
                if ((idColaboradorIntegrador.ToString().Trim() != cpf.Trim()))
                    ret.AddResult(ErroSmartApi.CPFIdColaboradorComTipoDocumento);
                else
                {
                    if (tipoDocumento.Trim().ToUpper() != "RG" && tipoDocumento.Trim().ToUpper() != "RNE")
                        ret.AddResult(ErroSmartApi.TipoDocumentoInvalido);
                }
            }
            // Numero do documento.
            if (string.IsNullOrEmpty(numDocumento) && (wIdTipoDocumento > 0) && (!cpf.IsBrCpf()))
                ret.AddResult(ErroSmartApi.TipoDocumentoNumDocumentoObrigatorios);
            else
            {
                if (wIdTipoDocumento > 0 && (numDocumento.Length < 2 || numDocumento.Length > 20))
                    ret.AddResult(ErroSmartApi.NumDocumentoTamanhoExcedido);
                // Tipo de dcoumento / CPF válido:
                if ((wIdTipoDocumento == 0) && (cpf.IsBrCpf()))
                    wIdTipoDocumento = Dados.GetIdTipoDocumento(ambiente);
                // Tipo de dcoumento / numDocumento
                if (Dados.NumDocumentoDuplicado(ambiente, wIdTipoDocumento, numDocumento, cpf))
                    ret.AddResult(ErroSmartApi.NumDocumentoDuplicado);
            }
            // Nome
            if (string.IsNullOrEmpty(nome))
                ret.AddResult(ErroSmartApi.NomeObrigatorio);
            else
            {
                if (nome.Length > 50)
                    ret.AddResult(ErroSmartApi.NomeTamanhoExcedido);
            }
            return ret;
        }

        /// <summary>
        /// Valida os parâmetros enviados para a consulta de acessos de um equipamento.
        /// </summary>
        /// <param name="item">Requisição do tipo <see cref="PontoDeControleViewModel"/>.</param>
        /// <returns>Objeto do tipo <see cref="Retorno"/>.</returns>
        public Retorno SmartValidaConsultaAcesso(PontoDeControleViewModel item)
        {
            Retorno ret = new Retorno();
            // Validar a descrição do equipamento enviada:
            ValidLenghtResult temp = item.PontoControle.HasValidLenght(1, 20);
            switch (temp)
            {
                case ValidLenghtResult.IsEmpty:
                case ValidLenghtResult.IsNull:
                    ret.AddResult(ErroSmartApi.EquipamentoObrigatorio);
                    break;
                case ValidLenghtResult.IsInvalid:
                case ValidLenghtResult.IsLower:
                case ValidLenghtResult.IsGreater:
                    ret.AddResult(ErroSmartApi.EquipamentoTamanhoInvalido);
                    break;
                case ValidLenghtResult.Ok:
                default:
                    // Se está em ordem (foi informado e é válido), saber se o equipamento existe:
                    if (!Dados.EquipamentoExiste(item.Ambiente, item.PontoControle))
                        ret.AddResult(ErroSmartApi.EquipamentoNaoCadastrado);
                    break;
            }
            return ret;
        }

        /// <summary>
        /// Valida o ambiente da requisição.
        /// </summary>
        /// <param name="ambiente">Parâmetro "ambiente" da requisição.</param>
        /// <returns>Objeto do tipo <see cref="Retorno"/>.</returns>
        public Retorno SmartValidaAmbiente(string ambiente)
        {
            Retorno ret = new Retorno();
            // Ambiente
            ValidLenghtResult temp = ("" + ambiente + "").HasValidLenght(1, 1, "TPtp", true);
            switch (temp)
            {
                case ValidLenghtResult.IsNull:
                case ValidLenghtResult.IsEmpty:
                    ret.AddResult(ErroSmartApi.AmbienteObrigatorio);
                    break;
                case ValidLenghtResult.IsGreater:
                case ValidLenghtResult.IsInvalid:
                case ValidLenghtResult.IsLower:
                    ret.AddResult(ErroSmartApi.AmbienteInvalido);
                    break;
                case ValidLenghtResult.Ok:
                    break;
            }
            if (ret.Ok())
            {
                string ambiente2 = ambiente.DefaultIfNull("P");
                if (!Dados.CanConnect(ambiente2))
                {
                    ret.AddResult(ambiente2 == "P" ?
                         ErroSmartApi.AmbienteIndisponivelProducao :
                         ErroSmartApi.AmbienteIndisponivelTeste);
                    return ret;
                }
            }
            return ret;
        }

        /// <summary>
        /// Validar os parâmetros da requisição de liberação das balanças.
        /// </summary>
        /// <param name="item">Requisição do tipo <see cref="LiberarBalancaDTO"/>.</param>
        /// <returns>Objeto do tipo <see cref="Retorno"/>.</returns>
        internal Retorno SmartValidaParametrosLiberarBalanca(LiberarBalancaDTO item)
        {
            Retorno ret = new Retorno();
            // Balanca:
            if (string.IsNullOrEmpty(item.Balanca) || string.IsNullOrWhiteSpace(item.Balanca))
                ret.AddResult(ErroSmartApi.BalancaObrigatorio);
            ValidLenghtResult temp = item.Balanca.HasValidLenght(1, 20);
            switch (temp)
            {
                case ValidLenghtResult.IsEmpty:
                case ValidLenghtResult.IsNull:
                    ret.AddResult(ErroSmartApi.BalancaObrigatorio);
                    break;
                case ValidLenghtResult.IsInvalid:
                case ValidLenghtResult.IsLower:
                case ValidLenghtResult.IsGreater:
                    ret.AddResult(ErroSmartApi.BalancaInvalido);
                    break;
                default: break;
            }
            // Número da O.S.:
            if (string.IsNullOrEmpty(item.NumOs) || string.IsNullOrWhiteSpace(item.NumOs))
                ret.AddResult(ErroSmartApi.NumOsObrigatorio);
            temp = item.NumOs.HasValidLenght(1, 20);
            switch (temp)
            {
                case ValidLenghtResult.IsEmpty:
                case ValidLenghtResult.IsNull:
                    ret.AddResult(ErroSmartApi.NumOsObrigatorio);
                    break;
                case ValidLenghtResult.IsInvalid:
                case ValidLenghtResult.IsLower:
                case ValidLenghtResult.IsGreater:
                    ret.AddResult(ErroSmartApi.NumOsTamanhoExcedido);
                    break;
                default: break;
            }
            return ret;
        }


        /// <summary>
        /// Trata o objeto <see cref="Exception"/> indicado e retorna um 
        /// <see cref="HttpResponseMessage"/> com a mensagem de erro padrão.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public HttpResponseMessage SmartTrataErro(Exception ex)
        {
            ex.Log();
            return Request.CreateResponse<Retorno>(new Retorno(ErroSmartApi.ErroNaoEspecificado));
        }

    }
}