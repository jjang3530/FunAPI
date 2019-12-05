using System.Threading.Tasks;
using Jay_A4_FunAPI.Models;

namespace Jay_A4_FunAPI.Services
{
    public interface IGiphyServices
    {
        Task<GiphyModel> GetRandomGifBasedOnSearchCritera(string searchCritera);
    }
}