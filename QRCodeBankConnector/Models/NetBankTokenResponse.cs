using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QRCodeService.Connectors
{
    record TokenResponse
    {
        [JsonPropertyName("token_type")]
        public string? TokenType { get; init; }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; init; }
    }
}
