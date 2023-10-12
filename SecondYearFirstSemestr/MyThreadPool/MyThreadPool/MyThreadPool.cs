using ThreadPool.Exceptions;
using System.Collections.Concurrent;

namespace ThreadPool
{
    public class MyThreadPool
    {
        private BlockingCollection<Action> tasks;
        private MyThread[] threads;
        private CancellationTokenSource cancellationTokenSource;

        public MyThreadPool()
        {
            tasks = new BlockingCollection<Action>();
            threads = new MyThread[Environment.ProcessorCount];
            cancellationTokenSource = new CancellationTokenSource();
            for (int i = 0; i < Environment.ProcessorCount; ++i)
            {
                threads[i] = new MyThread(tasks, cancellationTokenSource.Token);
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

            var task = new MyTask<TResult>(func, tasks, cancellationTokenSource.Token);
            tasks.Add(() => task.Start());
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


        private class MyThread
        {
            private Thread thread;
            private BlockingCollection<Action> actions;
            private CancellationToken token;

            public MyThread(BlockingCollection<Action> actions, CancellationToken token)
            {
                this.token = token;
                this.actions = actions;
                thread = new Thread(() => this.Start());
                thread.Start();
            }

            /// <summary>
            /// Joins thread
            /// </summary>
            public void Join()
            {
                thread.Join();
            }

            /// <summary>
            /// puts the thread to work
            /// </summary>
            private void Start()
            {
                while (true)
                {
                    if (actions.TryTake(out var action))
                    {
                        IsWorking = true;
                        action();
                        IsWorking = false;
                    }
                    else if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }

            public bool IsWorking { get; set; }
        }


        private class MyTask<TResult> : IMyTask<TResult>
        {
            private Exception exception;
            private TResult result;
            private BlockingCollection<Action> tasks;
            private ManualResetEvent resetEvent = new ManualResetEvent(false);
            private BlockingCollection<Action> continuedTaks;
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

            public MyTask(Func<TResult> func, BlockingCollection<Action> tasks, CancellationToken token)
            {
                IsCompleted = false;
                Func = func;
                this.tasks = tasks;
                continuedTaks = new BlockingCollection<Action>();
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
                    tasks.Add(continuedTask);
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

                var task = new MyTask<TNewResult>(() => func(Result), tasks, token);
                if (IsCompleted)
                {
                    tasks.Add(() => task.Start());
                    return task;
                }

                continuedTaks.Add(() => task.Start());
                return task;
            }
        }
    }
}
