namespace WEBAPI_VOPAK.Models
{
    using Newtonsoft.Json;

    public class EnderecoPostalViewModel
    {
        [JsonProperty("cep")]
        public string cep { get; set; } = "";

        [JsonProperty("logradouro")]
        public string logradouro { get; set; } = "";

        [JsonProperty("complemento")]
        public string complemento { get; set; } = "";

        [JsonProperty("bairro")]
        public string bairro { get; set; } = "";

        [JsonProperty("localidade")]
        public string localidade { get; set; } = "";

        [JsonProperty("uf")]
        public string uf { get; set; } = "";

        [JsonProperty("unidade")]
        public string unidade { get; set; } = "";

        [JsonProperty("ibge")]
        public string ibge { get; set; } = "";

        [JsonProperty("gia")]
        public string gia { get; set; } = "";
    }
}