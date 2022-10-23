namespace AuthArmor.Sdk.Models.Auth.MessageValidation
{
    public class AuthValidationResult
    {
        public uint CounterValue { get; set; }

        public bool isValid { get; set; }

        public string Message { get; set; }
    }
}
