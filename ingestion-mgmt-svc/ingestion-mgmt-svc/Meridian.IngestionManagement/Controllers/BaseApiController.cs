using Microsoft.AspNetCore.Mvc;

namespace Meridian.IngestionManagement.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
