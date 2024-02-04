using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using org.huage.MessageQueue.Entity.Config;

namespace org.huage.MessageQueue.Client.service;

public class UserService : IUseService,IDynamicApiController 
{
    public string GetName([FromQuery] QueueBaseOption queueBaseOption)
    {
        Console.WriteLine(queueBaseOption.Queue);
        Console.WriteLine("Austin");
        return "Ok";
    }
}