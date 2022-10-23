namespace AuthArmor.Sdk.Models.Auth
{
    using AuthArmor.Sdk.Exceptions;    
    using System;
    using System.Text.Json.Serialization;
    /// <summary>
    /// Secured Signed Message object. Defines and describes the object propererties for a secured signed message and helpers.
    /// </summary>

    public partial class SecuredSignedDetails
    //public partial class SignedMessage
    {
        /// <summary>
        /// Gets or sets the SignedDataString - This is the signed data blob in base64
        /// </summary>
        [JsonPropertyName("signed_data")]
        public string SignedDataString { get; set; }

        /// <summary>
        /// Gets or sets the SignatureDataDetail - Details about how the signed data was signed
        /// </summary>
        [JsonPropertyName("signature_data")]
        public SignatureDataDetail SignatureDataDetail { get; set; }

        /// <summary>
        ///  Gets or sets the SignedDataObjectType - Defines the message type that is in the signed data blob
        /// </summary>
        [JsonPropertyName("signed_data_type")]
        public Enums.SignedMessageType SignedDataObjectType { get; set; }

        /// <summary>
        /// Gets or sets the SignatureValidationDetails - Validation details needed to validate the signature, such as public key, etc
        /// </summary>
        [JsonPropertyName("signature_validation_details")]
        public dynamic SignatureValidationDetails { get; set; }

        /// <summary>
        /// Helper that takes the base64 data and returns a typed object
        /// </summary>
        /// <typeparam name="T">Object type that is being decoded</typeparam>
        /// <param name="base64">Base64 encoded Input string representing signed data blog</param>
        /// <returns>SignedData object</returns>
        public static SignedData<T> DecodeSignedData<T>(string base64)
        {
            try
            {
                var n1 = Convert.FromBase64String(base64);
                var json3 = System.Text.Encoding.UTF8.GetString(n1);
                return System.Text.Json.JsonSerializer.Deserialize<SignedData<T>>(json3);
            }
            catch (Exception ex)
            {
                throw new AuthArmorException("Error Decoding Signed Data", ex);
            }
        }
    }

    /// <summary>
    /// This is the Signed Auth Response message that is inside the signed_data base64 response
    /// </summary>
    public class SignedAuthResponse
    {
        /// <summary>
        /// Gets or sets the AuthRequestID - Unique Auth Request ID
        /// </summary>
        [JsonPropertyName("auth_request_id")]
        public Guid AuthRequestID { get; set; }

        /// <summary>
        /// Gets or sets the AuthProfileID - The Auth profile ID that the auth request was sent to
        /// </summary>
        [JsonPropertyName("auth_profile_id")]
        public Guid AuthProfileID { get; set; }

        /// <summary>
        /// Gets or sets the Project ID  - The project ID for this auth request
        /// </summary>
        [JsonPropertyName("project_id")]
        public Guid ProjectID { get; set; }
    }

    /// <summary>
    /// Object Defined Signed Message
    /// </summary>
    /// <typeparam name="T">Type for Signed Message</typeparam>
    public partial class SignedData<T>
    {
        /// <summary>
        /// Gets or sets the SignedObject - this is the object that was signed
        /// </summary>
        [JsonPropertyName("message_data")]
        public T SignedObject { get; set; }

        /// <summary>
        /// Gets or sets the signing_data used to sign this message
        /// </summary>
        [JsonPropertyName("signing_data")]
        //public MetaData signing_data { get; set; }
        public MetaData SigningData { get; set; }
    }

    /// <summary>
    /// Signing Meta data that is signed with the secure message - this data is signed
    /// </summary>
    public partial class MetaData
    {
        /// <summary>
        /// Gets or sets the nonce in the request
        /// </summary>
        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        /// <summary>
        /// Gets or sets the DateSigned value - The timestamp the message was signed in UTC
        /// </summary>
        [JsonPropertyName("date_signed")]
        public DateTime DateSigned { get; set; }

        /// <summary>
        /// Gets or sets the ProfileDeviceID that facilitated the signing.
        /// </summary>
        [JsonPropertyName("profile_device_id")]
        public Guid ProfileDeviceID { get; set; }

        /// <summary>
        /// Gets or sets the counter of the profile device - used to detect a replay, clone and other attacks
        /// </summary>
        [JsonPropertyName("profile_device_counter")]
        public long? ProfileDeviceCounter { get; set; }
    }

    /// <summary>
    /// Signing Data Details - This is the info that can be used to validate the signed info in conjunction with the auth validation details
    /// </summary>
    public partial class SignatureDataDetail
    {
        /// <summary>
        /// Gets or sets the SignedDataHashValue - The hash of the signed data
        /// </summary>
        [JsonPropertyName("hash_value")]
        public string SignedDataHashValue { get; set; }

        /// <summary>
        /// Gets or sets the RawSignatureData - signature data for the selected auth method
        /// </summary>
        [JsonPropertyName("signature_data")]
        public string RawSignatureData { get; set; }

        /// <summary>
        /// Gets or sets the AuthMethodUseType - the use type of the selected auth method
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("auth_method_usetype")]
        public Enums.AuthMethodUseType AuthMethodUseType { get; set; }

        /// <summary>
        /// Gets or sets the AuthMethod -  selected auth method for this request
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("auth_method")]
        public Enums.AuthMethod AuthMethod { get; set; }

        /// <summary>
        /// Gets or sets the HashMethod that was used to hash the signed message
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("hash_method")]
        public Enums.HashType HashMethod { get; set; }
    }
}
