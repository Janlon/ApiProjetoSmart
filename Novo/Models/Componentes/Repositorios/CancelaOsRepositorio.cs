namespace WEBAPI_VOPAK
{

    using WEBAPI_VOPAK.Models;

    internal class CancelaOsRepositorio
    {

        private static Retorno ValidaRequisicao(CancelaOSViewModel item)
        {
            Retorno ret = new Retorno();
            if (string.IsNullOrEmpty(item.numOS) || string.IsNullOrWhiteSpace(item.numOS))
                ret.AddResult(ErroSmartApi.NumOsObrigatorio);
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="finalizar"></param>
        /// <returns></returns>
        public static Retorno Add(CancelaOSViewModel item, bool finalizar)
        {
            Retorno ret = ValidaRequisicao(item);
            if (ret.Ok())
                ret.AddResult(Dados.CancelaOS(item, finalizar));
            return ret;
        }
    }
}
