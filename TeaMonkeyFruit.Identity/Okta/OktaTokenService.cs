using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Baseproject_records_netcore2_graphql.Identity.Okta
{
    internal interface ITokenService
    {
        Task<string> GetToken();
    }

    internal class OktaTokenService : ITokenService
    {
        private OktaToken _token = new OktaToken();

        private readonly IdentityConfig _identityConfig;
        private readonly HttpClient _httpClient;

        public OktaTokenService(HttpClient client, IdentityConfig identityConfig)
        {
            _httpClient = client;
            _identityConfig = identityConfig;
        }

        public async Task<string> GetToken()
        {
            if (_token.IsValidAndNotExpiring)
            {
                return _token.AccessToken;
            }

            _token = await GetNewAccessToken();

            return _token.AccessToken;
        }


        private async Task<OktaToken> GetNewAccessToken()
        {
            var client = _httpClient;
            var clientId = _identityConfig.ClientId;
            var clientSecret = _identityConfig.ClientSecret;
            var clientCreds = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCreds));

            var postMessage = _identityConfig.Claims;

            var request = new HttpRequestMessage(HttpMethod.Post, _identityConfig.TokenUrl)
            {
                Content = new FormUrlEncodedContent(postMessage)
            };

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var newToken = JsonConvert.DeserializeObject<OktaToken>(json);
                newToken.ExpiresAt = DateTime.UtcNow.AddSeconds(_token.ExpiresIn);

                return newToken;
            }

            throw new ApplicationException("Unable to retrieve access token from Okta.");
        }
    }
}