using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WEBAPI_VOPAK.Models;

namespace WEBAPI_VOPAK
{
    public static class ApiQuery
    {
        #region Construtor e manutenção
        /// <summary>
        /// Construtor estático.
        /// </summary>
        static ApiQuery() { }
        private static ApiClient myApiClient { get; set; } = null;


        /// <summary>
        /// Propriedade de componente.
        /// </summary>
        private static ApiClient consumerApi
        {
            get
            {
                if (myApiClient == null) myApiClient = new ApiClient();
                return myApiClient;
            }
        }
        #endregion

        #region Propriedades
        private static string rotaLogin { get { return "api/Login"; } }
        private static string rotaListarPessoas(int? id) { return $"api/pessoa/{id}"; }
        private static string rotaListarUsuarios(int? Id) { return $"api/usuario/{Id}"; }
        private static string rotaListarAgendados(int? Id) { return $"api/agenda/{Id}"; }
        #endregion

        public static async Task<List<PessoaViewModel2>> GetListaPessoasAsync(int? Id = null)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new PessoaViewModel2(), rotaListarPessoas(Id));

            List<PessoaViewModel2> ret = new List<PessoaViewModel2>();
            ret = JsonConvert.DeserializeObject<List<PessoaViewModel2>>(lista);

            return ret;
        }

        public static async Task<PessoaViewModel2> GetPessoasAsync(int Id)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new PessoaViewModel2(), rotaListarPessoas(Id));
            PessoaViewModel2 ret = new PessoaViewModel2();
            ret = JsonConvert.DeserializeObject<PessoaViewModel2>(lista);
            return ret;
        }

        public static async Task<List<EquipamentoViewModel2>> GetListaEquipamentosPorSetorAsync(int Id)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new EquipamentoViewModel2(), $"api/Setor/{Id}/Equipamentos");
            List<EquipamentoViewModel2> ret = new List<EquipamentoViewModel2>();
            ret.AddRange(JsonConvert.DeserializeObject<List<EquipamentoViewModel2>>(lista));
            return ret;
        }

        public static async Task<List<CrachaViewModel2>> GetListaCredenciaisAsync(int? Id = null)
        {
            var lista = await consumerApi.Use(HttpMethod.Get, new CrachaViewModel2(), $"api/Cracha/{Id}");
            return JsonConvert
                .DeserializeObject<List<CrachaViewModel2>>(JsonConvert
                .SerializeObject(lista.Result));
        }

        public static async Task<List<EquipamentoViewModel2>> GetListaEquipamentosAsync(int? Id = null)
        {
            string lista = await consumerApi.Lista(HttpMethod.Get, new EquipamentoViewModel2(), $"api/equipamento/{Id}");
            List<EquipamentoViewModel2> ret = new List<EquipamentoViewModel2>();
            ret.AddRange(JsonConvert.DeserializeObject<List<EquipamentoViewModel2>>(lista));
            return ret;
        }

        public static async Task<EquipamentoViewModel2> GetEquipamentoAsync(int Id)
        {
            string lista = await consumerApi.Lista(HttpMethod.Get, new EquipamentoViewModel2(), $"api/equipamento/{Id}");
            List<EquipamentoViewModel2> ret = new List<EquipamentoViewModel2>();
            ret = JsonConvert.DeserializeObject<List<EquipamentoViewModel2>>(lista);
            return ret.FirstOrDefault();
        }

        public static async Task<List<EmpresaViewModel2>> GetListaEmpresasAsync(int? Id = null)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new EmpresaViewModel2(), $"api/empresa/{Id}");
            List<EmpresaViewModel2> ret = new List<EmpresaViewModel2>();
            ret.AddRange(JsonConvert.DeserializeObject<List<EmpresaViewModel2>>(lista));
            return ret;
        }

        public static async Task<CrachaViewModel2> GetCrachaPeloNumeroCredencialAsync(string cracha)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new CrachaViewModel2(), $"api/cracha/credencial?numero={cracha}");
            CrachaViewModel2 ret = new CrachaViewModel2();
            ret = JsonConvert.DeserializeObject<CrachaViewModel2>(lista);
            return ret;
        }

        public static async Task<EmpresaViewModel2> GetEmpresaPeloCrachaAsync(int crachaId)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new EmpresaViewModel2(), $"api/Cracha/{crachaId}/Empresa");
            return JsonConvert.DeserializeObject<EmpresaViewModel2>(lista);
        }

        internal static async Task<EmpresaViewModel2> GetEmpresaPeloCNPJ(string cnpj)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new EmpresaViewModel2(), $"api/Empresa/CNPJ?numero={cnpj}");
           List<EmpresaViewModel2> ret = new List<EmpresaViewModel2>();
            ret = JsonConvert.DeserializeObject<List<EmpresaViewModel2>>(lista);
            return ret.FirstOrDefault();
        }

        internal static async Task<PessoaViewModel2> GetPessoaPeloCPF(string cpf)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new PessoaViewModel2(), $"api/Pessoa/CPF?numero={cpf}");
            return JsonConvert.DeserializeObject<PessoaViewModel2>(lista);
        }

        internal static async Task<PessoaViewModel2> GetPessoaPeloCNH(string cnh)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new PessoaViewModel2(), $"api/Pessoa/CNH?numero={cnh}");
            return JsonConvert.DeserializeObject<PessoaViewModel2>(lista);
        }

        internal static async Task<PessoaViewModel2> GetPessoaPeloDOC(string doc)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new PessoaViewModel2(), $"api/Pessoa/DOC?numero={doc}");
            return JsonConvert.DeserializeObject<PessoaViewModel2>(lista);
        }

        internal static async Task<VeiculoViewModel2> GetVeiculoPelaPLaca(string placa)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new VeiculoViewModel2(), $"api/Veiculo/placa?numero={placa}");
            VeiculoViewModel2 ret = new VeiculoViewModel2();
            ret = JsonConvert.DeserializeObject<VeiculoViewModel2>(lista);
            return ret;
        }

        internal static async Task<PessoaViewModel2> GePessoaEVeiculo(string placa)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new PessoaViewModel2(), $"api/Pessoa/placa?numero={placa}");
            PessoaViewModel2 ret = new PessoaViewModel2();
            ret = JsonConvert.DeserializeObject<PessoaViewModel2>(lista);
            return ret;
        }

        internal static async Task<PessoaViewModel2> GePessoaEEmpresa(string cnpj)
        {
            var lista = await consumerApi.Lista(HttpMethod.Get, new PessoaViewModel2(), $"api/Pessoa/cnpj?numero={cnpj}");
            PessoaViewModel2 ret = new PessoaViewModel2();
            ret = JsonConvert.DeserializeObject<PessoaViewModel2>(lista);
            return ret;
        }
    }
}