namespace AuthArmor.Sdk.Services.User
{    
    using AuthArmor.Sdk.Services._base;
    using AuthArmor.Sdk.Services.Auth;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;    
    using System;    
    using System.Net.Http;
    using System.Text.Json.Serialization;
    using System.Text.Json;    
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Polly.Retry;

    public class UserService : AuthArmorBaseService
    {
        //general user routes
        private const string _updateUserPath = "/v3/users/{0}";
        private const string _getUserByIdPath = "/v3/users/{0}";
        private const string _getUsersPath = "/v3/users";
        private const string _getAuthHistoryForUserPath = "/v3/users/{0}/auth_history";

        //authenticator - user registration
        private const string _startAuthenticatorNewUserRegistrationRequestPath = "/v3/users/authenticator/register/start";
        private const string _startAuthenticatorExistingUserRegistrationRequestPath = "/v3/users/{0}/authenticator/register/start";

        //webauthn - user registration
        private const string _startWebAuthnNewUserRegistrationRequestPath = "/v3/users/webauthn/register/start";
        private const string _finishWebAuthnNewUserRegistrationRequestPath = "/v3/users/webauthn/register/finish";
        private const string _startWebAuthnExistingUserRegistrationRequestPath = "/v3/users/{0}/webauthn/register/start";
        private const string _finishWebAuthnExistingUserRegistrationRequestPath = "/v3/users/{0}/webauthn/register/finish";

        //magiclink emails - user registration
        private const string _startMagiclinkEmailNewUserRegistrationRequestPath = "/v3/users/magiclink_email/register/start";
        private const string _startMagiclinkEmailExistingUserRegistrationRequestPath = "/v3/users/{0}/magiclink_email/register/start";
        private const string _startMagiclinkEmailChangeEmailRequestPath = "/v3/users/{0}/magiclink_email/update/start";
        private const string _validateMagiclinkEmailRegistrationTokenRequestPath = "/v3/users/register/magiclink_email/validate";

        private readonly ILogger<AuthService>? _logger;

        public UserService(ILogger<AuthService> logger, IOptions<Infrastructure.AuthArmorConfiguration> settings)
            : base(settings)
        {
            this._logger = logger;
            ValidateSettings();
        }

        public UserService(IOptions<Infrastructure.AuthArmorConfiguration> settings)
            : base(settings)
        {
            ValidateSettings();
        }

        /// <summary>
        /// Start Magiclink Email Registration for Existing user by Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="request">StartMagiclinkEmailRegistrationRequest</param>
        /// <returns>StartMagiclinkEmailRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse> StartMagiclinkEmailRegistrationForExistingUserByUsername(string username, Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string postPath = _startMagiclinkEmailExistingUserRegistrationRequestPath;

                SetUsernameOrUser_Id(username, Guid.Empty, ref headers, ref postPath);

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);

