using Newtonsoft.Json;
namespace Lab_15;

public class Task_2
{
    // Интерфейс для репозитория
    public interface ILogRepository
    {
        void SaveLog(string logMessage);
    }

// Реализация репозитория для текстового файла
    public class TextFileLogRepository : ILogRepository
    {
        private readonly string filePath;

        public TextFileLogRepository(string filePath)
        {
            this.filePath = filePath;
        }

        public void SaveLog(string logMessage)
        {
            File.AppendAllText(filePath, $"{logMessage}{Environment.NewLine}");
        }
    }

// Реализация репозитория для JSON-файла
    public class JsonFileLogRepository : ILogRepository
    {
        private readonly string filePath;

        public JsonFileLogRepository(string filePath)
        {
            this.filePath = filePath;
        }

        public void SaveLog(string logMessage)
        {
            var logEntry = new { Timestamp = DateTime.Now, Message = logMessage };
            var serializedLogEntry = JsonConvert.SerializeObject(logEntry);

            File.AppendAllText(filePath, $"{serializedLogEntry}{Environment.NewLine}");
        }
    }

// Класс MyLogger с использованием паттерна Repository
    public class MyLogger
    {
        private readonly List<ILogRepository> repositories;

        public MyLogger(List<ILogRepository> repositories)
        {
            this.repositories = repositories ?? throw new ArgumentNullException(nameof(repositories));
        }

        public void LogMessage(string message)
        {
            foreach (var repository in repositories)
            {
                repository.SaveLog(message);
            }
        }
    }
}