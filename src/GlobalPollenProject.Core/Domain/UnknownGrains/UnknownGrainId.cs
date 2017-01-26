using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core
{
    [ComplexType]
    public class UnknownGrainId : NumberedIdentity
    {
        public UnknownGrainId() : base()
        {

        }

        public UnknownGrainId(int id) : base(id)
        {

        }
    }

}