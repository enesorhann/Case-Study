namespace CaseStudy.Domain.Entities
{

    public class Answer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string RespondentName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public Question Question { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}
