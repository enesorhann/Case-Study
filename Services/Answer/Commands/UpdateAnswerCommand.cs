namespace CaseStudy.Services.Answer.Commands
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Dtos.Answer;
    using CaseStudy.Interfaces.Answer;

    public class UpdateAnswerCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public UpdateAnswerDto Answer { get; set; } = null!;
    }

    public class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand, bool>
    {
        private readonly IAnswerRepository answerRepository;
        private readonly IMapper mapper;

        public UpdateAnswerCommandHandler(IAnswerRepository answerRepository, IMapper mapper)
        {
            this.answerRepository = answerRepository;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
        {
            var answer = await answerRepository.GetByIdAsync(request.Id);

            if (answer is null)
            {
                return false;
            }

            mapper.Map(request.Answer, answer);

            await answerRepository.UpdateAsync(answer);

            return true;
        }
    }
}
