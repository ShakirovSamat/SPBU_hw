using MyNUnit;
using System.Reflection;
class Program
{
	public static void Main(String[] args)
	{
		string path = string.Empty;
		while (true)
		{
			Console.WriteLine("Введите полный путь папки, в которой хранятся сборки.");
			path = Console.ReadLine();
			if (Directory.Exists(path))
			{
				break;
			}

			Console.WriteLine("Папка не существет. Попробуйте ввести другой путь.");
		}

		string[] paths = Directory.GetFiles(path, "*.dll");
		foreach(var pathh in paths)
		{
			var result = TestRunner.RunTests(Assembly.LoadFrom(pathh)).Result;
			Console.WriteLine(result.ToString());
        }
	}
}