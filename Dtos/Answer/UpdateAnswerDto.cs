using System.ComponentModel.DataAnnotations;

namespace CaseStudy.Dtos.Answer
{
    public class UpdateAnswerDto
    {
        [Required]
        [StringLength(150)]
        public string RespondentName { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Value { get; set; } = string.Empty;
    }
}
