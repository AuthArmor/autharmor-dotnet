namespace AuthArmor.Sdk.Models.Auth.MessageValidation
{
    using System;
    using System.Text.Json.Serialization;

    [Serializable]
    public class AuthAnywhereSignatureData
    {
        [JsonPropertyName("counter")]
        public int Counter { get; set; }

        [JsonPropertyName("signature_value")]
        public string SignatureValue { get; set; }

        [JsonPropertyName("challenge")]
        public string Challenge { get; set; }
    }

    [Serializable]
    public class AuthAnywhereMessageSignature
    {
        [JsonPropertyName("signature_data")]
        public AuthAnywhereSignatureData SignatureData { get; set; }

        [JsonPropertyName("metadata")]
        public AuthAnywhereMetaData MetaData { get; set; }

    }

    [Serializable]
    public class AuthAnywhereMetaData
    {
        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("format")]
        public int Format { get; set; }

        [JsonPropertyName("key_id")]
        public string Key_Id { get; set; }
    }
}
