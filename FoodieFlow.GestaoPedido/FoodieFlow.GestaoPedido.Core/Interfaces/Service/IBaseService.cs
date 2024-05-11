namespace FoodieFlow.GestaoPedido.Core.Interfaces.Service
{
    public interface IBaseService<T>
    {
        T GetByModel(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);

    }
}
