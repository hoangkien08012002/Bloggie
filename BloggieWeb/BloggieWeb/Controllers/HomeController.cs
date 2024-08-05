using BloggieWeb.Models;
using BloggieWeb.Models.ViewModels;
using BloggieWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BloggieWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ITagRepository _tagrepository;

        public HomeController(ILogger<HomeController> logger,
            IBlogPostRepository blogPostRepository, ITagRepository tagpepository)
        {
            _logger = logger;
            this._blogPostRepository = blogPostRepository;
            this._tagrepository = tagpepository;
        }

        public async Task<IActionResult> Index()
        {
            var blogPost = await _blogPostRepository.GetAllAsync();
            var tags = await _tagrepository.GetAllAsync();
            var model = new HomeViewModel
            {
                Posts = blogPost,
                Tags = tags
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
