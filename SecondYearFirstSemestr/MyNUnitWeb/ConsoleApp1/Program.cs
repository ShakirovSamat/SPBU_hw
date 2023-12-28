using Microsoft.AspNetCore.Mvc;
using MyNUnit.Attributes;

namespace TestProject
{

	public class Program
	{
		public static void Main()
		{

		}

		[Test]
		public void DoFail()
		{
			throw new NotImplementedException();
		}

		[Test]
		public void Do()
		{

		}

		[Test]
		public int GetNumber()
		{
			return 23;
		}

		[Test("Lazy method")]
		public void IngonreMethod()
		{

		}

		[Test(typeof(ArgumentNullException))]
		public void ThrowException()
		{
			throw new NotImplementedException();
		}
	}
}