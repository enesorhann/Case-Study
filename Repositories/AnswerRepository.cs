using Microsoft.EntityFrameworkCore;
using CaseStudy.Data;
using CaseStudy.Domain.Entities;
using CaseStudy.Interfaces.Answer;

namespace CaseStudy.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly DataContext _context;

        public AnswerRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Answer?> GetByIdAsync(Guid id)
        {
            return await _context.Answers
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Answer>> GetByQuestionIdAsync(Guid questionId)
        {
            return await _context.Answers
                .AsNoTracking()
                .Where(a => a.QuestionId == questionId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Answer answer)
        {
            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Answer answer)
        {
            _context.Answers.Update(answer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Answer answer)
        {
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
        }

    }
}
