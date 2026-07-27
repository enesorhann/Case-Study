namespace CaseStudy.Services.Answer.Commands
{
    using MediatR;
    using CaseStudy.Interfaces.Answer;

    public class DeleteAnswerCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand, bool>
    {
        private readonly IAnswerRepository answerRepository;

        public DeleteAnswerCommandHandler(IAnswerRepository answerRepository)
        {
            this.answerRepository = answerRepository;
        }

        public async Task<bool> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
        {
            var answer = await answerRepository.GetByIdAsync(request.Id);

            if (answer is null)
            {
                return false;
            }

            await answerRepository.DeleteAsync(answer);

            return true;
        }
    }
}
