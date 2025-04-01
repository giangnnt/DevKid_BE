using DevKid.src.Application.Constant;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Principal;
using static DevKid.src.Domain.Entities.Material;

namespace DevKid.src.Application.Service
{
    public interface IMediaService
    {
        Task<ResponseDto> UploadMedia(IFormFile file, string type);
        Task<ResponseDto> UploadVideo(IFormFile file, Guid lessonId);
    }
    public class MediaService : IMediaService
    {
        private readonly IGCService _gcService;
        private readonly ILessonRepo _lessonRepo;
        private readonly IMaterialRepo _materialRepo;
        public MediaService(IGCService gcService, ILessonRepo lessonRepo, IMaterialRepo materialRepo)
        {
            _gcService = gcService;
            _lessonRepo = lessonRepo;
            _materialRepo = materialRepo;
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

        public async Task<ResponseDto> UploadVideo(IFormFile file, Guid lessonId)
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
                if (Array.Exists(FileConst.VIDEO_CONTENT_TYPES, ct => ct.Equals(contentType, StringComparison.OrdinalIgnoreCase)))
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
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Type is not valid";
                    return result;
                }
                // add new material
                var material = await _materialRepo.ReturnAddMaterial(new Material
                {
                    Name = file.FileName,
                    Type = MaterialType.Video,
                    Url = "",
                    LessonId = lessonId
                });
                if (material == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Upload failed";
                    return result;
                }
                // generate guid file name
                string guid = material.Id.ToString();
                var lesson = await _lessonRepo.GetLessonById(lessonId);
                var dirPath = Path.Combine(FileConst.uploadPath, lessonId.ToString());
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                var filePath = Path.Combine(dirPath, $"{guid}.mp4");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                // dirPath = /upload/lessonId
                // chunkFolder = /upload/lessonId/fileName
                // chunkPath = /upload/lessonId/fileName/filename.mpd
                var chunkFolder = Path.Combine(dirPath, guid);
                var chunkPath = Path.Combine(chunkFolder, $"{guid}.mpd");
                if (!Directory.Exists(chunkFolder))
                {
                    Directory.CreateDirectory(chunkFolder);
                }
                var ffmpegAgrs = $"-i {filePath} -c:v libx264 -crf 23 -c:a aac -b:a 128k -f dash -seg_duration 5 -init_seg_name init-stream$RepresentationID$.m4s -media_seg_name chunk-stream$RepresentationID$-$Number%05d$.m4s {chunkPath}";
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = FileConst.ffmpegPath,
                        Arguments = ffmpegAgrs,
                        WorkingDirectory = chunkFolder,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                using (process)
                {
                    process.Start();
                    Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                    Task<string> errorTask = process.StandardError.ReadToEndAsync();
                    Console.WriteLine($"Output:{outputTask}");
                    Console.WriteLine($"Error: {errorTask}");
                    await process.WaitForExitAsync();
                }
                // delete original file
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine("Deleted");
                }
                else
                {
                    Console.WriteLine("Not exist file");
                }
                // update material url
                material.Url = chunkPath;
                var response = await _materialRepo.UpdateMaterial(material);
                if (!response)
                {
                    result.IsSuccess = false;
                    result.Message = "Upload failed";
                    return result;
                }
                else
                {
                    result.IsSuccess = true;
                    result.Message = "Upload and chunk successfully";
                    return result;
                }
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
