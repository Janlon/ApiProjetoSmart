namespace WEBAPI_VOPAK
{
    using Newtonsoft.Json;


    /// <summary>
    /// Resposta de processamentos com mensagens de erro, ou de sucesso, apenas.
    /// </summary>
    public class Mensagens
    {

        internal int nrCode { get; set; } = 0;

        internal Mensagens(int Code, string Message)
        {
            nrCode = Code;
            this.Message = Message;
        }

        [JsonProperty("Code")]
        public string Code { get { return nrCode.ToString().PadLeft(4, '0'); } }

        [JsonProperty("Message")]
        public string Message { get; internal set; } = "";
    }


}