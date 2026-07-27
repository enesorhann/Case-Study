namespace CaseStudy.Services.Answer.Queries
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Dtos.Answer;
    using CaseStudy.Interfaces.Answer;
    using CaseStudy.Interfaces.Question;

    public class GetAnswersByQuestionQuery : IRequest<List<AnswerDto>?>
    {
        public Guid QuestionId { get; set; }
    }

    public class GetAnswersByQuestionQueryHandler : IRequestHandler<GetAnswersByQuestionQuery, List<AnswerDto>?>
    {
        private readonly IAnswerRepository answerRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IMapper mapper;

        public GetAnswersByQuestionQueryHandler(
            IAnswerRepository answerRepository,
            IQuestionRepository questionRepository,
            IMapper mapper)
        {
            this.answerRepository = answerRepository;
            this.questionRepository = questionRepository;
            this.mapper = mapper;
        }

        public async Task<List<AnswerDto>?> Handle(GetAnswersByQuestionQuery request, CancellationToken cancellationToken)
        {
            if (!await questionRepository.ExistsAsync(request.QuestionId))
            {
                return null;
            }

            var answers = await answerRepository.GetByQuestionIdAsync(request.QuestionId);

            return mapper.Map<List<AnswerDto>>(answers);
        }
    }
}
