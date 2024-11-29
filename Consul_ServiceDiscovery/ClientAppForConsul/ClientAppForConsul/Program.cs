// See https://aka.ms/new-console-template for more information
using Consul;

Console.WriteLine("Hello, World!");

var consulClient = new ConsulClient();

//specify the service name to discovery
string serviceName = "my-service-name";

//Query consul for healthy instance of the services
var services = consulClient.Health.Service(serviceName, tag: null, passingOnly: true).Result.Response;

//Iterate through the discovered services.
foreach (var service in services)
{
    var serviceAddress = service.Service.Address;
    var servicePort = service.Service.Port;

    Console.WriteLine($"Found service at {serviceAddress}:{servicePort}");
    // we can use the serviceaddress and port to communicate with discovered service.
}

Console.ReadLine();
