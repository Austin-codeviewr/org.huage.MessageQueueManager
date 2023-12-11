using System.Text;
using System.Timers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using org.huage.MessageQueue.Entity.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Timer = System.Timers.Timer;

namespace org.huage.MessageQueue.Client.service;

public class MessageQueueManager : IMessageQueueManager
{
    /// <summary>
    /// connection to the server.
    /// </summary>
    private IConnection _connection;

    /// <summary>
    /// The rabbit connect options
    /// </summary>
    private readonly RabbitConnectOption _rabbitConnectOptions;

    private readonly ILogger<MessageQueueManager> _logger;
    
    /// <summary>
    /// The channel
    /// </summary>
    private IModel _channel;

    /// <summary>
    /// The batch advance option
    /// </summary>
    private readonly QueueBaseOption _queueBaseOption;

    public MessageQueueManager(IOptions<MessageQueueOption> messageQueueOption, ILogger<MessageQueueManager> logger)
    {
        _logger = logger;
        //拿到配置文件
        _queueBaseOption = messageQueueOption.Value.QueueBase;
        _rabbitConnectOptions = messageQueueOption.Value?.RabbitConnect ??
                                throw new ArgumentNullException(nameof(messageQueueOption));
        ConnectToMq();
    }


    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="exchange"></param>
    /// <param name="queue"></param>
    /// <param name="exchangeType"></param>
    /// <param name="routeKey"></param>
    /// <param name="arguments"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task SendMessageAsync(string msg, string exchange, string queue, string routeKey,
        string exchangeType = "topic",IDictionary<string, object>? arguments = null)
    {
        _channel.ExchangeDeclare(exchange, exchangeType, true);
        _channel.QueueDeclare(queue, true, false, false, arguments);
        _channel.QueueBind(queue, exchange, routeKey, arguments);
        var basicProperties = _channel.CreateBasicProperties();
        //1：非持久化 2：可持久化
        basicProperties.DeliveryMode = 2;
        //var json = JsonConvert.SerializeObject(msg);
        var payload = Encoding.UTF8.GetBytes(msg);
        var address = new PublicationAddress(ExchangeType.Direct, exchange, routeKey);
        _channel.BasicPublish(address, basicProperties, payload);
        return Task.CompletedTask;
    }


    /// <summary>
    /// 删除队列
    /// </summary>
    /// <param name="queue"></param>
    /// <returns></returns>
    public Task DeleteQueue(string queue)
    {
        if (_channel is not null)
        {
            _channel.QueueDelete(queue, false, false);
        }

        return Task.CompletedTask;
    }


    /// <summary>
    /// 消费消息
    /// </summary>
    /// <param name="exchange"></param>
    /// <param name="clientQueue"></param>
    /// <param name="routingKey"></param>
    /// <param name="arguments"></param>
    /// <param name="isAutoAck"></param>
    /// <param name="exchangeType"></param>
    /// <returns></returns>
    public async Task<string> ConsumerMessageAsync(string exchange, string clientQueue
        , string routingKey, IDictionary<string, object>? arguments = null
        , bool isAutoAck = true, string exchangeType = "topic")
    {
        _logger.LogInformation("调用ConsumerMessageAsync");
        string result = "";
        try
        {
            //我们在消费端 从新进行一次 队列和交换机的绑定 ，防止 因为消费端在生产端 之前运行的 问题。
            _channel.ExchangeDeclare(exchange, exchangeType, true);
            _channel.QueueDeclare(clientQueue, true, false, false, null);
            _channel.QueueBind(clientQueue, exchange, routingKey, arguments);

            _logger.LogInformation("开始监听队列：" + clientQueue); // 监听客户端列队
            _channel.BasicQos(0, 1, false); //设置一个消费者在同一时间只处理一个消息，这个rabbitmq 就会将消息公平分发

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                try
                {
                    result = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation($"{clientQueue}[org.huage]获取到消息：{result}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "");
                }
                finally
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(clientQueue, isAutoAck, consumer: consumer);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Consumer Error!");
        }

        return result;
    }

    /// <summary>
    /// create a new queue.
    /// </summary>
    /// <param name="queue"></param>
    /// <param name="exchange"></param>
    /// <param name="exchangeType"></param>
    /// <param name="routingKey"></param>
    /// <param name="isDur"></param>
    /// <param name="isExclusive"></param>
    /// <param name="autoDelete"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public Task CreateQueue(string queue, string exchange, string exchangeType, string routingKey, bool isDur = true,
        bool isExclusive = false, bool autoDelete = false,
        IDictionary<string, object>? arguments = null)
    {
        //declare a exchange
        _channel.ExchangeDeclare(exchange, exchangeType, true);
        //declare a queue.
        var queueDeclareOk = _channel.QueueDeclare(queue, isDur, isExclusive, autoDelete, arguments);

        //bind the queue to a exchange.
        _channel.QueueBind(queueDeclareOk, exchange, routingKey,arguments);

        return Task.CompletedTask;
    }

    /// <summary>
    /// connect to mq. Just init.
    /// </summary>
    /// <returns></returns>
    public Task ConnectToMq()
    {
        try
        {
            //去连接rabbitmq，然后开始监听消息进行消费。
            if (_rabbitConnectOptions == null)
                return Task.CompletedTask;
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitConnectOptions.HostName,
                Port = _rabbitConnectOptions.Port,
                UserName = _rabbitConnectOptions.UserName,
                Password = _rabbitConnectOptions.Password,
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _rabbitConnectOptions.Connection = _connection;
            _rabbitConnectOptions.Channel = _channel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Rabbit连接出现异常");
        }

        return Task.CompletedTask;
    }
    
}