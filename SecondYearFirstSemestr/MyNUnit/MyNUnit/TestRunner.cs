using MyNUnit.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace MyNUnit
{
	public static class TestRunner
	{
		public static List<MethodInforamtion> Results = new List<MethodInforamtion>();
		public static AssemblyInformation RunTests(Assembly assembly)
		{
			var result = new AssemblyInformation(assembly.FullName);
			foreach (Type type in assembly.GetTypes())
			{
				if (type.IsClass && ContainsTestMethods(type))
				{
					result.classInformations.Add(ClassTest(type));
				}
			}
			return result;
		}


		private static ClassInformation ClassTest(Type type)
		{
			var result = new ClassInformation(type.FullName);
			var storage = new MethodsStorage();
			var instace = Activator.CreateInstance(type);

			foreach (var method in type.GetMethods())
			{
				storage.DistributeMethod(method);
			}

			foreach(var beforeClassMethod in storage.BeforeClassMethods)
			{
				beforeClassMethod.Invoke(null, null);
			}

			foreach(var testMethod in storage.TestMethods)
			{
				result.methodInformations.Add(MethodTest(instace, testMethod.method, testMethod.expected, storage.BeforerMethods, storage.AfterMethods));
			}

			foreach (var afterClassMethod in storage.AfterClassMethods)
			{
				afterClassMethod.Invoke(null, null);
			}

			return result;
		}

		private static MethodInforamtion MethodTest(object instance, MethodInfo methodInfo, Type? expected, List<MethodInfo> before, List<MethodInfo> after)
		{
			foreach(var method in before)
			{
				method.Invoke(instance, null);
			}

			bool isCaught = false;
			var sw = new Stopwatch();
			sw.Start();
			try
			{
				var result = methodInfo.Invoke(instance, null);				
			}
			catch(Exception e)
			{
				isCaught = true;
				if (e.InnerException!.GetType() != expected || expected == null)
				{
					sw.Stop();
					return new MethodInforamtion(methodInfo.Name, sw.ElapsedMilliseconds, "Other exception was thrown", e);
				}
			}

			sw.Stop();
			if (!isCaught && expected != null)
			{
				return new MethodInforamtion(methodInfo.Name, sw.ElapsedMilliseconds, "The expected exception wasn't trown");
			}

			foreach (var method in after)
			{
				method.Invoke(instance, null);
			}

			return new MethodInforamtion(methodInfo.Name, sw.ElapsedMilliseconds, "Succeed");
		}

		private static bool ContainsTestMethods(Type type)
		{
			foreach (var method in type.GetMethods())
			{
				foreach (var attribute in Attribute.GetCustomAttributes(method))
				{
					if (attribute is TestAttribute)
					{
						return true;
					}
				}
			}

			return false;
		}
	}
	public enum TargetAttributes
	{
		Test,
		Before,
		After,
		BeforeClass,
		AfterClass,
		Undefiend,
	}
	public class MethodsStorage
	{
		public List<(MethodInfo method, Type? expected)> TestMethods { get; set; }
		public List<MethodInfo> BeforerMethods { get; set; }
		public List<MethodInfo> AfterMethods { get; set; }
		public List<MethodInfo> BeforeClassMethods { get; set; }
		public List<MethodInfo> AfterClassMethods { get; set; }

		public MethodsStorage()
		{
			TestMethods = new List<(MethodInfo, Type?)>();
			BeforerMethods = new List<MethodInfo>();
			AfterMethods = new List<MethodInfo>();
			BeforeClassMethods = new List<MethodInfo>();
			AfterClassMethods = new List<MethodInfo>();
		}

		public TargetAttributes getAttributeType(Attribute attribute)
		{
			var type = attribute.GetType();
			if (type == typeof(TestAttribute))
			{
				return TargetAttributes.Test;
			}

			if (type == typeof(BeforeAttribute))
			{
				return TargetAttributes.Before;
			}

			if (type == typeof(AfterAttribute))
			{
				return TargetAttributes.After;
			}

			if (type == typeof(BeforeClassAttribute))
			{
				return TargetAttributes.BeforeClass;
			}

			if (type == typeof(AfterClassAttribute))
			{
				return TargetAttributes.AfterClass;
			}

			return TargetAttributes.Undefiend;
		}
		public void DistributeMethod(MethodInfo method)
		{
			foreach (var attribute in method.GetCustomAttributes())
			{
				var type = getAttributeType(attribute);
				switch (type)
				{
					case (TargetAttributes.Test):
						TestMethods.Add((method, (attribute as TestAttribute).Expected));
						break;
					case (TargetAttributes.Before):
						BeforerMethods.Add(method);
						break;
					case (TargetAttributes.After):
						AfterMethods.Add(method);
						break;
					case (TargetAttributes.BeforeClass):
						BeforeClassMethods.Add(method);
						break;
					case (TargetAttributes.AfterClass):
						AfterClassMethods.Add(method);
						break;
				}
			}
		}
	}
}
