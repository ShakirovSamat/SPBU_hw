using MyNUnit.Attributes;

namespace MyNUnit
{
	public class Test
	{

		[After]
		public static void BeforeDo()
		{
			throw new NotImplementedException();
		}
		[Test(typeof(Exception), "123")]
		public string Do()
		{
			Thread.Sleep(1000);
			return "1";
		}

		[Test(typeof(ArgumentNullException))]
		public int DoWrongResult()
		{
			Thread.Sleep(1000);
			return 6;
		}

		[Test(typeof(ArgumentNullException))]
		public int Do2WrongResult()
		{
			Thread.Sleep(1000);
			throw new ArgumentNullException();
		}

		[Test]
		public void DoWrong()
		{
			Thread.Sleep(1000);
			throw new Exception();
		}
	}
}
