using System;

namespace GlobalPollenProject.Core.Imagery
{
    public class Base64Image
    {
        private Base64Image(string encodedString)
        {
            this.EncodedImage = encodedString;
        }

        public static Base64Image TryCreateBase64Image(string encodedString)
        {
            if (IsBase64Image(encodedString)) return new Base64Image(encodedString);
            return null;
        }

        public string EncodedImage {get; private set; }

        private static bool IsBase64Image(string encoded)
        {
            try
            {
                byte[] data = Convert.FromBase64String(encoded);
                return (encoded.Replace(" ", "").Length % 4 == 0);
            }
            catch
            {
                return false;
            }
        }

    }
}