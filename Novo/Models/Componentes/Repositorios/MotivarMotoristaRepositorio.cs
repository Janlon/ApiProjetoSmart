namespace WEBAPI_VOPAK
{
    using WEBAPI_VOPAK.Models;
    using System;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Collections.Generic;

    internal class MotivarMotoristaRepositorio //: IMotivarMotoristaRepositorio
    {

        /// <summary>
        /// Validar a requisição:
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static Retorno ValidaRequisicao(MotivarMotorista item)
        {
            Retorno ret = new Retorno();
            // Requisição
            if (item == null) { ret.AddResult(ErroSmartApi.NenhumDadoInformado); return ret; }
            if (item.Ambiente == null)
                ret.AddResult(ErroSmartApi.AmbienteObrigatorio);
            else if (item.Ambiente.Length == 0)
                ret.AddResult(ErroSmartApi.AmbienteObrigatorio);
            else if (item.Ambiente.ToUpper() != "T" && item.Ambiente.ToUpper() != "P")
                ret.AddResult(ErroSmartApi.AmbienteInvalido);
            if (string.IsNullOrEmpty(item.placa) || string.IsNullOrWhiteSpace(item.placa))
                ret.AddResult(ErroSmartApi.PlacaObrigatorio);
            else
            {
                if (item.placa.Length == 0)
                    ret.AddResult(ErroSmartApi.PlacaObrigatorio);
                if (item.placa.Length != 7)
                    ret.AddResult(ErroSmartApi.PlacaInvalido);
            }
            //if (item.area <= 0)
            //    ret.AddResult(ErroSmartApi.AreaObrigatorio);

            if (string.IsNullOrEmpty(item.numOs) || string.IsNullOrWhiteSpace(item.numOs))
                ret.AddResult(ErroSmartApi.NumOsObrigatorio);
            else
            {
                if (item.numOs.IsBiggerThan(50))
                    ret.AddResult(ErroSmartApi.NumOsTamanhoExcedido);
            }
            return ret;
        }

        /// <summary>
        /// Cria a motivação temporária.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="wIdMotivacaoTemporaria">Inteiro, receberá o identificador de registro da motivação temporária processada.</param>
        /// <returns></returns>
        public static Retorno Add(MotivarMotorista item, ref int wIdMotivacaoTemporaria)
        {
            Retorno ret = ValidaRequisicao(item);
            wIdMotivacaoTemporaria = 0;
            try
            {
                if (ret.Ok())
                {
                    int num3 = item.idContratadaIntegradora;
                    if (num3 <= 0) { ret.AddResult(ErroSmartApi.ContratadaNaoLocalizada); return ret; }
                    //isso aqui nao esta sendo usado pra nada
                    //int num5 = Dados.GetIdArea(item.Ambiente, item.area);
                   // if (num5 <= 0) { ret.AddResult(ErroSmartApi.AreaObrigatorio); return ret; }
                    int wIdColaborador = item.idColaboradorIntegrador;
                    if (wIdColaborador <= 0) { ret.AddResult(ErroSmartApi.IdColaboradorIntegradorNaoEncontrado); return ret; }
                   // int num4 = Dados.GetIdCracha(item.Ambiente, wIdColaborador, false);
                    //if (num4 <= 0) { ret.AddResult(ErroSmartApi.NumeroCrachaInvalido); return ret; }
                    int NumOSDuplicado = Dados.ChecaOSDuplicada(item.Ambiente, item.numOs);
                    if (NumOSDuplicado > 0) 
                    {
                        // ret.AddResult(ErroSmartApi.NumOsjaExiste); 
                        //faz um update no registro
                        bool gravou = Dados.UpdateMotivacaoTemporaria(NumOSDuplicado, item.Ambiente,
                             wIdColaborador,
                             item.numOs,
                             item.placa,
                             ref wIdMotivacaoTemporaria);
                    }
                    //if (ret.Ok())
                    else
                    {
                        bool gravou = Dados.IncluiMotivacaoTemporaria(item.Ambiente,
                            item.idContratadaIntegradora, 
                            wIdColaborador,
                            item.numOs,
                            item.placa,
                            ref wIdMotivacaoTemporaria);
                    }
                }
            }
            catch (Exception ex) { ex.Log(); ret.AddResult(ErroSmartApi.ErroNaoEspecificado); }
            return ret;
        }

     
    }

}
