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
            var task = new MyTask<TResult>(func);
            tasks.Add(() => task.Start(tasks));
            return task;
        }

        /// <summary>
        /// blocks adding new tasks,
        /// waits waits until all already assigned tasks are completed and closes the threads
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
                thread = new Thread(() => this.Start());
                thread.Start();
                this.actions = actions;
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
                    if (token.IsCancellationRequested)
                    {
                        actions.CompleteAdding();
                    }

                    if (actions.TryTake(out var action))
                    {
                        IsWorking = true;
                        action();
                        IsWorking = false;
                    }
                    else if (actions.IsCompleted)
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
            private ManualResetEvent resetEvent = new ManualResetEvent(false);

            public bool IsComplited { get; private set; }

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

            public MyTask(Func<TResult> func)
            {
                IsComplited = false;
                Func = func;
            }

            /// <summary>
            /// puts the task to work
            /// </summary>
            /// <param name="actions"></param>
            public void Start(BlockingCollection<Action> actions)
            {
                try
                {
                    result = Func();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                IsComplited = true;
                resetEvent.Set();
            }

            public IMyTask<TResult> ContinueWith(Func<TResult, TResult> func)
            {
                throw new NotImplementedException();
            }
        }
    }
}
