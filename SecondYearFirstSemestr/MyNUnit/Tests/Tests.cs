using MyNUnit.Attributes;

namespace MyNUnit
{
	public class Tests
	{

		[Attributes.Test(typeof(Exception), "123")]
		public string IgnoreTest()
		{
			return "1";
		}

		[Attributes.Test(typeof(ArgumentNullException))]
		public int WasntExpectedExceptionTest()
		{
			return 6;
		}

		[Attributes.Test(typeof(ArgumentNullException))]
		public int CaughtExpectedExceptionTest()
		{
			throw new ArgumentNullException();
		}

		[Attributes.Test]
		public void CaughtUnexpectedExceptionTest()
		{
			throw new Exception();
		}
	}

	public class TestWithBeforeAttribute
	{
		[Before]
		public static void Do()
		{
			Console.WriteLine("Dp before method");
		}

		[Attributes.Test]
		public int Test()
		{
			return 6;
		}
	}

	public class BadTestWithBeforeAttribute
	{
		[Before]
		public static void Do()
		{
			throw new NotImplementedException();
		}

		[Attributes.Test]
		public int Test()
		{
			return 6;
		}
	}

	public class TestWithAfterAttribute
	{
		[After]
		public static void Do()
		{
			Console.WriteLine("Do ater method");
		}

		[Attributes.Test]
		public int Test()
		{
			return 6;
		}
	}
	public class BadTestWithAfterAttribute
	{
		[After]
		public static void Do()
		{
			throw new Exception();
		}

		[Attributes.Test]
		public int Test()
		{
			return 6;
		}
	}

	public class TestWithBeforeClassAttribute
	{
		[BeforeClass]
		public static void Do()
		{
			Console.WriteLine("Do before class method");
		}

		[Attributes.Test]
		public int Test()
		{
			return 5;
		}
	}

	public class BadTestWithBeforeClassAttribute
	{
		[BeforeClass]
		public static void Do()
		{
			throw new Exception();
		}

		[Attributes.Test]
		public int Test()
		{
			return 5;
		}
	}

	public class TestWithAfterClassAttribute
	{
		[AfterClass]
		public static void Do()
		{
			Console.WriteLine("Do after class method");
		}

		[Attributes.Test]
		public int Test()
		{
			return 5;
		}
	}

	public class BadTestWithAfterClassAttrribute
	{
		[AfterClass]
		public static void Do()
		{
			throw new Exception();
		}

		[Attributes.Test]
		public int Test()
		{
			return 5;
		}
	}
}
