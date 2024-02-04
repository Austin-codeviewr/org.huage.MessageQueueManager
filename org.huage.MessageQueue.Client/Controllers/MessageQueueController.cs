using Microsoft.AspNetCore.Mvc;
using org.huage.MessageQueue.Client.Aop;
using org.huage.MessageQueue.Client.service;

namespace org.huage.MessageQueue.Client.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class MessageQueueController : Controller
{
    private readonly ILogger<MessageQueueController> _logger;

    private readonly IServiceScopeFactory _factory;
    [AutowiredProperty] public IMessageQueueManager MessageQueueManager { get; set; }

    public MessageQueueController(ILogger<MessageQueueController> logger, IServiceScopeFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    [HttpPost]
    public async Task SendMessage(string msg, string exchange, string queue, string routeKey,
        string exchangeType = "topic")
    {
        try
        {
            //转成执行的值
            await MessageQueueManager.SendMessageAsync(msg, exchange, queue, routeKey);
        }
        catch (Exception _)
        {
            _logger.LogError($"Error in org.huage.Service.MessageQueueManager.MessageQueueController.SendMessage.");
            throw;
        }
    }

    [HttpPost]
    public async Task ConsumerMessage(string exchange, string queue, string routeKey)
    {
        try
        {
            await MessageQueueManager.ConsumerMessageAsync(exchange, queue, routeKey);
        }
        catch (Exception _)
        {
            _logger.LogError($"Error in org.huage.Service.MessageQueueManager.MessageQueueController.ConsumerMessage.");
            throw;
        }
    }

    [HttpPost]
    public async Task CreateQueue(string queue, string exchange, string exchangeType, string routingKey)
    {
        try
        {
            await MessageQueueManager.CreateQueue(queue, exchange, exchangeType, routingKey);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error in org.huage.Service.MessageQueueManager.MessageQueueController.ConsumerMessage.");
            throw;
        }
    }
}