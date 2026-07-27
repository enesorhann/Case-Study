namespace CaseStudy.Services.Survey.Queries
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Dtos.Survey;
    using CaseStudy.Interfaces.Survey;

    public class GetSurveyByIdQuery : IRequest<SurveyDetailDto?>
    {
        public Guid Id { get; set; }
    }

    public class GetSurveyByIdQueryHandler : IRequestHandler<GetSurveyByIdQuery, SurveyDetailDto?>
    {
        private readonly ISurveyRepository surveyRepository;
        private readonly IMapper mapper;

        public GetSurveyByIdQueryHandler(ISurveyRepository surveyRepository, IMapper mapper)
        {
            this.surveyRepository = surveyRepository;
            this.mapper = mapper;
        }

        public async Task<SurveyDetailDto?> Handle(GetSurveyByIdQuery request, CancellationToken cancellationToken)
        {
            var survey = await surveyRepository.GetWithQuestionsAsync(request.Id);

            return survey is null ? null : mapper.Map<SurveyDetailDto>(survey);
        }
    }
}
