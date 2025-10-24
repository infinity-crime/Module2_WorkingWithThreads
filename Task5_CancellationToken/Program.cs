namespace Task5_CancellationToken
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            using CancellationTokenSource cts = new CancellationTokenSource();
            var token = cts.Token;

            try
            {
                var task = SomethingOperationAsync(token);

                Thread.Sleep(2000); // ждем 2 секунды
                cts.Cancel(); // отменяем задачу

                await task;
            }
            catch(OperationCanceledException)
            {
                Console.WriteLine("Асинхронная задача отменена");
            }
        }

        static async Task SomethingOperationAsync(CancellationToken cancellationToken)
        {
            for(int i = 0; i < 10; ++i)
            {
                cancellationToken.ThrowIfCancellationRequested(); // выбрасываем исключение, если отменилась

                // Если пришел сигнал отмены на момент Task.Delay(), то будет тоже выбрашено исключение
                await Task.Delay(500, cancellationToken);

                Console.WriteLine($"Работа номер {i} выполнена!");
            }
        }
    }
}
