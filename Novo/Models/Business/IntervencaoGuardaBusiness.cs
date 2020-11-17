// Decompiled with JetBrains decompiler
// Type: WebApiBusiness.Business.IntervencaoGuardaBusiness
// Assembly: WebApiBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2441E174-C302-4098-B419-99DA0C4BEE41
// Assembly location: G:\VOPAK\Source\sam\Api_Smart\bin\WebApiBusiness.dll

using WebApiBusiness.App_Data;
using WebApiBusiness.Models;

namespace WebApiBusiness.Business
{
  public class IntervencaoGuardaBusiness
  {
    public bool InserirIntervencaoGuarda(IntervencaoGuarda intervencaoGuarda)
    {
      return new IntervencaoGuardaModel().InserirIntervencaoGuarda(intervencaoGuarda);
    }
  }
}
