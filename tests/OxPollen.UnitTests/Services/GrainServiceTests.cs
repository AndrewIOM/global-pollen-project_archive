using OxPollen.Models;
using OxPollen.Services.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OxPollen.UnitTests.Services
{
    public class GrainServiceTests
    {
        [Theory]
        public void Add_NewRecordNull_NoGrainAdded()
        {
            var sut = new GrainService();
        }
    }
}
