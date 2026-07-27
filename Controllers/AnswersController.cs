using MediatR;
using Microsoft.AspNetCore.Mvc;
using CaseStudy.Dtos.Answer;
using CaseStudy.Models;
using CaseStudy.Services.Answer.Commands;
using CaseStudy.Services.Answer.Queries;

namespace CaseStudy.Controllers
{
    [Route("api")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnswersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("questions/{questionId:guid}/answers")]
        public async Task<IActionResult> GetByQuestion([FromRoute] Guid questionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new GetAnswersByQuestionQuery { QuestionId = questionId });
            if (result == null)
            {
                ModelState.AddModelError("", $"Question '{questionId}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return Ok(result);
        }

        [HttpPost("questions/{questionId:guid}/answers")]
        public async Task<IActionResult> Create([FromRoute] Guid questionId, [FromBody] CreateAnswerDto createAnswerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AnswerDto? result;
            try
            {
                result = await _mediator.Send(new CreateAnswerCommand { QuestionId = questionId, Answer = createAnswerDto });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return Conflict(new SerializableError(ModelState));
            }

            if (result == null)
            {
                ModelState.AddModelError("", $"Question '{questionId}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("answers/{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new GetAnswerByIdQuery { Id = id });
            if (result == null)
            {
                ModelState.AddModelError("", $"Answer '{id}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return Ok(result);
        }

        [HttpPut("answers/{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAnswerDto updateAnswerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _mediator.Send(new UpdateAnswerCommand { Id = id, Answer = updateAnswerDto });
            if (!updated)
            {
                ModelState.AddModelError("", $"Answer '{id}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return NoContent();
        }

        [HttpDelete("answers/{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleted = await _mediator.Send(new DeleteAnswerCommand { Id = id });
            if (!deleted)
            {
                ModelState.AddModelError("", $"Answer '{id}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return NoContent();
        }
    }
}
