using Consul;
using Microsoft.AspNetCore.Hosting.Server;

namespace FacadeService
{
    public class ConsulHostedService(IServer server) : IHostedService
    {
        public static ConsulClient client = new(config => config.Address = new Uri("http://127.0.0.1:8500"));

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var selfAddress = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? throw new InvalidOperationException();
            RegisterAddressWithConsul(selfAddress);

            return Task.CompletedTask;
        }

        private static async void RegisterAddressWithConsul(string address)
        {
            Console.WriteLine($"Registering {address} with Consul");
            var registration = new AgentServiceRegistration
            {
                ID = Guid.NewGuid().ToString(),
                Name = "messagesService",
                Address = new Uri(address).Host,
                Port = new Uri(address).Port,
            };

            await client.Agent.ServiceRegister(registration);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await client.Agent.ServiceDeregister("messagesService", cancellationToken);
        }
    }
}