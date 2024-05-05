using Microsoft.AspNetCore.Mvc;

namespace LoggingService.Controllers
{
    [Route("api/log")]
    [ApiController]
    public class LoggingController(ILogger<LoggingController> logger) : ControllerBase
    {
        // GET: api/<LoggingController>
        [HttpGet]
        public async Task<IReadOnlyCollection<string>> Get()
        {
            logger.LogInformation("Received GET request");

            return await HazelcastHandler.messages!.GetValuesAsync()!;
        }

        // POST api/<LoggingController>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using var reader = new StreamReader(Request.Body);
            var plainText = await reader.ReadToEndAsync();

            logger.LogInformation("Received POST request, value: {0}", plainText);

            await HazelcastHandler.messages!.SetAsync(Guid.NewGuid().ToString(), plainText);
            return Ok();
        }
    }
}