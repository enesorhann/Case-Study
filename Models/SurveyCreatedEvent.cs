namespace CaseStudy.Models
{
    public record SurveyCreatedEvent
    {
        public Guid SurveyId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    }
}
