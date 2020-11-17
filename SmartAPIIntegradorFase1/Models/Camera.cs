using System.ComponentModel.DataAnnotations;

namespace WebApiBusiness.Models
{
    public class Camera
    {
        private int mrotacao;

        [Range(1, 10)]
        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Posição da câmera no totem.", Name = "Posicao")]
        public int Posicao { get; set; }

        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Endereço (URI) do serviço de imagens do dispositivo.", Name = "Servico")]
        public string Servico { get; set; }

        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Informa se há detecção facial (\"1\") neste dispositivo, ou não (\"0\").", Name = "DeteccaoFacial")]
        public string DeteccaoFacial { get; set; }

        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Informa se há reconhecimento facial (\"1\") neste dispositivo, ou não (\"0\").", Name = "ReconhecimentoFacial")]
        public string ReconhecimentoFacial { get; set; }

        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Informa o endereço de IP deste dispositivo.", Name = "IP")]
        public string IP { get; set; }

        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Informa o nome da conta de acesso ao serviço de imagens deste dispositivo.", Name = "Conta")]
        public string Conta { get; set; }

        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Informa a senha da conta de acesso ao serviço de imagens deste dispositivo.", Name = "Conta")]
        public string Senha { get; set; }

        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Informa a porta de acesso ao serviço de imagens deste dispositivo.", Name = "Porta")]
        public string Porta { get; set; }

        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Informa o grau de rotação deste dispositivo.", Name = "Rotacao")]
        public int Rotacao
        {
            get
            {
                return this.mrotacao;
            }
            set
            {
                if (this.mrotacao == value || value != 0 && value != 90 && (value != 180 && value != 270))
                    return;
                this.mrotacao = value;
            }
        }

        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Informa o fabricante (enumeração Fabricantes) deste dispositivo.", Name = "Fabricante")]
        public Fabricantes Fabricante { get; set; } = Fabricantes.Axis;
    }
}
