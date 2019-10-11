# TeaMonkeyFruit.IdentityTokenProvider

### A simple library for making authenticaiton requests to Okta using Client Credential Flow.

When a token is requested for the first time a request is made to the Okta auth server for a Jwt access token. If a token is requested a second time the same token stored in the client is returned without making a request to the auth server. On subsequent requests the same token continues to be provided until 30 seconds before it's expiration, at which time a new token is requested and stored from the Okta auth server.

Code example:
```
var tokenService = new IdentityProviderClient(
                new HttpClient(),
                new IdentityConfig
                {
                    TokenUrl = $"https://{yourOktaDomain}/oauth2/{authServerId}/v1/token",
                    ClientId = $"{clientId}",
                    ClientSecret = $"{clientSecret}",
                    Claims = new Dictionary<string, string>
                    {
                        {"grant_type", "client_credentials"},
                        {"scope", "access_token"} //This is a custom scope and is user defined in Okta Auth Server UI
                    }
                });

var token = await tokenService.GetAccessToken();

```
