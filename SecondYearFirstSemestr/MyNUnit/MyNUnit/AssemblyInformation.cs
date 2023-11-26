namespace MyNUnit
{
	public class AssemblyInformation
	{
		public string Name { get; set; }

		public List<ClassInformation> classInformations { get; set; }

		public AssemblyInformation(string name)
		{
			Name = name;
			classInformations = new List<ClassInformation>();
		}
	}
}
