namespace Lazy
{
    public class LazyOneThread<T> : ILazy<T>
    {
        private Func<T?>? supplier;
        private bool isCalculated;
        private T? value;
        private Exception? exception;

        public LazyOneThread(Func<T> func)
        {
            supplier = func;
            isCalculated = false;
            exception = null;
        }

        public T? Get()
        {
            if (!isCalculated)
            {
                try
                {
                    value = supplier();
                }

                catch(Exception ex)
                {
                    exception = ex;
                }
                isCalculated = true;
                supplier = null;
                return value;
            }

            if (exception != null)
            {
                throw exception;
            }

            return value;
        }
    }
}
