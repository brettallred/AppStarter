namespace AppStarter.Contracts.Repositories
{
    public interface IRepository<T>
    {
        T Load(string id);
        void Save(T item);
        void Delete(T item);
    }
}
