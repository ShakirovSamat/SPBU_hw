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
