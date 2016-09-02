using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Im.Acm.Pollen.Utilities
{
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
}