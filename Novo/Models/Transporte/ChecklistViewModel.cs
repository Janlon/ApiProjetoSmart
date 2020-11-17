namespace WEBAPI_VOPAK.Models
{
    using System;
    public class ChecklistViewModel
    {
        public int IdChecklist { get; set; }
        public string NrPlaca { get; set; }
        public DateTime? DtCadastro { get; set; }
        public DateTime? DtValidade { get; set; }
        public bool IsValid { get; set; }
    }
}
