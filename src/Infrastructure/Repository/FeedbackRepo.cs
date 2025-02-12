using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Infrastructure.Repository
{
    public class FeedbackRepo : IFeedbackRepo
    {
        private readonly DevKidContext _context;
        public FeedbackRepo(DevKidContext context)
        {
            _context = context;
        }
        public async Task<bool> AddFeedback(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteFeedback(Guid id)
        {
            _context.Feedbacks.Remove(_context.Feedbacks.Find(id) ?? throw new Exception("Feedback not found"));
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Feedback> GetFeedback(Guid id)
        {
            return await _context.Feedbacks.FindAsync(id) ?? throw new Exception("Feedback not found");
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacks()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        public async Task<bool> UpdateFeedback(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
