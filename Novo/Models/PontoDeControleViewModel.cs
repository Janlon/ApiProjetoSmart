using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBAPI_VOPAK.Models
{
    public class PontoDeControleViewModel:Basica
    {
        public string PontoControle { get; set; }

        public string CPF { get; set; }

        public string Nome { get; set; }

        public DateTime DtAcesso { get; set; }
    }
}