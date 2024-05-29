using Newtonsoft.Json;

namespace QRCodeService.Models
{
    public class QRCode
    {
        public string? FileId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? FileLink { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? QRType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsDeleted { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public byte[]? QRCodeBytes { get; set; }
    }
}
