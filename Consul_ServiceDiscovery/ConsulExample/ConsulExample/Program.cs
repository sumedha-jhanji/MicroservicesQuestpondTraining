using Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//registering consul as singleton object
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
    var consulHost = builder.Configuration["Consul:Host"];
    var consulPort = Convert.ToInt32(builder.Configuration["Consul:Port"]);
    consulConfig.Address = new Uri($"http://{consulHost}:{consulPort}");

}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


var consulClient = app.Services.GetRequiredService<IConsulClient>();
var registration = new AgentServiceRegistration()
{
    ID = "my-service-id",
    Name = "my-service-name",
    Address = "localhost",
    Port = 7062,
    Check = new AgentServiceCheck // to let know that where health check can be done. health check will let us know whether service is running and up
    {
        HTTP = "https://localhost:7062/Health",
        Interval = TimeSpan.FromSeconds(10),
        Timeout = TimeSpan.FromSeconds(5)
    }
};

await consulClient.Agent.ServiceRegister(registration);

app.Run();
