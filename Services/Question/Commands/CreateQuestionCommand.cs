namespace CaseStudy.Services.Question.Commands
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Domain.Entities;
    using CaseStudy.Dtos.Question;
    using CaseStudy.Interfaces.Question;
    using CaseStudy.Interfaces.Survey;

    public class CreateQuestionCommand : IRequest<QuestionDto?>
    {
        public Guid SurveyId { get; set; }
        public CreateQuestionDto Question { get; set; } = null!;
    }

    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, QuestionDto?>
    {
        private readonly IQuestionRepository questionRepository;
        private readonly ISurveyRepository surveyRepository;
        private readonly IMapper mapper;

        public CreateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ISurveyRepository surveyRepository,
            IMapper mapper)
        {
            this.questionRepository = questionRepository;
            this.surveyRepository = surveyRepository;
            this.mapper = mapper;
        }

        public async Task<QuestionDto?> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            if (!await surveyRepository.ExistsAsync(request.SurveyId))
            {
                return null;
            }

            var question = mapper.Map<Domain.Entities.Question>(request.Question);

            question.Id = Guid.NewGuid();
            question.SurveyId = request.SurveyId;
            question.CreatedAt = DateTime.UtcNow;

            question.Order = request.Question.Order
                ?? await questionRepository.GetMaxOrderAsync(request.SurveyId) + 1;

            await questionRepository.AddAsync(question);

            return mapper.Map<QuestionDto>(question);
        }
    }
}
