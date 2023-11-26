namespace MyNUnit
{
	public class TestInforamtion
	{
	}

	public class MethodInforamtion
	{
		public string Name { get; set; }
		public long Time { get; set; }

		public string Message { get; set; }

		public Exception? Exception { get; set; }

		public MethodInforamtion(string name, long time, string message, Exception exception = null)
		{
			Name = name;
			Time = time;
			Message = message;
			Exception = exception;
		}
	}

}
