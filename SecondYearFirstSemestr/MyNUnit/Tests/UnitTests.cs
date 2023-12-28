using MyNUnit;
using MyNUnit.Information;

namespace Tests
{
	public class Tests
	{
		Assembly assembly;
		AssemblyInformation assemblyInformation;
		List<ClassInformation> classesInformation;

		[SetUp]
		public void Setup()
		{
			assembly = Assembly.LoadFrom("Tests.dll");
			assemblyInformation = TestRunner.RunTests(assembly).Result;
			classesInformation = assemblyInformation.classInformations;
		}

		[Test]
		public void IgnoreTest()
		{
			Assert.That(classesInformation[0].methodInformations[0].Succeed, Is.EqualTo(true));
			Assert.That(classesInformation[0].methodInformations[0].Ignore, Is.EqualTo("123"));
		}

		[Test]
		public void WasntExpectedExceptionTest()
		{
			Assert.That(classesInformation[0].methodInformations[1].Succeed, Is.EqualTo(false));
			Assert.That(classesInformation[0].methodInformations[1].Message, Is.EqualTo("The expected exception wasn't trown"));
		}

		[Test]
		public void CaughtExpectedExceptionTest()
		{
			Assert.That(classesInformation[0].methodInformations[2].Succeed, Is.EqualTo(true));
			Assert.That(classesInformation[0].methodInformations[2].Message, Is.EqualTo("Succeed"));
		}

		[Test]
		public void CaughtUnexpectedExceptionTest()
		{
			Assert.That(classesInformation[0].methodInformations[3].Succeed, Is.EqualTo(false));
			Assert.That(classesInformation[0].methodInformations[3].Message, Is.EqualTo("Other exception was thrown"));
		}

		[Test]
		public void BeforeTests()
		{
			Assert.That(classesInformation[1].methodInformations[0].Succeed, Is.EqualTo(true));
			Assert.That(classesInformation[2].methodInformations[0].Succeed, Is.EqualTo(false));
		}

		[Test]
		public void AfterTests()
		{
			Assert.That(classesInformation[3].methodInformations[0].Succeed, Is.EqualTo(true));
			Assert.That(classesInformation[4].methodInformations[0].Succeed, Is.EqualTo(false));
		}

		[Test]
		public void BeforeClassTests()
		{
			Assert.That(classesInformation[5].methodInformations[0].Succeed, Is.EqualTo(true));
			Assert.That(classesInformation[6].Exception, !Is.EqualTo(null));
		}

		[Test]
		public void AfterClassTests()
		{
			Assert.That(classesInformation[7].methodInformations[0].Succeed, Is.EqualTo(true));
			Assert.That(classesInformation[8].Exception, !Is.EqualTo(null));
		}
	
	}
}