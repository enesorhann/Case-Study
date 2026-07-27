namespace CaseStudy.Services.Answer.Commands
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Dtos.Answer;
    using CaseStudy.Interfaces.Answer;

    public class UpdateAnswerCommand : IRequest<AnswerDto?>
    {
        public Guid Id { get; set; }
        public UpdateAnswerDto Answer { get; set; } = null!;
    }

    public class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand, AnswerDto?>
    {
        private readonly IAnswerRepository answerRepository;
        private readonly IMapper mapper;

        public UpdateAnswerCommandHandler(IAnswerRepository answerRepository, IMapper mapper)
        {
            this.answerRepository = answerRepository;
            this.mapper = mapper;
        }

        public async Task<AnswerDto?> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
        {
            var answer = await answerRepository.GetByIdAsync(request.Id);

            if (answer is null)
            {
                return null;
            }

            mapper.Map(request.Answer, answer);
            answer.UpdatedAt = DateTime.UtcNow;

            await answerRepository.UpdateAsync(answer);

            return mapper.Map<AnswerDto>(answer);
        }
    }
}
