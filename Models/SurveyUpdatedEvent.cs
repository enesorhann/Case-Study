namespace CaseStudy.Models
{
    public record SurveyUpdatedEvent
    {
        public Guid SurveyId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public bool IsActive { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    }
}
