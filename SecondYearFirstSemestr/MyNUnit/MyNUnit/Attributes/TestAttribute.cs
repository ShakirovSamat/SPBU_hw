using System.Diagnostics.CodeAnalysis;

namespace MyNUnit.Attributes
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public class TestAttribute : Attribute
	{
		public object? Expected { get; private set; }

		public string? Ignore { get; private set; }

		public TestAttribute() { }

		public TestAttribute(object expected)
		{
			Expected = expected;
		}

		public TestAttribute(string ignore)
		{
			Ignore = ignore;
		}

		public TestAttribute(object? expected, string? ignore)
		{
			Expected = expected;
			Ignore = ignore;
		}
	}
}
