namespace AuthArmor.Sdk.Services._base
{
    using AuthArmor.Sdk.Exceptions;
    using IdentityModel.Client;
    using Microsoft.Extensions.Options;
    using Polly.Retry;
    using Polly;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Net;

    public abstract class AuthArmorBaseService
    {
        private readonly Uri _apiBaseUri = new Uri("https://api.autharmor.com/");
        private readonly string _oidcBaseUri = "https://login.autharmor.com";

        private string _oauthToken;
        private readonly IOptions<Infrastructure.AuthArmorConfiguration> _settings;

        protected AuthArmorBaseService(IOptions<Infrastructure.AuthArmorConfiguration> settings)
        {
            this._settings = settings;
            ValidateSettings();
        }

        protected async Task<Stream> Post(string url, string postBody, Dictionary<string, string> headers = null)
        {
            using (var httpClient = new HttpClient())
            {
                using (var postBodySC = new StringContent(postBody, System.Text.Encoding.UTF8, "application/json"))
                {

                    httpClient.BaseAddress = this._apiBaseUri;
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetAccessToken());
                    httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(GetType().Assembly.GetName().Version.ToString());
                    if (headers?.Any() == true)
                    {
                        foreach (var header in headers)
                        {
                            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    var result = await httpClient.PostAsync(url, postBodySC);

                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsStreamAsync();
                    }
                    else
                    {
                        string errContent = await result.Content.ReadAsStringAsync();
                        throw new Exceptions.AuthArmorHttpResponseException(result.StatusCode, errContent);
                    }
                }
            }
        }

        protected async Task<Stream> Put(string url, string postBody, Dictionary<string, string> headers = null)
        {
            using (var httpClient = new HttpClient())
            {
                using (var postBodySC = new StringContent(postBody, System.Text.Encoding.UTF8, "application/json"))
                {

                    httpClient.BaseAddress = this._apiBaseUri;
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetAccessToken());
                    httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(GetType().Assembly.GetName().Version.ToString());
                    if (headers?.Any() == true)
                    {
                        foreach (var header in headers)
                        {
                            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    var result = await httpClient.PutAsync(url, postBodySC);

                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsStreamAsync();
                    }
                    else
                    {
                        string errContent = await result.Content.ReadAsStringAsync();
                        throw new Exceptions.AuthArmorHttpResponseException(result.StatusCode, errContent);
                    }
                }
            }
        }

        protected async Task<Stream> Get(string url, Dictionary<string, string> headers = null)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = this._apiBaseUri;
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetAccessToken());
                httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("AuthArmor_dotnet_SDK_v3.0.2");

                if (headers?.Any() == true)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                var result = await httpClient.GetAsync(url);

                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStreamAsync();
                }
                else
                {
                    string errContent = await result.Content.ReadAsStringAsync();
                    throw new Exceptions.AuthArmorHttpResponseException(result.StatusCode, errContent);
                }
            }
        }

        protected static AsyncRetryPolicy GetAsyncRetryPolicy()
        {
            //setup polly rules
            //retry on timeout or toomanyrequests response.
            //throttle down to one request every 300ms
            return Policy.Handle<AuthArmorHttpResponseException>(ex => ex.StatusCode == HttpStatusCode.RequestTimeout)
                         .Or<AuthArmorHttpResponseException>(ex => ex.StatusCode == HttpStatusCode.TooManyRequests)
                         .RetryAsync(3, async (exception, retryCount) => await Task.Delay(300));
        }

        protected void ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(this._settings.Value.ClientId))
            {
                throw new AuthArmorException("The required value 'client_id' was not set in appSettings - Auth Armor SDK");
            }
            if (string.IsNullOrWhiteSpace(this._settings.Value.ClientSecret))
            {
                throw new AuthArmorException("The required value 'client_secret' was not set in appSettings - Auth Armor SDK");
            }
        }

        private async Task<string> GetAccessToken()
        {
            //check if we have a token value already
            if (!string.IsNullOrEmpty(_oauthToken))
            {
                try
                {
                    //token found, try to parse and get expiration value
                    var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(this._oauthToken);
                    var expire = jwtToken.ValidTo;
                    if (DateTime.UtcNow <= expire)
                    {
                        //token is not expired
                        //return token we already have
                        return this._oauthToken;
                    }
                }
                catch (Exception)
                {
                    //error getting value from current token
                    //supress error and just get a new token
                }
            }

            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(_oidcBaseUri);

                using (var _ccr = new ClientCredentialsTokenRequest()
                {
                    Address = disco.TokenEndpoint,
                    ClientId = this._settings.Value.ClientId,
                    ClientSecret = this._settings.Value.ClientSecret,
                    //Scope = selectedScopes
                })
                {
                    var tokenResponse = await client.RequestClientCredentialsTokenAsync(_ccr);

                    if (tokenResponse.IsError)
                    {
                        throw new AuthArmorException(string.Format("Error while obtaining access_tokens for client {0}. Error Message/Description: {1}", _settings.Value.ClientId, tokenResponse.Error));
                    }

                    this._oauthToken = tokenResponse.AccessToken;
                    return this._oauthToken;
                }
            }

        }
    }
}
