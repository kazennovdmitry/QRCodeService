using QRCodeService.Contracts;
using QRCodeService.Models;

namespace QRCodeService.Interfaces
{
    public interface IQRCodeBankConnector
    {
        public void Setup(BPMGetQRCodesRequest request);
        public QRCodeDTO GetQRCode(BPMGetQRCodesRequest request, Agreement agreement);
    }
}
