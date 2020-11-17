using Dapper;
using Newtonsoft.Json;
using SmartAPIIntegradorFase1.Codigos.novo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Models
{
    public class AcessoModel : BaseModel
    {
        private new StringBuilder StrSql = new StringBuilder();

        public async Task<string> MotivarMotorista(Acesso acesso)
        {
            string content = JsonConvert.SerializeObject((object)new Dictionary<string, string>()
            {
                {"Ambiente",acesso.Ambiente == "" ? "P" : acesso.Ambiente},
                {"area",Convert.ToString(acesso.area)},
                {"bairroEndereco",acesso.bairroEndereco},
                {"bairroEnderecoMotorista",acesso.bairroEnderecoMotorista},
                {"cep",acesso.cep},
                {"cepMotorista",acesso.cepMotorista},
                {"cidade",acesso.cidade},
                {"cidadeMotorista",acesso.cidadeMotorista},
                {"cnh",acesso.cnh},
                {"cnpj",acesso.cnpj},
                {"cpf",acesso.cpf},
                {"dtNascimento",Convert.ToString(acesso.dtNascimento)},
                {"emailColaborador",acesso.emailColaborador},
                {"emailRepresentante",acesso.emailRepresentante},
                {"emissorCnh",acesso.emissorCnh},
                {"endereco",acesso.endereco},
                {"enderecoMotorista",acesso.enderecoMotorista},
                {"foto",acesso.foto},
                {"idColaboradorIntegrador",Convert.ToString(acesso.idColaboradorIntegrador)},
                {"idEmpresaIntegrador",Convert.ToString(acesso.idEmpresaIntegrador)},
                {"nome",acesso.nome},
                {"nomeFantasia",acesso.nomeFantasia},
                {"numDocumento",acesso.numDocumento},
                {"numEndereco",acesso.numEndereco},
                {"numEnderecoMotorista",acesso.numEnderecoMotorista},
                {"numeroCracha",acesso.numeroCracha},
                {"numOs",Convert.ToString(acesso.numOs)},
                {"orgaoEmissorCnh",acesso.orgaoEmissorCnh},
                {"placa",acesso.placa},
                {"razaoSocial",acesso.razaoSocial},
                {"representante",acesso.representante},
                {"sexo",acesso.sexo},
                {"tel",acesso.tel},
                {"tipoDocumento",acesso.tipoDocumento },
                {"tipoOperacao",acesso.tipoOperacao },
                {"uf",acesso.uf },
                {"ufMotorista",acesso.ufMotorista}
            });
            string raiz = System.Web.HttpContext.Current.Request.Path.Substring(1);
            string str = string.Format("http://{0}", ConfigHelper.Get("ApiMotivarMotorista",raiz));
            Encoding utF8 = Encoding.UTF8;
            StringContent stringContent = new StringContent(content, utF8, "application/json");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(str)
            };
            string result;
            using (HttpClient client = new HttpClient())
                result = (await client.PostAsync(str, (HttpContent)stringContent).ConfigureAwait(false)).Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> ValidarBDCC(Acesso acesso)
        {
            string content = JsonConvert.SerializeObject((object)new Dictionary<string, string>()
            {
                {"ambiente",acesso.Ambiente == "" ? "P" : acesso.Ambiente },
                {"numDocumento",Convert.ToString(acesso.numDocumento)},
                {"tipoDocumento",Convert.ToString(acesso.tipoDocumento)},
                {"cpf",Convert.ToString(acesso.cpf)}
            });
            string str = string.Format("http://{0}", (object)ConfigurationManager.AppSettings["ApiBDCC"].ToString());
            Encoding utF8 = Encoding.UTF8;
            StringContent stringContent = new StringContent(content, utF8, "application/json");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(str)
            };
            string result;
            using (HttpClient client = new HttpClient())
                result = (await client.PostAsync(str, (HttpContent)stringContent).ConfigureAwait(false)).Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> CancelarOs(Acesso acesso)
        {
            string content = JsonConvert.SerializeObject((object)new Dictionary<string, string>()
            {
                {"ambiente",acesso.Ambiente == "" ? "P" : acesso.Ambiente },
                {"numOs",Convert.ToString(acesso.numOs)}
            });
            string str = string.Format("http://{0}", (object)ConfigurationManager.AppSettings["ApiCancelarOs"].ToString());
            Encoding utF8 = Encoding.UTF8;
            StringContent stringContent = new StringContent(content, utF8, "application/json");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(str)
            };
            string result;
            using (HttpClient client = new HttpClient())
                result = (await client.PostAsync(str, (HttpContent)stringContent).ConfigureAwait(false)).Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> FinalizarOs(Acesso acesso)
        {
            string content = JsonConvert.SerializeObject((object)new Dictionary<string, string>()
            {
                {"ambiente", acesso.Ambiente == "" ? "P" : acesso.Ambiente },
                {"numOs",Convert.ToString(acesso.numOs)}
            });

            string str = string.Format("http://{0}", (object)ConfigurationManager.AppSettings["ApiFinalizarOs"].ToString());
            Encoding utF8 = Encoding.UTF8;
            StringContent stringContent = new StringContent(content, utF8, "application/json");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(str)
            };
            string result;
            using (HttpClient client = new HttpClient())
                result = (await client.PostAsync(str, (HttpContent)stringContent).ConfigureAwait(false)).Content.ReadAsStringAsync().Result;
            return result;
        }

        public PontoDeControle SelecionarEquipamento(PontoDeControle equipamento)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@DsEquipamento", (object)equipamento.PontoControle.Trim(), new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            this.StrSql.Append("SELECT DsEquipamento as PontoControle FROM TB_EQUIPAMENTO WHERE DsEquipamento = @DsEquipamento");
            return this.Query<PontoDeControle>((Basica)equipamento, this.StrSql.ToString(), (object)dynamicParameters).FirstOrDefault<PontoDeControle>();
        }

        public PontoDeControle ConsultarAcesso(PontoDeControle equipamento)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            StringBuilder stringBuilder = new StringBuilder();
            dynamicParameters.Add("@DsEquipamento", (object)equipamento.PontoControle.Trim(), new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            stringBuilder.Append("select top 1 colaborador.CdCpf as CPF, colaborador.DsColaborador as Nome");
            stringBuilder.Append("\t\t\t from TB_Acesso acesso");
            stringBuilder.Append("\t\t\t join TB_Cracha cracha on acesso.IdCredencial = cracha.CdCracha");
            stringBuilder.Append("\t\t\t join TB_Colaborador colaborador on cracha.IdColaborador = colaborador.IdColaborador");
            stringBuilder.Append("\t\t\t join TB_Equipamento equipamento on equipamento.IdEquipamento =  acesso.IdEquipamento");
            stringBuilder.Append("\t\t\t where acesso.CdAcao in (2,81,191,192,201) and equipamento.DsEquipamento = @DsEquipamento");
            stringBuilder.Append("\t\t\t order by acesso.DtAcesso desc");
            return this.Query<PontoDeControle>((Basica)equipamento, stringBuilder.ToString(), (object)dynamicParameters).FirstOrDefault<PontoDeControle>();
        }

        public List<PontoDeControle> ConsultarVeiculosNaArea(
          PontoDeControle equipamento)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            StringBuilder stringBuilder = new StringBuilder();
            dynamicParameters.Add("@DsEquipamento", (object)equipamento.PontoControle.Trim(), new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            stringBuilder.Append(" SELECT A.DTACESSO,C.CDCPF AS CPF,C.DSCOLABORADOR AS NOME ");
            stringBuilder.Append(" FROM ");
            stringBuilder.Append(" (TB_DuploAcesso A INNER JOIN TB_CRACHA B ON A.IDCREDENCIAL = B.CDCRACHA) ");
            stringBuilder.Append(" INNER JOIN TB_COLABORADOR C ON B.IDCOLABORADOR = C.IDCOLABORADOR ");
            stringBuilder.Append(" WHERE CDSENTIDO = 'E' AND IDEQUIPAMENTO IN ");
            stringBuilder.Append(" (SELECT IDEQUIPAMENTO FROM TB_EQUIPAMENTO WHERE IDLOCAL = ");
            stringBuilder.Append(" (SELECT IDLOCAL FROM TB_EQUIPAMENTO WHERE DSEQUIPAMENTO = @DsEquipamento) AND FlagBalanca = 1)  ");
            stringBuilder.Append(" AND DATEDIFF(HH, A.DTACESSO, getdate()) < 12 ");
            stringBuilder.Append(" ORDER BY DTACESSO ");
            return this.Query<PontoDeControle>((Basica)equipamento, stringBuilder.ToString(), (object)dynamicParameters);
        }

        public bool EntradaNaoAutorizada(EntradaNaoAutorizadaDTO entradaNaoAutorizadaDTO)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            int num = 0;
            dynamicParameters.Add("@Cpf", (object)entradaNaoAutorizadaDTO.CPF, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@numOs", (object)entradaNaoAutorizadaDTO.NumOs, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            for (int index = 0; index <= 3; ++index)
            {
                this.StrSql = new StringBuilder();
                this.StrSql.Append("\tUPDATE TB_MotivacaoTemporaria SET OrdemServico = 0, FlSaida = 1, DtSaida =  getDate() where Id = (");
                this.StrSql.Append("\tSELECT TOP 1 A.Id from TB_MotivacaoTemporaria A ");
                this.StrSql.Append("\tJOIN TB_Colaborador B ON A.IdColaborador = B.IdColaborador ");
                this.StrSql.Append(" WHERE A.FlSaida = 0 AND ");
                this.StrSql.Append("\tA.dtCancelamento IS NULL  AND ");
                this.StrSql.Append("\tA.DtSaida IS NULL AND ");
                if (index == 0)
                    this.StrSql.Append("\tA.OrdemServico =  @numOs");
                else if (index == 1)
                    this.StrSql.Append("\tB.CdCpf =  @Cpf");
                else if (index == 2)
                {
                    dynamicParameters.Add("@Placa", (object)entradaNaoAutorizadaDTO.PlacaOs, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
                    this.StrSql.Append("\tA.Placa = @Placa ");
                }
                else if (index == 3)
                {
                    dynamicParameters.Add("@Placa", (object)entradaNaoAutorizadaDTO.PlacaOcr, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
                    this.StrSql.Append("\tA.Placa = @Placa ");
                }
                this.StrSql.Append(")");
                num = this.Execute((Basica)entradaNaoAutorizadaDTO, this.StrSql.ToString(), (object)dynamicParameters);
                if (num > 0)
                    break;
            }
            return num > 0;
        }

        public bool InserirLapRequisicaoScore(int idSessao, byte[] imagem, string ambiente)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@ID_SECAO", (object)idSessao, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@ID_LAP_TIPO_REQUISICAO", (object)2, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@NR_POSICAO_CAMERA_TOTEM", (object)1, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@VL_SCORE", (object)0.0, new DbType?(DbType.Double), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@ID", (object)0, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@IMAGEM", (object)imagem, new DbType?(DbType.Binary), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            this.StrSql.Append(" INSERT INTO [dbo].[TB_LAP_REQUISICAO_SCORE] ");
            this.StrSql.Append("            ([ID_SECAO] ");
            this.StrSql.Append("            ,[ID_LAP_TIPO_REQUISICAO] ");
            this.StrSql.Append("            ,[NR_POSICAO_CAMERA_TOTEM] ");
            this.StrSql.Append("            ,[VL_SCORE] ");
            this.StrSql.Append("            ,[ID] ");
            this.StrSql.Append("            ,[DT_REQUISICAO_SCORE] ");
            this.StrSql.Append("            ,[ST_ANALISADO] ");
            this.StrSql.Append("            ,[ST_BIOMETRIA] ");
            this.StrSql.Append("            ,[VL_SCORE_OUT] ");
            this.StrSql.Append("            ,[IMAGEM]) ");
            this.StrSql.Append("      VALUES ");
            this.StrSql.Append("            (@ID_SECAO, ");
            this.StrSql.Append("             @ID_LAP_TIPO_REQUISICAO,");
            this.StrSql.Append("             @NR_POSICAO_CAMERA_TOTEM, ");
            this.StrSql.Append("             @VL_SCORE, ");
            this.StrSql.Append("             @ID, ");
            this.StrSql.Append("             getdate(),");
            this.StrSql.Append("             0, ");
            this.StrSql.Append("             0, ");
            this.StrSql.Append("             0, ");
            this.StrSql.Append("             @IMAGEM) ");
            return this.Execute(new Basica()
            {
                Ambiente = ambiente
            }, this.StrSql.ToString(), (object)dynamicParameters) > 0;
        }
    }
}