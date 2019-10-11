using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Baseproject_records_netcore2_graphql.Identity.Okta;

namespace Baseproject_records_netcore2_graphql.Identity
{
    public interface IIdentityProviderClient
    {
        Task<string> GetAccessToken();
    }

    public class IdentityProviderClient : IIdentityProviderClient
    {
        private readonly HttpClient _httpClient;
        private readonly OktaTokenService _oktaTokenService;
        
        public IdentityProviderClient(HttpClient client, IdentityConfig config)
        {
            _httpClient = client;
            
            _oktaTokenService = new OktaTokenService(_httpClient, config);
        }

        public async Task<string> GetAccessToken()
        {
            var token = await _oktaTokenService.GetToken();

            return token;
        } 
    }

    public class IdentityConfig
    {
        public string TokenUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        //Claims in format <name, value>
        public Dictionary<string, string> Claims { get; set; }
    }
    
    
}