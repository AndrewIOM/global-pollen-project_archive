using Im.Acm.Pollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Im.Acm.Pollen.Tests.Unit.TestBuilders
{
    public static class GrainTestBuilder
    {
        public static List<Grain> GetSampleGrains()
        {
            var imageSample = new GrainImage()
            {
                FileName = "samplefile.jpg",
                FileNameThumbnail = "sample.jpg",
                GrainImageId = 1
            };

            var grains = new List<Grain>();
            grains.Add(new Grain()
            {
                Id = 1,
                TimeAdded = DateTime.Today,
                Images = new List<GrainImage> { imageSample }
            });
            grains.Add(new Grain()
            {
                Id = 2,
                TimeAdded = DateTime.Today - new TimeSpan(2, 0, 0, 0),
                Images = new List<GrainImage> { imageSample }
            });
            grains.Add(new Grain()
            {
                Id = 3,
                TimeAdded = DateTime.Today,
                Images = new List<GrainImage> { imageSample }
            });
            grains.Add(new Grain()
            {
                Id = 4,
                TimeAdded = DateTime.Today - new TimeSpan(365, 0, 0, 0),
                Images = new List<GrainImage> { imageSample }
            });
            grains.Add(new Grain()
            {
                Id = 5,
                TimeAdded = DateTime.Today - new TimeSpan(22, 0, 0, 0),
                Images = new List<GrainImage> { imageSample }
            });
            grains.Add(new Grain()
            {
                Id = 6,
                TimeAdded = DateTime.Today - new TimeSpan(21, 0, 0, 0),
                Images = new List<GrainImage> { imageSample }
            });
            grains.Add(new Grain()
            {
                Id = 7,
                TimeAdded = DateTime.Today - new TimeSpan(15, 0, 0, 0),
                Images = new List<GrainImage> { imageSample }
            });
            grains.Add(new Grain()
            {
                Id = 8,
                TimeAdded = DateTime.Today - new TimeSpan(12, 0, 0, 0),
                Images = new List<GrainImage> { imageSample }
            });
            grains.Add(new Grain()
            {
                Id = 9,
                TimeAdded = DateTime.Today - new TimeSpan(3, 0, 0, 0),
                Images = new List<GrainImage> { imageSample }
            });
            grains.Add(new Grain()
            {
                Id = 10,
                TimeAdded = DateTime.Today - new TimeSpan(55, 0, 0, 0),
                Images = new List<GrainImage> { imageSample }
            });
            grains.Add(new Grain()
            {
                Id = 11,
                TimeAdded = DateTime.Today,
                Images = new List<GrainImage> { imageSample }
            });
            grains.Add(new Grain()
            {
                Id = 12,
                TimeAdded = DateTime.Today
            });
            grains.Add(new Grain()
            {
                Id = 13,
                TimeAdded = DateTime.Today,
                Images = new List<GrainImage> { imageSample }
            });
            return grains;
        }
    }
}
