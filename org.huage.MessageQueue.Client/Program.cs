using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using org.huage.MessageQueue.Client.Extension;
using org.huage.MessageQueue.Entity.Config;

var builder = WebApplication.CreateBuilder(args).Inject();

//设置接口请求超时和上传的限制，因为默认大小为4m，太小了
builder.WebHost.UseKestrel(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(20);
    options.Limits.MaxRequestBodySize = null;

});

/*//1.替换内置的ServiceProviderFactory
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    //注册AutoFac批量注入
    containerBuilder.RegisterModule<AutofacRegisterExtension>();
});
//让controller由容器创建
builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

builder.Services.AddControllers().AddInject();*/
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MessageQueueOption>(builder.Configuration.GetSection("MessageQueue"));
builder.Services.RegisterRateLimit(builder.Configuration);
//builder.Services.ConfigureRedis(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
//启用客户端IP限制速率
app.UseIpRateLimiting();
app.UseInject();
app.MapControllers();

app.Run();