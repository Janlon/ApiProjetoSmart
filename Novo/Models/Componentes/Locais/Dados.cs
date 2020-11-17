using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WEBAPI_VOPAK.Models;

namespace WEBAPI_VOPAK
{

    /// <summary>
    /// Auxiliar estática para lide com persistência dos dados.
    /// </summary>
    namespace Data
    {

        #region Operações CRUD

        /// <summary>
        /// Relacionamentos entre Contratada e Colaborador.
        /// </summary>
        public static class TB_RelacContratadaColaborador
        {
            /// <summary>
            /// Retorna se existe um registro de relacionamento entre contratada e colaborador para os valores indicados.
            /// </summary>
            /// <param name="ambient"></param>
            /// <param name="idColaborador"></param>
            /// <param name="idContratada"></param>
            /// <returns></returns>
            public static bool ExisteRelacContratadaColaborador(string ambient, int idColaborador = 0, int idContratada = 0)
            {
                bool ret = false;
                try
                {
                    string sql = @" SELECT  
                                        COUNT(*) AS [Rows]  
                                    FROM  
                                        [TB_RelacContratadaColaborador]  
                                    WHERE  
                                        ([IdContratada] = @IdContratada)   
                                        AND ([IdColaborador = @IdColaborador)";
                    using (DbHelper db = new DbHelper(ambient, sql))
                    {
                        db.Add("@IdContratada", idContratada, DbType.Int32);
                        db.Add("@IdColaborador", idColaborador, DbType.Int32);
                        ret = db.QueryFirstOrDefault<int>() > 0;
                    }
                }
                catch (Exception ex) { ex.Log(); ret = false; }
                return ret;
            }
            /// <summary>
            /// Insere um registro de relacionamento entre contratada e colaborador.
            /// </summary>
            /// <param name="ambient"></param>
            /// <param name="idContratada"></param>
            /// <param name="idColaborador"></param>
            /// <param name="usuarioId"></param>
            /// <param name="ativo"></param>
            /// <param name="excluido"></param>
            /// <returns></returns>
            public static bool InsereRelacContratadaColaborador(string ambient, int idContratada = 0, int idColaborador = 0, int usuarioId = 0, bool ativo = true, bool excluido = false)
            {
                bool ret = false;
                if (!ExisteRelacContratadaColaborador(ambient, idContratada, idColaborador))
                {
                    try
                    {
                        DateTime? dtCadastro = DateTime.Now;
                        DateTime? dtAlteracao = dtCadastro.Value;
                        TipoDeOperacao operacao = TipoDeOperacao.Incluir;
                        string sql = @"INSERT INTO [TB_RelacContratadaColaborador] (
                                            [IdContratada]
                                            ,[IdColaborador]
                                            ,[UsuarioId]
                                            ,[DtCadastro]
                                            ,[DtAlteracao]
                                            ,[CdOperacao]
                                            ,[CdAtivo]
                                            ,[cdExcluido]
                                       ) VALUES (
	                                        @IdContratada,
                                            @IdColaborador,
                                            @UsuarioId,
                                            @DtCadastro,
                                            @DtAlteracao,
                                            @CdOperacao,
                                            @CdAtivo,
                                            @cdExcluido)";
                        using (DbHelper db = new DbHelper(ambient, sql))
                        {
                            db.Add("@IdContratada", idContratada, DbType.Int32);
                            db.Add("@IdColaborador", idColaborador, DbType.Int32);
                            db.Add("@UsuarioId", usuarioId, DbType.Int32);
                            db.Add("@DtCadastro", dtCadastro.Value, DbType.Int32);
                            db.Add("@DtAlteracao", dtAlteracao.Value, DbType.Int32);
                            db.Add("@CdOperacao", (int)operacao, DbType.Int32);
                            db.Add("@CdAtivo", ativo, DbType.Boolean);
                            db.Add("@cdExcluido", excluido, DbType.Boolean);
                            ret = (db.Execute(true) > 0);
                        }
                    }
                    catch (Exception ex) { ex.Log(); ret = false; }
                }
                return ret;
            }
            /// <summary>
            /// Altera o status de ativo/inativo de um registro de relacionamento entre contratada e colaborador.
            /// </summary>
            /// <param name="ambient"></param>
            /// <param name="idContratada"></param>
            /// <param name="idColaborador"></param>
            /// <param name="usuarioId"></param>
            /// <param name="ativo"></param>
            /// <returns></returns>
            public static bool AtivaDesativaRelacContratadaColaborador(string ambient, int idContratada = 0, int idColaborador = 0, int usuarioId = 0, bool ativo = true)
            {
                bool ret = false;
                if (ExisteRelacContratadaColaborador(ambient, idContratada, idColaborador))
                {
                    try
                    {
                        DateTime? dtAlteracao = DateTime.Now;
                        TipoDeOperacao? operacao = (ativo ? TipoDeOperacao.Ativar : TipoDeOperacao.Desativar);
                        string sql = @"UPDATE [TB_RelacContratadaColaborador] SET
                                            [CdAtivo]       = @CdAtivo,
                                            [DtAlteracao]   = @DtAlteracao,
                                            [CdOperacao]    = @CdOperacao,
                                            [UsuarioId]     = @UsuarioId
                                        WHERE
                                            ([IdContratada] = @IdContratada)
                                            AND ([IdColaborador] = @IdColaborador)";
                        using (DbHelper db = new DbHelper(ambient, sql))
                        {
                            db.Add("@IdContratada", idContratada, DbType.Int32);
                            db.Add("@IdColaborador", idColaborador, DbType.Int32);
                            db.Add("@UsuarioId", usuarioId, DbType.Int32);
                            db.Add("@DtAlteracao", dtAlteracao.Value, DbType.Int32);
                            db.Add("@CdOperacao", operacao, DbType.Int32);
                            db.Add("@CdAtivo", ativo, DbType.Boolean);
                            ret = (db.Execute(true) > 0);
                        }
                    }
                    catch (Exception ex) { ex.Log(); ret = false; }
                }
                return ret;
            }
            /// <summary>
            /// Altera o status de ativo/inativo de um registro de relacionamento entre contratada e colaborador.
            /// </summary>
            /// <param name="ambient"></param>
            /// <param name="idContratada"></param>
            /// <param name="idColaborador"></param>
            /// <param name="usuarioId"></param>
            /// <param name="excluido"></param>
            /// <returns></returns>
            public static bool ExcluiRestauraRelacContratadaColaborador(string ambient, int idContratada = 0, int idColaborador = 0, int usuarioId = 0, bool excluido = true)
            {
                bool ret = false;
                if (ExisteRelacContratadaColaborador(ambient, idContratada, idColaborador))
                {
                    try
                    {
                        DateTime? dtAlteracao = DateTime.Now;
                        TipoDeOperacao operacao = (excluido ? TipoDeOperacao.Excluir : TipoDeOperacao.Resetar);
                        string sql = @"UPDATE [TB_RelacContratadaColaborador] SET
                                            [CdAtivo]       = @CdAtivo,
                                            [DtAlteracao]   = @DtAlteracao,
                                            [CdOperacao]    = @CdOperacao,
                                            [UsuarioId]     = @UsuarioId
                                        WHERE
                                            ([IdContratada] = @IdContratada)
                                            AND ([IdColaborador] = @IdColaborador)";
                        using (DbHelper db = new DbHelper(ambient, sql))
                        {
                            db.Add("@IdContratada", idContratada, DbType.Int32);
                            db.Add("@IdColaborador", idColaborador, DbType.Int32);
                            db.Add("@UsuarioId", usuarioId, DbType.Int32);
                            db.Add("@DtAlteracao", dtAlteracao.Value, DbType.Int32);
                            db.Add("@CdOperacao", operacao, DbType.Int32);
                            db.Add("@cdExcluido", excluido, DbType.Boolean);
                            ret = (db.Execute(true) > 0);
                        }
                    }
                    catch (Exception ex) { ex.Log(); ret = false; }
                }
                return ret;
            }
        }

        public static class TB_Contratada
        {


