using BloggieWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BloggieWeb.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blog = await blogPostRepository.GetBuUrlHandleAsync(urlHandle);

            return View(blog);
        }
    }
}
