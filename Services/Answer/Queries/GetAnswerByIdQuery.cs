namespace CaseStudy.Services.Answer.Queries
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Dtos.Answer;
    using CaseStudy.Interfaces.Answer;

    public class GetAnswerByIdQuery : IRequest<AnswerDto?>
    {
        public Guid Id { get; set; }
    }

    public class GetAnswerByIdQueryHandler : IRequestHandler<GetAnswerByIdQuery, AnswerDto?>
    {
        private readonly IAnswerRepository answerRepository;
        private readonly IMapper mapper;

        public GetAnswerByIdQueryHandler(IAnswerRepository answerRepository, IMapper mapper)
        {
            this.answerRepository = answerRepository;
            this.mapper = mapper;
        }

        public async Task<AnswerDto?> Handle(GetAnswerByIdQuery request, CancellationToken cancellationToken)
        {
            var answer = await answerRepository.GetByIdAsync(request.Id);

            return answer is null ? null : mapper.Map<AnswerDto>(answer);
        }
    }
}
