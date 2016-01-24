﻿using System;

namespace OxPollen.Models
{
    public class GrainSearchFilter
    {
        //Sort
        public GrainSort Sort { get; set; }
        public bool Descending { get; set; }

        //Filter by Geography
        public double LatitudeHigh { get; set; }
        public double LatitudeLow { get; set; }
        public double LongitudeHigh { get; set; }
        public double LongitudeLow { get; set; }

        //Filter by ID Level
        public bool UnknownSpecies { get; set; }
        public bool UnknownGenus { get; set; }
        public bool UnknownFamily { get; set; }

        public GrainSearchFilter()
        {
            Sort = GrainSort.Bounty;
            Descending = true;
            LatitudeHigh = 90;
            LatitudeLow = -90;
            LongitudeHigh = 180;
            LongitudeLow = -180;
            UnknownSpecies = true;
            UnknownGenus = true;
            UnknownFamily = true;
        }
    }
}