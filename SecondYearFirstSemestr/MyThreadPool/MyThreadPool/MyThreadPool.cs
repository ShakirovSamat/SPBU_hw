using ThreadPool.Exceptions;
using System.Collections.Concurrent;
using System;

namespace ThreadPool
{
    public class MyThreadPool
    {
        private ConcurrentQueue<Action> tasks;
        private Thread[] threads;
        private AutoResetEvent taskOver;
        private CancellationTokenSource cancellationTokenSource;

        public MyThreadPool()
        {
            tasks = new ConcurrentQueue<Action>();
            threads = new Thread[Environment.ProcessorCount];
            cancellationTokenSource = new CancellationTokenSource();
            for (int i = 0; i < Environment.ProcessorCount; ++i)
            {
                threads[i] = new Thread(() => Start());
                threads[i].Start();
            }
        }

        /// <summary>
        /// adds task to the pool 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public IMyTask<TResult> AddTask<TResult>(Func<TResult> func)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
            {
                throw new ShutDownedException();
            }

            var task = new MyTask<TResult>(this, func, tasks, cancellationTokenSource.Token);
            tasks.Enqueue(() => task.Start());

            return task;
        }

        /// <summary>
        /// blocks adding new tasks,
        /// waits until all already assigned tasks are completed and closes the threads
        /// </summary>
        public void ShutDown()
        {
            cancellationTokenSource.Cancel();
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

		private void Start()
		{
			while (true)
			{
				if (tasks.TryDequeue(out var task))
				{
					task();
				}
				else if (cancellationTokenSource.IsCancellationRequested)
				{
					break;
				}
			}
		}


		private class MyTask<TResult> : IMyTask<TResult>
        {
            private MyThreadPool pool;
            private Exception exception;
            private TResult result;
            private ConcurrentQueue<Action> tasks;
            private ManualResetEvent resetEvent = new ManualResetEvent(false);
            private ConcurrentQueue<Action> continuedTaks;
            private CancellationToken token;

            public bool IsCompleted { get; private set; }

            public TResult Result
            {
                get
                {
                    resetEvent.WaitOne();
                    if (exception != null)
                    {
                        throw new AggregateException(exception);
                    }
                    return result;
                }
                set { }
            }


            public Func<TResult> Func { get; }

            public MyTask(MyThreadPool pool, Func<TResult> func, ConcurrentQueue<Action> tasks, CancellationToken token)
            {
                this.pool = pool;
                IsCompleted = false;
                Func = func;
                this.tasks = tasks;
                continuedTaks = new ConcurrentQueue<Action>();
                this.token = token;
            }

            /// <summary>
            /// puts the task to work
            /// </summary>
            /// <param name="actions"></param>
            public void Start()
            {
                try
                {
                    result = Func();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                IsCompleted = true;
                resetEvent.Set();

                foreach (var continuedTask in continuedTaks)
                {
                   tasks.Enqueue(continuedTask);
                }
            }

            /// <summary>
            /// using the result of the current task calculates func<Tresult, Tresult> and returns a new task
            /// </summary>
            /// <param name="func"></param>
            /// <returns></returns>
            public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
            {
                if (token.IsCancellationRequested)
                {
                    throw new ShutDownedException();
                }

                var task = new MyTask<TNewResult>(pool, () => func(Result), tasks, token);
                if (IsCompleted)
                {
                    pool.AddTask(() => func(Result));
                    return task;
                }

                continuedTaks.Enqueue(() => task.Start());
                return task;
            }
        }
    }
}
