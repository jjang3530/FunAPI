using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Jay_A4_FunAPI.Models;
using Giphy.Libs.Models;
using Giphy.Libs.Services;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Jay_A4_FunAPI.Controllers
{

    public class HomeController : Controller
    {
        public string gifsearch
        {
            get; set;
        }

        public string jokesearch
        {
            get; set;
        }

        private readonly IGiphyServices _giphyServices;

        public HomeController(IGiphyServices giphyServices)
        {
            _giphyServices = giphyServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SearchResults()
        {
            //Get GIF Image
            string searchQuery = Request.Form[nameof(gifsearch)];
            var result = await _giphyServices.GetRandomGifBasedOnSearchCritera(searchQuery);

            //Get Joke
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var url = new Uri($"https://icanhazdadjoke.com/search?term={searchQuery}");
            var response = await client.GetAsync(url);
            string json;
            using (var content = response.Content)
            {
                json = await content.ReadAsStringAsync();
            }
            var joke = JsonConvert.DeserializeObject<JokeModel>(json);

            //Merge Results
            MergeModel model = new MergeModel();
            model.gifResult = result;
            model.jokeResult = joke;
            
            return View(model);
        }

        //public async Task<IActionResult> JokeResults()
        //{
        //    string searchQuery = Request.Form[nameof(jokesearch)];

        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var url = new Uri($"https://icanhazdadjoke.com/search?term={searchQuery}");
        //        var response = await client.GetAsync(url);

        //        string json;
        //        using (var content = response.Content)
        //        {
        //            json = await content.ReadAsStringAsync();
        //        }

        //        return View(JsonConvert.DeserializeObject <JokeModel>(json));
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
