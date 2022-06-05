using System;
using System.Linq;
using System.Threading;

namespace DinerPhilosophers
{
    static class Program
    {
        // Константа обозначающая количество философов
        private const int PhilosopherCount = 5;
        // Массив вилок
        private static readonly Fork[] Forks = new Fork[PhilosopherCount];
        // Массив философов
        private static readonly Philosopher[] Philosophers = new Philosopher[PhilosopherCount];
        // Массив потоков
        private static readonly Thread[] Threads = new Thread[PhilosopherCount];
        
        static void Main()
        {
            // Создаём вилки и присваиваем им состояние "На столе"
            for (int i = 0; i < Forks.Length; i++)
            {
                Forks[i] = new Fork
                {
                    ForkId = $"{i + 1}",
                    State = ForkState.OnTheTable
                };
            }

            // Создаём философов и заставляем их думать
            for (int i = 0; i < Philosophers.Length; i++)
            {
                // В массив добавляем объект философа
                Philosophers[i] = new Philosopher( Forks[(i + 1) % Forks.Length], Forks[i], $"{i + 1}", 5);
                // Создаём новый поток для философа. В качестве точки старта берём метод Think
                Threads[i] = new Thread(Philosophers[i].Think);
                // Стартуем поток
                Threads[i].Start();
            }
            
            // Ждём ввода клавиши пользователя, чтобы завершить обед
            Console.ReadKey();

            // Прекращаем все потоки
            foreach (Thread thread in Threads)
            {
                if (thread.IsAlive)
                {
                    thread.Interrupt();
                }
            }
            
            PrintStats(Philosophers);
        }

        private static void PrintStats(Philosopher[] philosophers)
        {
            var totalEat = philosophers.Sum(philosopher => philosopher.EatCount);

            if (totalEat <= 0) return;
            {
                Console.WriteLine("Total statistics");
                Console.WriteLine($"Total: {totalEat}");
                foreach (var philosopher in philosophers)
                {
                    Console.WriteLine(
                        $"Philosopher {philosopher.Name} eaten {100.0 * philosopher.EatCount / totalEat}%");
                }
            }
        }
    }
}