using System.Collections.Generic;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface INameConfirmationAlgorithm
    {
        IDictionary<Rank,string> ConfirmName(UnknownGrain grain);
    }
}