using System.Threading.Tasks;
using Jay_A4_FunAPI.Models;

namespace Jay_A4_FunAPI.Services
{
    public class GiphyServices : IGiphyServices
    {
        private static IGetRandomGif _getRandomGif;

        public GiphyServices(IGetRandomGif getRandomGif)
        {
            _getRandomGif = getRandomGif;
        }

        public async Task<GiphyModel> GetRandomGifBasedOnSearchCritera(string searchCritera)
        {
            return await _getRandomGif.ReturnRandomGifBasedOnTag(searchCritera);
        }
    }
}