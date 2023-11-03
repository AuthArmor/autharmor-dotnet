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
    using System.Transactions;
    using System.Threading;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using System.Net.Http.Json;
    using System.Net.Http.Headers;

    public abstract class AuthArmorBaseService
    {
        private const string _apiBaseUri = "https://api.autharmor.com";
        private const string _oidcBaseUri = "https://login.autharmor.com";

        private static HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri(_apiBaseUri) };

        /// <remarks>
        /// Optional.
        /// </remarks>
        private readonly ILogger<AuthArmorBaseService> _logger;

        private string _oauthToken;
        private readonly IOptions<Infrastructure.AuthArmorConfiguration> _settings;

        protected AuthArmorBaseService(ILogger<AuthArmorBaseService> logger, IOptions<Infrastructure.AuthArmorConfiguration> settings)
        {
            this._logger = logger;
            this._settings = settings;
            ValidateSettings();
        }

        protected async Task<TResponse> InvokeApiAsync<TBody, TResponse>(
            HttpMethod method,
            string path,
            Dictionary<string, object> queryParameters,
            TBody body,
            string actionName,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var queryParamStrings = queryParameters
                    .Select(param => $"{Uri.EscapeDataString(param.Key)}={Uri.EscapeDataString(param.Value.ToString())}");

                var queryParamString = string.Join('&', queryParamStrings);

                var finalPath = $"{path}?${queryParamString}";

                string bodyJson = body is null ? null : JsonSerializer.Serialize(body);

                var request = new HttpRequestMessage()
                {
                    Method = method,
                    RequestUri = new Uri(finalPath),
                    Content = body is null ? null : JsonContent.Create(body)
                };

                request.Headers.UserAgent.TryParseAdd(GetType().Assembly.GetName().Version.ToString());
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

                this._logger?.LogInformation($"Starting {actionName} ({method}) from Auth Armor API");
                this._logger?.LogInformation("Calling Auth Armor API");

                this._logger?.LogDebug($"Request endpoint: {method} {finalPath}");

                var polly = GetAsyncRetryPolicy();
                var result = await polly.ExecuteAsync(() => _httpClient.SendAsync(request, cancellationToken));

                if (!result.IsSuccessStatusCode)
                {
                    this._logger?.LogInformation($"FAILED {actionName} ({method})");
                    this._logger?.LogError($"Error trying {actionName} ({method}).");
                    this._logger?.LogError($"Error code: {result.StatusCode}");

                    var errContent = await result.Content.ReadAsStringAsync();

                    this._logger?.LogError($"HTTP body: {errContent}");
                    throw new Exceptions.AuthArmorHttpResponseException(result.StatusCode, errContent);
                }

                var response = await result.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);

                this._logger?.LogInformation($"Successfully called {actionName} on Auth Armor API");

                return response;
            }
            catch (AuthArmorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation($"FAILED ${actionName} (${method})");
                this._logger?.LogError($"Error trying ${actionName} (${method}).");

                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }
        
        protected Task<TResponse> InvokeApiAsync<TResponse>(
            HttpMethod method,
            string path,
            Dictionary<string, object> queryParameters,
            string actionName,
            CancellationToken cancellationToken = default
        ) =>
            InvokeApiAsync<object, TResponse>(method, path, queryParameters, body: null, actionName, cancellationToken);

        protected Task<TResponse> GetAsync<TResponse>(string path, Dictionary<string, object> queryParameters, string actionName, CancellationToken cancellationToken = default) =>
            InvokeApiAsync<TResponse>(HttpMethod.Get, path, queryParameters, actionName, cancellationToken);

        protected Task<TResponse> PostAsync<TBody, TResponse>(string path, Dictionary<string, object> queryParameters, TBody body, string actionName, CancellationToken cancellationToken = default) =>
            InvokeApiAsync<TBody, TResponse>(HttpMethod.Post, path, queryParameters, body, actionName, cancellationToken);

        protected Task<TResponse> PutAsync<TBody, TResponse>(string path, Dictionary<string, object> queryParameters, TBody body, string actionName, CancellationToken cancellationToken = default) =>
            InvokeApiAsync<TBody, TResponse>(HttpMethod.Put, path, queryParameters, body, actionName, cancellationToken);

        protected Task<TResponse> DeleteAsync<TResponse>(string path, Dictionary<string, object> queryParameters, string actionName, CancellationToken cancellationToken = default) =>
            InvokeApiAsync<TResponse>(HttpMethod.Delete, path, queryParameters, actionName, cancellationToken);

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
