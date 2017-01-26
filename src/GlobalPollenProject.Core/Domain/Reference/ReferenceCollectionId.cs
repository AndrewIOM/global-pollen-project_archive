using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core
{
    [ComplexType]
    public class ReferenceCollectionId : NumberedIdentity
    {
        public ReferenceCollectionId() : base()
        {

        }

        public ReferenceCollectionId(int id) : base(id)
        {

        }
    }

}