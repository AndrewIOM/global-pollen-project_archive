using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class ImageState
    {
        public ImageId Id { get; set; }
        public string FileName { get; set; }
        public string FileNameThumbnail { get; set; }

        public bool IsFocusImage { get; set; }
        public string FocusLowUrl { get; set; }
        public string FocusMedLowUrl { get; set; }
        public string FocusMedUrl { get; set; }
        public string FocusMedHighUrl { get; set; }
        public string FocusHighUrl { get; set; }

        public bool IsDeleted { get; set; }
    }
}