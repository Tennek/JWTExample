using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTSandbox.Audience.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public ActionResult<string> HelloAuthenticatedWorld()
        {
            var usr = User;
            return "Hello World";
        }
    }
}
