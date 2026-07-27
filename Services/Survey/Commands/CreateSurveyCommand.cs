namespace CaseStudy.Services.Survey.Commands
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Constants;
    using CaseStudy.Domain.Entities;
    using CaseStudy.Dtos.Survey;
    using CaseStudy.Interfaces;
    using CaseStudy.Interfaces.Survey;
    using CaseStudy.Models;

    public class CreateSurveyCommand : IRequest<SurveyDetailDto>
    {
        public CreateSurveyDto Survey { get; set; } = null!;
    }

    public class CreateSurveyCommandHandler : IRequestHandler<CreateSurveyCommand, SurveyDetailDto>
    {
        private readonly ISurveyRepository surveyRepository;
        private readonly IEventPublisher eventPublisher;
        private readonly IMapper mapper;

        public CreateSurveyCommandHandler(
            ISurveyRepository surveyRepository,
            IEventPublisher eventPublisher,
            IMapper mapper)
        {
            this.surveyRepository = surveyRepository;
            this.eventPublisher = eventPublisher;
            this.mapper = mapper;
        }

        public async Task<SurveyDetailDto> Handle(CreateSurveyCommand request, CancellationToken cancellationToken)
        {
            var survey = mapper.Map<Survey>(request.Survey);

            survey.Id = Guid.NewGuid();
            survey.CreatedAt = DateTime.UtcNow;

            await surveyRepository.AddAsync(survey);

            await eventPublisher.PublishAsync(
                new SurveyCreatedEvent
                {
                    SurveyId = survey.Id,
                    Title = survey.Title,
                    Description = survey.Description,
                    IsActive = survey.IsActive,
                    CreatedAt = survey.CreatedAt
                },
                RabbitMqConstants.SurveyCreatedRoutingKey,
                cancellationToken);

            return mapper.Map<SurveyDetailDto>(survey);
        }
    }
}
