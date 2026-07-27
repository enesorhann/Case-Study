using CaseStudy.Interfaces;
using CaseStudy.Models;

namespace CaseStudy.Services.Notifications
{

    // Dış servise gerçek bir çağrı yapmak yerine
    // iletilecek bilgiyi log'a yazdırdık
    public class LoggingSurveyNotificationService : ISurveyNotificationService
    {
        private readonly ILogger<LoggingSurveyNotificationService> _logger;

        public LoggingSurveyNotificationService(ILogger<LoggingSurveyNotificationService> logger)
        {
            _logger = logger;
        }

        public Task NotifySurveyCreatedAsync(SurveyCreatedEvent surveyCreatedEvent, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Forwarding survey.created to downstream service. SurveyId: {SurveyId}, Title: {Title}, IsActive: {IsActive}",
                surveyCreatedEvent.SurveyId,
                surveyCreatedEvent.Title,
                surveyCreatedEvent.IsActive);

            return Task.CompletedTask;
        }

        public Task NotifySurveyUpdatedAsync(SurveyUpdatedEvent surveyUpdatedEvent, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Forwarding survey.updated to downstream service. SurveyId: {SurveyId}, Title: {Title}, IsActive: {IsActive}",
                surveyUpdatedEvent.SurveyId,
                surveyUpdatedEvent.Title,
                surveyUpdatedEvent.IsActive);

            return Task.CompletedTask;
        }
    }
}
