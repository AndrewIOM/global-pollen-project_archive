using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalPollenProject.Core.Imagery;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core.Factories
{
    public class ImageFactory
    {
        private Func<Uri, Uri, List<Uri>, Image> _imageCreate;
        private readonly IImageProcessor _processor;

        public ImageFactory(Func<Uri, Uri, List<Uri>, Image> ctorCaller, IImageProcessor processor)
        {
            _imageCreate = ctorCaller;
            _processor = processor;
        }

        public async Task<Image> TryCreateImage(Base64Image staticImage)
        {
            var uploaded = await _processor.Upload(staticImage);
            return _imageCreate(uploaded.FullSizeImage, uploaded.ThumbnailImage, null);
        }

        public async Task<Image> TryCreateImage(List<Base64Image> focusStack)
        {
            if (focusStack.Count != 5)
            {
                // Currently only support focus stacks of exactly 5 images
                return null;
            }

            var result = new List<SavedImage>();
            foreach (var level in focusStack)
            {
                result.Add(await _processor.Upload(level));
            }

            var savedStack = result.Select(m => m.FullSizeImage).ToList();
            return _imageCreate(result[2].FullSizeImage, result[2].ThumbnailImage, savedStack);
        }
    }
}