// Decompiled with JetBrains decompiler
// Type: WebApiBusiness.App_Data.CameraUtil
// Assembly: WebApiBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2441E174-C302-4098-B419-99DA0C4BEE41
// Assembly location: G:\VOPAK\Source\sam\Api_Smart\bin\WebApiBusiness.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WebApiBusiness.Business;
using WebApiBusiness.Models;

namespace WebApiBusiness.App_Data
{
  public class CameraUtil
  {
    public static string GetFrame(CameraEquipamento camera)
    {
      NetworkCredential networkCredential = new NetworkCredential(camera.Conta, camera.Senha);
      HttpClientHandler httpClientHandler = new HttpClientHandler();
      httpClientHandler.Credentials = networkCredential == null ? (ICredentials) null : (ICredentials) networkCredential;
      httpClientHandler.UseDefaultCredentials = networkCredential == null;
      httpClientHandler.UseProxy = false;
      HttpClient httpClient = new HttpClient((HttpMessageHandler) httpClientHandler);
      httpClient.BaseAddress = new Uri(camera.DsLinkCameraVideo);
      httpClient.Timeout = new TimeSpan(0, 0, 0, 2);
      byte[] result = httpClient.GetByteArrayAsync(camera.DsLinkCameraVideo).Result;
      httpClient.Dispose();
      httpClientHandler.Dispose();
      return Convert.ToBase64String(result);
    }

    public static async Task<string> GetCarmenApi(string base64)
    {
      string str1;
      try
      {
        string content = JsonConvert.SerializeObject((object) JsonConvert.SerializeObject((object) new Dictionary<string, string>()
        {
          {
            "lane",
            "31"
          },
          {
            "id",
            "1"
          },
          {
            "timeout",
            "1000"
          },
          {
            "bytes",
            base64
          }
        }));
        string str2 = string.Format("http://{0}", (object) ConfigurationManager.AppSettings["ServidorLAP"].ToString());
        Encoding utF8 = Encoding.UTF8;
        StringContent stringContent = new StringContent(content, utF8, "application/json");
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
        {
          Method = HttpMethod.Post,
          RequestUri = new Uri(str2)
        };
        using (HttpClient client = new HttpClient())
        {
          HttpResponseMessage httpResponseMessage = await client.PostAsync(str2, (HttpContent) stringContent).ConfigureAwait(false);
          if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
            throw new Exception("PlateNotFound");
          string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
          string str3 = result.Substring(result.IndexOf("{"));
          while (!str3.EndsWith("}"))
            str3 = str3.Substring(0, str3.Length - 1);
          str1 = str3.Replace("\\", "");
        }
      }
      catch (Exception ex)
      {
        throw;
      }
      return str1;
    }

    public static Carmen GetCarmenLevenstein(List<string> imgsBase64, Captura captura)
    {
      Carmen carmen1 = new Carmen();
      Carmen carmen2 = new Carmen();
      string str = "";
      LogApi logApi = new LogApi();
      foreach (string base64 in imgsBase64)
      {
        try
        {
          int totalConfidence = carmen1.totalConfidence;
          carmen2 = new JavaScriptSerializer().Deserialize<Carmen>(CameraUtil.GetCarmenApi(base64).Result);
          if (carmen2.totalConfidence >= totalConfidence)
          {
            carmen1 = carmen2;
            carmen1.base64 = base64;
          }
          str = "ok";
        }
        catch (Exception)
        {
          str = str == "ok" ? "ok" : "error";
        }
        LogBusiness.InserirLogApi(new LogApi()
        {
          texto = ">Captura OCR Placa: " + carmen1.plate,
          idSessao = captura.IdSecao.ToString()
        });
        if (carmen1.plate != null && !string.IsNullOrEmpty(carmen1.plate) && 
            (captura.NumOs !="" && 
            carmen1 == carmen2))
        {
          string placa = new CapturaBusiness().PesquisarMotivacaoBusiness(captura).Placa;
          float num = carmen1.plate.Compare(ref placa);
          if ((double) num >= (double) carmen1.score)
          {
            carmen1.score = num;
            carmen1.ConfirmedPlate = placa;
          }
        }
        if (carmen1.plate == null || string.IsNullOrEmpty(carmen1.plate))
        {
          carmen1.confidence = "0";
          carmen1.proc = "";
          carmen1.ptype = "";
          carmen1.score = 0.0f;
        }
      }
      carmen1.score = (float) ((double) carmen1.score / (double) byte.MaxValue * 100.0);
      LogBusiness.InserirLogApi(new LogApi()
      {
        texto = ">Score: " + (object) carmen1.score,
        idSessao = captura.IdSecao.ToString()
      });
      if (str == "ok")
        return carmen1;
      return (Carmen) null;
    }

    public static List<string> CapturaFrames(List<CameraEquipamento> listCamBalanca, int framesPorCamera, List<string> imgsBase64)
    {
      if (framesPorCamera <= 0)
        return imgsBase64;
      for (int index = 0; index < listCamBalanca.Count; ++index)
      {
        try
        {
          imgsBase64.Add(CameraUtil.GetFrame(listCamBalanca[index]));
        }
        catch (Exception ex)
        {
        }
      }
      return CameraUtil.CapturaFrames(listCamBalanca, framesPorCamera - 1, imgsBase64);
    }
  }
}
