using QRCodeService.Models;
using QRCodeService.Interfaces;

namespace QRCodeService.Contracts
{
    public class BPMBaseRequest
    {
        public List<Agreement>? Agreements { get; set; }
        public string? RequestId { get; set; }
    }

    public class BPMRepositoryRequest : BPMBaseRequest
    {
        public string? FileStorageURL { get; set; }
        public string? FileStorageToken { get; set; }
    }

    public class BPMGetQRCodesRequest : BPMRepositoryRequest
    {
        public string? BankAuthURL { get; set; }
        public string? ClientId { get; set; }
        public string? Secret { get; set; }
        public string? BankApiURL { get; set; }
        public string? BankToken { get; set; }
        public string? DestinationAccount { get; set; }
        public string? StoreLabel { get; set; }
        public string? MerchantName { get; set; }
        public string? MerchantCity { get; set; }
        public string? MerchantId { get; set; }
    }

    public class BPMDeleteQRCodeRequest : BPMRepositoryRequest
    {

    }

    public class Agreement
    {
        public string? AgreementId { get; set; }
        public string? AgreementNumber { get; set; }
        public int Resolution { get; set; }
        public string? Purpose { get; set; }
        public string? QRType { get; set; }
        public string? QRTransactionType { get; set; }
        public string? Currency { get; set; }
        public decimal? Amount { get; set; }
    }

    public class BPMGetQRCodesResponse
    {
        public List<QRCodeDTO>? QRCodeDTObjects { get; set; }
        public string? RequestId { get; set; }
    }
}
