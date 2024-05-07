using LoggingService;
using Microsoft.AspNetCore.Mvc;

namespace MessagesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController(ILogger<MessagesController> logger) : ControllerBase
    {
        private static List<string> messages = new();

        // GET: api/<MessagesController>
        [HttpGet]
        public List<string> Get()
        {
            logger.LogInformation("Received GET request");
            return messages;
        }

        public static async void Loop()
        {
            while (true)
            {
                while (await HazelcastHandler.messageQueue!.PeekAsync() == null) { }

                var value = await HazelcastHandler.messageQueue!.TakeAsync();
                messages.Add(value);
            }
        }
    }
}