using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core
{
    [ComplexType]
    public class BackboneId : NumberedIdentity
    {
        public BackboneId() : base()
        {

        }

        public BackboneId(int id) : base(id)
        {

        }
    }

}