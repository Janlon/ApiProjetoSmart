namespace WEBAPI_VOPAK
{
    using WEBAPI_VOPAK.Models;
    using System;
    using WEBAPI_VOPAK.Models.Componentes;

    internal class MotivarMotoristaCompletoRepositorio //: IMotivarMotoristaCompletoRepositorio
    {
        #region Validação
        //private static Retorno ValidaRequisicao(MotivarMotoristaCompleto item)
        //{
        //    Retorno ret = new Retorno();
        //    // Requisição
        //    if (item == null) { ret.AddResult(ErroSmartApi.NenhumDadoInformado); return ret; }
        //    // Ambiente
        //    ValidLenghtResult temp = item.Ambiente.HasValidLenght(1, 1, "TP", false);
        //    switch (temp)
        //    {
        //        case ValidLenghtResult.IsNull:
        //        case ValidLenghtResult.IsEmpty:
        //            ret.AddResult(ErroSmartApi.AmbienteObrigatorio);
        //            break;
        //        case ValidLenghtResult.IsGreater:
        //        case ValidLenghtResult.IsInvalid:
        //        case ValidLenghtResult.IsLower:
        //            ret.AddResult(ErroSmartApi.AmbienteInvalido);
        //            break;
        //        case ValidLenghtResult.Ok:
        //            break;
        //    }
        //    if (ret.Ok())
        //    {
        //        string ambiente = item.Ambiente.DefaultIfNull("P");
        //        if (!Dados.CanConnect(ambiente))
        //        {
        //            ret.AddResult(ambiente == "P" ?
        //                 ErroSmartApi.AmbienteIndisponivelProducao :
        //                 ErroSmartApi.AmbienteIndisponivelTeste);
        //            return ret;
        //        }
        //    }
        //    temp = item.tipoOperacao.HasValidLenght(1, 1, "IADR", false);
        //    switch (temp)
        //    {
        //        case ValidLenghtResult.IsNull:
        //        case ValidLenghtResult.IsEmpty:
        //            ret.AddResult(ErroSmartApi.TipoOperacaoObrigatorio);
        //            break;
        //        case ValidLenghtResult.IsGreater:
        //        case ValidLenghtResult.IsInvalid:
        //        case ValidLenghtResult.IsLower:
        //            ret.AddResult(ErroSmartApi.TipoOperacaoInvalido);
        //            break;
        //        case ValidLenghtResult.Ok:
        //            break;
        //    }
        //    // Contratada
        //    if (item.idEmpresaIntegrador == 0)
        //        ret.AddResult(ErroSmartApi.IdEmpresaIntegradorObrigatorio);
        //    // Colaborador
        //    if (item.idColaboradorIntegrador == 0)
        //        ret.AddResult(ErroSmartApi.IdColaboradorIntegradorObrigatorio);
        //    // Tipo de documento informado.
        //    if (string.IsNullOrEmpty(item.tipoDocumento))
        //        item.tipoDocumento = "";
        //    else
        //    {
        //        if (item.tipoDocumento.Length > 10)
        //            ret.AddResult(ErroSmartApi.TipoDocumentoTamanhoExcedido);
        //    }
        //    // Numero do documento.
        //    if (string.IsNullOrEmpty(item.numDocumento))
        //        item.numDocumento = "";
        //    // Nome
        //    if (string.IsNullOrEmpty(item.nome))
        //        ret.AddResult(ErroSmartApi.NomeObrigatorio);
        //    else
        //    {
        //        if (item.nome.Length > 50)
        //            ret.AddResult(ErroSmartApi.NomeTamanhoExcedido);
        //    }
        //    // Sexo
        //    temp = item.sexo.HasValidLenght(1, 1, "FM");
        //    switch (temp)
        //    {
        //        case ValidLenghtResult.IsEmpty:
        //        case ValidLenghtResult.IsNull:
        //            ret.AddResult(ErroSmartApi.SexoNaoInformado);
        //            break;
        //        case ValidLenghtResult.IsGreater:
        //        case ValidLenghtResult.IsLower:
        //        case ValidLenghtResult.IsInvalid:
        //            ret.AddResult(ErroSmartApi.SexoTamanhoExcedido);
        //            break;
        //        default: break;
        //    }
        //    // Nascimento
        //    if (item.dtNascimento != null)
        //    {
        //        if (item.dtNascimento.IsDate() == false)
        //            ret.AddResult(ErroSmartApi.DtNascimentoInvalido);
        //        else
        //            item.dtNascimento = DateTime.Parse(item.dtNascimento).ToString("yyyy-MM-dd");
        //    }
        //    else
        //        item.dtNascimento = "";
        //    // Endereço
        //    if (item.endereco != null)
        //    {
        //        if (item.endereco.Length > 150)
        //            ret.AddResult(ErroSmartApi.EnderecoTamanhoExcedido);
        //    }
        //    else
        //        item.endereco = "";
        //    // Número do endereço
        //    if (item.numEndereco != null)
        //    {
        //        if (item.numEndereco.Length > 10)
        //            ret.AddResult(ErroSmartApi.NumEnderecoTamanhoExcedido);
        //    }
        //    else
        //        item.numEndereco = "";
        //    // Bairro
        //    if (item.bairroEndereco != null)
        //    {
        //        if (item.bairroEndereco.Length > 50)
        //            ret.AddResult(ErroSmartApi.BairroEnderecoTamanhoExcedido);
        //    }
        //    else
        //        item.bairroEndereco = "";
        //    // Cidade
        //    if (string.IsNullOrEmpty(item.cidade) || string.IsNullOrWhiteSpace(item.cidade))
        //        ret.AddResult(ErroSmartApi.CidadeObrigatorio);
        //    else
        //    {
        //        if (item.cidade.Length > 50)
        //            ret.AddResult(ErroSmartApi.CidadeTamanhoExcedido);
        //    }
        //    // UF
        //    if (string.IsNullOrWhiteSpace(item.uf) || string.IsNullOrEmpty(item.uf))
        //        ret.AddResult(ErroSmartApi.UFObrigatorio);
        //    else
        //    {
        //        if (item.uf.IsBiggerThan(2,true))
        //            ret.AddResult(ErroSmartApi.UFTamanhoExcedido);
        //        if (!item.uf.IsBrUF())
        //            ret.AddResult(ErroSmartApi.UFInvalido);
        //    }
        //    // CEP
        //    if (!string.IsNullOrEmpty(item.cep))
        //    {
        //        if (item.cep.Length > 8 || item.cep.Length < 8)
        //            ret.AddResult(ErroSmartApi.CEPTamanhoExcedido);
        //        else
        //        {
        //            if (!item.cep.IsBrZIP())
        //                ret.AddResult(ErroSmartApi.CEPInvalido);
        //        }
        //    }
        //    else
        //        item.cep = "";
        //    // Telefone
        //    if (!string.IsNullOrEmpty(item.tel) && !string.IsNullOrWhiteSpace(item.tel))
        //    {
        //        if (!(item.tel.Trim().Length > 0 || item.tel.Trim().Length <= 13))
        //            ret.AddResult(ErroSmartApi.TelTamanhoExcedido);
        //        else
        //            if ((item.tel.JustNumbers() != item.tel) && (item.tel.JustNumbers().Length > 0))
        //            ret.AddResult(ErroSmartApi.TelSomenteNumeros);
        //    }
        //    else
        //        item.tel = "";
        //    // Crachá
        //    if (!string.IsNullOrEmpty(item.numeroCracha))
        //    {
        //        if (item.numeroCracha.Length > 10)
        //            ret.AddResult(ErroSmartApi.NumeroCrachaTamanhoExcedido);
        //    }
        //    else
        //        item.numeroCracha = "";
        //    // CNH
        //    if (!string.IsNullOrEmpty(item.cnh))
        //    {
        //        if (item.cnh.Length > 11)
        //            ret.AddResult(ErroSmartApi.CNHTamanhoExcedido);
        //    }
        //    else
        //        ret.AddResult(ErroSmartApi.CNHObrigatorio);
        //    // Órgão emissor do CNH (Detran ou SSP)
        //    if (!string.IsNullOrEmpty(item.orgaoEmissorCnh))
        //    {
        //        if (item.orgaoEmissorCnh.Length > 50)
        //            ret.AddResult(ErroSmartApi.OrgaoEmissorCNHTamanhoExcedido);
        //    }
        //    else
        //        item.orgaoEmissorCnh = "";
        //    // Emissor do CNH (SP ou MS ou RJ ...)
        //    if (!string.IsNullOrEmpty(item.emissorCnh))
        //    {
        //        if (item.emissorCnh.Length > 2)
        //            ret.AddResult(ErroSmartApi.EmissorCNHTamanhoExcedido);
        //    }
        //    else
        //        item.emissorCnh = "";
        //    if (item.idEmpresaIntegrador == 0)
        //        ret.AddResult(ErroSmartApi.IdEmpresaIntegradorObrigatorio);
        //    if (string.IsNullOrEmpty(item.cnpj))
        //        ret.AddResult(ErroSmartApi.CNPJObrigatorio);
        //    if (!item.cnpj.IsBrCnpj())
        //        ret.AddResult(ErroSmartApi.CNPJInvalido);
        //    if (string.IsNullOrEmpty(item.razaoSocial))
        //        ret.AddResult(ErroSmartApi.RazaoSocialObrigtorio);
        //    if (item.razaoSocial.IsBiggerThan(100, true))
        //        ret.AddResult(ErroSmartApi.RazaoSocialTamanhoExcedido);
        //    if (item.nomeFantasia.IsBiggerThan(100))
        //        ret.AddResult(ErroSmartApi.NomeFantasiaTamanhoExcedido);
        //    if (item.endereco.IsBiggerThan(150))
        //        ret.AddResult(ErroSmartApi.EnderecoTamanhoExcedido);
        //    if (item.numEndereco.IsBiggerThan(10))
        //        ret.AddResult(ErroSmartApi.NumEnderecoTamanhoExcedido);
        //    if (item.bairroEndereco.IsBiggerThan(50))
        //        ret.AddResult(ErroSmartApi.BairroEnderecoTamanhoExcedido);
        //    if (item.cidade.IsBiggerThan(50, true))
        //        ret.AddResult(ErroSmartApi.CidadeObrigatorio);
        //    if (!item.uf.IsBrUF())
        //        ret.AddResult(ErroSmartApi.UFInvalido);
        //    if (!item.cep.IsBrZIP())
        //        ret.AddResult(ErroSmartApi.CEPInvalido);
        //    if (item.representante.IsBiggerThan(100))
        //        ret.AddResult(ErroSmartApi.RepresentanteTamanhoExcedido);            
        //    return ret;
        //}
        #endregion

        public static Retorno Add(MotivarMotoristaCompleto item)
        {
            Retorno ret = new Retorno();
            try
            {
                int wIdContratada = 0;
                int wIdColaborador = 0;
                int wIdMotivacao = 0;
                ret.AddResult(EmpresaRepositorio.Add(new EmpresaViewModel()
                {
                    ambiente = item.Ambiente,
                    bairroEndereco = item.bairroEndereco,
                    cep = item.cep,
                    cidade = item.cidade,
                    cnpj = item.cnpj,
                    emailRepresentante = item.emailRepresentante,
                    endereco = item.endereco,
                    idEmpresaIntegrador = item.idEmpresaIntegrador,
                    nomeFantasia = item.nomeFantasia,
                    numEndereco = item.numEndereco,
                    razaoSocial = item.razaoSocial,
                    representante = item.representante,
                    tipoOperacao = item.tipoOperacao,
                    uf = item.uf,
                }, ref wIdContratada));
                if (wIdContratada > 0)
                {
                    ret.AddResult(ColaboradorRepositorio.Add(
                    new ColaboradorViewModel()
                    {
                        Ambiente = item.Ambiente,
                        bairroEndereco = item.bairroEnderecoMotorista,
                        cep = item.cepMotorista,
                        cidade = item.cidadeMotorista,
                        cnh = item.cnh,
                        tipoDocumento = item.tipoDocumento,
                        numDocumento = (string.IsNullOrEmpty(item.numDocumento) || string.IsNullOrWhiteSpace(item.numDocumento)) ? item.cpf : item.numDocumento,
                        cpf = item.cpf,
                        dtNascimento = item.dtNascimento,
                        emailColaborador = item.emailColaborador,
                        emissorCnh = item.emissorCnh,
                        endereco = item.enderecoMotorista,
                        foto = item.foto,
                        idColaboradorIntegrador = item.idColaboradorIntegrador,
                        idEmpresaIntegrador = item.idEmpresaIntegrador,
                        nome = item.nome,
                        numEndereco = item.numEnderecoMotorista,
                        numeroCracha = item.numeroCracha,
                        orgaoEmissorCnh = item.orgaoEmissorCnh,
                        sexo = item.sexo,
                        tel = item.tel,
                        tipoOperacao = "I",
                        uf = item.ufMotorista

                    }, ref wIdColaborador));
                    if (wIdColaborador > 0)
                    {
                        // Se for informada uma O.S. e uma placa motivar
                        // Se não for informada Os e nem placa, nao motivar, ira apenas inserir ou atualizar,

                        // Se não for informada uma area, quer dizer que é uma area de terceiros,
                        // no caso aqui é a area 6

                        if ((!string.IsNullOrEmpty(item.numOs) || !string.IsNullOrWhiteSpace(item.numOs))
                            &&
                            ((!string.IsNullOrEmpty(item.placa) || !string.IsNullOrWhiteSpace(item.placa))
                        //    &&
                        //    (item.area > 0)
                        ))
                            ret.AddResult(
                                MotivarMotoristaRepositorio.Add(new MotivarMotorista()
                                {
                                    idColaboradorIntegrador = wIdColaborador,
                                    Ambiente = item.Ambiente,
                                    area = item.area > 0 ? item.area : item.area = 6,
                                    numOs = item.numOs,
                                    placa = item.placa,
                                    idContratadaIntegradora = wIdContratada
                                }, ref wIdMotivacao));
                        else
                            wIdMotivacao = 9999;
                    }
                }
                if (wIdContratada == 0)
                    ret.AddResult(ErroSmartApi.ErroEmpresaNaoEspecificado);
                if (wIdColaborador == 0)
                    ret.AddResult(ErroSmartApi.ErroColaboradorNaoEspecificado);
                if (wIdMotivacao == 0)
                    ret.AddResult(ErroSmartApi.ErroMotivacaoNaoEspecificado);
            }
            catch (Exception ex) { ex.Log(); ret.AddResult(ErroSmartApi.ErroNaoEspecificado); }
            return ret;
        }
    }
}
