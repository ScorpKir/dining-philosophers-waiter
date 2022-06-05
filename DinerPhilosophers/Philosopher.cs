using System;
using System.Threading;

namespace DinerPhilosophers
{
    // Перечисление описывает состояния философа
    public enum PhilosopherState
    {
        // В данный момент ест
        Eating,
        // В данный момент думает
        Thinking
    }

    // Класс описывает сущность философа
    public class Philosopher
    {
        // Имя философа
        public string Name { get; set; }

         // Состояние философа
         private PhilosopherState _state;

        // Максимальное количество идущих подряд размышлений без приема пищи
        private readonly int _starvationThreshold;

        // Вилки справа и слева относительно философа
        private readonly Fork _rightFork;
        private readonly Fork _leftFork;

        // Штука чтобы рандомить :)
        private readonly Random _rand = new Random();
        
        // Текущая серия размышлений
        private int _contThinkCount;
        
        public int EatCount { get; private set; }

        // Конструктор класса
        public Philosopher(Fork rightFork, Fork leftFork, string name, int starvThreshold)
        {
            _rightFork = rightFork;
            _leftFork = leftFork;
            Name = name;
            _state = PhilosopherState.Thinking;
            _starvationThreshold = starvThreshold;
            _contThinkCount = 0;
            EatCount = 0;
        }

        // Метод приёма пищи
        private void Eat()
        {
            // Берём правую вилку в правую руку
            if (TakeForkInRightHand())
            {
                // Если получилось, то берём левую вилку в левую руку незамедлительно
                if (TakeForkInLeftHand())
                {
                    // Если обе вилки в руке, то кушаем
                    // Меняем состояние на "Кушающий"
                    _state = PhilosopherState.Eating;
                    // Пишем что философ ест и указываем номера вилок
                    Console.WriteLine("(:::) {0} is eating..with {1} and {2}", Name, _rightFork.ForkId, _leftFork.ForkId);
                    // Симулируем процесс поедания пищи
                    Thread.Sleep(_rand.Next(5000, 10000));

                    // Обнуляем серию размышлений
                    _contThinkCount = 0;
                    // Увеличиваем количество съеденной еды
                    EatCount++;

                    // Помещаем вилки обратно
                    _rightFork.Put();
                    _leftFork.Put();
                }
                // Получилось взять правую вилку, но левую не получилось
                else
                {
                    // Пробуем подождать ещё чуть-чуть, чтобы попытаться ещё раз
                    Thread.Sleep(_rand.Next(100, 400));
                    if (TakeForkInLeftHand())
                    {
                        // Если всё таки получилось взять левую вилку
                        // Меняем состояние на "кушающий"
                        _state = PhilosopherState.Eating;
                        // Пишем что философ кушает и указываем вилки
                        Console.WriteLine("(:::) {0} is eating..with {1} and {2}", Name, _rightFork.ForkId, _leftFork.ForkId);
                        // Симулируем процесс поедания пищи
                        Thread.Sleep(_rand.Next(5000, 10000));

                        // Обнуляем серию размышлений без еды
                        _contThinkCount = 0;
                        // Увеличиваем количество съеденной еды
                        EatCount++;

                        // Кладём вилки
                        _rightFork.Put();
                        _leftFork.Put();
                    }
                    // Если левую вилку так и не получилось взять, то кладём правую
                    else
                    {
                        _rightFork.Put();
                    }
                }
            }
            // Если не вышло взять правую вилку
            else
            {
                // Берём левую вилку
                if (TakeForkInLeftHand())
                {
                    // Ждём небольшой период времени чтобы попытаться взять правую вилку 
                    Thread.Sleep(_rand.Next(100, 400));
                    if (TakeForkInRightHand())
                    {
                        // Если получилось взять правую вилку, то кушаем
                        // Действия здесь идентичны процессу поедания пищи в прошлый раз
                        this._state = PhilosopherState.Eating;
                        Console.WriteLine("(:::) {0} is eating..with {1} and {2}", Name, _rightFork.ForkId, _leftFork.ForkId);
                        Thread.Sleep(_rand.Next(5000, 10000));

                        _contThinkCount = 0;
                        EatCount++;

                        _rightFork.Put();
                        _leftFork.Put();
                    }
                    else
                    {
                        // Если правая вилка все равно оказалась недоступна, то кладём и левую
                        _leftFork.Put();
                    }
                }
            }
            // Размышляем
            Think();
        }

        public void Think()
        {
            // Обозначаем состояние как "Размышляющий"
            this._state = PhilosopherState.Thinking;
            // Пишем, что философ думает и пишем насколько долго он это делает
            Console.WriteLine("^^*^^ {0} is thinking...on {1}", Name, Thread.CurrentThread.Priority.ToString());
            // Симулируем процесс размышления
            Thread.Sleep(_rand.Next(2500,20000));
            // Увеличиваем серию размышлений на 1
            _contThinkCount++;

            if (_contThinkCount > _starvationThreshold)
            {
                // Если серия размышлений достигла предела, то пишем, что философ голодает
                Console.WriteLine(":ooooooooooooooooooooooooooooooooooooooooooooooo: {0} is starving", Name);
            }

            // Пытаемся поесть
            Eat();
        }

        // Метод взятия левой вилки
        private bool TakeForkInLeftHand()
        {
            return _leftFork.Take(Name);
        }

        // Метод взятия правой вилки
        private bool TakeForkInRightHand()
        {
            return _rightFork.Take(Name);
        }
    }
}