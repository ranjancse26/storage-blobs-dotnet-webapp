using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApp_Storage_DotNet
{
    public interface IConfigurationService
    {
        string GetStorageConnectionString();
    }
    public class ConfigurationService : IConfigurationService
    {
        public string GetStorageConnectionString()
        {
            return ConfigurationManager.AppSettings["StorageConnectionString"].ToString();
        }
    }
}