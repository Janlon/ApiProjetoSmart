namespace WEBAPI_VOPAK
{
    /// <summary>
    ///  Tipos de requisição.
    /// </summary>
    public enum TipoDeRequisicao
    {
        None = 0,
        LBF = 1,
        LAP = 2,
    }

    /// <summary>
    /// Posições relativas ao tótem das câmeras.
    /// </summary>
    public enum PosicaoDaCamera
    {
        None = 0,
        Top = 1,
        Middle = 2,
        Down = 3,
    }

    /// <summary>
    /// Tipo de operação crud da requisição.
    /// </summary>
    public enum TipoDeOperacao
    {
        Incluir = 10,
        Alterar = 20,
        Excluir = 30,
        Listar = 40,
        Selecionar = 50,
        Resetar = 60,
        Ativar = 70,
        Liberar = 71,
        Desativar = 72,
    }

    /// <summary>
    /// Indicador de Erro (para retornos de requisições).
    /// </summary>
    public enum ErroSmartApi
    {
        AmbienteIndisponivelProducao,
        AmbienteIndisponivelTeste,
        AmbienteInvalido,
        AmbienteObrigatorio,
        AreaObrigatorio,
        BairroEnderecoTamanhoExcedido,
        CEPInexistente,
        CEPInvalido,
        CEPTamanhoExcedido,
        CNHObrigatorio,
        CNHTamanhoExcedido,
        CNPJDuplicado,
        CNPJInvalido,
        CNPJObrigatorio,
        CPFEmUso,
        CPFIdColaboradorComTipoDocumento,
        CPFObrigatorio,
        CPFTamanhoExcedido,
        CidadeObrigatorio,
        CidadeTamanhoExcedido,
        ContratadaNaoLocalizada,
        CpfInvalido,
        DtNascimentoInvalido,
        EmailColaboradorInvalido,
        EmailColaboradorTamanhoExcedido,
        EmailRepresentanteInvalido,
        EmailRepresentanteTamanhoExcedido,
        EmissorCNHTamanhoExcedido,
        EnderecoTamanhoExcedido,
        ErroNaoEspecificado,
        FalhaDeConexao,
        FotoObrigatorio,
        IdColaboradorIntegradorNaoEncontrado,
        IdColaboradorIntegradorObrigatorio,
        IdEmpresaIntegradorNaoEncontrado,
        IdEmpresaIntegradorObrigatorio,
        NenhumDadoInformado,
        NomeFantasiaTamanhoExcedido,
        NomeObrigatorio,
        NomeTamanhoExcedido,
        NumDocumentoTamanhoExcedido,
        NumEnderecoTamanhoExcedido,
        NumOsCancelado,
        NumOsComSaida,
        NumOsFechado,
        NumOsNaoLocalizado,
        NumOsObrigatorio,
        NumOsTamanhoExcedido,
        NumOsjaExiste,
        NumeroCrachaInvalido,
        NumeroCrachaJaExiste,
        NumeroCrachaTamanhoExcedido,
        OrgaoEmissorCNHTamanhoExcedido,
        PlacaInvalido,
        PlacaObrigatorio,
        RazaoSocialDuplicada,
        RazaoSocialObrigtorio,
        RazaoSocialTamanhoExcedido,
        RepresentanteTamanhoExcedido,
        SexoInvalido,
        SexoNaoInformado,
        SexoTamanhoExcedido,
        TelInvalido,
        TelSomenteNumeros,
        TelTamanhoExcedido,
        TipoDocumentoInvalido,
        TipoDocumentoNumDocumentoObrigatorios,
        TipoDocumentoObrigatorio,
        TipoDocumentoTamanhoExcedido,
        TipoOperacaoInvalido,
        TipoOperacaoObrigatorio,
        UFInvalido,
        UFObrigatorio,
        UFTamanhoExcedido,
        NumDocumentoDuplicado,
        CNHInvalido,
        CEPSomenteNumeros,
        ErroEmpresaNaoEspecificado,
        ErroColaboradorNaoEspecificado,
        ErroMotivacaoNaoEspecificado,
        EquipamentoObrigatorio,
        EquipamentoTamanhoInvalido,
        EquipamentoNaoCadastrado,
        NenhumaAcaoRealizada,
        BalancaObrigatorio,
        BalancaInvalido,
        CamerasNaoCadastradas,
        ColaboradorNaoLocalizado,
        CpfNaoLocalizado,
    }
}