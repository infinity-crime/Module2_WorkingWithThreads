namespace Task9_Progress
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            /*
                Создаем экземпляр Progress<double> для вывода в консоль состояние
                операции в процентном соотношении.
                Преимущество этого механизма является то, что он захватывает текущий 
                контекст синхронизации (например в UI-приложениях).
                Когда вызывается Report(), обработчик прогресса выполняется в данном
                контексте. Это позволяет безопасно обновлять UI.
                Но пример приведен к консольном приложении, где нет UI-потока.
             */
            var progress = new Progress<double>(percent =>
            {
                Console.WriteLine($"Прогресс операции: {percent:F1}%");
            });

            await OperationAsync(progress);
        }

        static async Task OperationAsync(IProgress<double> progress)
        {
            for(int i = 0; i <= 200; ++i)
            {
                await Task.Delay(20);
                progress.Report((i / 200.0) * 100.0); // уведомляем о прогрессе
            }
        }
    }
}
