namespace WEBAPI_VOPAK
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class RetornoMensagem
    {
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public RetornoMensagem() { }

        /// <summary>
        /// Construtor parametrizado com enumeração de erros.
        /// </summary>
        /// <param name="ex"></param>
        internal RetornoMensagem(ErroSmartApi ex) { AddResult(ex); }

        /// <summary>
        /// Código de status do retorno.
        /// </summary>
        [JsonProperty("StatusCode", Order =1)]
        public int StatusCode
        {
            get
            {
                return (Ok()) ? 0 : 1;
            }
        }
        /// <summary>
        /// Mensagem do status do retorno.
        /// </summary>
        [JsonProperty("StatusMessage", Order =2)]
        public string StatusMessage
        {
            get
            {
                return (Ok()) ? "Sucess" : "Error";
            }
        }


        // ALTERADO:
        //[JsonProperty("Result")]
        //public List<Mensagens> Result = new List<Mensagens>();
        // De lista de Mensagens para lista de qualquer coisa.
        /// <summary>
        /// Lista de objetos dinâmicos.
        /// </summary>
        [JsonProperty("Result", Order = 3)]
        public dynamic Result = new List<dynamic>();


        /// <summary>
        /// Adiciona um ítem de mensagem de retorno custmozada (que não está na enumeração combinada).
        /// </summary>
        /// <param name="code">Código de retorno.</param>
        /// <param name="message">Mensagem de retorno.</param>
        public virtual void AddResult(int code, string message)
        {
            if (Result == null)
                Result = new List<dynamic>();
            Result.Add(new Mensagens(code, message));
        }
        /// <summary>
        /// Inclui as mensagens de um <see cref="Retorno"/> à este.
        /// </summary>
        /// <param name="ex"><see cref="Retorno"/>.</param>
        public virtual void AddResult(Retorno ex)
        {
            foreach (Mensagens item in ex.Result)
                AddResult((ErroSmartApi)item.nrCode);
        }
        /// <summary>
        /// Inclui um erro da enumeração <see cref="ErroSmartApi"/>.
        /// </summary>
        /// <param name="ex"></param>
        public virtual void AddResult(ErroSmartApi ex)
        {
            Result = ex;
        }

        #region Para inclusão de listas, itens e filas.
        /// <summary>
        /// Inclui uma lista dinâmica.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista"></param>
        public virtual void AddResult<T>(IEnumerable<T> lista) where T : class { Result = lista; }
        /// <summary>
        /// Inclui uma lista dinâmica.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista"></param>
        public virtual void AddResult<T>(IList<T> lista) where T : class { Result = lista; }
        /// <summary>
        /// Inclui uma lista dinâmica.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public virtual void AddResult<T>(T item) where T : class { Result = item; }
        #endregion


        /// <summary>
        /// Método para identificação do status de erros.
        /// </summary>
        /// <returns></returns>
        internal virtual bool Ok()
        {
            Type tInner;
            var oType = Result.GetType();
            bool isLista= (oType.IsGenericType && 
                (oType.GetGenericTypeDefinition() == typeof(List<>)));
            if(!isLista)
                tInner = Result.GetType();
            else
                tInner = Result[0].GetType();
            if ((tInner != null) && (tInner == typeof(Mensagens)))
                return (Result==null);
            return true;
        }

        /// <summary>
        /// Retorna a mensagem da enumeração <see cref="ErroSmartApi"/>.
        /// </summary>
        /// <param name="ex">Item da enumeração <see cref="ErroSmartApi"/>.</param>
        /// <returns>Texto com a mensagem do erro indicado.</returns>
        internal virtual string FromEnum(ErroSmartApi ex)
        {
            switch (ex)
            {
                case ErroSmartApi.AmbienteIndisponivelProducao: return @"Ambiente de produção não disponível ".Trim();
                case ErroSmartApi.AmbienteIndisponivelTeste: return @"Ambiente de testes não disponível ".Trim();
                case ErroSmartApi.AmbienteInvalido: return @"Parâmetro ""Ambiente"" deve ser preenchido com T ou P ".Trim(); ;
                case ErroSmartApi.AmbienteObrigatorio: return @"Parâmetro ""Ambiente"" obrigatório".Trim();
                case ErroSmartApi.AreaObrigatorio: return @"Parâmetro ""area"" obrigatório ".Trim();
                case ErroSmartApi.BalancaInvalido: return @"BALANÇA NÃO ENCONTRADA ".Trim();
                case ErroSmartApi.BalancaObrigatorio: return @"Parâmetro ""Balança"" obrigatório ".Trim();
                case ErroSmartApi.BairroEnderecoTamanhoExcedido: return @"Parâmetro ""BairroEndereco"" deve ser informado com tamanho máximo de 50 caracteres ".Trim();
                case ErroSmartApi.CamerasNaoCadastradas: return @"Câmeras não cadastradas para o local informado".Trim();
                case ErroSmartApi.CEPInexistente: return @"Parâmetro ""CEP"" valor informado não é um CEP existente".Trim();
                case ErroSmartApi.CEPInvalido: return @"Parâmetro ""CEP"" informado é inválido".Trim();
                case ErroSmartApi.CEPSomenteNumeros: return @"Parâmetro ""CEP"" só pode conter números".Trim();
                case ErroSmartApi.CEPTamanhoExcedido: return @"Parâmetro ""CEP"" deve ser informado sempre com tamanho de 8 dígitos numéricos ".Trim();
                case ErroSmartApi.CNHObrigatorio: return @"Parâmetro ""CNH"" obrigatório ".Trim();
                case ErroSmartApi.CNHInvalido: return @"Parâmetro ""CNH"" deve ser informado apenas com dígitos numéricos".Trim();
                case ErroSmartApi.CNHTamanhoExcedido: return @"Parâmetro ""CNH"" deve ser informado com tamanho máximo de 11 caracteres ".Trim();
                case ErroSmartApi.ColaboradorNaoLocalizado: return @"Colaborador não localizado ".Trim();
                case ErroSmartApi.CNPJDuplicado: return @"Parâmetro ""CNPJ"" duplicado".Trim();
                case ErroSmartApi.CNPJInvalido: return @"Parâmetro ""CNPJ"" inválido".Trim();
                case ErroSmartApi.CNPJObrigatorio: return @"Parâmetro ""CNPJ"" obrigatório ".Trim();
                case ErroSmartApi.CPFEmUso: return @"Parâmetro ""CPF"" já em uso por outro colaborador".Trim();
                case ErroSmartApi.CPFIdColaboradorComTipoDocumento: return @"Parâmetro ""CPF"" deve ser igual ao ""idColaboradorIntegrador"" quando ""tipoDocumento"" / ""numDocumento"" forem fornecidos".Trim();
                case ErroSmartApi.CPFObrigatorio: return @"Parâmetro ""CPF"" obrigatório ".Trim();
                case ErroSmartApi.CPFTamanhoExcedido: return @"Parâmetro ""CPF"" deve ser informado com tamanho máximo de 11 caracteres ".Trim();
                case ErroSmartApi.CpfNaoLocalizado: return @"Nenhum registro encontrado para atualização".Trim();
                case ErroSmartApi.CidadeObrigatorio: return @"Parâmetro ""Cidade""  obrigatório ".Trim();
                case ErroSmartApi.CidadeTamanhoExcedido: return @"Parâmetro ""Cidade"" deve ser informado com tamanho máximo de 50 caracteres ".Trim();
                case ErroSmartApi.ContratadaNaoLocalizada: return @"Parâmetro ""idEmpresaIntegrador"" não localizado".Trim();
                case ErroSmartApi.CpfInvalido: return @"Parâmetro ""CPF"" inválido".Trim();
                case ErroSmartApi.DtNascimentoInvalido: return @"Parâmetro ""dtNascimento"" com formato inválido ".Trim();
                case ErroSmartApi.EmailColaboradorInvalido: return @"Parâmetro ""EmailColaborador"" inválido ".Trim();
                case ErroSmartApi.EmailColaboradorTamanhoExcedido: return @"Parâmetro ""EmailColaborador"" deve ser informado com tamanho máximo de 100 caracteres ".Trim();
                case ErroSmartApi.EmailRepresentanteInvalido: return @"Parâmetro ""EmailRepresentante"" inválido ".Trim();
                case ErroSmartApi.EmailRepresentanteTamanhoExcedido: return @"Parâmetro ""EmailRepresentante"" deve ser informado com tamanho máximo de 100 caracteres ".Trim();
                case ErroSmartApi.EmissorCNHTamanhoExcedido: return @"Parâmetro ""EmissorCNH"" deve ser informado com tamanho máximo de 2 caracteres ".Trim();
                case ErroSmartApi.EnderecoTamanhoExcedido: return @"Parâmetro ""Endereco"" deve ser informado com tamanho máximo de 150 caracteres ".Trim();
                case ErroSmartApi.EquipamentoNaoCadastrado: return @"Parâmetro ""PontoControle"" informado não foi localizado ".Trim();
                case ErroSmartApi.EquipamentoObrigatorio: return @"Parâmetro ""PontoControle""  é obrigatório ".Trim();
                case ErroSmartApi.EquipamentoTamanhoInvalido: return @"Parâmetro ""PontoControle"" deve ter entre 1 e 20 caracteres ".Trim();
                case ErroSmartApi.ErroColaboradorNaoEspecificado: return @"Erro não especificado ao cadastrar colaborador".Trim();
                case ErroSmartApi.ErroEmpresaNaoEspecificado: return @"Erro não especificado ao cadastrar empresa".Trim();
                case ErroSmartApi.ErroMotivacaoNaoEspecificado: return @"Erro não especificado ao gerar motivação".Trim();
                case ErroSmartApi.FalhaDeConexao: return @"Falha de conexão com a base de dados".Trim();
                case ErroSmartApi.FotoObrigatorio: return @"Parâmetro ""Foto"" não informado".Trim();
                case ErroSmartApi.IdColaboradorIntegradorNaoEncontrado: return @"Parâmetro ""idColaboradorIntegrador"" não encontrado".Trim();
                case ErroSmartApi.IdColaboradorIntegradorObrigatorio: return @"Parâmetro ""idColaboradorIntegrador"" obrigatório".Trim();
                case ErroSmartApi.IdEmpresaIntegradorNaoEncontrado: return @"Parâmetro "" idEmpresaIntegrador "" não encontrado".Trim();
                case ErroSmartApi.IdEmpresaIntegradorObrigatorio: return @"Parâmetro ""idEmpresaIntegrador""  obrigatório ".Trim();
                case ErroSmartApi.NenhumDadoInformado: return @"Requisição sem dados ".Trim();
                case ErroSmartApi.NenhumaAcaoRealizada: return @"Nenhum parâmetro informado, logo nenhuma alteração realizada".Trim();
                case ErroSmartApi.NomeFantasiaTamanhoExcedido: return @"Parâmetro ""NomeFantasia"" deve ser informado com tamanho máximo de 50 caracteres ".Trim();
                case ErroSmartApi.NomeObrigatorio: return @"Parâmetro ""Nome""  obrigatório ".Trim();
                case ErroSmartApi.NomeTamanhoExcedido: return @"Parâmetro ""Nome"" deve ser informado com tamanho máximo de 50 caracteres ".Trim();
                case ErroSmartApi.NumDocumentoTamanhoExcedido: return @"Parâmetro ""numDocumento"" deve ser informado com tamanho máximo de 20 caracteres ".Trim();
                case ErroSmartApi.NumDocumentoDuplicado: return @"Parâmetro ""numDocumento"" informado já está cadastrado para outro colaborador".Trim(); ;
                case ErroSmartApi.NumEnderecoTamanhoExcedido: return @"Parâmetro ""NumEndereco"" deve ser informado com tamanho máximo de 10 caracteres ".Trim();
                case ErroSmartApi.NumOsCancelado: return @"Parâmetro ""numOs"" já foi cancelado".Trim();
                case ErroSmartApi.NumOsComSaida: return @"Parâmetro ""numOs"" já possui saída".Trim();
                case ErroSmartApi.NumOsFechado: return @"Parâmetro ""numOs"" já foi encerrado".Trim();
                case ErroSmartApi.NumOsNaoLocalizado: return @"Não foi encontrada uma motivação ativa para a Ordem de Serviço solicitada".Trim();
                case ErroSmartApi.NumOsObrigatorio: return @"Parâmetro ""numOs"" obrigatório ".Trim();
                case ErroSmartApi.NumOsTamanhoExcedido: return @"Parâmetro ""numOs"" deve ter até 50 caracteres".Trim();
                case ErroSmartApi.NumOsjaExiste: return @"Parâmetro ""numOs"" é único, já existe motivação com este número de OS   ".Trim();
                case ErroSmartApi.NumeroCrachaInvalido: return @"Parâmetro ""numeroCracha"" deve pertencer ao colaborador informado ".Trim();
                case ErroSmartApi.NumeroCrachaJaExiste: return @"Parâmetro ""numCracha"" em uso por outro colaborador".Trim();
                case ErroSmartApi.NumeroCrachaTamanhoExcedido: return @"Parâmetro ""numeroCracha"" deve ser informado com tamanho de 10 caracteres ".Trim();
                case ErroSmartApi.OrgaoEmissorCNHTamanhoExcedido: return @"Parâmetro ""OrgaoEmissorCNH"" deve ser informado com tamanho máximo de 6 caracteres ".Trim();
                case ErroSmartApi.PlacaInvalido: return @"Parâmetro ""placa"" com formato inválido ".Trim();
                case ErroSmartApi.PlacaObrigatorio: return @"Parâmetro ""placa"" obrigatório ".Trim();
                case ErroSmartApi.RazaoSocialDuplicada: return @"Parâmetro ""razaoSocial"" já cadastrado ".Trim();
                case ErroSmartApi.RazaoSocialObrigtorio: return @"Parâmetro ""Razão Social""  obrigatório ".Trim();
                case ErroSmartApi.RazaoSocialTamanhoExcedido: return @"Parâmetro ""Razão Social"" deve ser informado com tamanho máximo de 50 caracteres ".Trim();
                case ErroSmartApi.RepresentanteTamanhoExcedido: return @"Parâmetro ""Representante"" deve ser informado com tamanho máximo de 100 caracteres ".Trim();
                case ErroSmartApi.SexoInvalido: return @"Parâmetro ""Sexo"" com valor inválido ".Trim();
                case ErroSmartApi.SexoNaoInformado: return @"Parâmetro ""sexo"" não informado".Trim();
                case ErroSmartApi.SexoTamanhoExcedido: return @"Parâmetro ""Sexo"" deve ser informado com tamanho máximo de 1 caractere ".Trim();
                case ErroSmartApi.TelInvalido: return @"Parâmetro ""Tel"" com formato inválido ".Trim();
                case ErroSmartApi.TelSomenteNumeros: return @"Parâmetro ""Tel"" só pode conter números".Trim();
                case ErroSmartApi.TelTamanhoExcedido: return @"Parâmetro ""Tel"" deve ser informado com tamanho máximo de 13 caracteres ".Trim();
                case ErroSmartApi.TipoDocumentoInvalido: return @"Parâmetro ""tipoDocumento"" inválido (somente ""RG"" ou ""RNE"" quando o ""CPF"" informado é o mesmo que o ""idColaboradorIntegrador"")".Trim();
                case ErroSmartApi.TipoDocumentoNumDocumentoObrigatorios: return @"Parâmetros ""TipoDocumento"" e ""NumDocumento"" obrigatórios na ausência do ""CPF""".Trim();
                case ErroSmartApi.TipoDocumentoTamanhoExcedido: return @"Parâmetro ""tipoDocumento"" deve ser informado com tamanho máximo de 10 caracteres ".Trim();
                case ErroSmartApi.TipoOperacaoInvalido: return @"Parâmetro ""TipoOperacao"" deve ser preenchido com ""I"" ";
                case ErroSmartApi.TipoOperacaoObrigatorio: return @"Parâmetro ""TipoOperacao"" obrigatório ";
                case ErroSmartApi.UFInvalido: return @"Parâmetro ""UF"" informado inválido".Trim();
                case ErroSmartApi.UFObrigatorio: return @"Parâmetro ""UF""  obrigatório ".Trim();
                case ErroSmartApi.UFTamanhoExcedido: return @"Parâmetro ""UF"" deve ser informado com tamanho máximo de 2 caracteres ".Trim();
                default: return @"Erro não especificado".Trim();
            }
        }
    }


    public class Retorno
    {
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public Retorno() { }

        /// <summary>
        /// Construtor parametrizado com enumeração de erros.
        /// </summary>
        /// <param name="ex"></param>
        internal Retorno(ErroSmartApi ex) { AddResult(ex); }


        // ALTERADO:
        //[JsonProperty("Result")]
        //public List<Mensagens> Result = new List<Mensagens>();
        // De lista de Mensagens para lista de qualquer coisa.
        /// <summary>
        /// Lista de objetos dinâmicos.
        /// </summary>
        [JsonProperty("Result")]
        public List<dynamic> Result = new List<dynamic>();

        /// <summary>
        /// Código de status do retorno.
        /// </summary>
        [JsonProperty("StatusCode")]
        public int StatusCode 
        { 
            get 
            { 
                return (Ok()) ? 0 : 1; 
            } 
        }
        /// <summary>
        /// Mensagem do status do retorno.
        /// </summary>
        [JsonProperty("StatusMessage")]
        public string StatusMessage 
        { 
            get 
            { 
                return (Ok()) ? "Sucess" : "Error"; 
            } 
        }

        /// <summary>
        /// Adiciona um ítem de mensagem de retorno custmozada (que não está na enumeração combinada).
        /// </summary>
        /// <param name="code">Código de retorno.</param>
        /// <param name="message">Mensagem de retorno.</param>
        public virtual void AddResult(int code, string message)
        {
            if (Result == null)
                Result = new List<dynamic>();
            Result.Add(new Mensagens(code,message));
        }
        /// <summary>
        /// Inclui as mensagens de um <see cref="Retorno"/> à este.
        /// </summary>
        /// <param name="ex"><see cref="Retorno"/>.</param>
        public virtual void AddResult(Retorno ex)
        {
            foreach (Mensagens item in ex.Result)
                AddResult( (ErroSmartApi) item.nrCode );
        }
        /// <summary>
        /// Inclui um erro da enumeração <see cref="ErroSmartApi"/>.
        /// </summary>
        /// <param name="ex"></param>
        public virtual void AddResult(ErroSmartApi ex)
        {
            if (Result.Where(p => p.nrCode == (int)ex).Count() == 0)
                Result.Add(new Mensagens((int)(ex), FromEnum(ex)));
        }

        #region Para inclusão de listas, itens e filas.
        /// <summary>
        /// Inclui uma lista dinâmica.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista"></param>
        public virtual void AddResult<T>(IEnumerable<T> lista) where T : class { Result.AddRange(lista); }
        /// <summary>
        /// Inclui uma lista dinâmica.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista"></param>
        public virtual void AddResult<T>(IList<T> lista) where T : class { Result.AddRange(lista); }
        /// <summary>
        /// Inclui uma lista dinâmica.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public virtual void AddResult<T>(T item) where T : class { Result.Add(item); }
        #endregion




        /// <summary>
        /// Método para identificação do status de erros.
        /// </summary>
        /// <returns></returns>
        internal virtual bool Ok()
        {
            bool temp = Result
                .Where(p => p.GetType() == typeof(Mensagens))
                .Count() == 0;
            return temp;
        }

        /// <summary>
        /// Retorna a mensagem da enumeração <see cref="ErroSmartApi"/>.
        /// </summary>
        /// <param name="ex">Item da enumeração <see cref="ErroSmartApi"/>.</param>
        /// <returns>Texto com a mensagem do erro indicado.</returns>
        internal virtual string FromEnum(ErroSmartApi ex)
        {
            switch (ex)
            {
                case ErroSmartApi.AmbienteIndisponivelProducao: return @"Ambiente de produção não disponível ".Trim();
                case ErroSmartApi.AmbienteIndisponivelTeste: return @"Ambiente de testes não disponível ".Trim();
                case ErroSmartApi.AmbienteInvalido: return @"Parâmetro ""Ambiente"" deve ser preenchido com T ou P ".Trim(); ;
                case ErroSmartApi.AmbienteObrigatorio: return @"Parâmetro ""Ambiente"" obrigatório".Trim();
                case ErroSmartApi.AreaObrigatorio: return @"Parâmetro ""area"" obrigatório ".Trim();
                case ErroSmartApi.BalancaInvalido: return @"BALANÇA NÃO ENCONTRADA ".Trim(); 
                case ErroSmartApi.BalancaObrigatorio: return @"Parâmetro ""Balança"" obrigatório ".Trim();
                case ErroSmartApi.BairroEnderecoTamanhoExcedido: return @"Parâmetro ""BairroEndereco"" deve ser informado com tamanho máximo de 50 caracteres ".Trim();
                case ErroSmartApi.CamerasNaoCadastradas: return @"Câmeras não cadastradas para o local informado".Trim();
                case ErroSmartApi.CEPInexistente: return @"Parâmetro ""CEP"" valor informado não é um CEP existente".Trim();
                case ErroSmartApi.CEPInvalido: return @"Parâmetro ""CEP"" informado é inválido".Trim();
                case ErroSmartApi.CEPSomenteNumeros: return @"Parâmetro ""CEP"" só pode conter números".Trim();
                case ErroSmartApi.CEPTamanhoExcedido: return @"Parâmetro ""CEP"" deve ser informado sempre com tamanho de 8 dígitos numéricos ".Trim();
                case ErroSmartApi.CNHObrigatorio: return @"Parâmetro ""CNH"" obrigatório ".Trim();
                case ErroSmartApi.CNHInvalido: return @"Parâmetro ""CNH"" deve ser informado apenas com dígitos numéricos".Trim();
                case ErroSmartApi.CNHTamanhoExcedido: return @"Parâmetro ""CNH"" deve ser informado com tamanho máximo de 11 caracteres ".Trim();
                case ErroSmartApi.ColaboradorNaoLocalizado: return @"Colaborador não localizado ".Trim();
                case ErroSmartApi.CNPJDuplicado: return @"Parâmetro ""CNPJ"" duplicado".Trim();
                case ErroSmartApi.CNPJInvalido: return @"Parâmetro ""CNPJ"" inválido".Trim();
                case ErroSmartApi.CNPJObrigatorio: return @"Parâmetro ""CNPJ"" obrigatório ".Trim();
                case ErroSmartApi.CPFEmUso: return @"Parâmetro ""CPF"" já em uso por outro colaborador".Trim();
                case ErroSmartApi.CPFIdColaboradorComTipoDocumento: return @"Parâmetro ""CPF"" deve ser igual ao ""idColaboradorIntegrador"" quando ""tipoDocumento"" / ""numDocumento"" forem fornecidos".Trim();
                case ErroSmartApi.CPFObrigatorio: return @"Parâmetro ""CPF"" obrigatório ".Trim();
                case ErroSmartApi.CPFTamanhoExcedido: return @"Parâmetro ""CPF"" deve ser informado com tamanho máximo de 11 caracteres ".Trim();
                case ErroSmartApi.CpfNaoLocalizado: return @"Nenhum registro encontrado para atualização".Trim();
                case ErroSmartApi.CidadeObrigatorio: return @"Parâmetro ""Cidade""  obrigatório ".Trim();
                case ErroSmartApi.CidadeTamanhoExcedido: return @"Parâmetro ""Cidade"" deve ser informado com tamanho máximo de 50 caracteres ".Trim();
                case ErroSmartApi.ContratadaNaoLocalizada: return @"Parâmetro ""idEmpresaIntegrador"" não localizado".Trim();
                case ErroSmartApi.CpfInvalido: return @"Parâmetro ""CPF"" inválido".Trim();
                case ErroSmartApi.DtNascimentoInvalido: return @"Parâmetro ""dtNascimento"" com formato inválido ".Trim();
                case ErroSmartApi.EmailColaboradorInvalido: return @"Parâmetro ""EmailColaborador"" inválido ".Trim();
                case ErroSmartApi.EmailColaboradorTamanhoExcedido: return @"Parâmetro ""EmailColaborador"" deve ser informado com tamanho máximo de 100 caracteres ".Trim();
                case ErroSmartApi.EmailRepresentanteInvalido: return @"Parâmetro ""EmailRepresentante"" inválido ".Trim();
                case ErroSmartApi.EmailRepresentanteTamanhoExcedido: return @"Parâmetro ""EmailRepresentante"" deve ser informado com tamanho máximo de 100 caracteres ".Trim();
                case ErroSmartApi.EmissorCNHTamanhoExcedido: return @"Parâmetro ""EmissorCNH"" deve ser informado com tamanho máximo de 2 caracteres ".Trim();
                case ErroSmartApi.EnderecoTamanhoExcedido: return @"Parâmetro ""Endereco"" deve ser informado com tamanho máximo de 150 caracteres ".Trim();
                case ErroSmartApi.EquipamentoNaoCadastrado: return @"Parâmetro ""PontoControle"" informado não foi localizado ".Trim();
                case ErroSmartApi.EquipamentoObrigatorio: return @"Parâmetro ""PontoControle""  é obrigatório ".Trim();
                case ErroSmartApi.EquipamentoTamanhoInvalido: return @"Parâmetro ""PontoControle"" deve ter entre 1 e 20 caracteres ".Trim();
                case ErroSmartApi.ErroColaboradorNaoEspecificado: return @"Erro não especificado ao cadastrar colaborador".Trim();
                case ErroSmartApi.ErroEmpresaNaoEspecificado: return @"Erro não especificado ao cadastrar empresa".Trim();
                case ErroSmartApi.ErroMotivacaoNaoEspecificado: return @"Erro não especificado ao gerar motivação".Trim();
                case ErroSmartApi.FalhaDeConexao: return @"Falha de conexão com a base de dados".Trim();
                case ErroSmartApi.FotoObrigatorio: return @"Parâmetro ""Foto"" não informado".Trim();
                case ErroSmartApi.IdColaboradorIntegradorNaoEncontrado: return @"Parâmetro ""idColaboradorIntegrador"" não encontrado".Trim();
                case ErroSmartApi.IdColaboradorIntegradorObrigatorio: return @"Parâmetro ""idColaboradorIntegrador"" obrigatório".Trim();
                case ErroSmartApi.IdEmpresaIntegradorNaoEncontrado: return @"Parâmetro "" idEmpresaIntegrador "" não encontrado".Trim();
                case ErroSmartApi.IdEmpresaIntegradorObrigatorio: return @"Parâmetro ""idEmpresaIntegrador""  obrigatório ".Trim();
                case ErroSmartApi.NenhumDadoInformado: return @"Requisição sem dados ".Trim();
                case ErroSmartApi.NenhumaAcaoRealizada: return @"Nenhum parâmetro informado, logo nenhuma alteração realizada".Trim();
                case ErroSmartApi.NomeFantasiaTamanhoExcedido: return @"Parâmetro ""NomeFantasia"" deve ser informado com tamanho máximo de 50 caracteres ".Trim();
                case ErroSmartApi.NomeObrigatorio: return @"Parâmetro ""Nome""  obrigatório ".Trim();
                case ErroSmartApi.NomeTamanhoExcedido: return @"Parâmetro ""Nome"" deve ser informado com tamanho máximo de 50 caracteres ".Trim();
                case ErroSmartApi.NumDocumentoTamanhoExcedido: return @"Parâmetro ""numDocumento"" deve ser informado com tamanho máximo de 20 caracteres ".Trim();
                case ErroSmartApi.NumDocumentoDuplicado: return @"Parâmetro ""numDocumento"" informado já está cadastrado para outro colaborador".Trim(); ;
                case ErroSmartApi.NumEnderecoTamanhoExcedido: return @"Parâmetro ""NumEndereco"" deve ser informado com tamanho máximo de 10 caracteres ".Trim();
                case ErroSmartApi.NumOsCancelado: return @"Parâmetro ""numOs"" já foi cancelado".Trim();
                case ErroSmartApi.NumOsComSaida: return @"Parâmetro ""numOs"" já possui saída".Trim();
                case ErroSmartApi.NumOsFechado: return @"Parâmetro ""numOs"" já foi encerrado".Trim();
                case ErroSmartApi.NumOsNaoLocalizado: return @"Não foi encontrada uma motivação ativa para a Ordem de Serviço solicitada".Trim();
                case ErroSmartApi.NumOsObrigatorio: return @"Parâmetro ""numOs"" obrigatório ".Trim();
                case ErroSmartApi.NumOsTamanhoExcedido: return @"Parâmetro ""numOs"" deve ter até 50 caracteres".Trim();
                case ErroSmartApi.NumOsjaExiste: return @"Parâmetro ""numOs"" é único, já existe motivação com este número de OS   ".Trim();
                case ErroSmartApi.NumeroCrachaInvalido: return @"Parâmetro ""numeroCracha"" deve pertencer ao colaborador informado ".Trim();
                case ErroSmartApi.NumeroCrachaJaExiste: return @"Parâmetro ""numCracha"" em uso por outro colaborador".Trim();
                case ErroSmartApi.NumeroCrachaTamanhoExcedido: return @"Parâmetro ""numeroCracha"" deve ser informado com tamanho de 10 caracteres ".Trim();
                case ErroSmartApi.OrgaoEmissorCNHTamanhoExcedido: return @"Parâmetro ""OrgaoEmissorCNH"" deve ser informado com tamanho máximo de 6 caracteres ".Trim();
                case ErroSmartApi.PlacaInvalido: return @"Parâmetro ""placa"" com formato inválido ".Trim();
                case ErroSmartApi.PlacaObrigatorio: return @"Parâmetro ""placa"" obrigatório ".Trim();
                case ErroSmartApi.RazaoSocialDuplicada: return @"Parâmetro ""razaoSocial"" já cadastrado ".Trim();
                case ErroSmartApi.RazaoSocialObrigtorio: return @"Parâmetro ""Razão Social""  obrigatório ".Trim();
                case ErroSmartApi.RazaoSocialTamanhoExcedido: return @"Parâmetro ""Razão Social"" deve ser informado com tamanho máximo de 50 caracteres ".Trim();
                case ErroSmartApi.RepresentanteTamanhoExcedido: return @"Parâmetro ""Representante"" deve ser informado com tamanho máximo de 100 caracteres ".Trim();
                case ErroSmartApi.SexoInvalido: return @"Parâmetro ""Sexo"" com valor inválido ".Trim();
                case ErroSmartApi.SexoNaoInformado: return @"Parâmetro ""sexo"" não informado".Trim();
                case ErroSmartApi.SexoTamanhoExcedido: return @"Parâmetro ""Sexo"" deve ser informado com tamanho máximo de 1 caractere ".Trim();
                case ErroSmartApi.TelInvalido: return @"Parâmetro ""Tel"" com formato inválido ".Trim();
                case ErroSmartApi.TelSomenteNumeros: return @"Parâmetro ""Tel"" só pode conter números".Trim();
                case ErroSmartApi.TelTamanhoExcedido: return @"Parâmetro ""Tel"" deve ser informado com tamanho máximo de 13 caracteres ".Trim();
                case ErroSmartApi.TipoDocumentoInvalido: return @"Parâmetro ""tipoDocumento"" inválido (somente ""RG"" ou ""RNE"" quando o ""CPF"" informado é o mesmo que o ""idColaboradorIntegrador"")".Trim();
                case ErroSmartApi.TipoDocumentoNumDocumentoObrigatorios: return @"Parâmetros ""TipoDocumento"" e ""NumDocumento"" obrigatórios na ausência do ""CPF""".Trim();
                case ErroSmartApi.TipoDocumentoTamanhoExcedido: return @"Parâmetro ""tipoDocumento"" deve ser informado com tamanho máximo de 10 caracteres ".Trim();
                case ErroSmartApi.TipoOperacaoInvalido: return @"Parâmetro ""TipoOperacao"" deve ser preenchido com ""I"" ";
                case ErroSmartApi.TipoOperacaoObrigatorio: return @"Parâmetro ""TipoOperacao"" obrigatório ";
                case ErroSmartApi.UFInvalido: return @"Parâmetro ""UF"" informado inválido".Trim();
                case ErroSmartApi.UFObrigatorio: return @"Parâmetro ""UF""  obrigatório ".Trim();
                case ErroSmartApi.UFTamanhoExcedido: return @"Parâmetro ""UF"" deve ser informado com tamanho máximo de 2 caracteres ".Trim();
                default: return @"Erro não especificado".Trim();
            }
        }
    }
}
