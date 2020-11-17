using System.Collections.Generic;

namespace WebApiBusiness.Unisolution
{
    public class RootObject
    {
        public int statusCode { get; set; }

        public List<string> statusMessages { get; set; }

        public Result result { get; set; }
    }
}
