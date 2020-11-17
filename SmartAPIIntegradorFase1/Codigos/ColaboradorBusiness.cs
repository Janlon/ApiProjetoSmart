using WebApiBusiness.App_Data;
using WebApiBusiness.Models;

namespace WebApiBusiness.Business
{
    public class ColaboradorBusiness
    {
        public Colaborador GetColaborador(int idCracha, string ambiente)
        {
            return new ColaboradorModel().GetColaborador(idCracha, ambiente);
        }
    }
}
