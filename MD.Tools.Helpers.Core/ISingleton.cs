namespace MD.Tools.Helpers.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISingleton<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T GetSingletonInstance();
    }
}
