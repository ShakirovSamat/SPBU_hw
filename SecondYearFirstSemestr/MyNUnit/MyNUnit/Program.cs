using MyNUnit.Attributes;
using System.Reflection;
class Program
{
	public static void Main(String[] args)
	{
		Console.WriteLine("Введите полный к папке, в которой хранятся сбоки");
		string path = Console.ReadLine();

		var  assembliesPaths = Directory.GetFiles(path, "*.dll");

		foreach ( var assemblyPath in assembliesPaths)
		{
			var assembly = Assembly.LoadFrom(assemblyPath);
			foreach(Type t in assembly.ExportedTypes)
			{
				foreach( MethodInfo t2 in t.GetMethods())
				{
					object[] attr = t2.GetCustomAttributes(false);
					foreach (Attribute atr in attr)
					{
                        if (atr is TestAttribute testAttribute)
						{
                            Console.WriteLine(testAttribute.Ignore);
                        }
                    }
                }
			}
		}
	}
}