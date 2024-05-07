using System.Text;
using FacadeService;
using Hazelcast;
using Hazelcast.DistributedObjects;

namespace LoggingService
{
    public static class HazelcastHandler
    {
        public static IHazelcastClient? client;
        public static IHQueue<string>? messageQueue;

        public static async Task Initialize()
        {
            var options = new HazelcastOptionsBuilder().Build();
            options.ClusterName = Encoding.UTF8.GetString(ConsulHostedService.client.KV.Get("clusterName").Result.Response.Value);

            client = await HazelcastClientFactory.StartNewClientAsync(options);
            var queueName = Encoding.UTF8.GetString(ConsulHostedService.client.KV.Get("messagesQueue").Result.Response.Value);
            messageQueue = await client.GetQueueAsync<string>(queueName);
        }
    }
}