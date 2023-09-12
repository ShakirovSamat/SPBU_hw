using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultiplication.Exceptions
{

	[Serializable]
	public class BadMatrixElementException : Exception
	{
		public BadMatrixElementException() { }
		public BadMatrixElementException(string message) : base(message) { }
		public BadMatrixElementException(string message, Exception inner) : base(message, inner) { }
		protected BadMatrixElementException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
