namespace CaseStudy.Exceptions
{
    // Pasif bir ankete ait soruya cevap eklenmeye çalışıldığında fırlatılır.
    public class SurveyInactiveException : Exception
    {
        public SurveyInactiveException(string message) : base(message)
        {
        }
    }
}
