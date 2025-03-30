using System.Runtime.InteropServices;

namespace DevKid.src.Application.Constant
{
    public static class FileConst
    {
        public const string IMAGE = "image";
        public const string VIDEO = "video";
        public const string DOC = "doc";

        public const int MAX_IMAGE_SIZE = 5 * 1024 * 1024; // 5MB
        public const int MAX_VIDEO_SIZE = 1024 * 1024 * 1024; // 1GB
        public const int MAX_DOC_SIZE = 5 * 1024 * 1024; // 5MB

        public static readonly string[] IMAGE_CONTENT_TYPES = { "image/jpeg", "image/png", "image/gif", "image/webp" };
        public static readonly string[] VIDEO_CONTENT_TYPES = { "video/mp4", "video/avi", "video/quicktime", "video/x-ms-wmv", "video/x-flv", "video/x-matroska" };
        public static readonly string[] DOC_CONTENT_TYPES = { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };

        public static readonly string uploadPath = Environment.GetEnvironmentVariable("UPLOAD_PATH");
        public static readonly string ffmpegPath = Environment.GetEnvironmentVariable("FFMPEG_PATH");
    }
}
