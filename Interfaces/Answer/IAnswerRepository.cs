namespace CaseStudy.Interfaces.Answer
{
    using CaseStudy.Domain.Entities;
    public interface IAnswerRepository
    {
        Task<Answer?> GetByIdAsync(Guid id);
        Task<List<Answer>> GetByQuestionIdAsync(Guid questionId);
        Task AddAsync(Answer answer);
        Task UpdateAsync(Answer answer);
        Task DeleteAsync(Answer answer);
    }
}
