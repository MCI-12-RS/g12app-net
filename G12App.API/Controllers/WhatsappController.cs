using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace G12App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]

    public class WhatsappController : ControllerBase
    {
        
    }
}
