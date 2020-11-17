namespace WEBAPI_VOPAK
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WEBAPI_VOPAK.Models;

    internal class ColaboradorRepositorio
    {
        /// <summary>
        /// Cadastra ou altera os dados de um MOTORISTA.
        /// </summary>
        /// <param name="item">Objeto do tipo <see cref="Colaborador"/>.</param>
        /// <param name="wIdColaborador">Inteiro, referência. Retorna o id do registro processado, ou 0 em caso de erros.</param>
        /// <returns>Objeto do tipo <see cref="Retorno"/>.</returns>
        public static Retorno Add(ColaboradorViewModel item, ref int wIdColaborador)
        {
            Retorno ret = new Retorno();
            wIdColaborador = 0;
            try
            {
                // Para decisão;
                ColaboradorViewModel colaboradorPesquisado = null;

                if (!item.dtNascimento.IsDate())
                    item.dtNascimento = (new DateTime(1899, 01, 01)).ToString("dd/MM/yyyy");

                // Nascimento para Data/Hora.
                DateTime wNascimento = Convert.ToDateTime(item.dtNascimento);

                // Trazer o ID do crachá informado.
                //int wIdCracha = Dados.GetIdCracha(item.Ambiente, item.numeroCracha);
                // Tipo do crachá informado.
                //int wIdTipoCracha = Dados.GetIdTipoDoCracha(item.Ambiente, item.numeroCracha);
                // Trazer o tipo de claborador.
                int wIdTipoColaborador = Dados.GetIdTipoMotorista(item.Ambiente);
                // Tipo do documento.
                int wIdTipoDocumento = Dados.GetIdTipoDocumento(item.Ambiente, item.tipoDocumento);
                if (wIdTipoDocumento == 0) wIdTipoDocumento = Dados.GetIdTipoDocumento(item.Ambiente);
                // Perfil do colaborador.
                int wIdPerfilColaborador = Dados.GetIdPerfilColaborador(item.Ambiente, "Motorista");

                // Trazer a contratada indicada, se existir.
                EmpresaViewModel contr = Dados.GetContratadaIntegrada(item.Ambiente, item.idEmpresaIntegrador, true);

                // A contratada informada existe?
                bool ContratadaExiste = false;
                if (contr != null) ContratadaExiste = (contr.IdContratada > 0);

                // Trazer o(s) colaborador(es) pelo id da integra OU pelo CPF:
                List<ColaboradorViewModel> colab = Dados
                    .GetColaboradorIntegrado(item.Ambiente, item.idEmpresaIntegrador, item.idColaboradorIntegrador, item.cpf)
                    .ToList();
                // Se a contratada não foi localizada
                if (!ContratadaExiste)
                    ret.AddResult(ErroSmartApi.ContratadaNaoLocalizada);
                else
                {
                    // Tem algum colaborador informado?
                    if (colab.Count > 0)
                    {
                        //  1: Colaborador existe mas com outro id integra.
                        //      Atualizar o colaborador e integrar o colaborador com a 
                        //      empresa informada.
                        colaboradorPesquisado = (colab
                              .Where(p => (p.idEmpresaIntegrador != item.idEmpresaIntegrador ||
                                           p.idColaboradorIntegrador != item.idColaboradorIntegrador))
                              .FirstOrDefault());
                        if (colaboradorPesquisado != null)
                        {
                            if (Dados.AlteraColaborador(item.Ambiente, item.idEmpresaIntegrador, item.idColaboradorIntegrador, wIdTipoColaborador, colaboradorPesquisado.cdAtivo, colaboradorPesquisado.cdExcluido,
                                        colaboradorPesquisado.idColaborador, item.nome, contr.cnpj, item.cpf, item.cnh, item.orgaoEmissorCnh, item.emissorCnh, item.nrNis, item.numDocumento, wIdTipoDocumento,
                                        item.sexo, wNascimento, item.validadeDtInicio, item.validadeDtTermino, item.foto,
                                        item.endereco, item.numEndereco, item.bairroEndereco, item.cidade, item.uf,
                                        item.cep, item.tel, item.emailColaborador, item.usuarioId, colaboradorPesquisado.idPerfilColaborador)) ;
                            bool temp = Dados.InsereImagemDoColaborador(item.Ambiente, item.foto, (int)colaboradorPesquisado.idColaborador, colaboradorPesquisado.cpf);
                            temp = Dados.InsereRelacContratadaColaborabor(item.Ambiente, item.usuarioId, colaboradorPesquisado.idColaborador,
                                contr.IdContratada);
                            //if (wIdCracha <= 0)
                            //{
                            //temp = Dados.InsereCrachaColaborador(item.Ambiente,
                            //    item.usuarioId,
                            //    wIdTipoCracha,
                            //    colaboradorPesquisado.cnpj,
                            //    (int)colaboradorPesquisado.idColaborador,
                            //    item.numeroCracha);
                            //}
                            wIdColaborador = colaboradorPesquisado.idColaborador.Value;
                        }
                        else
                        {
                            //  2 Colaborador existe e com o mesmo id integra.
                            //      Alterar os dados do colaborador e do crachá, 
                            //      quando for um crachá diferente.
                            colaboradorPesquisado = (colab
                                .Where(p => (p.idEmpresaIntegrador != item.idEmpresaIntegrador &&
                                             p.idColaboradorIntegrador == item.idColaboradorIntegrador))
                                .FirstOrDefault());
                            if (colaboradorPesquisado != null)
                            {
                                if (Dados.AlteraColaborador(item.Ambiente, item.idEmpresaIntegrador, item.idColaboradorIntegrador, wIdTipoColaborador, colaboradorPesquisado.cdAtivo, colaboradorPesquisado.cdExcluido,
                                                                    colaboradorPesquisado.idColaborador, item.nome, contr.cnpj, item.cpf, item.cnh, item.orgaoEmissorCnh, item.emissorCnh, item.nrNis, item.numDocumento, wIdTipoDocumento,
                                                                    item.sexo, wNascimento, item.validadeDtInicio, item.validadeDtTermino, item.foto,
                                                                    item.endereco, item.numEndereco, item.bairroEndereco, item.cidade, item.uf,
                                                                    item.cep, item.tel, item.emailColaborador, item.usuarioId, colaboradorPesquisado.idPerfilColaborador)) ;
                                bool temp = Dados.InsereImagemDoColaborador(item.Ambiente, item.foto, (int)colaboradorPesquisado.idColaborador, colaboradorPesquisado.cpf);
                                //if (wIdCracha <= 0)
                                //{
                                //    temp = Dados.InsereCrachaColaborador(item.Ambiente,
                                //        item.usuarioId,
                                //        wIdTipoCracha,
                                //        colaboradorPesquisado.cnpj,
                                //        (int)colaboradorPesquisado.idColaborador,
                                //        item.numeroCracha);
                                //}
                                wIdColaborador = colaboradorPesquisado.idColaborador.Value;
                            }
                        }
                    }

                    //  3 Colaborador não existe.
                    //      Inserir os dados do colaborador, dos relacionamentos e a imagem.
                    if (colab.Count == 0)
                    {
                        //if (wIdCracha > 0)
                        //{ ret.AddResult(ErroSmartApi.NumeroCrachaJaExiste); return ret; }
                        //else
                        wIdColaborador =
                            Dados.InsereColaborador(item.Ambiente, item.idColaboradorIntegrador, wIdTipoColaborador, contr.IdContratada,
                            item.nome, contr.cnpj, item.cpf, item.cnh, item.orgaoEmissorCnh, item.emissorCnh, item.nrNis, item.numDocumento, wIdTipoDocumento,
                            item.sexo, wNascimento, item.validadeDtInicio, item.validadeDtTermino,
                            item.foto, item.endereco, item.numEndereco, item.bairroEndereco, item.cidade, item.uf,
                            item.cep, item.tel, item.emailColaborador, item.usuarioId, item.numeroCracha, wIdPerfilColaborador);
                    }
                }



                //// O CPF informado já existe?
                //bool CPFDuplicado = false;
                //if (colab != null)
                //    CPFDuplicado = (colab
                //        .Where(p=> (p.idColaboradorIntegrador != item.idColaboradorIntegrador && 
                //        p.idColaborador > 0))
                //        .Count()>0);                
                ////4 CPF duplicado.
                ////5 Novo CPF.

                //if (!ContratadaExiste)
                //    ret.AddResult(ErroSmartApi.ContratadaNaoLocalizada);
                //else
                //{
                //    if (CPFDuplicado)
                //        ret.AddResult(ErroSmartApi.CPFEmUso);
                //    else
                //    {
                //        // Verificar 
                //        bool gravou = false;
                //        if (ColaboradorExiste)
                //        {
                //            bool ativo, excluido;
                //            ColaboradorViewModel colaboradorEfetivo = colab
                //                .Where(p => (p.idEmpresaIntegrador.Equals(item.idEmpresaIntegrador) && 
                //                             p.idColaborador > 0))
                //                .FirstOrDefault();
                //            switch (item.tipoOperacao.ToUpper())
                //            {
                //                case "I":
                //                case "R":
                //                    ativo = true;
                //                    excluido = false;
                //                    break;
                //                case "A": 
                //                    ativo = colaboradorEfetivo.cdAtivo.Value; 
                //                    excluido = colaboradorEfetivo.cdExcluido.Value; 
                //                    break;
                //                default: 
                //                    excluido = true; 
                //                    ativo = false; 
                //                    break;
                //            }
                //            gravou = Dados.AlteraColaborador(item.Ambiente, item.idEmpresaIntegrador, item.idColaboradorIntegrador, wIdTipoColaborador, ativo, excluido,
                //                colab[0].idColaborador, item.nome, contr.cnpj, item.cpf, item.cnh, item.orgaoEmissorCnh, item.emissorCnh, item.nrNis, item.numDocumento, wIdTipoDocumento,
                //                item.sexo, wNascimento, item.validadeDtInicio, item.validadeDtTermino, item.foto,
                //                item.endereco, item.numEndereco, item.bairroEndereco, item.cidade, item.uf,
                //                item.cep, item.tel, item.emailColaborador, item.usuarioId);
                //            if (gravou)
                //                  wIdColaborador = colab[0].idColaborador.Value;
                //        }
                //        else
                //        {
                //            if (wIdCracha > 0)
                //            { ret.AddResult(ErroSmartApi.NumeroCrachaJaExiste); return ret; }
                //            else
                //                wIdColaborador =
                //                    Dados.InsereColaborador(item.Ambiente, item.idColaboradorIntegrador, wIdTipoColaborador, contr.IdContratada,
                //                    item.nome, contr.cnpj, item.cpf, item.cnh, item.orgaoEmissorCnh, item.emissorCnh, item.nrNis, item.numDocumento, wIdTipoDocumento,
                //                    item.sexo, wNascimento, item.validadeDtInicio, item.validadeDtTermino,
                //                    item.foto, item.endereco, item.numEndereco, item.bairroEndereco, item.cidade, item.uf,
                //                    item.cep, item.tel, item.emailColaborador, item.usuarioId, item.numeroCracha);
                //        }
                //    }
                //}
            }
            catch (Exception ex) { ret.AddResult(ErroSmartApi.ErroNaoEspecificado); ex.Log(); }
            return ret;
        }
    }
}








