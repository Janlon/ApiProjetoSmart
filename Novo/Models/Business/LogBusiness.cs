// Decompiled with JetBrains decompiler
// Type: WebApiBusiness.Business.LogBusiness
// Assembly: WebApiBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2441E174-C302-4098-B419-99DA0C4BEE41
// Assembly location: G:\VOPAK\Source\sam\Api_Smart\bin\WebApiBusiness.dll

using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using WebApiBusiness.App_Data;
using WebApiBusiness.Models;
using WebApiBusiness.Validation;

namespace WebApiBusiness.Business
{
  public class LogBusiness
  {
    private static LogSmartApi logSmartApi { get; set; }

    private static LogApi logApi { get; set; }

    private LogBusiness()
    {
    }

    public static bool InserirLog(string pTexto, string pMetodo)
    {
      try
      {
        LogBusiness.logSmartApi = new LogSmartApi()
        {
          texto = pTexto,
          metodo = pMetodo,
          ip = LogBusiness.GetLocalIPv4(NetworkInterfaceType.Ethernet)
        };
        if (!new LogValidator().Validate(LogBusiness.logSmartApi).IsValid)
          return false;
        LogBusiness.logSmartApi.Ambiente = "P";
        return new LogModel().InserirLog(LogBusiness.logSmartApi);
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static bool InserirLogApi(LogApi log)
    {
      try
      {
        LogBusiness.logApi = new LogApi()
        {
          texto = log.texto,
          idSessao = log.idSessao
        };
        LogBusiness.logSmartApi.Ambiente = "P";
        return new LogModel().InserirLogApi(LogBusiness.logApi);
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    internal static string GetLocalIPv4(NetworkInterfaceType _type)
    {
      string str = "";
      foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
      {
        if (networkInterface.NetworkInterfaceType == _type && networkInterface.OperationalStatus == OperationalStatus.Up)
        {
          IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
          if (ipProperties.GatewayAddresses.FirstOrDefault<GatewayIPAddressInformation>() != null)
          {
            foreach (UnicastIPAddressInformation unicastAddress in ipProperties.UnicastAddresses)
            {
              if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                str = unicastAddress.Address.ToString();
            }
          }
        }
      }
      return str;
    }
  }
}
