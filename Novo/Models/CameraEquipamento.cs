using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBAPI_VOPAK.Models
{
    public class CameraEquipamento
    {
        public int IdEquipamentoCamera { get; set; }

        public int IdEquipamento { get; set; }

        public string Ip { get; set; }

        public string DsLinkCameraVideo { get; set; }

        public string Conta { get; set; }

        public string Senha { get; set; }
    }
}