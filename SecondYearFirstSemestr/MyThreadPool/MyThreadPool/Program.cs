using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ThreadPool
{
    class Program
    {
        public static void Main(string[] args)
        {
            var pool = new MyThreadPool();
            var task = pool.AddTask(() => 5 * 5);
            var continuedTask = task.ContinueWith((int x) => x * 5);
            pool.ShutDown();
            Console.WriteLine(task.Result);
        }
    }

}