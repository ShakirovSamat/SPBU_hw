namespace MyNUnitWeb.Data
{
	public class MethodData
	{
		public int MethodDataId { get; set; }

		public string Name { get; set; }

		public bool IsSucceed { get; set; }

		public long Time { get; set; }

		public string? IgnoreReason { get; set; }

		public int AssemblyDataId { get; set; }

		public AssemblyData AssemblyData { get; set; }
	}
}
