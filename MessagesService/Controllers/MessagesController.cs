using Microsoft.AspNetCore.Mvc;

namespace MessagesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public string Get()
        {
            return "Messages service is not implemented";
        }
    }
}
