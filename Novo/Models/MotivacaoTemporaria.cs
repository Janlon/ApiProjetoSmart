namespace WEBAPI_VOPAK.Models
{
    using System;

    public class MotivacaoTemporaria
    {
        public int Id { get; set; } = 0;
        public int IdColaborador { get; set; } = 0;
        public int IdCracha { get; set; } = 0;
        public DateTime DtCadastro { get; set; } = DateTime.Now;
        public string OrdemServico { get; set; } = "";
        public string Placa { get; set; } = "";
        public bool FlSaida { get; set; } = false;
        public DateTime DtValidadeInicial { get; set; } = DateTime.Now;
        public DateTime DtValidadeFinal { get; set; } = DateTime.Now;
        public DateTime DtSaida { get; set; } = DateTime.Now;
        public DateTime dtCancelamento { get; set; } = DateTime.Now;
        public int IdContratada { get; set; } = 0;
        public string Finalizador { get; set; } = "";
        public bool FlagPermiteSaida { get; set; } = false;
    }
}