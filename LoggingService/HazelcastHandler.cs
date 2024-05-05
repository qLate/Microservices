using Hazelcast;
using Hazelcast.DistributedObjects;

namespace LoggingService
{
    public static class HazelcastHandler
    {
        public static IHazelcastClient? client;
        public static IHMap<string, string>? messages;

        public static async void Initialize()
        {
            var options = new HazelcastOptionsBuilder().Build();
            options.ClusterName = "hello-world";

            client = await HazelcastClientFactory.StartNewClientAsync(options);
            messages = await client.GetMapAsync<string, string>("loggerMessages");
        }
    }
}
