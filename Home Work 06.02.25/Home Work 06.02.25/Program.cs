namespace Home_Work_06._02._25
{
    internal class Program
    {
        private static readonly object lockObject = new object();
        private static Random random = new Random();
        private static int peopleAtStop = 0;
        private const int maxBusCapacity = 30;
        private const int totalBuses = 10;
        private static AutoResetEvent busArrived = new AutoResetEvent(false);

        /// <summary>
        /// Точка входу в програму.
        /// </summary>
        static void Main()
        {
            try
            {
                Thread passengerThread = new Thread(GeneratePassengers);
                Thread busThread = new Thread(OperateBuses);

                passengerThread.Start();
                busThread.Start();

                passengerThread.Join();
                busThread.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка у головному потоці: {ex.Message}");
            }
        }

        /// <summary>
        /// Генерує пасажирів, які прибувають на зупинку.
        /// </summary>
        private static void GeneratePassengers()
        {
            try
            {
                for (int i = 0; i < totalBuses; i++)
                {
                    lock (lockObject)
                    {
                        int newPassengers = random.Next(5, 20);
                        peopleAtStop += newPassengers;
                        Console.WriteLine($"Прибуло {newPassengers} нових пасажирів. Всього на зупинці: {peopleAtStop}");
                    }
                    Thread.Sleep(random.Next(1000, 3000));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка у генерації пасажирів: {ex.Message}");
            }
        }

        /// <summary>
        /// Обробляє прибуття автобусів і посадку пасажирів.
        /// </summary>
        private static void OperateBuses()
        {
            try
            {
                for (int i = 1; i <= totalBuses; i++)
                {
                    Thread.Sleep(random.Next(2000, 5000));
                    lock (lockObject)
                    {
                        int boardingPassengers = Math.Min(peopleAtStop, maxBusCapacity);
                        peopleAtStop -= boardingPassengers;
                        Console.WriteLine($"Автобус №175 приїхав. Взято пасажирів: {boardingPassengers}. Залишилось на зупинці: {peopleAtStop}");
                    }
                    busArrived.Set();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка у роботі автобусів: {ex.Message}");
            }
        }
    }
}
