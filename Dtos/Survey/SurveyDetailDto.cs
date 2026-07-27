using CaseStudy.Dtos.Question;

namespace CaseStudy.Dtos.Survey
{
    public class SurveyDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
    }
}
