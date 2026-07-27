namespace CaseStudy.Services.Survey.Commands
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Constants;
    using CaseStudy.Dtos.Survey;
    using CaseStudy.Interfaces;
    using CaseStudy.Interfaces.Survey;
    using CaseStudy.Models;

    public class UpdateSurveyCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public UpdateSurveyDto Survey { get; set; } = null!;
    }

    public class UpdateSurveyCommandHandler : IRequestHandler<UpdateSurveyCommand, bool>
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

        public async Task<bool> Handle(UpdateSurveyCommand request, CancellationToken cancellationToken)
        {
            var survey = await surveyRepository.GetByIdAsync(request.Id);

            if (survey is null)
            {
                return false;
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
                RabbitMqConstants.SurveyUpdatedRoutingKey,
                cancellationToken);

            return true;
        }
    }
}
