using System;

namespace WebApiBusiness.Unisolution
{
    public class Result
    {
        public string numOs { get; set; }

        public DateTime dtEmissao { get; set; }

        public string cnpjCliente { get; set; }

        public string nomeCliente { get; set; }

        public DateTime dtPrevisaoChegada { get; set; }

        public string placaCavalo { get; set; }

        public string placaCarreta1 { get; set; }

        public string placaCarreta2 { get; set; }

        public string tipoDocMotorista { get; set; }

        public string numDocMotorista { get; set; }

        public string nomeMotorista { get; set; }

        public string cnpjTransportadora { get; set; }

        public string nomeTransportadora { get; set; }

        public int codSituacaoVeiculo { get; set; }

        public string descrSituacaoVeiculo { get; set; }
    }
}