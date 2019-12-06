using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Jay_A4_FunAPI.Models;
using Jay_A4_FunAPI.Services;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Jay_A4_FunAPI.Helper;
using System.Collections.Generic;

namespace Jay_A4_FunAPI.Controllers
{

    public class HomeController : Controller
    {
        FunAPI _funApi = new FunAPI();
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

        public async Task<IActionResult> FunIndex()
        {
            List<FunModel> funs = new List<FunModel>();
            HttpClient client = _funApi.Initial();
            HttpResponseMessage res = await client.GetAsync("api/funapi");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                funs = JsonConvert.DeserializeObject<List<FunModel>>(results);
            }
            return View(funs);
        }

        public async Task<IActionResult> FunDetails(int Id)
        {
            var fun = new FunModel();
            HttpClient client = _funApi.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/funapi/{Id}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                fun = JsonConvert.DeserializeObject<FunModel>(results);
            }
            return View(fun);
        }

        public ActionResult FunCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FunCreate(FunModel fun)
        {
            HttpClient client = _funApi.Initial();

            //HTTP POST
            var postTask = client.PostAsJsonAsync<FunModel>("api/funapi", fun);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("funindex");
            }
            return View(fun);
        }
            
        public async Task<IActionResult> FunEdit(int Id)
        {
            var fun = new FunModel();
            HttpClient client = _funApi.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/funapi/{Id}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                fun = JsonConvert.DeserializeObject<FunModel>(results);
            }
            return View(fun);
        }

        [HttpPost]
        public async Task<FunModel> FunEdit(FunModel fun)
        {
            HttpClient client = _funApi.Initial();

            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/funapi/{fun.FunId}", fun);
            response.EnsureSuccessStatusCode();

            fun = await response.Content.ReadAsAsync<FunModel>();
 
            return fun;
        }



        public async Task<IActionResult> FunDelete(int Id)
        {
            var fun = new FunModel();
            HttpClient client = _funApi.Initial();
            HttpResponseMessage res = await client.DeleteAsync($"api/funapi/{Id}");

            return RedirectToAction("funindex");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
