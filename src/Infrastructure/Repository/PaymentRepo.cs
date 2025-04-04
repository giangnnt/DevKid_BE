﻿using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Infrastructure.Repository
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly DevKidContext _context;
        public PaymentRepo(DevKidContext context)
        {
            _context = context;
        }
        public async Task<bool> AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeletePayment(Guid id)
        {
            _context.Payments.Remove(_context.Payments.Find(id) ?? throw new Exception("Payment not found"));
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Payment> GetPayment(Guid id)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Payment not found");
        }

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<IEnumerable<Payment?>> GetPaymentsByUserId(Guid userId)
        {
            var orders = await _context.Orders.Where(o => o.StudentId == userId)
                .Include(o => o.Payment)
                .ToListAsync();
            var payments = orders.Select(o => o.Payment).ToList();
            return payments;
        }

        public async Task<bool> UpdatePayment(Payment payment)
        {
            payment.UpdateAt = DateTime.UtcNow;
            _context.Payments.Update(payment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
