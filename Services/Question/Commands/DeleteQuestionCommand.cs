namespace CaseStudy.Services.Question.Commands
{
    using MediatR;
    using CaseStudy.Interfaces.Question;

    public class DeleteQuestionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, bool>
    {
        private readonly IQuestionRepository questionRepository;

        public DeleteQuestionCommandHandler(IQuestionRepository questionRepository)
        {
            this.questionRepository = questionRepository;
        }

        public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await questionRepository.GetByIdAsync(request.Id);

            if (question is null)
            {
                return false;
            }

            await questionRepository.DeleteAsync(question);

            return true;
        }
    }
}
