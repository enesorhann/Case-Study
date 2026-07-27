namespace CaseStudy.Interfaces.Question
{
    using CaseStudy.Domain.Entities;

    public interface IQuestionRepository
    {
        Task<Question?> GetByIdAsync(Guid id);
        Task<Question?> GetWithSurveyAsync(Guid id);
        Task<List<Question>> GetBySurveyIdAsync(Guid surveyId);
        Task<bool> ExistsAsync(Guid id);
        Task<int> GetMaxOrderAsync(Guid surveyId);
        Task AddAsync(Question question);
        Task UpdateAsync(Question question);
        Task DeleteAsync(Question question);
    }
}
