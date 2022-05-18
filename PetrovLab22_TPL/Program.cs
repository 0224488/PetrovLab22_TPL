using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetrovLab22_TPL
{
	// Сформировать массив случайных целых чисел (размер задается пользователем).
	// Вычислить сумму чисел массива и максимальное число в массиве.
	// Реализовать решение задачи с использованием механизма задач продолжения

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Введите размер массива");
			int count = int.Parse(Console.ReadLine());

			Console.WriteLine("Введите возможное минимальное число массива");
			int minValue = int.Parse(Console.ReadLine());

			Console.WriteLine("Введите максимальное возможное число массива");
			int maxValue = int.Parse(Console.ReadLine());

			// задача генерации массива случайных чисел
			Task<int[]> generateTask = new Task<int[]>(
				() =>
				{
					Random rnd = new Random();
					
					var array = new int[count];
					// заполняем массив случайными числами
					for(int i = 0; i < array.Length; i++)
					{
						array[i] = rnd.Next(minValue, maxValue);
					}
					return array;
				});
			// задача вычисления суммы
			Task<int> calcSumTask = generateTask.ContinueWith(task => task.Result.Sum());
			// задача вычисления максимального значения
			Task<int> maxValueTask = calcSumTask.ContinueWith(t => generateTask.Result.Max());
			
			//запускаем исходную задачу
			generateTask.Start();

			//дожидаемся окончания выполнения последней задачи
			maxValueTask.Wait();

			Console.Write("\r\nМассив: ");
			for(int i = 0; i < count; i++)
				Console.Write($"{generateTask.Result[i]} ");

			Console.WriteLine($"\r\nСумма чисел массива: {calcSumTask.Result}");
			Console.WriteLine($"Максимальное число в массиве: {maxValueTask.Result}");

			Console.ReadKey();
		}
	}
}
