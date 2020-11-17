using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBAPI_VOPAK.Models
{
    [Serializable]
    public class Carmen : IDisposable
    {
        #region Manutenção
        /// <summary>
        /// Mantém o status de chamadas à destruição.
        /// </summary>
        private bool disposedValue;
        #endregion

        #region Propriedades
        /// <summary>
        /// Essa é a data e a hora da resposta do serviço do Carmen.
        /// </summary>
        public DateTime DataHora { get; set; } = DateTime.Now;

        /// <summary>
        /// Identificador da seção de requisição ao serviço do Carmen. 
        /// Serve para identificar, em multiplas chamadas, a identidade da resposta à ser considerada.
        /// </summary>
        public int id { get; set; } = 0;

        /// <summary>
        /// Essa é a taxa de confiança (score) de guia para o próprio serviço do Carmen, 
        /// que varia entre 0 e 255, e que indica o score mínimo para indicar que uma placa 
        /// foi corretamente detectada, quando o modelo de placa (ISO. ex: Mercosul, USA, UK etc)
        /// está ativado no serviço.
        /// </summary>
        public int totalConfidence { get; set; } = 0;

        /// <summary>
        /// Esse é o score percentual do serviço do Carmen, já processada a comparação entre 
        /// a placa detectada pela câmera e aquela esperada pela ordem de serviço na Motivação,
        /// utilizando-se o algoritimo de Levenshtein.
        /// </summary>
        public float score { get; set; } = 0f;

        /// <summary>
        /// Esse é o valor de score do próprio serviço do Carmen, que varia entre 0 e 255, em hexa;
        /// </summary>
        public string confidence { get; set; } = "";

        /// <summary>
        /// Placa detectada pelo serviço do Carmen na imagem submetida.
        /// </summary>
        public string plate { get; set; } = "";

        /// <summary>
        /// Essa é a placa "confirmada", ou seja, que é 
        /// resultado de um score igual ou superior ao pré-determinado pela chamada. 
        /// Como nesta versão esse score é zero, tudo o que esteja igual ou acima disso 
        /// esta versão de aplicativo "entende" como sendo a placa confirmada.
        /// </summary>
        public string ConfirmedPlate { get; set; } = "";

        /// <summary>
        /// Esse é o "frame" (quadro) onde o serviço do Carmen detectou o quadro com a placa.
        /// </summary>
        public string plateframe { get; set; } = "";

        /// <summary>
        /// Esse campo é onde o serviço do Carmen retornaria o tipo de placa (Mercosul, USA, UK etc), caso o serviço 
        /// estivesse configurado para usar os modelos de placas. Se não estiver, retorna o mesmo valor para tudo.
        /// </summary>
        public string ptype { get; set; } = "";

        /// <summary>
        /// Este valor serve para manter o processo durante as chamadas ao serviço do Carmen.
        /// </summary>
        public string proc { get; set; } = "";

        /// <summary>
        /// Essa é a imagem capturada em formato Base64.
        /// </summary>
        public string base64 { get; set; } = "";
        #endregion

        #region Instância
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        internal Carmen() { this.disposedValue = false; }
        /// <summary>
        /// Construtor com a imagem em formato Base64.
        /// </summary>
        /// <param name="image64"></param>
        public Carmen(string image64) { this.disposedValue = false; this.base64 = image64; }
        private void CleanUp() { }
        /// <summary>
        /// Destruidor efetivo.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposedValue) { CleanUp(); }
            this.disposedValue = true;
        }
        /// <summary>
        /// Destruidor padrão.
        /// </summary>
        public void Dispose()
        { this.Dispose(true); GC.SuppressFinalize((object)this); }
        /// <summary>
        /// Equivale ao Finalize do VB.
        /// </summary>
        ~Carmen() { this.Dispose(false); }
        #endregion
    }
}