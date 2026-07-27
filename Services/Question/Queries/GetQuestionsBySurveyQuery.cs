namespace CaseStudy.Services.Question.Queries
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Dtos.Question;
    using CaseStudy.Interfaces.Question;
    using CaseStudy.Interfaces.Survey;

    public class GetQuestionsBySurveyQuery : IRequest<List<QuestionDto>?>
    {
        public Guid SurveyId { get; set; }
    }

    public class GetQuestionsBySurveyQueryHandler : IRequestHandler<GetQuestionsBySurveyQuery, List<QuestionDto>?>
    {
        private readonly IQuestionRepository questionRepository;
        private readonly ISurveyRepository surveyRepository;
        private readonly IMapper mapper;

        public GetQuestionsBySurveyQueryHandler(
            IQuestionRepository questionRepository,
            ISurveyRepository surveyRepository,
            IMapper mapper)
        {
            this.questionRepository = questionRepository;
            this.surveyRepository = surveyRepository;
            this.mapper = mapper;
        }

        public async Task<List<QuestionDto>?> Handle(GetQuestionsBySurveyQuery request, CancellationToken cancellationToken)
        {
            if (!await surveyRepository.ExistsAsync(request.SurveyId))
            {
                return null;
            }

            var questions = await questionRepository.GetBySurveyIdAsync(request.SurveyId);

            return mapper.Map<List<QuestionDto>>(questions);
        }
    }
}
