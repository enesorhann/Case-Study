using System.ComponentModel.DataAnnotations;
using CaseStudy.Constants.Enums;

namespace CaseStudy.Dtos.Question
{
    public class UpdateQuestionDto
    {
        [Required]
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;

        public QuestionType QuestionType { get; set; }

        [Range(1, int.MaxValue)]
        public int Order { get; set; }
    }
}
