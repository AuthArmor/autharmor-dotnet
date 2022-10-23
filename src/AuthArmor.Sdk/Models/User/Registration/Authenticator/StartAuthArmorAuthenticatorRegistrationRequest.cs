using System.Text.Json.Serialization;

namespace AuthArmor.Sdk.Models.User.Registration.Authenticator
{

    
    public class StartAuthArmorAuthenticatorRegistrationRequest
    {        
        [JsonPropertyName("username")]        
        public string Username { get; set; }

        /// <summary>
        /// Set to true if you want to reset and reinvite a user that is already setup
        /// </summary>
        [JsonPropertyName("reset_and_reinvite")]        
        public bool FullReset { get; set; }

        [JsonPropertyName("revoke_previous_invites")]        
        public bool RevokePreviousInvites { get; set; }
    }
}

