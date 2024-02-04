using org.huage.MessageQueue.Client.IOC;
using org.huage.MessageQueue.Entity.Config;

namespace org.huage.MessageQueue.Client.service;

public interface IUseService : IScopeDenpendency
{
    String GetName(QueueBaseOption queueBaseOption);
}