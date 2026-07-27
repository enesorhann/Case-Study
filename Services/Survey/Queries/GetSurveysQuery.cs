namespace CaseStudy.Services.Survey.Queries
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Dtos.Survey;
    using CaseStudy.Interfaces.Survey;

    public class GetSurveysQuery : IRequest<List<SurveyListDto>>
    {
        public bool? IsActive { get; set; }
    }

    public class GetSurveysQueryHandler : IRequestHandler<GetSurveysQuery, List<SurveyListDto>>
    {
        private readonly ISurveyRepository surveyRepository;
        private readonly IMapper mapper;

        public GetSurveysQueryHandler(ISurveyRepository surveyRepository, IMapper mapper)
        {
            this.surveyRepository = surveyRepository;
            this.mapper = mapper;
        }

        public async Task<List<SurveyListDto>> Handle(GetSurveysQuery request, CancellationToken cancellationToken)
        {
            var surveys = await surveyRepository.GetAllAsync(request.IsActive);

            return mapper.Map<List<SurveyListDto>>(surveys);
        }
    }
}
