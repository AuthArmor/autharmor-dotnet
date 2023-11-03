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
    using System.Collections.Generic;

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

        public AuthService(ILogger<AuthService> logger, IOptions<Infrastructure.AuthArmorConfiguration> settings)
            : base(logger, settings) { }

        public AuthService(IOptions<Infrastructure.AuthArmorConfiguration> settings)
            : base(logger: null, settings) { }

        /// <summary>
        /// Gets Auth Information by auth histoy id
        /// </summary>
        /// <param name="request">Auth History ID (Guid)</param>
        /// <returns>Auth information for the given auth_request_id</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.GetAuthInfoResponse> GetAuthInfo(Models.Auth.GetAuthInfoRequest request)
        {
            var path = string.Format(_getAuthRequestInfoPath, request.AuthHistory_Id);
            var queryParams = new Dictionary<string, object>();

            return await GetAsync<Models.Auth.GetAuthInfoResponse>(path, queryParams, nameof(GetAuthInfo));
        }

        /// <summary>
        /// Validates an Auth Armor Authenticator Auth request
        /// </summary>
        /// <param name="request">Object containing the auth_request_id and auth_validation_token</param>
        /// <returns>Validation Information for Authenticator Auth request</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.Authenticator.ValidateAuthenticatorAuthResponse> ValidateAuthenticatorAuth(Models.Auth.Authenticator.ValidateAuthenticatorAuthRequest request)
        {
            var path = _validateAuthenticatorAuthPath;
            var queryParams = new Dictionary<string, object>();

            return await PostAsync<
                Models.Auth.Authenticator.ValidateAuthenticatorAuthRequest,
                Models.Auth.Authenticator.ValidateAuthenticatorAuthResponse
            >(path, queryParams, request, nameof(ValidateAuthenticatorAuth));
        }

        /// <summary>
        /// Starts a new Auth Armor Authenticator auth request
        /// </summary>
        /// <param name="request">StartAuthenticatorAuthRequest</param>
        /// <returns>StartAuthenticatorAuthResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.Authenticator.StartAuthenticatorAuthResponse> StartAuthenticatorAuth(Models.Auth.Authenticator.StartAuthenticatorAuthRequest request)
        {
            var path = _startAuthenticatorRequestPath;
            var queryParams = new Dictionary<string, object>();

            return await PostAsync<
                Models.Auth.Authenticator.StartAuthenticatorAuthRequest,
                Models.Auth.Authenticator.StartAuthenticatorAuthResponse
            >(path, queryParams, request, nameof(StartAuthenticatorAuth));
        }

        /// <summary>
        /// Starts a new WebAuthn auth request
        /// </summary>
        /// <param name="request">StartWebAuthnAuthRequest</param>
        /// <returns>StartWebAuthnAuthResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.WebAuthn.StartWebAuthnAuthResponse> StartWebAuthnAuth(Models.Auth.WebAuthn.StartWebAuthnAuthRequest request)
        {
            var path = _startWebAuthnRequestPath;
            var queryParams = new Dictionary<string, object>();

            return await PostAsync<
                Models.Auth.WebAuthn.StartWebAuthnAuthRequest,
                Models.Auth.WebAuthn.StartWebAuthnAuthResponse
            >(path, queryParams, request, nameof(StartWebAuthnAuth));
        }

        /// <summary>
        /// Finishes a WebAuthn Auth Request
        /// </summary>
        /// <param name="request">FinishWebAuthnAuthRequest</param>
        /// <returns>FinishWebAuthnAuthResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.WebAuthn.FinishWebAuthnAuthResponse> FinishWebAuthnAuth(Models.Auth.WebAuthn.FinishWebAuthnAuthRequest request)
        {
            var path = _finishWebAuthnRequestPath;
            var queryParams = new Dictionary<string, object>();

            return await PostAsync<
                Models.Auth.WebAuthn.FinishWebAuthnAuthRequest,
                Models.Auth.WebAuthn.FinishWebAuthnAuthResponse
            >(path, queryParams, request, nameof(FinishWebAuthnAuth));
        }

        /// <summary>
        /// Starts a Magiclink email Auth Request
        /// </summary>
        /// <param name="request">StartMagiclinkEmailAuthRequest</param>
        /// <returns>StartMagiclinkEmailAuthResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.MagiclinkEmail.StartMagiclinkEmailAuthResponse> StartMagiclinkEmailAuth(Models.Auth.MagiclinkEmail.StartMagiclinkEmailAuthRequest request)
        {
            var path = _startMagiclinkEmailRequestPath;
            var queryParams = new Dictionary<string, object>();

            return await PostAsync<
                Models.Auth.MagiclinkEmail.StartMagiclinkEmailAuthRequest,
                Models.Auth.MagiclinkEmail.StartMagiclinkEmailAuthResponse
            >(path, queryParams, request, nameof(StartMagiclinkEmailAuth));
        }

        /// <summary>
        /// Validates a Magiclink Email Auth Request
        /// </summary>
        /// <param name="request">ValidateMagiclinkEmailAuthRequest</param>
        /// <returns>ValidateMagiclinkEmailAuthResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.MagiclinkEmail.ValidateMagiclinkEmailAuthResponse> ValidateMagiclinkEmailAuth(Models.Auth.MagiclinkEmail.ValidateMagiclinkEmailAuthRequest request)
        {
            var path = _validateMagiclinkEmailPath;
            var queryParams = new Dictionary<string, object>();

            return await PostAsync<
                Models.Auth.MagiclinkEmail.ValidateMagiclinkEmailAuthRequest,
                Models.Auth.MagiclinkEmail.ValidateMagiclinkEmailAuthResponse
            >(path, queryParams, request, nameof(ValidateMagiclinkEmailAuth));
        }

        /// <summary>
        /// Validates a WebAuthn Auth Request
        /// </summary>
        /// <param name="request">ValidateWebAuthnAuthRequest</param>
        /// <returns>ValidateWebAuthnAuthResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.Auth.WebAuthn.ValidateWebAuthnAuthResponse> ValidateWebAuthn(Models.Auth.WebAuthn.ValidateWebAuthnAuthRequest request)
        {
            var path = _validateWebAuthnPath;
            var queryParams = new Dictionary<string, object>();

            return await PostAsync<
                Models.Auth.WebAuthn.ValidateWebAuthnAuthRequest,
                Models.Auth.WebAuthn.ValidateWebAuthnAuthResponse
            >(path, queryParams, request, nameof(ValidateWebAuthn));
        }
    }
}
