using Microsoft.AspNetCore.Mvc;
using QRCodeService.Contracts;
using Newtonsoft.Json;
using QRCodeService.Interfaces;
using QRCodeService.Authentication;

namespace QRCodeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QRCodeController : ControllerBase
    {
        private readonly IQRCodeBusinessService _QRCodeBusinessService;

        public QRCodeController(IQRCodeBusinessService QRCodeBusinessService)
        {
            _QRCodeBusinessService = QRCodeBusinessService;
        }

        [HttpPost, BasicAuthorization]
        public async Task<ContentResult> CreateQRCodes(BPMGetQRCodesRequest request)
        {
            var QRCodeDTObjects = await _QRCodeBusinessService.GetQRCodes(request);

            BPMGetQRCodesResponse result = new()
            {
                QRCodeDTObjects = QRCodeDTObjects,
                RequestId = request.RequestId
            };

            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpGet, BasicAuthorization]
        public async Task<ContentResult> GetQRCodeFiles(BPMRepositoryRequest request)
        {
            var QRCodeDTObjects = await _QRCodeBusinessService.GetQRCodeFilesInfo(request);

            BPMGetQRCodesResponse result = new()
            {
                QRCodeDTObjects = QRCodeDTObjects,
                RequestId = request.RequestId
            };

            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpDelete, BasicAuthorization]
        public async Task<ContentResult> DeleteQRCodeFiles(BPMDeleteQRCodeRequest request)
        {
            var QRCodeDTObjects = await _QRCodeBusinessService.DeleteQRCode(request);

            BPMGetQRCodesResponse result = new()
            {
                QRCodeDTObjects = QRCodeDTObjects,
                RequestId = request.RequestId
            };

            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
    }
}
