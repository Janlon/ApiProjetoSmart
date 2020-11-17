namespace WEBAPI_VOPAK.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using WEBAPI_VOPAK.Models.Componentes;

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

        public Captura() { }

        public int NumOs { get; set; } = 0;

        [StringLength(6, ErrorMessage = "Tamanho máximo do Local 6")]
        public string Local { get; set; } = "";

        public int IdLocal { get; set; } = 0;

        public int Pesagem { get; set; } = 0;

        public string TextoPlacaCarmen { get; set; } = "";

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public string Placa { get; set; } = "";

        public int Score { get; set; } = 0;

        public int IdOCR { get; set; } = 0;

        public int IdCapturaImagemOcr { get; set; } = 0;

        public byte[] Image { get; set; } = CameraUtil.SemImagemBytes;

        public int IdSecao { get; set; } = 0;
    }
}