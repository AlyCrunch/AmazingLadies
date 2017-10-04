using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Web.AmazingLadies.Helpers.APIs.SunDwarf
{
    public class OverwatchAPI
    {
        static HttpClient client = new HttpClient();
        static string path = "api/v3/u/{0}-{1}/stats";

        static OverwatchAPI()
        {
            client.BaseAddress = new Uri("https://owapi.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<RootObject> GetOverwatchProfile(string name, int tag)
        {
            RootObject product = null;
            HttpResponseMessage response = await client.GetAsync(string.Format(path,name,tag));
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var m = JsonConvert.DeserializeObject<RootObject>(json);
            }
            return product;
        }
    }
}
