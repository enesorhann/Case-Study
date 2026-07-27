using CaseStudy.Constants.Enums;

namespace CaseStudy.Dtos.Question
{
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }
        public string Text { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
