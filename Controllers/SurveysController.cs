using MediatR;
using Microsoft.AspNetCore.Mvc;
using CaseStudy.Dtos.Survey;
using CaseStudy.Services.Survey.Commands;
using CaseStudy.Services.Survey.Queries;

namespace CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveysController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SurveysController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new GetSurveysQuery { IsActive = isActive });
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new GetSurveyByIdQuery { Id = id });
            if (result == null)
            {
                ModelState.AddModelError("", $"Survey '{id}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSurveyDto createSurveyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new CreateSurveyCommand { Survey = createSurveyDto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSurveyDto updateSurveyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _mediator.Send(new UpdateSurveyCommand { Id = id, Survey = updateSurveyDto });
            if (updated == null)
            {
                ModelState.AddModelError("", $"Survey '{id}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleted = await _mediator.Send(new DeleteSurveyCommand { Id = id });
            if (!deleted)
            {
                ModelState.AddModelError("", $"Survey '{id}' was not found.");
                return NotFound(new SerializableError(ModelState));
            }

            return NoContent();
        }
    }
}
