using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core
{
    [ComplexType]
    public class SlideId : NumberedIdentity
    {
        public SlideId() : base()
        {

        }

        public SlideId(int id) : base(id)
        {

        }
    }

}