namespace CaseStudy.Services.Answer.Commands
{
    using AutoMapper;
    using MediatR;
    using CaseStudy.Dtos.Answer;
    using CaseStudy.Exceptions;
    using CaseStudy.Interfaces.Answer;
    using CaseStudy.Interfaces.Question;
    using CaseStudy.Models;

    public class CreateAnswerCommand : IRequest<AnswerDto?>
    {
        public Guid QuestionId { get; set; }
        public CreateAnswerDto Answer { get; set; } = null!;
    }

    public class CreateAnswerCommandHandler : IRequestHandler<CreateAnswerCommand, AnswerDto?>
    {
        private readonly IAnswerRepository answerRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IMapper mapper;

        public CreateAnswerCommandHandler(
            IAnswerRepository answerRepository,
            IQuestionRepository questionRepository,
            IMapper mapper)
        {
            this.answerRepository = answerRepository;
            this.questionRepository = questionRepository;
            this.mapper = mapper;
        }

        public async Task<AnswerDto?> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
        {
            var question = await questionRepository.GetWithSurveyAsync(request.QuestionId);

            if (question is null)
            {
                return null;
            }

            if (!question.Survey.IsActive)
            {
                throw new SurveyInactiveException(
                    $"Survey '{question.Survey.Title}' is not active and cannot accept new answers.");
            }

            var answer = mapper.Map<Domain.Entities.Answer>(request.Answer);

            answer.Id = Guid.NewGuid();
            answer.QuestionId = request.QuestionId;
            answer.CreatedAt = DateTime.UtcNow;

            await answerRepository.AddAsync(answer);

            return mapper.Map<AnswerDto>(answer);
        }
    }
}
