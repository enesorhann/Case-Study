using System.ComponentModel.DataAnnotations;

namespace CaseStudy.Dtos.Survey
{
    public class UpdateSurveyDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
