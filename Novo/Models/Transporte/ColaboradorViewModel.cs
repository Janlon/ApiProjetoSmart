namespace WEBAPI_VOPAK.Models
{
	//public class Colaborador
	//   {
	//       public int idEmpresaIntegrador { get; set; } = 0;

	//       public int idColaboradorIntegrador { get; set; } = 0;

	//       public string nome { get; set; } = "";

	//       public string tipoDocumento { get; set; } = "";

	//       public string numDocumento { get; set; } = "";

	//       public string cpf { get; set; } = "";

	//       public string cnh { get; set; } = "";

	//       public string orgaoEmissorCnh { get; set; } = "";

	//       public string emissorCnh { get; set; }

	//       public string numeroCracha { get; set; } = "";

	//       public string sexo { get; set; } = "";

	//       public string dtNascimento { get; set; } = "";

	//       public string endereco { get; set; } = "";

	//       public string numEndereco { get; set; } = "";

	//       public string bairroEndereco { get; set; } = "";

	//       public string cidade { get; set; }

	//       public string uf { get; set; } = "";

	//       public string cep { get; set; } = "";

	//       public string tel { get; set; } = "";

	//       public string emailColaborador { get; set; } = "";

	//       public byte[] foto { get; set; } = new byte[0];

	//       public string Ambiente { get; set; } = "";

	//       public string tipoOperacao { get; set; } = "";
	//   }
	using System;

	public class ColaboradorViewModel
	{
		//"idEmpresaIntegrador": 9876,
		//"idColaboradorIntegrador": 1234,
		//"nome": "JOSE LUIZ DATENA",
		//"tipoDocumento": "999999",
		//"numDocumento": "sample string 5",
		//"cpf": "99u9uu999",
		//"cnh": "sample string 7",
		//"orgaoEmissorCnh": "sample string 8",
		//"emissorCnh": "sample string 9",
		//"numeroCracha": "sample string 10",
		//"sexo": "sample string 11",
		//"dtNascimento": "sample string 12",
		//"endereco": "sample string 13",
		//"numEndereco": "sample string 14",
		//"bairroEndereco": "sample string 15",
		//"cidade": "sample string 16",
		//"uf": "PP",
		//"cep": "sample string 18",
		//"tel": "sample string 19",
		//"emailColaborador": "sample string 20",
		//"foto": "apapapapapapapapa",
		//"Ambiente": "T",
		//"tipoOperacao": "I"

		public int idEmpresaIntegrador { get; set; } = 0;
		public int idColaboradorIntegrador { get; set; } = 0;
		public string nome { get; set; } = "";
		public string tipoDocumento { get; set; } = "";
		public string numDocumento { get; set; } = "";
		public string cpf { get; set; } = "";
		public string cnh { get; set; } = "";
		public string orgaoEmissorCnh { get; set; } = "";
		public string emissorCnh { get; set; } = "";
		public string numeroCracha { get; set; } = "";
		public string sexo { get; set; } = "";
		public string dtNascimento { get; set; } = "";
		public string endereco { get; set; } = "";
		public string numEndereco { get; set; } = "";
		public string bairroEndereco { get; set; } = "";
		public string cidade { get; set; } = "";
		public string uf { get; set; } = "";
		public string cep { get; set; } = "";
		public string tel { get; set; } = "";
		public string emailColaborador { get; set; } = "";
		public string foto { get; set; } = "";
		public string Ambiente { get; set; } = "";
		public string tipoOperacao { get; set; } = "";



		public int? idTipoDocumento { get; set; } = 0;
		public int? idPerfilColaborador { get; set; } = 4;
		public int? idExterno { get; set; } = 0;
		public int? idColaborador { get; set; }
		public int? idRegraAcesso { get; set; } = 0;
		public int? idTipoColaborador { get; set; } = 0;
		public int? usuarioId { get; set; } = 0;
		public int? cdOperacao { get; set; } = 0;
		
		public string nrNis { get; set; } = "";
		public string cnpj { get; set; } = "";
		public string dsObservacao { get; set; } = "";
		public string funcao { get; set; } = "";

		public bool? cdAtivo { get; set; } = true;
		public bool? cdExcluido { get; set; } = false;

		public DateTime? dtCadastro { get; set; } = null;
		public DateTime? dtAlteracao { get; set; } = null;
		public DateTime? Nascimento { get; set; } = null;
		public DateTime? dtValidadeLib { get; set; } = null;
		public DateTime? validadeDtInicio { get; set; } = null;
		public DateTime? validadeDtTermino { get; set; } = null;
	}
}
