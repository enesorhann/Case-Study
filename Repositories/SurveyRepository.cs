using Microsoft.EntityFrameworkCore;
using CaseStudy.Data;
using CaseStudy.Domain.Entities;
using CaseStudy.Interfaces.Survey;

namespace CaseStudy.Repositories
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly DataContext _context;

        public SurveyRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Survey?> GetByIdAsync(Guid id)
        {
            return await _context.Surveys
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Survey?> GetWithQuestionsAsync(Guid id)
        {
            return await _context.Surveys
                .AsNoTracking()
                .Include(s => s.Questions.OrderBy(q => q.Order))
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Survey>> GetAllAsync(bool? isActive)
        {
            var query = _context.Surveys.AsNoTracking();

            if (isActive.HasValue)
            {
                query = query.Where(s => s.IsActive == isActive.Value);
            }

            return await query
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Surveys
                .AnyAsync(s => s.Id == id);
        }

        public async Task AddAsync(Survey survey)
        {
            await _context.Surveys.AddAsync(survey);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Survey survey)
        {
            _context.Surveys.Update(survey);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Survey survey)
        {
            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();
        }


    }
}
