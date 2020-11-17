using Newtonsoft.Json;
using System;

namespace WEBAPI_VOPAK.Models
{

    public class EmpresaViewModel
    {
        [JsonProperty("idEmpresaIntegrador")]
        public int? idEmpresaIntegrador { get; set; } = 0;

        [JsonProperty("IdContratada")]
        public int? IdContratada { get; set; } = 0;

        [JsonProperty("cnpj")]
        public string cnpj { get; set; } = "";

        [JsonProperty("razaoSocial")]
        public string razaoSocial { get; set; } = "";

        [JsonProperty("nomeFantasia")]
        public string nomeFantasia { get; set; } = "";

        [JsonProperty("endereco")]
        public string endereco { get; set; } = "";

        [JsonProperty("numEndereco")]
        public string numEndereco { get; set; } = "";

        [JsonProperty("bairroEndereco")]
        public string bairroEndereco { get; set; } = "";

        [JsonProperty("cidade")]
        public string cidade { get; set; } = "";

        [JsonProperty("uf")]
        public string uf { get; set; } = "";

        [JsonProperty("cep")]
        public string cep { get; set; } = "";

        [JsonProperty("representante")]
        public string representante { get; set; } = "";

        [JsonProperty("emailRepresentante")]
        public string emailRepresentante { get; set; } = "";


        [JsonProperty("dtInicioValidade")]
        public string dtInicioValidade { get; set; } = "";

        [JsonProperty("ValidadeDtInicio")]
        public DateTime? ValidadeDtInicio { get; set; }

        [JsonProperty("dtFimValidade")]
        public string dtFimValidade { get; set; } = "";

        [JsonProperty("ValidadeDtTermino")]
        public DateTime? ValidadeDtTermino { get; set; }

        [JsonProperty("usuarioId")]
        public int? usuarioId { get; set; } = 0;

        [JsonProperty("cdExcluido")]
        public bool? cdExcluido { get; set; } = false;

        [JsonProperty("cdAtivo")]
        public bool? cdAtivo { get; set; } = true;

        [JsonProperty("idTipoOperacao")]
        public TipoDeOperacao idTipoOperacao
        {
            get
            {
                switch ("" + tipoOperacao + "".Normalized())
                {
                    case "A": return TipoDeOperacao.Alterar;
                    case "D": return TipoDeOperacao.Excluir;
                    case "R": return TipoDeOperacao.Ativar;
                    default: return TipoDeOperacao.Incluir;
                }
            }
        }

        [JsonProperty("tipoOperacao")]
        public string tipoOperacao { get; set; } = "";

        [JsonProperty("Ambiente")]
        public string ambiente { get; set; } = "";
    }
}