            /// <summary>
            /// Inclui um registro de Contratada.
            /// </summary>
            /// <param name="ambiente"></param>
            /// <param name="inicioValidade"></param>
            /// <param name="fimValidade"></param>
            /// <param name="cnpj"></param>
            /// <param name="razao"></param>
            /// <param name="fantasia"></param>
            /// <param name="endereco"></param>
            /// <param name="numero"></param>
            /// <param name="bairro"></param>
            /// <param name="cidade"></param>
            /// <param name="uf"></param>
            /// <param name="cep"></param>
            /// <param name="representante"></param>
            /// <param name="email"></param>
            /// <param name="usuarioId"></param>
            /// <param name="excluido"></param>
            /// <param name="ativo"></param>
            /// <returns></returns>
            public static bool InsereContratada(string ambiente, DateTime? inicioValidade, DateTime? fimValidade, string cnpj = "", string razao = "", string fantasia = "", string endereco = "", string numero = "", string bairro = "", string cidade = "", string uf = "", string cep = "", string representante = "", string email = "", int usuarioId = 0, bool excluido = false, bool ativo = true)
            {
                if (!inicioValidade.HasValue) inicioValidade = DateTime.Now;
                if (!fimValidade.HasValue) fimValidade = inicioValidade.Value.AddMonths(6);
                DateTime dtCadastro = DateTime.Now;
                DateTime dtAlteracao = dtCadastro;
                TipoDeOperacao operacao = TipoDeOperacao.Incluir;
                bool ret = false;
                try
                {
                    string sql = @"INSERT INTO [TB_Contratada] (
                                       [CdCnpj]
                                       ,[DsRazaoSocial]
                                       ,[DsNomeFantasia]
                                       ,[UsuarioId]
                                       ,[DtCadastro]
                                       ,[DtAlteracao]
                                       ,[CdOperacao]
                                       ,[CdAtivo]
                                       ,[Endereco]
                                       ,[NumEndereco]
                                       ,[BairroEndereco]
                                       ,[Cidade]
                                       ,[UF]
                                       ,[CEP]
                                       ,[Representante]
                                       ,[EmailRepresentante]
                                       ,[ValidadeDtInicio]
                                       ,[ValidadeDtTermino]
                                       ,[cdExcluido]
                                    ) VALUES(
                                       @CdCnpj,
                                       @DsRazaoSocial,
                                       @DsNomeFantasia,
                                       @UsuarioId,
                                       @DtCadastro,
                                       @DtAlteracao,
                                       @CdOperacao,
                                       @CdAtivo,
                                       @Endereco,
                                       @NumEndereco,
                                       @BairroEndereco,
                                       @Cidade,
                                       @UF,
                                       @CEP,
                                       @Representante,
                                       @EmailRepresentante,
                                       @ValidadeDtInicio,
                                       @ValidadeDtTermino,
                                       @cdExcluido)";
                    using (DbHelper db = new DbHelper(ambiente, sql))
                    {
                        db.Add("CdCnpj", cnpj.Normalized(), DbType.String);
                        db.Add("DsRazaoSocial", razao.Normalized(), DbType.String);
                        db.Add("DsNomeFantasia", fantasia.Normalized(), DbType.String);
                        db.Add("UsuarioId", usuarioId, DbType.Int32);
                        db.Add("DtCadastro", dtCadastro, DbType.DateTime);
                        db.Add("DtAlteracao", dtAlteracao, DbType.DateTime);
                        db.Add("CdOperacao", (int)operacao, DbType.Int32);
                        db.Add("CdAtivo", ativo, DbType.Int32);
                        db.Add("Endereco", endereco.Normalized(), DbType.String);
                        db.Add("NumEndereco", numero.Normalized(), DbType.String);
                        db.Add("BairroEndereco", bairro.Normalized(), DbType.String);
                        db.Add("Cidade", cidade.Normalized(), DbType.String);
                        db.Add("UF", uf.Normalized(), DbType.String);
                        db.Add("CEP", cep.Normalized(), DbType.String);
                        db.Add("Representante", representante.Normalized(), DbType.String);
                        db.Add("EmailRepresentante", email.Normalized(StringCases.Lower), DbType.String);
                        db.Add("ValidadeDtInicio", inicioValidade.Value, DbType.DateTime);
                        db.Add("ValidadeDtTermino", fimValidade.Value, DbType.DateTime);
                        db.Add("cdExcluido", excluido, DbType.Boolean);
                    }
                }
                catch (Exception ex) { ex.Log(); ret = false; }
                return ret;
            }
        }







    }
    public static class Dados
    {

        /// <summary>
        /// Inserir um registro de OCR.
        /// </summary>
        /// <param name="captura"></param>
        /// <returns></returns>
        internal static int GravaOcr(Captura captura)
        {
            int ret = 0;
            try
            {
                string sql = @" INSERT INTO TB_CapturaOCR (
                                [IdLocal]
                                ,[Pesagem]
                                ,[TextoPlacaCarmen]
                                ,[TextoConfirmado]
                                ,[Score]
                            ) VALUES (@IdLocal
                                ,@Pesagem
                                ,@TextoPlacaCarmen
                                ,@Placa
                                ,@Score); 
                            SELECT CAST(SCOPE_IDENTITY() as int)";
                using (DbHelper db = new DbHelper(captura.Ambiente, sql))
                {
                    db.Add("@IdLocal", captura.IdLocal, DbType.Int32);
                    db.Add("@Pesagem", captura.Pesagem, DbType.Int32);
                    db.Add("@TextoPlacaCarmen", captura.TextoPlacaCarmen, DbType.String);
                    db.Add("@Placa", captura.Placa, DbType.String);
                    db.Add("@Score", captura.Score, DbType.Int32);
                    ret = db.Query<int>().Single();
                }
            }
            catch (Exception ex) { ex.Log(); ret = 0; }
            return ret;
        }

        /// <summary>
        /// Insere um registro de ImagemOCR
        /// </summary>
        /// <param name="captura"></param>
        /// <returns></returns>
        internal static int GravaImagem(Captura captura)
        {
            int ret = 0;
            try
            {
                string sql = @" INSERT INTO [TB_CapturaImagemOCR] (
                                [IdOCR],
                                [Image]
                            ) VALUES (
                                @IdOCR
                                ,@Image); 
                            SELECT CAST(SCOPE_IDENTITY() as int)";
                using (DbHelper db = new DbHelper(captura.Ambiente, sql, "@IdOCR", captura.IdOCR, DbType.Int32))
                {
                    db.Add("@Image", captura.Image, DbType.Binary);
                    ret = db.Query<int>().Single<int>();
                }
            }
            catch(Exception ex) { ex.Log(); ret = 0; }
            return ret;
        }




        /// <summary>
        /// Vincula um colaborador e uma contratada (TB_RelacContratadaColaborador).
        /// </summary>
        /// <param name="ambient">Define se deve utilizar o ambiente de teste ou de produção.</param>
        /// <param name="idColaborador">Identificação do registro de colaborador.</param>
        /// <param name="idContratada">Identificação do registro de contratada.</param>
        /// <returns>
        /// Sucesso da operação.
        /// </returns>
        public static bool RelacionaContrtadaColaborador(string ambient, int idColaborador, int idContratada, int idExterno)
        {
            bool ret = false;
            try
            {
                int rows = 0;
                string sql = @"SELECT  
                                    COUNT(*) AS [Rows]  
                               FROM  
                                    [TB_RelacContratadaColaborador]  
                               WHERE  
                                    ([IdContratada] = @IdContratada)   
                                    AND ([IdColaborador = @IdColaborador)";
                using (DbHelper db = new DbHelper(ambient, sql))
                {
                    db.Add("@IdContratada", idContratada, DbType.Int32);
                    db.Add("@IdColaborador", idColaborador, DbType.Int32);
                    rows = db.QueryFirstOrDefault<int>();
                    if (rows == 0)
                    {
                        db.SqlStatement = @"INSERT INTO TB_RelacContratadaColaborador (
			                                    IdContratada,IdColaborador,UsuarioId,CdOperacao 
		                                    ) VALUES (
			                                    @IdContratada,@IdColaborador,@UsuarioId,@CdOperacao)";
                        ret = (db.Execute(true) > 0);
                    }
                    db.SqlStatement = @"SELECT * FROM [TB_IntegraColaborador] WHERE ([IdColaborador] = @IdColaborador) AND ([IdContratada] = @IdContratada) AND ([IdExterno] = @IdExterno)";
                    db.Add("@IdExterno", idExterno, DbType.Int32);
                    rows = db.QueryFirstOrDefault<int>();
                    if (rows == 0)
                    {
                        db.Add("@IdIntegra", 1, DbType.Int32);
                        db.Add("@Display1", "", DbType.String);
                        db.Add("@Display2", "", DbType.String);
                        db.SqlStatement = @"INSERT INTO TB_IntegraColaborador (
                                                IdColaborador       ,IdContratada
                                                ,IdExterno          ,IdIntegra
                                                ,Display1           ,Display2
                                          ) VALUES (
                                                @IdColaborador      ,@IdContratada		 
                                                ,@IdExterno         ,@IdIntegra      	
                                                ,@Display1          ,@Display2 )";
                        ret = (db.Execute() > 0);
                    }
                }
            }
            catch { ret = false; }
            return ret;
        }

        /// <summary>
        /// Inclui uma foto de colaborador no cadastro de imagens.
        /// </summary>
        /// <param name="ambient">Define se deve utilizar o ambiente de teste ou de produção.</param>
        /// <param name="imagemBytes">Mariz com a imagem à ser grvada.</param>
        /// <param name="idColaborador">Identificação do registro de colaborador ao qual a imagem pertencerá.</param>
        /// <returns>Sucesso da operação.</returns>
        public static bool InsereImagemDoColaborador(string ambient, string imagemBytes, int idColaborador, string cpf)
        {
            bool ret = false;

            byte[] b = null;
            //try
            //{
            //    if (imagemBytes.IsBase64())
                    b = (string.IsNullOrEmpty(imagemBytes) ? Models.Componentes.CameraUtil.SemImagemBytes : Convert.FromBase64String(imagemBytes));
            //    else
            //        b = Models.Componentes.CameraUtil.SemImagemBytes;
            //}
            //catch { b = Models.Componentes.CameraUtil.SemImagemBytes; }
            //if(b==null)
            //    b = Models.Componentes.CameraUtil.SemImagemBytes;

            try
            {
                using (DbHelper db = new DbHelper(ambient,
                    "DELETE FROM [TB_PESSOA_IMAGENS] WHERE ([IdColaborador]=@IdColaborador)",
                    "@IdColaborador", idColaborador, DbType.Int32))
                {
                    ret = (db.Execute(true) > 0);
                    db.Add("@img", b, DbType.Binary);
                    db.Add("@cpf", cpf.JustNumbers().Normalized(), DbType.String);
                    db.SqlStatement = @"INSERT INTO [dbo].[TB_PESSOA_IMAGENS] (
                                          [IdColaborador],  [Cadastro], [Modelo],       
                                          [Imagem],         [StCracha], [VlLiberacoes], [CPF]
                                        ) VALUES (
                                          @IdColaborador,   GETDATE(),  null,
                                          @img,             1,          0,              @cpf);";
                    ret = (db.Execute(true) > 0);
                }
            }
            catch { ret = false; }
            return ret;
        }

        internal static bool NumDocumentoDuplicado(string ambient, int id, string numDocumento, string cpf)
        {
            bool ret = false;
            try
            {
                if ((string.IsNullOrEmpty(numDocumento)) || 
                    (string.IsNullOrWhiteSpace(numDocumento)))
                    numDocumento = cpf
                        .JustNumbers()
                        .Normalized();
                string sql= @"SELECT 
                                    COUNT( [IdColaborador] )  
                              FROM 
                                    [TB_Colaborador] 
                              WHERE 
                                    ( [IdTipoDocumento] = @Id ) 
                                    AND ( UPPER(LTRIM(RTRIM([DsDocumento]))) = UPPER(LTRIM(RTRIM(@numDocumento))) ) 
                                    AND ( UPPER(LTRIM(RTRIM([CdCpf]))) <> UPPER(LTRIM(RTRIM(@CdCpf))) )";
                using (DbHelper db = new DbHelper(ambient,sql, "@Id", id, DbType.Int32))
                {
                    db.Add("@CdCpf", cpf.JustNumbers().Normalized(), DbType.String);
                    db.Add("@numDocumento", numDocumento.JustNumbers().Normalized(), DbType.String);
                    ret = (db.QueryFirstOrDefault<int>(true) > 0);
                }
            }
            catch { ret = false; }
            return ret;
        }

        /// <summary>
        /// Cria um crachá vinculado mediante o CNPJ da contradada e o indentificador do colaborador com o hexa informado. 
        /// Obs.:
        /// <para>
        /// Opcionalmente, antes de efetuar a inclusão do crachá, esta rotina pode 
        /// desativr todos os crachás anteriores vinculados ao mesmo colaborador.
        /// </para>
        /// </summary>
        /// <param name="ambient">Define se deve utilizar o ambiente de teste ou de produção.</param>
        /// <param name="hexa">Texto com a chave hexadecimal do crachá à ser gerado.</param>
        /// <param name="cnpj">CNPJ da contratada.</param>
        /// <param name="idColaborador">Identificação do registro de colaborador.</param>
        /// <returns>Sucesso da operação.</returns>
        public static bool GeraCrachaDeColaborador(string ambient, string hexa, string cnpj, int idColaborador)
        {
            bool ret = false;
            try
            {
                using (DbHelper db = new DbHelper(ambient, "sp_CrachaInserir"))
                {
                    db.Add("CdCnpj", cnpj, DbType.String);
                    db.Add("CdCracha", hexa, DbType.String);
                    db.Add("IdColaborador", idColaborador, DbType.Int32);
                    db.Add("UsuarioId", 1, DbType.Int32);
                    db.Add("CdOperacao", 10, DbType.Int32);
                    db.Add("IdTipoCracha", 1, DbType.Int32);
                    ret = (db.Execute(true) > 0);
                }
            }
            catch { ret = false; }
            return ret;
        }

        /// <summary>
        /// Atualiza o vinculo de integração tornando-o inativo.
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="idColaborador"></param>
        /// <returns></returns>
        public static bool IntegraColaboradorDesativar(string ambient, int idColaborador)
        {
            bool ret = false;
            try
            {
                using (DbHelper db = new DbHelper(ambient, "sp_IntegraColaboradorDesativar", "@IdColaborador", idColaborador))
                {
                    db.Add("Display1", "", DbType.String);
                    db.Add("Display2", "", DbType.String);
                    ret = (db.Execute(true) > 0);
                }
            }
            catch { ret = false; }
            return ret;
        }

        /// <summary>
        /// Atualiza o vínculo de integração tornando-o ativo.
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="idColaborador"></param>
        /// <returns></returns>
        public static bool IntegraColaboradorAtivar(string ambient, int idColaborador)
        {
            bool ret = false;
            try
            {
                using (DbHelper db = new DbHelper(ambient, "sp_IntegraColaboradorAtivar", "@IdColaborador", idColaborador))
                {
                    db.Add("Display1", "", DbType.String);
                    db.Add("Display2", "", DbType.String);
                    ret = (db.Execute(true) > 0);
                }
            }
            catch { ret = false; }
            return ret;
        }




        public static bool AlteraColaborador(string ambient, int? idEmpresaIntegrador = 0, int? idColaboradorIntegra = 0, int? idTipoColaborador = 0, bool? ativo = true, bool? excluido = false, int? idColaborador = 0, string nome = "", string cnpj = "", string cpf = "", string cnh = "", string orgaoEmissorCnh = "", string emissorCnh = "", string nrNis = "", string numDocumento = "", int? idTipoDocumento = 0, string sexo = "", DateTime? nascimento = null, DateTime? dtValidadeInicio = null, DateTime? dtValidadeTermino = null, string foto = "", string endereco = "", string numEndereco = "", string bairroEndereco = "", string cidade = "", string uf = "", string cep = "", string tel = "", string emailColaborador = "", int? usuarioId = 0, int? idPerfilColaborador = 0)
        {
            bool ret = false;
            string sql = @"UPDATE TB_Colaborador SET 
	                            DtNascimento		=	@DtNascimento, 
	                            ValidadeDtInicio	=	@ValidadeDtInicio, 
	                            ValidadeDtTermino	=	@ValidadeDtTermino,
	                            DtAlteracao			=	GETDATE(),
                                UsuarioId			=	@UsuarioId, 
	                            CdOperacao			=	@CdOperacao, 
	                            IdTipoColaborador	=	@IdTipoColaborador, 
	                            IdTipoDocumento     =   @IdTipoDocumento,
	                            CdAtivo				=	@CdAtivo,
	                            cdExcluido			=	@CdExcluido,
	                            DsColaborador		=	@DsColaborador, 
	                            CdCpf				=	@CdCpf, 
	                            CdCnpj				=	@CdCnpj, 	                            
	                            DsGenero			=	@DsGenero,
	                            NrNis				=	@NrNis, 
	                            DsDocumento         =   @DsDocumento,
	                            DsObservacao		=	@DsObservacao,
	                            Endereco			=	@Endereco, 
	                            NumEndereco		    =   @NumEndereco,
	                            BairroEndereco		=   @BairroEndereco, 
	                            Cidade				=   @Cidade, 
	                            UF					=	@UF,
	                            CEP					=	@CEP, 
	                            Tel					= 	@Tel,
	                            Funcao				=	@Funcao, 
	                            EmailColaborador	=	@EmailColaborador, 
	                            Cnh                 =   @Cnh,
	                            OrgaoEmissorCNH     =   @OrgaoEmissorCNH,
	                            EmissorCNH          =   @EmissorCNH,
                                IdPerfilColaborador =   @IdPerfilColaborador
                            WHERE	
	                            (IdColaborador		= @IdColaborador)";
            TipoDeOperacao top = TipoDeOperacao.Alterar;
            DateTime inicio = (dtValidadeInicio.HasValue) ? dtValidadeInicio.Value : DateTime.Now;
            DateTime fim = (dtValidadeTermino.HasValue) ? dtValidadeTermino.Value : inicio.AddMonths(6);
            DateTime nascto = (nascimento.HasValue) ? nascimento.Value : new DateTime(1899, 1, 1);
            try
            {
                using (DbHelper db = new DbHelper(ambient, sql))//"sp_ColaboradorEditar"
                {
                    db.Add("IdPerfilColaborador", idPerfilColaborador.Value, DbType.Int32);
                    db.Add("DtNascimento", nascto, DbType.Date);
                    db.Add("ValidadeDtInicio", inicio.Date, DbType.Date);
                    db.Add("ValidadeDtTermino", fim.Date, DbType.Date);
                    db.Add("UsuarioId", usuarioId.Value, DbType.Int32);
                    db.Add("CdOperacao", (int)top, DbType.Int32);
                    db.Add("IdTipoColaborador", idTipoColaborador, DbType.Int32);
                    db.Add("IdTipoDocumento", idTipoDocumento, DbType.Int32);
                    db.Add("IdColaborador", idColaborador, DbType.Int32);
                    db.Add("CdAtivo", ativo.Value, DbType.Boolean);
                    db.Add("CdExcluido", excluido.Value, DbType.Boolean);
                    db.Add("DsColaborador", nome.Normalized(), DbType.String);
                    db.Add("CdCpf", cpf.JustNumbers().Normalized(), DbType.String);
                    db.Add("CdCnpj", cnpj.JustNumbers().Normalized(), DbType.String);
                    db.Add("DsGenero", sexo.Normalized(), DbType.String);
                    db.Add("NrNis", nrNis.JustNumbers().Normalized(), DbType.String);
                    db.Add("DsDocumento", numDocumento.DefaultIfNull(cpf.JustNumbers()).Normalized(), DbType.String);
                    db.Add("DsObservacao", "".Trim().ToUpper(), DbType.String);
                    db.Add("Endereco", endereco.Normalized(), DbType.String);
                    db.Add("NumEndereco", numEndereco.Normalized(), DbType.String);
                    db.Add("BairroEndereco", bairroEndereco.Normalized(), DbType.String);
                    db.Add("Cidade", cidade.Normalized(), DbType.String);
                    db.Add("UF", uf.Normalized(), DbType.String);
                    db.Add("CEP", cep.JustNumbers().Normalized(), DbType.String);
                    db.Add("Tel", tel.JustNumbers().Normalized(), DbType.String);
                    db.Add("Funcao", "".Normalized(), DbType.String);
                    db.Add("EmailColaborador", emailColaborador.Normalized(StringCases.Lower), DbType.String);
                    db.Add("Cnh", cnh.JustNumbers().Normalized(), DbType.String);
                    db.Add("OrgaoEmissorCNH", orgaoEmissorCnh.Normalized(), DbType.String);
                    db.Add("EmissorCNH", emissorCnh.Normalized(), DbType.String);
                    ret = (db.Execute(true) > 0);
                    if (ret)
                        ret = InsereIntegraColaborabor(ambient, Dados.GetIdContratadaIntegrada(ambient, 1, idEmpresaIntegrador.Value, false), idColaborador, idColaboradorIntegra);
                }
            }
            catch (Exception ex) { ex.Log(); ret = false; }
            return ret;
        }

        public static bool InsereRelacContratadaColaborabor(string ambiente, int? usuarioId=0, int? idColaborador=0, int? idContratada=0)
        {
            bool ret = false;
            try
            {
                TipoDeOperacao top = TipoDeOperacao.Incluir;
                using (DbHelper db = new DbHelper(ambiente))
                {
                    db.SqlStatement = @"SELECT COUNT(IdColaborador) FROM [TB_RelacContratadaColaborador]
		                                WHERE ([IdContratada]=@IdContratada) AND ([IdColaborador]=@IdColaborador)";
                    db.Add("IdColaborador", idColaborador, DbType.Int32);
                    db.Add("IdContratada", idContratada, DbType.Int32);
                    ret = (db.QueryFirstOrDefault<int>(true) > 0);
                    if(!ret)
                    {
                        db.SqlStatement = @"INSERT INTO [TB_RelacContratadaColaborador]
		                                    ([IdContratada], [IdColaborador], [UsuarioId], [CdOperacao]) 		
	                                    VALUES
		                                    (@IdContratada, @IdColaborador, @UsuarioId, @CdOperacao)";
                        db.Add("CdOperacao", (int)top, DbType.Int32);
                        db.Add("UsuarioId", usuarioId.Value, DbType.Int32);
                        ret = (db.Execute(true) > 0);
                    }
                }
            }
            catch (Exception ex) { ex.Log(); ret = false; }
            return ret;
        }
        public static bool InsereIntegraColaborabor(string ambiente, int? idContratada = 0, int? idColaborador = 0, int? idColaboradorIntegra = 0)
        {
            bool ret = false;
            try
            {
                string sql = @"SELECT 
	                                COUNT([IdColaborador])
                               FROM 
	                                [dbo].[TB_IntegraColaborador]
                               WHERE
	                                ([IdContratada]			= @IdContratada)
	                                AND ([IdColaborador]	= @IdColaborador)
	                                AND ([IdExterno]		= @IdColaboradorIntegrador)
	                                AND ([IdIntegra]		= 1)";
                using (DbHelper db = new DbHelper(ambiente, sql, "@IdColaborador", idColaborador))
                {
                    db.Add("IdContratada", idContratada, DbType.Int32);
                    db.Add("IdColaboradorIntegrador", idColaboradorIntegra, DbType.Int32);
                    ret = (db.QueryFirstOrDefault<int>(true) > 0);
                    if (!ret)
                    {
                        db.SqlStatement = @"INSERT INTO [TB_IntegraColaborador] (
                                                [IdColaborador]
                                                ,[IdContratada]
                                                ,[IdExterno]
                                                ,[IdIntegra]
                                                ,[Display1]
                                                ,[Display2]
                                                ,[IsAtivo]
                                                ,[IsAtivoContratada]
                                                ,[DtCadastro]
                                            ) VALUES (
	                                            @IdColaborador
                                                ,@IdContratada
                                                ,@IdColaboradorIntegrador
                                                ,1
                                                ,N''
                                                ,N''
                                                ,1
                                                ,1
                                                ,GETDATE())";
                        ret = (db.Execute(true) > 0);
                    }
                }
            }
            catch (Exception ex) { ex.Log(); ret = false; }
            return ret;
        }
        public static bool InsereCrachaColaborador(string ambient, int? usuarioId, int? idTipoCracha, string cnpj, int idColaborador, string numCracha)
        {
            bool ret = false;
            try
            {
                TipoDeOperacao top = TipoDeOperacao.Incluir;
                string sql = @"INSERT INTO [TB_Cracha] (
                                   [CdCracha]
                                   ,[UsuarioId]
                                   ,[DtCadastro]
                                   ,[DtAlteracao]
                                   ,[CdOperacao]
                                   ,[CdAtivo]
                                   ,[cdExcluido]
                                   ,[IdColaborador]
                                   ,[CdCnpj]
                                   ,[IdTipoCracha]
                                   ,[CdDisponivel]
                                   ,[Motivo]
                                   ,[FlagDiretor]
                             ) VALUES (
                                   @CdCracha --varchar(150),>
                                   ,@UsuarioId --int,>
                                   ,GETDATE() --datetime,>
                                   ,GETDATE() --datetime,>
                                   ,@CdOperacao --int,>
                                   ,@CdAtivo --bit,>
                                   ,@cdExcluido --bit,>
                                   ,@IdColaborador --int,>
                                   ,@CdCnpj --varchar(14),>
                                   ,@IdTipoCracha --int,>
                                   ,@CdDisponivel --bit,>
                                   ,@Motivo --varchar(100),>
                                   ,@FlagDiretor) --int,>)";
                using (DbHelper db = new DbHelper(ambient, sql))
                {
                    db.Add("CdCracha", numCracha, DbType.String);
                    db.Add("UsuarioId", usuarioId, DbType.Int32);
                    db.Add("CdOperacao", (int)top, DbType.Int32);
                    db.Add("CdAtivo", true, DbType.Boolean);
                    db.Add("cdExcluido", false, DbType.Boolean);
                    db.Add("IdColaborador", idColaborador, DbType.Int32);
                    db.Add("CdCnpj", cnpj, DbType.String);
                    db.Add("IdTipoCracha", idTipoCracha, DbType.Int32);
                    db.Add("CdDisponivel", false, DbType.Boolean);
                    db.Add("Motivo", "Emissão de primeira via", DbType.String);
                    db.Add("FlagDiretor", 0, DbType.Int32);
                    ret = (db.Execute(true) > 0);
                }
            }
            catch (Exception ex) { ex.Log(); ret = false; }
            return ret;
        }

        public static int InsereColaborador(string ambient, int? idColaboradorIntegra = 0, int? idTipoColaborador = 0, int? idContratada = 0, string nome = "", string cnpj = "", string cpf = "", string cnh = "", string orgaoEmissorCnh = "", string emissorCnh = "", string nrNis = "", string numDocumento = "", int? idTipoDocumento = 0, string sexo = "", DateTime? nascimento = null, DateTime? dtValidadeInicio = null, DateTime? dtValidadeTermino = null, string foto = "", string endereco = "", string numEndereco = "", string bairroEndereco = "", string cidade = "", string uf = "", string cep = "", string tel = "", string emailColaborador = "", int? usuarioId = 0, string numCracha="", int? idPerfilColaborador=0)
        {
            int ret = 0;
            try
            {
                // Colaboradores inseridos por aqui são sempre motoristas...
                int? idTipoCracha = GetIdTipoCracha(ambient, "Motorista");
                bool? ativo = true, excluido = false;
                string sql = @"INSERT INTO TB_Colaborador (
			                        DsColaborador,
			                        CdCpf,					
			                        CdCnpj,
			                        CdAtivo, 
			                        CdExcluido, 
			                        IdTipoColaborador, 		
			                        UsuarioId,
			                        CdOperacao,
			                        DtNascimento, 
			                        DsGenero,
			                        DsObservacao,
			                        Endereco, 
			                        NumEndereco,
			                        NrNis, 
			                        BairroEndereco, 
			                        Cidade, 
			                        UF,
			                        CEP, 
			                        Funcao, 
			                        EmailColaborador, 
			                        Tel,
			                        Cnh,
			                        OrgaoEmissorCNH,
			                        EmissorCNH ,
			                        ValidadeDtInicio, 
			                        ValidadeDtTermino,
			                        IdTipoDocumento,
			                        DsDocumento, 
                                    IdPerfilColaborador
		                        ) VALUES (
			                        @DsColaborador, 		
			                        @CdCpf,					
			                        @CdCnpj,
                                    @CdAtivo,
                                    @CdExcluido,
			                        @IdTipoColaborador, 	
			                        @UsuarioId,
			                        @CdOperacao,
			                        @DtNascimento, 
			                        @DsGenero,
			                        @DsObservacao,
			                        @Endereco, 
			                        @NumEndereco,
			                        @NrNis, 
			                        @BairroEndereco, 
			                        @Cidade, 
			                        @UF,
			                        @CEP, 
			                        @Funcao, 
			                        @EmailColaborador, 
			                        @Tel,
			                        @Cnh,
			                        @EmissorCNH ,
			                        @OrgaoEmissorCNH,
			                        @ValidadeDtInicio, 
			                        @ValidadeDtTermino,
			                        @IdTipoDocumento,
			                        @DsDocumento,
			                        @IdPerfilColaborador);
			                        SELECT CAST(SCOPE_IDENTITY() as int) AS [Id]";
                TipoDeOperacao top = TipoDeOperacao.Incluir;
                DateTime inicio = (dtValidadeInicio.HasValue) ? dtValidadeInicio.Value : DateTime.Now;
                DateTime fim = (dtValidadeTermino.HasValue) ? dtValidadeTermino.Value : inicio.AddMonths(6);
                DateTime nascto = (nascimento.HasValue) ? nascimento.Value : new DateTime(1899, 1, 1);
                using (DbHelper db = new DbHelper(ambient, sql))//"sp_ColaboradorInserir"
                {
                    //,< IdRegraAcesso, int,>                
                    //,< DtCadastro, datetime,>                
                    //,< DtAlteracao, datetime,>
                    //,< DtValidadeLib, datetime,>
                    //,< IdPerfilColaborador, int,>
                    db.Clear();
                    db.Add("UsuarioId", usuarioId.Value, DbType.Int32);                     //UsuarioId, int,>  
                    db.Add("CdOperacao", (int)top, DbType.Int32);                           //CdOperacao, int,>                
                    db.Add("IdTipoColaborador", idTipoColaborador.Value, DbType.Int32);     //IdTipoColaborador, int,>                
                    db.Add("IdTipoDocumento", idTipoDocumento.Value, DbType.Int32);         //IdTipoDocumento, int,>                
                    db.Add("DtNascimento", nascto, DbType.DateTime);                        //DtNascimento, datetime,>                
                    db.Add("CdAtivo", ativo.Value, DbType.Boolean);                         //CdAtivo, bit,>                
                    db.Add("CdExcluido", excluido.Value, DbType.Boolean);                   //cdExcluido, bit,>                
                    db.Add("DsColaborador", nome.Normalized(), DbType.String);              //DsColaborador, varchar(50),>
                    db.Add("CdCPF", cpf.JustNumbers().Normalized(), DbType.String);         //CdCpf, varchar(11),>
                    db.Add("CdCnpj", cnpj.JustNumbers().Normalized(), DbType.String);       //CdCnpj, varchar(14),>
                    db.Add("DsGenero", sexo.Normalized(), DbType.String);                   //DsGenero, char(1),>                
                    db.Add("NrNis", nrNis.JustNumbers().Normalized(), DbType.String);       //NrNis, varchar(11),>                
                    db.Add("DsDocumento", numDocumento.Normalized(), DbType.String);        //DsDocumento, varchar(20),>
                    db.Add("DsObservacao", "".Trim().ToUpper(), DbType.String);             //DsObservacao, varchar(100),>
                    db.Add("Endereco", endereco.Normalized(), DbType.String);               //Endereco, varchar(200),>
                    db.Add("NumEndereco", numEndereco.Normalized(), DbType.String);         //NumEndereco, nchar(10),>
                    db.Add("BairroEndereco", bairroEndereco.Normalized(), DbType.String);   //BairroEndereco, varchar(100),>
                    db.Add("Cidade", cidade.Normalized(), DbType.String);                   //Cidade, varchar(50),>                
                    db.Add("UF", uf.Normalized(), DbType.String);                           //UF, nchar(2),>                
                    db.Add("CEP", cep.JustNumbers().Normalized(), DbType.String);           //CEP, nchar(8),>                
                    db.Add("TEL", tel.JustNumbers().Normalized(), DbType.String);           //Tel, nchar(13),>
                    db.Add("Funcao", "".Normalized(), DbType.String);                       //Funcao, varchar(50),>
                    db.Add("EmailColaborador", emailColaborador
                        .Normalized(StringCases.Lower), 
                        DbType.String);                                                     //EmailColaborador, varchar(100),>
                    db.Add("Cnh", cnh.JustNumbers().Normalized(), DbType.String);           //CNH, nchar(11),>                
                    db.Add("OrgaoEmissorCNH", orgaoEmissorCnh.Normalized(), DbType.String); //OrgaoEmissorCNH, varchar(6),>
                    db.Add("EmissorCNH", emissorCnh.Normalized(), DbType.String);           //EmissorCNH, varchar(50),>                
                    db.Add("ValidadeDtInicio", inicio, DbType.Date);                        //ValidadeDtInicio, date,>
                    db.Add("ValidadeDtTermino", fim, DbType.Date);                          //ValidadeDtTermino, date,>
                    db.Add("IdPerfilColaborador", idPerfilColaborador.Value, DbType.Int32);
                    ret = db.Insert(true);
                    if (ret == 0)
                        ret = db.InsertComDapper();
                    if (ret > 0)
                    {
                        bool temp = InsereImagemDoColaborador(ambient, foto, ret, cpf);
                        temp = InsereRelacContratadaColaborabor(ambient, usuarioId, ret, idContratada);
                        temp = InsereIntegraColaborabor(ambient, idContratada, ret, idColaboradorIntegra);
                        //temp = InsereCrachaColaborador(ambient, usuarioId, idTipoCracha, cnpj, ret, numCracha);
                    }
                }
            }
            catch { ret = 0; }
            return ret;
        }

        



        /// <summary>
        /// Cancela ou finaliza uma O.S. em aberto e retorna o status do processo.
        /// </summary>
        /// <param name="item">Requisição do tipo <see cref="Models.CancelaOS"/> </param>
        /// <param name="finalizar">Indica se deve Cancelar (false) ou finalizar (true) a O.S. indicada.</param>
        /// <returns>Objeto do tipo <see cref="Retorno"/>.</returns>
        public static Retorno CancelaOS(CancelaOSViewModel item, bool finalizar = false)
        {
            Retorno retorno = new Retorno();
            try
            {
                // Consulta OSs cadastradas.
                string sql = @"SELECT  
                                    [Id], [IdSecao], [FlSaida], [DtSaida], [dtCancelamento] 
                               FROM 
                                    [TB_MotivacaoTemporaria]    A LEFT JOIN 
                                    [TB_Acesso]                 B ON (A.[Id] = B.[IdMotivacaoTemporaria])  
                               WHERE  
                                    (A.[OrdemServico] = @OS)";
                // Mantém as O.S. localizadas.
                IEnumerable<ConsultaOSViewModel> oss = null;
                // Consulta aos dados, sem referenciar Dapper ou Sql Server.
                using (DbHelper db = new DbHelper(item.ambiente, sql, "@OS", item.numOS, DbType.String))
                {
                    oss = db.Query<ConsultaOSViewModel>();
                    if (oss.Count() == 0)
                        // A O.S. não foi localizada.
                        retorno.AddResult(ErroSmartApi.NumOsNaoLocalizado);
                  //  else
                   // {
                        // A O.S. já foi usada?
                      //  if (oss.Where(p => p.IdSecao > 0).FirstOrDefault() != null)
                          //  retorno.AddResult(ErroSmartApi.NumOsFechado);
                       // else
                       // {
                            // A O.S. foi cancelada?
                         //   if (oss.Where(p => (p.dtCancelamento != null)).FirstOrDefault() != null)
                           //     retorno.AddResult(ErroSmartApi.NumOsCancelado);
                         //   else
                         //   {
                                // A O.S. já foi fechada? 
                                //...ok., então lembra que está gravando motivação em tudo quanto é canto?
                                // Acho que por conta disso, tem váááááááárias OS idênticas na base de dados.
                                // Dessa forma, teriam TODAS (para a mesma OS) de estar com esse flag como false, 
                                // pois se tiver um só registro com o flag marcado, aqui ele acusa que já saiu.
                                // É isso mesmo?
                              //  if (oss.Where(p => ((p.DtSaida != null) || (p.FlSaida))).FirstOrDefault() != null)
                               //     retorno.AddResult(ErroSmartApi.NumOsFechado);
                             //   else
                              //  {
                                    // Tentando cancelar ou finalizar a O.S. de acordo com os parâmetros.
                                    db.SqlStatement = ((finalizar == false) ?
                                        "SP_MotivacaoTemporariaCancelar" :
                                        "SP_MotivacaoTemporariaFinalizar");
                                    db.Clear();
                                    db.Add("@OrdemServico", item.numOS, DbType.String);
                                    int i = db.Execute();
                                    // Se alterou um ou mais registros, funcionou.
                                    if (i <= 0)
                                        retorno.AddResult(ErroSmartApi.FalhaDeConexao);
                                //}
                            //}
                       // }
                   // }
                }

            }
            catch (Exception ex) { ex.Log(); retorno.AddResult(ErroSmartApi.FalhaDeConexao); }
            return retorno;
        }


        /// <summary>
        /// Inclui uma motivação temporária.
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="idColaborador"></param>
        /// <param name="os"></param>
        /// <param name="placa"></param>
        /// <param name="wIdMotivacaoTemporaria"></param>
        /// <returns></returns>
        internal static bool IncluiMotivacaoTemporaria(string ambient, int idContratada, int idColaborador, string os, string placa, ref int wIdMotivacaoTemporaria)
        {
            bool ret = false;
            try
            {
                int wIdCracha = 0;
                dynamic temp = null;
                DateTime dti = DateTime.Now;
                DateTime dtf = dti.AddMonths(6);
                String sql = @"UPDATE TB_MotivacaoTemporaria SET
		                            DtSaida = GETDATE(), 
		                            FlSaida = 1, 
		                            Finalizador = 'Sphera' 
	                           WHERE 
		                            (DtSaida IS NULL) 
		                            AND (dtCancelamento IS NULL) 
		                            AND (FlSaida = 0) 
		                            AND (IdColaborador = @IdColaborador);";
                using (DbHelper db = new DbHelper(ambient, sql, "@IdColaborador", idColaborador, DbType.Int32))
                {
                    db.Execute(true);
                    db.SqlStatement = @"SELECT TOP(1)
	                                        A.IdCracha
                                        FROM 
	                                        [TB_Cracha]						A										 INNER JOIN 
	                                        [TB_Colaborador]				B ON (A.IdColaborador = B.IdColaborador) INNER JOIN 
	                                        [TB_RelacContratadaColaborador] C ON (A.IdColaborador = C.IdColaborador)
                                        WHERE 
	                                        (A.cdExcluido = 0) 
	                                        AND (A.CdAtivo = 1)
	                                        AND (A.[IdTipoCracha] <> 8)
	                                        AND (A.IdColaborador = @IdColaborador) 
	                                        AND (C.IdContratada = @IdContratada) 
                                        ORDER BY
                                            A.DtCadastro DESC";
                    db.Add("IdContratada", idContratada, DbType.Int32);
                    wIdCracha = db.QueryFirstOrDefault<int>(true);
                    //if (wIdCracha > 0)
                   // {
                        db.SqlStatement = @"INSERT INTO 
                                        TB_MotivacaoTemporaria
                                        (IdCracha, DtValidadeInicial, DtValidadeFinal, OrdemServico, Placa, IdColaborador)
                                        VALUES
                                        (@IdCracha, @ValidadeDtInicio, @ValidadeDtTermino, @OrdemServico, @Placa, @IdColaborador);
                                        SELECT CAST(scope_identity() AS INT)";
                        db.Add("IdCracha", wIdCracha, DbType.Int32);
                        db.Add("OrdemServico", os, DbType.String);
                        db.Add("Placa", placa, DbType.String);
                        db.Add("ValidadeDtInicio", dti, DbType.DateTime);
                        db.Add("ValidadeDtTermino", dtf, DbType.DateTime);
                        temp = db.Insert(true);
                    //}
                }
                if (temp != null)
                {
                    wIdMotivacaoTemporaria = (int)temp;
                    ret = true;
                }
            }
            catch (Exception ex) { ex.Log(); ret = false; }
            return ret;
        }
        /// <summary>
        /// Insere uma requisição enviada, válida ou não, no banco de dados, no momento em que a recebe.
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="item"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        internal static bool InsereRequisicao(string ambient, string item, string ip)
        {
            bool ret = false;
            string sql = "SP_RequisicaoApiInserir";
            using (DbHelper db = new DbHelper(ambient, sql, "@ip", ip, DbType.String))
            {
                db.Add("@texto", item, DbType.String);
                ret = (db.Execute(true) > 0);
            }
            return ret;
        }
        /// <summary>
        /// Altera os dados de uma empresa contratada.
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="idExterno"></param>
        /// <param name="cnpj"></param>
        /// <param name="razao"></param>
        /// <param name="fantasia"></param>
        /// <param name="endereco"></param>
        /// <param name="numero"></param>
        /// <param name="bairro"></param>
        /// <param name="cidade"></param>
        /// <param name="uf"></param>
        /// <param name="cep"></param>
        /// <param name="representante"></param>
        /// <param name="emailRepresentante"></param>
        /// <param name="validadeInicial"></param>
        /// <param name="validadeFinal"></param>
        /// <param name="ativo"></param>
        /// <param name="excluido"></param>
        /// <param name="usuarioId"></param>
        /// <param name="tipoOperacao"></param>
        /// <param name="idContratada"></param>
        /// <returns></returns>
        internal static bool AlteraContratada(string ambient, int? idExterno = 0, string cnpj = "", string razao = "", string fantasia = "", string endereco = "", string numero = "", string bairro = "", string cidade = "", string uf = "", string cep = "", string representante = "", string emailRepresentante = "", DateTime? validadeInicial = null, DateTime? validadeFinal = null, bool ativo = true, bool excluido = false, int? usuarioId = 0, string tipoOperacao = "I", int? idContratada = 0)
        {
            bool ret = false;
            if (validadeInicial.HasValue == false) validadeInicial = DateTime.Now;
            if (validadeFinal.HasValue == false) validadeFinal = validadeInicial.Value.AddMonths(6);
            TipoDeOperacao top = TipoDeOperacao.Alterar;
            if (!usuarioId.HasValue) usuarioId = 0;
            string sql = @"UPDATE [TB_Contratada] SET
			                    [CdCnpj]                = @CdCnpj, 			
			                    [DsRazaoSocial]         = @DsRazaoSocial, 			
			                    [DsNomeFantasia]        = @DsNomeFantasia, 
                                [DtAlteracao]           = GETDATE(),
			                    [Endereco]              = @Endereco, 
			                    [NumEndereco]           = @NumEndereco, 
			                    [BairroEndereco]        = @BairroEndereco, 
			                    [Cidade]                = @Cidade, 
			                    [UF]                    = @UF,
			                    [CEP]                   = @CEP, 
			                    [Representante]         = @Representante, 
			                    [EmailRepresentante]    = @EmailRepresentante,
                                [CdAtivo]               = @CdAtivo,
                                [cdExcluido]            = @CdExcluido,
                                [ValidadeDtInicio]      = @ValidadeInicial,
                                [ValidadeDtTermino]     = @ValidadeFinal,
                                [UsuarioId]             = @UsuarioId,
                                [CdOperacao]            = @CdOperacao
                           WHERE
			                    ([IdContratada] = @IdContratada);";
            using (DbHelper db = new DbHelper(ambient, sql))
            {
                db.Add("@CdCnpj", cnpj.JustNumbers().Normalized(), DbType.String);
                db.Add("@DsRazaoSocial", razao.Normalized(), DbType.String);
                db.Add("@DsNomeFantasia", fantasia.Normalized(), DbType.String);
                db.Add("@Representante", representante.Normalized(), DbType.String);
                db.Add("@EmailRepresentante", emailRepresentante.Normalized(StringCases.Lower), DbType.String);
                db.Add("@ValidadeInicial", validadeInicial.Value, DbType.DateTime);
                db.Add("@ValidadeFinal", validadeFinal.Value, DbType.DateTime);
                db.Add("@Endereco", endereco.Normalized(), DbType.String);
                db.Add("@NumEndereco", numero.Normalized(), DbType.String);
                db.Add("@BairroEndereco", bairro.Normalized(), DbType.String);
                db.Add("@Cidade", cidade.Normalized(), DbType.String);
                db.Add("@UF", uf.Normalized(), DbType.String);
                db.Add("@CEP", cep.Normalized(), DbType.String);
                db.Add("@CdAtivo", ativo, DbType.Boolean);
                db.Add("@CdExcluido", excluido, DbType.Boolean);
                db.Add("@CdOperacao", (int)top, DbType.Int32);
                db.Add("@UsuarioId", usuarioId, DbType.Int32);
                db.Add("@IdContratada", idContratada, DbType.Int32);
                ret = (db.Execute(true) > 0);
            }
            return ret;
        }

        /// <summary>
        /// Inclui os dados de uma empresa (tabela TB_Contratada).
        /// </summary>
        /// <param name="ambient">Define a base/catálogo à ser utilizado.</param>
        /// <param name="cnpj">CNPJ da empresa.</param>
        /// <param name="razao">Razão social.</param>
        /// <param name="fantasia">Nome fantasia.</param>
        /// <param name="endereco">Endereço (logradouro).</param>
        /// <param name="numero">Número do logradouro.</param>
        /// <param name="bairro">Bairro do logradouro.</param>
        /// <param name="cidade">Cidade.</param>
        /// <param name="uf">Unidade federativa.</param>
        /// <param name="cep">Código postal.</param>
        /// <param name="representante">Nome completo do representante.</param>
        /// <param name="emailRepresentante">Endereço de email do representante.</param>
        /// <param name="validadeInicial">Data de validade inicial.</param>
        /// <param name="validadeFinal">Data de validade final.</param>
        /// <param name="ativo">Indica se deve ser incluído como ativo (true) ou inativo (false).</param>
        /// <param name="excluido">Indica se deve ser incluído como ativo (true) ou excluído (false).</param>
        /// <returns>Status da operação (true, efetuou a inclusão, ou false, não efetuou a inclusão.)</returns>
        internal static bool IncluiContratada(string ambient, int? idExterno = 0, string cnpj = "", string razao = "", string fantasia = "", string endereco = "", string numero = "", string bairro = "", string cidade = "", string uf = "", string cep = "", string representante = "", string emailRepresentante = "", DateTime? validadeInicial = null, DateTime? validadeFinal = null, bool ativo = true, bool excluido = false, int? usuarioId = 0, string tipoOperacao = "I")
        {
            bool ret = false;
            if (validadeInicial.HasValue == false) validadeInicial = DateTime.Now;
            if (validadeFinal.HasValue == false) validadeFinal = validadeInicial.Value.AddMonths(6);
            if (!usuarioId.HasValue) usuarioId = 0;
            TipoDeOperacao top;
            
            switch (tipoOperacao)
            {
                case "A": top = TipoDeOperacao.Alterar; break;
                case "D": top = TipoDeOperacao.Excluir; break;
                case "R": top = TipoDeOperacao.Ativar; break;
                default: top = TipoDeOperacao.Incluir; break;
            }
            string sql = @"INSERT INTO TB_Contratada (
			                    [CdCnpj], 			
			                    [DsRazaoSocial], 			
			                    [DsNomeFantasia], 
			                    [Endereco], 
			                    [NumEndereco], 
			                    [BairroEndereco], 
			                    [Cidade], 
			                    [UF],
			                    [CEP], 
			                    [Representante], 
			                    [EmailRepresentante],
                                [ValidadeDtInicio],
                                [ValidadeDtTermino],
                                [DtCadastro],
                                [DtAlteracao],
                                [CdAtivo],
                                [CdExcluido],
                                [UsuarioId],
                                [CdOperacao]
		                   ) VALUES (
			                    @CdCnpj, 			
			                    @DsRazaoSocial, 		
			                    @DsNomeFantasia,
			                    @Endereco, 
			                    @NumEndereco, 
			                    @BairroEndereco, 
			                    @Cidade, 
			                    @UF,
			                    @CEP, 
			                    @Representante, 
			                    @EmailRepresentante, 
                                @ValidadeInicial,
                                @ValidadeFinal, 
                                GETDATE(),
                                GETDATE(),
                                @Ativo,
                                @Excluido,
                                @UsuarioId,
                                @CdOperacao);SELECT CAST(SCOPE_IDENTITY() as int) AS [Id]";
            using (DbHelper db = new DbHelper(ambient, sql))
            {
                db.Add("CdCnpj", cnpj.JustNumbers().Normalized(), DbType.String);
                db.Add("DsRazaoSocial", razao.Normalized(), DbType.String);
                db.Add("DsNomeFantasia", fantasia.Normalized(), DbType.String);
                db.Add("Endereco", endereco.Normalized(), DbType.String);
                db.Add("NumEndereco", numero.Normalized(), DbType.String);
                db.Add("BairroEndereco", bairro.Normalized(), DbType.String);
                db.Add("Cidade", cidade.Normalized(), DbType.String);
                db.Add("UF", uf.Normalized(), DbType.String);
                db.Add("CEP", cep.Normalized(), DbType.String);
                db.Add("Representante", representante.Normalized(), DbType.String);
                db.Add("EmailRepresentante", emailRepresentante.Normalized(StringCases.Lower), DbType.String);
                db.Add("ValidadeInicial", validadeInicial.Value, DbType.DateTime);
                db.Add("ValidadeFinal", validadeFinal.Value, DbType.DateTime);
                db.Add("Ativo", ativo, DbType.Boolean);
                db.Add("Excluido", excluido, DbType.Boolean);
                db.Add("@UsuarioId", usuarioId, DbType.Int32);
                db.Add("@CdOperacao", (int)top, DbType.Int32);
                db.Add("Id", null, DbType.Int32, ParameterDirection.Output);
                int temp = db.Insert(true);
                if (temp > 0)
                {
                    db.SqlStatement = @"INSERT INTO [TB_IntegraContratada] (
                                            [IdContratada]
                                            ,[IdIntegra]
                                            ,[IdExterno]
                                            ,[IsAtivo]
                                        ) VALUES (
                                            @IdContratada
                                            ,@IdIntegra
                                            ,@IdExterno
                                            ,@IsAtivo)";
                    db.Clear();
                    db.Add("IdContratada", temp, DbType.Int32);
                    db.Add("IdIntegra", 1, DbType.Int32);
                    db.Add("IdExterno", idExterno.Value, DbType.String);
                    db.Add("IsAtivo", true, DbType.Boolean);
                    ret = db.Execute(true) > 0;
                }
            }
            return ret;
        }
        #endregion

        #region Validar duplicidades de registros
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="numOs"></param>
        /// <returns></returns>
        internal static int ChecaOSDuplicada(string ambient, string numOs)
        {
            int ret = 0;
            try
            {
                string sql = @"SELECT [Id] FROM [TB_MotivacaoTemporaria] WHERE ([OrdemServico] = @OS)";
                using (DbHelper db = new DbHelper(ambient, sql, "@OS", numOs, DbType.String))
                    ret = db.QueryFirstOrDefault<int>();
            }
            catch (Exception Ex) { Ex.Log(); ret = 0; }
            return ret;
        }
        /// <summary>
        /// Verifica se o CNPJ informado já está cadastrado.
        /// </summary>
        /// <param name="ambiente">Indica o servidor dos dados.</param>
        /// <param name="cnpj">CNPJ a ser localizado.</param>
        /// <param name="id">Se informado, utiliza este número como indicador do registro que pode estar com o CNPJ indicado.</param>
        /// <returns></returns>
        internal static bool ChecaCnpjDuplicado(string ambiente, string cnpj, int id = 0, bool externo = false)
        {
            bool ret = false;
            string sql = string.Format(@"SELECT 
                                                A.[IdContratada]	AS [Key],
                                                B.[IdExterno]       AS [Value]
                                         FROM 
	                                            [TB_Contratada]         A INNER JOIN
	                                            [TB_IntegraContratada]  B ON A.[IdContratada] = B.[IdContratada] 
                                         WHERE
	                                            (LTRIM(RTRIM(UPPER([CdCnpj]))) = LTRIM(RTRIM(UPPER(@CdCnpj))))
                                                {0}", (id <= 0) ?
                                                       "" :
                                                       (externo) ?
                                                       "AND (B.[IdExterno] <> @Id)" :
                                                       "AND (A.[IdContratada] <> @Id)");
            using (DbHelper db = new DbHelper(ambiente, sql, "@CdCnpj", cnpj.JustNumbers(), DbType.String))
            {
                if (id > 0)
                    db.Add("@Id", id, DbType.Int32);
                ret = (db.Query<KeyValuePair<int, int>>(true).Count() > 0);
            }
            return ret;
        }
        /// <summary>
        /// Retorna se já existe algum registro de Colaborador contendo o CPF informado. Opcionalmente, 
        /// se informado o parâmetro "idRegistro", retorna se há algum registro de Colaborador com 
        /// o CPF informado e que tenha um identificador diferente desse valor informado.
        /// </summary>
        /// <param name="ambient">Indica se deve utilizar o ambiente de testes ou de produção.</param>
        /// <param name="cpf">Texto com o CPF à ser localizado.</param>
        /// <param name="idRegistro">
        /// Inteiro, opcional. Se informado, desconsiderará registros com esse valor como identificador.
        /// </param>
        /// <returns>Verdadeiro ou falso.</returns>
        public static bool ChecaCpfDuplicado(string ambient, string cpf, int idRegistro = 0)
        {
            bool ret;
            try
            {
                ColaboradorViewModel temp = null;
                string sql = @"SELECT TOP (1) [IdColaborador] FROM [TB_COLABORADOR] WHERE (CdCpf = @cpf)";
                using (DbHelper db = new DbHelper(ambient, sql, "@cpf", cpf, DbType.String))
                    temp = db.QueryFirstOrDefault<ColaboradorViewModel>(true);
                ret = (temp != null);
                if (ret && (idRegistro > 0))
                    ret = (temp.idColaborador != idRegistro);
            }
            catch { ret = false; }
            return ret;
        }
        /// <summary>
        /// A razão social informada já foi cadastrada, com outro CNPJ?
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="razaoSocial"></param>
        /// <returns></returns>
        internal static bool ChecaRazaoDuplicada(string ambient = "", string cnpj = "", int? idExterno = 0, bool externo = true)
        {
            bool ret = false;
            try
            {
                string sql = string.Format(@"SELECT 
                                                    COUNT(A.[CdCnpj]) 
                                             FROM 
                                                    [TB_Contratada] A INNER JOIN 
                                                    [TB_IntegraContratada] B ON (A.[IdContratada] = B.[IdContratada])
		                                     WHERE
			                                        (UPPER(LTRIM(RTRIM(A.[CdCnpj]))) = UPPER(LTRIM(RTRIM(@cnpj))))
			                                        AND NOT ({0} = @Id)", externo ? "B.[IdExterno]" : "A.[IdContratada]");
                using (DbHelper db = new DbHelper(ambient, sql, "@CdCnpj", cnpj, DbType.String))
                {
                    db.Add("Id", idExterno.Value, DbType.Int32);
                    ret = db.QueryFirstOrDefault<int>(true) > 0;
                }
            }
            catch (Exception ex) { ex.Log(); ret = false; }
            return ret;
        }

        internal static string PlacaDaOS(string ambiente, int numOs)
        {
            string ret = "";
            try 
            {
                string sql = @"SELECT Placa FROM TB_MOtivacaoTemporaria WHERE ([OrdemServico] = @OrdemServico)";
                using (DbHelper db = new DbHelper(ambiente, sql, "@OrdemServico", numOs, DbType.String))
                    ret = db.QueryFirstOrDefault<string>(true);
            }
            catch (Exception ex) { ex.Log(); ret = ""; }
            return ret;
        }
        #endregion

        #region Consultar identificadores de registros.
        /// <summary>
        /// Identidade do tipo de crachá com base no texto descritivo.
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private static int? GetIdTipoCracha(string ambient, string tipo = "")
        {
            int? ret = 0;
            try
            {
                string sql = @" SELECT TOP (1) CAST(ISNULL(MAX([IdTipoCracha]),0) AS INT) AS [Id]
                            FROM 
	                            [TB_TipoCracha]
                            WHERE
	                            ([CdAtivo] = 1)
	                            AND (UPPER(LTRIM(RTRIM([DsTipoCracha]))) LIKE UPPER(LTRIM(RTRIM(@Tipo))) OR ISNULL(@Tipo,'')='')";
                using (DbHelper db = new DbHelper(ambient, sql, "@Tipo", tipo, DbType.String))
                    ret = db.QueryFirstOrDefault<int>(true);
            }
            catch (Exception ex) { ex.Log(); ret = 0; }
            return ret;
        }
        /// <summary>
        /// Retorna o identificador de registro que possui a 
        /// descrição de tipo de colaborador definida como "Motorista".
        /// </summary>
        /// <param name="ambient">Define se utiliza o ambiente de teste ou de produção.</param>
        /// <param name="tipo">Filtro com a descrição do tipo de colaborador à ser consultada.</param>
        /// <returns>Se localizado o registro, a identificação do mesmo. Caso contrário, zero.</returns>
        public static int GetIdTipoMotorista(string ambient, string tipo = "Motorista")
        {
            int ret = 0;
            try
            {
                string sql = @"SELECT TOP (1) [IdTipoColaborador] FROM [TB_TipoColaborador] WHERE ([DsTipoColaborador] = @DsTipoColaborador)";
                using (DbHelper db = new DbHelper(ambient, sql, "@DsTipoColaborador", tipo, DbType.String))
                    ret = (db.QueryFirstOrDefault<int>(true));
            }
            catch { ret = 0; }
            return ret;
        }
        /// <summary>
        /// Retorna o identificador de registro que possui a 
        /// descrição de tipo de documento definida como "CPF".
        /// </summary>
        /// <param name="ambient">Define se deve utilizar o ambiente de teste ou de produção.</param>
        /// <param name="tipo">Filtro com a descrição do tipo de documento à ser consultado.</param>
        /// <returns>Se localizado o registro, a identificação do mesmo. Caso contrário, zero.</returns>
        public static int GetIdTipoDocumento(string ambient, string tipo = "CPF")
        {
            int ret = 0;
            if (tipo == "") tipo = "CPF";
            try
            {
                string sql = $"SELECT TOP (1) [IdTipoDocumento] FROM [TB_TipoDocumento] WHERE ([DsTipoDocumento] = N'{tipo}')";
                using (DbHelper db = new DbHelper(ambient, sql))
                    ret = db.QueryFirstOrDefault<int>(true);
            }
            catch { ret = 0; }
            return ret;
        }
        /// <summary>
        /// Retorna a identificação do registro de área indicado.
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        internal static int GetIdArea(string ambient, int area)
        {
            int ret = 0;
            try
            {
                string sql = @"SELECT TOP(1) 
                                    [IdArea] 
                               FROM 
                                    [TB_Area] 
                               WHERE 
                                    ([IdArea] = @IdArea)";
                using (DbHelper db = new DbHelper(ambient, sql, "@IdArea", area, DbType.Int32))
                    ret = db.QueryFirstOrDefault<int>(true);
            }
            catch (Exception ex) { ex.Log(); ret = 0; }
            return ret;
        }
        /// <summary>
        /// Retorna o identificador de registro de crachá que possui a 
        /// chave hexadecimal informada.
        /// </summary>
        /// <param name="ambient">Define se deve utilizar o ambiente de teste ou de produção.</param>
        /// <param name="hexa">Texto com a chave hexadecimal à ser localizada.</param>
        /// <param name="incluiInativos">Informa se a busca deve ou não incluir crachás inativos.</param>
        /// <returns>Se localizado, a identificação do registro de crachá.</returns>
        public static int GetIdCracha(string ambient, string hexa, bool incluiInativos = false)
        {
            int ret = 0;
            try
            {
                string sql = string.Format(@"SELECT TOP (1) 
                                                [idCracha]  
                                             FROM  
                                                [TB_Cracha]  
                                             WHERE  
                                                (UPPER(LTRIM(RTRIM( [CdCracha] ))) = UPPER(LTRIM(RTRIM( @cdCracha ))))").Trim();
                using (DbHelper db = new DbHelper(ambient, sql, "@cdCracha", hexa, DbType.String))
                    ret = db.QueryFirstOrDefault<int>(true);
            }
            catch { ret = 0; }
            return ret;
        }
        /// <summary>
        /// Retorna o identificador de registro de crachá que pertence ao colaborador indicado.
        /// </summary>
        /// <param name="ambient">Define se deve utilizar o ambiente de teste ou de produção.</param>
        /// <param name="idColaborador">Identificador do registro de colaborador.</param>
        /// <param name="incluiInativos">Informa se a busca deve ou não incluir crachás inativos.</param>
        /// <returns>Se localizado, a identificação do registro de crachá.</returns>
        public static int GetIdCracha(string ambient, int idColaborador, bool incluiInativos = true)
        {
            int ret = 0;
            try
            {
                string sql = string.Format(@"SELECT TOP (1) 
                                                [idCracha]  
                                             FROM  
                                                [TB_Cracha]  
                                             WHERE  
                                                ([IdColaborador] = @IdColaborador)  
                                                {0}", (incluiInativos) ? "" : " AND ([CdAtivo] = 1)").Trim();
                using (DbHelper db = new DbHelper(ambient, sql, "@IdColaborador", idColaborador, DbType.Int32))
                    ret = db.QueryFirstOrDefault<int>(true);
            }
            catch { ret = 0; }
            return ret;
        }

        public static string GetCredencial(string ambient, int idColaborador, bool incluiInativos = true)
        {
            string ret = "";
            try
            {
                string sql = string.Format(@"SELECT TOP (1) 
                                                [CdCracha]  
                                             FROM  
                                                [TB_Cracha]  
                                             WHERE  
                                                ([IdColaborador] = @IdColaborador)  
                                                {0}", (incluiInativos) ? "" : " AND ([CdAtivo] = 1)").Trim();
                using (DbHelper db = new DbHelper(ambient, sql, "@IdColaborador", idColaborador, DbType.Int32))
                    ret = db.QueryFirstOrDefault<string>(true);
            }
            catch { ret = ""; }
            return ret;
        }

        /// <summary>
        /// Retorna o identificador de registro de colaborador ativo que possui o identificador de registro externo informado, vinculado ao identificador de contratada indicado e com a identificação de integração informada.
        /// </summary>
        /// <param name="ambient">Define se deve utilizar o ambiente de teste ou de produção.</param>
        /// <param name="idIntegra">Identificação do registro de integração. Esta porra tem sempre o valor "1".</param>
        /// <param name="idContratada">Identificação do registro de contratada que deve estar vinculado ao registro do colaborador.</param>
        /// <param name="idExterno">Identificação do registro do colaborador em outras bases de dados.</param>
        /// <param name="isAtivo">Indica se deve restringir apenas aos ativos ou apenas aos inativos. Não há uma terceira opção.</param>
        /// <returns>Se localizado, a identificação do registro de colaborador.</returns>
        public static int GetIdColaboradorIntegrado(string ambient, int idIntegra, int idContratada, int idExterno, bool isAtivo)
        {
            int ret = 0;
            try
            {
                string sql = @"SELECT [IdColaborador] FROM [TB_IntegraColaborador] WHERE ([IdIntegra] = @idIntegra) AND ([IdContratada] = @IdContratada) AND ([IdExterno] = @IdExterno) AND ([IsAtivo] = @isAtivo)";
                using (DbHelper db = new DbHelper(ambient, sql))
                {
                    db.Add("@idIntegra", idIntegra, DbType.Int32);
                    db.Add("@IdContratada", idContratada, DbType.Int32);
                    db.Add("@IdExterno", idExterno, DbType.Int32);
                    db.Add("@isAtivo", isAtivo, DbType.Boolean);
                    ret = db.QueryFirstOrDefault<int>(true);
                }
            }
            catch { ret = 0; }
            return ret;
        }
        /// <summary>
        /// Retorna o identificador de registro de contratada ativa que possui o identificador de registro externo informado, com a identificação de integração informada.
        /// </summary>
        /// <param name="ambient">Define se deve utilizar o ambiente de teste ou de produção.</param>
        /// <param name="idIntegra">Identificação do registro de integração. Esta porra tem sempre o valor "1".</param>
        /// <param name="idExterno">Identificação do registro do contratada em outras bases de dados.</param>
        /// <param name="isAtivo">Indica se deve restringir apenas aos ativos ou apenas aos inativos. Não há uma terceira opção.</param>
        /// <returns>Se localizado, a identificação do registro de contratada.</returns>
        public static int GetIdContratadaIntegrada(string ambient, int idIntegra, int idExterno, bool isAtivo)
        {
            int ret = 0;
            try
            {

                string sql = @"SELECT [IdContratada] FROM [TB_IntegraContratada] WHERE ([IdIntegra] = @idIntegra) AND ([IdExterno] = @IdExterno)";
                using (DbHelper db = new DbHelper(ambient, sql))
                {
                    db.Add("@idIntegra", idIntegra, DbType.Int32);
                    db.Add("@IdExterno", idExterno, DbType.Int32);
                    ret = db.QueryFirstOrDefault<int>(true);
                }
            }
            catch { ret = 0; }
            return ret;
        }
        #endregion

        #region Consultar ViewModels
        /// <summary>
        /// Retorna um <see cref="ColaboradorViewModel"/> com base no id informado,
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="idColaboradorIntegrador"></param>
        /// <param name="cpf"></param>
        /// <returns></returns>
        internal static IEnumerable<ColaboradorViewModel> GetColaboradorIntegrado(string ambient, int idContratadaIntegrada, int idColaboradorIntegrador, string cpf)
        {
            IEnumerable<ColaboradorViewModel> ret = new List<ColaboradorViewModel>();
            try
            {
                string sql = string.Format(@"SELECT 
                                                ISNULL(A.[IdContratada],0)	AS [IdContratada],
                                                ISNULL(A.[IdExterno],0)		AS [idColaboradorIntegrador],
	                                            ISNULL(C.[IdExterno],0)		AS [idContratadaIntegrador],
                                                ISNULL(A.[IdExterno],0)		AS [IdExterno],
	                                            B.[IdColaborador]           AS idColaborador
                                                ,B.[DsColaborador]          AS nome
                                                ,B.[CdCpf]                  AS cpf
                                                ,B.[CdCnpj]                 AS cnpj
                                                ,B.[IdRegraAcesso]          AS idRegraAcesso
                                                ,B.[IdTipoColaborador]      AS idTipoColaborador
                                                ,B.[UsuarioId]              AS usuarioId
                                                ,B.[DtCadastro]             AS dtCadastro
                                                ,B.[DtAlteracao]            AS dtAlteracao
                                                ,B.[CdOperacao]             AS cdOperacao
                                                ,B.[CdAtivo]                AS cdAtivo
                                                ,B.[DtNascimento]           AS Nascimento
                                                ,B.[DtNascimento]           AS dtNascimento
                                                ,B.[DsGenero]               AS sexo
                                                ,B.[NrNis]                  AS nrNis
                                                ,B.[DtValidadeLib]          AS dtValidadeLib
                                                ,B.[DsObservacao]           AS dsObservacao
                                                ,B.[Endereco]               AS endereco
                                                ,B.[NumEndereco]            AS numEndereco
                                                ,B.[BairroEndereco]         AS bairroEndereco
                                                ,B.[Cidade]                 AS cidade
                                                ,B.[UF]                     AS uf
                                                ,B.[CEP]                    AS CEP
                                                ,B.[Tel]                    AS tel
                                                ,B.[EmailColaborador]       AS emailColaborador
                                                ,B.[Funcao]                 AS funcao
                                                ,B.[ValidadeDtInicio]       AS validadeDtInicio
                                                ,B.[ValidadeDtTermino]      AS validadeDtTermino
                                                ,B.[cdExcluido]             AS cdExcluido
                                                ,B.[OrgaoEmissorCNH]        AS orgaoEmissorCnh
                                                ,B.[CNH]                    AS cnh
                                                ,B.[EmissorCNH]             AS emissorCnh
                                                ,B.[IdTipoDocumento]        AS idTipoDocumento
                                                ,B.[DsDocumento]            AS numDocumento
                                                ,B.[IdPerfilColaborador]    AS idPerfilColaborador
                                            FROM 
	                                            [TB_IntegraColaborador]		A                                               LEFT JOIN
	                                            [TB_Colaborador]			B ON (A.[IdColaborador] = B.[IdColaborador])    LEFT JOIN
	                                            [TB_IntegraContratada]      C ON (A.[IdContratada] = C.[IdContratada])
                                            WHERE
	                                            (A.[IdExterno] = @Id)");
                using (DbHelper db = new DbHelper(ambient, sql, "@Id", idColaboradorIntegrador, DbType.Int32))
                {
                    ret = db.Query<ColaboradorViewModel>(true);
                    if (ret.Count() == 0)
                    {
                        db.SqlStatement = string.Format(@"SELECT 
                                                                (0)							AS [IdContratada],
                                                                (0)							AS [idColaboradorIntegrador],
	                                                            (0)							AS [idContratadaIntegrador],
                                                                (0)							AS [IdExterno],
	                                                            B.[IdColaborador]           AS idColaborador
                                                                ,B.[DsColaborador]          AS nome
                                                                ,B.[CdCpf]                  AS cpf
                                                                ,B.[CdCnpj]                 AS cnpj
                                                                ,B.[IdRegraAcesso]          AS idRegraAcesso
                                                                ,B.[IdTipoColaborador]      AS idTipoColaborador
                                                                ,B.[UsuarioId]              AS usuarioId
                                                                ,B.[DtCadastro]             AS dtCadastro
                                                                ,B.[DtAlteracao]            AS dtAlteracao
                                                                ,B.[CdOperacao]             AS cdOperacao
                                                                ,B.[CdAtivo]                AS cdAtivo
                                                                ,B.[DtNascimento]           AS Nascimento
                                                                ,B.[DtNascimento]           AS dtNascimento
                                                                ,B.[DsGenero]               AS sexo
                                                                ,B.[NrNis]                  AS nrNis
                                                                ,B.[DtValidadeLib]          AS dtValidadeLib
                                                                ,B.[DsObservacao]           AS dsObservacao
                                                                ,B.[Endereco]               AS endereco
                                                                ,B.[NumEndereco]            AS numEndereco
                                                                ,B.[BairroEndereco]         AS bairroEndereco
                                                                ,B.[Cidade]                 AS cidade
                                                                ,B.[UF]                     AS uf
                                                                ,B.[CEP]                    AS CEP
                                                                ,B.[Tel]                    AS tel
                                                                ,B.[EmailColaborador]       AS emailColaborador
                                                                ,B.[Funcao]                 AS funcao
                                                                ,B.[ValidadeDtInicio]       AS validadeDtInicio
                                                                ,B.[ValidadeDtTermino]      AS validadeDtTermino
                                                                ,B.[cdExcluido]             AS cdExcluido
                                                                ,B.[OrgaoEmissorCNH]        AS orgaoEmissorCnh
                                                                ,B.[CNH]                    AS cnh
                                                                ,B.[EmissorCNH]             AS emissorCnh
                                                                ,B.[IdTipoDocumento]        AS idTipoDocumento
                                                                ,B.[DsDocumento]            AS numDocumento
                                                                ,B.[IdPerfilColaborador]    AS idPerfilColaborador
                                                            FROM 
	                                                            [TB_Colaborador]			B
                                                            WHERE
	                                                            ( UPPER( LTRIM( RTRIM( B.[CdCpf] ))) = UPPER( LTRIM( RTRIM( @Cpf ) ) ) )");
                        db.Add("@Cpf", cpf.JustNumbers().Normalized(), DbType.String);
                        ret = db.Query<ColaboradorViewModel>(true);
                    }
                }
            }
            catch (Exception ex) { ex.Log(); ret = new List<ColaboradorViewModel>(); }
            return ret;
        }
        /// <summary>
        /// Retorna um <see cref="EmpresaViewModel"/> com nase no identificador  informado.
        /// </summary>
        /// <param name="ambient">Define se deve utilizar o ambiente de teste ou de produção.</param>
        /// <param name="id">Identificador do registro.</param>
        /// <param name="externo">
        /// Indica se o id informado se refere ao identificador de registro para esta base de dados 
        /// ou para bases de dados de terceiros.
        /// </param>
        /// <returns>Se encontrado, um objeto do tipo <see cref="ContratadaDb"/>.</returns>
        internal static EmpresaViewModel GetContratadaIntegrada(string ambient, int id, bool externo)
        {
            EmpresaViewModel ret = null;
            try
            {
                string sql = string.Format(@"SELECT TOP (1)
	                                            A.[IdContratada]    
	                                            ,[CdCnpj]           AS [CNpj]
	                                            ,[DsRazaoSocial]    AS [razaoSocial]
	                                            ,[DsNomeFantasia]   AS [nomeFantasia]
	                                            ,[UsuarioId]        
	                                            ,[DtCadastro]       
	                                            ,[DtAlteracao]      
	                                            ,[CdOperacao]       
	                                            ,[CdAtivo]          
	                                            ,[Endereco]
	                                            ,[NumEndereco]
	                                            ,[BairroEndereco]
	                                            ,[Cidade]
	                                            ,[UF]
	                                            ,[CEP]
	                                            ,[Representante]
	                                            ,[EmailRepresentante]
	                                            ,[ValidadeDtInicio]
	                                            ,[ValidadeDtTermino]
	                                            ,[cdExcluido]
                                           FROM 
	                                            [TB_Contratada] A INNER JOIN 
	                                            [TB_IntegraContratada] B  ON (A.[IdContratada] = B.[IdContratada])
                                           WHERE
	                                            ({0} = @Id);", externo ? "B.[IdExterno]" : "A.[IdContratada]");
                using (DbHelper db = new DbHelper(ambient, sql, "@Id", id, DbType.Int32))
                    ret = db.QueryFirstOrDefault<EmpresaViewModel>(true);
            }
            catch { ret = null; }
            return ret;
        }
        #endregion

        #region Status de conexão
        /// <summary>
        /// Retorna se o ambiente solicitado pode ser conectado.
        /// </summary>
        /// <param name="ambiente">Texto, somente "P" ou "T".</param>
        /// <returns>Sucesso na conectividade com o servidor de dados.</returns>
        internal static bool CanConnect(string ambiente)
        {
            return DbHelper.CanConnect(ambiente);
        }

        internal static bool EquipamentoExiste(string ambient, string descricaoDoEquipamento)
        {
            bool ret = false;
            try
            {
                string sql = @"SELECT COUNT([DsEquipamento]) FROM [TB_EQUIPAMENTO] WHERE ([DsEquipamento] = @DsEquipamento)";
                using (DbHelper db = new DbHelper(ambient, sql, "@DsEquipamento", descricaoDoEquipamento.Normalized(), DbType.String))
                    ret = (db.QueryFirstOrDefault<int>(true) > 0);
            }
            catch (Exception ex) { ex.Log(); ret = false; }
            return ret;
        }

        internal static IEnumerable<dynamic> ConsultaAcessos(PontoDeControleViewModel equipamento)
        {
            IEnumerable<dynamic> ret = new List<dynamic>();
            try
            {
                // ERA ASSIM:
                //string sql = @"SELECT 
                //                    A.DTACESSO, 
                //                    C.CDCPF AS CPF, 
                //                    C.DSCOLABORADOR AS NOME 
                //               FROM 
                //                    TB_DuploAcesso A INNER JOIN TB_CRACHA B ON (A.IDCREDENCIAL = B.CDCRACHA) INNER JOIN 
                //                    TB_COLABORADOR C ON (B.IDCOLABORADOR = C.IDCOLABORADOR) 
                //               WHERE 
                //                    (CDSENTIDO = 'E')  
                //                    AND IDEQUIPAMENTO IN(
                //                                SELECT   
                //                                    IDEQUIPAMENTO 
                //                                FROM  
                //                                    TB_EQUIPAMENTO  
                //                                WHERE  
                //                                    IDLOCAL = (SELECT 
                //                                                    IDLOCAL  
                //                                               FROM 
                //                                                    TB_EQUIPAMENTO 
                //                                               WHERE  
                //                                                    (DSEQUIPAMENTO = @DsEquipamento)
                //                                                    AND (FlagBalanca = 1)
                //                                              )
                //                                        )
                //                    AND DATEDIFF(HH, A.DTACESSO, getdate()) < 12 
                //               ORDER BY 
                //                 DTACESSO";
                string sql = @"SELECT 
                                 C.[CdCpf]           AS Cpf,
                                 C.[DsColaborador]   AS Nome,
                                 A.[DtAcesso]        AS DtAcesso
                               FROM 
                                 TB_DuploAcesso	A INNER JOIN 
                                 TB_CRACHA		B ON (A.IDCREDENCIAL = B.CDCRACHA) INNER JOIN 
                                 TB_COLABORADOR	C ON (B.IDCOLABORADOR = C.IDCOLABORADOR) INNER JOIN
                                 TB_EQUIPAMENTO  D ON (A.IdEquipamento = D.IdEquipamento)
                               WHERE 
                                 (FlagBalanca = 1)
                                 AND	(CDSENTIDO = 'E') 
                                 AND (DSEQUIPAMENTO = @DsEquipamento)
                                 AND DATEDIFF(HH, A.DTACESSO, getdate()) < 12 
                               ORDER BY 
                                 DTACESSO";
                sql = @"SELECT A.DTACESSO AS ACESSO,C.CDCPF AS CPF,C.DSCOLABORADOR AS NOME
                        FROM
                        (TB_DuploAcesso A INNER JOIN TB_CRACHA B ON A.IDCREDENCIAL = B.CDCRACHA)
                        INNER JOIN TB_COLABORADOR C ON B.IDCOLABORADOR = C.IDCOLABORADOR
                        WHERE CDSENTIDO = 'E' AND IDEQUIPAMENTO IN
                        (SELECT IDEQUIPAMENTO FROM TB_EQUIPAMENTO WHERE IDLOCAL =
                        (SELECT IDLOCAL FROM TB_EQUIPAMENTO WHERE DSEQUIPAMENTO = @DsEquipamento) AND FlagBalanca = 1) 
                        AND DATEDIFF(HH, A.DTACESSO, getdate()) < 12
                        ORDER BY DTACESSO";
                using (DbHelper db = new DbHelper(equipamento.Ambiente, sql, "@DsEquipamento", equipamento.PontoControle.Normalized(), DbType.String))
                    ret = (db.Query<RetornoDaConsultaAcesso>(true));
                if(ret!=null)
                    foreach(RetornoDaConsultaAcesso item in ret)
                        item.DtAcesso = item.Acesso.Value.ToString("HH:mm dd/MM/yyyy");
            }
            catch (Exception ex) { ex.Log(); ret = new List<dynamic>(); }
            return ret;
        }

        internal static bool EntradaNaoAutorizadaDTO(EntradaNaoAutorizadaDTO item)
        {
            bool ret = false;
            try 
            {
                int recs = 0;
                List<int> ids = new List<int>();
                string sql = @"SELECT 
                                    A.[Id]
                               FROM
					                [TB_MotivacaoTemporaria]    A LEFT JOIN
					                [TB_Colaborador]            B ON A.IdColaborador = B.IdColaborador
                               WHERE
					                (A.FlSaida = 0)
					                AND (A.dtCancelamento IS NULL)
					                AND (A.DtSaida IS NULL)
					                AND (    (A.OrdemServico = @numOs)
								          OR (B.CdCpf = @Cpf)
								          OR (A.Placa = @Placa)  
								          OR (A.Placa = @Placa1) ) ";
                using (DbHelper db = new DbHelper(item.Ambiente,sql, "@numOs", item.NumOs, DbType.String))
                {
                    db.Add("@Cpf", item.CPF, DbType.String);
                    db.Add("@Placa", item.PlacaOs, DbType.String);
                    db.Add("@Placa1", item.PlacaOcr, DbType.String);
                    ids.AddRange(db.Query<int>(true));
                    if (ids.Count > 0)
                        foreach (int id in ids)
                        {
                            db.SqlStatement = string.Format(@"UPDATE [TB_MotivacaoTemporaria] SET 
                                                                    [OrdemServico] = 0, 
                                                                    [FlSaida] = 1, 
                                                                    [DtSaida] =  GETDATE()
                                                              WHERE ([Id]={0})", id);
                            recs += db.Execute(true);
                        }
                }
                ret = (recs > 0);
            }
            catch (Exception ex){ ex.Log(); ret = false; }
            return ret;
        }

        internal static List<LiberarBalancaDTO> GetEquipamentoEth03ByBalanca(LiberarBalancaDTO item)
        {
            List<LiberarBalancaDTO> ret = new List<LiberarBalancaDTO>();
            try
            {
                //              IdEquipamento, Ip, NrPorta, UrlBaseService, TipoEth03, LabelLocal, Setido
                string sql = @" SELECT 
	                                ECE.*
                                FROM 
	                                TB_RelacBalancaEthernet3		RLE  JOIN 
	                                TB_Equipamento					E ON RLE.Idbalanca =  E.IdEquipamento JOIN 
	                                TB_EquipamentoConfiguracaoEth03 ECE ON ECE.IdEquipamento =  RLE.IdEthernet3 
                                WHERE 
	                                (E.DsEquipamento = @DsEquipamento)";
                using (DbHelper db = new DbHelper(item.Ambiente, sql, "@DsEquipamento", item.Balanca, DbType.String))
                    ret.AddRange(db.Query<LiberarBalancaDTO>(true));
            }
            catch (Exception ex) { ex.Log(); ret = Activator.CreateInstance<List<LiberarBalancaDTO>>(); }
            return ret;
        }

        internal static List<MotivacaoTemporaria> GetMotivacoesTemporarias(string ambiente, string numOs)
        {
            List<MotivacaoTemporaria> ret = new List<MotivacaoTemporaria>();
            try
            {
                string sql = @" SELECT 
                                    Id, 
                                    IdColaborador, 
                                    IdCracha, 
                                    DtCadastro, 
                                    OrdemServico, 
                                    Placa, 
                                    FlSaida, 
                                    DtValidadeInicial, 
                                    DtValidadeFinal, 
                                    DtSaida, 
                                    dtCancelamento, 
                                    IdContratada, 
                                    Finalizador, 
                                    FlagPermiteSaida  
                                FROM  
                                    TB_MotivacaoTemporaria  
                                WHERE
                                    (OrdemServico = @numOs)  
                                    AND (FlSaida = 0)  
                                    AND (DtSaida IS NULL)  
                                    AND (dtCancelamento IS NULL)";
                using (DbHelper db = new DbHelper(ambiente, sql, "@numOs", numOs, DbType.String))
                    ret.AddRange(db.Query<MotivacaoTemporaria>(true));
            }
            catch (Exception ex) { ex.Log(); ret = Activator.CreateInstance<List<MotivacaoTemporaria>>(); }
            return ret;
        }


        /// <summary>
        /// ???
        /// </summary>
        /// <param name="ambiente"></param>
        /// <param name="idSessao"></param>
        /// <param name="imagem"></param>
        /// <param name="idLapTipoRequisicao"></param>
        /// <param name="nrPosicaoCameraTotem"></param>
        /// <param name="score"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static bool InserirLapRequisicaoScore(string ambiente, int idSessao, byte[] imagem, int idLapTipoRequisicao, int nrPosicaoCameraTotem, float score, int id)
        {
            bool ret = false;
            try
            {
                string sql = @"INSERT INTO [dbo].[TB_LAP_REQUISICAO_SCORE] (
	                                [ID_SECAO] 
	                                ,[ID_LAP_TIPO_REQUISICAO] 
	                                ,[NR_POSICAO_CAMERA_TOTEM]
	                                ,[VL_SCORE] 
	                                ,[ID] 
	                                ,[DT_REQUISICAO_SCORE] 
	                                ,[ST_ANALISADO] 
	                                ,[ST_BIOMETRIA] 
	                                ,[VL_SCORE_OUT] 
	                                ,[IMAGEM]
                               ) VALUES (
	                                @ID_SECAO, 
	                                @ID_LAP_TIPO_REQUISICAO,
	                                @NR_POSICAO_CAMERA_TOTEM, 
	                                @VL_SCORE, 
	                                @ID, 
	                                GETDATE(),
	                                0, 
	                                0, 
	                                0, 
	                                @IMAGEM) ";
                using (DbHelper db = new DbHelper(ambiente, sql, "@ID_SECAO", idSessao, DbType.Int32))
                {
                    db.Add("@ID_LAP_TIPO_REQUISICAO", idLapTipoRequisicao, DbType.Int32);
                    db.Add("@NR_POSICAO_CAMERA_TOTEM", nrPosicaoCameraTotem, DbType.Int32);
                    db.Add("@VL_SCORE", score, DbType.Double);
                    db.Add("@ID", id, DbType.Int32);
                    db.Add("@IMAGEM", imagem, DbType.Binary);
                    ret = (db.Execute() > 0);
                }
            }
            catch (Exception ex) { ex.Log(); ret = false; }
            return ret;
        }

        /// <summary>
        /// Retorna um colaborador usando o id do registro do crachá.
        /// </summary>
        /// <param name="ambiente"></param>
        /// <param name="idCracha"></param>
        /// <returns></returns>
        internal static ColaboradorViewModel ColaboradorByIdCracha(string ambiente, int idCracha)
        {
            ColaboradorViewModel ret = new ColaboradorViewModel();
            try
            {
                string sql = @"SELECT 
	                            A.IdColaborador, 
	                            A.CdCpf,  
	                            B.CdCracha 
                            FROM 
	                            TB_Colaborador A JOIN 
	                            TB_Cracha B on (A.IdColaborador =  B.IdColaborador) 
                            WHERE 
	                            (A.CdAtivo = 1)
	                            AND (A.cdExcluido = 0)
	                            AND (B.CdAtivo = 1)
	                            AND (B.cdExcluido = 0)
	                            AND (B.IdCracha = @IdCracha)";
                using (DbHelper db = new DbHelper(ambiente, sql, "@IdCracha", idCracha, DbType.Int32))
                    ret = db.QueryFirstOrDefault<ColaboradorViewModel>(true);

            }
            catch (Exception ex) { ex.Log(); ret = null; }
            return ret;
        }

        /// <summary>
        /// Retorna a lista de câmeras do local indicado.
        /// </summary>
        /// <param name="ambiente"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        internal static List<CameraEquipamento> ListaDeCamerasDoLocal(string ambiente, string local)
        {
            List<CameraEquipamento> ret = new List<CameraEquipamento>();
            try 
            {
                string sql = @"SELECT  
	                            A.IdEquipamento as IdEquipamentoCamera,
	                            A.IdEquipamentoPai as IdEquipamento,
	                            A.Ip, 
	                            A.Conta,
	                            A.Senha,
	                            A.DsLinkCameraVideo, B.IdEquipamentoTipo
                            FROM 
	                            TB_EquipamentoConfiguracao A JOIN 
	                            TB_Equipamento B ON A.IdEquipamento = B.IdEquipamento 
                            WHERE
	                            A.IdEquipamentoPai =  (SELECT IdEquipamento FROM TB_Equipamento WHERE DsEquipamento = @LOCAL)
	                            AND B.IdEquipamentoTipo IN(4, 8)";
                using (DbHelper db = new DbHelper(ambiente, sql, "@LOCAL", local, DbType.String))
                    ret.AddRange(db.Query<CameraEquipamento>(true));

            }
            catch (Exception ex){ ex.Log(); ret = new List<CameraEquipamento>(); }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="captura"></param>
        /// <returns></returns>
        internal static Captura RecuperaOcr(string ambiente, int id)
        {
            Captura captura = new Captura();
            try
            {
                string sql = @"SELECT * FROM TB_CapturaOCR A INNER JOIN TB_CapturaImagemOCR B ON B.IdOCR = A.IdOCR WHERE A.IdOCR  = @Id";
                using (DbHelper db = new DbHelper(ambiente, sql, "@Id", id, DbType.Int32))
                    captura = db.QueryFirstOrDefault<Captura>(true);

                if(captura == null)
                    captura.StatusCode = 1;
                else
                    captura.StatusCode = 0;
            }
            catch (Exception ex) { ex.Log(); captura.StatusCode = 1; }
            return captura;
        }

        /// <summary>
        /// Recupera a data de cadastro do registro de TB_CapturaOCR indicado.
        /// </summary>
        /// <param name="ambiente"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static DateTime RecuperaDataDaOcr( string ambiente, int id)
        {
            DateTime ret = DateTime.Now;
            try 
            {
                string sql = @"SELECT [DataCadastro] FROM [TB_CapturaOCR] WHERE ([IdOCR] = @Id)";
                using (DbHelper db = new DbHelper(ambiente, sql, "@Id", id, DbType.Int32))
                    ret = db.QueryFirstOrDefault<DateTime>(true);
            }
            catch (Exception ex) { ex.Log(); ret = DateTime.Now; }
            return ret;
        }

        internal static byte[] SelecionaImagemCapturada(string ambiente, int idCapturaImagemOcr, int idOcr)
        {
            byte[] ret = new byte[] { 0 };
            try {
                string sql = @"SELECT [Image] FROM [TB_CapturaImagemOCR] WHERE ([IdCapturaImagemOcr] = @IdCapturaImagemOcr) AND ([IdOcr] = @IdOcr)";
                using (DbHelper db = new DbHelper(ambiente, sql, "@IdCapturaImagemOcr", idCapturaImagemOcr, DbType.Int32))
                {
                    db.Add("@IdOcr", idOcr, DbType.Int32);
                    ret = db.QueryFirstOrDefault<byte[]>(true);
                }
            } catch (Exception ex) { ex.Log(); ret = new byte[] { 0 }; }
            return ret;
        }

        public static Captura ConfirmaOcr(string ambiente, int Id)
        {
            Captura captura = new Captura();
            try 
            {
                string sql = @"SELECT * FROM TB_CapturaOCR A INNER JOIN TB_CapturaImagemOCR B ON B.IdOCR = A.IdOCR WHERE A.IdOCR  = @Id";
                using (DbHelper db = new DbHelper(ambiente, sql, "@Id", Id, DbType.Int32))
                    captura = db.QueryFirstOrDefault<Captura>(true);
                if (captura == null)
                    captura = new Captura() { StatusCode = 1 };
            }
            catch (Exception ex) { ex.Log(); captura = new Captura() { StatusCode = 1 }; }
            return captura;  
        }

        public static Captura ConfirmaOcr(string ambiente, int Id, string placa, int score=0)
        {
            Captura captura = new Captura();
            try
            {
                bool temp = false;
                string sql = @"UPDATE TB_CapturaOCR SET [TextoConfirmado] = @Placa, [Score]=@Score  WHERE ([IdOCR]  = @Id)";
                using (DbHelper db = new DbHelper(ambiente, sql, "@Id", Id, DbType.Int32))
                {
                    db.Add("@Placa", placa, DbType.String);
                    db.Add("@Score", score, DbType.Int32);
                    temp = (db.Execute(true) > 0);
                }
                captura.StatusCode = (temp) ? 0 : 1;
            }
            catch (Exception ex) { ex.Log(); captura = new Captura() { StatusCode = 1 }; }
            return captura;
        }

        internal static int GetIdTipoDoCracha(string ambiente, string numeroCracha, string tipo = "Motorista")
        {
            int ret = 0;
            try
            {
                string sql = @"SELECT A.IdTipoCracha FROM 
                                TB_TipoCracha A INNER JOIN
                                TB_Cracha B ON (A.IdTipoCracha=B.IdTipoCracha)
                                WHERE (B.CdCracha=@CdCracha)";
                using (DbHelper db = new DbHelper(ambiente, sql, "@CdCracha", numeroCracha, DbType.String))
                {
                    ret = db.QueryFirstOrDefault<int>(true);
                    if (ret == 0)
                    {
                        db.Clear();
                        db.SqlStatement = $"SELECT A.[IdTipoCracha] FROM [TB_TipoCracha] A WHERE (A.[DsTipoCracha] = N'{tipo}')";
                        ret = db.QueryFirstOrDefault<int>(true);
                    }
                }
            }
            catch (Exception ex) { ret = 0; ex.Log(); }
            return ret;
        }

        /// <summary>
        /// Retorna o identificador de registro de um perfil de colaborador, sendo o padrão para perfil de motorista.
        /// </summary>
        /// <param name="ambiente">Indica a base de dadso a ser utilizada.</param>
        /// <param name="tipo">Informa o nome do perfil.</param>
        /// <returns>Inteiro, contendo zero ou o identificado do registro.</returns>
        internal static int GetIdPerfilColaborador(string ambiente, string tipo = "Motorista")
        {
            int ret = 0;
            try 
            {
                string sql = $"SELECT [IdPerfilColaborador] FROM [TB_PerfilColaborador] A WHERE A.[Descricao] = '{tipo}' ";
                using (DbHelper db = new DbHelper(ambiente, sql))
                {
                    ret = db.QueryFirstOrDefault<int>(true);
                    if (ret == 0)
                    {
                        db.Clear();
                        db.SqlStatement = $"SELECT A.[IdTipoCracha] FROM [TB_TipoCracha] A WHERE (A.[DsTipoCracha] = N'{tipo}')";
                        ret = db.QueryFirstOrDefault<int>(true);
                    }
                }
            }
            catch (Exception ex) { ex.Log(); ret = 0; }
            return ret;
        }

        internal static bool UpdateMotivacaoTemporaria(int Id, string ambiente, int wIdColaborador, string numOs, string placa, ref int wIdMotivacaoTemporaria)
        {
            bool ret = false;
            try { 
                dynamic temp = null;
                DateTime dti = DateTime.Now;
                DateTime dtf = dti.AddMonths(6);
                string sql = @"UPDATE 
                                TB_MotivacaoTemporaria
                                SET 
                                    IdCracha = @IdCracha, 
                                    DtValidadeInicial = @ValidadeDtInicio, 
                                    DtValidadeFinal = @ValidadeDtTermino, 
                                    Placa = @Placa, 
                                    IdColaborador = @IdColaborador,
                                    OrdemServico = @OrdemServico
                                WHERE
                                    Id = @Id";
                using (DbHelper db = new DbHelper(ambiente, sql, "Id", Id, DbType.Int32))
                {
                    db.Add("Placa", placa, DbType.String);
                    db.Add("ValidadeDtInicio", dti, DbType.DateTime);
                    db.Add("ValidadeDtTermino", dtf, DbType.DateTime);
                    db.Add("IdColaborador", wIdColaborador, DbType.Int32);
                    db.Add("OrdemServico", numOs, DbType.String);
                    temp = (db.Execute(true) > 0);
                }

                if (temp != null)
                {
                    wIdMotivacaoTemporaria = Id;
                    ret = true;
                }
            }
            catch (Exception ex) { ex.Log(); ret = false; }
            return ret;
        }
        #endregion
    }
}















//public static Retorno Add(Colaborador item, string IP)
//{
//    Retorno retorno = new Retorno();

//    int idContratada = 0;
//    int idColaborador = 0;
//    // Validar ambiente:
//    switch (item.Ambiente.HasValidLenght(1, 1, "TP"))
//    {
//        case ValidLenghtResult.IsEmpty:
//            retorno.AddResult(1, "Parâmetro Ambiente obrigatório.");
//            break;
//        case ValidLenghtResult.IsInvalid:
//        case ValidLenghtResult.IsGreater:
//        case ValidLenghtResult.IsLower:
//            retorno.AddResult(2, "Parâmetro Ambiente deve ser preenchido com T ou P.");
//            break;
//        case ValidLenghtResult.IsNull:
//            retorno.AddResult(1, "Parâmetro Ambiente obrigatório.");
//            break;
//    }
//    // Descrição do tipo de documento:
//    if (string.IsNullOrEmpty(item.tipoDocumento))
//        item.tipoDocumento = "";
//    // Número do documento:
//    if (string.IsNullOrEmpty(item.numDocumento))
//        item.numDocumento = "";




//    // Validar contratada.
//    if (item.idEmpresaIntegrador == 0)
//        retorno.AddResult(7, "Parâmetro idEmpresaIntegrador Obrigatório.");
//    else
//        idContratada = IdContratadaIntegrada(item.Ambiente, item.idEmpresaIntegrador);

//    // Validar colaborador.
//    if (item.idColaboradorIntegrador == 0)
//        retorno.AddResult(28, "Parâmetro idColaboradorIntegrador Obrigatório.");
//    else
//        idColaborador = IdColaboradorIntegrada(item.Ambiente, item.idColaboradorIntegrador);


//    if ((!string.IsNullOrEmpty(item.cpf) && (!item.cpf.IsBrCpf()) |
//        ((!item.cpf.IsBrCpf()) && (!string.IsNullOrEmpty(item.numDocumento)) ||
//        !string.IsNullOrEmpty(item.tipoDocumento))))
//    {
//        if (item.tipoDocumento == "" || string.IsNullOrEmpty(item.tipoDocumento))
//        {
//            retorno.AddResult(30, "Parâmetro tipoDocumento Obrigatório."));
//        }
//        else
//        {
//            if (item.tipoDocumento.Length > 10)
//                retorno.AddResult(30, "Parâmetro tipoDocumento deve ser informado com o tamanho máximo de 10 caracteres."));
//            if (!string.IsNullOrEmpty(item.tipoDocumento))
//            {
//                this.sql = string.Format("SELECT IdTipoDocumento from tb_tipoDocumento where DsTipoDocumento = '{0}'", (object)item.tipoDocumento);
//                this.cmd = new SqlCommand(this.sql, this.DtCon);
//                this.dr = this.cmd.ExecuteReader();
//                DataTable dataTable = new DataTable();
//                dataTable.Load((IDataReader)this.dr);
//                this.dr.Close();
//                if (dataTable.Rows.Count != 1)
//                {
//                    retorno.AddResult(31, " Parâmetro tipoDocumento inválido."));
//                }
//                else
//                {
//                    item.tipoDocumento = dataTable.Rows[0]["IdTipoDocumento"].ToString();
//                    if (item.idColaboradorIntegrador.ToString().Trim() != item.cpf.Trim())
//                        retorno.AddResult(500, " Parâmetro CPF deve ser igual ao idColaboradorIntegrador quando tipoDocumento/documento for fornecido."));
//                }
//            }
//        }
//        if (item.numDocumento == null)
//            retorno.AddResult(32, "Parâmetro numDocumento Obrigatório quando o cpf não existir."));
//        else if (item.numDocumento.Length == 0 || item.numDocumento.Length > 20)
//            retorno.AddResult(32, "Parâmetro numDocumento deve ter entre 2 e 20 caracteres."));
//    }
//    else
//    {
//        if (item.cpf.Length > 11)
//            retorno.AddResult(33, "Parâmetro cpf deve ser informado com no máximo de 11 caracteres."));
//        if (item.tipoDocumento.Length > 0)
//        {
//            if (item.tipoDocumento.Length > 10)
//                retorno.AddResult(30, "Parâmetro tipoDocumento deve ser informado com o tamanho máximo de 10 caracteres."));
//            if (item.tipoDocumento.Trim().ToUpper() != "RG" && item.tipoDocumento.Trim().ToUpper() != "RNE")
//                retorno.AddResult(31, " Parâmetro tipoDocumento inválido."));
//        }
//        if (item.numDocumento.Length > 0)
//        {
//            if (item.numDocumento.Length > 20)
//                retorno.AddResult(32, "Parâmetro numDocumento deve ser informado com o tamanho máximo de 20 caracteres."));
//        }
//        else if (!item.cpf.IsBrCpf())
//            retorno.AddResult(33, "Parâmetro cpf inválido."));
//    }
//    if (item.nome == null)
//    {
//        retorno.AddResult(35, "Parâmetro nome não informado."));
//    }
//    else
//    {
//        if (item.nome.Length == 0)
//            retorno.AddResult(35, "Parâmetro nome não informado."));
//        if (item.nome.Length > 50)
//            retorno.AddResult(36, "Parâmetro nome deve ser informado com no máximo 50 caracteres"));
//    }
//    if (item.sexo == null)
//    {
//        retorno.AddResult(37, "Parâmetro sexo não informado."));
//    }
//    else
//    {
//        if (item.sexo.Length == 0)
//            retorno.AddResult(37, "Parâmetro sexo não informado."));
//        if (item.sexo.Length > 1)
//            retorno.AddResult(37, "Parâmetro sexo deve ser informado com no máximo 1 caractere"));
//        if (item.sexo.ToUpper() != "F" && item.sexo.ToUpper() != "M")
//            retorno.AddResult(38, "Parâmetro sexo com valor inválido"));
//    }
//    if (item.dtNascimento != null)
//    {
//        if (!item.dtNascimento.IsDate())
//            retorno.AddResult(39, "Parâmetro dtNascimento com valor inválido"));
//        else
//            item.dtNascimento = DateTime.Parse(item.dtNascimento).ToString("yyyy-MM-dd");
//    }
//    else
//        item.dtNascimento = "";
//    if (item.endereco != null)
//    {
//        if (item.endereco.Length > 150)
//            retorno.AddResult(15, "Parâmetro endereco deve ser informado com no máximo 150 caracteres"));
//    }
//    else
//        item.endereco = "";
//    if (item.numEndereco != null)
//    {
//        if (item.numEndereco.Length > 10)
//            retorno.AddResult(16, "Parâmetro numEndereco deve ser informado com no máximo 10 caracteres"));
//    }
//    else
//        item.numEndereco = "";
//    if (item.bairroEndereco != null)
//    {
//        if (item.bairroEndereco.Length > 50)
//            retorno.AddResult(17, "Parâmetro bairroEndereco deve ser informado com no máximo 50 caracteres"));
//    }
//    else
//        item.bairroEndereco = "";
//    if (item.cidade == null)
//    {
//        retorno.AddResult(18, "Parâmetro cidade obrigatório"));
//    }
//    else
//    {
//        if (item.cidade.Length == 0)
//            retorno.AddResult(18, "Parâmetro cidade obrigatório"));
//        if (item.cidade.Length > 50)
//            retorno.AddResult(19, "Parâmetro cidade deve ser informado com no máximo 50 caracteres"));
//    }
//    if (item.uf == null)
//    {
//        retorno.AddResult(20, "Parâmetro ufMotorista obrigatório"));
//    }
//    else
//    {
//        if (item.uf.Length == 0)
//            retorno.AddResult(20, "Parâmetro ufMotorista obrigatório"));
//        if (item.uf == "")
//            retorno.AddResult(20, "Parâmetro ufMotorista obrigatório"));
//        if (item.uf.Length != 2)
//            retorno.AddResult(21, "Parâmetro ufMotorista deve ser informado com no máximo 2 caracteres"));
//        if (!Regex.IsMatch(item.uf, "^[a-zA-Z]+$"))
//            retorno.AddResult(22, "Parâmetro ufMotorista deve conter somente letras."));
//    }
//    if (item.cep != null)
//    {
//        if (item.cep.Length > 8 || item.cep.Length < 8)
//            retorno.AddResult(23, "Parâmetro cepMotorista deve ser informado com no máximo 8 caracteres."));
//        else if (!item.cep.IsBrZIP())
//            retorno.AddResult(24, "Parâmetro cepMotorista deve conter somente números."));
//    }
//    else
//        item.cep = "";
//    if (item.tel != null)
//    {
//        if (item.tel.Trim().Length > 13)
//            retorno.AddResult(40, "Parâmetro tel deve ser informado com no máximo 13 caracteres (55 11 987654321)."));
//        if ((item.tel.Trim().Length > 0 || item.tel.Trim().Length <= 13) && !item.tel.All<char>((Func<char, bool>)(c =>
//        {
//            if (c >= '0')
//                return c <= '9';
//            return false;
//        })))
//            retorno.AddResult(41, "Parâmetro tel deve conter somente números. (55 11 987654321)"));
//    }
//    else
//        item.tel = "";
//    if (item.emailColaborador != null)
//    {
//        if (item.emailColaborador.Length > 100)
//            retorno.AddResult(43, "Parâmetro emailColaborador deve ser informado com no máximo 100 caracteres."));
//        if (item.emailColaborador.Length > 0 && !item.emailColaborador.IsEmail())
//            retorno.AddResult(44, "Parâmetro emailColaborador inválido."));
//    }
//    else
//        item.emailColaborador = "";
//    if (item.numeroCracha != null)
//    {
//        if ((uint)item.numeroCracha.Length > 0U && item.numeroCracha.Length != 10)
//            retorno.AddResult(45, "Parâmetro numeroCracha deve ser informado com no máximo 10 caracteres."));
//    }
//    else
//        item.numeroCracha = "";
//    if (item.cnh != null)
//    {
//        if ((uint)item.cnh.Length > 0U)
//        {
//            if (item.cnh.Length > 11)
//                retorno.AddResult(46, "Parâmetro cnh deve ser informado com no máximo 11 caracteres."));
//        }
//        else
//            retorno.AddResult(53, "Parâmetro cnh obrigatório."));
//    }
//    else
//        retorno.AddResult(53, "Parâmetro cnh obrigatório."));
//    if (item.orgaoEmissorCnh != null)
//    {
//        if ((uint)item.orgaoEmissorCnh.Length > 0U && item.orgaoEmissorCnh.Length > 50)
//            retorno.AddResult(48, "Parâmetro orgaoEmissorCnh deve ser informado com no máximo 50 caracteres."));
//    }
//    else
//        item.orgaoEmissorCnh = "";
//    if (item.emissorCnh != null)
//    {
//        if ((uint)item.emissorCnh.Length > 0U && item.emissorCnh.Length > 2)
//            retorno.AddResult(47, "Parâmetro emissorCnh deve ser informado com no máximo 2 caracteres."));
//    }
//    else
//        item.emissorCnh = "";
//    if (item.foto == null)
//        retorno.AddResult(54, "Foto não informada.Teste"));
//    if (MensagensList.Count == 0)
//    {
//        int num1 = 0;
//        try
//        {
//            try
//            {
//                int num2 = 1;
//                int num3 = 0;
//                int num4 = 0;
//                string str1 = "";
//                this.cmd = new SqlCommand("select IdContratada from tb_integraContratada where idIntegra = " + (object)num2 + " and IdExterno = " + (object)item.idEmpresaIntegrador + " and isAtivo = 1", this.DtCon);
//                this.dr = this.cmd.ExecuteReader();
//                if (this.dr.Read())
//                    num3 = Convert.ToInt32(this.dr["IdContratada"]);
//                this.dr.Close();
//                if (num2 > 0)
//                {
//                    if (item.tipoOperacao.ToUpper() == "A" || item.tipoOperacao.ToUpper() == "D")
//                    {
//                        this.cmd = new SqlCommand("select IdContratada from tb_integraContratada where idIntegra = " + (object)num2 + " and IdExterno = " + (object)item.idEmpresaIntegrador + " and isAtivo = 1", this.DtCon);
//                        this.dr = this.cmd.ExecuteReader();
//                        if (this.dr.Read())
//                            num3 = Convert.ToInt32(this.dr["IdContratada"]);
//                        this.dr.Close();
//                        this.cmd = new SqlCommand("select IdColaborador from tb_integraColaborador where idIntegra = " + (object)num2 + " and IdContratada = '" + (object)num3 + "' and IdExterno = " + (object)item.idColaboradorIntegrador + " and isAtivo = 1", this.DtCon);
//                        this.dr = this.cmd.ExecuteReader();
//                        if (this.dr.Read())
//                            num4 = Convert.ToInt32(this.dr["IdColaborador"]);
//                        this.dr.Close();
//                    }
//                    if (item.tipoOperacao.ToUpper() == "R")
//                    {
//                        this.cmd = new SqlCommand("select IdContratada from tb_integraContratada where idIntegra = " + (object)num2 + " and IdExterno = " + (object)item.idEmpresaIntegrador + " and isAtivo = 1", this.DtCon);
//                        this.dr = this.cmd.ExecuteReader();
//                        if (this.dr.Read())
//                            num3 = Convert.ToInt32(this.dr["IdContratada"]);
//                        this.dr.Close();
//                        this.cmd = new SqlCommand("select IdColaborador from tb_integraColaborador where idIntegra = " + (object)num2 + " and IdContratada = '" + (object)num3 + "' and IdExterno = " + (object)item.idColaboradorIntegrador + " and isAtivo = 0", this.DtCon);
//                        this.dr = this.cmd.ExecuteReader();
//                        if (this.dr.Read())
//                            num4 = Convert.ToInt32(this.dr["IdColaborador"]);
//                        this.dr.Close();
//                    }
//                    if (item.tipoOperacao.ToUpper() == "I")
//                    {
//                        this.cmd = new SqlCommand("select a.IdContratada from tb_integraColaborador a inner join tb_Colaborador b on a.IdColaborador = b.IdColaborador where a.idExterno = " + (object)item.idColaboradorIntegrador + " and b.cdcpf = '" + item.cpf + "'", this.DtCon);
//                        this.dr = this.cmd.ExecuteReader();
//                        if (!this.dr.Read())
//                            ;
//                        this.dr.Close();
//                    }
//                    this.cmd = new SqlCommand("select cdcnpj from tb_IntegraContratada a inner join Tb_Contratada b on a.IdContratada = b.idContratada where  IdExterno = " + (object)item.idEmpresaIntegrador + " and isAtivo = 1", this.DtCon);
//                    this.dr = this.cmd.ExecuteReader();
//                    if (this.dr.Read())
//                        str1 = this.dr["cdcnpj"].ToString();
//                    this.dr.Close();
//                    if (item.numeroCracha.Length == 10)
//                    {
//                        this.cmd = new SqlCommand("select idCracha from tb_cracha where cdCracha = '" + (object)item.numeroCracha.Length + "'", this.DtCon);
//                        this.dr = this.cmd.ExecuteReader();
//                        if (this.dr.Read())
//                            retorno.AddResult(41, "Parâmetro numeroCracha inválido."));
//                        this.dr.Close();
//                    }
//                    if (item.cpf.IsBrCpf())
//                    {
//                        this.sql = "select IdTipoDocumento from tb_tipoDocumento where dsTipoDocumento =  'CPF'";
//                        this.cmd = new SqlCommand(this.sql, this.DtCon);
//                        this.dr = this.cmd.ExecuteReader();
//                        DataTable dataTable = new DataTable();
//                        dataTable.Load((IDataReader)this.dr);
//                        item.tipoDocumento = dataTable.Rows[0]["IdTipoDocumento"].ToString();
//                        item.numDocumento = item.cpf;
//                        this.dr.Close();
//                    }
//                    if (MensagensList.Count == 0)
//                    {
//                        if (item.tipoOperacao.ToUpper() == "I")
//                        {
//                            try
//                            {
//                                this.sql = "SELECT colab.* FROM TB_Colaborador colab";
//                                this.sql += " join TB_IntegraColaborador intCol on colab.IdColaborador = intCol.IdColaborador";
//                                this.sql += string.Format(" where intCol.IdExterno = {0}", (object)item.idColaboradorIntegrador);
//                                this.cmd = new SqlCommand(this.sql, this.DtCon);
//                                this.dr = this.cmd.ExecuteReader();
//                                DataTable dataTable1 = new DataTable();
//                                dataTable1.Load((IDataReader)this.dr);
//                                this.dr.Close();
//                                if (!string.IsNullOrEmpty(item.cpf) && dataTable1.Rows.Count == 0)
//                                {
//                                    this.sql = string.Format(" SELECT * FROM TB_COLABORADOR WHERE CdCpf = {0}", (object)item.cpf);
//                                    this.cmd = new SqlCommand(this.sql, this.DtCon);
//                                    this.dr = this.cmd.ExecuteReader();
//                                    if (this.dr.HasRows)
//                                    {
//                                        retorno.AddResult(502, "Cpf duplicado"));
//                                        retorno.Result = MensagensList;
//                                        return retorno;
//                                    }
//                                }
//                                this.sql = "select IdTipoColaborador from tb_tipoColaborador where dsTipoColaborador =  'Motorista'";
//                                this.cmd = new SqlCommand(this.sql, this.DtCon);
//                                this.dr = this.cmd.ExecuteReader();
//                                DataTable dataTable2 = new DataTable();
//                                dataTable2.Load((IDataReader)this.dr);
//                                string str2 = dataTable2.Rows[0]["IdTipoColaborador"].ToString();
//                                this.dr.Close();
//                                SqlCommand sqlCommand1 = new SqlCommand();
//                                if (dataTable1.Rows.Count == 0)
//                                {
//                                    SqlCommand sqlCommand2 = new SqlCommand("sp_ColaboradorInserir", this.DtCon);
//                                    sqlCommand2.CommandType = CommandType.StoredProcedure;
//                                    db.Add("DsColaborador", (object)item.nome);
//                                    db.Add("cdCPF", (object)item.cpf);
//                                    db.Add("CdCnpj", (object)str1);
//                                    db.Add("IdTipoColaborador", (object)str2);
//                                    db.Add("UsuarioId", (object)1);
//                                    db.Add("CdOperacao", (object)10);
//                                    db.Add("TipoInstituicao", (object)"CONTRATADA");
//                                    db.Add("DtNascimento", (object)item.dtNascimento);
//                                    db.Add("DsGenero", (object)item.sexo);
//                                    if (item.tipoDocumento.ToUpper().Trim() == "RG")
//                                        db.Add("NrNis", (object)item.numDocumento);
//                                    db.Add("IdTipoDocumento", (object)item.tipoDocumento);
//                                    db.Add("DsDocumento", (object)item.numDocumento.ToUpper());
//                                    db.Add("DsObservacao", (object)"");
//                                    db.Add("Endereco", (object)item.endereco);
//                                    db.Add("NumEndereco", (object)item.numEndereco);
//                                    db.Add("BairroEndereco", (object)item.bairroEndereco);
//                                    db.Add("Cidade", (object)item.cidade);
//                                    db.Add("UF", (object)item.uf);
//                                    db.Add("CEP", (object)item.cep);
//                                    db.Add("TEL", (object)item.tel);
//                                    db.Add("Funcao", (object)"");
//                                    db.Add("EmailColaborador", (object)item.emailColaborador);
//                                    db.Add("Cnh", (object)item.cnh);
//                                    db.Add("OrgaoEmissorCNH", (object)item.orgaoEmissorCnh);
//                                    db.Add("EmissorCNH", (object)item.emissorCnh);
//                                    db.Add("ValidadeDtInicio", (object)"");
//                                    db.Add("ValidadeDtTermino", (object)"");
//                                    SqlParameter sqlParameter1 = sqlCommand2.Parameters.Add("IdRetorno", SqlDbType.Int);
//                                    sqlParameter1.Direction = ParameterDirection.Output;
//                                    SqlParameter sqlParameter2 = sqlCommand2.Parameters.Add("IdInstituicao", SqlDbType.Int);
//                                    sqlParameter2.Direction = ParameterDirection.Output;
//                                    num1 = sqlCommand2.ExecuteNonQuery();
//                                    num4 = Convert.ToInt32(sqlParameter1.Value);
//                                    num3 = Convert.ToInt32(sqlParameter2.Value);
//                                }
//                                else
//                                {
//                                    this.sql = string.Format(" SELECT * FROM TB_COLABORADOR WHERE CdCpf = {0} and idColaborador <> {1}", (object)item.cpf, dataTable1.Rows[0]["IdColaborador"]);
//                                    this.cmd = new SqlCommand(this.sql, this.DtCon);
//                                    this.dr = this.cmd.ExecuteReader();
//                                    if (this.dr.HasRows)
//                                    {
//                                        retorno.AddResult(502, "Cpf duplicado"));
//                                        retorno.Result = MensagensList;
//                                        return retorno;
//                                    }
//                                    string str3 = dataTable1.Rows[0]["IdColaborador"].ToString();
//                                    num4 = Convert.ToInt32(str3);
//                                    SqlCommand sqlCommand2 = new SqlCommand("sp_ColaboradorEditar", this.DtCon);
//                                    sqlCommand2.CommandType = CommandType.StoredProcedure;
//                                    db.Add("idColaborador", (object)str3);
//                                    db.Add("DsColaborador", (object)item.nome);
//                                    db.Add("cdCPF", (object)item.cpf);
//                                    db.Add("CdCnpj", (object)str1);
//                                    db.Add("IdTipoColaborador", (object)str2);
//                                    db.Add("UsuarioId", (object)1);
//                                    db.Add("CdOperacao", (object)10);
//                                    db.Add("CdAtivo", (object)1);
//                                    db.Add("DtNascimento", (object)item.dtNascimento);
//                                    db.Add("DsGenero", (object)item.sexo);
//                                    db.Add("Tel", (object)item.tel);
//                                    db.Add("IdTipoDocumento", (object)item.tipoDocumento);
//                                    db.Add("DsDocumento", (object)item.numDocumento);
//                                    if (item.tipoDocumento.ToUpper().Trim() == "RG")
//                                        db.Add("NrNis", (object)item.numDocumento);
//                                    db.Add("DsObservacao", (object)"");
//                                    db.Add("Endereco", (object)item.endereco);
//                                    db.Add("NumEndereco", (object)item.numEndereco);
//                                    db.Add("BairroEndereco", (object)item.bairroEndereco);
//                                    db.Add("Cidade", (object)item.cidade);
//                                    db.Add("UF", (object)item.uf);
//                                    db.Add("CEP", (object)item.cep);
//                                    db.Add("funcao", (object)"");
//                                    db.Add("EmailColaborador", (object)item.emailColaborador);
//                                    db.Add("Cnh", (object)item.cnh);
//                                    db.Add("OrgaoEmissorCNH", (object)item.orgaoEmissorCnh);
//                                    db.Add("EmissorCNH", (object)item.emissorCnh);
//                                    db.Add("ValidadeDtInicio", (object)"");
//                                    db.Add("ValidadeDtTermino", (object)"");
//                                    num1 = sqlCommand2.ExecuteNonQuery();
//                                    this.sql = " select * from TB_RelacContratadaColaborador";
//                                    this.sql += string.Format("  where idContratada = {0} and idColaborador = {1}", (object)num3, (object)num4);
//                                    this.cmd = new SqlCommand(this.sql, this.DtCon);
//                                    this.dr = this.cmd.ExecuteReader();
//                                    if (!this.dr.Read())
//                                    {
//                                        SqlCommand sqlCommand3 = new SqlCommand("[SP_RelacContratadaColaboradorInserir]", this.DtCon);
//                                        sqlCommand3.CommandType = CommandType.StoredProcedure;
//                                        sqlCommand3.Parameters.AddWithValue("IdContratada", (object)num3);
//                                        sqlCommand3.Parameters.AddWithValue("idColaborador", (object)num4);
//                                        sqlCommand3.Parameters.AddWithValue("UsuarioId", (object)1);
//                                        sqlCommand3.Parameters.AddWithValue("CdOperacao", (object)10);
//                                        sqlCommand3.ExecuteNonQuery();
//                                    }
//                                }
//                                this.sql = string.Format(" select * from TB_IntegraColaborador  ic where ic.IdColaborador ={0}  and ic.IdContratada = {1} and ic.IdExterno = {2}", (object)num4, (object)num3, (object)item.idColaboradorIntegrador);
//                                this.cmd = new SqlCommand(this.sql, this.DtCon);
//                                this.dr = this.cmd.ExecuteReader();
//                                if (!this.dr.Read())
//                                {
//                                    SqlCommand sqlCommand2 = new SqlCommand("[SP_IntegraColaboradorInserir]", this.DtCon);
//                                    sqlCommand2.CommandType = CommandType.StoredProcedure;
//                                    db.Add("idColaborador", (object)num4);
//                                    db.Add("IdContratada", (object)num3);
//                                    db.Add("IdIntegra", (object)1);
//                                    db.Add("IdExterno", (object)item.idColaboradorIntegrador);
//                                    db.Add("Display1", (object)"");
//                                    db.Add("Display2", (object)"");
//                                    sqlCommand2.ExecuteNonQuery();
//                                }
//                                this.dr.Close();
//                                if (item.foto != null)
//                                {
//                                    this.cmd = new SqlCommand(string.Format(" SELECT * FROM [TB_PESSOA_IMAGENS] WHERE IdColaborador = {0}  AND StCracha = 1", (object)num4), this.DtCon);
//                                    this.dr = this.cmd.ExecuteReader();
//                                    if (this.dr.Read())
//                                    {
//                                        this.cmd = new SqlCommand(string.Format("DELETE FROM TB_PESSOA_IMAGENS WHERE IdColaborador = {0}  AND StCracha = 1", (object)num4), this.DtCon);
//                                        this.cmd.ExecuteNonQuery();
//                                    }
//                                    SqlCommand sqlCommand2 = new SqlCommand("SP_ImagemInserir", this.DtCon);
//                                    sqlCommand2.CommandType = CommandType.StoredProcedure;
//                                    db.Add("IdColaborador", (object)num4);
//                                    db.Add("cpf", (object)item.cpf);
//                                    db.Add("img", (object)item.foto);
//                                    num1 = sqlCommand2.ExecuteNonQuery();
//                                }
//                            }
//                            catch (SqlException ex)
//                            {
//                                int errorCode = ex.ErrorCode;
//                                string message = ex.Message;
//                                if (message.Contains("UQ_IdTipoDocumentoDsDocumento"))
//                                    retorno.AddResult(501, "TipoDocumento/Documento duplicados"));
//                                else if (message.Contains("UQ_Cpf"))
//                                    retorno.AddResult(502, "Cpf duplicado"));
//                                else if (message.Contains("UQ_TB_IntegraColaborador_IdColaboradorIdcontratada"))
//                                    retorno.AddResult(503, "Motorista já cadastrado para esta contratada."));
//                                throw;
//                            }
//                            catch (Exception ex)
//                            {
//                                if (MensagensList.Count == 0)
//                                    retorno.AddResult(99, "Erro não especificado" + ex.Message));
//                            }
//                        }
//                        if (item.tipoOperacao.ToUpper() == "A")
//                        {
//                            try
//                            {
//                                this.cmd = new SqlCommand("select IdColaborador, CdAtivo from tb_Colaborador where (CdCPF = '" + item.cpf + "' or NrNis = '" + item.numDocumento.ToUpper() + "')", this.DtCon);
//                                this.dr = this.cmd.ExecuteReader();
//                                if (this.dr.Read())
//                                {
//                                    SqlCommand sqlCommand1 = new SqlCommand("sp_ColaboradorEditar", this.DtCon);
//                                    sqlCommand1.CommandType = CommandType.StoredProcedure;
//                                    sqlCommand1.Parameters.AddWithValue("idColaborador", (object)this.dr["idColaborador"].ToString());
//                                    sqlCommand1.Parameters.AddWithValue("DsColaborador", (object)item.nome);
//                                    sqlCommand1.Parameters.AddWithValue("cdCpf", (object)item.cpf);
//                                    sqlCommand1.Parameters.AddWithValue("CdCnpj", (object)str1);
//                                    sqlCommand1.Parameters.AddWithValue("IdTipoColaborador", (object)1212);
//                                    sqlCommand1.Parameters.AddWithValue("UsuarioId", (object)1);
//                                    sqlCommand1.Parameters.AddWithValue("CdOperacao", (object)10);
//                                    sqlCommand1.Parameters.AddWithValue("CdAtivo", (object)1);
//                                    sqlCommand1.Parameters.AddWithValue("DtNascimento", (object)item.dtNascimento);
//                                    sqlCommand1.Parameters.AddWithValue("DsGenero", (object)item.sexo);
//                                    sqlCommand1.Parameters.AddWithValue("Tel", (object)item.tel);
//                                    sqlCommand1.Parameters.AddWithValue("IdTipoDocumento", (object)item.tipoDocumento);
//                                    sqlCommand1.Parameters.AddWithValue("DsDocumento", (object)item.numDocumento.ToUpper());
//                                    sqlCommand1.Parameters.AddWithValue("DsObservacao", (object)"");
//                                    sqlCommand1.Parameters.AddWithValue("Endereco", (object)item.endereco);
//                                    sqlCommand1.Parameters.AddWithValue("NumEndereco", (object)item.numEndereco);
//                                    sqlCommand1.Parameters.AddWithValue("BairroEndereco", (object)item.bairroEndereco);
//                                    sqlCommand1.Parameters.AddWithValue("Cidade", (object)item.cidade);
//                                    sqlCommand1.Parameters.AddWithValue("UF", (object)item.uf);
//                                    sqlCommand1.Parameters.AddWithValue("CEP", (object)item.cep);
//                                    sqlCommand1.Parameters.AddWithValue("Funcao", (object)"");
//                                    sqlCommand1.Parameters.AddWithValue("EmailColaborador", (object)item.emailColaborador);
//                                    sqlCommand1.Parameters.AddWithValue("Cnh", (object)item.cnh);
//                                    sqlCommand1.Parameters.AddWithValue("OrgaoEmissorCNH", (object)item.orgaoEmissorCnh);
//                                    sqlCommand1.Parameters.AddWithValue("EmissorCNH", (object)item.emissorCnh);
//                                    sqlCommand1.Parameters.AddWithValue("ValidadeDtInicio", (object)"");
//                                    sqlCommand1.Parameters.AddWithValue("ValidadeDtTermino", (object)"");
//                                    if ((uint)sqlCommand1.ExecuteNonQuery() > 0U)
//                                    {
//                                        if (item.numeroCracha.Length == 10)
//                                        {
//                                            SqlDataReader sqlDataReader = new SqlCommand("select IdCracha , CdCracha from tb_Cracha where IdColaborador = " + this.dr["idColaborador"].ToString(), this.DtCon).ExecuteReader();
//                                            if (!sqlDataReader.Read())
//                                            {
//                                                SqlCommand sqlCommand2 = new SqlCommand("sp_CrachaInserir", this.DtCon);
//                                                sqlCommand2.CommandType = CommandType.StoredProcedure;
//                                                db.Add("CdCracha", (object)item.numeroCracha);
//                                                db.Add("CdCnpj", (object)str1);
//                                                db.Add("IdColaborador", (object)num4);
//                                                db.Add("UsuarioId", (object)1);
//                                                db.Add("CdOperacao", (object)10);
//                                                db.Add("IdTipoCracha", (object)1);
//                                                int num5 = 0;
//                                                num5 = sqlCommand2.ExecuteNonQuery();
//                                            }
//                                            else if (!new SqlCommand("select IdCracha , CdCracha from tb_Cracha where IdColaborador = " + this.dr["idColaborador"].ToString() + " and cdCracha = '" + item.numeroCracha + "'", this.DtCon).ExecuteReader().Read())
//                                            {
//                                                SqlCommand command = this.DtCon.CreateCommand();
//                                                command.CommandText = "update tb_cracha set cdAtivo=0 where IdColaborador =" + this.dr["idColaborador"].ToString();
//                                                command.ExecuteNonQuery();
//                                                SqlCommand sqlCommand2 = new SqlCommand("sp_CrachaInserir", this.DtCon);
//                                                sqlCommand2.CommandType = CommandType.StoredProcedure;
//                                                db.Add("CdCracha", (object)item.numeroCracha);
//                                                db.Add("CdCnpj", (object)str1);
//                                                db.Add("IdColaborador", (object)num4);
//                                                db.Add("UsuarioId", (object)1);
//                                                db.Add("CdOperacao", (object)10);
//                                                db.Add("IdTipoCracha", (object)1);
//                                                int num5 = 0;
//                                                num5 = sqlCommand2.ExecuteNonQuery();
//                                            }
//                                            sqlDataReader.Close();
//                                        }
//                                    }
//                                    else
//                                    {
//                                        retorno.AddResult(99, "Erro não especificado."));
//                                        retorno.StatusCode = 1;
//                                        retorno.StatusMessage = "Error";
//                                    }
//                                }
//                                else
//                                {
//                                    retorno.AddResult(99, "Erro não especificado."));
//                                    retorno.StatusCode = 1;
//                                    retorno.StatusMessage = "Error";
//                                }
//                                this.dr.Close();
//                            }
//                            catch (Exception ex)
//                            {
//                                retorno.AddResult(99, "Erro não especificado" + ex.Message));
//                                retorno.StatusCode = 1;
//                                retorno.StatusMessage = "Error";
//                            }
//                        }
//                        if (item.tipoOperacao.ToUpper() == "D")
//                        {
//                            try
//                            {
//                                SqlCommand sqlCommand = new SqlCommand("sp_IntegraColaboradorDesativar", this.DtCon);
//                                sqlCommand.CommandType = CommandType.StoredProcedure;
//                                sqlCommand.Parameters.AddWithValue("IdColaborador", (object)num4);
//                                sqlCommand.Parameters.AddWithValue("Display1", (object)"");
//                                sqlCommand.Parameters.AddWithValue("Display2", (object)"");
//                                if ((uint)sqlCommand.ExecuteNonQuery() <= 0U)
//                                    ;
//                            }
//                            catch (Exception ex)
//                            {
//                                retorno.AddResult(99, "Erro não especificado" + ex.Message));
//                            }
//                        }
//                        if (item.tipoOperacao.ToUpper() == "R")
//                        {
//                            try
//                            {
//                                SqlCommand sqlCommand = new SqlCommand("sp_IntegraColaboradorAtivar", this.DtCon);
//                                sqlCommand.CommandType = CommandType.StoredProcedure;
//                                sqlCommand.Parameters.AddWithValue("IdColaborador", (object)num4);
//                                sqlCommand.Parameters.AddWithValue("Display1", (object)"");
//                                sqlCommand.Parameters.AddWithValue("Display2", (object)"");
//                                if ((uint)sqlCommand.ExecuteNonQuery() <= 0U) { };
//                            }
//                            catch (Exception ex)
//                            {
//                                retorno.AddResult(99, "Erro não especificado" + ex.Message));
//                            }
//                        }
//                    }
//                }
//                this.DtCon.Close();
//            }
//            catch (Exception ex)
//            {
//                if (MensagensList.Count == 0)
//                    retorno.AddResult(99, "Erro não especificado" + ex.Message));
//            }
//        }
//        catch (Exception ex)
//        {
//            retorno.AddResult(98, "Sem conexão com a base de dados"));
//        }
//    }
//    else
//    {
//        retorno.StatusCode = 1;
//        retorno.StatusMessage = "Error";
//    }
//    retorno.Result = MensagensList;
//    return retorno;
//}