namespace FayoumGovPortal.Core.Umbraco.MediaFiles.Models
{
    public class MediaNotificationHandlerSettings
    {
        public int ArticleSizeInMegaByte { get; set; }
        public int AudioSizeInMegaByte { get; set; }
        public int FileSizeInMegaByte { get; set; }
        public int SVgsizeInMegaByte { get; set; }
        public int VideoSizeInMegaByte { get; set; }
        public int ImageSizeInMegaByte { get; set; }
        public string? MembersRegistrationAllowedExtensions { get; set; }
        public string? AllowedExtensions { get; set; }
        public string? AllowedFileExtensions { get; set; }
    }
}
