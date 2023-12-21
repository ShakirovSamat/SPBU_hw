
namespace Test3
{
	public class TestClass<T>
	{
		public string Name { get; set; }

		private int Age { get; set; }

		public TestClass(string name, int age)
		{
			Name = name;
			age = Age;
		}

		public static void Print(string color)
		{

		}

		public int getAge(T isMan)
		{
			return Age;
		}

		public class Nested()
		{
			public string Name { get; set; }

			public void Print()
			{
				throw new NotImplementedException();
			}
		}

		public interface IAddable
		{

		}
	}
}
