using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core.Services
{
    public class SimpleNameConfirmationAlgorithm : INameConfirmationAlgorithm
    {
        private readonly IRepository<IBackboneTaxon> _taxonRepo;

        public SimpleNameConfirmationAlgorithm(IRepository<IBackboneTaxon> taxonRepo)
        {
            _taxonRepo = taxonRepo;
        }

        public IDictionary<Rank, string> ConfirmName(UnknownGrain grain)
        {
            throw new NotImplementedException();
        }

    }
}