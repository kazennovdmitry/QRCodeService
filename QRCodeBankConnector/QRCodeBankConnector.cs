using QRCodeService.Interfaces;
using QRCodeService.Contracts;
using RestSharp;
using QRCodeService.Models;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace QRCodeService.Connectors
{
    public class QRCodeBankConnector : IQRCodeBankConnector
    {
        private string _bankAuthkUrl = string.Empty;
        private string _clientId = string.Empty;
        private string _clientSecret = string.Empty;
        private string _bankApiUrl = string.Empty;
        private readonly ILogger<QRCodeBankConnector> _logger;

        private string? RequestId { get; set; }

        public void Setup(BPMGetQRCodesRequest request)
        {
            if (!string.IsNullOrEmpty(request.BankAuthURL) &&
                !string.IsNullOrEmpty(request.ClientId) &&
                !string.IsNullOrEmpty(request.Secret) &&
                !string.IsNullOrEmpty(request.BankApiURL))
            {
                _bankAuthkUrl = request.BankAuthURL;
                _clientId = request.ClientId;
                _clientSecret = request.Secret;
                _bankApiUrl = request.BankApiURL!;
            }

            RequestId = request.RequestId is null ? string.Empty : request.RequestId;
        }

        public QRCodeBankConnector(ILogger<QRCodeBankConnector> logger)
        {
            _logger = logger;
        }

        public QRCodeDTO GetQRCode(BPMGetQRCodesRequest getCodesRequest, Agreement agreement)
        {
            var bankQRCodeRequest = MapBankQRCodeRequest(getCodesRequest, agreement);

            var QRCodeDTO = new QRCodeDTO() 
            { 
                AgreementId = agreement.AgreementId,
                QRCode = new QRCode()
            };

            var options = new RestClientOptions();
            options.Authenticator = new NetBankAuthenticator(_bankAuthkUrl, _clientId, _clientSecret);
            options.BaseUrl = new Uri(_bankApiUrl);

            var client = new RestClient(options);

            var request = new RestRequest("v1/qrph/generate", Method.Post);
            request.AddHeader("Content-type", "application/json");
            request.AddParameter("application/json",
                JsonSerializer.Serialize(bankQRCodeRequest),
                ParameterType.RequestBody);

            try
            {
                var response = client.Execute(request);

                if (response is not null && !string.IsNullOrEmpty(response.Content))
                {
                    var bankQRCodeResponse = JsonSerializer.Deserialize<NetBankQRCodeResponse>(response.Content);

                    if (bankQRCodeResponse is not null && bankQRCodeResponse.QRCode is not null)
                    {
                        QRCodeDTO.QRCode.QRCodeBytes = Convert.FromBase64String(bankQRCodeResponse.QRCode);
                        QRCodeDTO.QRCode.QRType = bankQRCodeRequest.QRType;

                        _logger.LogInformation($"BankConnector GetQRCode RequestId {RequestId} AgreementId {agreement.AgreementId} Success.");
                    }
                    else
                    {
                        QRCodeDTO.Message = "Bank Response error: qr_code is null";
                        QRCodeDTO.BankRequest = GetRestRequestDetails(request);
                        QRCodeDTO.BankResponse = GetRestResponseDetails(response);
                    }
                }
                else
                {
                    QRCodeDTO.Message = "Bank Response error. Response.Content is empty";
                    QRCodeDTO.BankRequest = GetRestRequestDetails(request);
                    QRCodeDTO.BankResponse = GetRestResponseDetails(response);
                }
            }
            catch (Exception e)
            {
                QRCodeDTO.Message = $"Bank Request error: {e.Message}";
                QRCodeDTO.BankRequest = GetRestRequestDetails(request);
                QRCodeDTO.Exception = e.ToString();
            }

            LogNonEmptyString(QRCodeDTO.Message);
            LogNonEmptyString(QRCodeDTO.BankRequest);
            LogNonEmptyString(QRCodeDTO.BankResponse);
            LogNonEmptyString(QRCodeDTO.Exception);

            return QRCodeDTO;
        }

        private string GetRestRequestDetails(RestRequest request)
        {
            if (request is not null)
            {
                var parameters = request.Parameters
					.Where(parameter => parameter.Name != "Authorization")
					.Select(parameter => new
					{
						name = parameter.Name,
                        type = parameter.Type.ToString(),
                        value = parameter.Value
					});

                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine($"HTTP method: {request.Method.ToString()}");

                foreach (var parameter in parameters)
                {
                    stringBuilder.AppendLine(parameter.ToString());
                }

                return stringBuilder.ToString();
            }

            return string.Empty;
        }

        private string GetRestResponseDetails(RestResponse? response)
        {
            if (response is not null)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"response.StatusCode: {response.StatusCode}");
                stringBuilder.AppendLine($"response.ErrorMessage: {response.ErrorMessage}");
                return stringBuilder.ToString();
            }

            return string.Empty;
        }

        private BankQRCodeRequest MapBankQRCodeRequest(BPMGetQRCodesRequest request, Agreement agreement)
        {
            var amount = new Amount()
            {
                Numeric = GetNumericAmount(agreement.Amount)
            };

            if (!string.IsNullOrEmpty(agreement.Currency))
            {
                amount.Currency = agreement.Currency; // ISO 4217 Philippine peso by default.
            }

            var bankQRCodeRequest = new BankQRCodeRequest()
            {
                DestinationAccount = request.DestinationAccount,
                StoreLabel = request.StoreLabel,
                MerchantName = request.MerchantName,
                MerchantCity = request.MerchantCity,
                MerchantId = request.MerchantId,
                Resolution = agreement.Resolution,
                ReferenceId = agreement.AgreementNumber,
                Purpose = agreement.Purpose,
                Amount = amount
            };

            if(!string.IsNullOrEmpty(agreement.QRType))
            {
                bankQRCodeRequest.QRType = agreement.QRType;
            }

            if (!string.IsNullOrEmpty(agreement.QRTransactionType))
            {
                bankQRCodeRequest.QRTransactionType = agreement.QRTransactionType;
            }

            return bankQRCodeRequest;
        }

        private void LogNonEmptyString(string? value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _logger.LogInformation(value);
            }
        }

        // The value must include the 2 decimal places without the decimal point (e.g. 50000 = 500.00). See the API.
        private string GetNumericAmount(decimal? amount)
        {
            var nonNullableAmount = amount.GetValueOrDefault();
            return ((int)Math.Truncate(nonNullableAmount * 100)).ToString();
        }
    }
}