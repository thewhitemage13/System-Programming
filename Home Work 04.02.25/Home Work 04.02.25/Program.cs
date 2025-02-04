namespace Home_Work_04._02._25
{

    /// <summary>
    /// Клас для імітації роботи автобусної кінцевої зупинки.
    /// </summary>
    internal class Program
    {
        static readonly object locker = new object();
        static int peopleAtStop = 0;
        static readonly Random random = new Random();
        static readonly int busCapacity = 30;
        static readonly int totalBuses = 10;
        static AutoResetEvent busArrived = new AutoResetEvent(false);

        /// <summary>
        /// Точка входу в програму.
        /// Запускає генерацію пасажирів та прибуття автобусів.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                Thread generatorThread = new Thread(GeneratePassengers);
                generatorThread.Start();

                for (int i = 0; i < totalBuses; i++)
                {
                    Thread busThread = new Thread(BusArrives);
                    busThread.Start(i + 1);
                    Thread.Sleep(random.Next(1000, 5000));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка у виконанні програми: {ex.Message}");
            }
        }

        /// <summary>
        /// Генерує випадкову кількість пасажирів на зупинці.
        /// </summary>
        static void GeneratePassengers()
        {
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    int newPeople = random.Next(5, 15);
                    lock (locker)
                    {
                        peopleAtStop += newPeople;
                        Console.WriteLine($"Прибуло {newPeople} людей. Всього на зупинці: {peopleAtStop}");
                    }
                    Thread.Sleep(random.Next(2000, 4000));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка у генерації пасажирів: {ex.Message}");
            }
        }

        /// <summary>
        /// Обробляє прибуття автобуса та посадку пасажирів.
        /// </summary>
        /// <param name="busNumber">Номер автобуса.</param>
        static void BusArrives(object busNumber)
        {
            try
            {
                lock (locker)
                {
                    Console.WriteLine($"Автобус №{busNumber} прибув. Людей на зупинці: {peopleAtStop}");
                    int passengersToBoard = Math.Min(peopleAtStop, busCapacity);
                    peopleAtStop -= passengersToBoard;
                    Console.WriteLine($"Автобус №{busNumber} забрав {passengersToBoard} пасажирів. Залишилось на зупинці: {peopleAtStop}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка у роботі автобуса №{busNumber}: {ex.Message}");
            }
        }
    }
}
