using System.Collections.Generic;

namespace WebApiBusiness.Models
{
    public class RetornoBDCCIntegrada
    {
        public List<WebApiBusiness.Models.Result> Result { get; set; }

        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }
    }
}
