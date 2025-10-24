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
            var progress = new Progress<int>(percent =>
            {
                Console.WriteLine($"Прогресс операции: {percent}%");
            });

            await OperationAsync(progress);
        }

        static async Task OperationAsync(IProgress<int> progress)
        {
            for(int i = 0; i <= 200; ++i)
            {
                await Task.Delay(20);
                progress.Report((i * 100) / 200); // уведомляем о прогрессе
            }
        }
    }
}
