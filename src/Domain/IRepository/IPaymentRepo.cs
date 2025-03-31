using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IRepository
{
    public interface IPaymentRepo
    {
        Task<IEnumerable<Payment>> GetPayments();
        Task<Payment> GetPayment(Guid id);
        Task<bool> AddPayment(Payment payment);
        Task<bool> UpdatePayment(Payment payment);
        Task<bool> DeletePayment(Guid id);
        Task<IEnumerable<Payment?>> GetPaymentsByUserId(Guid userId);
    }
}
