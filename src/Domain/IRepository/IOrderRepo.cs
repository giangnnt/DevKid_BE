using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IRepository
{
    public interface IOrderRepo
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrder(Guid id);
        Task<bool> AddOrder(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrder(Guid id);
    }
}
