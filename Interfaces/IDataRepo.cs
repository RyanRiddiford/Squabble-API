namespace Squabble.Interfaces
{
    internal interface IDataRepo<TEntity, TKey> where TEntity : class
    {
        // Find and return entity row with matching id
        TEntity GetById(TKey id);
    }
}
