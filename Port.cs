using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace lab12
{
	class Port : IContainer, ICollection
	{
		public List<IComponent> Ships { get; private set; }

		public Port()
		{
			Ships = new List<IComponent>();//инициализация списка
		}

		public Port(params Ship[] items)
		{
			Ships = new List<IComponent>();
			foreach (var item in items)
				Ships.Add(item);//добавление элемента
		}

		public void Add(Ship item)
		{
			Add(item, item.GetHashCode().ToString());//добавление элемента
		}

		public void Add(IComponent item)
		{
			Add(item, item.GetHashCode().ToString());//добавление элемента
		}

		public void Add(Ship item, string ID)
		{
			Ships.Add(item);//добавление элемента
		}

		public void Add(IComponent item, string ID)
		{
			if (Ships.Any(el => el.Site.Name == ID))
				return;
			item.Site.Name = ID;
			Ships.Add(item);//добавление элемента
		}

		public bool Remove(int index)
		{
			if (index < Components.Count && index > 0)
			{
				Remove(Components[index]);//удаление элемента с индексом
				return true;
			}
			return false;
		}

		public void Remove(IComponent item)
		{
			Ships.Remove(item);
		}

		public ComponentCollection Components
		{
			get
			{
				var datalist = new IComponent[Ships.Count];
				Ships.CopyTo(datalist);//копирование 
				return new ComponentCollection(datalist);//коллекция только для чтения
			}
		}

		public int Count => throw new NotImplementedException();

		public bool IsSynchronized => throw new NotImplementedException();

		public object SyncRoot => throw new NotImplementedException();

		public void Dispose()
		{
			foreach (var item in Ships)
				item.Dispose();
			Ships.Clear();//очистка
		}

		public void PrintContent()
		{
			Console.WriteLine("№  Тип корабля   Водозамещение    Вместимость	Возраст капитана");
			int counter = 0;
			foreach (var component in Components)
			{
				var item = (Ship)component;//упаковка
				Console.WriteLine($"{++counter})   {item.GetType()}\t\t{item.Displacement}\t\t{item.Capacity}\t\t{item.CapAge}");
			}
		}

		public IEnumerator GetEnumerator()
		{
			return Ships.GetEnumerator();
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();//генерация исключения
		}
	}
}