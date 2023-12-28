namespace MyNUnit.Attributes
{
	/// <summary>
	/// Marks the method that will be called after the test of each tested method
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public class AfterAttribute: Attribute
	{
		public AfterAttribute() { }
	}
}
