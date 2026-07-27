using CaseStudy.Models;

namespace CaseStudy.Interfaces
{
    /// <summary>
    /// Kuyruktan okunan anket olaylarının iletileceği dış servis sözleşmesi.
    /// Consumer bu soyutlamayı çağırır; gerçek bir kurulumda bunun arkasında
    /// bir HTTP istemcisi, e-posta servisi veya raporlama servisi bulunur.
    /// </summary>
    public interface ISurveyNotificationService
    {
        Task NotifySurveyCreatedAsync(SurveyCreatedEvent surveyCreatedEvent, CancellationToken cancellationToken = default);

        Task NotifySurveyUpdatedAsync(SurveyUpdatedEvent surveyUpdatedEvent, CancellationToken cancellationToken = default);
    }
}
