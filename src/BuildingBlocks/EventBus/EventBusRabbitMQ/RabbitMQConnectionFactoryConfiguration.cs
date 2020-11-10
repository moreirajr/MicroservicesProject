namespace EventBus.RabbitMQ
{
    public class RabbitMQConnectionFactoryConfiguration
    {
        public string EventBusConnection { get; private set; }
        public string EventBusUserName { get; private set; }
        public string EventBusPassword { get; private set; }
        public int EventBusRetryCount { get; private set; }

        public RabbitMQConnectionFactoryConfiguration(string connection, string userName, string password, int retryCount = 5)
        {
            EventBusConnection = connection;
            EventBusUserName = userName;
            EventBusPassword = password;
            EventBusRetryCount = retryCount;
        }
    }
}