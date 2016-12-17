using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Interfaces;
using Newtonsoft.Json;

namespace GlobalPollenProject.Infrastructure.Communication
{
    public class ExternalDatabaseLinker : IExternalDatabaseLinker
    {
        public async Task<int> GetGlobalBiodiversityInformationFacilityId(string family, string genus, string species)
        {
            var rank = Rank.Family;
            if (!string.IsNullOrEmpty(genus)) rank = Rank.Genus;
            if (!string.IsNullOrEmpty(species)) rank = Rank.Species;
            var result = await GbifUtility.GetGbifId(rank, family, genus, species);
            return result;
        }

        public async Task<int> GetNeotomaDatabaseId(string family, string genus, string species)
        {
            string name;
            if (!string.IsNullOrEmpty(species))
            {
                name = species;
            } else if (!string.IsNullOrEmpty(genus))
            {
                name = genus;
            }
            else {
                name = family;
            }
            var result = await NeotomaUtility.GetTaxonId(name);
            return result;
        }
    }

    public static class NeotomaUtility
    {
        public static async Task<int> GetTaxonId(string name)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.neotomadb.org/v1/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var query = "data/taxa?taxonname=" + name;
                HttpResponseMessage response = await client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    string jsonMessage;
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        jsonMessage = new StreamReader(responseStream).ReadToEnd();
                    }

                    NeotomaResult neotomaResult = (NeotomaResult)JsonConvert.DeserializeObject(jsonMessage, typeof(NeotomaResult));
                    if (neotomaResult.Success == 0) return 0;
                    if (neotomaResult.Result.Count != 1) return 0;
                    return neotomaResult.Result.First().TaxonId;
                }
            }
            return 0;
        }
    }

    class NeotomaResult
    {
        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("data")]
        public List<NeotomaTaxonResult> Result { get; set; }
    }

    class NeotomaTaxonResult
    {
        [JsonProperty("TaxonName")]
        public string TaxonName { get; set; }

        [JsonProperty("TaxonCode")]
        public string TaxonCode { get; set; }

        [JsonProperty("TaxonID")]
        public int TaxonId { get; set; }
    }

    public static class GbifUtility
    {
        /// <summary>
        /// Gets GBIF ID or a particular species, genus or family.
        /// Only retrieves direct matches to increase confidence in results.
        /// Returns zero if a match could not be found or an error occurs.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="family"></param>
        /// <param name="genus"></param>
        /// <param name="species"></param>
        /// <returns>Gbif ID of the directly-matching species</returns>
        public static async Task<int> GetGbifId(Rank rank, string family, string genus, string species)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.gbif.org/v1/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Create query
                var query = "species/match?status=accepted&strict=true&kingdom=Plantae";
                if (!string.IsNullOrEmpty(family))
                {
                    query += "&family=" + family;
                }
                if (!string.IsNullOrEmpty(genus))
                {
                    query += "&genus=" + genus;
                }
                if (!string.IsNullOrEmpty(species))
                {
                    query += "&species=" + species;
                }

                if (rank == Rank.Family)
                {
                    query += "&rank=family&name=" + family;
                }
                else if (rank == Rank.Genus)
                {
                    query += "&rank=genus&name=" + genus;
                }
                else if (rank == Rank.Species)
                {
                    query += "&rank=species&name=" + genus + " " + species;
                }

                HttpResponseMessage response = await client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    string jsonMessage;
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        jsonMessage = new StreamReader(responseStream).ReadToEnd();
                    }

                    GbifTaxonResult gbifResult = (GbifTaxonResult)JsonConvert.DeserializeObject(jsonMessage, typeof(GbifTaxonResult));
                    if (gbifResult.MatchType != "EXACT") return 0;
                    return gbifResult.GbifId;
                }
            }
            return 0;
        }
    }

    class GbifTaxonResult
    {
        [JsonProperty("scientificName")]
        public string ScientificName { get; set; }

        [JsonProperty("canonicalName")]
        public string CanonicalName { get; set; }

        [JsonProperty("rank")]
        public string Rank { get; set; }

        [JsonProperty("matchType")]
        public string MatchType { get; set; }

        [JsonProperty("usageKey")]
        public int GbifId { get; set; }
    }
}