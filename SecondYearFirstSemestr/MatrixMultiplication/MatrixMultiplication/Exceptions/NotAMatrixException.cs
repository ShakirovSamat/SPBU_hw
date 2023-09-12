using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultiplication.Exceptions
{

	[Serializable]
	public class NotAMatrixException : Exception
	{
		public NotAMatrixException() { }
		public NotAMatrixException(string message) : base(message) { }
		public NotAMatrixException(string message, Exception inner) : base(message, inner) { }
		protected NotAMatrixException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
