namespace MyNUnit.Attributes
{
	/// <summary>
	/// Marks a method that will be called before testing the class methods under test
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public class BeforeClassAttribute: Attribute
	{
		public BeforeClassAttribute() { }
	}
}
