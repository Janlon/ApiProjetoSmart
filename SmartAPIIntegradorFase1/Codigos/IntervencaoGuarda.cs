// Decompiled with JetBrains decompiler
// Type: WebApiBusiness.App_Data.IntervencaoGuarda
// Assembly: WebApiBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2441E174-C302-4098-B419-99DA0C4BEE41
// Assembly location: G:\VOPAK\Source\sam\Api_Smart\bin\WebApiBusiness.dll

using System;

namespace WebApiBusiness.App_Data
{
  public class IntervencaoGuarda : Basica
  {
    public DateTime DT_INTERVENCAO_GUARDA { get; set; }

    public string CD_CREDENCIAL_PESSOA { get; set; }

    public string CD_PLACA_VEICULO { get; set; }

    public string CD_SECAO { get; set; }

    public int ID_EQUIPAMENTO { get; set; }

    public char CD_SENTIDO { get; set; }

    public string CD_VCO_PESSOA { get; set; }

    public string ORDEM_SERVICO { get; set; }

    public int IdCapturaOcr { get; set; }

    public string OrdemServico { get; set; }
  }
}
