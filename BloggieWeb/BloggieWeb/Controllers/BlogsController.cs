using BloggieWeb.Models.Domain;
using BloggieWeb.Models.ViewModels;
using BloggieWeb.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BloggieWeb.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostComment blogPostComment;

        public BlogsController(IBlogPostRepository blogPostRepository,
            IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostComment blogPostComment)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostComment = blogPostComment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var blog = await blogPostRepository.GetBuUrlHandleAsync(urlHandle);
            var blogDetailViewModel = new BlogDetailsViewModel();
            if (blog != null)
            {

                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blog.Id);

                if (signInManager.IsSignedIn(User))
                {
                    //Get like for blog for this user
                    var likeForBlog = await blogPostLikeRepository.GetLikeBlogForuser(blog.Id);

                    var userId = userManager.GetUserId(User);

                    if (userId != null)
                    {
                        var likeFromUser = likeForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
                }

                // Get comment for blog post
                var blogComment = await blogPostComment.GetCommentByBlogByIdAsync(blog.Id);

                var blogCommentsForView = new List<BlogComment>();

                foreach(var blogcomment in blogComment)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogcomment.Description,
                        DateAdded = blogcomment.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogcomment.Userid.ToString())).UserName
                    });
                }

                blogDetailViewModel = new BlogDetailsViewModel
                {
                    Id = blog.Id,
                    Content = blog.Content,
                    PageTitle = blog.PageTitle,
                    Author = blog.Author,
                    FearturedImageUrl = blog.FearturedImageUrl,
                    Heading = blog.Heading,
                    PublishedDate = blog.PublishedDate,
                    ShortDescription = blog.ShortDescription,
                    UrlHandle = blog.UrlHandle,
                    Visible = blog.Visible,
                    Tags = blog.Tags,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = blogCommentsForView
                };
            }

            return View(blogDetailViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domein = new BlogPostComment
                {
                    Blogpostid = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    Userid = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now,
                };
               await blogPostComment.AddAsync(domein);

                return RedirectToAction("Index","Blogs",new {urlhandle = blogDetailsViewModel.UrlHandle});
            }
            return View();

           
        }
    }
}
