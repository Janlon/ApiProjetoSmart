namespace WEBAPI_VOPAK
{
    using WEBAPI_VOPAK.Models;
    using System;

    internal static class EmpresaRepositorio
    {
        /// <summary>
        /// Efetua a inclusão ou alteração da empresa informada e retorna um <see cref="Retorno"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="idContratada">Integer, passado por referência, retorna o identificado do registro que foi processado, ou zero.</param>
        /// <returns></returns>
        public static Retorno Add(EmpresaViewModel item, ref int idContratada)
        {
            Retorno ret = new Retorno();
            idContratada = 0;
            if (ret.Ok())
            {
                DateTime? dInicio = DateTime.Now;
                DateTime? dFim = dInicio.Value.AddMonths(6);
                EmpresaViewModel contratada = Dados.GetContratadaIntegrada(item.ambiente, item.idEmpresaIntegrador.Value, true);
                bool gravou;
                if (contratada == null)
                {
                    if (!string.IsNullOrWhiteSpace(item.dtFimValidade))
                        dFim = Convert.ToDateTime(item.dtFimValidade);
                    if (!string.IsNullOrWhiteSpace(item.dtInicioValidade))
                        dInicio = Convert.ToDateTime(item.dtInicioValidade);
                    if (string.IsNullOrWhiteSpace(item.dtInicioValidade))
                        item.dtInicioValidade = dInicio.Value.ToString();
                    if (string.IsNullOrWhiteSpace(item.dtFimValidade))
                        item.dtFimValidade = dFim.Value.ToString();
                    //-------------------------------------------------------------
                    // TO-DO
                    //-------------------------------------------------------------
                    //if (dFim < dInicio)
                    //{
                    //    ret.AddResult(ErroSmartApi.ValidadeInicialPosterior);
                    //    return ret;
                    //}
                    gravou = Dados.IncluiContratada(
                        item.ambiente, item.idEmpresaIntegrador,
                        item.cnpj, item.razaoSocial, item.nomeFantasia,
                        item.endereco, item.numEndereco, item.bairroEndereco, item.cidade, item.uf, item.cep,
                        item.representante, item.emailRepresentante,
                        Convert.ToDateTime(item.dtInicioValidade),
                        Convert.ToDateTime(item.dtFimValidade), true, false, null, item.tipoOperacao);

                    if (gravou)
                        contratada = Dados.GetContratadaIntegrada(item.ambiente, item.idEmpresaIntegrador.Value, true);

                    if (contratada != null)
                        idContratada = contratada.IdContratada.Value;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(item.dtFimValidade))
                        dFim = Convert.ToDateTime(item.dtFimValidade);
                    else
                        dFim = contratada.ValidadeDtTermino;
                    if (!string.IsNullOrWhiteSpace(item.dtInicioValidade))
                        dInicio = Convert.ToDateTime(item.dtInicioValidade);
                    else
                        dInicio = contratada.ValidadeDtInicio;
                    if (string.IsNullOrWhiteSpace(item.dtInicioValidade))
                        item.dtInicioValidade = dInicio.Value.ToString();
                    if (string.IsNullOrWhiteSpace(item.dtFimValidade))
                        item.dtFimValidade = dFim.Value.ToString();
                    //-------------------------------------------------------------
                    // TO-DO
                    //-------------------------------------------------------------
                    //if (dFim < dInicio)
                    //{
                    //    ret.AddResult(ErroSmartApi.ValidadeInicialPosterior);
                    //    return ret;
                    //}
                    bool ativo, excluido;
                    switch (item.tipoOperacao.ToUpper())
                    {
                        // Alteração: Usar como está gravado.
                        case "A":
                            ativo = contratada.cdAtivo.Value;
                            excluido = contratada.cdExcluido.Value;
                            break;
                        // Exclusão: Mudar os flags.
                        case "D":
                            excluido = true;
                            ativo = false;
                            break;
                        // Inclusão e restauração: sempre ativo.
                        default:
                            ativo = true;
                            excluido = false;
                            break;
                    }
                    gravou = Dados.AlteraContratada(item.ambiente, item.idEmpresaIntegrador,
                        item.cnpj, item.razaoSocial, item.nomeFantasia,
                        item.endereco, item.numEndereco, item.bairroEndereco, item.cidade, item.uf, item.cep,
                        item.representante, item.emailRepresentante,
                        dInicio, dFim, ativo, excluido, item.usuarioId, item.tipoOperacao, contratada.IdContratada);
                    if (gravou)
                        contratada = Dados.GetContratadaIntegrada(item.ambiente, item.idEmpresaIntegrador.Value, true);
                    if (contratada != null)
                        idContratada = contratada.IdContratada.Value;
                }
            }
            return ret;
        }
    }
}


