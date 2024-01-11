namespace Lab_15
{
    public class Task_1
    {
        // Интерфейс для наблюдателей

        public interface IObserver
        {
            void Update(string filePath, FileChangeType changeType);
        }

// Типы изменений в файле
        public enum FileChangeType
        {
            Created,
            Deleted,
            Changed
        }

// Класс, представляющий изменение в файле
        public class FileChangedEventArgs : EventArgs
        {
            public string FilePath { get; }
            public FileChangeType ChangeType { get; }

            public FileChangedEventArgs(string filePath, FileChangeType changeType)
            {
                FilePath = filePath;
                ChangeType = changeType;
            }
        }

// Класс, представляющий наблюдаемый объект
        public class FileWatcher
        {
            private readonly List<IObserver> observers = new List<IObserver>();
            private readonly Timer timer;
            private readonly string directoryPath;
            private readonly Dictionary<string, DateTime> fileLastModified = new Dictionary<string, DateTime>();

            public FileWatcher(string directoryPath)
            {
                this.directoryPath = directoryPath;
                timer = new Timer(OnTimerElapsed, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            public void Subscribe(IObserver observer)
            {
                if (!observers.Contains(observer))
                {
                    observers.Add(observer);
                }
            }

            public void Unsubscribe(IObserver observer)
            {
                observers.Remove(observer);
            }

            private void OnTimerElapsed(object state)
            {
                // Получаем все файлы в директории
                string[] files = Directory.GetFiles(directoryPath);

                // Проверяем новые и измененные файлы
                foreach (var filePath in files)
                {
                    DateTime lastModified;

                    if (fileLastModified.TryGetValue(filePath, out lastModified))
                    {
                        if (File.GetLastWriteTime(filePath) > lastModified)
                        {
                            NotifyObservers(filePath, FileChangeType.Changed);
                            fileLastModified[filePath] = DateTime.Now;
                        }
                    }
                    else
                    {
                        fileLastModified.Add(filePath, File.GetLastWriteTime(filePath));
                        NotifyObservers(filePath, FileChangeType.Created);
                    }
                }

                // Проверяем удаленные файлы
                var removedFiles = new List<string>(fileLastModified.Keys);
                foreach (var filePath in removedFiles)
                {
                    if (!Array.Exists(files, f => f == filePath))
                    {
                        fileLastModified.Remove(filePath);
                        NotifyObservers(filePath, FileChangeType.Deleted);
                    }
                }
            }

            private void NotifyObservers(string filePath, FileChangeType changeType)
            {
                foreach (var observer in observers)
                {
                    observer.Update(filePath, changeType);
                }
            }
        }

// Класс-наблюдатель
        public class FileObserver : IObserver
        {
            public void Update(string filePath, FileChangeType changeType)
            {
                Console.WriteLine($"File {filePath} has been {changeType.ToString().ToLower()}");
            }
        }
    }
}