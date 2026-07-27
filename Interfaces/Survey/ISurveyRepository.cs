namespace CaseStudy.Interfaces.Survey
{
    using CaseStudy.Domain.Entities;
    public interface ISurveyRepository
    {
        Task<Survey?> GetByIdAsync(Guid id);
        Task<Survey?> GetWithQuestionsAsync(Guid id);
        Task<List<Survey>> GetAllAsync(bool? isActive);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Survey survey);
        Task UpdateAsync(Survey survey);
        Task DeleteAsync(Survey survey);
    }
}
