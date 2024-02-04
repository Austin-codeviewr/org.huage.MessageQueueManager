using System.Reflection;
using Autofac;

namespace org.huage.MessageQueue.Client.Extension;

public static class SugarExtension
{
    public static void AddDataBase(ContainerBuilder builder)
    {
        var repository = Assembly.Load("org.huage.Repository");
        //根据名称约定（服务层的接口和实现均以Services结尾），实现服务接口和服务实现的依赖
        
        builder.RegisterAssemblyTypes(repository)
            .Where(t => t.Name.EndsWith("Repository")).AsSelf();
    }
}