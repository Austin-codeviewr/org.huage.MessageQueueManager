using IdGen;

namespace org.huage.MessageQueue.Client.Util;

public static class IdUtil
{

    //生成唯一分布式id
    public static long CreateId()
    {
        var idGenerator = new IdGenerator(0);
        return idGenerator.CreateId();
    }
}