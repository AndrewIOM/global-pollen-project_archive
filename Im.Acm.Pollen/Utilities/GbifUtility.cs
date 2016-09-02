using Newtonsoft.Json;
using Im.Acm.Pollen.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Im.Acm.Pollen.Utilities
{
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
        public static async Task<int> GetGbifId(Taxonomy rank, string family, string genus, string species)
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

                if (rank == Taxonomy.Family)
                {
                    query += "&rank=family&name=" + family;
                }
                else if (rank == Taxonomy.Genus)
                {
                    query += "&rank=genus&name=" + genus;
                }
                else if (rank == Taxonomy.Species)
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
