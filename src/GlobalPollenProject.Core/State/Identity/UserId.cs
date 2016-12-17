using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core.State.Identity
{
    [ComplexType]
    public class UserId : Identity
    {
        public UserId() : base()
        {

        }

        public UserId(string id) : base(id)
        {

        }
    }

}