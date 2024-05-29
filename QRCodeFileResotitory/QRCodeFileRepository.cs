using QRCodeService.Models;
using QRCodeService.Interfaces;
using QRCodeService.Contracts;
using RestSharp;
using QRCodeService.Repositories.Contracts;
using System.Text;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace QRCodeService.Repositories
{
    public class QRCodeFileRepository : IQRCodeFileRepository
    {
        private string _fileStorageURL = string.Empty;
        private string _fileStorageToken = string.Empty;

        private RestClient Client { get; set; } = new RestClient();

        public void Setup(BPMRepositoryRequest request)
        {
            if (!string.IsNullOrEmpty(request.FileStorageURL) &&
                !string.IsNullOrEmpty(request.FileStorageToken))
            {
                _fileStorageURL = request.FileStorageURL;
                _fileStorageToken = request.FileStorageToken;

                GetClient();
            }
        }

        private void GetClient()
        {
            var options = new RestClientOptions();
            options.BaseUrl = new Uri(_fileStorageURL);
            Client = new RestClient(options);
        }

        public QRCodeDTO GetQRCodeFileInfo(Agreement agreement)
        {
            var QRCodeDTO = new QRCodeDTO();
            QRCodeDTO.QRCode = new QRCode();

            if (agreement is not null)
            {
                QRCodeDTO.AgreementId = agreement.AgreementId;
                var request = GetRequest($"/files/{agreement.AgreementId}", Method.Get);

                try
                {
                    var response = Client.Execute(request);

                    if (response is not null && !string.IsNullOrEmpty(response.Content))
                    {
                        var getFileInfoResponse = JsonSerializer.Deserialize<ClickMoneyFileStorageGetResponce>(response.Content);

                        if (getFileInfoResponse is not null && getFileInfoResponse.Result == "ok")
                        {
                            QRCodeDTO.QRCode.FileId = getFileInfoResponse.FileId;
                            QRCodeDTO.QRCode.FileLink = _fileStorageURL + "/files/" + QRCodeDTO.QRCode.FileId;
                        }
                        else if (getFileInfoResponse != null && getFileInfoResponse.ResultMessage != null)
                        {
                            QRCodeDTO.Message = "Filestorage error: " + getFileInfoResponse.ResultMessage.ToString();
                            QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                            QRCodeDTO.StorageResponse = GetRestResponseDetails(response);
                        }
                        else
                        {
                            QRCodeDTO.Message = "Filestorage error";
                            QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                            QRCodeDTO.StorageResponse = GetRestResponseDetails(response);
                        }
                    }
                    else if (response is not null)
                    {
                        QRCodeDTO.Message = $"Filestorage error. StatusCode: {response.StatusCode} , ErrorMessage: {response.ErrorMessage}";
                        QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                        QRCodeDTO.StorageResponse = GetRestResponseDetails(response);
                    }
                    else
                    {
                        QRCodeDTO.Message = $"Filestorage error: Response is null";
                        QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                    }
                }
                catch (Exception e)
                {
                    QRCodeDTO.Message = $"Filestorage Request error: {e.Message}";
                    QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                    QRCodeDTO.Exception = e.ToString();
                }
            }
            else
            {
                QRCodeDTO.Message = "Filestorage error: no agreement data to get the file info";
            }

            return QRCodeDTO;
        }

        public void CreateQRCodeFile(QRCodeDTO QRCodeDTO)
        {
            if (QRCodeDTO is not null
                && QRCodeDTO.AgreementId is not null
                && QRCodeDTO.QRCode is not null 
                && QRCodeDTO.QRCode.QRCodeBytes is not null)
            {
                var agreementId = QRCodeDTO.AgreementId.ToString();
                var fileData = QRCodeDTO.QRCode.QRCodeBytes;
                var fileSize = QRCodeDTO.QRCode.QRCodeBytes.Length;

                var jsonFileToSend = GetFileToSend(agreementId, fileData, fileSize);
                var request = GetRequest($"/files", Method.Post, jsonFileToSend);

                try
                {
                    var response = Client.Execute(request);

                    if (response is not null && response.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
                    {
                        var uploadResponse = JsonSerializer.Deserialize<ClickMoneyFileStorageUploadResponce>(response.Content);

                        if (uploadResponse is not null && uploadResponse.Result == "ok")
                        {
                            QRCodeDTO.QRCode.FileId = uploadResponse.FileId;
                            QRCodeDTO.QRCode.FileLink = _fileStorageURL + "/files/" + QRCodeDTO.QRCode.FileId;
                        }
                        else if (uploadResponse is not null && !string.IsNullOrEmpty(uploadResponse.ResultMessage))
                        {
                            QRCodeDTO.Message = $"Filestorage error: {uploadResponse.ResultMessage}";
                            QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                            QRCodeDTO.StorageResponse = GetRestResponseDetails(response);
                        }
                        else
                        {
                            QRCodeDTO.Message = "Filestorage error";
                            QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                            QRCodeDTO.StorageResponse = GetRestResponseDetails(response);
                        }
                    }
                    else if (response is not null)
                    {
                        QRCodeDTO.Message = $"Filestorage error. StatusCode: {response.StatusCode} , ErrorMessage: {response.ErrorMessage}";
                        QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                        QRCodeDTO.StorageResponse = GetRestResponseDetails(response);
                    }
                    else
                    {
                        QRCodeDTO.Message = $"Filestorage error: Response is null";
                        QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                    }
                }
                catch (Exception e)
                {
                    QRCodeDTO.Message = $"Filestorage Request error: {e.Message}";
                    QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                    QRCodeDTO.Exception = e.ToString();
                }

            }
            else if (QRCodeDTO is not null)
            {
                QRCodeDTO.Message = "Filestorage error: no data to create a file";
            }
        }

        public QRCodeDTO DeleteQRCodeFile(Agreement agreement)
        {
            var QRCodeDTO = new QRCodeDTO();
            QRCodeDTO.QRCode = new QRCode();

            if (agreement is not null)
            {
                QRCodeDTO.AgreementId = agreement.AgreementId;
                var request = GetRequest($"/files/{agreement.AgreementId}", Method.Delete);

                try
                {
                    var response = Client.Execute(request);

                    if (response is not null && response.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
                    {
                        var deleteResponse = JsonSerializer.Deserialize<ClickMoneyFileStorageDeleteResponce>(response.Content);

                        if (deleteResponse is not null && deleteResponse.Result == "ok")
                        {
                            QRCodeDTO.QRCode.FileId = deleteResponse.FileId;
                            QRCodeDTO.QRCode.IsDeleted = true;
                        }
                        else if (deleteResponse is not null && !string.IsNullOrEmpty(deleteResponse.ResultMessage))
                        {
                            QRCodeDTO.Message = $"Filestorage error: {deleteResponse.ResultMessage}";
                            QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                            QRCodeDTO.StorageResponse = GetRestResponseDetails(response);
                        }
                    }
                    else if (response is not null)
                    {
                        QRCodeDTO.Message = $"Filestorage error. StatusCode: {response.StatusCode} , ErrorMessage: {response.ErrorMessage}";
                        QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                        QRCodeDTO.StorageResponse = GetRestResponseDetails(response);
                    }
                    else
                    {
                        QRCodeDTO.Message = $"Filestorage error: Response is null";
                        QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                    }
                }
                catch (Exception e)
                {
                    QRCodeDTO.Message = $"Filestorage Request error: {e.Message}";
                    QRCodeDTO.StorageRequest = GetRestRequestDetails(request);
                    QRCodeDTO.Exception = e.ToString();
                }
            }
            else
            {
                QRCodeDTO.Message = "Filestorage error: no agreement data to delete a file";
            }

            return QRCodeDTO;
        }

        private RestRequest GetRequest(string path, Method method)
        {
            var request = new RestRequest(path, method);
            request.AddHeader("Authorization", _fileStorageToken);
            request.AddHeader("content-type", "application/json");

            return request;
        }

        private RestRequest GetRequest(string path, Method method, string body)
        {
            var request = GetRequest(path, method);
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            return request;
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
                        value = GetRequestValueWithoutContent(parameter.Value)
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

        private string GetRequestValueWithoutContent(object? value)
        {
            if (value is not null)
            {
                var stringValue = value.ToString();

                if (stringValue is not null && stringValue.Contains("content"))
                {
                    try
                    {
                        var index1 = stringValue.IndexOf("content");
                        var index2 = stringValue.IndexOf("backend");

                        return stringValue.Substring(0, index1) + stringValue.Substring(index2);
                    }
                    catch
                    {
                        return stringValue;
                    }
                }
                else if (stringValue is not null)
                {
                    return stringValue;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetRestResponseDetails(RestResponse? response)
        {
            if (response is not null)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"response.StatusCode: {response.StatusCode}");
                stringBuilder.AppendLine($"response.Content: {response.Content}");
                stringBuilder.AppendLine($"response.ErrorMessage: {response.ErrorMessage}");
                return stringBuilder.ToString();
            }

            return string.Empty;
        }

        private string GetFileToSend(string agreementId, byte[] fileData, int fileSize)
        {
            dynamic fileToSend = new JsonObject()
            {
                ["file_foreign_id"] = agreementId,
                ["name"] = agreementId,
                ["descr"] = "QRCode",
                ["media_type"] = "image/jpeg",
                ["length"] = fileSize,
                ["content"] = Convert.ToBase64String(fileData),
                ["backend"] = "aws"
            };

            return JsonSerializer.Serialize(fileToSend);
        }
    }
}
