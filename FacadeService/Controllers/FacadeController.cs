using LoggingService;
using Microsoft.AspNetCore.Mvc;

namespace FacadeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacadeController(ILogger<FacadeController> logger) : ControllerBase
    {
        //private static List<string> loggingServiceUrls = new()
        //{
        //    "https://localhost:7003/api/log",
        //    "https://localhost:7004/api/log",
        //    "https://localhost:7005/api/log"
        //};
        //private static List<string> messagesServiceUrls = new()
        //{
        //    "https://localhost:7006/api/messages",
        //    "https://localhost:7007/api/messages"
        //};


        private HttpClient _client = new();

        // GET api/<FacadeController>
        [HttpGet]
        public async Task<string> Get()
        {
            logger.LogInformation("Received GET request");

            var loggingServiceUrl = await GetLoggingServiceUrl();
            var loggingResponse = await (await _client.GetAsync(loggingServiceUrl)).Content.ReadAsStringAsync();

            var messagesServiceUrl = await GetMessageServerUrl();
            var messagesResponse = await (await _client.GetAsync(messagesServiceUrl)).Content.ReadAsStringAsync();
            return loggingResponse + ": " + messagesResponse;
        }

        // POST api/<FacadeController>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] string value)
        {
            logger.LogInformation("Received POST request, value: {0}", value);

            HazelcastHandler.messageQueue!.PutAsync(value);

            var loggingServiceUrl = GetLoggingServiceUrl().Result;
            return _client.PostAsync(loggingServiceUrl, new StringContent(value)).Result;
        }

        private static async Task<string> GetMessageServerUrl()
        {
            var messagesResult = await ConsulHostedService.client.Catalog.Service("messagesService");
            var messagesServiceUrls = messagesResult.Response.Select(s => $"https://{s.ServiceAddress}:{s.ServicePort}/api/messages").ToList();
            var messagesServiceUrl = messagesServiceUrls[Random.Shared.Next(0, messagesServiceUrls.Count)];
            return messagesServiceUrl;
        }
        private static async Task<string> GetLoggingServiceUrl()
        {
            var loggingResult = await ConsulHostedService.client.Catalog.Service("loggingService");
            var loggingServiceUrls = loggingResult.Response.Select(s => $"https://{s.ServiceAddress}:{s.ServicePort}/api/log").ToList();
            var loggingServiceUrl = loggingServiceUrls[Random.Shared.Next(0, loggingServiceUrls.Count)];
            return loggingServiceUrl;
        }
    }
}


