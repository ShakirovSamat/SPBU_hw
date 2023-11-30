using System.Diagnostics;

namespace Test2
{
	class Program
	{
		public static void Main(String[] args)
		{
			string path = String.Empty;
			while (true)
			{
				Console.WriteLine("Введите полный путь до директории");
				path = Console.ReadLine();
				if (Directory.Exists(path))
				{
					break;
				}
				Console.WriteLine("Директория не существует. Попробуйте ввести другой путь");
			}

			var timer = new Stopwatch();

			timer.Start();
			string hashSum = string.Empty;
			try
			{
				hashSum = SumChecker.getCheckSumForDirectory(path);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			timer.Stop();
			long nonParallelTime = timer.ElapsedMilliseconds;
			Console.WriteLine($"Time: {nonParallelTime}. Hash: {hashSum}");

			timer.Reset();
			Console.WriteLine("\n");

			timer.Start();
			try
			{
				hashSum = SumChecker.getCheckSumForDirectoryParallel(path);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			timer.Stop();
			long parallelTime = timer.ElapsedMilliseconds;
			Console.WriteLine($"Time: {parallelTime}. Hash: {hashSum}\n");

			if (nonParallelTime < parallelTime)
			{
				Console.WriteLine("Single threaded option is faster");
			}
			else
			{
				Console.WriteLine("Multithreaded option is faster");
			}
		}
	}
}