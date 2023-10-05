using System.Threading;
using ThreadPool;

namespace MyThreadPoolTests
{
    public class Tests
    {
        [Test]
        public void SimpleTaskTest()
        {
            MyThreadPool pool = new MyThreadPool();
            var task = pool.AddTask(() => 2 + 2);
            pool.ShutDown();
            Assert.That(task.Result, Is.EqualTo(25));
        }
    }
}