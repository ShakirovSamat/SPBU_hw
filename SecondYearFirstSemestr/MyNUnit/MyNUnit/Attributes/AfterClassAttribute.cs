namespace MyNUnit.Attributes
{
	/// <summary>
	/// Marks a method that will be called after testing the tested methods of the class
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public class AfterClassAttribute: Attribute
	{
		public AfterClassAttribute() { }
	}
}
