namespace AuthArmor.Sdk.Enums
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    using System.Xml.Linq;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AuthResponseStatus
    {
        Unknown = 0,
        [Display(Name = "Pending User Approval")]
        PendingApproval = 1,
        Success = 2,
        Declined = 3,
        InvalidSignature = 4,
        [Display(Name = "Approval Timeout")]
        Timeout = 5,        
        [Display(Name = "Pending Validation")]
        PendingValidation = 8,
        [Display(Name = "Validation Timeout")]
        ValidationTimeout = 9,
        [Display(Name = "Security Exception")]
        SecurityException = 10
    }
}
