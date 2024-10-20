namespace BooksAPI.Service.Interface
{
    public interface ICacheService
    {
        T GetOrCreate<T>(string key, Func<T> createItem, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);
        void Remove(string key);
    }
}
