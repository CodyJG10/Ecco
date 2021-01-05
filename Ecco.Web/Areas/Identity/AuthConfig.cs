using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecco.Web.Areas.Identity
{
    public class AuthConfig
    {
        public static IEnumerable<ApiScope> ApiScopes =>
          new List<ApiScope>
          {
                new ApiScope("api1", "Ecco Web API")
          };

        public static IEnumerable<Client> GetClients(string secret)=>
            new List<Client>
            {
                new Client
                {
                    ClientId = "ecco-mobile",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret(secret.Sha256())
                    },

                    AllowedScopes = { "api1" }
                }
            };
    }
}
