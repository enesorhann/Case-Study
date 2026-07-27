using AutoMapper;
using CaseStudy.Domain.Entities;
using CaseStudy.Dtos.Answer;
using CaseStudy.Dtos.Question;
using CaseStudy.Dtos.Survey;

namespace CaseStudy.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Survey, SurveyListDto>();
            CreateMap<Survey, SurveyDetailDto>();

            CreateMap<CreateSurveyDto, Survey>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.Questions, o => o.Ignore());

            CreateMap<UpdateSurveyDto, Survey>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.Questions, o => o.Ignore());

            CreateMap<Question, QuestionDto>();

            CreateMap<CreateQuestionDto, Question>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.SurveyId, o => o.Ignore())
                .ForMember(d => d.Order, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.Survey, o => o.Ignore())
                .ForMember(d => d.Answers, o => o.Ignore());

            CreateMap<UpdateQuestionDto, Question>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.SurveyId, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.Survey, o => o.Ignore())
                .ForMember(d => d.Answers, o => o.Ignore());

            CreateMap<Answer, AnswerDto>();

            CreateMap<CreateAnswerDto, Answer>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.QuestionId, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.Question, o => o.Ignore());

            CreateMap<UpdateAnswerDto, Answer>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.QuestionId, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.Question, o => o.Ignore());
        }
    }
}
