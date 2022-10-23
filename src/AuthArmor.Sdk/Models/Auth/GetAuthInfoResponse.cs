using System;
using System.Collections.Generic;
using System.Text;

namespace AuthArmor.Sdk.Models.Auth
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    /// <summary>
    ///  Get Auth Info Response object
    /// </summary>
    public class GetAuthInfoResponse
    {
        /// <summary>
        /// Gets or sets the AuthResponse
        /// </summary>
        [JsonPropertyName("auth_response")]
        public AuthResponse AuthResponse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("auth_request_status_id")]
        public int AuthRequestStatusId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("auth_request_status_name")]
        public string AuthRequestStatusName { get; set; }

    }

    /// <summary>
    /// Auth Response Object
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// Gets or sets the AuthHistory_Id - the Unique auth History ID 
        /// </summary>
        [JsonPropertyName("auth_history_id")]
        public Guid AuthHistory_Id { get; set; }

        /// <summary>
        /// Gets or sets the ResultCode for the request
        /// </summary>
        [JsonPropertyName("result_code")]
        public int ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the ResultMessage for the request
        /// </summary>
        [JsonPropertyName("result_message")]
        public string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the Authorized Flag
        /// </summary>
        [JsonPropertyName("authorized")]
        public bool Authorized { get; set; }

        /// <summary>
        /// Gets or sets the AuthDetails
        /// </summary>
        [JsonPropertyName("auth_details")]
        public AuthDetails AuthDetails { get; set; }

    }

    /// <summary>
    /// Response Auth Method Details
    /// </summary>
    public class SelectedAuthMethodDetail
    {
        /// <summary>
        /// Gets or sets the AuthMethod - The auth method used
        /// </summary>
        [JsonPropertyName("name")]
        [EnumDataType(typeof(Enums.AuthMethod))]
        public Enums.AuthMethod AuthMethod { get; set; }

        /// <summary>
        /// Gets or sets the AuthMethodUseType - How the auth method was used
        /// </summary>
        [JsonPropertyName("usetype")]
        [EnumDataType(typeof(Enums.AuthMethodUseType))]
        public Enums.AuthMethodUseType AuthMethodUseType { get; set; }
    }

    /// <summary>
    /// Auth details - Request and Response
    /// </summary>
    public class AuthDetails
    {
        /// <summary>
        /// Gets or sets the AuthRequestDetails
        /// </summary>
        [JsonPropertyName("request_details")]
        public AuthRequestDetails AuthRequestDetails { get; set; }

        /// <summary>
        /// Gets or sets the AuthResponseDetails 
        /// </summary>
        [JsonPropertyName("response_details")]
        public AuthResponseDetails AuthResponseDetails { get; set; }
    }

    /// <summary>
    /// Auth response Detail Object
    /// </summary>
    public class AuthResponseDetails
    {
        /// <summary>
        /// Gets or sets the DateResponsed -  Timestamp for when the request was responsed to
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime DateResponsed { get; set; }

        /// <summary>
        /// Gets or sets the SelectedAuthMethodDetail used in the response
        /// </summary>
        [JsonPropertyName("auth_method")]
        public SelectedAuthMethodDetail SelectedAuthMethodDetail { get; set; }

        /// <summary>
        /// Gets or sets the SecuredSignedMessageDetails
        /// </summary>
        [JsonPropertyName("secure_signed_message")]
        public SecuredSignedDetails SecuredSignedMessageDetails { get; set; }

        /// <summary>
        /// Gets or sets the  the AuthArmorAppMobileDevice - Information about the mobile device that facilitated the auth
        /// </summary>
        [JsonPropertyName("mobile_device_details")]
        public AuthArmorAppMobileDevice AuthArmorAppMobileDevice { get; set; }

        /// <summary>
        /// Gets or sets the AuthProfileDetails where the Auth request was sent
        /// </summary>        
        [JsonPropertyName("auth_profile_details")]
        public AuthProfileDetails AuthProfileDetails { get; set; }
    }

    /// <summary>
    /// Mobile Device info inbject
    /// </summary>
    public class AuthArmorAppMobileDevice
    {
        /// <summary>
        /// Gets or sets the Platform - Mobile Device platform used - Android or iOS
        /// </summary>
        [JsonPropertyName("platform")]
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the Model - The mobile device model that was used
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; }
    }

    /// <summary>
    /// Auth profile detail object
    /// </summary>
    public class AuthProfileDetails
    {
        /// <summary>
        /// Gets or sets the AuthProfileID
        /// </summary>
        [JsonPropertyName("user_id")]
        public Guid User_Id { get; set; }

        /// <summary>
        /// Gets or sets the nickname that was set during invite for the Auth Profile
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }       
    }

    /// <summary>
    /// The Object for Accepted Auth methods in the request
    /// </summary>
    public class RequestAuthMethod
    {
        /// <summary>
        /// Gets or sets the AuthMethod - The auth method in the request
        /// </summary>
        [JsonPropertyName("name")]
        [EnumDataType(typeof(Enums.AuthMethod))]
        public Enums.AuthMethod AuthMethod { get; set; }

    }

    /// <summary>
    /// The Request details from the auth request
    /// </summary>
    public class AuthRequestDetails
    {
        /// <summary>
        /// Gets or sets the DateRequest - the date the Auth was requested
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime DateRequested { get; set; }

        /// <summary>
        /// Gets or sets the AuthProfileDetails where the Auth request was sent
        /// </summary>
        [JsonPropertyName("auth_profile_details")]
        public AuthProfileDetails AuthProfileDetails { get; set; }        

    }
}
