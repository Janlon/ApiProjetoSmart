namespace WEBAPI_VOPAK.Models
{
    using System;

    /// <summary>
    /// Modelo interno apenas para consulta ao status de uma O.S. cadastrada.
    /// </summary>
    internal class ConsultaOSViewModel
    {
        public int Id { get; set; } = 0; //,
        public int IdSecao { get; set; } = 0; //,
        public bool FlSaida { get; set; } = false; //,
        public DateTime? DtSaida { get; set; } = null; //,
        public DateTime? dtCancelamento { get; set; } = null; //
    }
}