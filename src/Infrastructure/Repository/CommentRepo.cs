using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Infrastructure.Repository
{
    public class CommentRepo : ICommentRepo
    {
        private readonly DevKidContext _context;
        public CommentRepo(DevKidContext context)
        {
            _context = context;
        }
        public async Task<bool> AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteComment(Guid id)
        {
            _context.Comments.Remove(_context.Comments.Find(id) ?? throw new Exception("Comment not found"));
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Comment> GetComment(Guid id)
        {
            return await _context.Comments.FindAsync(id) ?? throw new Exception("Comment not found");
        }

        public async Task<IEnumerable<Comment>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<bool> UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
