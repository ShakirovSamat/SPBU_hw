namespace MatrixMultiplication.Exceptions
{

    [Serializable]
    public class FileIsEmptyException : Exception
    {
        public FileIsEmptyException() { }
        public FileIsEmptyException(string message) : base(message) { }
        public FileIsEmptyException(string message, Exception inner) : base(message, inner) { }
        protected FileIsEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
