namespace WEBAPI_VOPAK
{
    using WEBAPI_VOPAK.Models;

    internal class ValidaBDCCRepositorio
    {
        private static Retorno ValidaRequisicao(ValidaBDCCViewModel item)
        {
            Retorno ret = new Retorno();
            
            return ret;
        }

        internal static Retorno Add(ValidaBDCCViewModel item)
        {
            Retorno ret = ValidaRequisicao(item);
            if (ret.Ok())
            {
                //// Pegar o ID do tipo de documento enviado, para poder validar os parâmetros.
                //int wIdTipoDocumento = Dados.GetIdTipoDocumento(item.ambiente, item.tipoDocumento);
                //// CPF não informado.
                //if ((string.IsNullOrEmpty(item.cpf)))
                //    ret.AddResult(ErroSmartApi.CPFObrigatorio);
                //// Tipo de documento informado.
                //if (string.IsNullOrEmpty(item.tipoDocumento))
                //    item.tipoDocumento = "";
                //else
                //{
                //    // Tipo de documento excede.
                //    if (item.tipoDocumento.Length > 10)
                //        ret.AddResult(ErroSmartApi.TipoDocumentoTamanhoExcedido);
                //    else
                //        // Carregou o tipo.
                //        wIdTipoDocumento = Dados.GetIdTipoDocumento(item.ambiente, item.tipoDocumento);
                //}
                //// Não tem tipo e não é um CPF.
                //if ((wIdTipoDocumento == 0) && (!item.cpf.IsBrCpf()))
                //    ret.AddResult(ErroSmartApi.TipoDocumentoInvalido);
                //// Tem um tipo e não tem um CPF
                //if ( string.IsNullOrEmpty(item.tipoDocumento) && string.IsNullOrEmpty(item.cpf) )
                //    ret.AddResult(ErroSmartApi.TipoDocumentoObrigatorio);
                //// Tem o tipo válido e o CPF foi preencido: O tipo é RG ou RGE?
                //if ( wIdTipoDocumento > 0 && ( !string.IsNullOrEmpty(item.cpf)) )
                //    if (item.tipoDocumento.Trim().ToUpper() != "RG" && item.tipoDocumento.Trim().ToUpper() != "RNE")
                //        ret.AddResult(ErroSmartApi.TipoDocumentoInvalido);
                //// Numero do documento.
                //if (string.IsNullOrEmpty(item.numDocumento))
                //    item.numDocumento = "";
                //// NumDocumento obrigatório quando o CPF não é um CPF.
                //if (string.IsNullOrEmpty(item.numDocumento) && (wIdTipoDocumento > 0) && (!item.cpf.IsBrCpf()))
                //    ret.AddResult(ErroSmartApi.TipoDocumentoNumDocumentoObrigatorios);
                //else
                //{
                //    // Tamanho do numDocumento inválido.
                //    if (wIdTipoDocumento > 0 && (item.numDocumento.Length < 2 || item.numDocumento.Length > 20))
                //        ret.AddResult(ErroSmartApi.NumDocumentoTamanhoExcedido);
                //}
            }
            return ret;
        }
    }
}