//    SqlConnection connection = new SqlConnection();
//    int num1 = 0;
//    if (item.Ambiente.ToUpper() == "P")
//        connection.ConnectionString = ConfigurationManager.AppSettings["BANCO_PRODUCAO"];
//    else
//        connection.ConnectionString = ConfigurationManager.AppSettings["BANCO_TESTE"];
//    try
//    {
//        connection.Open();
//        try
//        {
//            int num2 = 0;
//            SqlCommand sqlCommand1 = new SqlCommand();
//            if (item.tipoOperacao.ToUpper() == "A" || item.tipoOperacao.ToUpper() == "D")
//            {
//                SqlDataReader sqlDataReader = new SqlCommand("select IdContratada from tb_integraContratada where IdExterno = " + (object)item.idEmpresaIntegrador + " and isAtivo = 1", connection).ExecuteReader();
//                if (!sqlDataReader.Read())
//                    ret.AddResult(8, "Parâmetro idEmpresaIntegrador não encontrado.");
//                else
//                    num2 = Convert.ToInt32(sqlDataReader["IdContratada"]);
//                sqlDataReader.Close();
//            }
//            if (item.tipoOperacao.ToUpper() == "R")
//            {
//                SqlDataReader sqlDataReader = new SqlCommand("select IdContratada from tb_integraContratada where IdExterno = " + (object)item.idEmpresaIntegrador + " and isAtivo = 0", connection).ExecuteReader();
//                if (!sqlDataReader.Read())
//                    ret.AddResult(8, "Parâmetro idEmpresaIntegrador não encontrado.");
//                else
//                    num2 = Convert.ToInt32(sqlDataReader["IdContratada"]);
//                sqlDataReader.Close();
//            }
//            if (item.tipoOperacao.ToUpper() == "I")
//            {
//                SqlDataReader sqlDataReader = new SqlCommand("select IdContratada from tb_integraContratada where  IdContratada = " + (object)item.idEmpresaIntegrador, connection).ExecuteReader();
//                if (!sqlDataReader.Read())
//                    ;
//                sqlDataReader.Close();
//            }
//            if (MensagensList.Count == 0)
//            {
//                if (item.tipoOperacao.ToUpper() == "I")
//                {
//                    try
//                    {
//                        SqlDataReader sqlDataReader = new SqlCommand(" select ct.IdContratada, ct.CdAtivo from tb_Contratada ct" + " join TB_IntegraContratada ic on ct.IdContratada = ic.IdContratada" + string.Format(" where ct.CdCnpj = '{0}' and ic.IdExterno = {1}", (object)item.cnpj, (object)item.idEmpresaIntegrador), connection).ExecuteReader();
//                        int int32;
//                        if (!sqlDataReader.Read())
//                        {
//                            SqlCommand sqlCommand2 = new SqlCommand("sp_ContratadaInserir", connection);
//                            sqlCommand2.CommandType = CommandType.StoredProcedure;
//                            sqlCommand2.Parameters.AddWithValue("CdCnpj", (object)item.cnpj);
//                            sqlCommand2.Parameters.AddWithValue("DsRazaoSocial", (object)item.razaoSocial);
//                            sqlCommand2.Parameters.AddWithValue("DsNomeFantasia", (object)item.nomeFantasia);
//                            sqlCommand2.Parameters.AddWithValue("CdOperacao", (object)10);
//                            sqlCommand2.Parameters.AddWithValue("UsuarioId", (object)1);
//                            sqlCommand2.Parameters.AddWithValue("Endereco", (object)item.endereco);
//                            sqlCommand2.Parameters.AddWithValue("NumEndereco", (object)"");
//                            sqlCommand2.Parameters.AddWithValue("BairroEndereco", (object)item.bairroEndereco);
//                            sqlCommand2.Parameters.AddWithValue("Cidade", (object)item.cidade);
//                            sqlCommand2.Parameters.AddWithValue("UF", (object)item.uf);
//                            sqlCommand2.Parameters.AddWithValue("CEP", (object)item.cep);
//                            sqlCommand2.Parameters.AddWithValue("Representante", (object)item.representante);
//                            sqlCommand2.Parameters.AddWithValue("EmailRepresentante", (object)item.emailRepresentante);
//                            sqlCommand2.Parameters.AddWithValue("ValidadeDtInicio", (object)"");
//                            sqlCommand2.Parameters.AddWithValue("ValidadeDtTermino", (object)"");
//                            SqlParameter sqlParameter = sqlCommand2.Parameters.Add("IdRetorno", SqlDbType.Int);
//                            sqlParameter.Direction = ParameterDirection.Output;
//                            num1 = sqlCommand2.ExecuteNonQuery();
//                            int32 = Convert.ToInt32(sqlParameter.Value);
//                        }
//                        else
//                        {
//                            SqlCommand sqlCommand2 = new SqlCommand("sp_ContratadaEditar", connection);
//                            sqlCommand2.CommandType = CommandType.StoredProcedure;
//                            sqlCommand2.Parameters.AddWithValue("IdContratada", sqlDataReader["IdContratada"]);
//                            sqlCommand2.Parameters.AddWithValue("CdCnpj", (object)item.cnpj);
//                            sqlCommand2.Parameters.AddWithValue("DsRazaoSocial", (object)item.razaoSocial);
//                            sqlCommand2.Parameters.AddWithValue("DsNomeFantasia", (object)item.nomeFantasia);
//                            sqlCommand2.Parameters.AddWithValue("UsuarioId", (object)1);
//                            sqlCommand2.Parameters.AddWithValue("CdOperacao", (object)10);
//                            sqlCommand2.Parameters.AddWithValue("CdAtivo", (object)1);
//                            sqlCommand2.Parameters.AddWithValue("Endereco", (object)item.endereco.ToString();
//                            sqlCommand2.Parameters.AddWithValue("NumEndereco", (object)item.numEndereco);
//                            sqlCommand2.Parameters.AddWithValue("BairroEndereco", (object)item.bairroEndereco);
//                            sqlCommand2.Parameters.AddWithValue("Cidade", (object)item.cidade);
//                            sqlCommand2.Parameters.AddWithValue("UF", (object)item.uf);
//                            sqlCommand2.Parameters.AddWithValue("CEP", (object)item.cep);
//                            sqlCommand2.Parameters.AddWithValue("Representante", (object)item.representante);
//                            sqlCommand2.Parameters.AddWithValue("EmailRepresentante", (object)item.emailRepresentante);
//                            sqlCommand2.Parameters.AddWithValue("ValidadeDtInicio", (object)"");
//                            sqlCommand2.Parameters.AddWithValue("ValidadeDtTermino", (object)"");
//                            num1 = sqlCommand2.ExecuteNonQuery();
//                            int32 = Convert.ToInt32(sqlDataReader["IdContratada"]);
//                        }
//                        sqlDataReader.Close();
//                        if (!new SqlCommand("select IdContratada from tb_IntegraContratada where IdContratada = " + (object)int32 + " and idIntegra=1", connection).ExecuteReader().Read())
//                        {
//                            SqlCommand sqlCommand2 = new SqlCommand("sp_IntegraContratadaInserir", connection);
//                            sqlCommand2.CommandType = CommandType.StoredProcedure;
//                            sqlCommand2.Parameters.AddWithValue("IdContratada", (object)int32);
//                            sqlCommand2.Parameters.AddWithValue("IdExterno", (object)item.idEmpresaIntegrador);
//                            sqlCommand2.Parameters.AddWithValue("IdIntegra", (object)"1");
//                            if ((uint)sqlCommand2.ExecuteNonQuery() <= 0U)
//                            {
//                                ret.AddResult(99, "Erro não especificado");
//                                retorno.StatusCode = 1;
//                                retorno.StatusMessage = "Error";
//                            }
//                        }
//                    }
//                    catch (SqlException ex)
//                    {
//                        int errorCode = ex.ErrorCode;
//                        if (ex.Message.Contains("UQ_Tb_Contratada_CdCnpj"))
//                            ret.AddResult(503, "Já existe uma Empresa com o CNPJ fornecido.");
//                    }
//                    catch (Exception ex)
//                    {
//                        if (MensagensList.Count > 0)
//                        {
//                            ret.AddResult(99, "Erro não especificado : " + ex.Message + item.endereco);
//                            retorno.StatusCode = 1;
//                            retorno.StatusMessage = "Error";
//                        }
//                    }
//                }
//                if (item.tipoOperacao.ToUpper() == "A")
//                {
//                    try
//                    {
//                        SqlCommand sqlCommand2 = new SqlCommand("sp_ContratadaEditar", connection);
//                        sqlCommand2.CommandType = CommandType.StoredProcedure;
//                        sqlCommand2.Parameters.AddWithValue("IdContratada", (object)num2);
//                        sqlCommand2.Parameters.AddWithValue("CdCnpj", (object)item.cnpj);
//                        sqlCommand2.Parameters.AddWithValue("DsRazaoSocial", (object)item.razaoSocial);
//                        sqlCommand2.Parameters.AddWithValue("DsNomeFantasia", (object)item.nomeFantasia);
//                        sqlCommand2.Parameters.AddWithValue("UsuarioId", (object)1);
//                        sqlCommand2.Parameters.AddWithValue("CdOperacao", (object)10);
//                        sqlCommand2.Parameters.AddWithValue("CdAtivo", (object)1);
//                        sqlCommand2.Parameters.AddWithValue("Endereco", (object)item.endereco);
//                        sqlCommand2.Parameters.AddWithValue("NumEndereco", (object)item.numEndereco);
//                        sqlCommand2.Parameters.AddWithValue("BairroEndereco", (object)item.bairroEndereco);
//                        sqlCommand2.Parameters.AddWithValue("Cidade", (object)item.cidade);
//                        sqlCommand2.Parameters.AddWithValue("UF", (object)item.uf);
//                        sqlCommand2.Parameters.AddWithValue("CEP", (object)item.cep);
//                        sqlCommand2.Parameters.AddWithValue("Representante", (object)item.representante);
//                        sqlCommand2.Parameters.AddWithValue("EmailRepresentante", (object)item.emailRepresentante);
//                        sqlCommand2.Parameters.AddWithValue("ValidadeDtInicio", (object)"");
//                        sqlCommand2.Parameters.AddWithValue("ValidadeDtTermino", (object)"");
//                        if ((uint)sqlCommand2.ExecuteNonQuery() <= 0U)
//                        {
//                            ret.AddResult(99, "Erro não especificado . ");
//                            retorno.StatusCode = 1;
//                            retorno.StatusMessage = "Error";
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        ret.AddResult(99, "Erro não especificado : " + ex.Message);
//                        retorno.StatusCode = 1;
//                        retorno.StatusMessage = "Error";
//                    }
//                }
//                if (item.tipoOperacao.ToUpper() == "D")
//                {
//                    try
//                    {
//                        SqlCommand sqlCommand2 = new SqlCommand("SP_IntegraContratadaDesativar", connection);
//                        sqlCommand2.CommandType = CommandType.StoredProcedure;
//                        sqlCommand2.Parameters.AddWithValue("IdContratada", (object)num2);
//                        if ((uint)sqlCommand2.ExecuteNonQuery() <= 0U)
//                        {
//                            ret.AddResult(99, "Erro não especificado . ");
//                            retorno.StatusCode = 1;
//                            retorno.StatusMessage = "Error";
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        ret.AddResult(99, "Erro não especificado : " + ex.Message);
//                        retorno.StatusCode = 1;
//                        retorno.StatusMessage = "Error";
//                    }
//                }
//                if (item.tipoOperacao.ToUpper() == "R")
//                {
//                    try
//                    {
//                        SqlCommand sqlCommand2 = new SqlCommand("SP_IntegraContratadaAtivar", connection);
//                        sqlCommand2.CommandType = CommandType.StoredProcedure;
//                        sqlCommand2.Parameters.AddWithValue("IdContratada", (object)num2);
//                        if ((uint)sqlCommand2.ExecuteNonQuery() <= 0U)
//                        {
//                            ret.AddResult(99, "Erro não especificado . ");
//                            retorno.StatusCode = 1;
//                            retorno.StatusMessage = "Error";
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        ret.AddResult(99, "Erro não especificado : " + ex.Message);
//                        retorno.StatusCode = 1;
//                        retorno.StatusMessage = "Error";
//                    }
//                }
//            }
//            connection.Close();
//        }
//        catch (Exception ex)
//        {
//            retorno.StatusCode = 1;
//            retorno.StatusMessage = "Error";
//            ret.AddResult(99, "Erro não especificado : " + ex.Message);
//        }
//    }
//    catch (Exception ex)
//    {
//        retorno.StatusCode = 1;
//        retorno.StatusMessage = "Error";
//        ret.AddResult(98, "Falha na conexão com a base de dados. " + ex.Message);
//    }
//}
//else
//{
//    retorno.StatusCode = 1;
//    retorno.StatusMessage = "Error";
//}
//retorno.Result = MensagensList;


