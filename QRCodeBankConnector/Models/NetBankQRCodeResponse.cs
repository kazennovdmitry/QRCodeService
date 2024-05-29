using System.Text.Json.Serialization;

namespace QRCodeService.Connectors
{
    internal class NetBankQRCodeResponse
    {
        [JsonPropertyName("qr_code")]
        public string? QRCode { get; init; }
    }
}
