using BloggieWeb.Data;
using BloggieWeb.Models.Domain;
using BloggieWeb.Models.ViewModels;
using BloggieWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace BloggieWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _postRepository;

        public AdminBlogPostController(ITagRepository tagRepository, IBlogPostRepository postRepository)
        {
            this._tagRepository = tagRepository;
            this._postRepository = postRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //get tag from repository
            var tag = await _tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tag.Select(x => new SelectListItem
                {
                    Text = x.Displayname,
                    Value = x.Id.ToString()
                }),
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            // map view model to domain model
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FearturedImageUrl = addBlogPostRequest.FearturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,

            };

            // map tag from selected tag
            var selectedtags = new List<Tag>();
            foreach (var selectedtagId in addBlogPostRequest.SelectTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedtagId);
                var existingtag = await _tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existingtag != null)
                {
                    selectedtags.Add(existingtag);
                }
            }

            blogPost.Tags = selectedtags;
            await _postRepository.AddAsync(blogPost);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            // call the repository
            var blogpost = await _postRepository.GetAllAsync();

            return View(blogpost);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // retrieve the resul;t from the ripository
            var blogPost = await _postRepository.GetAsync(id);
            var tagsDomaionModel = await _tagRepository.GetAllAsync();

            if (blogPost != null)
            {
                // map the domain mdel into the view model
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FearturedImageUrl = blogPost.FearturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Tags = tagsDomaionModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }
            

            //pass data to view
           return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // map view model back to domain model
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                FearturedImageUrl = editBlogPostRequest.FearturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                ShortDescription = editBlogPostRequest.ShortDescription,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Visible = editBlogPostRequest.Visible,

            };
            // map tag domain model
            var selectedTags = new List<Tag>();
            foreach(var selectedtag in editBlogPostRequest.SelectTags)
            {
                if(Guid.TryParse(selectedtag,out var tag))
                {
                    var foundtag = await _tagRepository.GetAsync(tag);

                    if(foundtag != null)
                    {
                        selectedTags.Add(foundtag);
                    }
                }
            }

            blogPostDomainModel.Tags = selectedTags;

           var updateBlog = await _postRepository.UpdateAsync(blogPostDomainModel);

            if(updateBlog != null)
            {
                // show success
               
            }
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var deleteBlog = await _postRepository.DeleteAsync(editBlogPostRequest.Id);
            if(deleteBlog != null)
            {
                // show success
                return RedirectToAction("List");
            }
            // show no notification
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }

    }
}
