using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBAPI_VOPAK.Models
{
    public class LiberarBalancaDTO : Basica
    {
        public int IdEquipamento { get; set; }//IdEquipamento, Ip, NrPorta, UrlBaseService, TipoEth03, LabelLocal, Sentido,    Balanca,NumOs,Placa

        public string Ip { get; set; }

        public string NrPorta { get; set; }

        public string UrlBaseService { get; set; }

        public string TipoEth03 { get; set; }

        public string LabelLocal { get; set; }

        public string Sentido { get; set; }

        public string Balanca { get; set; }

        public string NumOs { get; set; }

        public string Placa { get; set; }
    }
}