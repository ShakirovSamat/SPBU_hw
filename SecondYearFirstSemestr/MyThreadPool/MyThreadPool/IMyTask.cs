namespace ThreadPool
{
    public interface IMyTask<TResult>
    {
        public bool IsComplited { get; }

        public TResult Result { get; }

        public Func<TResult> Func { get; }

        public IMyTask<TResult> ContinueWith(Func<TResult, TResult> func);


    }
}
