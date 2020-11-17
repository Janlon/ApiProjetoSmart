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