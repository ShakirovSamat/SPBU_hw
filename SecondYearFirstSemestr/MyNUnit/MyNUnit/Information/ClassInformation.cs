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
