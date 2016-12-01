using Microsoft.Azure.KeyVault;
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
        private static string ConnectionString;

        public async Task<string> GetStorageConnectionString()
        {
            if(ConnectionString != null)
            {
                return ConnectionString;
            }

            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));

            var value = (await kv.GetSecretAsync(ConfigurationManager.AppSettings["SecretUri"])).Value;

            ConnectionString = value;

            return value;
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