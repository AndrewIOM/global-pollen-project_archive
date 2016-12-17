using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core.State.Identity
{
    [ComplexType]
    public class ImageId : Identity
    {
        public ImageId() : base()
        {

        }

        public ImageId(string id) : base(id)
        {

        }
    }

}