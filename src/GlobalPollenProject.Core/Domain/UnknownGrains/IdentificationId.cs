using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core
{
    [ComplexType]
    public class IdentificationId : NumberedIdentity
    {
        public IdentificationId() : base()
        {

        }

        public IdentificationId(int id) : base(id)
        {

        }
    }

}