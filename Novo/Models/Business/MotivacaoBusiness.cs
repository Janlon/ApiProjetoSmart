// Decompiled with JetBrains decompiler
// Type: WebApiBusiness.Business.MotivacaoBusiness
// Assembly: WebApiBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2441E174-C302-4098-B419-99DA0C4BEE41
// Assembly location: G:\VOPAK\Source\sam\Api_Smart\bin\WebApiBusiness.dll

using System;
using WebApiBusiness.App_Data;
using WebApiBusiness.Models;

namespace WebApiBusiness.Business
{
  public class MotivacaoBusiness
  {
    public bool InserirMotivacaoTemporaria(MotivacaoTemporaria motivacaoTemporaria)
    {
      if (motivacaoTemporaria.IdColaborador < 1)
        return false;
      if (motivacaoTemporaria.IdCracha < 1)
        return false;
      try
      {
        return new MotivacaoModel().InserirMotivacaoTemporaria(motivacaoTemporaria);
      }
      catch (Exception ex)
      {
        return false;
      }
    }
  }
}
