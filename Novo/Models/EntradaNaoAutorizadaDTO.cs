using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBAPI_VOPAK.Models
{

    public class EntradaNaoAutorizadaDTO : Basica
    {
        public string CPF { get; set; }

        public string PlacaOs { get; set; }

        public string PlacaOcr { get; set; }

        public string NumDocumento { get; set; }

        public string NumOs { get; set; }
    }
}