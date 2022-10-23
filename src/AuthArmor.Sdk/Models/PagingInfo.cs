namespace AuthArmor.Sdk.Models
{
    using System.Text.Json.Serialization;

    public class PagingInfo
    {
        [JsonPropertyName("currnet_page_number")]
        public int PageNumber { get; set; }

        [JsonPropertyName("currnet_page_size")]
        public int PageSize { get; set; }

        [JsonPropertyName("total_page_count")]
        public int PageCount { get; set; }

        [JsonPropertyName("total_record_count")]
        public long TotalRecordCount { get; set; }
    }
}
