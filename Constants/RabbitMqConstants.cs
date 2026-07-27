namespace CaseStudy.Constants
{
    public static class RabbitMqConstants
    {
        public const string ExchangeName = "casestudy.events";
        public const string ExchangeType = "topic";

        public const string SurveyQueueName = "casestudy.survey.events";

        public const string SurveyBindingKey = "survey.*";

        public const string SurveyCreatedRoutingKey = "survey.created";
        public const string SurveyUpdatedRoutingKey = "survey.updated";
    }
}
