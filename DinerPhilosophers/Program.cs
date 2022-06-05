using System;
using System.Threading;

namespace DinerPhilosophers
{
    static class Program
    {
        // Константа обозначающая количество философов
        private const int PHILOSOPHER_COUNT = 5;
        // Массив вилок
        private static Fork[] _forks = new Fork[PHILOSOPHER_COUNT];
        // Массив философов
        private static Philosopher[] _philosophers = new Philosopher[PHILOSOPHER_COUNT];
        // Массив потоков
        private static Thread[] _threads = new Thread[PHILOSOPHER_COUNT];
        
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
                // В массив добавляем объект философа
                _philosophers[i] = new Philosopher( _forks[(i + 1) % _forks.Length], _forks[i], $"{i + 1}", 5);
                // Создаём новый поток для философа. В качестве точки старта берём метод Think
                _threads[i] = new Thread(_philosophers[i].Think);
                // Стартуем поток
                _threads[i].Start();
            }
            
            // Ждём ввода клавиши пользователя, чтобы завершить обед
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