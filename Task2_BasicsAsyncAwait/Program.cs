using System.Threading.Tasks;

namespace Task2_BasicsAsyncAwait
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Начало метода Main() - основного потока");

            var dataStr = await DownloadDataAsync();
            Console.WriteLine($"Полученые данные из метода DownloadDataAsync(): {dataStr}");

            Console.WriteLine("Конец метода Main() - основного потока");
        }

        static async Task<string> DownloadDataAsync()
        {
            await Task.Delay(2000);
            return "DataDataDataData...";
        }
    }
}
