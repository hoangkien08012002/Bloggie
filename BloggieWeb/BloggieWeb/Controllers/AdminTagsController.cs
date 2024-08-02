using BloggieWeb.Data;
using BloggieWeb.Models.Domain;
using BloggieWeb.Models.ViewModels;
using BloggieWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloggieWeb.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this._tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            //mapping Addtagrequest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                Displayname = addTagRequest.DisplayName
            };
            await _tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            var tags = await _tagRepository.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Displayname = tag.Displayname,
                };
                return View(editTagRequest);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                Displayname = editTagRequest.Displayname,
            };
            var updateTag = await _tagRepository.UpdateAsync(tag);
            if(updateTag != null)
            {
                //show success notification
            }
            else
            {
                //show error notification
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });

        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deleteTag = await _tagRepository.DeleteAsync(editTagRequest.Id);
            if(deleteTag != null)
            {
                //show success notification
            }
            else
            {
                //show error notification
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}
