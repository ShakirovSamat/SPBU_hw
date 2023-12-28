using System.Text;

namespace MyNUnit.Information
{
	public class ClassInformation
	{
		public string Name { get; set; }

		public string Message { get; set; }

		public Exception? Exception { get; set; }

		public List<MethodInformation> methodInformations { get; set; }

		public ClassInformation(string name, string message = "", Exception exception = null)
		{
			Name = name;
			Message = message;
			Exception = exception;
			methodInformations = new List<MethodInformation>();
		}

		public int GetAmountOfTests()
		{
			return methodInformations.Count;
		}

		public int GetAmountOfSucceedTests()
		{
			int sum = 0;
			foreach (var method in methodInformations)
			{
				if (method.Succeed && method.Ignore == null)
				{
					++sum;
				}
			}

			return sum;
		}

		public int GetAmountOfFailedTests()
		{
			int sum = 0;
			foreach (var method in methodInformations)
			{
				if (!method.Succeed)
				{
					++sum;
				}
			}

			return sum;
		}

		public int GetAmountOfIgnoredTests()
		{
			int sum = 0;
			foreach (var method in methodInformations)
			{
				if (method.Ignore != null)
				{
					++sum;
				}
			}

			return sum;
		}


		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append("\tClass Name: " + Name + "\n");
			if (Exception == null)
			{
				foreach (var methodInformation in methodInformations)
				{
					builder.Append(methodInformation.ToString());
				}
			}
			else
			{
				builder.Append("\t\t Message: " + Message + "\n");
				builder.Append("\t\t Exception: " + Exception + "\n");
			}

			return builder.ToString();
		}
	}
}
