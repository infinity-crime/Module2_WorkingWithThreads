using System.Threading.Tasks;

namespace Task7_IAsyncEnumerable
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Текущий поток (Main): {Environment.CurrentManagedThreadId}");
            await foreach(var number in GetNumbersAsync())
            {
                Console.WriteLine($"Полученное число из асинхронного стрима: {number}");
            }
        }

        /*
            Первый вызов MoveNextAsync() будет происходить в текущем потоке, затем в
            ThreadPool. Это можно заметить в выводе информации в консоли о текущем
            Id потока.
         */
        static async IAsyncEnumerable<int> GetNumbersAsync()
        {
            for(int i = 0; i < 4; ++i)
            {
                Console.WriteLine($"Текущий поток: {Environment.CurrentManagedThreadId}");
                await Task.Delay(1000);
                yield return i * 2;
            }
        }
    }
}
