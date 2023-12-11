namespace org.huage.MessageQueue.Client.service;

public interface IMessageQueueManager
{
    Task SendMessageAsync(string msg, string exchange, string queue, string routeKey,
        string exchangeType = "topic", IDictionary<string, object>? arguments = null);

    Task<string> ConsumerMessageAsync(string exchange, string clientQueue
        , string routingKey, IDictionary<string, object>? arguments = null
        , bool isAutoAck = true, string exchangeType = "topic");

    Task ConnectToMq();

    Task DeleteQueue(string queue);

    Task CreateQueue(string queue, string exchange, string exchangeType, string routingKey, bool isDur = true,
        bool isExclusive = false, bool autoDelete = false,
        IDictionary<string, object>? arguments = null);
}