using RabbitMQ.Client;

namespace org.huage.MessageQueue.Entity.Config;

public class RabbitConnectOption
{
    /// <summary>
    /// Gets or sets the name of the host.
    /// </summary>
    /// <value>
    /// The name of the host.
    /// </value>
    public string HostName { get; set; }

    /// <summary>
    /// Gets or sets the port.
    /// </summary>
    /// <value>
    /// The port.
    /// </value>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>
    /// The name of the user.
    /// </value>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>
    /// The password.
    /// </value>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the virtual host.
    /// </summary>
    /// <value>
    /// The virtual host.
    /// </value>
    public string VirtualHost { get; set; }

    /// <summary>
    /// Gets or sets the connection.
    /// </summary>
    /// <value>
    /// The connection.
    /// </value>
    public IConnection Connection { get; set; }

    /// <summary>
    /// Gets or sets the channel.
    /// </summary>
    /// <value>
    /// The channel.
    /// </value>
    public IModel Channel { get; set; }
}

/// <summary>
/// 队列配置基类
/// </summary>
public class QueueBaseOption
{
    /// <summary>
    /// 交换机名称
    /// </summary>
    /// <value>
    /// The exchange.
    /// </value>
    public string Exchange { get; set; }

    /// <summary>
    /// 队列名称
    /// </summary>
    /// <value>
    /// The queue.
    /// </value>
    public string Queue { get; set; }

    /// <summary>
    /// 交换机类型 direct、fanout、headers、topic 必须小写
    /// </summary>
    /// <value>
    /// The type of the exchange.
    /// </value>
    public string ExchangeType { get; set; }
    /// <summary>
    /// 路由
    /// </summary>
    /// <value>
    /// The route key.
    /// </value>
    //public string RouteKey { get; set; }

    /// <summary>
    /// 客户端队列名称
    /// </summary>
    /// <value>
    /// The client queue
    public string ClientQueue { get; set; }
}

/// <summary>
/// 约定 强类型配置
/// </summary>
public class MessageQueueOption
{
    /// <summary>
    /// rabbit 连接配置
    /// </summary>
    /// <value>
    /// The rabbit connect option.
    /// </value>
    public RabbitConnectOption RabbitConnect { get; set; }

    /// <summary>
    /// 批量推送消息到云信的队列配置信息
    /// </summary>
    /// <value>
    /// 
    /// </value>
    public QueueBaseOption QueueBase { get; set; }
}