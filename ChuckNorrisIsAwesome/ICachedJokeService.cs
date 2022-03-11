using System.Threading.Tasks;

namespace ChuckNorrisIsAwesome
{
    public interface ICachedJokeService
    {
        Task<ChuckNorrisFact> GetNewJoke();

        ChuckNorrisFact GetPreviousJokeFromCache(int index);

        Task<ChuckNorrisFact> GetNextJokeFromCache(int index);

        Task PopulateCache(int numberOfJokes);
    }
}