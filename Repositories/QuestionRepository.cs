using Microsoft.EntityFrameworkCore;
using CaseStudy.Data;
using CaseStudy.Domain.Entities;
using CaseStudy.Interfaces.Question;

namespace CaseStudy.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DataContext _context;

        public QuestionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Question?> GetByIdAsync(Guid id)
        {
            return await _context.Questions
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Question?> GetWithSurveyAsync(Guid id)
        {
            return await _context.Questions
                .AsNoTracking()
                .Include(q => q.Survey)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<Question>> GetBySurveyIdAsync(Guid surveyId)
        {
            return await _context.Questions
                .AsNoTracking()
                .Where(q => q.SurveyId == surveyId)
                .OrderBy(q => q.Order)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Questions
                .AnyAsync(q => q.Id == id);
        }

        public async Task<int> GetMaxOrderAsync(Guid surveyId)
        {
            return await _context.Questions
                .Where(q => q.SurveyId == surveyId)
                .MaxAsync(q => (int?)q.Order) ?? 0;
        }

        public async Task AddAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Question question)
        {
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }
    }
}
