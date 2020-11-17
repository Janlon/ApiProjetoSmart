// Decompiled with JetBrains decompiler
// Type: WebApiBusiness.Business.AcessoBusiness
// Assembly: WebApiBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2441E174-C302-4098-B419-99DA0C4BEE41
// Assembly location: G:\VOPAK\Source\sam\Api_Smart\bin\WebApiBusiness.dll

using FluentValidation.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using WebApiBusiness.App_Data;
using WebApiBusiness.BO;
using WebApiBusiness.Models;
using WebApiBusiness.Validation;

namespace WebApiBusiness.Business
{
  public class AcessoBusiness
  {
    private AcessoModel acessoModel { get; set; }

    public AcessoBusiness()
    {
      this.acessoModel = new AcessoModel();
    }

    public static object MotivarMotoristaArea(
      string sessao,
      int idCracha,
      string local,
      string ambiente,
      char sentido)
    {
      LogBusiness.InserirLog(JsonConvert.SerializeObject((object) new
      {
        sessao = sessao,
        idCracha = idCracha,
        local = local,
        ambiente = ambiente,
        sentido = sentido
      }), nameof (MotivarMotoristaArea));
      return new MotivarMotoristaAreaBO().MotivarMotoristaArea(sessao, idCracha, local, ambiente, sentido).Result;
    }

    public string MotivarMotorista(Acesso acesso)
    {
      return this.acessoModel.MotivarMotorista(acesso).Result;
    }

    public string FinalizaOS(Acesso acesso)
    {
      LogBusiness.InserirLog(JsonConvert.SerializeObject((object) acesso), nameof (FinalizaOS));
      return this.acessoModel.FinalizarOs(acesso).Result;
    }

    public string CancelaOS(Acesso acesso)
    {
      LogBusiness.InserirLog(JsonConvert.SerializeObject((object) acesso), nameof (CancelaOS));
      return this.acessoModel.CancelarOs(acesso).Result;
    }

    public string ValidaBDCC(Acesso acesso)
    {
      LogBusiness.InserirLog(JsonConvert.SerializeObject((object) acesso), nameof (ValidaBDCC));
      return this.acessoModel.ValidarBDCC(acesso).Result;
    }

    public object ConsultarAcesso(PontoDeControle pontoDeControle)
    {
      LogBusiness.InserirLog(JsonConvert.SerializeObject((object) pontoDeControle), nameof (ConsultarAcesso));
      try
      {
        if (pontoDeControle == null)
          return (object) new
          {
            StatusCode = 1,
            StatusMessage = "Erro(s) encontrado(s)",
            Result = new
            {
              Message = "Formatação do Objeto está invalido.",
              Code = "0096"
            }
          };
        ValidationResult validationResult = new EquipamentoValidator().Validate(pontoDeControle);
        if (!validationResult.IsValid)
          return (object) new
          {
            StatusCode = 1,
            StatusMessage = "Erro(s) encontrado(s)",
            Result = new
            {
              Message = validationResult.Errors.First<ValidationFailure>().ErrorMessage,
              Code = validationResult.Errors.First<ValidationFailure>().ErrorCode
            }
          };
        if (this.acessoModel.SelecionarEquipamento(pontoDeControle) == null)
          return (object) new
          {
            StatusCode = 1,
            StatusMessage = "Erro(s) encontrado(s)",
            Result = new
            {
              Message = "Parâmetro \"PontoControle\" não cadastrado.",
              Code = "0005"
            }
          };
        List<PontoDeControle> pontoDeControleList = this.acessoModel.ConsultarVeiculosNaArea(pontoDeControle);
        if (pontoDeControleList == null)
          return (object) new
          {
            StatusCode = 1,
            StatusMessage = "Erro(s) encontrado(s)",
            Result = new
            {
              Message = "Não existem registros de acesso para o \"PontoControle\" informado.",
              Code = "0006"
            }
          };
        List<object> objectList = new List<object>();
        foreach (PontoDeControle pontoDeControle1 in pontoDeControleList)
          objectList.Add((object) new
          {
            Cpf = pontoDeControle1.CPF,
            Nome = pontoDeControle1.Nome,
            DtAcesso = pontoDeControle1.DtAcesso.ToString("HH:mm dd/MM/yyyy")
          });
        return (object) new
        {
          StatusCode = 0,
          StatusMessage = "Executado com sucesso",
          Result = objectList
        };
      }
      catch (SqlException ex)
      {
        if (ex.Number == 1326)
          return (object) new
          {
            StatusCode = 1,
            StatusMessage = "Erro(s) encontrado(s)",
            Result = new
            {
              Message = "Falha de conexão com base de dados.",
              Code = "0098"
            }
          };
        return (object) new
        {
          StatusCode = 1,
          StatusMessage = "Erro(s) encontrado(s)",
          Result = new
          {
            Message = "Erro de banco de dados não especificado.",
            Code = "0097"
          }
        };
      }
      catch (Exception ex)
      {
        return (object) new
        {
          StatusCode = 1,
          StatusMessage = "Erro(s) encontrado(s)",
          Result = new
          {
            Message = "Erro não especificado.",
            Code = "0099"
          }
        };
      }
    }

    public bool InserirLapRequisicaoScore(int idSessao, byte[] imagem, string ambiente)
    {
      return new AcessoModel().InserirLapRequisicaoScore(idSessao, imagem, ambiente);
    }

