using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApp_Storage_DotNet
{
    public interface IConfigurationService
    {
        Task<string> GetStorageConnectionString();
    }
    public class ConfigurationService : IConfigurationService
    {
        private static IConfiguration config;

        async private static Task Initialize()
        {
            if(config != null)
            {
                return;
            }

            var builder = new ConfigurationBuilder();

            builder.AddAzureKeyVault(
                ConfigurationManager.AppSettings["Vault"],
                ConfigurationManager.AppSettings["ClientId"],
                ConfigurationManager.AppSettings["ClientSecret"]);

            await Task.Run(() => config = builder.Build());
        }

        public async Task<string> GetStorageConnectionString()
        {
            await Initialize();

            return config[ConfigurationManager.AppSettings["ConnectionStringKey"]];
        }

        private static async Task<string> GetToken(string authority, string resource, string scope)
        {

            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(ConfigurationManager.AppSettings["ClientId"],
                        ConfigurationManager.AppSettings["ClientSecret"]);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
    }
}