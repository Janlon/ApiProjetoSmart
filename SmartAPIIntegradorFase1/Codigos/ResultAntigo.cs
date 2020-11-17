using System.Collections.Generic;

namespace WebApiBusiness.Unisolution
{
    public class ResultAntigo
    {
        public int statusCode { get; set; }

        public List<string> statusMessages { get; set; }

        public string result { get; set; }
    }
}