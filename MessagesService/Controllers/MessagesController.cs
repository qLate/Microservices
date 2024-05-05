using Microsoft.AspNetCore.Mvc;

namespace MessagesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController(ILogger<MessagesController> logger) : ControllerBase
    {
        // GET: api/<MessagesController>
        [HttpGet]
        public string Get()
        {
            logger.LogInformation("Received GET request");

            return "Messages service is not implemented";
        }
    }
}
