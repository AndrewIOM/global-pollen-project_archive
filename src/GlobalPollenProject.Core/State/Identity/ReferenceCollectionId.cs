using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core.State.Identity
{
    [ComplexType]
    public class ReferenceCollectionId : Identity
    {
        public ReferenceCollectionId() : base()
        {

        }

        public ReferenceCollectionId(string id) : base(id)
        {

        }
    }

}