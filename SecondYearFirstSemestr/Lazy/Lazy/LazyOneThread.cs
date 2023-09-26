namespace Lazy
{
    public class LazyOneThread<T>: ILazy<T>
    {
        private Func<T> supplier;
        private bool isCalculated;
        private T? value;

        public LazyOneThread(Func<T> func)
        {
            supplier = func;
            isCalculated = false;
        }

        public T Get()
        {
            if (!isCalculated)
            {
                value = supplier();
                isCalculated = true;
                return value;
            }
            return value;
        }
    }
}
