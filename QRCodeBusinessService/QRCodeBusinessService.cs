using QRCodeService.Contracts;
using QRCodeService.Interfaces;
using QRCodeService.Models;

namespace QRCodeService.BusinessServices
{
    public class QRCodeBusinessService : IQRCodeBusinessService
    {
        private readonly IQRCodeFileRepository _QRCodeFileRepository;
        private readonly IQRCodeBankConnector _QRCodeBankConnector;

        public QRCodeBusinessService(IQRCodeFileRepository QRCodeFileRepository, IQRCodeBankConnector QRCodeBankConnector)
        {
            _QRCodeFileRepository = QRCodeFileRepository;
            _QRCodeBankConnector = QRCodeBankConnector;
        }

        public async Task<List<QRCodeDTO>> GetQRCodes(BPMGetQRCodesRequest request)
        {
            var QRCodeDTObjects = new List<QRCodeDTO>();

            _QRCodeBankConnector.Setup(request);
            _QRCodeFileRepository.Setup(request);

            if (request.Agreements is not null)
            {
                foreach (var agreement in request.Agreements)
                {
                    var QRCodeDTO = _QRCodeBankConnector.GetQRCode(request, agreement);
                    
                    if (string.IsNullOrEmpty(QRCodeDTO.Message) 
                        && QRCodeDTO.QRCode?.QRCodeBytes is not null 
                        && agreement.AgreementId is not null)
                    {
                        _QRCodeFileRepository.CreateQRCodeFile(QRCodeDTO);
                    }

                    QRCodeDTO.QRCode.QRCodeBytes = null;
                    QRCodeDTObjects.Add(QRCodeDTO);
                }
            }

            return await Task.FromResult(QRCodeDTObjects);
        }

        public async Task<List<QRCodeDTO>> DeleteQRCode(BPMDeleteQRCodeRequest request)
        {
            var QRCodeDTObjects = new List<QRCodeDTO>();

            _QRCodeFileRepository.Setup(request);

            if (request.Agreements is not null)
            {
                foreach (var agreement in request.Agreements)
                {
                    var QRCodeDTO = _QRCodeFileRepository.DeleteQRCodeFile(agreement);
                    QRCodeDTObjects.Add(QRCodeDTO);
                }
            }

            return await Task.FromResult(QRCodeDTObjects);
        }

        public async Task<List<QRCodeDTO>> GetQRCodeFilesInfo(BPMRepositoryRequest request)
        {
            var QRCodeDTObjects = new List<QRCodeDTO>();

            _QRCodeFileRepository.Setup(request);

            if (request.Agreements is not null)
            {
                foreach (var agreement in request.Agreements)
                {
                    var QRCodeDTO = _QRCodeFileRepository.GetQRCodeFileInfo(agreement);
                    QRCodeDTObjects.Add(QRCodeDTO);
                }
            }

            return await Task.FromResult(QRCodeDTObjects);
        }
    }
}