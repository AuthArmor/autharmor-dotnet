namespace AuthArmor.Sdk.Services.Auth
{    
    using AuthArmor.Sdk.Services._base;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;    
    using System;    
    using System.Net.Http;
    using System.Text.Json.Serialization;
    using System.Text.Json;    
    using System.Threading.Tasks;
    using Polly.Retry;

    public class AuthService : AuthArmorBaseService
    {
        //authenticator - auth
        private const string _startAuthenticatorRequestPath =                           "/v3/auth/authenticator/start";
        private const string _validateAuthenticatorAuthPath =                           "/v3/auth/authenticator/validate/";

        //magiclink emails - auth
        private const string _startMagiclinkEmailRequestPath =                          "/v3/auth/magiclink_email/start";
        private const string _validateMagiclinkEmailPath =                              "/v3/auth/magiclink_email/validate/";

        //webauthn - auth
        private const string _startWebAuthnRequestPath =                                "/v3/auth/webauthn/start";
        private const string _finishWebAuthnRequestPath =                               "/v3/auth/webauthn/finish";
        private const string _validateWebAuthnPath =                                    "v3/auth/webauthn/validate/";

        //general auth routes
        private const string _getAuthRequestInfoPath =                                  "/v3/auth/{0}";

        private readonly ILogger<AuthService>? _logger;

        public AuthService(ILogger<AuthService> logger, IOptions<Infrastructure.AuthArmorConfiguration> settings)
            : base(settings)
        {
            this._logger = logger;
            ValidateSettings();
        }

        public AuthService(IOptions<Infrastructure.AuthArmorConfiguration> settings)
            : base(settings)
        {
            ValidateSettings();
        }

        /// <summary>
        /// Gets Auth Information by auth histoy id
        /// </summary>
        /// <param name="request">Auth History ID (Guid)</param>
        /// <returns>Auth information for the given auth_request_id</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.GetAuthInfoResponse> GetAuthInfo(Models.Auth.GetAuthInfoRequest request)
        {
            try
            {
                //get retry policy
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();


                this._logger?.LogInformation("Starting request to Get Auth Info from Auth Armor API");
                this._logger?.LogDebug("=== Auth Request Parms ===");
                this._logger?.LogDebug("Auth History Id: {0}", request.AuthHistory_Id);
                this._logger?.LogInformation("Getting Auth Info from API");

                Models.Auth.GetAuthInfoResponse getAuthInfoResponse;

                string getPath = string.Format(_getAuthRequestInfoPath, request.AuthHistory_Id);

                //make api call using retry policy
                using (var responseBody = await polly.ExecuteAsync(() => Get(getPath)))
                {
                    //serialize response into object
                    getAuthInfoResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.Auth.GetAuthInfoResponse>(responseBody);

                    this._logger?.LogInformation("Successfully Finished Call to Get Auth Info from Auth Armor API");
                    return getAuthInfoResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while getting Auth Info from Auth Armor API");
                this._logger?.LogError(ex, "Error while getting Auth Info from Auth Armor API");                
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while getting Auth Info from Auth Armor API");
                this._logger?.LogError(ex, "Error while getting Auth Info from Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while getting Auth Info from Auth Armor API");
                this._logger?.LogError(ex, "Error while getting Auth Info from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while getting Auth Info from Auth Armor API");
                this._logger?.LogError(ex, "Error while getting Auth Info from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Validates an Auth Armor Authenticator Auth request
        /// </summary>
        /// <param name="request">Object containing the auth_request_id and auth_validation_token</param>
        /// <returns>Validation Information for Authenticator Auth request</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.Authenticator.ValidateAuthenticatorAuthResponse> ValidateAuthenticatorAuth(Models.Auth.Authenticator.ValidateAuthenticatorAuthRequest request)
        {
            try
            {
                //get retry policy
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();


                this._logger?.LogInformation("Starting Validation of Authenticator Auth from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"Auth History Id: {request.AuthRequestId}");

                //serilize request object to json
                var postContent = System.Text.Json.JsonSerializer.Serialize(request);


                Models.Auth.Authenticator.ValidateAuthenticatorAuthResponse validateAuthenticatorAuthResponse;

                //make api call using retry policy
                using (var responseBody = await polly.ExecuteAsync(() => Post(_validateAuthenticatorAuthPath, postContent)))                
                {
                    //setup json options to enable enum Serialization. 
                    var options = new JsonSerializerOptions();
                    options.Converters.Add(new JsonStringEnumConverter());

                    //serialize response into object
                    validateAuthenticatorAuthResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.Auth.Authenticator.ValidateAuthenticatorAuthResponse>(responseBody, options);

                    this._logger?.LogInformation("Successfully Finished Call to Validate Authenticator Auth from the Auth Armor API ");
                    return validateAuthenticatorAuthResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while validating Auth Armor Authenticator Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while validating Auth Armor Authenticator Auth Request from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while validating Auth Armor Authenticator Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while validating Auth Armor Authenticator Auth Request from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while validating Auth Armor Authenticator Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while validating Auth Armor Authenticator Auth Request from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while validating Auth Armor Authenticator Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while validating Auth Armor Authenticator Auth Request from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Starts a new Auth Armor Authenticator auth request
        /// </summary>
        /// <param name="request">StartAuthenticatorAuthRequest</param>
        /// <returns>StartAuthenticatorAuthResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.Authenticator.StartAuthenticatorAuthResponse> StartAuthenticatorAuth(Models.Auth.Authenticator.StartAuthenticatorAuthRequest request)
        {
            try
            {
                //get retry policy
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();


                this._logger?.LogInformation("Starting Auth Armor Authenticator Auth Request from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"User Id: {request.UserId}");
                this._logger?.LogDebug($"Username: {request.Username}");

                //serilize request object to json
                var postContent = System.Text.Json.JsonSerializer.Serialize(request);


                Models.Auth.Authenticator.StartAuthenticatorAuthResponse startAuthenticatorAuthResponse;

                //make api call using retry policy
                using (var responseBody = await polly.ExecuteAsync(() => Post(_startAuthenticatorRequestPath, postContent)))                
                {
                    //serialize response into object
                    startAuthenticatorAuthResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.Auth.Authenticator.StartAuthenticatorAuthResponse>(responseBody);

                    this._logger?.LogInformation("Successfully Finished Call to Start Auth Armor Authenticator Auth Request from the Auth Armor API");
                    return startAuthenticatorAuthResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while starting Auth Armor Authenticator Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while starting Auth Armor Authenticator Auth Request from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while starting Auth Armor Authenticator Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while starting Auth Armor Authenticator Auth Request from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while starting Auth Armor Authenticator Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while starting Auth Armor Authenticator Auth Request from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while starting Auth Armor Authenticator Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while starting Auth Armor Authenticator Auth Request from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Starts a new WebAuthn auth request
        /// </summary>
        /// <param name="request">StartWebAuthnAuthRequest</param>
        /// <returns>StartWebAuthnAuthResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.WebAuthn.StartWebAuthnAuthResponse> StartWebAuthnAuth(Models.Auth.WebAuthn.StartWebAuthnAuthRequest request)
        {
            try
            {
                //get retry policy
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();


                this._logger?.LogInformation("Starting WebAuthn Auth Request from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"User Id: {request.UserId}");
                this._logger?.LogDebug($"Username: {request.Username}");

                //serilize request object to json
                var postContent = System.Text.Json.JsonSerializer.Serialize(request);


                Models.Auth.WebAuthn.StartWebAuthnAuthResponse startWebAuthnAuthResponse;

                //make api call using retry policy
                using (var responseBody = await polly.ExecuteAsync(() => Post(_startWebAuthnRequestPath, postContent)))                
                {
                    //serialize response into object
                    startWebAuthnAuthResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.Auth.WebAuthn.StartWebAuthnAuthResponse>(responseBody);

                    this._logger?.LogInformation("Successfully Finished Call Starting WebAuthn Auth Request from the Auth Armor API");
                    return startWebAuthnAuthResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting WebAuthn Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting WebAuthn Auth Request from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting WebAuthn Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting WebAuthn Auth Request from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting WebAuthn Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting WebAuthn Auth Request from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting WebAuthn Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting WebAuthn Auth Request from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Finishes a WebAuthn Auth Request
        /// </summary>
        /// <param name="request">FinishWebAuthnAuthRequest</param>
        /// <returns>FinishWebAuthnAuthResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.WebAuthn.FinishWebAuthnAuthResponse> FinishWebAuthnAuth(Models.Auth.WebAuthn.FinishWebAuthnAuthRequest request)
        {
            try
            {
                //get retry policy
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();


                this._logger?.LogInformation("Starting Finish WebAuthn Auth Request from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"Auth Request Id: {request.AuthRequest_Id}");
                this._logger?.LogDebug($"WebAuthn Client Id: {request.WebAuthnClient_Id}");

                //serilize request object to json
                var postContent = System.Text.Json.JsonSerializer.Serialize(request);


                Models.Auth.WebAuthn.FinishWebAuthnAuthResponse finishWebAuthnAuthResponse;

                //make api call using retry policy
                using (var responseBody = await polly.ExecuteAsync(() => Post(_finishWebAuthnRequestPath, postContent)))                
                {
                    //serialize response into object
                    finishWebAuthnAuthResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.Auth.WebAuthn.FinishWebAuthnAuthResponse>(responseBody);

                    this._logger?.LogInformation("Successfully Finished Call to Finish WebAuthn Auth Request from the Auth Armor API");
                    return finishWebAuthnAuthResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Finishing WebAuthn Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Finishing WebAuthn Auth Request from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Finishing WebAuthn Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Finishing WebAuthn Auth Request from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Finishing WebAuthn Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Finishing WebAuthn Auth Request from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Finishing WebAuthn Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Finishing WebAuthn Auth Request from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Starts a Magiclink email Auth Request
        /// </summary>
        /// <param name="request">StartMagiclinkEmailAuthRequest</param>
        /// <returns>StartMagiclinkEmailAuthResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.MagiclinkEmail.StartMagiclinkEmailAuthResponse> StartMagiclinkEmailAuth(Models.Auth.MagiclinkEmail.StartMagiclinkEmailAuthRequest request)
        {
            try
            {
                //get retry policy
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();


                this._logger?.LogInformation("Starting Start Magiclink Email Auth Request from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"User Id: {request.UserId}");
                this._logger?.LogDebug($"Username: {request.Username}");

                //serilize request object to json
                var postContent = System.Text.Json.JsonSerializer.Serialize(request);


                Models.Auth.MagiclinkEmail.StartMagiclinkEmailAuthResponse startMagiclinkEmailAuthResponse;

                //make api call using retry policy
                using (var responseBody = await polly.ExecuteAsync(() => Post(_startMagiclinkEmailRequestPath, postContent)))                
                {
                    //serialize response into object
                    startMagiclinkEmailAuthResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.Auth.MagiclinkEmail.StartMagiclinkEmailAuthResponse>(responseBody);

                    this._logger?.LogInformation("Successfully Finished Call Starting Start Magiclink Email Auth Request from the Auth Armor API");
                    return startMagiclinkEmailAuthResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while trying to Start Magiclink Email Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while trying to Start Magiclink Email Auth Request from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while trying to Start Magiclink Email Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while trying to Start Magiclink Email Auth Request from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while trying to Start Magiclink Email Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while trying to Start Magiclink Email Auth Request from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while trying to Start Magiclink Email Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while trying to Start Magiclink Email Auth Request from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Validates a Magiclink Email Auth Request
        /// </summary>
        /// <param name="request">ValidateMagiclinkEmailAuthRequest</param>
        /// <returns>ValidateMagiclinkEmailAuthResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.MagiclinkEmail.ValidateMagiclinkEmailAuthResponse> ValidateMagiclinkEmailAuth(Models.Auth.MagiclinkEmail.ValidateMagiclinkEmailAuthRequest request)
        {
            try
            {
                //get retry policy
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();


                this._logger?.LogInformation("Starting Validate Magiclink Email Auth Request from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"Auth History Id: {request.AuthRequestId}");

                //serilize request object to json
                var postContent = System.Text.Json.JsonSerializer.Serialize(request);


                Models.Auth.MagiclinkEmail.ValidateMagiclinkEmailAuthResponse validateMagiclinkEmailAuthResponse;

                //make api call using retry policy
                using (var responseBody = await polly.ExecuteAsync(() => Post(_validateMagiclinkEmailPath, postContent)))                
                {
                    //setup json options to enable enum Serialization. 
                    var options = new JsonSerializerOptions();
                    options.Converters.Add(new JsonStringEnumConverter());

                    //serialize response into object
                    validateMagiclinkEmailAuthResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.Auth.MagiclinkEmail.ValidateMagiclinkEmailAuthResponse>(responseBody, options);

                    this._logger?.LogInformation("Successfully Finished validate Magiclink Email call from Auth Armor API");
                    return validateMagiclinkEmailAuthResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Validating Magiclink Email Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Validating Magiclink Email Auth Request from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Validating Magiclink Email Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Validating Magiclink Email Auth Request from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Validating Magiclink Email Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Validating Magiclink Email Auth Request from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Validating Magiclink Email Auth Request from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Validating Magiclink Email Auth Request from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Validates a WebAuthn Auth Request
        /// </summary>
        /// <param name="request">ValidateWebAuthnAuthRequest</param>
        /// <returns>ValidateWebAuthnAuthResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.WebAuthn.ValidateWebAuthnAuthResponse> ValidateWebAuthn(Models.Auth.WebAuthn.ValidateWebAuthnAuthRequest request)
        {
            try
            {
                //get retry policy
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();


                this._logger?.LogInformation("Starting Validate WebAuthn call to Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"Auth History Id: {request.AuthRequestId}");

                //serilize request object to json
                var postContent = System.Text.Json.JsonSerializer.Serialize(request);


                Models.Auth.WebAuthn.ValidateWebAuthnAuthResponse validateWebAuthnResponse;

                //make api call using retry policy
                using (var responseBody = await polly.ExecuteAsync(() => Post(_validateWebAuthnPath, postContent)))                
                {
                    //setup json options to enable enum Serialization. 
                    var options = new JsonSerializerOptions();
                    options.Converters.Add(new JsonStringEnumConverter());

                    //serialize response into object
                    validateWebAuthnResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.Auth.WebAuthn.ValidateWebAuthnAuthResponse>(responseBody, options);

                    this._logger?.LogInformation("Successfully Finished Validate WebAuthn call to Auth Armor API");
                    return validateWebAuthnResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Failed while validating WebAuthn Auth Request via the Auth Armor API");
                this._logger?.LogError(ex, "Error while validating WebAuthn Auth Request via the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Failed while validating WebAuthn Auth Request via the Auth Armor API");
                this._logger?.LogError(ex, "Error while validating WebAuthn Auth Request via the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Failed while validating WebAuthn Auth Request via the Auth Armor API");
                this._logger?.LogError(ex, "Error while validating WebAuthn Auth Request via the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Failed while validating WebAuthn Auth Request via the Auth Armor API");
                this._logger?.LogError(ex, "Error while validating WebAuthn Auth Request via the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }
    }
}
