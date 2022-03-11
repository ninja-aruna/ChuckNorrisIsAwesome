using System;
using System.Threading.Tasks;

namespace ChuckNorrisIsAwesome
{
    internal class Worker
    {
        private readonly ICachedJokeService _cachedJokeService;

        public Worker(ICachedJokeService cachedJokeService)
        {
            _cachedJokeService = cachedJokeService;
        }

        public async Task DoWork()
        {
            var chuck = AsciiArt.GetChuckArt();

            Console.ForegroundColor = ConsoleColor.DarkYellow;

            foreach (var line in chuck) Console.WriteLine(line);

            Console.ResetColor();
            Console.WriteLine(
                "============================================================================================");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Instructions:");
            Console.WriteLine("1. Hit 'j' to fetch a new fact");
            Console.WriteLine("2. Hit 'p' to fetch previous fact");
            Console.WriteLine("3. Hit 'n' to fetch next fact");
            Console.ResetColor();
            Console.WriteLine(
                "============================================================================================");
            Console.ForegroundColor = ConsoleColor.Cyan;
            while (true)
            {
                var cki = Console.ReadKey();
                var index = 0;

                ChuckNorrisFact chuckNorrisFact;

                if (cki.Key == ConsoleKey.J)
                {
                    chuckNorrisFact = await _cachedJokeService.GetNewJoke();
                    index++;
                    Console.WriteLine(
                        "============================================================================================");
                    Console.WriteLine(chuckNorrisFact.Value);
                    Console.WriteLine(
                        "============================================================================================");
                }
                else if (cki.Key == ConsoleKey.P)
                {
                    chuckNorrisFact = _cachedJokeService.GetPreviousJokeFromCache(index - 1);
                    Console.WriteLine(
                        "============================================================================================");
                    Console.WriteLine(chuckNorrisFact.Value);
                    Console.WriteLine(
                        "============================================================================================");
                }
                else if (cki.Key == ConsoleKey.N)
                {
                    chuckNorrisFact = _cachedJokeService.GetNextJokeFromCache(index + 1);
                    Console.WriteLine(
                        "============================================================================================");
                    Console.WriteLine(chuckNorrisFact.Value);
                    Console.WriteLine(
                        "============================================================================================");
                }
                else
                {
                    break;
                }
            }
        }
    }
}