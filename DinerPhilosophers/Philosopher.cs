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
        public PhilosopherState State { get; set; }

        // Максимальное количество идущих подряд размышлений без приема пищи
        readonly int _starvationThreshold;

        // Вилки справа и слева относительно философа
        public readonly Fork RightFork;
        public readonly Fork LeftFork;

        // Штука чтобы рандомить :)
        Random rand = new Random();
        
        // Текущая серия размышлений
        int _contThinkCount = 0;

        // Конструктор класса
        public Philosopher(Fork rightFork, Fork leftFork, string name, int starvThreshold)
        {
            RightFork = rightFork;
            LeftFork = leftFork;
            Name = name;
            State = PhilosopherState.Thinking;
            _starvationThreshold = starvThreshold;
        }

        // Метод приёма пищи
        public void Eat()
        {
            // Берём правую вилку в правую руку
            if (TakeForkInRightHand())
            {
                // Если получилось, то берём левую вилку в левую руку незамедлительно
                if (TakeForkInLeftHand())
                {
                    // Если обе вилки в руке, то кушаем
                    // Меняем состояние на "Кушающий"
                    this.State = PhilosopherState.Eating;
                    // Пишем что философ ест и указываем номера вилок
                    Console.WriteLine("(:::) {0} is eating..with {1} and {2}", Name, RightFork.ForkId, LeftFork.ForkId);
                    // Симулируем процесс поедания пищи
                    Thread.Sleep(rand.Next(5000, 10000));

                    // Обнуляем серию размышлений
                    _contThinkCount = 0;

                    // Помещаем вилки обратно
                    RightFork.Put();
                    LeftFork.Put();
                }
                // Получилось взять правую вилку, но левую не получилось
                else
                {
                    // Пробуем подождать ещё чуть-чуть, чтобы попытаться ещё раз
                    Thread.Sleep(rand.Next(100, 400));
                    if (TakeForkInLeftHand())
                    {
                        // Если всё таки получилось взять левую вилку
                        // Меняем состояние на "кушающий"
                        this.State = PhilosopherState.Eating;
                        // Пишем что философ кушает и указываем вилки
                        Console.WriteLine("(:::) {0} is eating..with {1} and {2}", Name, RightFork.ForkId, LeftFork.ForkId);
                        // Симулируем процесс поедания пищи
                        Thread.Sleep(rand.Next(5000, 10000));

                        // Обнуляем серию размышлений без еды
                        _contThinkCount = 0;

                        // Кладём вилки
                        RightFork.Put();
                        LeftFork.Put();
                    }
                    // Если левую вилку так и не получилось взять, то кладём правую
                    else
                    {
                        RightFork.Put();
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
                    Thread.Sleep(rand.Next(100, 400));
                    if (TakeForkInRightHand())
                    {
                        // Если получилось взять правую вилку, то кушаем
                        // Действия здесь идентичны процессу поедания пищи в прошлый раз
                        this.State = PhilosopherState.Eating;
                        Console.WriteLine("(:::) {0} is eating..with {1} and {2}", Name, RightFork.ForkId, LeftFork.ForkId);
                        Thread.Sleep(rand.Next(5000, 10000));

                        _contThinkCount = 0;

                        RightFork.Put();
                        LeftFork.Put();
                    }
                    else
                    {
                        // Если правая вилка все равно оказалась недоступна, то кладём и левую
                        LeftFork.Put();
                    }
                }
            }
            // Размышляем
            Think();
        }

        public void Think()
        {
            // Обозначаем состояние как "Размышляющий"
            this.State = PhilosopherState.Thinking;
            // Пишем, что философ думает и пишем насколько долго он это делает
            Console.WriteLine("^^*^^ {0} is thinking...on {1}", Name, Thread.CurrentThread.Priority.ToString());
            // Симулируем процесс размышления
            Thread.Sleep(rand.Next(2500,20000));
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
            return LeftFork.Take(Name);
        }

        // Метод взятия правой вилки
        private bool TakeForkInRightHand()
        {
            return RightFork.Take(Name);
        }
    }
}