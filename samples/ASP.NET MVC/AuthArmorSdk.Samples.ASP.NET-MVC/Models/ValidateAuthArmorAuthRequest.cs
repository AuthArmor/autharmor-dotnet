using System.Text.Json.Serialization;

namespace AuthArmorSdk.Samples.ASP.NET_MVC.Models
{
    public class ValidateAuthArmorAuthRequest
    {        
        public Guid AuthRequest_Id { get; set; }
        
        public string AuthValidationToken { get; set; }
        
        public string AuthMethod { get; set; }

        public string Nonce { get; set; }

    }
}
