using System;
using System.Threading;

namespace DinerPhilosophers
{
    static class Program
    {
        // Константа обозначающая количество философов
        private const int PhilosopherCount = 5;
        // Массив вилок
        private static Fork[] _forks = new Fork[PhilosopherCount];
        // Массив философов
        private static Philosopher[] _philosophers = new Philosopher[PhilosopherCount];
        // Массив потоков
        private static Thread[] _threads = new Thread[PhilosopherCount];
        
        static void Main()
        {
            // Создаём вилки и присваиваем им состояние "На столе"
            for (int i = 0; i < _forks.Length; i++)
            {
                _forks[i] = new Fork
                {
                    ForkId = $"{i + 1}",
                    State = ForkState.OnTheTable
                };
            }

            // Создаём философов и заставляем их думать
            for (int i = 0; i < _philosophers.Length; i++)
            {
                _philosophers[i] = new Philosopher( _forks[(i + 1) % _forks.Length], _forks[i], $"{i + 1}", 5);
                _threads[i] = new Thread(_philosophers[i].Think);
                _threads[i].Start();
            }
            
            // Ждём пока пользователь прекратит это безумное пиршество
            Console.ReadKey();

            // Прекращаем все потоки
            foreach (Thread thread in _threads)
            {
                if (thread.IsAlive)
                {
                    thread.Interrupt();
                }
            }
        }
    }
}