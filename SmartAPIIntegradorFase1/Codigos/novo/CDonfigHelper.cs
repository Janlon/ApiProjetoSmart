using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SmartAPIIntegradorFase1.Codigos.novo
{
    public static class ConfigHelper
    {
        public static string Get(string key, string defaultvalue)
        {
            string ret = defaultvalue;
            if (ConfigurationManager.AppSettings[key] != null)
                ret = ConfigurationManager.AppSettings[key];
            return ret;
        }
    }
}