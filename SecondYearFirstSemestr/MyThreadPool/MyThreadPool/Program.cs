using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ThreadPool
{
    class Program
    {
        public static void Main(string[] args)
        {
            var pool = new MyThreadPool();
            var task = pool.AddTask(() =>
            {
                Thread.Sleep(1000);
                return 2 + 2;
            });
            var task2 = task.ContinueWith((int restult) => restult * 5);
            Console.WriteLine(task.Result);
            Console.WriteLine(task2.Result);
            pool.ShutDown();
        }
    }

}