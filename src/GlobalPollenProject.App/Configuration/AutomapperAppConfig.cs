using System;
using AutoMapper;
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.App
{
    public class AutoMapperWebConfiguration : Profile
    {
        [Obsolete]
        protected override void Configure()
        {
            CreateMap<Core.Models.ReferenceCollection, DigitisedCollection>();
        }
    }
}