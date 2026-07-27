namespace CaseStudy.Services.Question.Commands
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Dtos.Question;
    using CaseStudy.Interfaces.Question;

    public class UpdateQuestionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public UpdateQuestionDto Question { get; set; } = null!;
    }

    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, bool>
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IMapper mapper;

        public UpdateQuestionCommandHandler(IQuestionRepository questionRepository, IMapper mapper)
        {
            this.questionRepository = questionRepository;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await questionRepository.GetByIdAsync(request.Id);

            if (question is null)
            {
                return false;
            }

            mapper.Map(request.Question, question);

            await questionRepository.UpdateAsync(question);

            return true;
        }
    }
}
