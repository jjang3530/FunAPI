using System;
using System.Net.Http;
using System.Threading.Tasks;
using Jay_A4_FunAPI.Models;
using Newtonsoft.Json;

namespace Jay_A4_FunAPI
{
    public class GetRandomGif : IGetRandomGif
    {
        public async Task<GiphyModel> ReturnRandomGifBasedOnTag(string searchCritera)
        {
            const string giphyKey = "RxjQEbIfrOmYOXmW7pW9GWV0ic8ne9Y6";

            using (var client = new HttpClient())
            {
                var url = new Uri($"http://api.giphy.com/v1/gifs/search?api_key={giphyKey}&q={searchCritera}&limit=1");

                var response = await client.GetAsync(url);

                string json;
                using (var content = response.Content)
                {
                    json = await content.ReadAsStringAsync();
                }

                return JsonConvert.DeserializeObject<GiphyModel>(json);
            }
        }
    }
}