using System.Text;

namespace MyNUnit.Information
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

		public List<MethodInformation> GetAllMethods()
		{
			List<MethodInformation> list = new();
			foreach (var classInformation in classInformations)
			{
				list.AddRange(classInformation.methodInformations);
			}

			return list;
		}

        public int GetAmountOfTest()
        {
            int sum = 0;
            foreach (var classInformation in classInformations)
            {
                sum += classInformation.GetAmountOfTests();
            }

            return sum;
        }

		public int GetAmountOfSucceedTest()
		{
			int sum = 0;
			foreach (var classInformation in classInformations)
			{
				sum += classInformation.GetAmountOfSucceedTests();
			}

			return sum;
		}

		public int GetAmountOfFailedTest()
		{
			int sum = 0;
			foreach (var classInformation in classInformations)
			{
				sum += classInformation.GetAmountOfFailedTests();
			}

			return sum;
		}

		public int GetAmountOfIgnoreTest()
		{
			int sum = 0;
			foreach (var classInformation in classInformations)
			{
				sum += classInformation.GetAmountOfIgnoredTests();
			}

			return sum;
		}

		public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("ASsembly name: " + Name + "\n");
            foreach (var classInformation in classInformations)
            {
                builder.Append(classInformation.ToString());
            }

            return builder.ToString();
        }
    }
}
