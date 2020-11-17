// Decompiled with JetBrains decompiler
// Type: WebApiBusiness.App_Data.Checklist
// Assembly: WebApiBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2441E174-C302-4098-B419-99DA0C4BEE41
// Assembly location: G:\VOPAK\Source\sam\Api_Smart\bin\WebApiBusiness.dll

using System;

namespace WebApiBusiness.App_Data
{
  public class Checklist : Basica
  {
    public int IdChecklist { get; set; }

    public string NrPlaca { get; set; }

    public DateTime? DtCadastro { get; set; }

    public DateTime? DtValidade { get; set; }

    public bool IsValid { get; set; }
  }
}
