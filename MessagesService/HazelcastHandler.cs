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
            options.ClusterName = "hello-world";

            client = await HazelcastClientFactory.StartNewClientAsync(options);
            messageQueue = await client.GetQueueAsync<string>("messagesQueue");
        }
    }
}
