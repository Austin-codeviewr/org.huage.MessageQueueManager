using Autofac;
using Microsoft.AspNetCore.Mvc;
using org.huage.MessageQueue.Client.Aop;
using org.huage.MessageQueue.Client.IOC;
using org.huage.MessageQueue.Client.service;

namespace org.huage.MessageQueue.Client.Extension;

public class AutofacRegisterExtension: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //获取所有控制器类型并使用属性注入
        Type[] controllersTypeAssembly = typeof(Program).Assembly.GetExportedTypes()
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();
        builder.RegisterTypes(controllersTypeAssembly).PropertiesAutowired(new AutowiredPropertySelector());
        //批量自动注入,把需要注入层的程序集传参数,注入Service层的类
        builder.BatchAutowired(typeof(MessageQueueManager).Assembly);
        builder.BatchAutowired(typeof(UserService).Assembly);
    }
}