// Decompiled with JetBrains decompiler
// Type: WebApiBusiness.Business.ChecklistBusiness
// Assembly: WebApiBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2441E174-C302-4098-B419-99DA0C4BEE41
// Assembly location: G:\VOPAK\Source\sam\Api_Smart\bin\WebApiBusiness.dll

using WebApiBusiness.App_Data;
using WebApiBusiness.Models;
using WebApiBusiness.Util;

namespace WebApiBusiness.Business
{
  public class ChecklistBusiness
  {
    public Checklist GetChecklist(Checklist checklist)
    {
      checklist.NrPlaca = checklist.NrPlaca.Replace("-", "").Trim().ToUpper();
      if (!checklist.NrPlaca.IsPlate())
        return (Checklist) null;
      return new ChecklisModel().GetCheckList(checklist);
    }
  }
}