//public bool ValidaCEP(string cep)
//{
//    cep = cep.Replace(".", "");
//    cep = cep.Replace("-", "");
//    cep = cep.Replace(" ", "");
//    Regex Rgx = new Regex(@"^\d{8}$");
//    if (!Rgx.IsMatch(cep))
//        return false;
//    else
//        return true;
//}
////internal class ColaboradorRepositorio : IColaboradorRepositorio
////{
////    private string sql = "";
////    private SqlCommand cmd = new SqlCommand();
////    private SqlConnection DtCon = new SqlConnection();
////    private SqlCommand cmd1;
////    private SqlDataReader dr1;
////    private SqlDataReader dr;
////    public Retorno Add(Colaborador item, string IP)
////    {
////        Retorno retorno = new Retorno();
////        List<Mensagens> MensagensList = new List<Mensagens>();
////        if (item.Ambiente == null)
////            MensagensList.Add(new Mensagens(1, "Parâmetro Ambiente obrigatório."));
////        else if (item.Ambiente.Length == 0)
////        {
////            MensagensList.Add(new Mensagens(1, "Parâmetro Ambiente obrigatório."));
////        }
////        else
////        {
////            if (item.Ambiente.ToUpper() != "T" && item.Ambiente.ToUpper() != "P")
////            {
////                MensagensList.Add(new Mensagens(2, "Parâmetro Ambiente deve ser preenchido com T ou P."));
////                retorno.Result = MensagensList;
////                return retorno;
////            }
////            if (item.Ambiente.ToUpper() == "P")
////            {
////                this.DtCon.ConnectionString = ConfigurationManager.AppSettings["BANCO_PRODUCAO"].ToString();
////                this.DtCon.Open();
////            }
////            else
////            {
////                this.DtCon.ConnectionString = ConfigurationManager.AppSettings["BANCO_TESTE"].ToString();
////                this.DtCon.Open();
////            }
////        }
////        if (item.idEmpresaIntegrador == 0)
////            MensagensList.Add(new Mensagens(7, "Parâmetro idEmpresaIntegrador Obrigatório."));
////        if (item.idColaboradorIntegrador == 0)
////            MensagensList.Add(new Mensagens(28, "Parâmetro idColaboradorIntegrador Obrigatório."));
////        if (string.IsNullOrEmpty(item.tipoDocumento))
////            item.tipoDocumento = "";
////        if (string.IsNullOrEmpty(item.numDocumento))
////            item.numDocumento = "";
////        if ((!string.IsNullOrEmpty(item.cpf) && (!item.cpf.IsBrCpf()) | 
////            ((!item.cpf.IsBrCpf()) && (!string.IsNullOrEmpty(item.numDocumento)) || 
////            !string.IsNullOrEmpty(item.tipoDocumento))))
////        {
////            if (item.tipoDocumento == "" || string.IsNullOrEmpty(item.tipoDocumento))
////            {
////                MensagensList.Add(new Mensagens(30, "Parâmetro tipoDocumento Obrigatório."));
////            }
////            else
////            {
////                if (item.tipoDocumento.Length > 10)
////                    MensagensList.Add(new Mensagens(30, "Parâmetro tipoDocumento deve ser informado com o tamanho máximo de 10 caracteres."));
////                if (!string.IsNullOrEmpty(item.tipoDocumento))
////                {
////                    this.sql = string.Format("SELECT IdTipoDocumento from tb_tipoDocumento where DsTipoDocumento = '{0}'", (object)item.tipoDocumento);
////                    this.cmd = new SqlCommand(this.sql, this.DtCon);
////                    this.dr = this.cmd.ExecuteReader();
////                    DataTable dataTable = new DataTable();
////                    dataTable.Load((IDataReader)this.dr);
////                    this.dr.Close();
////                    if (dataTable.Rows.Count != 1)
////                    {
////                        MensagensList.Add(new Mensagens(31, " Parâmetro tipoDocumento inválido."));
////                    }
////                    else
////                    {
////                        item.tipoDocumento = dataTable.Rows[0]["IdTipoDocumento"].ToString();
////                        if (item.idColaboradorIntegrador.ToString().Trim() != item.cpf.Trim())
////                            MensagensList.Add(new Mensagens(500, " Parâmetro CPF deve ser igual ao idColaboradorIntegrador quando tipoDocumento/documento for fornecido."));
////                    }
////                }
////            }
////            if (item.numDocumento == null)
////                MensagensList.Add(new Mensagens(32, "Parâmetro numDocumento Obrigatório quando o cpf não existir."));
////            else if (item.numDocumento.Length == 0 || item.numDocumento.Length > 20)
////                MensagensList.Add(new Mensagens(32, "Parâmetro numDocumento deve ter entre 2 e 20 caracteres."));
////        }
////        else
////        {
////            if (item.cpf.Length > 11)
////                MensagensList.Add(new Mensagens(33, "Parâmetro cpf deve ser informado com no máximo de 11 caracteres."));
////            if (item.tipoDocumento.Length > 0)
////            {
////                if (item.tipoDocumento.Length > 10)
////                    MensagensList.Add(new Mensagens(30, "Parâmetro tipoDocumento deve ser informado com o tamanho máximo de 10 caracteres."));
////                if (item.tipoDocumento.Trim().ToUpper() != "RG" && item.tipoDocumento.Trim().ToUpper() != "RNE")
////                    MensagensList.Add(new Mensagens(31, " Parâmetro tipoDocumento inválido."));
////            }
////            if (item.numDocumento.Length > 0)
////            {
////                if (item.numDocumento.Length > 20)
////                    MensagensList.Add(new Mensagens(32, "Parâmetro numDocumento deve ser informado com o tamanho máximo de 20 caracteres."));
////            }
////            else if (!item.cpf.IsBrCpf())
////                MensagensList.Add(new Mensagens(33, "Parâmetro cpf inválido."));
////        }
////        if (item.nome == null)
////        {
////            MensagensList.Add(new Mensagens(35, "Parâmetro nome não informado."));
////        }
////        else
////        {
////            if (item.nome.Length == 0)
////                MensagensList.Add(new Mensagens(35, "Parâmetro nome não informado."));
////            if (item.nome.Length > 50)
////                MensagensList.Add(new Mensagens(36, "Parâmetro nome deve ser informado com no máximo 50 caracteres"));
////        }
////        if (item.sexo == null)
////        {
////            MensagensList.Add(new Mensagens(37, "Parâmetro sexo não informado."));
////        }
////        else
////        {
////            if (item.sexo.Length == 0)
////                MensagensList.Add(new Mensagens(37, "Parâmetro sexo não informado."));
////            if (item.sexo.Length > 1)
////                MensagensList.Add(new Mensagens(37, "Parâmetro sexo deve ser informado com no máximo 1 caractere"));
////            if (item.sexo.ToUpper() != "F" && item.sexo.ToUpper() != "M")
////                MensagensList.Add(new Mensagens(38, "Parâmetro sexo com valor inválido"));
////        }
////        if (item.dtNascimento != null)
////        {
////            if (!item.dtNascimento.IsDate())
////                MensagensList.Add(new Mensagens(39, "Parâmetro dtNascimento com valor inválido"));
////            else
////                item.dtNascimento = DateTime.Parse(item.dtNascimento).ToString("yyyy-MM-dd");
////        }
////        else
////            item.dtNascimento = "";
////        if (item.endereco != null)
////        {
////            if (item.endereco.Length > 150)
////                MensagensList.Add(new Mensagens(15, "Parâmetro endereco deve ser informado com no máximo 150 caracteres"));
////        }
////        else
////            item.endereco = "";
////        if (item.numEndereco != null)
////        {
////            if (item.numEndereco.Length > 10)
////                MensagensList.Add(new Mensagens(16, "Parâmetro numEndereco deve ser informado com no máximo 10 caracteres"));
////        }
////        else
////            item.numEndereco = "";
////        if (item.bairroEndereco != null)
////        {
////            if (item.bairroEndereco.Length > 50)
////                MensagensList.Add(new Mensagens(17, "Parâmetro bairroEndereco deve ser informado com no máximo 50 caracteres"));
////        }
////        else
////            item.bairroEndereco = "";
////        if (item.cidade == null)
////        {
////            MensagensList.Add(new Mensagens(18, "Parâmetro cidade obrigatório"));
////        }
////        else
////        {
////            if (item.cidade.Length == 0)
////                MensagensList.Add(new Mensagens(18, "Parâmetro cidade obrigatório"));
////            if (item.cidade.Length > 50)
////                MensagensList.Add(new Mensagens(19, "Parâmetro cidade deve ser informado com no máximo 50 caracteres"));
////        }
////        if (item.uf == null)
////        {
////            MensagensList.Add(new Mensagens(20, "Parâmetro ufMotorista obrigatório"));
////        }
////        else
////        {
////            if (item.uf.Length == 0)
////                MensagensList.Add(new Mensagens(20, "Parâmetro ufMotorista obrigatório"));
////            if (item.uf == "")
////                MensagensList.Add(new Mensagens(20, "Parâmetro ufMotorista obrigatório"));
////            if (item.uf.Length != 2)
////                MensagensList.Add(new Mensagens(21, "Parâmetro ufMotorista deve ser informado com no máximo 2 caracteres"));
////            if (!Regex.IsMatch(item.uf, "^[a-zA-Z]+$"))
////                MensagensList.Add(new Mensagens(22, "Parâmetro ufMotorista deve conter somente letras."));
////        }
////        if (item.cep != null)
////        {
////            if (item.cep.Length > 8 || item.cep.Length < 8)
////                MensagensList.Add(new Mensagens(23, "Parâmetro cepMotorista deve ser informado com no máximo 8 caracteres."));
////            else if (!item.cep.IsBrZIP())
////                MensagensList.Add(new Mensagens(24, "Parâmetro cepMotorista deve conter somente números."));
////        }
////        else
////            item.cep = "";
////        if (item.tel != null)
////        {
////            if (item.tel.Trim().Length > 13)
////                MensagensList.Add(new Mensagens(40, "Parâmetro tel deve ser informado com no máximo 13 caracteres (55 11 987654321)."));
////            if ((item.tel.Trim().Length > 0 || item.tel.Trim().Length <= 13) && !item.tel.All<char>((Func<char, bool>)(c =>
////           {
////               if (c >= '0')
////                   return c <= '9';
////               return false;
////           })))
////                MensagensList.Add(new Mensagens(41, "Parâmetro tel deve conter somente números. (55 11 987654321)"));
////        }
////        else
////            item.tel = "";
////        if (item.emailColaborador != null)
////        {
////            if (item.emailColaborador.Length > 100)
////                MensagensList.Add(new Mensagens(43, "Parâmetro emailColaborador deve ser informado com no máximo 100 caracteres."));
////            if (item.emailColaborador.Length > 0 && !item.emailColaborador.IsEmail())
////                MensagensList.Add(new Mensagens(44, "Parâmetro emailColaborador inválido."));
////        }
////        else
////            item.emailColaborador = "";
////        if (item.numeroCracha != null)
////        {
////            if ((uint)item.numeroCracha.Length > 0U && item.numeroCracha.Length != 10)
////                MensagensList.Add(new Mensagens(45, "Parâmetro numeroCracha deve ser informado com no máximo 10 caracteres."));
////        }
////        else
////            item.numeroCracha = "";
////        if (item.cnh != null)
////        {
////            if ((uint)item.cnh.Length > 0U)
////            {
////                if (item.cnh.Length > 11)
////                    MensagensList.Add(new Mensagens(46, "Parâmetro cnh deve ser informado com no máximo 11 caracteres."));
////            }
////            else
////                MensagensList.Add(new Mensagens(53, "Parâmetro cnh obrigatório."));
////        }
////        else
////            MensagensList.Add(new Mensagens(53, "Parâmetro cnh obrigatório."));
////        if (item.orgaoEmissorCnh != null)
////        {
////            if ((uint)item.orgaoEmissorCnh.Length > 0U && item.orgaoEmissorCnh.Length > 50)
////                MensagensList.Add(new Mensagens(48, "Parâmetro orgaoEmissorCnh deve ser informado com no máximo 50 caracteres."));
////        }
////        else
////            item.orgaoEmissorCnh = "";
////        if (item.emissorCnh != null)
////        {
////            if ((uint)item.emissorCnh.Length > 0U && item.emissorCnh.Length > 2)
////                MensagensList.Add(new Mensagens(47, "Parâmetro emissorCnh deve ser informado com no máximo 2 caracteres."));
////        }
////        else
////            item.emissorCnh = "";
////        if (item.foto == null)
////            MensagensList.Add(new Mensagens(54, "Foto não informada.Teste"));
////        if (MensagensList.Count == 0)
////        {
////            int num1 = 0;
////            try
////            {
////                try
////                {
////                    int num2 = 1;
////                    int num3 = 0;
////                    int num4 = 0;
////                    string str1 = "";
////                    this.cmd = new SqlCommand("select IdContratada from tb_integraContratada where idIntegra = " + (object)num2 + " and IdExterno = " + (object)item.idEmpresaIntegrador + " and isAtivo = 1", this.DtCon);
////                    this.dr = this.cmd.ExecuteReader();
////                    if (this.dr.Read())
////                        num3 = Convert.ToInt32(this.dr["IdContratada"]);
////                    this.dr.Close();
////                    if (num2 > 0)
////                    {
////                        if (item.tipoOperacao.ToUpper() == "A" || item.tipoOperacao.ToUpper() == "D")
////                        {
////                            this.cmd = new SqlCommand("select IdContratada from tb_integraContratada where idIntegra = " + (object)num2 + " and IdExterno = " + (object)item.idEmpresaIntegrador + " and isAtivo = 1", this.DtCon);
////                            this.dr = this.cmd.ExecuteReader();
////                            if (this.dr.Read())
////                                num3 = Convert.ToInt32(this.dr["IdContratada"]);
////                            this.dr.Close();
////                            this.cmd = new SqlCommand("select IdColaborador from tb_integraColaborador where idIntegra = " + (object)num2 + " and IdContratada = '" + (object)num3 + "' and IdExterno = " + (object)item.idColaboradorIntegrador + " and isAtivo = 1", this.DtCon);
////                            this.dr = this.cmd.ExecuteReader();
////                            if (this.dr.Read())
////                                num4 = Convert.ToInt32(this.dr["IdColaborador"]);
////                            this.dr.Close();
////                        }
////                        if (item.tipoOperacao.ToUpper() == "R")
////                        {
////                            this.cmd = new SqlCommand("select IdContratada from tb_integraContratada where idIntegra = " + (object)num2 + " and IdExterno = " + (object)item.idEmpresaIntegrador + " and isAtivo = 1", this.DtCon);
////                            this.dr = this.cmd.ExecuteReader();
////                            if (this.dr.Read())
////                                num3 = Convert.ToInt32(this.dr["IdContratada"]);
////                            this.dr.Close();
////                            this.cmd = new SqlCommand("select IdColaborador from tb_integraColaborador where idIntegra = " + (object)num2 + " and IdContratada = '" + (object)num3 + "' and IdExterno = " + (object)item.idColaboradorIntegrador + " and isAtivo = 0", this.DtCon);
////                            this.dr = this.cmd.ExecuteReader();
////                            if (this.dr.Read())
////                                num4 = Convert.ToInt32(this.dr["IdColaborador"]);
////                            this.dr.Close();
////                        }

