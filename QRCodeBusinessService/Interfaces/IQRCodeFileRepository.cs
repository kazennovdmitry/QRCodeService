using QRCodeService.Contracts;
using QRCodeService.Models;

namespace QRCodeService.Interfaces
{
    public interface IQRCodeFileRepository
    {
        public void Setup(BPMRepositoryRequest request);
        public QRCodeDTO GetQRCodeFileInfo(Agreement agreement);
        public void CreateQRCodeFile(QRCodeDTO QRCodeDTO);
        public QRCodeDTO DeleteQRCodeFile(Agreement agreement);
    }
}