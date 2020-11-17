namespace WEBAPI_VOPAK.Models
{
    using Newtonsoft.Json;

    public class ValidaBDCCViewModel
    {
        [JsonProperty("Ambiente")]
        public string Ambiente { get; set; }

        [JsonProperty("ambiente")]
        public string ambiente { get; set; }

        [JsonProperty("cpf")]
        public string cpf { get; set; }

        [JsonProperty("numDocumento")]
        public string numDocumento { get; set; }

        [JsonProperty("tipoDocumento")]
        public string tipoDocumento { get; set; }
    }
}
