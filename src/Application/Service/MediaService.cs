using DevKid.src.Application.Constant;
using DevKid.src.Application.Dto.ResponseDtos;

namespace DevKid.src.Application.Service
{
    public interface IMediaService
    {
        Task<ResponseDto> UploadMedia(IFormFile file, string type);
    }
    public class MediaService : IMediaService
    {
        private readonly IGCService _gcService;
        public MediaService(IGCService gcService)
        {
            _gcService = gcService;
        }

        public async Task<ResponseDto> UploadMedia(IFormFile file, string type)
        {
            var result = new ResponseDto();
            try
            {
                if (file == null || file.Length == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "File is null";
                    return result;
                }
                var contentType = file.ContentType;
                if (type.Equals(FileConst.IMAGE.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    if (!contentType.Contains("image"))
                    {
                        result.IsSuccess = false;
                        result.Message = "File is not image";
                        return result;
                    }
                    if (file.Length > FileConst.MAX_IMAGE_SIZE)
                    {
                        result.IsSuccess = false;
                        result.Message = "File is too large";
                        return result;
                    }
                }
                else if (type.Equals(FileConst.VIDEO.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    if (!contentType.Contains("video"))
                    {
                        result.IsSuccess = false;
                        result.Message = "File is not video";
                        return result;
                    }
                    if (file.Length > FileConst.MAX_VIDEO_SIZE)
                    {
                        result.IsSuccess = false;
                        result.Message = "File is too large";
                        return result;
                    }
                }
                else if (type.Equals(FileConst.DOC.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    if (!contentType.Contains("application"))
                    {
                        result.IsSuccess = false;
                        result.Message = "File is not document";
                        return result;
                    }
                    if (file.Length > FileConst.MAX_DOC_SIZE)
                    {
                        result.IsSuccess = false;
                        result.Message = "File is too large";
                        return result;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Type is not valid";
                    return result;
                }
                // upload to google cloud
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var fileStream = file.OpenReadStream();
                var url = await _gcService.UploadFileAsync(fileStream, fileName, contentType);
                if (url == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Upload failed";
                    return result;
                }
                result.IsSuccess = true;
                result.Message = "Upload success";
                result.Result = new ResultDto
                {
                    Data = url
                };
                return result;

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = e.Message;
                return result;
            }
        }
    }
}
