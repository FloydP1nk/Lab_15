namespace Lab_15;

public class Task_3
{
    public class SingleRandomizer
    {
        private static volatile SingleRandomizer instance;
        private static readonly object syncRoot = new object();
        private readonly Random random;

        private SingleRandomizer()
        {
            random = new Random();
        }

        public static SingleRandomizer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SingleRandomizer();
                        }
                    }
                }

                return instance;
            }
        }

        public int Next()
        {
            lock (syncRoot)
            {
                return random.Next();
            }
        }
    }


}