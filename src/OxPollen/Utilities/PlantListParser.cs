using Microsoft.Data.Entity.ChangeTracking;
using OxPollen.Data.Concrete;
using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Utilities
{
    /// <summary>
    /// Parses Kew's 'The Plant List' formatted as a GBIF Darwin Core Archive
    /// </summary>
    public class PlantListParser
    {
        private string _filePath;
        private OxPollenDbContext _context;

        public PlantListParser(string filePath, OxPollenDbContext context)
        {
            //TODO check file is CSV
            _filePath = filePath;
            _context = context;
        }

        public void Refresh()
        {
            var parsedTaxa = new List<ParsedPlantListTaxon>();
            var reader = new StreamReader(File.OpenRead(_filePath));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split('	');
                parsedTaxa.Add(new ParsedPlantListTaxon()
                {
                    TaxonId = values[0],
                    AcceptedNameUsageID = values[1],
                    TaxonomicStatus = values[2],
                    Family = values[3],
                    Genus = values[4],
                    ScientificName = values[7],
                    TaxonRank = values[8],
                    ScientificNameAuthorship = values[9]
                });
            }

            parsedTaxa.RemoveAt(0); //Remove header row
            parsedTaxa = parsedTaxa.Where(m => m.TaxonRank == "species").ToList(); //Remove varieties etc.
            foreach (var parsedTaxon in parsedTaxa)
            {
                var taxon = new PlantListTaxon()
                {
                    LatinName = parsedTaxon.ScientificName,
                    Rank = Taxonomy.Species,
                    LatinNameAuthorship = parsedTaxon.ScientificNameAuthorship,
                    Status = (TaxonomicStatus)Enum.Parse(typeof(TaxonomicStatus), parsedTaxon.TaxonomicStatus, true),
                };
                if (taxon.Status != TaxonomicStatus.Accepted) continue; //Drop all species not confirmed
                if (taxon.Rank != Taxonomy.Species) continue; //Ignore varieties etc.

                //Add family
                var family = _context.PlantListTaxa.FirstOrDefault(m => m.LatinName == parsedTaxon.Family
                    && m.Rank == Taxonomy.Family);
                if (family == null)
                {
                    family = new PlantListTaxon()
                    {
                        LatinName = parsedTaxon.Family,
                        Rank = Taxonomy.Family,
                        Status = TaxonomicStatus.Accepted
                    };
                    _context.PlantListTaxa.Add(family);
                    _context.SaveChanges();
                }

                //Add Genus
                var genus = _context.PlantListTaxa.FirstOrDefault(m => m.LatinName == parsedTaxon.Genus
                    && m.Rank == Taxonomy.Genus);
                if (genus == null)
                {
                    genus = new PlantListTaxon()
                    {
                        LatinName = parsedTaxon.Genus,
                        Rank = Taxonomy.Genus,
                        Status = TaxonomicStatus.Accepted
                    };
                    _context.PlantListTaxa.Add(genus);
                    genus.ParentTaxa = family;
                    _context.SaveChanges();
                }

                //Add species
                taxon.ParentTaxa = genus;
                var existing = _context.PlantListTaxa.FirstOrDefault(m => m.LatinName == parsedTaxon.ScientificName
                    && m.Rank == Taxonomy.Species);
                if (existing == null)
                {
                    _context.PlantListTaxa.Add(taxon);
                    taxon.ParentTaxa = genus;
                    _context.SaveChanges();
                }
            }

        }

        private class ParsedPlantListTaxon
        {
            public string TaxonId { get; set; }
            public string AcceptedNameUsageID { get; set; }
            public string TaxonomicStatus { get; set; }
            public string Family { get; set; }
            public string Genus { get; set; }
            public string ScientificName { get; set; }
            public string TaxonRank { get; set; }
            public string ScientificNameAuthorship { get; set; }
        }
    }
}
