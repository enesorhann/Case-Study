using MediatR;
using Microsoft.AspNetCore.Mvc;
using CaseStudy.Dtos.Question;
using CaseStudy.Services.Question.Commands;
using CaseStudy.Services.Question.Queries;

namespace CaseStudy.Controllers
{
    [Route("api")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuestionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("surveys/{surveyId:guid}/questions")]
        public async Task<IActionResult> GetBySurvey([FromRoute] Guid surveyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new GetQuestionsBySurveyQuery { SurveyId = surveyId });
            if (result == null)
            {
                ModelState.AddModelError("", $"Survey '{surveyId}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return Ok(result);
        }

        [HttpPost("surveys/{surveyId:guid}/questions")]
        public async Task<IActionResult> Create([FromRoute] Guid surveyId, [FromBody] CreateQuestionDto createQuestionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new CreateQuestionCommand { SurveyId = surveyId, Question = createQuestionDto });
            if (result == null)
            {
                ModelState.AddModelError("", $"Survey '{surveyId}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("questions/{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new GetQuestionByIdQuery { Id = id });
            if (result == null)
            {
                ModelState.AddModelError("", $"Question '{id}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return Ok(result);
        }

        [HttpPut("questions/{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateQuestionDto updateQuestionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _mediator.Send(new UpdateQuestionCommand { Id = id, Question = updateQuestionDto });
            if (!updated)
            {
                ModelState.AddModelError("", $"Question '{id}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return NoContent();
        }

        [HttpDelete("questions/{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleted = await _mediator.Send(new DeleteQuestionCommand { Id = id });
            if (!deleted)
            {
                ModelState.AddModelError("", $"Question '{id}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return NoContent();
        }
    }
}
