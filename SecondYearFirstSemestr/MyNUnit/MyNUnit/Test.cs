using MyNUnit.Attributes;

namespace MyNUnit
{
	public class Test
	{
		[Test]
		public string Do()
		{
			Thread.Sleep(1000);
			return "1";
		}

		[Test(typeof(ArgumentNullException))]
		public int DoWrongResult()
		{
			Thread.Sleep(500);
			return 6;
		}

		[Test(typeof(ArgumentNullException))]
		public int Do2WrongResult()
		{
			Thread.Sleep(300);
			throw new ArgumentNullException();
		}

		[Test]
		public void DoWrong()
		{
			Thread.Sleep(100);
			throw new Exception();
		}
	}
}
