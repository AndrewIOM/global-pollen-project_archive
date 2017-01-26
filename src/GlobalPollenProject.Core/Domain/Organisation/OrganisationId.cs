using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core
{
    [ComplexType]
    public class OrganisationId : Identity
    {
        public OrganisationId() : base()
        {

        }

        public OrganisationId(string id) : base(id)
        {

        }
    }

}