﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultiplication.Exceptions
{

	[Serializable]
	public class FileAlreadyExistException : Exception
	{
		public FileAlreadyExistException() { }
		public FileAlreadyExistException(string message) : base(message) { }
		public FileAlreadyExistException(string message, Exception inner) : base(message, inner) { }
		protected FileAlreadyExistException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
