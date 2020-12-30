using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;

namespace lab12
{
	// Исследуемый тип
	class InvestigatedType
	{
		// Сборка
		public string Assembly { get; set; }
		// Есть ли публичные конструкторы
		public bool HasPublicConstructors { get; set; }
		// Публичные методы
		public IEnumerable<string> PublicMethods { get; set; }
		// Поля и свойства
		public IEnumerable<string> FieldsAndProperties { get; set; }
		// Интерфейсы
		public IEnumerable<string> Interfaces { get; set; }

		// Вывод информации
		public void Info()
		{
			Console.WriteLine(
				$"Assembly: {Assembly}" +
				$"\nHas public constructors: {HasPublicConstructors}" +
				$"\nPublic methods:"
				);
			foreach (var item in PublicMethods)
				Console.WriteLine(item);
			Console.WriteLine("Field and properties:");
			foreach (var item in FieldsAndProperties)
				Console.WriteLine(item);
			Console.WriteLine("Interfaces:");
			foreach (var item in Interfaces)
				Console.WriteLine(item);
		}
	}

	static class Reflector
	{
		public static void Research(Type type, bool show = false)
		{
			var result = new InvestigatedType()
			{
				Assembly = GetAssembly(type).FullName,
				HasPublicConstructors = HasPublicConstructors(type),
				FieldsAndProperties = GetFieldsAndProperties(type),
				Interfaces = GetInterfaces(type),
				PublicMethods = GetPublicMethods(type)
			};
			if (show)
				result.Info();


			try
			{
				using (var sw = new StreamWriter("text.json"))
				{
					sw.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		// Получение сборки
		public static Assembly GetAssembly(Type type) => type.Assembly;

		// Получение есть ли публичные конструкторы
		public static bool HasPublicConstructors(Type type) =>
			type.GetConstructors().Length != 0;

		// Получение публичных методов
		public static IEnumerable<string> GetPublicMethods(Type type)
		{
			var methods = type.GetMethods();
			var result = new List<string>();
			foreach (var item in methods)
				result.Add(item.Name);
			// Возвращение коллекции без повторений
			return result.Distinct();
		}

		// Получение полей и свойств
		public static IEnumerable<string> GetFieldsAndProperties(Type type)
		{
			var fields = type.GetFields();
			var properties = type.GetProperties();
			var result = new List<string>();
			foreach (var item in fields)
				result.Add(item.Name);
			foreach (var item in properties)
				result.Add(item.Name);
			return result;
		}

		// Получение интерфейсов
		public static IEnumerable<string> GetInterfaces(Type type)
		{
			var interfaces = type.GetInterfaces();
			var result = new List<string>();
			foreach (var item in interfaces)
				result.Add(item.Name);
			return result;
		}

		// Получение методов с типами параметров, соответствующих заданному
		public static IEnumerable<string> GetMethods(Type type, string paramType)
		{
			if (type == null)
				return null;
			var methods = type.GetMethods();
			var result = new List<string>();
			foreach (var item in methods)
				// Если хотя бы один тип параметра равен заданному
				if (item.GetParameters()
					.Any(param => param.ParameterType.Name == paramType))
					result.Add(item.Name);

			return result.Count != 0 ? result : null;
		}

		// Вызов метода типа через рефлексию
		public static void Invoke(Type type, string methodName, params (Type type, object value)[] paramTuples)
		{
			// Получение заданных значений параметров и их типов
			var paramsTypes = paramTuples.Select(item => item.type).ToArray();
			var paramsValues = paramTuples.Select(item => item.value).ToArray();

			// Поиск метода
			var method = type.GetMethod(methodName, paramsTypes);
			if (method == null)
			{
				Console.WriteLine("Метод не найден");
				return;
			}

			// Вызов метода
			method.Invoke(null, paramsValues);
		}

		// Создание экземпляра через публичный конструктор
		public static T Create<T>()
		{
			var type = typeof(T);
			Type[] types = new Type[0];
			var constructor = type.GetConstructor(types);
			if (constructor == null)
				// Если конструктор не найден, вернуть 0 или null
				return default;
			else
				// Иначе вернуть новый экземпляр, созданный через Invoke
				return (T)constructor.Invoke(null);
		}

		// То же самое, но с параметрами
		public static T Create<T>(params (Type type, object value)[] paramTuples)
		{
			var paramsTypes = paramTuples.Select(item => item.type).ToArray();
			var paramsValues = paramTuples.Select(item => item.value).ToArray();
			var type = typeof(T);
			var constructor = type.GetConstructor(paramsTypes);
			if (constructor == null)
				return default;
			else
				return (T)constructor.Invoke(paramsValues);
		}
	}
}