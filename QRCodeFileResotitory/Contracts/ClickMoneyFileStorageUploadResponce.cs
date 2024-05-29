using System.Text.Json.Serialization;

namespace QRCodeService.Repositories.Contracts
{
    public class ClickMoneyFileStorageGetResponce
    {
        [JsonPropertyName("file_id")]
        public string? FileId { get; set; }

        [JsonPropertyName("file_foreign_id")]
        public string? FileForeignId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("descr")]
        public string? Description { get; set; }

        [JsonPropertyName("media_type")]
        public string? MediaType { get; set; }

        [JsonPropertyName("length")]
        public int? Length { get; set; }

        [JsonPropertyName("upload_date")]
        public DateTime? UploadDate { get; set; }

        [JsonPropertyName("result")]
        public string? Result { get; set; }

        [JsonPropertyName("result_message")]
        public string? ResultMessage { get; set; }
    }

    public class ClickMoneyFileStorageUploadResponce
    {
        [JsonPropertyName("file_id")]
        public string? FileId { get; set; }

        [JsonPropertyName("file_foreign_id")]
        public string? FileForeignId { get; set; }

        [JsonPropertyName("upload_date")]
        public DateTime? UploadDate { get; set; }

        [JsonPropertyName("result")]
        public string? Result { get; set; }

        [JsonPropertyName("result_message")]
        public string? ResultMessage { get; set; }
    }

    public class ClickMoneyFileStorageDeleteResponce
    {
        [JsonPropertyName("file_id")]
        public string? FileId { get; set; }

        [JsonPropertyName("result")]
        public string? Result { get; set; }

        [JsonPropertyName("result_message")]
        public string? ResultMessage { get; set; }
    }
}
