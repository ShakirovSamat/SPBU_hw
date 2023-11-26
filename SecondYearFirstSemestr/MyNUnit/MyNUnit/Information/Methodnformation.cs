using System.Text;

namespace MyNUnit.Information
{
	public class MethodInformation
	{
		public string Name { get; set; }
		public long Time { get; set; }

		public string Message { get; set; }

		public bool Succeed { get; set; }

		public Exception? Exception { get; set; }

		public MethodInformation(string name, long time, string message, bool succeed, Exception exception = null)
		{
			Name = name;
			Time = time;
			Message = message;
			Succeed = succeed;
			Exception = exception;
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append("\t\tMethod: " + Name + "\n");
			if (Succeed)
			{
				builder.Append("\t\t\tState: Succeed\n");
			}
			else
			{
				builder.Append("\t\t\tState: Failed\n");
			}

			builder.Append("\t\t\t Time: " + Time + "\n");
			builder.Append("\t\t\t Message: " + Message + "\n");
			if (Exception != null)
			{
				builder.Append("\t\t\tException : " + Exception.ToString());
			}

			builder.Append("\n\n");
			return builder.ToString();
		}
	}

}
