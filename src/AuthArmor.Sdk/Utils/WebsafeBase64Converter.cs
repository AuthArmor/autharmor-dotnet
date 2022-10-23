namespace AuthArmor.Sdk.Utils
{
    public class WebsafeBase64Converter
    {
        public static string ConvertWebSafeBase64ToNormalBase64(string base64Value)
        {
            return base64Value.Replace('-', '+').Replace('_', '/').PadRight(base64Value.Length + (4 - base64Value.Length % 4) % 4, '=');
        }
    }
}
