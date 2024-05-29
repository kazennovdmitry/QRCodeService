using QRCodeService.Contracts;
using QRCodeService.Models;

namespace QRCodeService.Interfaces
{
    public interface IQRCodeBusinessService
    {
        public Task<List<QRCodeDTO>> GetQRCodes(BPMGetQRCodesRequest request);
        public Task<List<QRCodeDTO>> DeleteQRCode(BPMDeleteQRCodeRequest request);
        public Task<List<QRCodeDTO>> GetQRCodeFilesInfo(BPMRepositoryRequest request);
    }
}
