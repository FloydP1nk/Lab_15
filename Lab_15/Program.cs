namespace Lab_15
{
    class Program
    {
        static void Main()
        {
            string task = Console.ReadLine();
            switch (task)
            {
                case "1": // Task_1

                    var fileWatcher = new Task_1.FileWatcher("path_to_direktory");
                    var fileObserver = new Task_1.FileObserver();

                    fileWatcher.Subscribe(fileObserver);

                    Console.WriteLine("FileWatcher is running. Press Enter to stop.");
                    Console.ReadLine();

                    fileWatcher.Unsubscribe(fileObserver);
                    break;
                case "2": // Task_2

                    var textFileRepository = new Task_2.TextFileLogRepository("log.txt");
                    var jsonFileRepository = new Task_2.JsonFileLogRepository("log.json");

                    var logger = new Task_2.MyLogger(new List<Task_2.ILogRepository>
                        { textFileRepository, jsonFileRepository });

                    // Логирование сообщения
                    logger.LogMessage("This is a log message.");

                    Console.WriteLine("Log messages have been written to text and JSON files.");
                    break;
                case "3": // Task_3

                    for (int i = 0; i < 5; i++)
                    {
                        // В разных потоках можно использовать один и тот же экземпляр SingleRandomizer
                        var thread = new System.Threading.Thread(() =>
                        {
                            var randomizer = Task_3.SingleRandomizer.Instance;
                            Console.WriteLine(
                                $"Thread {System.Threading.Thread.CurrentThread.ManagedThreadId}: {randomizer.Next()}");
                        });

                        thread.Start();
                        thread.Join();
                    }

                    break;
            }
        }
    }
}