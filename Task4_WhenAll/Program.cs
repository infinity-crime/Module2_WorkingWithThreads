using System.Diagnostics;
using System.Threading.Tasks;

namespace Task4_WhenAll
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var task1 = Task.Run(async () =>
            {
                Console.WriteLine($"Запуск 1 задачи (ThreadID: {Environment.CurrentManagedThreadId})");
                await Task.Delay(1000);
                Console.WriteLine($"Завершение 1 задачи (ThreadID: {Environment.CurrentManagedThreadId})");
            });

            var task2 = Task.Run(async () =>
            {
                Console.WriteLine($"Запуск 2 задачи (ThreadID: {Environment.CurrentManagedThreadId})");
                await Task.Delay(1500);
                Console.WriteLine($"Завершение 2 задачи (ThreadID: {Environment.CurrentManagedThreadId})");
            });

            var task3 = Task.Run(async () =>
            {
                Console.WriteLine($"Запуск 3 задачи (ThreadID: {Environment.CurrentManagedThreadId})");
                await Task.Delay(2000);
                Console.WriteLine($"Завершение 3 задачи (ThreadID: {Environment.CurrentManagedThreadId})");
            });

            /*
                Task.WhenAll() завершится, когда завершится самая длинная из задач.
                Сам Task.Run() ставит делегат в очередь ThreadPool и запускает его
                (по возможности сразу, но бывает вывод, когда первой запускается 3 или 2
                задачи, не смотря на то, что в коде идет по порядку).
             */
            await Task.WhenAll(task1, task2, task3);
            Console.WriteLine($"Время, прошедшее с начала запуска задач до заверешния " +
                $"(возможна погрешность): {sw.ElapsedMilliseconds}");
        }
    }
}
