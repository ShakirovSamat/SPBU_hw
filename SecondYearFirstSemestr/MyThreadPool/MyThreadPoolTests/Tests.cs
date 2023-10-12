using System.Threading;
using ThreadPool;
using ThreadPool.Exceptions;

namespace Test
{
    public class Tests
    {
        MyThreadPool pool;
        [SetUp]
        public void SetUp()
        {
            pool = new MyThreadPool();
        }

        [Test]
        public void OneTaskTest()
        {
            var task = pool.AddTask(() => 5 * 5);
            pool.ShutDown();
            Assert.That(task.Result, Is.EqualTo(25));
        }

        [Test]
        public void ManyTasksTest()
        {
            Random random = new Random();
            var tasksCount = 12;
            var tasks = new IMyTask<int>[tasksCount];
            for (int i = 0; i < tasksCount; ++i)
            {
                var localI = i;
                tasks[i] = pool.AddTask(() =>
                {
                    Thread.Sleep(random.Next(200));
                    return localI * localI;
                });
            }

            for (int i = 0; i < tasksCount; ++i)
            {
                Assert.That(tasks[i].Result, Is.EqualTo(i * i));
            }
        }

        [Test]
        public void ContinueWithTest()
        {
            var task = pool.AddTask(() => 5 * 5);
            var continuedTask = task.ContinueWith((int x) => x * 5);
            Assert.That(continuedTask.Result, Is.EqualTo(125));
            pool.ShutDown();
        }

        [Test]
        public void ContinuWithDelayTest()
        {
            var task = pool.AddTask(() => 
            {
                Thread.Sleep(300);
                return 5 * 5;
            });
            var continuedTask = task.ContinueWith((int x) => x * 5);
            Assert.That(continuedTask.Result, Is.EqualTo(125));
        }

        [Test]
        public void BadContinueWithTest()
        {
            var task = pool.AddTask(() => 5 * 5);
            pool.ShutDown();
            Assert.Throws<ShutDownedException>(() => task.ContinueWith((int x) => x * 5));
            pool.ShutDown();
        }
    }
}