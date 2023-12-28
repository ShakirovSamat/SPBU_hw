using MyNUnit.Attributes;
using MyNUnit.Information;
using System.Diagnostics;
using System.Reflection;

namespace MyNUnit
{
	public enum TargetAttributes
	{
		Test,
		Before,
		After,
		BeforeClass,
		AfterClass,
		Undefined,
	}

	public static class TestRunner
	{
		public static List<MethodInformation> Results = new List<MethodInformation>();

		public static async Task<AssemblyInformation> RunTests(Assembly assembly)
		{
			var result = new AssemblyInformation(assembly.FullName);
			var tasks = new List<Task<ClassInformation>>();
			foreach (Type type in assembly.GetTypes())
			{
				if (type.IsClass && ContainsTestMethods(type))
				{
					tasks.Add(Task.Run(() => ClassTest(type)));
				}
			}

			foreach (var task in tasks)
			{
				result.classInformations.Add(await task);
			}

			return result;
		}


		private static async Task<ClassInformation> ClassTest(Type type)
		{
			var result = new ClassInformation(type.FullName);
			var storage = new MethodsStorage();
			var instance = Activator.CreateInstance(type);

			foreach (var method in type.GetMethods())
			{
				storage.DistributeMethod(method);
			}

			foreach (var beforeClassMethod in storage.BeforeClassMethods)
			{
				try
				{
					beforeClassMethod.Invoke(null, null);
				}
				catch (Exception e)
				{
					return new ClassInformation(type.FullName, "BeforeClass method had thrown exception. Method name: " + beforeClassMethod.Name + "", e);
				}
			}

			var tasks = new List<Task<MethodInformation>>();
			foreach (var testMethod in storage.TestMethods)
			{
				if (testMethod.Ignore != null)
				{
					result.methodInformations.Add(new MethodInformation(testMethod.method.Name, 0, "Was ignored. Ignore message: " + testMethod.Ignore, true, null, testMethod.Ignore));
				}
				else
				{
					tasks.Add(Task.Run(() => MethodTest(instance, testMethod.method, testMethod.expected, storage.BeforeMethods, storage.AfterMethods)));
				}
			}

			foreach (var task in tasks)
			{
				result.methodInformations.Add(await task);
			}

			foreach (var afterClassMethod in storage.AfterClassMethods)
			{
				try
				{
					afterClassMethod.Invoke(null, null);
				}
				catch (Exception e)
				{
					return new ClassInformation(type.FullName, "AfterClass method had thrown exception. Method name: " + afterClassMethod.Name + "", e);
				}
			}

			return result;
		}

		private static async Task<MethodInformation> MethodTest(object instance, MethodInfo methodInfo, Type? expected, List<MethodInfo> beforeMethods, List<MethodInfo> afterMethods)
		{
			MethodInformation methodInformation = null;
			foreach (var method in beforeMethods)
			{
				try
				{
					method.Invoke(instance, null);
				}
				catch (Exception e)
				{
					return new MethodInformation(methodInfo.Name, 0, "Before method had trown exception. Method name: " + method.Name + "", false, e);
				}
			}

			bool isCaught = false;
			var sw = new Stopwatch();
			sw.Start();
			try
			{
				var result = methodInfo.Invoke(instance, null);
			}
			catch (Exception e)
			{
				isCaught = true;
				if (e.InnerException!.GetType() != expected || expected == null)
				{
					sw.Stop();
					methodInformation = new MethodInformation(methodInfo.Name, sw.ElapsedMilliseconds, "Other exception was thrown", false, e);
				}
			}

			sw.Stop();
			if (!isCaught && expected != null)
			{
				methodInformation = new MethodInformation(methodInfo.Name, sw.ElapsedMilliseconds, "The expected exception wasn't trown", false);
			}

			foreach (var method in afterMethods)
			{
				try
				{
					method.Invoke(instance, null);
				}
				catch (Exception e)
				{
					return new MethodInformation(methodInfo.Name, 0, "After method had trown exception. Method name: " + method.Name + "", false, e);

				}
			}

			if (methodInformation == null)
			{
				methodInformation = new MethodInformation(methodInfo.Name, sw.ElapsedMilliseconds, "Succeed", true);
			}

			return methodInformation;
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

	public class MethodsStorage
	{
		public List<(MethodInfo method, Type? expected, string? Ignore)> TestMethods { get; set; }

		public List<MethodInfo> BeforeMethods { get; set; }

		public List<MethodInfo> AfterMethods { get; set; }

		public List<MethodInfo> BeforeClassMethods { get; set; }

		public List<MethodInfo> AfterClassMethods { get; set; }

		public MethodsStorage()
		{
			TestMethods = new List<(MethodInfo, Type?, string?)>();
			BeforeMethods = new List<MethodInfo>();
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

			return TargetAttributes.Undefined;
		}

		public void DistributeMethod(MethodInfo method)
		{
			foreach (var attribute in method.GetCustomAttributes())
			{
				var type = getAttributeType(attribute);
				switch (type)
				{
					case (TargetAttributes.Test):
						TestMethods.Add((method, (attribute as TestAttribute).Expected, (attribute as TestAttribute).Ignore));
						break;
					case (TargetAttributes.Before):
						BeforeMethods.Add(method);
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
