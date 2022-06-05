using System;

namespace DinerPhilosophers
{
    // Перечисление состояний вилки
    public enum ForkState
    {
        // Вилка взята
        Taken, 
        // Вилка на столе
        OnTheTable
    }

    // Класс описывает сущность вилки
    public class Fork
    {
        // Номер вилки
        public string ForkId { get; set; }
        // Состояние вилки
        public ForkState State { get; set; }
        
        // Кем взята вилка
        private string _takenBy;
        
        // Метод взятия вилки
        public bool Take(string takenBy)
        {
            
            // объект - заглушка, нужен для синхронизации потока.
            object lockObject = new object();
            // Блокируем доступ к следующему блоку кода для других потоков
            lock (lockObject)
            {
                // Если вилка на столе
                if (State == ForkState.OnTheTable)
                {
                    // Берём вилку
                    State = ForkState.Taken;
                    // Записываем взявшего
                    _takenBy = takenBy;
                    // Выводим соответствующее значение
                    Console.WriteLine("||| {0} is taken by {1}", ForkId, _takenBy);
                    // Возвращаем истину
                    return true;
                }
                // Если она кем-то взята, обозначаем её как взятую
                State = ForkState.Taken;
                // Возвращаем ложь
                return false;
            }
        }

        // Метод описывает процесс возвращения вилки
        public void Put()
        {
            // Меняем состояние на "На столе"
            State = ForkState.OnTheTable;
            // Записываем кто положил вилку на стол
            Console.WriteLine("||| {0} is place on the table by {1}", ForkId, _takenBy);
            // Вилка никем не взята
            _takenBy = string.Empty;   
        }
    }   
}