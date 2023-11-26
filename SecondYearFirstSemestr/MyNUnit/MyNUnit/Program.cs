using MyNUnit;
using System.Reflection;
class Program
{
	public static void Main(String[] args)
	{
		string path = string.Empty;
		while (true)
		{
			Console.WriteLine("Введите полный путь папки, в которой хранятся сборки");
			path = Console.ReadLine();
			if (Directory.Exists(path))
			{
				break;
			}

			Console.WriteLine("Папка не существет. Попробуйте ввести другой путь");
		}
		string[] paths = Directory.GetFiles(path, "*.dll");
		foreach(var pathh in paths)
		{
			var result = TestRunner.RunTests(Assembly.LoadFrom(pathh));
            Console.WriteLine(result.Name);
            foreach (var res in result.classInformations)
			{
                Console.WriteLine($"\t{res.Name}");
				foreach(var method in res.methodInformations)
				{
				Console.WriteLine($"\t\tMemthod {method.Name}:\n\t\t\ttime: {method.Time}\n\t\t\tmesesage: {method.Message}" +
					$"\n\t\t\texception: {method.Exception?.ToString()}\n\n");
				}
            }
        }

	}
}