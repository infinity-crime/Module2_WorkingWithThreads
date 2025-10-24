using System.Threading.Tasks;

namespace Task10_ValueTaskExample
{
    internal class Program
    {
        static UserInfoService _service = new UserInfoService();
        static async Task Main(string[] args)
        {
            var result = await _service.GetDataAsync(12);
            Console.WriteLine($"Результат 1 запроса: {result}");

            var result1 = await _service.GetDataAsync(13);
            Console.WriteLine($"Результат 2 запроса: {result}");

            var result2 = await _service.GetDataAsync(14);
            Console.WriteLine($"Результат 3 запроса: {result}");

            var result3 = await _service.GetDataAsync(13);
            Console.WriteLine($"Результат 4 запроса: {result}");
        }
    }
}
