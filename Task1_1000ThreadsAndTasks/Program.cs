using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Task1_1000ThreadsAndTasks
{
    public class Program
    {
        static void Main(string[] args)
        {
            const int count = 1000;
            const int sleepMs = 10;

            Console.WriteLine($"Запуск эксперимента: {count} потоков, каждый Sleep({sleepMs}) ms");

            // Форсируем GC перед началом теста
            GC.Collect();
            GC.WaitForPendingFinalizers(); // Приостанавливаем текущий поток до завершения работы всех финализаторов
            GC.Collect(); // сборка мусора, который мог появиться после выполнения финализаторов

            Console.WriteLine("============= new Thread() =============");
            ShowMemoryInfo("Использование памяти до ThreadProcessing()");
            var resultThreading = ThreadProcessing(count, sleepMs);
            ShowProcessingResult(resultThreading);
            ShowMemoryInfo("Использование памяти после ThreadProcessing()");

            Thread.Sleep(500); // сделаем небольшую паузу вывода результатов

            // Форсируем GC перед началом второго теста
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine("\n============= Task.Run() =============");
            ShowMemoryInfo("Использование памяти до TaskProcessing()");
            var resultTaskProc = TaskProcessing(count, sleepMs);
            ShowProcessingResult(resultTaskProc);
            ShowMemoryInfo("Использование памяти после TaskProcessing()");

            Console.WriteLine("\nСравнение (мс):");
            Console.WriteLine($"AllStarted: Threads={resultThreading.AllStartedMs}, Tasks={resultTaskProc.AllStartedMs}");
            Console.WriteLine($"AllFinished: Threads={resultThreading.AllFinishedMs}, Tasks={resultTaskProc.AllFinishedMs}");
        }

        static Result ThreadProcessing(int count, int sleepMs)
        {
            using var started = new CountdownEvent(count);
            using var finished = new CountdownEvent(count);

            var sw = Stopwatch.StartNew();

            List<Thread> threads = new List<Thread>();

            for(int i = 0; i < count; ++i)
            {
                Thread thread = new Thread(() =>
                {
                    started.Signal();

                    try
                    {
                        Thread.Sleep(sleepMs); // симуляция блокирующей работы
                    }
                    finally
                    {
                        finished.Signal();
                    }
                });

                threads.Add(thread);
                thread.Start();
            }

            started.Wait();
            long allStartedMs = sw.ElapsedMilliseconds;

            finished.Wait();
            long allFinishedMs = sw.ElapsedMilliseconds;

            return new Result(allStartedMs, allFinishedMs);
        }

        static Result TaskProcessing(int count, int sleepMs)
        {
            using var started = new CountdownEvent(count);
            using var finished = new CountdownEvent(count);

            var sw = Stopwatch.StartNew();

            var tasks = new Task[count];

            for(int i = 0; i < count; ++i)
            {
                tasks[i] = Task.Run(async () =>
                {
                    started.Signal();

                    try
                    {
                        /*
                            Используем асинхронную задержку вместо Thread.Sleep(),
                            так как пул потоков не выделяет сразу 1000 потоков, как это делает ОС с Thread.
                            Если использовать Thread.Sleep(), то эффективность по времени будет гораздо ниже,
                            чем работа с 1000 Thread.
                            Дело в том, что после вызова await поток в ThreadPool освобождается и возвращается в него.
                            Этот освободившийся поток может использоваться для выполнения других задач.
                            А по истечении времени задержки задача может быть продолжена в любом доступном
                            потоке из пула => рост производительности.
                         */
                        await Task.Delay(sleepMs);
                    }
                    finally
                    {
                        finished.Signal();
                    }
                });
            }

            started.Wait();
            long allStartedMs = sw.ElapsedMilliseconds;

            finished.Wait();
            long allFinishedMs = sw.ElapsedMilliseconds;

            return new Result(allStartedMs, allFinishedMs);
        }

        static void ShowProcessingResult(Result r)
        {
            Console.WriteLine($"All started  (все вошли в тело): {r.AllStartedMs} ms");
            Console.WriteLine($"All finished (все завершены) : {r.AllFinishedMs} ms");
        }

        static void ShowMemoryInfo(string prefix)
        {
            var process = Process.GetCurrentProcess();
            var info = new MemoryInfo(GC.GetTotalMemory(true), process.WorkingSet64);

            Console.WriteLine($"\n{prefix} Memory Stats:");
            Console.WriteLine($"Working Set: {info.WorkingSet64 / 1024.0} KB");
            Console.WriteLine($"Managed Memory: {info.ManagedMemory / 1024.0} KB");
        }
    }

    record Result(long AllStartedMs, long AllFinishedMs);
    record MemoryInfo(long ManagedMemory, long WorkingSet64);
}
