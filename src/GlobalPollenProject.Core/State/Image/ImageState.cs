using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core.State.Identity;

namespace GlobalPollenProject.Core.State
{
    public class ImageState : IEntity
    {
        public ImageId Id { get; set; }
        public string FileName { get; private set; }
        public string FileNameThumbnail { get; private set; }

        public bool IsFocusImage { get; private set; }
        public string FocusLowUrl { get; private set; }
        public string FocusMedLowUrl { get; private set; }
        public string FocusMedUrl { get; private set; }
        public string FocusMedHighUrl { get; private set; }
        public string FocusHighUrl { get; private set; }

        public bool IsDeleted { get; set; }
    }
}