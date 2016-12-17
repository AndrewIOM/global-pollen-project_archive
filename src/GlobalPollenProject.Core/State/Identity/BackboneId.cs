using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core.State.Identity
{
    [ComplexType]
    public class BackboneId : Identity
    {
        public BackboneId() : base()
        {

        }

        public BackboneId(string id) : base(id)
        {

        }
    }

}