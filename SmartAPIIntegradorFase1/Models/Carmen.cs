using System;

namespace WebApiBusiness.Models
{
    [Serializable]
    public class Carmen : IDisposable
    {
        private bool disposedValue;

        public int id { get; set; }

        public string plate { get; set; } = "";

        public string ConfirmedPlate { get; set; } = "";

        public string confidence { get; set; } = "";

        public int totalConfidence { get; set; }

        public string plateframe { get; set; } = "";

        public string ptype { get; set; } = "";

        public string proc { get; set; } = "";

        public string base64 { get; set; } = "";

        public float score { get; set; }

        public DateTime DataHora { get; set; } = DateTime.Now;

        public Carmen()
        {
            this.disposedValue = false;
            this.id = 0;
            this.plate = "";
            this.confidence = "";
            this.plateframe = "";
            this.ptype = "";
            this.proc = "";
            this.base64 = "";
            this.score = 0.0f;
        }

        public Carmen(string image64)
        {
            this.disposedValue = false;
            this.id = 0;
            this.plate = "";
            this.confidence = "";
            this.plateframe = "";
            this.ptype = "";
            this.proc = "";
            this.base64 = image64;
            this.score = 0.0f;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposedValue)
                return;
            int num = disposing ? 1 : 0;
            this.disposedValue = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        ~Carmen()
        {
            this.Dispose(false);
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }
    }
}