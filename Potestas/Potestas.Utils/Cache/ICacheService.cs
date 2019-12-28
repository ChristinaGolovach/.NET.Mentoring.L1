namespace Potestas.Utils.Cache
{
    public interface ICacheService<TKey, TItem>
    {
        TItem GetItem(TKey key);
        void AddItem(TKey key, TItem item, int lifeTimeInSeconds);
        void DeleteItem(TKey key);
        void Clear();
    }
}
