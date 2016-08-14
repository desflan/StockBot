using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace StockBot
{
    public static class Settings
    {
        public static string GetProxyDomain()
        {
            return ConfigurationManager.AppSettings["proxyDomain"];
        }

        public static string GetProxyUsername()
        {
            return ConfigurationManager.AppSettings["proxyUsername"];
        }

        public static string GetProxyPassword()
        {
            return ConfigurationManager.AppSettings["proxyPassword"];
        }

        public static string GetLuisUri()
        {
            return ConfigurationManager.AppSettings["luisUri"];
        }
        public static string GetLuisUriPreview()
        {
            return ConfigurationManager.AppSettings["luisUriPreview"];
        }
        public static string GetStockUri()
        {
            return ConfigurationManager.AppSettings["stockUri"];
        }
        public static string GetStockUriTag()
        {
            return ConfigurationManager.AppSettings["stockUriTag"];
        }
        public static bool IsPreview()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["preview"]);
        }
    }
}