namespace MyNUnit.Attributes
{
	/// <summary>
	/// Marks the method that will be called before the test of each tested method
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public class BeforeAttribute: Attribute
	{
		public BeforeAttribute() { }
	}
}
