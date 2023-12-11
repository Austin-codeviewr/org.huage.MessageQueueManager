using Microsoft.AspNetCore.Mvc;
using org.huage.MessageQueue.Client.service;
using RabbitMQ.Client;

namespace org.huage.MessageQueue.Client.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class MessageQueueController
{
    private readonly ILogger<MessageQueueController> _logger;

    private readonly IMessageQueueManager _messageQueueManager;

    public MessageQueueController(ILogger<MessageQueueController> logger, IMessageQueueManager messageQueueManager)
    {
        _logger = logger;
        _messageQueueManager = messageQueueManager;
    }

    [HttpPost]
    public async Task SendMessage(string msg, string exchange, string queue, string routeKey,
        string exchangeType = "topic")
    {
        try
        {
            //转成执行的值
            await _messageQueueManager.SendMessageAsync(msg, exchange, queue, routeKey);
        }
        catch (Exception e)
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
            await _messageQueueManager.ConsumerMessageAsync(exchange, queue, routeKey);
        }
        catch (Exception e)
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
            await _messageQueueManager.CreateQueue(queue, exchange, exchangeType, routingKey);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error in org.huage.Service.MessageQueueManager.MessageQueueController.ConsumerMessage.");
            throw;
        }
    }
}