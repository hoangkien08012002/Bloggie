using BloggieWeb.Data;
using BloggieWeb.Models.Domain;
using BloggieWeb.Models.ViewModels;
using BloggieWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloggieWeb.Controllers
{
    [Authorize(Roles = "Admin")]
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
            validateAddtagRequest(addTagRequest);
            if (ModelState.IsValid == false)
            {
                return View();
            }
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
        public async Task<IActionResult> List(string? searchQuery,
            string? sortBy, 
            string? sortDirection,
            int pageSize =3, 
            int pageNumber=1)
        {
            var totalRecords = await _tagRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if(pageNumber> totalPages)
            {
                pageNumber--;
            }
            if (pageNumber < 1)
            {
                pageNumber++;
            }

            ViewBag.ToTalPages = totalPages;

            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            //use dbContext to read tag
            var tags = await _tagRepository.GetAllAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);


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
            if (updateTag != null)
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
            if (deleteTag != null)
            {
                //show success notification
            }
            else
            {
                //show error notification
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        private void validateAddtagRequest(AddTagRequest request)
        {
            if (request.Name is not null && request.DisplayName is not null)
            {
                if (request.Name == request.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Name cannot be the same as Displayname");
                }
            }
        }
    }
}
