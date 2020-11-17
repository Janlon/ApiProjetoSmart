using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBAPI_VOPAK.Models
{
    public class RetornoDaConsultaAcesso
    {
        public string Cpf { get; set; } = "";
        public string Nome { get; set; } = "";
        public string DtAcesso { get; set; } = "";

        [JsonIgnore]
        public DateTime? Acesso { get; set; }
    }
}