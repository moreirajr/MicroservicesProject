namespace Contas.Infra.CrossCutting.IoC
{
    public class RabbitMQConfiguration
    {
        public string EventBusConnection { get; set; }
        public string SubscriptionClientName { get; set; }
        public string EventBusRetryCount { get; set; }
        public string EventBusUserName { get; set; }
        public string EventBusPassword { get; set; }
    }
}