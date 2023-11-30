using Test2;

namespace Tests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void SimpleTest()
		{
			var dir = Directory.GetCurrentDirectory();
			File.Create(dir + "//1.txt").Close();		
			File.Create(dir + "//2.txt").Close();
			File.Create(dir + "//3.txt").Close();

			var res1 = SumChecker.getCheckSumForDirectory(dir);
			var res2 = SumChecker.getCheckSumForDirectoryParallel(dir);

			File.Delete(dir + "//1.txt");
			File.Delete(dir + "//2.txt");
			File.Delete(dir + "//3.txt");

			Assert.Pass(res1, Is.EqualTo(res2));
		}

		[Test]
		public void EmptyDirectoryTest()
		{
			var dir = Directory.GetCurrentDirectory();

			var res1 = SumChecker.getCheckSumForDirectory(dir);
			var res2 = SumChecker.getCheckSumForDirectoryParallel(dir);

			Assert.Pass(res1, Is.EqualTo(res2));
		}
	}
}