using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool.Exceptions
{

	[Serializable]
	public class ShutDownedException : Exception
	{
		public ShutDownedException() { }
		public ShutDownedException(string message) : base(message) { }
		public ShutDownedException(string message, Exception inner) : base(message, inner) { }
		protected ShutDownedException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
