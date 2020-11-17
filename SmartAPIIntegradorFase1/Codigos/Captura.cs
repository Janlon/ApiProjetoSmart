// Decompiled with JetBrains decompiler
// Type: WebApiBusiness.App_Data.Captura
// Assembly: WebApiBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2441E174-C302-4098-B419-99DA0C4BEE41
// Assembly location: G:\VOPAK\Source\sam\Api_Smart\bin\WebApiBusiness.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiBusiness.App_Data
{
    public class Captura : Basica
    {
        private int v1;
        private string v2;
        private string v3;

        public Captura(int v1, string v2, string v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        public Captura()
        {
        }



        [StringLength(6, ErrorMessage = "Tamanho máximo do Local 6")]
        public string Local { get; set; }

        public int IdLocal { get; set; }

        public int Pesagem { get; set; }

        public string TextoPlacaCarmen { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Placa { get; set; }

        public int Score { get; set; }

        public int IdOCR { get; set; }

        public int IdCapturaImagemOcr { get; set; }

        public byte[] Image { get; set; }

        public int IdSecao { get; set; }

        /// <summary>
        /// aLTERADI
        /// </summary>
        public string NumOs { get; set; } = "";
    }
}
