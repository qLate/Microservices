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
        private const string MessagesServiceUrl = "https://localhost:7002/api/messages";

        private static string GetLoggingServiceUrl() => loggingServiceUrls[Random.Shared.Next(0, loggingServiceUrls.Count)];

        private HttpClient _client = new();

        // GET api/<FacadeController>
        [HttpGet]
        public string Get()
        {
            logger.LogInformation("Received GET request");

            var loggingServiceUrl = GetLoggingServiceUrl();
            logger.LogInformation("GET: Logging service URL: {0}", loggingServiceUrl);

            var loggingResponse = _client.GetAsync(loggingServiceUrl).Result;
            var messagesResponse = _client.GetAsync(MessagesServiceUrl).Result;
            return loggingResponse.Content.ReadAsStringAsync().Result + ": " + messagesResponse.Content.ReadAsStringAsync().Result;
        }

        // POST api/<FacadeController>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] string value)
        {
            logger.LogInformation("Received POST request, value: {0}", value);

            var loggingServiceUrl = GetLoggingServiceUrl();
            logger.LogInformation("POST: Logging service URL: {0}", loggingServiceUrl);
            return _client.PostAsync(loggingServiceUrl, new StringContent(value)).Result;
        }
    }
}