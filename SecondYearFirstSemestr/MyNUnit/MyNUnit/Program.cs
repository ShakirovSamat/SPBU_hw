using MyNUnit;
using System.Reflection;

class Program
{
	public static void Main(String[] args)
	{
		string assemblyPath = string.Empty;
		while (true)
		{
			Console.WriteLine("Введите полный путь папки, в которой хранятся сборки.");
			assemblyPath = Console.ReadLine();
			if (Directory.Exists(assemblyPath))
			{
				break;
			}

			Console.WriteLine("Папка не существет. Попробуйте ввести другой путь.");
		}

		string[] paths = Directory.GetFiles(assemblyPath, "*.dll");
		foreach (var path in paths)
		{
			var result = TestRunner.RunTests(Assembly.LoadFrom(path)).Result;
			Console.WriteLine(result.ToString());
		}
	}
}