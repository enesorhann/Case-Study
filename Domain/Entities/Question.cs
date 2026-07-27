using CaseStudy.Constants.Enums;

namespace CaseStudy.Domain.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }
        public string Text { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; }
        public int Order { get; set; }
        public Survey Survey { get; set; } = null!;
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