////                        if (item.tipoOperacao.ToUpper() == "I")
////                        {
////                            this.cmd = new SqlCommand("select a.IdContratada from tb_integraColaborador a inner join tb_Colaborador b on a.IdColaborador = b.IdColaborador where a.idExterno = " + (object)item.idColaboradorIntegrador + " and b.cdcpf = '" + item.cpf + "'", this.DtCon);
////                            this.dr = this.cmd.ExecuteReader();
////                            if (!this.dr.Read())
////                                ;
////                            this.dr.Close();
////                        }

////                        this.cmd = new SqlCommand("select cdcnpj from tb_IntegraContratada a inner join Tb_Contratada b on a.IdContratada = b.idContratada where  IdExterno = " + (object)item.idEmpresaIntegrador + " and isAtivo = 1", this.DtCon);
////                        this.dr = this.cmd.ExecuteReader();
////                        if (this.dr.Read())
////                            str1 = this.dr["cdcnpj"].ToString();
////                        this.dr.Close();
////                        if (item.numeroCracha.Length == 10)
////                        {
////                            this.cmd = new SqlCommand("select idCracha from tb_cracha where cdCracha = '" + (object)item.numeroCracha.Length + "'", this.DtCon);
////                            this.dr = this.cmd.ExecuteReader();
////                            if (this.dr.Read())
////                                MensagensList.Add(new Mensagens(41, "Parâmetro numeroCracha inválido."));
////                            this.dr.Close();
////                        }
////                        if ( item.cpf.IsBrCpf())
////                        {
////                            this.sql = "select IdTipoDocumento from tb_tipoDocumento where dsTipoDocumento =  'CPF'";
////                            this.cmd = new SqlCommand(this.sql, this.DtCon);
////                            this.dr = this.cmd.ExecuteReader();
////                            DataTable dataTable = new DataTable();
////                            dataTable.Load((IDataReader)this.dr);
////                            item.tipoDocumento = dataTable.Rows[0]["IdTipoDocumento"].ToString();
////                            item.numDocumento = item.cpf;
////                            this.dr.Close();
////                        }
////                        if (MensagensList.Count == 0)
////                        {
////                            if (item.tipoOperacao.ToUpper() == "I")
////                            {
////                                try
////                                {
////                                    this.sql = "SELECT colab.* FROM TB_Colaborador colab";
////                                    this.sql += " join TB_IntegraColaborador intCol on colab.IdColaborador = intCol.IdColaborador";
////                                    this.sql += string.Format(" where intCol.IdExterno = {0}", (object)item.idColaboradorIntegrador);
////                                    this.cmd = new SqlCommand(this.sql, this.DtCon);
////                                    this.dr = this.cmd.ExecuteReader();
////                                    DataTable dataTable1 = new DataTable();
////                                    dataTable1.Load((IDataReader)this.dr);
////                                    this.dr.Close();
////                                    if (!string.IsNullOrEmpty(item.cpf) && dataTable1.Rows.Count == 0)
////                                    {
////                                        this.sql = string.Format(" SELECT * FROM TB_COLABORADOR WHERE CdCpf = {0}", (object)item.cpf);
////                                        this.cmd = new SqlCommand(this.sql, this.DtCon);
////                                        this.dr = this.cmd.ExecuteReader();
////                                        if (this.dr.HasRows)
////                                        {
////                                            MensagensList.Add(new Mensagens(502, "Cpf duplicado"));
////                                            retorno.Result = MensagensList;
////                                            return retorno;
////                                        }
////                                    }
////                                    this.sql = "select IdTipoColaborador from tb_tipoColaborador where dsTipoColaborador =  'Motorista'";
////                                    this.cmd = new SqlCommand(this.sql, this.DtCon);
////                                    this.dr = this.cmd.ExecuteReader();
////                                    DataTable dataTable2 = new DataTable();
////                                    dataTable2.Load((IDataReader)this.dr);
////                                    string str2 = dataTable2.Rows[0]["IdTipoColaborador"].ToString();
////                                    this.dr.Close();
////                                    SqlCommand sqlCommand1 = new SqlCommand();
////                                    if (dataTable1.Rows.Count == 0)
////                                    {
////                                        SqlCommand sqlCommand2 = new SqlCommand("sp_ColaboradorInserir", this.DtCon);
////                                        sqlCommand2.CommandType = CommandType.StoredProcedure;
////                                        sqlCommand2.Parameters.AddWithValue("DsColaborador", (object)item.nome);
////                                        sqlCommand2.Parameters.AddWithValue("cdCPF", (object)item.cpf);
////                                        sqlCommand2.Parameters.AddWithValue("CdCnpj", (object)str1);
////                                        sqlCommand2.Parameters.AddWithValue("IdTipoColaborador", (object)str2);
////                                        sqlCommand2.Parameters.AddWithValue("UsuarioId", (object)1);
////                                        sqlCommand2.Parameters.AddWithValue("CdOperacao", (object)10);
////                                        sqlCommand2.Parameters.AddWithValue("TipoInstituicao", (object)"CONTRATADA");
////                                        sqlCommand2.Parameters.AddWithValue("DtNascimento", (object)item.dtNascimento);
////                                        sqlCommand2.Parameters.AddWithValue("DsGenero", (object)item.sexo);
////                                        if (item.tipoDocumento.ToUpper().Trim() == "RG")
////                                            sqlCommand2.Parameters.AddWithValue("NrNis", (object)item.numDocumento);
////                                        sqlCommand2.Parameters.AddWithValue("IdTipoDocumento", (object)item.tipoDocumento);
////                                        sqlCommand2.Parameters.AddWithValue("DsDocumento", (object)item.numDocumento.ToUpper());
////                                        sqlCommand2.Parameters.AddWithValue("DsObservacao", (object)"");
////                                        sqlCommand2.Parameters.AddWithValue("Endereco", (object)item.endereco);
////                                        sqlCommand2.Parameters.AddWithValue("NumEndereco", (object)item.numEndereco);
////                                        sqlCommand2.Parameters.AddWithValue("BairroEndereco", (object)item.bairroEndereco);
////                                        sqlCommand2.Parameters.AddWithValue("Cidade", (object)item.cidade);
////                                        sqlCommand2.Parameters.AddWithValue("UF", (object)item.uf);
////                                        sqlCommand2.Parameters.AddWithValue("CEP", (object)item.cep);
////                                        sqlCommand2.Parameters.AddWithValue("TEL", (object)item.tel);
////                                        sqlCommand2.Parameters.AddWithValue("Funcao", (object)"");
////                                        sqlCommand2.Parameters.AddWithValue("EmailColaborador", (object)item.emailColaborador);
////                                        sqlCommand2.Parameters.AddWithValue("Cnh", (object)item.cnh);
////                                        sqlCommand2.Parameters.AddWithValue("OrgaoEmissorCNH", (object)item.orgaoEmissorCnh);
////                                        sqlCommand2.Parameters.AddWithValue("EmissorCNH", (object)item.emissorCnh);
////                                        sqlCommand2.Parameters.AddWithValue("ValidadeDtInicio", (object)"");
////                                        sqlCommand2.Parameters.AddWithValue("ValidadeDtTermino", (object)"");
////                                        SqlParameter sqlParameter1 = sqlCommand2.Parameters.Add("IdRetorno", SqlDbType.Int);
////                                        sqlParameter1.Direction = ParameterDirection.Output;
////                                        SqlParameter sqlParameter2 = sqlCommand2.Parameters.Add("IdInstituicao", SqlDbType.Int);
////                                        sqlParameter2.Direction = ParameterDirection.Output;
////                                        num1 = sqlCommand2.ExecuteNonQuery();
////                                        num4 = Convert.ToInt32(sqlParameter1.Value);
////                                        num3 = Convert.ToInt32(sqlParameter2.Value);
////                                    }
////                                    else
////                                    {
////                                        this.sql = string.Format(" SELECT * FROM TB_COLABORADOR WHERE CdCpf = {0} and idColaborador <> {1}", (object)item.cpf, dataTable1.Rows[0]["IdColaborador"]);
////                                        this.cmd = new SqlCommand(this.sql, this.DtCon);
////                                        this.dr = this.cmd.ExecuteReader();
////                                        if (this.dr.HasRows)
////                                        {
////                                            MensagensList.Add(new Mensagens(502, "Cpf duplicado"));
////                                            retorno.Result = MensagensList;
////                                            return retorno;
////                                        }
////                                        string str3 = dataTable1.Rows[0]["IdColaborador"].ToString();
////                                        num4 = Convert.ToInt32(str3);
////                                        SqlCommand sqlCommand2 = new SqlCommand("sp_ColaboradorEditar", this.DtCon);
////                                        sqlCommand2.CommandType = CommandType.StoredProcedure;
////                                        sqlCommand2.Parameters.AddWithValue("idColaborador", (object)str3);
////                                        sqlCommand2.Parameters.AddWithValue("DsColaborador", (object)item.nome);
////                                        sqlCommand2.Parameters.AddWithValue("cdCPF", (object)item.cpf);
////                                        sqlCommand2.Parameters.AddWithValue("CdCnpj", (object)str1);
////                                        sqlCommand2.Parameters.AddWithValue("IdTipoColaborador", (object)str2);
////                                        sqlCommand2.Parameters.AddWithValue("UsuarioId", (object)1);
////                                        sqlCommand2.Parameters.AddWithValue("CdOperacao", (object)10);
////                                        sqlCommand2.Parameters.AddWithValue("CdAtivo", (object)1);
////                                        sqlCommand2.Parameters.AddWithValue("DtNascimento", (object)item.dtNascimento);
////                                        sqlCommand2.Parameters.AddWithValue("DsGenero", (object)item.sexo);
////                                        sqlCommand2.Parameters.AddWithValue("Tel", (object)item.tel);
////                                        sqlCommand2.Parameters.AddWithValue("IdTipoDocumento", (object)item.tipoDocumento);
////                                        sqlCommand2.Parameters.AddWithValue("DsDocumento", (object)item.numDocumento);
////                                        if (item.tipoDocumento.ToUpper().Trim() == "RG")
////                                            sqlCommand2.Parameters.AddWithValue("NrNis", (object)item.numDocumento);
////                                        sqlCommand2.Parameters.AddWithValue("DsObservacao", (object)"");
////                                        sqlCommand2.Parameters.AddWithValue("Endereco", (object)item.endereco);
////                                        sqlCommand2.Parameters.AddWithValue("NumEndereco", (object)item.numEndereco);
////                                        sqlCommand2.Parameters.AddWithValue("BairroEndereco", (object)item.bairroEndereco);
////                                        sqlCommand2.Parameters.AddWithValue("Cidade", (object)item.cidade);
////                                        sqlCommand2.Parameters.AddWithValue("UF", (object)item.uf);
////                                        sqlCommand2.Parameters.AddWithValue("CEP", (object)item.cep);
////                                        sqlCommand2.Parameters.AddWithValue("funcao", (object)"");
////                                        sqlCommand2.Parameters.AddWithValue("EmailColaborador", (object)item.emailColaborador);
////                                        sqlCommand2.Parameters.AddWithValue("Cnh", (object)item.cnh);
////                                        sqlCommand2.Parameters.AddWithValue("OrgaoEmissorCNH", (object)item.orgaoEmissorCnh);
////                                        sqlCommand2.Parameters.AddWithValue("EmissorCNH", (object)item.emissorCnh);
////                                        sqlCommand2.Parameters.AddWithValue("ValidadeDtInicio", (object)"");
////                                        sqlCommand2.Parameters.AddWithValue("ValidadeDtTermino", (object)"");
////                                        num1 = sqlCommand2.ExecuteNonQuery();
////                                        this.sql = " select * from TB_RelacContratadaColaborador";
////                                        this.sql += string.Format("  where idContratada = {0} and idColaborador = {1}", (object)num3, (object)num4);
////                                        this.cmd = new SqlCommand(this.sql, this.DtCon);
////                                        this.dr = this.cmd.ExecuteReader();
////                                        if (!this.dr.Read())
////                                        {
////                                            SqlCommand sqlCommand3 = new SqlCommand("[SP_RelacContratadaColaboradorInserir]", this.DtCon);
////                                            sqlCommand3.CommandType = CommandType.StoredProcedure;
////                                            sqlCommand3.Parameters.AddWithValue("IdContratada", (object)num3);
////                                            sqlCommand3.Parameters.AddWithValue("idColaborador", (object)num4);
////                                            sqlCommand3.Parameters.AddWithValue("UsuarioId", (object)1);
////                                            sqlCommand3.Parameters.AddWithValue("CdOperacao", (object)10);
////                                            sqlCommand3.ExecuteNonQuery();
////                                        }
////                                    }
////                                    this.sql = string.Format(" select * from TB_IntegraColaborador  ic where ic.IdColaborador ={0}  and ic.IdContratada = {1} and ic.IdExterno = {2}", (object)num4, (object)num3, (object)item.idColaboradorIntegrador);
////                                    this.cmd = new SqlCommand(this.sql, this.DtCon);
////                                    this.dr = this.cmd.ExecuteReader();
////                                    if (!this.dr.Read())
////                                    {
////                                        SqlCommand sqlCommand2 = new SqlCommand("[SP_IntegraColaboradorInserir]", this.DtCon);
////                                        sqlCommand2.CommandType = CommandType.StoredProcedure;
////                                        sqlCommand2.Parameters.AddWithValue("idColaborador", (object)num4);
////                                        sqlCommand2.Parameters.AddWithValue("IdContratada", (object)num3);
////                                        sqlCommand2.Parameters.AddWithValue("IdIntegra", (object)1);
////                                        sqlCommand2.Parameters.AddWithValue("IdExterno", (object)item.idColaboradorIntegrador);
////                                        sqlCommand2.Parameters.AddWithValue("Display1", (object)"");
////                                        sqlCommand2.Parameters.AddWithValue("Display2", (object)"");
////                                        sqlCommand2.ExecuteNonQuery();
////                                    }
////                                    this.dr.Close();
////                                    if (item.foto != null)
////                                    {
////                                        this.cmd = new SqlCommand(string.Format(" SELECT * FROM [TB_PESSOA_IMAGENS] WHERE IdColaborador = {0}  AND StCracha = 1", (object)num4), this.DtCon);
////                                        this.dr = this.cmd.ExecuteReader();
////                                        if (this.dr.Read())
////                                        {
////                                            this.cmd = new SqlCommand(string.Format("DELETE FROM TB_PESSOA_IMAGENS WHERE IdColaborador = {0}  AND StCracha = 1", (object)num4), this.DtCon);
////                                            this.cmd.ExecuteNonQuery();
////                                        }
////                                        SqlCommand sqlCommand2 = new SqlCommand("SP_ImagemInserir", this.DtCon);
////                                        sqlCommand2.CommandType = CommandType.StoredProcedure;
////                                        sqlCommand2.Parameters.AddWithValue("IdColaborador", (object)num4);
////                                        sqlCommand2.Parameters.AddWithValue("cpf", (object)item.cpf);
////                                        sqlCommand2.Parameters.AddWithValue("img", (object)item.foto);
////                                        num1 = sqlCommand2.ExecuteNonQuery();
////                                    }
////                                }
////                                catch (SqlException ex)
////                                {
////                                    int errorCode = ex.ErrorCode;
////                                    string message = ex.Message;
////                                    if (message.Contains("UQ_IdTipoDocumentoDsDocumento"))
////                                        MensagensList.Add(new Mensagens(501, "TipoDocumento/Documento duplicados"));
////                                    else if (message.Contains("UQ_Cpf"))
////                                        MensagensList.Add(new Mensagens(502, "Cpf duplicado"));
////                                    else if (message.Contains("UQ_TB_IntegraColaborador_IdColaboradorIdcontratada"))
////                                        MensagensList.Add(new Mensagens(503, "Motorista já cadastrado para esta contratada."));
////                                    throw;
////                                }
////                                catch (Exception ex)
////                                {
////                                    if (MensagensList.Count == 0)
////                                        MensagensList.Add(new Mensagens(99, "Erro não especificado" + ex.Message));
////                                }
////                            }
////                            if (item.tipoOperacao.ToUpper() == "A")
////                            {
////                                try
////                                {
////                                    this.cmd = new SqlCommand("select IdColaborador, CdAtivo from tb_Colaborador where (CdCPF = '" + item.cpf + "' or NrNis = '" + item.numDocumento.ToUpper() + "')", this.DtCon);
////                                    this.dr = this.cmd.ExecuteReader();
////                                    if (this.dr.Read())
////                                    {
////                                        SqlCommand sqlCommand1 = new SqlCommand("sp_ColaboradorEditar", this.DtCon);
////                                        sqlCommand1.CommandType = CommandType.StoredProcedure;
////                                        sqlCommand1.Parameters.AddWithValue("idColaborador", (object)this.dr["idColaborador"].ToString());
////                                        sqlCommand1.Parameters.AddWithValue("DsColaborador", (object)item.nome);
////                                        sqlCommand1.Parameters.AddWithValue("cdCpf", (object)item.cpf);
////                                        sqlCommand1.Parameters.AddWithValue("CdCnpj", (object)str1);
////                                        sqlCommand1.Parameters.AddWithValue("IdTipoColaborador", (object)1212);
////                                        sqlCommand1.Parameters.AddWithValue("UsuarioId", (object)1);
////                                        sqlCommand1.Parameters.AddWithValue("CdOperacao", (object)10);
////                                        sqlCommand1.Parameters.AddWithValue("CdAtivo", (object)1);
////                                        sqlCommand1.Parameters.AddWithValue("DtNascimento", (object)item.dtNascimento);
////                                        sqlCommand1.Parameters.AddWithValue("DsGenero", (object)item.sexo);
////                                        sqlCommand1.Parameters.AddWithValue("Tel", (object)item.tel);
////                                        sqlCommand1.Parameters.AddWithValue("IdTipoDocumento", (object)item.tipoDocumento);
////                                        sqlCommand1.Parameters.AddWithValue("DsDocumento", (object)item.numDocumento.ToUpper());
////                                        sqlCommand1.Parameters.AddWithValue("DsObservacao", (object)"");
////                                        sqlCommand1.Parameters.AddWithValue("Endereco", (object)item.endereco);
////                                        sqlCommand1.Parameters.AddWithValue("NumEndereco", (object)item.numEndereco);
////                                        sqlCommand1.Parameters.AddWithValue("BairroEndereco", (object)item.bairroEndereco);
////                                        sqlCommand1.Parameters.AddWithValue("Cidade", (object)item.cidade);
////                                        sqlCommand1.Parameters.AddWithValue("UF", (object)item.uf);
////                                        sqlCommand1.Parameters.AddWithValue("CEP", (object)item.cep);
////                                        sqlCommand1.Parameters.AddWithValue("Funcao", (object)"");
////                                        sqlCommand1.Parameters.AddWithValue("EmailColaborador", (object)item.emailColaborador);
////                                        sqlCommand1.Parameters.AddWithValue("Cnh", (object)item.cnh);
////                                        sqlCommand1.Parameters.AddWithValue("OrgaoEmissorCNH", (object)item.orgaoEmissorCnh);
////                                        sqlCommand1.Parameters.AddWithValue("EmissorCNH", (object)item.emissorCnh);
////                                        sqlCommand1.Parameters.AddWithValue("ValidadeDtInicio", (object)"");
////                                        sqlCommand1.Parameters.AddWithValue("ValidadeDtTermino", (object)"");
////                                        if ((uint)sqlCommand1.ExecuteNonQuery() > 0U)
////                                        {
////                                            if (item.numeroCracha.Length == 10)
////                                            {
////                                                SqlDataReader sqlDataReader = new SqlCommand("select IdCracha , CdCracha from tb_Cracha where IdColaborador = " + this.dr["idColaborador"].ToString(), this.DtCon).ExecuteReader();
////                                                if (!sqlDataReader.Read())
////                                                {
////                                                    SqlCommand sqlCommand2 = new SqlCommand("sp_CrachaInserir", this.DtCon);
////                                                    sqlCommand2.CommandType = CommandType.StoredProcedure;
////                                                    sqlCommand2.Parameters.AddWithValue("CdCracha", (object)item.numeroCracha);
////                                                    sqlCommand2.Parameters.AddWithValue("CdCnpj", (object)str1);
////                                                    sqlCommand2.Parameters.AddWithValue("IdColaborador", (object)num4);
////                                                    sqlCommand2.Parameters.AddWithValue("UsuarioId", (object)1);
////                                                    sqlCommand2.Parameters.AddWithValue("CdOperacao", (object)10);
////                                                    sqlCommand2.Parameters.AddWithValue("IdTipoCracha", (object)1);
////                                                    int num5 = 0;
////                                                    num5 = sqlCommand2.ExecuteNonQuery();
////                                                }
////                                                else if (!new SqlCommand("select IdCracha , CdCracha from tb_Cracha where IdColaborador = " + this.dr["idColaborador"].ToString() + " and cdCracha = '" + item.numeroCracha + "'", this.DtCon).ExecuteReader().Read())
////                                                {
////                                                    SqlCommand command = this.DtCon.CreateCommand();
////                                                    command.CommandText = "update tb_cracha set cdAtivo=0 where IdColaborador =" + this.dr["idColaborador"].ToString();
////                                                    command.ExecuteNonQuery();
////                                                    SqlCommand sqlCommand2 = new SqlCommand("sp_CrachaInserir", this.DtCon);
////                                                    sqlCommand2.CommandType = CommandType.StoredProcedure;
////                                                    sqlCommand2.Parameters.AddWithValue("CdCracha", (object)item.numeroCracha);
////                                                    sqlCommand2.Parameters.AddWithValue("CdCnpj", (object)str1);
////                                                    sqlCommand2.Parameters.AddWithValue("IdColaborador", (object)num4);
////                                                    sqlCommand2.Parameters.AddWithValue("UsuarioId", (object)1);
////                                                    sqlCommand2.Parameters.AddWithValue("CdOperacao", (object)10);
////                                                    sqlCommand2.Parameters.AddWithValue("IdTipoCracha", (object)1);
////                                                    int num5 = 0;
////                                                    num5 = sqlCommand2.ExecuteNonQuery();
////                                                }
////                                                sqlDataReader.Close();
////                                            }
////                                        }
////                                        else
////                                        {
////                                            MensagensList.Add(new Mensagens(99, "Erro não especificado."));
////                                            retorno.StatusCode = 1;
////                                            retorno.StatusMessage = "Error";
////                                        }
////                                    }
////                                    else
////                                    {
////                                        MensagensList.Add(new Mensagens(99, "Erro não especificado."));
////                                        retorno.StatusCode = 1;
////                                        retorno.StatusMessage = "Error";
////                                    }
////                                    this.dr.Close();
////                                }
////                                catch (Exception ex)
////                                {
////                                    MensagensList.Add(new Mensagens(99, "Erro não especificado" + ex.Message));
////                                    retorno.StatusCode = 1;
////                                    retorno.StatusMessage = "Error";
////                                }
////                            }
////                            if (item.tipoOperacao.ToUpper() == "D")
////                            {
////                                try
////                                {
////                                    SqlCommand sqlCommand = new SqlCommand("sp_IntegraColaboradorDesativar", this.DtCon);
////                                    sqlCommand.CommandType = CommandType.StoredProcedure;
////                                    sqlCommand.Parameters.AddWithValue("IdColaborador", (object)num4);
////                                    sqlCommand.Parameters.AddWithValue("Display1", (object)"");
////                                    sqlCommand.Parameters.AddWithValue("Display2", (object)"");
////                                    if ((uint)sqlCommand.ExecuteNonQuery() <= 0U)
////                                        ;
////                                }
////                                catch (Exception ex)
////                                {
////                                    MensagensList.Add(new Mensagens(99, "Erro não especificado" + ex.Message));
////                                }
////                            }
////                            if (item.tipoOperacao.ToUpper() == "R")
////                            {
////                                try
////                                {
////                                    SqlCommand sqlCommand = new SqlCommand("sp_IntegraColaboradorAtivar", this.DtCon);
////                                    sqlCommand.CommandType = CommandType.StoredProcedure;
////                                    sqlCommand.Parameters.AddWithValue("IdColaborador", (object)num4);
////                                    sqlCommand.Parameters.AddWithValue("Display1", (object)"");
////                                    sqlCommand.Parameters.AddWithValue("Display2", (object)"");
////                                    if ((uint)sqlCommand.ExecuteNonQuery() <= 0U) { };
////                                }
////                                catch (Exception ex)
////                                {
////                                    MensagensList.Add(new Mensagens(99, "Erro não especificado" + ex.Message));
////                                }
////                            }
////                        }
////                    }
////                    this.DtCon.Close();
////                }
////                catch (Exception ex)
////                {
////                    if (MensagensList.Count == 0)
////                        MensagensList.Add(new Mensagens(99, "Erro não especificado" + ex.Message));
////                }
////            }
////            catch (Exception ex)
////            {
////                MensagensList.Add(new Mensagens(98, "Sem conexão com a base de dados"));
////            }
////        }
////        else
////        {
////            retorno.StatusCode = 1;
////            retorno.StatusMessage = "Error";
////        }
////        retorno.Result = MensagensList;
////        return retorno;
////    }
////}
///





