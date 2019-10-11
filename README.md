# TeaMonkeyFruit.IdentityTokenProvider

### A simple library for making authenticaiton requests to Okta using Client Credential Flow.

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
```
