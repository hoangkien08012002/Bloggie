using Microsoft.AspNetCore.Mvc;

namespace BloggieWeb.Controllers
{
    public class AdminTagsController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}
