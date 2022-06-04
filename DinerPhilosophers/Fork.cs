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
        public string TakenBy { get; set; }

        private object lockObject = new object();
        
        // Метод взятия вилки
        public bool Take(string takenBy)
        {
            // Синхронизируем потоки
            lock (lockObject)
            {
                // Если вилка на столе
                if (this.State == ForkState.OnTheTable)
                {
                    // Берём вилку
                    State = ForkState.Taken;
                    // Записываем взявшего
                    TakenBy = takenBy;
                    // Выводим соответствующее значение
                    Console.WriteLine("||| {0} is taken by {1}", ForkId, TakenBy);
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
            Console.WriteLine("||| {0} is place on the table by {1}", ForkId, TakenBy);
            // Вилка никем не взята
            TakenBy = string.Empty;   
        }
    }   
}