using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IRepository
{
    public interface IOrderRepo
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrder(long id);
        Task<bool> AddOrder(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrder(long id);
        Task<bool> HaveUserBoughtCourse(Guid courseId, Guid userId);
    }
}
