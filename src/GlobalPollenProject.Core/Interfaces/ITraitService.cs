
namespace GlobalPollenProject.Core.Interfaces
{
    public interface ITraitService
    {
        // Function 1. Get raw data
        // Function 2. Summarise data
        //  2a) Summarise continuous traits (e.g. size). 2b) Summarise ordinal traits (e.g. texture)

        double[] ListSizes(int taxonId);

    }

}
