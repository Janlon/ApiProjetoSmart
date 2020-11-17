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
                return (Checklist)null;
            return new ChecklisModel().GetCheckList(checklist);
        }
    }
}
