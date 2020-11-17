using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBAPI_VOPAK.DB.SqlQueries
{

	public static class TB_Colaborador
	{
		public static string sqRec = @"UPDATE [TB_Colaborador] SET [cdExcluido] = 0 WHERE ([IdColaborador] = @IdColaborador)";
		public static string sqDel = @"UPDATE [TB_Colaborador] SET [cdExcluido] = 1 WHERE ([IdColaborador] = @IdColaborador)";
		public static string sqRea = @"UPDATE [TB_Colaborador] SET [CdAtivo] = 1, [cdExcluido] = 0 WHERE ([IdColaborador] = @IdColaborador)";
		public static string sqDea = @"UPDATE [TB_Colaborador] SET [CdAtivo] = 0, [cdExcluido] = 1 WHERE ([IdColaborador] = @IdColaborador)";
		public static string sqLis = @"
										SELECT 
											[IdColaborador]
											,[DsColaborador]
											,[CdCpf]
											,[CdCnpj]
											,[IdRegraAcesso]
											,[IdTipoColaborador]
											,[UsuarioId]
											,[DtCadastro]
											,[DtAlteracao]
											,[CdOperacao]
											,[CdAtivo]
											,[DtNascimento]
											,[DsGenero]
											,[NrNis]
											,[DtValidadeLib]
											,[DsObservacao]
											,[Endereco]
											,[NumEndereco]
											,[BairroEndereco]
											,[Cidade]
											,[UF]
											,[CEP]
											,[Tel]
											,[EmailColaborador]
											,[Funcao]
											,[ValidadeDtInicio]
											,[ValidadeDtTermino]
											,[cdExcluido]
											,[OrgaoEmissorCNH]
											,[CNH]
											,[EmissorCNH]
											,[IdTipoDocumento]
											,[DsDocumento]
											,[IdPerfilColaborador]
										FROM 
											[TB_Colaborador]
										ORDER BY
											[IdColaborador] DESC";
		public static string sqGet = @"
										SELECT 
											[IdColaborador]
											,[DsColaborador]
											,[CdCpf]
											,[CdCnpj]
											,[IdRegraAcesso]
											,[IdTipoColaborador]
											,[UsuarioId]
											,[DtCadastro]
											,[DtAlteracao]
											,[CdOperacao]
											,[CdAtivo]
											,[DtNascimento]
											,[DsGenero]
											,[NrNis]
											,[DtValidadeLib]
											,[DsObservacao]
											,[Endereco]
											,[NumEndereco]
											,[BairroEndereco]
											,[Cidade]
											,[UF]
											,[CEP]
											,[Tel]
											,[EmailColaborador]
											,[Funcao]
											,[ValidadeDtInicio]
											,[ValidadeDtTermino]
											,[cdExcluido]
											,[OrgaoEmissorCNH]
											,[CNH]
											,[EmissorCNH]
											,[IdTipoDocumento]
											,[DsDocumento]
											,[IdPerfilColaborador]
										FROM 
											[TB_Colaborador]
										WHERE 
											([IdColaborador] = @IdColaborador)
										ORDER BY
											[IdColaborador] DESC";
	}
    public static class TB_Contratada
    {
		public static string sqRec = @"UPDATE [TB_Contratada] SET [cdExcluido] = 0 WHERE ([IdContratada] = @IdContratada)";
		public static string sqDel = @"UPDATE [TB_Contratada] SET [cdExcluido] = 1 WHERE ([IdContratada] = @IdContratada)";
		public static string sqRea = @"UPDATE [TB_Contratada] SET [CdAtivo] = 1, [cdExcluido] = 0 WHERE ([IdContratada] = @IdContratada)";
		public static string sqDea = @"UPDATE [TB_Contratada] SET [CdAtivo] = 0, [cdExcluido] = 1 WHERE ([IdContratada] = @IdContratada)";
		public static string sqLis = @"
										SELECT 
											[IdContratada]
											,[CdCnpj]
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
										FROM 
											[TB_Contratada]
										ORDER BY
											[IdContratada] DESC";
		public static string sqGet = @"
										SELECT 
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
										FROM 
											[TB_Contratada]
										WHERE
											([IdContratada] = @IdContratada)
										ORDER BY
											[IdContratada] DESC";
		public static string sqFil = @"
										SELECT 
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
										FROM 
											[TB_Contratada]
										WHERE
											({0});
										ORDER BY
											[IdContratada] DESC";

		public static string sqIns = @"
										INSERT INTO [TB_Contratada] (
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
										) VALUES (
											@CdCnpj					--varchar(14),>
											,@DsRazaoSocial			--varchar(50),>
											,@DsNomeFantasia		--varchar(50),>
											,@UsuarioId				--int,>
											,@DtCadastro			--datetime,>
											,@DtAlteracao			--datetime,>
											,@CdOperacao			--int,>
											,@CdAtivo				--bit,>
											,@Endereco				--varchar(200),>
											,@NumEndereco			--nchar(10),>
											,@BairroEndereco		--varchar(100),>
											,@Cidade				--varchar(50),>
											,@UF					--nchar(2),>
											,@CEP					--nchar(8),>
											,@Representante			--varchar(100),>
											,@EmailRepresentante	--varchar(100),>
											,@ValidadeDtInicio		--date,>
											,@ValidadeDtTermino		--date,>
											,@cdExcluido			--bit,>)";
		public static string sqUpd = @" 
										UPDATE	[TB_Contratada]	SET
											[CdCnpj]				= @CdCnpj				--varchar(14),>
											,[DsRazaoSocial]		= @DsRazaoSocial		--varchar(50),>
											,[DsNomeFantasia]		= @DsNomeFantasia		--varchar(50),>
											,[UsuarioId]			= @UsuarioId			--int,>
											,[DtCadastro]			= @DtCadastro			--datetime,>
											,[DtAlteracao]			= @DtAlteracao			--datetime,>
											,[CdOperacao]			= @CdOperacao			--int,>
											,[CdAtivo]				= @CdAtivo				--bit,>
											,[Endereco]				= @Endereco				--varchar(200),>
											,[NumEndereco]			= @NumEndereco			--nchar(10),>
											,[BairroEndereco]		= @BairroEndereco		--varchar(100),>
											,[Cidade]				= @Cidade				--varchar(50),>
											,[UF]					= @UF					--nchar(2),>
											,[CEP]					= @CEP					--nchar(8),>
											,[Representante]		= @Representante		--varchar(100),>
											,[EmailRepresentante]	= @EmailRepresentante	--varchar(100),>
											,[ValidadeDtInicio]		= @ValidadeDtInicio		--date,>
											,[ValidadeDtTermino]	= @ValidadeDtTermino,	--date,>
											,[cdExcluido]			= @cdExcluido			--bit,>
										WHERE 
											([IdContratada] = @IdContratada)";

	}
}