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
            catch (Exception)
            {
                return false;
            }
        }
    }
}
