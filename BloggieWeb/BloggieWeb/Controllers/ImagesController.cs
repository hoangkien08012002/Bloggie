using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloggieWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            // call a reopsitory

        }
    }
}
