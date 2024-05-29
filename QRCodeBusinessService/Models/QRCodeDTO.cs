using Newtonsoft.Json;

namespace QRCodeService.Models
{
    public class QRCodeDTO
    {
        public string? AgreementId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? BankRequest { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? BankResponse { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? StorageRequest { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? StorageResponse { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Exception { get; set; }
        public QRCode? QRCode { get; set; }
    }
}
