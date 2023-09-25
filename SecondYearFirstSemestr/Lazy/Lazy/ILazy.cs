namespace Lazy
{
    /// <summary>
    /// provides lazy calculation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILazy<T>
    {
        /// <summary>
        /// The first call returns object that calculates by supplier
        /// Other calls return the same object as the first without recalculations
        /// </summary>
        /// <returns></returns>
        public T Get();
    }
}
