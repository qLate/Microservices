using Hazelcast.Client;

namespace Hazelcast.Examples.Org.Website.Samples
{
    public class MapSample
    {
        public static void Run(string[] args)
        {
            var options = new HazelcastOptionsBuilder().Build();
            options.ClusterName = "hello-world";

            var client = await HazelcastClientFactory.StartNewClientAsync(options);

            var map = client.GetMap("my-distributed-map");


            map.put("1", "John");
            map.put("2", "Mary");
            map.put("3", "Jane");
        }
    }
}