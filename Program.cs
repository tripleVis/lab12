using System;

namespace lab12
{
	class Private
	{
		private Private() { }
	}
	class SomeClass { }

	class Program
	{
		static void Main()
		{
			Reflector.Research(typeof(Ship));
			Reflector.Research(typeof(Console));
			Reflector.Research(typeof(string));

			//вызов с помощью Invoke
			Reflector.Invoke(typeof(Console), "WriteLine", (typeof(string), "Это сообщение выведено через Reflector.Invoke"));
			Console.ReadKey();

			Console.Write("\nВведите тип параметра для поиска метода в Convert: ");
			string userParamType = Console.ReadLine();
			var result = Reflector.GetMethods(typeof(Convert), userParamType);
			if (result == null)
				Console.WriteLine("Ничего не найдено");
			else
				foreach (var item in result)
					Console.WriteLine(item);

			Console.ReadKey();
			SomeClass someClass = Reflector.Create<SomeClass>();
			if (someClass != null)
				Console.WriteLine("\nЭкземпляр создан");
			else
				Console.WriteLine("\nЭкземпляр не создан");

			Port port = Reflector.Create<Port>(
				(typeof(string), "Линкор"),
				(typeof(int), 55),
				(typeof(int), 40),
				(typeof(int), 1));

			Console.ReadKey();
		}
	}
}