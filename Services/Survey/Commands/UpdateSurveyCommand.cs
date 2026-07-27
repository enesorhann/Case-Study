namespace CaseStudy.Services.Survey.Commands
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Constants;
    using CaseStudy.Dtos.Survey;
    using CaseStudy.Interfaces;
    using CaseStudy.Interfaces.Survey;
    using CaseStudy.Models;

    public class UpdateSurveyCommand : IRequest<SurveyDetailDto?>
    {
        public Guid Id { get; set; }
        public UpdateSurveyDto Survey { get; set; } = null!;
    }

    public class UpdateSurveyCommandHandler : IRequestHandler<UpdateSurveyCommand, SurveyDetailDto?>
    {
        private readonly ISurveyRepository surveyRepository;
        private readonly IEventPublisher eventPublisher;
        private readonly IMapper mapper;

        public UpdateSurveyCommandHandler(
            ISurveyRepository surveyRepository,
            IEventPublisher eventPublisher,
            IMapper mapper)
        {
            this.surveyRepository = surveyRepository;
            this.eventPublisher = eventPublisher;
            this.mapper = mapper;
        }

        public async Task<SurveyDetailDto?> Handle(UpdateSurveyCommand request, CancellationToken cancellationToken)
        {
            var survey = await surveyRepository.GetByIdAsync(request.Id);

            if (survey is null)
            {
                return null;
            }

            mapper.Map(request.Survey, survey);
            survey.UpdatedAt = DateTime.UtcNow;

            await surveyRepository.UpdateAsync(survey);

            await eventPublisher.PublishAsync(
                new SurveyUpdatedEvent
                {
                    SurveyId = survey.Id,
                    Title = survey.Title,
                    Description = survey.Description,
                    IsActive = survey.IsActive,
                    UpdatedAt = survey.UpdatedAt
                },
                cancellationToken);

            return mapper.Map<SurveyDetailDto>(survey);
        }
    }
}
