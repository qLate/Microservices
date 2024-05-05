using Hazelcast;

var options = new HazelcastOptionsBuilder().Build();
options.ClusterName = "hello-world";

var client = await HazelcastClientFactory.StartNewClientAsync(options);
var queue = await client.GetQueueAsync<int>("queue1");

var tasks = new List<Task>
{
    Task.Run(async () =>
    {
        for (var i = 1; i < 101; i++)
        {
            var item = await queue.TakeAsync();
            Console.WriteLine($"Consumer 1: {item}");
        }
    }),
};
await Task.WhenAll(tasks);

Console.Read();