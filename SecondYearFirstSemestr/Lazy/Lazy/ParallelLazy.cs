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

        private Func<T> supplier;
        private bool isCalculated;
        private Value? value;
        private object lockObject = "st";

        public ParallelLazy(Func<T> func)
        {
            supplier = func;
            isCalculated = false;
        }

        public T Get()
        {
            if (!isCalculated)
            {
                lock (lockObject)
                {
                    if (!isCalculated)
                    {
                        Volatile.Write(ref value, new Value(supplier()));
                        isCalculated = true;
                    }
                }
            }
            return Volatile.Read(ref value)!.Get();
        }
    }
}
