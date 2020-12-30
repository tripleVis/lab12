using System;
using System.ComponentModel;

namespace lab12
{
    enum TypesOfShips//перечисление
    {
        Steamer,
        Sailboat,
        Corvette,
        Battleship
    }

    class Ship : IComponent//класс реализует интерфейс
    {
        public TypesOfShips Type { get; private set; }//свойства

        public int Capacity { get; private set; }

        public double Displacement { get; private set; }

        public int CapAge { get; private set; }
        public ISite Site { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Ship(TypesOfShips type, double displacement, int capacity, int capAge)//конструктор
        {
            Type = type;
            Displacement = displacement;
            Capacity = capacity;
            CapAge = capAge;
        }

        public event EventHandler Disposed;

        public new string GetType()
        {
            return Type switch
            {
                TypesOfShips.Steamer => "Пароход",
                TypesOfShips.Sailboat => "Парусник",
                TypesOfShips.Corvette => "Корвет",
                TypesOfShips.Battleship => "Линкор",
                _ => "Тип не опознан",
            };
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}