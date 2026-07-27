namespace CaseStudy.Services.Question.Commands
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Dtos.Question;
    using CaseStudy.Interfaces.Question;

    public class UpdateQuestionCommand : IRequest<QuestionDto?>
    {
        public Guid Id { get; set; }
        public UpdateQuestionDto Question { get; set; } = null!;
    }

    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, QuestionDto?>
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IMapper mapper;

        public UpdateQuestionCommandHandler(IQuestionRepository questionRepository, IMapper mapper)
        {
            this.questionRepository = questionRepository;
            this.mapper = mapper;
        }

        public async Task<QuestionDto?> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await questionRepository.GetByIdAsync(request.Id);

            if (question is null)
            {
                return null;
            }

            mapper.Map(request.Question, question);
            question.UpdatedAt = DateTime.UtcNow;

            await questionRepository.UpdateAsync(question);

            return mapper.Map<QuestionDto>(question);
        }
    }
}
