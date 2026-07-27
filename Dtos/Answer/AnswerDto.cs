namespace CaseStudy.Dtos.Answer
{
    public class AnswerDto
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string RespondentName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
