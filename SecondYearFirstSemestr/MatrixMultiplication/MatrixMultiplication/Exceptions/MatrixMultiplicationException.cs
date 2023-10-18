﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultiplication.Exceptions
{

	[Serializable]
	public class MatrixMultiplicationException : Exception
	{
		public MatrixMultiplicationException() { }
		public MatrixMultiplicationException(string message) : base(message) { }
		public MatrixMultiplicationException(string message, Exception inner) : base(message, inner) { }
		protected MatrixMultiplicationException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
