namespace CaseStudy.Services.Question.Queries
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Dtos.Question;
    using CaseStudy.Interfaces.Question;

    public class GetQuestionByIdQuery : IRequest<QuestionDto?>
    {
        public Guid Id { get; set; }
    }

    public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDto?>
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IMapper mapper;

        public GetQuestionByIdQueryHandler(IQuestionRepository questionRepository, IMapper mapper)
        {
            this.questionRepository = questionRepository;
            this.mapper = mapper;
        }

        public async Task<QuestionDto?> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var question = await questionRepository.GetByIdAsync(request.Id);

            return question is null ? null : mapper.Map<QuestionDto>(question);
        }
    }
}