    public object EntradaNaoAutorizada(EntradaNaoAutorizadaDTO entradaNaoAutorizadaDTO)
    {
      LogBusiness.InserirLog(JsonConvert.SerializeObject((object) entradaNaoAutorizadaDTO), nameof (EntradaNaoAutorizada));
      if (entradaNaoAutorizadaDTO == null)
        return (object) new
        {
          StatusCode = 1,
          StatusMessage = "Erro(s) encontrado(s)",
          Result = new
          {
            Message = "Formatação do Objeto está invalida.",
            Code = "0096"
          }
        };
      try
      {
        ValidationResult validationResult = new EntradaNaoAutorizadaDTOValidator().Validate(entradaNaoAutorizadaDTO);
        if (!validationResult.IsValid)
          return (object) new
          {
            StatusCode = 1,
            StatusMessage = "Erro(s) encontrado(s)",
            Result = new
            {
              Message = validationResult.Errors.First<ValidationFailure>().ErrorMessage,
              Code = validationResult.Errors.First<ValidationFailure>().ErrorCode
            }
          };
        if (new AcessoModel().EntradaNaoAutorizada(entradaNaoAutorizadaDTO))
          return (object) new
          {
            StatusCode = 0,
            StatusMessage = "Executado com sucesso",
            Result = "Registro atualizado com sucesso"
          };
        return (object) new
        {
          StatusCode = 1,
          StatusMessage = "Erro(s) encontrado(s)",
          Result = new
          {
            Message = "Nenhum registro encontrado para atualização",
            Code = "0007"
          }
        };
      }
      catch (SqlException ex)
      {
        if (ex.Number == 1326)
          return (object) new
          {
            StatusCode = 1,
            StatusMessage = "Erro(s) encontrado(s)",
            Result = new
            {
              Message = "Falha de conexão com base de dados.",
              Code = "0098"
            }
          };
        return (object) new
        {
          StatusCode = 1,
          StatusMessage = "Erro(s) encontrado(s)",
          Result = new
          {
            Message = "Erro de banco de dados não especificado.",
            Code = "0097"
          }
        };
      }
      catch (Exception ex)
      {
        return (object) new
        {
          StatusCode = 1,
          StatusMessage = "Erro(s) encontrado(s)",
          Result = new
          {
            Message = "Erro não especificado.",
            Code = "0099"
          }
        };
      }
    }

    public object LiberarBalanca(LiberarBalancaDTO liberarBalancaDTO)
    {
      LogBusiness.InserirLog(JsonConvert.SerializeObject((object) liberarBalancaDTO), nameof (LiberarBalanca));
      if (liberarBalancaDTO == null)
        return (object) new
        {
          StatusCode = 1,
          StatusMessage = "Erro(s) encontrado(s)",
          Result = new
          {
            Message = "Formatação do Objeto está invalida.",
            Code = "0096"
          }
        };
      try
      {
        ValidationResult validationResult = new LiberarBalancaDTOValidator().Validate(liberarBalancaDTO);
        if (!validationResult.IsValid)
          return (object) new
          {
            StatusCode = 1,
            StatusMessage = "Erro(s) encontrado(s)",
            Result = new
            {
              Message = validationResult.Errors.First<ValidationFailure>().ErrorMessage,
              Code = validationResult.Errors.First<ValidationFailure>().ErrorCode
            }
          };
        LiberarBalancaDTO equipamentoEth03ByBalanca = new Ethernet3Model().GetEquipamentoEth03ByBalanca(liberarBalancaDTO);
        if (equipamentoEth03ByBalanca == null)
          return (object) new
          {
            StatusCode = 1,
            StatusMessage = "Erro(s) encontrado(s)",
            Result = new
            {
              Message = "BALANÇA NÃO ENCONTRADA",
              Code = "0005"
            }
          };
        MotivacaoTemporaria motivacaoTemporaria = new MotivacaoModel().ListarMotivacaoTemporariaAtiva(new MotivacaoTemporaria()
        {
          OrdemServico = liberarBalancaDTO.NumOs
        });
        if (motivacaoTemporaria == null)
          return (object) new
          {
            StatusCode = 1,
            StatusMessage = "Erro(s) encontrado(s)",
            Result = new
            {
              Message = "Não foi encontrado uma motivação ativa para a Ordem de Serviço solicitada",
              Code = "0006"
            }
          };
        using (HttpClient httpClient = new HttpClient())
        {
          string requestUri = string.Format("{0}/api/Eth03/LiberarBalanca?sentido={1}&NumOs={2}&placa={3}&token=5a9ba14e-0aca-446c-987c-8697620d9755", (object) equipamentoEth03ByBalanca.UrlBaseService, (object) liberarBalancaDTO.Balanca.Substring(liberarBalancaDTO.Balanca.Length - 1), (object) liberarBalancaDTO.NumOs, (object) motivacaoTemporaria.Placa);
          string result = httpClient.GetAsync(requestUri).Result.Content.ReadAsStringAsync().Result;
        }
        return (object) new
        {
          StatusCode = 0,
          StatusMessage = "Executado com sucesso",
          Result = "Processo Iniciado com sucesso."
        };
      }
      catch (SqlException ex)
      {
        if (ex.Number == 1326)
          return (object) new
          {
            StatusCode = 1,
            StatusMessage = "Erro(s) encontrado(s)",
            Result = new
            {
              Message = "Falha de conexão com base de dados.",
              Code = "0098"
            }
          };
        return (object) new
        {
          StatusCode = 1,
          StatusMessage = "Erro(s) encontrado(s)",
          Result = new
          {
            Message = "Erro de banco de dados não especificado.",
            Code = "0097"
          }
        };
      }
      catch (Exception ex)
      {
        return (object) new
        {
          StatusCode = 1,
          StatusMessage = "Erro(s) encontrado(s)",
          Result = new
          {
            Message = "Erro não especificado.",
            Code = "0099"
          }
        };
      }
    }
  }
}
