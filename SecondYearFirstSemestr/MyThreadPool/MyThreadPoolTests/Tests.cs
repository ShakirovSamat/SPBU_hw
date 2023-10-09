using System.Threading;
using ThreadPool;

namespace MyThreadPoolTests
{
    public class Tests
    {
        [SetUp]
        public void SetUp()
        {
        }
        [Test]
        public void SimpleTaskTest()
        {
            var pool = new MyThreadPool();
            var task = pool.AddTask(() => 5 * 5);
            pool.ShutDown();
            Assert.That(task.Result, Is.EqualTo(25));
        }

        [Test]
        public void SimpleContinueWithTest()
        {
            var pool = new MyThreadPool();
            var task = pool.AddTask(() => 5 * 5);
            var continuedTask = task.ContinueWith((int x) => x * 5);
            pool.ShutDown();
            Assert.That(continuedTask.Result, Is.EqualTo(125));
        }
    }
}