using System.Collections.Generic;

namespace WebApiBusiness.Models
{
    public class RetornoApiIntegrada
    {
        public List<RetornoApiIntegrada.Resultado> Result = new List<RetornoApiIntegrada.Resultado>();

        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public class Resultado
        {
            private int _Code;
            private string _Message;

            public Resultado(int Code, string Message)
            {
                this.Code = Code;
                this.Message = Message;
            }

            public int Code
            {
                get
                {
                    return this._Code;
                }
                set
                {
                    this._Code = value;
                }
            }

            public string Message
            {
                get
                {
                    return this._Message;
                }
                set
                {
                    this._Message = value;
                }
            }
        }
    }
}