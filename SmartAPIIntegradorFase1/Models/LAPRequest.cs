using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiBusiness.Models
{
    [DisplayName("LAPSection")]
    [Description("Representa uma requisição LAP.")]
    public class LAPRequest
    {
        [Category("LAP")]
        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Identidade da seção de acesso (entrada/saída).", Name = "SectionID")]
        public int SectionID { get; set; } = 999999;

        [Category("LAP")]
        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Identidade da controladora.", Name = "DeviceID")]
        public int DeviceID { get; set; } = 9999999;

        [Category("LAP")]
        [StringLength(10, ErrorMessage = "Credencial inválida", MinimumLength = 8)]
        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Credencial que solicitou o acesso (entrada/saída).", Name = "Credencial")]
        public string Credencial { get; set; } = "";

        [Category("LAP")]
        [DisplayName("Placa")]
        [Description("Placa automotiva que solicitou o acesso (entrada/saída).")]
        [StringLength(10, ErrorMessage = "Placa inválida", MinimumLength = 8)]
        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Placa que solicitou o acesso (entrada/saída).", Name = "Placa")]
        public string Placa { get; set; } = "";

        [Category("LAP")]
        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Menor nota de corte para a comparação da OCR.", Name = "Score")]
        public string Score { get; set; } = "95";

        [Category("LAP")]
        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Lista de câmeras á serem acionadas.", Name = "Cameras")]
        public Camera[] Cameras { get; set; } = new Camera[0];
    }
}