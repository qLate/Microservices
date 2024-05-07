using FacadeService;
using Hazelcast;
using Hazelcast.DistributedObjects;
using System.Text;

namespace LoggingService
{
    public static class HazelcastHandler
    {
        public static IHazelcastClient? client;
        public static IHMap<string, string>? messages;

        public static async Task Initialize()
        {
            var options = new HazelcastOptionsBuilder().Build();
            options.ClusterName = Encoding.UTF8.GetString(ConsulHostedService.client.KV.Get("clusterName").Result.Response.Value);

            client = await HazelcastClientFactory.StartNewClientAsync(options);
            var mapName = Encoding.UTF8.GetString(ConsulHostedService.client.KV.Get("loggerMessagesMap").Result.Response.Value);
            messages = await client.GetMapAsync<string, string>(mapName);
        }
    }
}