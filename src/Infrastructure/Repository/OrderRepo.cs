using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Infrastructure.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly DevKidContext _context;
        public OrderRepo(DevKidContext context)
        {
            _context = context;
        }
        public async Task<bool> AddOrder(Order order)
        {
            _context.Orders.Add(order);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteOrder(Guid id)
        {
            _context.Orders.Remove(_context.Orders.Find(id) ?? throw new Exception("Order not found"));
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Order> GetOrder(Guid id)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Order not found");
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
