using CaseStudy.Models;

namespace CaseStudy.Interfaces
{

    /// Kuyruktan okunan anket olaylarının iletileceği dış servis
    //  Consumer bu soyutlamayı çağırır; gerçek bir kurulumda bunun arkasında
    /// bir başka servis bulunur. 
    /// Örneğin, bir e-posta servisi veya bir bildirim servisi olabilir.

    public interface ISurveyNotificationService
    {
        Task NotifySurveyCreatedAsync(SurveyCreatedEvent surveyCreatedEvent, CancellationToken cancellationToken = default);

        Task NotifySurveyUpdatedAsync(SurveyUpdatedEvent surveyUpdatedEvent, CancellationToken cancellationToken = default);
    }
}
