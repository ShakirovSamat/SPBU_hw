namespace ThreadPool
{
    public interface IMyTask<TResult>
    {
        public bool IsComplited { get; }

        public TResult Result { get; }

        public Func<TResult> Func { get; }

        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);


    }
}
