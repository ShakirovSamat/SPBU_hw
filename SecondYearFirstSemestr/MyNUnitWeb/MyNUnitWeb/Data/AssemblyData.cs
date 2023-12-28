namespace MyNUnitWeb.Data
{
	public class AssemblyData
	{
		public int AssemblyDataId { get; set; }

		public string Name { get; set; }

		public int AmountOfSucceedTests { get; set; }

		public int AmountOfFailTests { get; set; }

		public int AmountOfIgnoredTests { get; set; }

		public int AmountOfTest { get; set; }

		public ICollection<MethodData> MethodDatas { get; set; }
	}
}
