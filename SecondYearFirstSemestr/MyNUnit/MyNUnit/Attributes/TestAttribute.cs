namespace MyNUnit.Attributes
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public class TestAttribute : Attribute
	{
		public Type? Expected { get; private set; }

		public string? Ignore { get; private set; }

		public TestAttribute() { }

		public TestAttribute(Type expected)
		{
			Expected = expected;
		}

		public TestAttribute(string ignore)
		{
			Ignore = ignore;
		}

		public TestAttribute(Type? expected, string? ignore)
		{
			Expected = expected;
			Ignore = ignore;
		}
	}
}
