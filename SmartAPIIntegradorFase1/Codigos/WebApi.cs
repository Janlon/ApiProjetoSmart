// Decompiled with JetBrains decompiler
// Type: WebApiBusiness.App_Data.WebApi
// Assembly: WebApiBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2441E174-C302-4098-B419-99DA0C4BEE41
// Assembly location: G:\VOPAK\Source\sam\Api_Smart\bin\WebApiBusiness.dll

using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace WebApiBusiness.App_Data
{
  public static class WebApi
  {
    public static string SendToWebAPI(string webAddr, string jsonMessage)
    {
      string str = "Sucesso";
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(webAddr);
        httpWebRequest.ContentType = "application/json; charset=utf-8";
        httpWebRequest.Method = "POST";
        using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
          streamWriter.Write(jsonMessage);
          streamWriter.Flush();
        }
        try
        {
          HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        }
        catch
        {
          str = "";
        }
      }
      catch (WebException ex)
      {
        str = ex.Message;
      }
      return str;
    }

    internal static string SendToWebAPI<T>(this T objToSend, string webAddr) where T : class
    {
      string str = JsonConvert.SerializeObject((object) objToSend);
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(webAddr);
      httpWebRequest.ContentType = "application/json; charset=utf-8";
      httpWebRequest.Method = "POST";
      using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
      {
        streamWriter.Write(str);
        streamWriter.Flush();
      }
      using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
        return streamReader.ReadToEnd();
    }

    internal class respostaWeb
    {
      public bool HasErrors { get; set; }

      public string Message { get; set; } = "";
    }
  }
}
