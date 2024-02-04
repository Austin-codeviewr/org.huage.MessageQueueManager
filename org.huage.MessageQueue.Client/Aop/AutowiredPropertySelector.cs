using System.Reflection;
using Autofac.Core;

namespace org.huage.MessageQueue.Client.Aop;
/**
 * 属性注入流程：
 * 1.设定注解标识需要注入的属性，因为controller的属性可能太多，所以使用注解的方式脱离全局影响；
 * 2.AutowiredPropertySelector 查找标识了注解的
 */
public class AutowiredPropertySelector: IPropertySelector
{
    public bool InjectProperty(PropertyInfo propertyInfo, object instance)
    {
        //判断属性是否有标识自定义的属性注入，有则返回true.
        return propertyInfo.CustomAttributes.Any(s => s.AttributeType == typeof(AutowiredPropertyAttribute));
    }
}