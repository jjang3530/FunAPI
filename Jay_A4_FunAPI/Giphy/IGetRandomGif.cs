using System.Threading.Tasks;
using Jay_A4_FunAPI.Models;

namespace Jay_A4_FunAPI
{
    public interface IGetRandomGif
    {
        Task<GiphyModel> ReturnRandomGifBasedOnTag(string searchCritera);
    }
}