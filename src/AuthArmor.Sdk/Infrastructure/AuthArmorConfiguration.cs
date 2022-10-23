namespace AuthArmor.Sdk.Infrastructure
{
    public class AuthArmorConfiguration
    {
        /// <summary>
        /// Gets or sets the client_id - Obtained from the developer dashboard at https://dashboard.autharmor.com
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client_secret
        /// </summary>
        public string ClientSecret { get; set; }        
    }
}
