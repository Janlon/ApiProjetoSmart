// Decompiled with JetBrains decompiler
// Type: WebApiBusiness.App_Data.MotivacaoTemporaria
// Assembly: WebApiBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2441E174-C302-4098-B419-99DA0C4BEE41
// Assembly location: G:\VOPAK\Source\sam\Api_Smart\bin\WebApiBusiness.dll

using System;

namespace WebApiBusiness.App_Data
{
  public class MotivacaoTemporaria : Basica
  {
    public int Id { get; set; }

    public int IdColaborador { get; set; }

    public int IdCracha { get; set; }

    public DateTime DtCadastro { get; set; }

    public string OrdemServico { get; set; }

    public string Placa { get; set; }

    public bool FlSaida { get; set; }

    public DateTime DtValidadeInicial { get; set; }

    public DateTime DtValidadeFinal { get; set; }

    public DateTime DtSaida { get; set; }
  }
}
