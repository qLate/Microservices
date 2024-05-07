using LoggingService;
using Microsoft.AspNetCore.Mvc;

namespace FacadeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacadeController(ILogger<FacadeController> logger) : ControllerBase
    {
        private static List<string> loggingServiceUrls = new()
        {
            "https://localhost:7003/api/log",
            "https://localhost:7004/api/log",
            "https://localhost:7005/api/log"
        };
        private static List<string> messagesServiceUrls = new()
        {
            "https://localhost:7006/api/messages",
            "https://localhost:7007/api/messages"
        };

        private static string GetLoggingServiceUrl() => loggingServiceUrls[Random.Shared.Next(0, loggingServiceUrls.Count)];
        private static string GetMessagesServiceUrl() => messagesServiceUrls[Random.Shared.Next(0, messagesServiceUrls.Count)];

        private HttpClient _client = new();

        // GET api/<FacadeController>
        [HttpGet]
        public async Task<string> Get()
        {
            logger.LogInformation("Received GET request");

            var loggingServiceUrl = GetLoggingServiceUrl();
            var loggingResponse = await (await _client.GetAsync(loggingServiceUrl)).Content.ReadAsStringAsync();

            var messagesServiceUrl = GetMessagesServiceUrl();
            var messagesResponse = await (await _client.GetAsync(messagesServiceUrl)).Content.ReadAsStringAsync();
            return loggingResponse + ": " + messagesResponse;
        }

        // POST api/<FacadeController>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] string value)
        {
            logger.LogInformation("Received POST request, value: {0}", value);

            HazelcastHandler.messageQueue!.PutAsync(value);

            var loggingServiceUrl = GetLoggingServiceUrl();
            return _client.PostAsync(loggingServiceUrl, new StringContent(value)).Result;
        }
    }
}