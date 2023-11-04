using System.Threading;

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
    using System.Runtime.InteropServices;

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

        //user credential management
        private const string _getUserCredentialsPath = "/v4/users/{0}/credentials";
        private const string _getUserAuthenticatorCredentialsPath = "/v4/users/{0}/credentials/authenticator";
        private const string _getUserWebAuthnCredentialsPath = "/v4/users/{0}/credentials/webauthn";
        private const string _getUserMagiclinkEmailCredentialsPath = "/v4/users/{0}/credentials/magiclink_email";
        private const string _getUserCredentialPath = "/v4/users/{0}/credentials/{1}";
        private const string _disableUserCredentialPath = "/v4/users/{0}/credentials/{1}";

        public UserService(ILogger<AuthService> logger, IOptions<Infrastructure.AuthArmorConfiguration> settings)
            : base(logger, settings) { }


        public UserService(IOptions<Infrastructure.AuthArmorConfiguration> settings)
            : base(logger: null, settings) { }

        /// <summary>
        /// Start Magiclink Email Registration for Existing user by Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="request">StartMagiclinkEmailRegistrationRequest</param>
        /// <returns>StartMagiclinkEmailRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse> StartMagiclinkEmailRegistrationForExistingUserByUsername(string username, Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationRequest request)
        {
            var path = _startMagiclinkEmailExistingUserRegistrationRequestPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);

            return await PostAsync<
                Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationRequest,
                Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse
            >(path, queryParams, request, nameof(StartMagiclinkEmailRegistrationForExistingUserByUsername));
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
            var path = _startMagiclinkEmailExistingUserRegistrationRequestPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);

            return await PostAsync<
                Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationRequest,
                Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse
            >(path, queryParams, request, nameof(StartMagiclinkEmailRegistrationForExistingUserByUsername));
        }

        /// <summary>
        /// Start Magiclink Email Registration for a new user
        /// </summary>
        /// <param name="request">StartMagiclinkEmailRegistrationRequest</param>
        /// <returns>StartMagiclinkEmailRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse> StartMagiclinkEmailRegistrationForNewUser(Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationRequest request)
        {
            var path = _startMagiclinkEmailNewUserRegistrationRequestPath;
            var queryParams = new Dictionary<string, object>();

            return await PostAsync<
                Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationRequest,
                Models.User.Registration.MagiclinkEmail.StartMagiclinkEmailRegistrationResponse
            >(path, queryParams, request, nameof(StartMagiclinkEmailRegistrationForExistingUserByUsername));
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
            var path = _startMagiclinkEmailChangeEmailRequestPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);

            return await PostAsync<
                Models.User.Registration.MagiclinkEmail.StartChangeMagiclinkEmailRequest,
                Models.User.Registration.MagiclinkEmail.StartChangeMagiclinkEmailResponse
            >(path, queryParams, request, nameof(StartChangeEmailForUserByUsername));
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
            var path = _startMagiclinkEmailChangeEmailRequestPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);

            return await PostAsync<
                Models.User.Registration.MagiclinkEmail.StartChangeMagiclinkEmailRequest,
                Models.User.Registration.MagiclinkEmail.StartChangeMagiclinkEmailResponse
            >(path, queryParams, request, nameof(StartChangeEmailForUserByUserId));
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
            var path = string.Format(_finishWebAuthnExistingUserRegistrationRequestPath, username);
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);

            return await PostAsync<
                Models.User.Registration.WebAuthn.FinishAddNewWebAuthnCredentialRequest,
                Models.User.Registration.WebAuthn.FinishAddNewWebAuthnCredentialResponse
            >(path, queryParams, request, nameof(FinishWebAuthnRegistrationForExistingUserByUsername));
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
            var path = _finishWebAuthnExistingUserRegistrationRequestPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);

            return await PostAsync<
                Models.User.Registration.WebAuthn.FinishAddNewWebAuthnCredentialRequest,
                Models.User.Registration.WebAuthn.FinishAddNewWebAuthnCredentialResponse
            >(path, queryParams, request, nameof(FinishWebAuthnRegistrationForExistingUserByUserId));
        }

        /// <summary>
        /// Finish WebAuthn Registration for new User
        /// </summary>
        /// <param name="request">FinishNewWebAuthnUserRegistrationRequest</param>
        /// <returns>FinishNewWebAuthnUserRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.WebAuthn.FinishNewWebAuthnUserRegistrationResponse> FinishWebAuthnRegistrationForNewUser(Models.User.Registration.WebAuthn.FinishNewWebAuthnUserRegistrationRequest request)
        {
            var path = _finishWebAuthnNewUserRegistrationRequestPath;
            var queryParams = new Dictionary<string, object>();

            return await PostAsync<
                Models.User.Registration.WebAuthn.FinishNewWebAuthnUserRegistrationRequest,
                Models.User.Registration.WebAuthn.FinishNewWebAuthnUserRegistrationResponse
            >(path, queryParams, request, nameof(FinishWebAuthnRegistrationForNewUser));
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
            var path = string.Format(_startWebAuthnExistingUserRegistrationRequestPath, username);
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);

            return await PostAsync<
                Models.User.Registration.WebAuthn.StartAddWebAuthnCredentialToExistingUserRequest,
                Models.User.Registration.WebAuthn.StartAddWebAuthnCredentialToExistingUserResponse
            >(path, queryParams, request, nameof(StartAddWebAuthnCredentialToExistingUserByUsername));
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
            var path = _startWebAuthnExistingUserRegistrationRequestPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);

            return await PostAsync<
                Models.User.Registration.WebAuthn.StartAddWebAuthnCredentialToExistingUserRequest,
                Models.User.Registration.WebAuthn.StartAddWebAuthnCredentialToExistingUserResponse
            >(path, queryParams, request, nameof(StartAddWebAuthnCredentialToExistingUserByUserId));
        }

        /// <summary>
        /// Start WebAuthn registration for a new User
        /// </summary>
        /// <param name="request">StartWebAuthnRegistrationRequestForNewUserRequest</param>
        /// <returns>StartWebAuthnRegistrationRequestForNewUserResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.WebAuthn.StartWebAuthnRegistrationRequestForNewUserResponse> StartWebAuthnRegistrationForNewUser(Models.User.Registration.WebAuthn.StartWebAuthnRegistrationRequestForNewUserRequest request)
        {
            var path = _startWebAuthnNewUserRegistrationRequestPath;
            var queryParams = new Dictionary<string, object>();

            return await PostAsync<
                Models.User.Registration.WebAuthn.StartWebAuthnRegistrationRequestForNewUserRequest,
                Models.User.Registration.WebAuthn.StartWebAuthnRegistrationRequestForNewUserResponse
            >(path, queryParams, request, nameof(StartWebAuthnRegistrationForNewUser));
        }

        /// <summary>
        /// Start Auth Armor Authenticator Registration for a new User
        /// </summary>
        /// <param name="request">StartAuthArmorAuthenticatorRegistrationRequest</param>
        /// <returns>StartAuthArmorAuthenticatorRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse> StartAuthenticatorRegistrationForNewUser(Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationRequest request)
        {
            var path = _startAuthenticatorNewUserRegistrationRequestPath;
            var queryParams = new Dictionary<string, object>();

            return await PostAsync<
                Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationRequest,
                Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse
            >(path, queryParams, request, nameof(StartWebAuthnRegistrationForNewUser));
        }

        /// <summary>
        /// Start Auth Armor Authenticator Registration for an existing user by Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>StartAuthArmorAuthenticatorRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse> StartAuthenticatorRegistrationForExistingUserByUsername(string username)
        {
            var path = _startAuthenticatorExistingUserRegistrationRequestPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);

            return await PostAsync<
                object,
                Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse
            >(path, queryParams, null, nameof(StartAuthenticatorRegistrationForExistingUserByUsername));
        }

        /// <summary>
        /// Start Auth Armor Authenticator Registration for an existing user by User Id
        /// </summary>
        /// <param name="user_Id">Iser Id</param>
        /// <returns>StartAuthArmorAuthenticatorRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse> StartAuthenticatorRegistrationForExistingUserByUserId(Guid user_Id)
        {
            var path = _startAuthenticatorExistingUserRegistrationRequestPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);

            return await PostAsync<
                object,
                Models.User.Registration.Authenticator.StartAuthArmorAuthenticatorRegistrationResponse
            >(path, queryParams, null, nameof(StartAuthenticatorRegistrationForExistingUserByUserId));
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
            var path = _getAuthHistoryForUserPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);
            SetPaging(pageNumber, pageSize, sortDirection, sortColumn, ref queryParams);

            return await GetAsync<
                Models.User.AuthHistory.GetAuthHistoryPagedResponse
            >(path, queryParams, nameof(GetUserAuthHistoryByUsername));
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
            var path = _getAuthHistoryForUserPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);
            SetPaging(pageNumber, pageSize, sortDirection, sortColumn, ref queryParams);

            return await GetAsync<
                Models.User.AuthHistory.GetAuthHistoryPagedResponse
            >(path, queryParams, nameof(GetUserAuthHistoryByUserId));
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
            var path = _getUsersPath;
            var queryParams = new Dictionary<string, object>();

            SetPaging(pageNumber, pageSize, sortDirection, sortColumn, ref queryParams);

            return await GetAsync<
                Models.User.GetUsersPagedResponse
            >(path, queryParams, nameof(GetUsers));
        }

        /// <summary>
        /// Get User by User Id
        /// </summary>
        /// <param name="user_Id">User Id</param>
        /// <returns>GetUserResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.GetUserResponse> GetUserByUserId(Guid user_Id)
        {
            var path = _getUserByIdPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);

            return await GetAsync<
                Models.User.GetUserResponse
            >(path, queryParams, nameof(GetUserByUserId));
        }

        /// <summary>
        /// Get User by Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>GetUserResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.GetUserResponse> GetUserByUsername(string username)
        {
            var path = _getUserByIdPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);

            return await GetAsync<
                Models.User.GetUserResponse
            >(path, queryParams, nameof(GetUserByUsername));
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
            var path = _updateUserPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);

            return await PutAsync<
                Models.User.UpdateUserRequest,
                Models.User.User
            >(path, queryParams, request, nameof(UpdateUserByUserId));
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
            var path = _updateUserPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);

            return await PutAsync<
                Models.User.UpdateUserRequest,
                Models.User.User
            >(path, queryParams, request, nameof(UpdateUserByUsername));
        }



        /// <summary>
        /// Validate Magiclink Email Registration Token
        /// </summary>
        /// <param name="request">ValidateMagiclinkEmailRegistrationRequest</param>
        /// <returns>ValidateMagiclinkEmailRegistrationResponse</returns>
        /// <exception cref="Exceptions.AuthArmorException"></exception>
        public async Task<Models.User.Registration.MagiclinkEmail.ValidateMagiclinkEmailRegistrationResponse> ValidateMagiclinkEmailRegistrationToken(Models.User.Registration.MagiclinkEmail.ValidateMagiclinkEmailRegistrationRequest request)
        {
            var path = _validateMagiclinkEmailRegistrationTokenRequestPath;
            var queryParams = new Dictionary<string, object>();

            return await PostAsync<
                Models.User.Registration.MagiclinkEmail.ValidateMagiclinkEmailRegistrationRequest,
                Models.User.Registration.MagiclinkEmail.ValidateMagiclinkEmailRegistrationResponse
            >(path, queryParams, request, nameof(ValidateMagiclinkEmailRegistrationToken));
        }

        public async Task<Models.User.Credentials.GetCredentialsPagedResponse> GetUserCredentialsByUsername(string username, int pageNumber, int pageSize, string sortDirection, string sortColumn)
        {
            var path = _getUserCredentialsPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);
            SetPaging(pageNumber, pageSize, sortDirection, sortColumn, ref queryParams);

            return await GetAsync<
                Models.User.Credentials.GetCredentialsPagedResponse
            >(path, queryParams, nameof(GetUserCredentialsByUsername));
        }

        public async Task<Models.User.Credentials.GetCredentialsPagedResponse> GetUserCredentialsByUserId(Guid user_Id, int pageNumber, int pageSize, string sortDirection, string sortColumn)
        {
            var path = _getUserCredentialsPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);
            SetPaging(pageNumber, pageSize, sortDirection, sortColumn, ref queryParams);

            return await GetAsync<
                Models.User.Credentials.GetCredentialsPagedResponse
            >(path, queryParams, nameof(GetUserCredentialsByUserId));
        }

        public async Task<Models.User.Credentials.GetCredentialsPagedResponse> GetUserAuthenticatorCredentialsByUsername(string username, int pageNumber, int pageSize, string sortDirection, string sortColumn)
        {
            var path = _getUserAuthenticatorCredentialsPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);
            SetPaging(pageNumber, pageSize, sortDirection, sortColumn, ref queryParams);

            return await GetAsync<
                Models.User.Credentials.GetCredentialsPagedResponse
            >(path, queryParams, nameof(GetUserAuthenticatorCredentialsByUsername));
        }

        public async Task<Models.User.Credentials.GetCredentialsPagedResponse> GetUserAuthenticatorCredentialsByUserId(Guid user_Id, int pageNumber, int pageSize, string sortDirection, string sortColumn)
        {
            var path = _getUserAuthenticatorCredentialsPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);
            SetPaging(pageNumber, pageSize, sortDirection, sortColumn, ref queryParams);

            return await GetAsync<
                Models.User.Credentials.GetCredentialsPagedResponse
            >(path, queryParams, nameof(GetUserAuthenticatorCredentialsByUserId));
        }

        public async Task<Models.User.Credentials.GetCredentialsPagedResponse> GetUserWebAuthnCredentialsByUsername(string username, int pageNumber, int pageSize, string sortDirection, string sortColumn)
        {
            var path = _getUserWebAuthnCredentialsPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);
            SetPaging(pageNumber, pageSize, sortDirection, sortColumn, ref queryParams);

            return await GetAsync<
                Models.User.Credentials.GetCredentialsPagedResponse
            >(path, queryParams, nameof(GetUserWebAuthnCredentialsByUsername));
        }

        public async Task<Models.User.Credentials.GetCredentialsPagedResponse> GetUserWebAuthnCredentialsByUserId(Guid user_Id, int pageNumber, int pageSize, string sortDirection, string sortColumn)
        {
            var path = _getUserWebAuthnCredentialsPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);
            SetPaging(pageNumber, pageSize, sortDirection, sortColumn, ref queryParams);

            return await GetAsync<
                Models.User.Credentials.GetCredentialsPagedResponse
            >(path, queryParams, nameof(GetUserWebAuthnCredentialsByUserId));
        }

        public async Task<Models.User.Credentials.GetCredentialsPagedResponse> GetUserMagiclinkEmailCredentialsByUsername(string username, int pageNumber, int pageSize, string sortDirection, string sortColumn)
        {
            var path = _getUserMagiclinkEmailCredentialsPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);
            SetPaging(pageNumber, pageSize, sortDirection, sortColumn, ref queryParams);

            return await GetAsync<
                Models.User.Credentials.GetCredentialsPagedResponse
            >(path, queryParams, nameof(GetUserMagiclinkEmailCredentialsByUsername));
        }

        public async Task<Models.User.Credentials.GetCredentialsPagedResponse> GetUserMagiclinkEmailCredentialsByUserId(Guid user_Id, int pageNumber, int pageSize, string sortDirection, string sortColumn)
        {
            var path = _getUserMagiclinkEmailCredentialsPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);
            SetPaging(pageNumber, pageSize, sortDirection, sortColumn, ref queryParams);

            return await GetAsync<
                Models.User.Credentials.GetCredentialsPagedResponse
            >(path, queryParams, nameof(GetUserMagiclinkEmailCredentialsByUserId));
        }

        public async Task<Models.User.Credentials.GetCredentialResponse> GetUserCredentialByUsername(string username, string credentialId)
        {
            var path = _getUserCredentialPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(username, Guid.Empty, ref queryParams, ref path);
            path = string.Format(path, null, credentialId);

            return await GetAsync<
                Models.User.Credentials.GetCredentialResponse
            >(path, queryParams, nameof(GetUserCredentialByUsername));
        }

        public async Task<Models.User.Credentials.GetCredentialResponse> GetUserCredentialByUserId(Guid user_Id, string credentialId)
        {
            var path = _getUserCredentialPath;
            var queryParams = new Dictionary<string, object>();

            SetUsernameOrUser_Id(string.Empty, user_Id, ref queryParams, ref path);
            path = string.Format(path, null, credentialId);

            return await GetAsync<
                Models.User.Credentials.GetCredentialResponse
            >(path, queryParams, nameof(GetUserCredentialByUserId));
        }

        private static void SetUsernameOrUser_Id(string username, Guid user_Id, ref Dictionary<string, object> queryParams, ref string urlPath)
        {
            //set url path to user_id - if the user_id is an empty guid, it sets empty, if its a real user_id, it sets that user_id.
            urlPath = string.Format(urlPath, user_Id);

            //check if username or user_id was provided
            if (false == string.IsNullOrWhiteSpace(username))
            {
                //we have a username, set it in the request
                queryParams["username"] = username;
            }
        }

        private static void SetPaging(int pageNumber, int pageSize, string sortDirection, string sortColumn, ref Dictionary<string, object> queryParams)
        {
            queryParams["page_number"] = pageNumber;
            queryParams["page_size"] = pageSize;
            queryParams["sort_direction"] = sortDirection;
            queryParams["sort_column"] = sortColumn;
        }
    }
}
