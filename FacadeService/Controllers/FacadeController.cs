using Microsoft.AspNetCore.Mvc;

namespace FacadeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacadeController(ILogger<FacadeController> logger) : ControllerBase
    {
        private HttpClient _client = new();

        private const string LoggingServiceUrl = "https://localhost:7001/api/log";
        private const string MessagesServiceUrl = "https://localhost:7002/api/messages";

        // GET api/<FacadeController>
        [HttpGet]
        public string Get()
        {
            var loggingResponse = _client.GetAsync(LoggingServiceUrl).Result;
            var messagesResponse = _client.GetAsync(MessagesServiceUrl).Result;
            return loggingResponse.Content.ReadAsStringAsync().Result + ": " + messagesResponse.Content.ReadAsStringAsync().Result;
        }

        // POST api/<FacadeController>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] string value)
        {
            return _client.PostAsync(LoggingServiceUrl, new StringContent(value)).Result;
        }
    }
}