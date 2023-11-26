namespace MyNUnit
{
	public class ClassInformation
	{
		public string Name { get; set; }

		public List<MethodInforamtion> methodInformations { get; set; }

		public ClassInformation(string name)
		{
			Name = name;	
			methodInformations = new List<MethodInforamtion>();
		}
	}
}