                this._logger?.LogInformation("Starting request to register magiclink email for existing user by username from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"Username: {username}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse startRegisterMagiclinkEmailResponse;
                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, postContent, headers)))
                {
                    startRegisterMagiclinkEmailResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse>(responseBody);

                    this._logger?.LogInformation("Successfully Finished Starting request to register magiclink email for existing user by username from the Auth Armor API");

                    return startRegisterMagiclinkEmailResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to register magiclink email for existing user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to register magiclink email for existing user from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("Start register magiclink email for existing user failed");
                this._logger?.LogError(ex, "Error attempting to start register magiclink email for existing user from Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("Start register magiclink email for existing user failed");
                this._logger?.LogError(ex, "Error attempting to start register magiclink email for existing user from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("Start register magiclink email for existing user failed");
                this._logger?.LogError(ex, "Error attempting to start register magiclink email for existing user from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Start magiclink Email Registration for existing user by User Id
        /// </summary>
        /// <param name="user_Id">User Id</param>
        /// <param name="request">StartMagiclinkEmailRegistrationRequest</param>
        /// <returns>StartMagiclinkEmailRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse> StartMagiclinkEmailRegistrationForExistingUserByUserId(Guid user_Id, Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string postPath = _startMagiclinkEmailExistingUserRegistrationRequestPath;

                SetUsernameOrUser_Id(String.Empty, user_Id, ref headers, ref postPath);

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);

                this._logger?.LogInformation("Starting request to register magiclink email for existing user by user id from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"User Id: {user_Id}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse startRegisterMagiclinkEmailResponse;
                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, postContent, headers)))
                {
                    startRegisterMagiclinkEmailResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse>(responseBody);

                    this._logger?.LogInformation("Successfully Finished Starting request to register magiclink email for existing user by user id from the Auth Armor API");

                    return startRegisterMagiclinkEmailResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to register magiclink email for existing user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to register magiclink email for existing user from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("Start register magiclink email for existing user failed");
                this._logger?.LogError(ex, "Error attempting to start register magiclink email for existing user from Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("Start register magiclink email for existing user failed");
                this._logger?.LogError(ex, "Error attempting to start register magiclink email for existing user from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("Start register magiclink email for existing user failed");
                this._logger?.LogError(ex, "Error attempting to start register magiclink email for existing user from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Start Magiclink Email Registration for a new user
        /// </summary>
        /// <param name="request">StartMagiclinkEmailRegistrationRequest</param>
        /// <returns>StartMagiclinkEmailRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse> StartMagiclinkEmailRegistrationForNewUser(Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                string postPath = _startMagiclinkEmailNewUserRegistrationRequestPath;

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);

                this._logger?.LogInformation("Starting request to register magiclink email for existing user from the Auth Armor API");

                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse startRegisterMagiclinkEmailResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, postContent)))
                {
                    startRegisterMagiclinkEmailResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse>(responseBody);
                    this._logger?.LogInformation("Successfully Finished Starting request to register magiclink email for existing user from the Auth Armor API");
                    return startRegisterMagiclinkEmailResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to register magiclink email for existing user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to register magiclink email for existing user from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to register magiclink email for existing user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to register magiclink email for existing user from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to register magiclink email for existing user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to register magiclink email for existing user from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to register magiclink email for existing user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to register magiclink email for existing user from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Start Change Email Address By Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="request">StartChangeMagiclinkEmailRequest</param>
        /// <returns>StartChangeMagiclinkEmailResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.MagiclinkEmail.StartChangeMagiclinkEmailResponse> StartChangeEmailForUserByUsername(string username, Models.User.Registration.MagiclinkEmail.StartChangeMagiclinkEmailRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string postPath = _startMagiclinkEmailChangeEmailRequestPath;

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);

                SetUsernameOrUser_Id(username, Guid.Empty, ref headers, ref postPath);

                this._logger?.LogInformation("Starting request to change email for existing user by username from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"Username: {username}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.MagiclinkEmail.StartChangeMagiclinkEmailResponse startChangeEmailResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, postContent, headers)))
                {
                    startChangeEmailResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.MagiclinkEmail.StartChangeMagiclinkEmailResponse>(responseBody);
                    this._logger?.LogInformation("Successfully Finished Starting request to change email for existing user by username from the Auth Armor API");
                    return startChangeEmailResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to change email for existing user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to change email for existing user by username from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to change email for existing user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to change email for existing user by username from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to change email for existing user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to change email for existing user by username from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to change email for existing user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to change email for existing user by username from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Start change Email address by User Id
        /// </summary>
        /// <param name="user_Id">User Id</param>
        /// <param name="request">StartChangeMagiclinkEmailRequest</param>
        /// <returns>StartChangeMagiclinkEmailResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.MagiclinkEmail.StartChangeMagiclinkEmailResponse> StartChangeEmailForUserByUserId(Guid user_Id, Models.User.Registration.MagiclinkEmail.StartChangeMagiclinkEmailRequest request)
        {            
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string postPath = _startMagiclinkEmailChangeEmailRequestPath;

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);

                SetUsernameOrUser_Id(String.Empty, user_Id, ref headers, ref postPath);

                this._logger?.LogInformation("Starting request to change email for existing user by username from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"User_Id: {user_Id}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.MagiclinkEmail.StartChangeMagiclinkEmailResponse startChangeEmailResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, postContent, headers)))
                {
                    startChangeEmailResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.MagiclinkEmail.StartChangeMagiclinkEmailResponse>(responseBody);
                    this._logger?.LogInformation("Successfully Finished Starting request to change email for existing user by user id from the Auth Armor API");
                    return startChangeEmailResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to change email for existing user by user id from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to change email for existing user by user id from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to change email for existing user by user id from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to change email for existing user by user id from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to change email for existing user by user id from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to change email for existing user by user id from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting request to change email for existing user by user id from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting request to change email for existing user by user id from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Finish New WebAuthn registration for Existing user by Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="request">FinishAddNewWebAuthnCredentialRequest</param>
        /// <returns>FinishAddNewWebAuthnCredentialResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.WebAuthn.FinishAddNewWebAuthnCredentialResponse> FinishWebAuthnRegistrationForExistingUserByUsername(string username, Models.User.Registration.WebAuthn.FinishAddNewWebAuthnCredentialRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string postPath = _finishWebAuthnExistingUserRegistrationRequestPath;

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);

                SetUsernameOrUser_Id(username, Guid.Empty, ref headers, ref postPath);

                this._logger?.LogInformation("Starting Finish WebAuthn Registration for existing user by username from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");

                this._logger?.LogDebug($"Username: {username}");
                this._logger?.LogDebug($"Registration Id: {request.RegistrationGuid}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.WebAuthn.FinishAddNewWebAuthnCredentialResponse finishWebAuthnRegistrationResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, postContent, headers)))
                {
                    finishWebAuthnRegistrationResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.WebAuthn.FinishAddNewWebAuthnCredentialResponse>(responseBody);
                    this._logger?.LogInformation("Successfully Finished call to finish WebAuthn Registration for existing user by username from the Auth Armor API");
                    return finishWebAuthnRegistrationResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Finishing WebAuthn Registration for existing user by username failed");
                this._logger?.LogError(ex, "Error while Finishing WebAuthn Registration for existing user by username failed");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Finishing WebAuthn Registration for existing user by username failed");
                this._logger?.LogError(ex, "Error while Finishing WebAuthn Registration for existing user by username failed");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Finishing WebAuthn Registration for existing user by username failed");
                this._logger?.LogError(ex, "Error while Finishing WebAuthn Registration for existing user by username failed");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Finishing WebAuthn Registration for existing user by username failed");
                this._logger?.LogError(ex, "Error while Finishing WebAuthn Registration for existing user by username failed");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Finish WebAuthn Registration for Existing user by User Id
        /// </summary>
        /// <param name="user_Id">User Id</param>
        /// <param name="request">FinishAddNewWebAuthnCredentialRequest</param>
        /// <returns>FinishAddNewWebAuthnCredentialResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.WebAuthn.FinishAddNewWebAuthnCredentialResponse> FinishWebAuthnRegistrationForExistingUserByUserId(Guid user_Id, Models.User.Registration.WebAuthn.FinishAddNewWebAuthnCredentialRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string postPath = _finishWebAuthnExistingUserRegistrationRequestPath;

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);

                SetUsernameOrUser_Id(String.Empty, user_Id, ref headers, ref postPath);

                this._logger?.LogInformation("Starting Finish WebAuthn Registration for existing user by user id from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"User_Id: {user_Id}");
                this._logger?.LogDebug($"Registration Id: {request.RegistrationGuid}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.WebAuthn.FinishAddNewWebAuthnCredentialResponse finishWebAuthnRegistrationResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, postContent, headers)))
                {
                    finishWebAuthnRegistrationResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.WebAuthn.FinishAddNewWebAuthnCredentialResponse>(responseBody);
                    this._logger?.LogInformation("Successfully Finished call to finish WebAuthn Registration for existing user by user id from the Auth Armor API");
                    return finishWebAuthnRegistrationResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Finishing WebAuthn Registration for existing user by user id failed");
                this._logger?.LogError(ex, "Error while Finishing WebAuthn Registration for existing user by user id failed");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Finishing WebAuthn Registration for existing user by user id failed");
                this._logger?.LogError(ex, "Error while Finishing WebAuthn Registration for existing user by user id failed");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Finishing WebAuthn Registration for existing user by user id failed");
                this._logger?.LogError(ex, "Error while Finishing WebAuthn Registration for existing user by user id failed");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Finishing WebAuthn Registration for existing user by user id failed");
                this._logger?.LogError(ex, "Error while Finishing WebAuthn Registration for existing user by user id failed");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Finish WebAuthn Registration for new User
        /// </summary>
        /// <param name="request">FinishNewWebAuthnUserRegistrationRequest</param>
        /// <returns>FinishNewWebAuthnUserRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.WebAuthn.FinishNewWebAuthnUserRegistrationResponse> FinishWebAuthnRegistrationForNewUser(Models.User.Registration.WebAuthn.FinishNewWebAuthnUserRegistrationRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                string postPath = _finishWebAuthnNewUserRegistrationRequestPath;

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);

                this._logger?.LogInformation("Starting Finish WebAuthn Registration for new user from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"Registration Id: {request.RegistrationGuid}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.WebAuthn.FinishNewWebAuthnUserRegistrationResponse finishWebAuthnRegistrationResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, postContent)))
                {
                    finishWebAuthnRegistrationResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.WebAuthn.FinishNewWebAuthnUserRegistrationResponse>(responseBody);
                    this._logger?.LogInformation("Successfully Finished WebAuthn Registration for new user from the Auth Armor API");
                    return finishWebAuthnRegistrationResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Finish WebAuthn Registration for new user failed");
                this._logger?.LogError(ex, "Error while calling Finish WebAuthn Registration for new user failed");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Finish WebAuthn Registration for new user failed");
                this._logger?.LogError(ex, "Error while calling Finish WebAuthn Registration for new user failed");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Finish WebAuthn Registration for new user failed");
                this._logger?.LogError(ex, "Error while calling Finish WebAuthn Registration for new user failed");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Finish WebAuthn Registration for new user failed");
                this._logger?.LogError(ex, "Error while calling Finish WebAuthn Registration for new user failed");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Start Add WebAuthn Registration for Existing User by Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="request">StartAddWebAuthnCredentialToExistingUserRequest</param>
        /// <returns>StartAddWebAuthnCredentialToExistingUserResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.WebAuthn.StartAddWebAuthnCredentialToExistingUserResponse> StartAddWebAuthnCredentialToExistingUserByUsername(string username, Models.User.Registration.WebAuthn.StartAddWebAuthnCredentialToExistingUserRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string postPath = _startWebAuthnExistingUserRegistrationRequestPath;

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);

                SetUsernameOrUser_Id(username, Guid.Empty, ref headers, ref postPath);

                this._logger?.LogInformation("Starting WebAuthn Registration for new user by username from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"Username: {username}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.WebAuthn.StartAddWebAuthnCredentialToExistingUserResponse startWebAuthnRegistrationResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, postContent, headers)))
                {
                    startWebAuthnRegistrationResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.WebAuthn.StartAddWebAuthnCredentialToExistingUserResponse>(responseBody);
                    this._logger?.LogInformation("Successfully Started WebAuthn Registration for new user by username from the Auth Armor API");
                    return startWebAuthnRegistrationResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Start WebAuthn Registration for new user by username from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Start WebAuthn Registration for new user by username from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Start WebAuthn Registration for new user by username from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Start WebAuthn Registration for new user by username from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Start WebAuthn Registration for new user by username from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Start WebAuthn Registration for new user by username from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Start WebAuthn Registration for new user by username from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Start WebAuthn Registration for new user by username from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Start Add WebAuthn Registration for Existing User by User Id
        /// </summary>
        /// <param name="user_Id">User Id</param>
        /// <param name="request">StartAddWebAuthnCredentialToExistingUserRequest</param>
        /// <returns>StartAddWebAuthnCredentialToExistingUserResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.WebAuthn.StartAddWebAuthnCredentialToExistingUserResponse> StartAddWebAuthnCredentialToExistingUserByUserId(Guid user_Id, Models.User.Registration.WebAuthn.StartAddWebAuthnCredentialToExistingUserRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string postPath = _startWebAuthnExistingUserRegistrationRequestPath;

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);

                SetUsernameOrUser_Id(String.Empty, user_Id, ref headers, ref postPath);

                this._logger?.LogInformation("Starting WebAuthn Registration for new user by user id from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"User_Id: {user_Id}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.WebAuthn.StartAddWebAuthnCredentialToExistingUserResponse startWebAuthnRegistrationResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, postContent, headers)))
                {
                    startWebAuthnRegistrationResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.WebAuthn.StartAddWebAuthnCredentialToExistingUserResponse>(responseBody);
                    this._logger?.LogInformation("Successfully Started WebAuthn Registration for new user by user id from the Auth Armor API");
                    return startWebAuthnRegistrationResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Start WebAuthn Registration for new user by user id from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Start WebAuthn Registration for new user by user id from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Start WebAuthn Registration for new user by user id from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Start WebAuthn Registration for new user by user id from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Start WebAuthn Registration for new user by user id from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Start WebAuthn Registration for new user by user id from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Start WebAuthn Registration for new user by user id from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Start WebAuthn Registration for new user by user id from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Start WebAuthn registration for a new User
        /// </summary>
        /// <param name="request">StartWebAuthnRegistrationRequestForNewUserRequest</param>
        /// <returns>StartWebAuthnRegistrationRequestForNewUserResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.WebAuthn.StartWebAuthnRegistrationRequestForNewUserResponse> StartWebAuthnRegistrationForNewUser(Models.User.Registration.WebAuthn.StartWebAuthnRegistrationRequestForNewUserRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                string postPath = _startWebAuthnNewUserRegistrationRequestPath;

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);

                this._logger?.LogInformation("Starting WebAuthn Registration for new user from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"Username: {request.Username}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.WebAuthn.StartWebAuthnRegistrationRequestForNewUserResponse startWebAuthnRegistrationResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, postContent)))
                {
                    startWebAuthnRegistrationResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.WebAuthn.StartWebAuthnRegistrationRequestForNewUserResponse>(responseBody);
                    this._logger?.LogInformation("Successfully Started WebAuthn Registration for new user from the Auth Armor API");
                    return startWebAuthnRegistrationResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Start WebAuthn Registration for new user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Start WebAuthn Registration for new user from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Start WebAuthn Registration for new user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Start WebAuthn Registration for new user from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Start WebAuthn Registration for new user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Start WebAuthn Registration for new user from the Auth Armor API"); ;
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Start WebAuthn Registration for new user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Start WebAuthn Registration for new user from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Start Auth Armor Authenticator Registration for a new User
        /// </summary>
        /// <param name="request">StartAuthArmorAuthenticatorRegistrationRequest</param>
        /// <returns>StartAuthArmorAuthenticatorRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse> StartAuthenticatorRegistrationForNewUser(Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                string postPath = _startAuthenticatorNewUserRegistrationRequestPath;

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);


                this._logger?.LogInformation("Starting Authenticator Registration for new user from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"Username: {request.Username}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse startAuthenticatorRegistrationResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, postContent)))
                {
                    startAuthenticatorRegistrationResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse>(responseBody);
                    this._logger?.LogInformation("Successfully Started Authenticator Registration for new user from the Auth Armor API");
                    return startAuthenticatorRegistrationResponse;
                }
            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting Authenticator Registration for new user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting Authenticator Registration for new user from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting Authenticator Registration for new user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting Authenticator Registration for new user from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting Authenticator Registration for new user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting Authenticator Registration for new user from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Starting Authenticator Registration for new user from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Starting Authenticator Registration for new user from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Start Auth Armor Authenticator Registration for an existing user by Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>StartAuthArmorAuthenticatorRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse> StartAuthenticatorRegistrationForExistingUserByUsername(string username)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string postPath = _startAuthenticatorExistingUserRegistrationRequestPath;

                SetUsernameOrUser_Id(username, Guid.Empty, ref headers, ref postPath);


                this._logger?.LogInformation("Start Authenticator Registration for existing user by username from Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");

                this._logger?.LogDebug($"Username: {username}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse startAuthenticatorRegistrationResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, string.Empty, headers)))
                {
                    startAuthenticatorRegistrationResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse>(responseBody);
                    this._logger?.LogInformation("Successfully Started Authenticator Registration for existing user by username from Auth Armor API");
                    return startAuthenticatorRegistrationResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                this._logger?.LogError(ex, "Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                this._logger?.LogError(ex, "Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                this._logger?.LogError(ex, "Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                this._logger?.LogError(ex, "Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Start Auth Armor Authenticator Registration for an existing user by User Id
        /// </summary>
        /// <param name="user_Id">Iser Id</param>
        /// <returns>StartAuthArmorAuthenticatorRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse> StartAuthenticatorRegistrationForExistingUserByUserId(Guid user_Id)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string postPath = _startAuthenticatorExistingUserRegistrationRequestPath;

                SetUsernameOrUser_Id(String.Empty, user_Id, ref headers, ref postPath);


                this._logger?.LogInformation("Start Authenticator Registration for existing user from Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"User_Id: {user_Id}");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse startAuthenticatorRegistrationResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(postPath, string.Empty, headers)))
                {
                    startAuthenticatorRegistrationResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse>(responseBody);
                    this._logger?.LogInformation("Successfully Started Authenticator Registration for existing user by username from Auth Armor API");
                    return startAuthenticatorRegistrationResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                this._logger?.LogError(ex, "Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                this._logger?.LogError(ex, "Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                this._logger?.LogError(ex, "Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                this._logger?.LogError(ex, "Error while starting Authenticator Registration for existing user by username from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get Users Auth History By Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="sortColumn">Sort Column</param>
        /// <returns>GetAuthHistoryPagedResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.AuthHistory.GetAuthHistoryPagedResponse> GetUserAuthHistoryByUsername(string username, int pageNumber, int pageSize, string sortDirection, string sortColumn)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string getPathBase = _getAuthHistoryForUserPath;

                SetUsernameOrUser_Id(username, Guid.Empty, ref headers, ref getPathBase);

                string getPath = $"{getPathBase}?page_number={pageNumber}&page_size={pageSize}&sort_column={sortColumn}&sort_direction={sortDirection}";

                this._logger?.LogInformation("Starting Get user auth history by username from Auth Armor API");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.AuthHistory.GetAuthHistoryPagedResponse getUsersPagedResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Get(getPath, headers)))
                {
                    getUsersPagedResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.AuthHistory.GetAuthHistoryPagedResponse>(responseBody);
                    this._logger?.LogInformation("Successfully called Get user auth history by userame from Auth Armor API");
                    return getUsersPagedResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Getting user auth history by username from Auth Armor API");
                this._logger?.LogError(ex, "Error while Getting user auth history by username from Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Getting user auth history by username from Auth Armor API");
                this._logger?.LogError(ex, "Error while Getting user auth history by username from Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Getting user auth history by username from Auth Armor API");
                this._logger?.LogError(ex, "Error while Getting user auth history by username from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Getting user auth history by username from Auth Armor API");
                this._logger?.LogError(ex, "Error while Getting user auth history by username from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get Users Auth History By User Id
        /// </summary>
        /// <param name="user_id">Username</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="sortColumn">Sort Column</param>
        /// <returns>GetAuthHistoryPagedResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.AuthHistory.GetAuthHistoryPagedResponse> GetUserAuthHistoryByUserId(Guid user_Id, int pageNumber, int pageSize, string sortDirection, string sortColumn)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string getPathBase = _getAuthHistoryForUserPath;

                SetUsernameOrUser_Id(String.Empty, user_Id, ref headers, ref getPathBase);

                string getPath = $"{getPathBase}?page_number={pageNumber}&page_size={pageSize}&sort_column={sortColumn}&sort_direction={sortDirection}";

                this._logger?.LogInformation("Starting Get user auth history by user id from Auth Armor API");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.AuthHistory.GetAuthHistoryPagedResponse getUsersPagedResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Get(getPath, headers)))
                {
                    getUsersPagedResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.AuthHistory.GetAuthHistoryPagedResponse>(responseBody);
                    this._logger?.LogInformation("Successfully called Get user auth history by user id from Auth Armor API");
                    return getUsersPagedResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Getting user auth history by user id from Auth Armor API");
                this._logger?.LogError(ex, "Error while Getting user auth history by user id from Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Getting user auth history by user id from Auth Armor API");
                this._logger?.LogError(ex, "Error while Getting user auth history by user id from Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Getting user auth history by user id from Auth Armor API");
                this._logger?.LogError(ex, "Error while Getting user auth history by user id from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Getting user auth history by user id from Auth Armor API");
                this._logger?.LogError(ex, "Error while Getting user auth history by user id from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get all Users with Pagination
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="sortColumn">Sort Column</param>
        /// <returns>GetUsersPagedResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.GetUsersPagedResponse> GetUsers(int pageNumber, int pageSize, string sortDirection, string sortColumn)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                string getPath = $"{_getUsersPath}?page_number={pageNumber}&page_size={pageSize}&sort_column={sortColumn}&sort_direction={sortDirection}";

                this._logger?.LogInformation("Starting Get users from Auth Armor API");
                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.GetUsersPagedResponse getUsersPagedResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Get(getPath)))
                {
                    getUsersPagedResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.GetUsersPagedResponse>(responseBody);
                    this._logger?.LogInformation("Successfully called Get users from Auth Armor API");
                    return getUsersPagedResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Getting users from Auth Armor API");
                this._logger?.LogError(ex, "Error while Getting users from Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Getting users from Auth Armor API");
                this._logger?.LogError(ex, "Error while Getting users from Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Getting users from Auth Armor API");
                this._logger?.LogError(ex, "Error while Getting users from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Getting users from Auth Armor API");
                this._logger?.LogError(ex, "Error while Getting users from Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get User by User Id
        /// </summary>
        /// <param name="user_Id">User Id</param>
        /// <returns>GetUserResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.GetUserResponse> GetUserByUserId(Guid user_Id)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string getPath = _getUserByIdPath;

                SetUsernameOrUser_Id(String.Empty, user_Id, ref headers, ref getPath);

                this._logger?.LogInformation("Starting Get User by User ID from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"User_Id: {user_Id}");

                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.GetUserResponse updateUserResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Get(getPath, headers)))
                {
                    updateUserResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.GetUserResponse>(responseBody);
                    this._logger?.LogInformation("Successfully called Get User by User ID from the Auth Armor API");
                    return updateUserResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Get User by User ID from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Get User by User ID from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Get User by User ID from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Get User by User ID from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Get User by User ID from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Get User by User ID from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Get User by User ID from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Get User by User ID from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get User by Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>GetUserResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.GetUserResponse> GetUserByUsername(string username)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;

                string getPath = _getUserByIdPath;

                SetUsernameOrUser_Id(username, Guid.Empty, ref headers, ref getPath);



                this._logger?.LogInformation("Starting Get User by username from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");

                this._logger?.LogDebug($"Username: {username}");

                this._logger?.LogInformation("Calling Auth Armor API");

                Models.User.GetUserResponse updateUserResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Get(getPath, headers)))
                {
                    updateUserResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.GetUserResponse>(responseBody);
                    this._logger?.LogInformation("Successfully called Get User by username from the Auth Armor API");
                    return updateUserResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Get User by username from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Get User by username from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Get User by username from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Get User by username from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Get User by username from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Get User by username from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Get User by username from the Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Get User by username from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Update a user by User Id
        /// </summary>
        /// <param name="user_Id">User Id</param>
        /// <param name="request">UpdateUserRequest</param>
        /// <returns>User</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.User> UpdateUserByUserId(Guid user_Id, Models.User.UpdateUserRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;
                string postPath = _updateUserPath;

                SetUsernameOrUser_Id(String.Empty, user_Id, ref headers, ref postPath);

                this._logger?.LogInformation("Starting Update User by user id from the Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug($"User_Id: {user_Id}");
                this._logger?.LogDebug($"New Username: {request.NewUsername}");
                this._logger?.LogInformation("Calling Auth Armor API");



                var postContent = System.Text.Json.JsonSerializer.Serialize(request);


                Models.User.User updateUserResponse;
                using (var responseBody = await polly.ExecuteAsync(() => Put(postPath, postContent, headers)))
                {
                    updateUserResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.User>(responseBody);
                    this._logger?.LogInformation("Successfully called Update User by user id from the Auth Armor API");
                    return updateUserResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Updating User by user id from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Updating User by user id from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Updating User by user id from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Updating User by user id from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Updating User by user id from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Updating User by user id from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Updating User by user id from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Updating User by user id from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Update a user by Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="request">UpdateUserRequest</param>
        /// <returns>User</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.User> UpdateUserByUsername(string username, Models.User.UpdateUserRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();

                Dictionary<string, string> headers = null;
                string postPath = _updateUserPath;

                SetUsernameOrUser_Id(username, Guid.Empty, ref headers, ref postPath);

                this._logger?.LogInformation("Updating User via Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");

                this._logger?.LogDebug($"Username: {username}");
                this._logger?.LogDebug($"New Username: {request.NewUsername}");

                this._logger?.LogInformation("Calling API");



                var postContent = System.Text.Json.JsonSerializer.Serialize(request);


                Models.User.User updateUserResponse;
                using (var responseBody = await polly.ExecuteAsync(() => Put(postPath, postContent, headers)))
                {
                    updateUserResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.User>(responseBody);
                    this._logger?.LogInformation("Successfully called Update User by username from the Auth Armor API");
                    return updateUserResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Updating User by username from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Updating User by username from the Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Updating User by username from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Updating User by username from the Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while Updating User by username from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Updating User by username from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while Updating User by username from the Auth Armor API");
                this._logger?.LogError(ex, "Error while Updating User by username from the Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }



        /// <summary>
        /// Validate Magiclink Email Registration Token
        /// </summary>
        /// <param name="request">ValidateMagiclinkEmailRegistrationRequest</param>
        /// <returns>ValidateMagiclinkEmailRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.MagiclinkEmail.ValidateMagiclinkEmailRegistrationResponse> ValidateMagiclinkEmailRegistrationToken(Models.User.Registration.MagiclinkEmail.ValidateMagiclinkEmailRegistrationRequest request)
        {
            try
            {
                AsyncRetryPolicy polly = GetAsyncRetryPolicy();


                this._logger?.LogInformation("Starting Validate Magiclink Email Registration Token via Auth Armor API");
                this._logger?.LogDebug("=== Request Parms ===");
                this._logger?.LogDebug("Registration Validation Token: {0}", request.MagiclinkEmailRegistrationValidationToken);
                this._logger?.LogInformation("Calling Auth Armor API");

                var postContent = System.Text.Json.JsonSerializer.Serialize(request);


                Models.User.Registration.MagiclinkEmail.ValidateMagiclinkEmailRegistrationResponse validateMagiclinkEmailAuthResponse;

                using (var responseBody = await polly.ExecuteAsync(() => Post(_validateMagiclinkEmailRegistrationTokenRequestPath, postContent)))
                {
                    var options = new JsonSerializerOptions();
                    options.Converters.Add(new JsonStringEnumConverter());
                    validateMagiclinkEmailAuthResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<Models.User.Registration.MagiclinkEmail.ValidateMagiclinkEmailRegistrationResponse>(responseBody, options);
                    this._logger?.LogInformation("Successfully called Validate Magiclink Email Registration Token via Auth Armor API");
                    return validateMagiclinkEmailAuthResponse;
                }

            }
            catch (Exceptions.AuthArmorException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Validate Magiclink Email Registration Token via Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Validate Magiclink Email Registration Token via Auth Armor API");
                throw ex;
            }
            catch (Exceptions.AuthArmorHttpResponseException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Validate Magiclink Email Registration Token via Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Validate Magiclink Email Registration Token via Auth Armor API");
                this._logger?.LogError($"Http Status code: {ex.StatusCode}");
                this._logger?.LogError($"Http Body: {ex.Message}");
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Validate Magiclink Email Registration Token via Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Validate Magiclink Email Registration Token via Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this._logger?.LogInformation("FAILED - Error while calling Validate Magiclink Email Registration Token via Auth Armor API");
                this._logger?.LogError(ex, "Error while calling Validate Magiclink Email Registration Token via Auth Armor API");
                throw new Exceptions.AuthArmorException(ex.Message, ex);
            }
        }

        private static void SetUsernameOrUser_Id(string username, Guid user_Id, ref Dictionary<string, string> headers, ref string urlPath)
        {
            //set url path to user_id - if the user_id is an empty guid, it sets empty, if its a real user_id, it sets that user_id.
            urlPath = string.Format(urlPath, user_Id);

            //check if username or user_id was provided
            if (false == string.IsNullOrWhiteSpace(username))
            {
                //we have a username, set it in the request
                headers = new Dictionary<string, string>();
                headers.Add("X-AuthArmor-UsernameValue", username);
            }
        }
    }
}
