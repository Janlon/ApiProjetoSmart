using System;
using System.Drawing;

namespace WebApiBusiness.Models
{
    public class ImagemCapturada : IDisposable
    {
        private bool disposedValue;

        public long ellapsedTime { get; set; }

        public int tentativa { get; set; } = 1;

        public Camera cam { get; set; }

        public Image img { get; set; }

        public byte[] imgBytes { get; set; }

        public float score { get; set; }

        public byte[] template { get; set; } = new byte[0];

        public int id_imagem { get; set; }

        public bool reconhecido { get; set; }

        public ImagemCapturada()
        {
            this.disposedValue = false;
        }

        public ImagemCapturada(ImagemCapturada item)
        {
            this.disposedValue = false;
            try
            {
                this.cam = new Camera();
                this.cam.Conta = item.cam.Conta;
                this.cam.DeteccaoFacial = item.cam.DeteccaoFacial;
                this.cam.Fabricante = item.cam.Fabricante;
                this.cam.IP = item.cam.IP;
                this.cam.Porta = item.cam.Porta;
                this.cam.Posicao = item.cam.Posicao;
                this.cam.ReconhecimentoFacial = item.cam.ReconhecimentoFacial;
                this.cam.Rotacao = item.cam.Rotacao;
                this.cam.Senha = item.cam.Senha;
                this.cam.Servico = item.cam.Servico;
                this.ellapsedTime = item.ellapsedTime;
                if (item.imgBytes.Length != 0)
                    this.imgBytes = item.imgBytes;
                this.score = item.score;
                if (item.template.Length != 0)
                    this.template = item.template;
                this.tentativa = item.tentativa;
                try
                {
                    this.img = item.img.Clone() as Image;
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        public static ImagemCapturada Clone(ImagemCapturada item)
        {
            return new ImagemCapturada(item);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposedValue)
                return;
            int num = disposing ? 1 : 0;
            this.disposedValue = true;
        }

        ~ImagemCapturada()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }
    }
}