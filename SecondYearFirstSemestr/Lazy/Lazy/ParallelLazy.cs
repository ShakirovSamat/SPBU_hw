using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazy
{
    public class ParallelLazy<T> : ILazy<T>
    {
        private class Value
        {
            private readonly T value;

            public Value(T value)
            {
                this.value = value;
            }

            public T? Get()
            {
                return value;
            }
        }

        private Func<T?>? supplier;
        private bool isCalculated;
        private volatile Value? value;
        private Exception? exception;
        private object lockObject = new();

        public ParallelLazy(Func<T> func)
        {
            supplier = func;
            isCalculated = false;
            exception = null;
        }

        public T? Get()
        {
            if (!isCalculated)
            {
                lock (lockObject)
                {
                    if (!isCalculated)
                    {
                        try
                        {
                            value = new Value(supplier());
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                        }
                        isCalculated = true;
                        supplier = null;
                    }
                }
            }

            if (exception != null)
            {
                throw exception;
            }

            return value.Get();
        }
    }
}
