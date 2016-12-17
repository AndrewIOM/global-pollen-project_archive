using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core.State.Identity
{
    [ComplexType]
    public class TaxonId : Identity
    {
        public TaxonId() : base()
        {

        }

        public TaxonId(string id) : base(id)
        {

        }
    }

}