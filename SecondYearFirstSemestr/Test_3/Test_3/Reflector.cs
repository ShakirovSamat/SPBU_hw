using System.Reflection;
using System.Text;

namespace Test3
{
	public static class Reflector
	{

		//Считывает данные класса и записывает их в файл
		public static void PrintStructure(Type someClass)
		{
			var data = getStructureInString(someClass);
			File.WriteAllText(someClass.Name + ".cs", data);
		}

		public static void DiffClassese(Type a, Type b)
		{
			throw new NotImplementedException();
		}


		// Считывает данные класса и превращает в строку
		private static string getStructureInString(Type someClass)
		{
			if (!someClass.IsClass && !someClass.IsInterface)
			{
				throw new ArgumentException();
			}

			var builder = new StringBuilder();
			if (someClass.IsClass)
			{
				builder.Append("public class " + someClass.Name);
				if (someClass.ContainsGenericParameters)
				{
					var args = someClass.GetGenericArguments();
					foreach(var arg in args)
					{
						builder.Append(arg.Name+ "");
					}
					builder.Append(">");
				}
			}
			else if (someClass.IsInterface)
			{
				builder.AppendLine("public interface " + someClass.Name);
			}

			builder.AppendLine("{");

			foreach (var memeber in someClass.GetMembers(BindingFlags.DeclaredOnly
			| BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static))
			{
				if (memeber.MemberType == MemberTypes.Constructor)
				{
					var constructor = (ConstructorInfo)memeber;
					builder.Append("public " + someClass.Name + " (");
					var parameters = constructor.GetParameters();
					for (int i = 0; i < parameters.Length; ++i)
					{
						builder.Append(parameters[i].ParameterType + " " + parameters[i].Name);
						if (i + 1 != parameters.Length)
						{
							builder.Append(", ");
						}
					}
					builder.Append("){}\n\n");
				}
				else if (memeber.MemberType == MemberTypes.Method)
				{
					var method = (MethodInfo)memeber;
					string availability = "";
					if (method.IsPublic)
					{
						availability = "public";
					}
					else if (method.IsPrivate)
					{
						availability = "private";
					}
					else
					{
						availability = "protected";
					}

					string IsStatic = method.IsStatic ? " static " : " ";
					var returnParametr = method.ReturnParameter.ParameterType;
					builder.Append(availability + "" + IsStatic + "" + method.ReturnParameter.ParameterType + " " + method.Name + "(");
					var parameters = method.GetParameters();
					for (int i = 0; i < parameters.Length; ++i)
					{
						builder.Append(parameters[i].ParameterType + " " + parameters[i].Name);
						if (i + 1 != parameters.Length)
						{
							builder.Append(", ");
						}
					}
					builder.Append(")\n{\nthrow new NotImplementedException();\n}\n\n");

				}
				else if (memeber.MemberType == MemberTypes.Property)
				{
					var property = (PropertyInfo)memeber;
					builder.Append((property.Name) + " {get;set;}\n\n");
				}
			}

			foreach (var type in someClass.GetNestedTypes())
			{
				builder.Append('\n');
				builder.Append(getStructureInString(type));
			}

			builder.Append("}\n\n");
			return builder.ToString();
		}
	}
}
