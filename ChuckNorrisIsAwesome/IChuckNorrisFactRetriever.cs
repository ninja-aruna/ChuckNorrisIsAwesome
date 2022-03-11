using System.Threading.Tasks;

namespace ChuckNorrisIsAwesome
{
    public interface IChuckNorrisFactRetriever
    {
        Task<ChuckNorrisFact> GetChuckNorrisFactFromApi();
    }
}