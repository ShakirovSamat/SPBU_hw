using NuGet.Frameworks;
using System;

namespace LazyTests
{
    public class Tests
    {
        Func<object> func;
        Random rand;

        [SetUp]
        public void Setup()
        {
            rand = new Random();
            func = () =>
            {
                return rand.Next(100) + rand.Next(100);
            };
        }

        [Test]
        public void LazyOneThreadTest()
        {
            var lazyOneThread = new LazyOneThread<object>(func);
            var result1 = lazyOneThread.Get();
            var result2 = lazyOneThread.Get();
            var result3 = lazyOneThread.Get();
            var result4 = new LazyOneThread<object>(func).Get();

            Assert.IsTrue(ReferenceEquals(result1, result2));
            Assert.IsTrue(ReferenceEquals(result1, result3));
            Assert.IsTrue(ReferenceEquals(result2, result3));
            Assert.IsFalse(ReferenceEquals(result1, result4));
        }

        [Test]
        public void ParallelLazyTest()
        {
            var parallelLazy = new ParallelLazy<object>(func);
            Thread[] threads = new Thread[3];
            object[] result = new object[3];
            for (int i = 0; i < 3; ++i)
            {
                int localI = i;
                threads[i] = new Thread(() =>
                {
                    result[localI] = parallelLazy.Get();
                });
            }

            foreach (Thread thread in threads)
            {
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            var result4 = new ParallelLazy<object>(func).Get();
            Assert.IsTrue(ReferenceEquals(result[0], result[1]));
            Assert.IsTrue(ReferenceEquals(result[0], result[2]));
            Assert.IsTrue(ReferenceEquals(result[1], result[2]));
            Assert.IsFalse(ReferenceEquals(result[0], result4));
        }

        [Test]
        public void RaceTest() 
        {
            var parallelLazy = new ParallelLazy<object>(func);
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            var thredsCount = 5;
            Thread[] threads = new Thread[thredsCount];
            Object?[] result = new Object[thredsCount];
            for (int i = 0; i < thredsCount; ++i)
            {
                var localI = i;
                threads[i] = new Thread(() =>
                {
                    resetEvent.WaitOne();
                    result[localI] = parallelLazy.Get();
                }); 
            }

            foreach (Thread thread in threads)
            {
                thread.Start();
            }

            resetEvent.Set();

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Assert.IsTrue(ReferenceEquals(result[0], result[1]));
            Assert.IsTrue(ReferenceEquals(result[0], result[2]));
            Assert.IsTrue(ReferenceEquals(result[0], result[3]));
            Assert.IsTrue(ReferenceEquals(result[0], result[4]));
            Assert.IsTrue(ReferenceEquals(result[1], result[2]));
            Assert.IsTrue(ReferenceEquals(result[1], result[3]));
            Assert.IsTrue(ReferenceEquals(result[1], result[4]));
            Assert.IsTrue(ReferenceEquals(result[2], result[3]));
            Assert.IsTrue(ReferenceEquals(result[2], result[4]));
            Assert.IsTrue(ReferenceEquals(result[3], result[4]));
        }
    }
}