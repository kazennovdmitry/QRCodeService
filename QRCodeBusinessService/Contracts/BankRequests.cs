using System.Text.Json.Serialization;


namespace QRCodeService.Contracts
{
    public class BankQRCodeRequest
    {
        [JsonPropertyName("qr_type")]
        public string? QRType { get; set; } = "Static";

        [JsonPropertyName("qr_transaction_type")]
        public string? QRTransactionType { get; set; } = "P2M";

        [JsonPropertyName("destination_account")]
        public string? DestinationAccount { get; set; }

        [JsonPropertyName("resolution")]
        public int Resolution { get; set; } = 480;

        [JsonPropertyName("reference_id")]
        public string? ReferenceId { get; set; }

        [JsonPropertyName("purpose")]
        public string? Purpose { get; set; }

        [JsonPropertyName("store_label")]
        public string? StoreLabel { get; set; }

        [JsonPropertyName("merchant_name")]
        public string? MerchantName { get; set; }

        [JsonPropertyName("merchant_city")]
        public string? MerchantCity { get; set; }

        [JsonPropertyName("merchant_id")]
        public string? MerchantId { get; set; }

        [JsonPropertyName("amount")]
        public Amount? Amount { get; set; }
    }

    public class Amount
    {
        [JsonPropertyName("cur")]
        public string? Currency { get; set; } = "PHP"; // ISO 4217 Philippine peso

        [JsonPropertyName("num")]
        public string? Numeric { get; set; }
    }
}
