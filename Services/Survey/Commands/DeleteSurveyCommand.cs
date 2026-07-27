namespace CaseStudy.Services.Survey.Commands
{
    using MediatR;
    using CaseStudy.Interfaces.Survey;

    public class DeleteSurveyCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteSurveyCommandHandler : IRequestHandler<DeleteSurveyCommand, bool>
    {
        private readonly ISurveyRepository surveyRepository;

        public DeleteSurveyCommandHandler(ISurveyRepository surveyRepository)
        {
            this.surveyRepository = surveyRepository;
        }

        public async Task<bool> Handle(DeleteSurveyCommand request, CancellationToken cancellationToken)
        {
            var survey = await surveyRepository.GetByIdAsync(request.Id);

            if (survey is null)
            {
                return false;
            }

            await surveyRepository.DeleteAsync(survey);

            return true;
        }
    }
}
