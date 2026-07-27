using Microsoft.EntityFrameworkCore;
using CaseStudy.Data;
using CaseStudy.Helpers;
using CaseStudy.Interfaces;
using CaseStudy.Interfaces.Answer;
using CaseStudy.Interfaces.Question;
using CaseStudy.Interfaces.Survey;
using CaseStudy.Models;
using CaseStudy.Repositories;
using CaseStudy.Services.Notifications;
using CaseStudy.Services.RabbitMQ;

namespace CaseStudy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(connectionString, npgsql => npgsql.EnableRetryOnFailure()));

            builder.Services.AddScoped<ISurveyRepository, SurveyRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();

            builder.Services.Configure<RabbitMqSettings>(
                builder.Configuration.GetSection(RabbitMqSettings.SectionName));

            builder.Services.AddSingleton<IEventPublisher, SurveyPublisher>();
            builder.Services.AddScoped<ISurveyNotificationService, LoggingSurveyNotificationService>();
            builder.Services.AddHostedService<SurveyConsumer>();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfiles).Assembly));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<DataContext>().Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
