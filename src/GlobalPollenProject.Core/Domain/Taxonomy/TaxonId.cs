using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core
{
    [ComplexType]
    public class TaxonId : NumberedIdentity
    {
        public TaxonId() : base()
        {

        }

        public TaxonId(int id) : base(id)
        {

        }
    }

}