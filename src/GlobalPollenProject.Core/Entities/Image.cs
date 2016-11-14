using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Factories;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class Image : IEntity
    {
        private Image() {}
        private Image(Uri image, Uri thumbnail, List<Uri> stack = null) 
        {
            this.FileName = image.AbsoluteUri;
            this.FileNameThumbnail = image.AbsoluteUri;
            if (stack != null)
            {
                if (stack.Count > 0)
                {
                    IsFocusImage = true;
                    this.FocusLowUrl = stack[0].AbsoluteUri;
                    this.FocusMedLowUrl = stack[1].AbsoluteUri;
                    this.FocusMedUrl = stack[2].AbsoluteUri;
                    this.FocusMedHighUrl = stack[3].AbsoluteUri;
                    this.FocusHighUrl = stack[4].AbsoluteUri;
                }
            }
        }

        public static ImageFactory GetFactory(IImageProcessor processor)
        {
            return new ImageFactory((i,t,s) => new Image (i,t,s), processor);
        }

        public int Id { get; set; }
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