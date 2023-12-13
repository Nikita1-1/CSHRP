using Blog.Web.Data;
using Bloggie.Models.Domain;
using Bloggie.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;

namespace Bloggie.Controllers
{
    public class AdminTagsController : Controller
    {
        private BlogDBContext blogDBContext;
        public AdminTagsController(BlogDBContext blogDBContext)
        {
           this.blogDBContext = blogDBContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        //collectiong data from form
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            // mapping addTagrequest to tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            blogDBContext.Tags.Add(tag);
            blogDBContext.SaveChanges();

           
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            //use dbContext to read the tags
            var tags = await blogDBContext.Tags.ToListAsync();
            
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await blogDBContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            

            if (tag != null) 
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            return View(null);
        }

        [HttpPost]
        
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag { 
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            var existingTag = await blogDBContext.Tags.FindAsync(tag.Id);
            if(existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName; 

                await blogDBContext.SaveChangesAsync();
                return RedirectToAction("List", new { id = editTagRequest.Id });
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete (EditTagRequest editTagRequest)
        {
            var tag = await blogDBContext.Tags.FindAsync(editTagRequest.Id);
             if(tag != null)
            {
                blogDBContext.Tags.Remove(tag);
                await blogDBContext.SaveChangesAsync();
            return RedirectToAction("List");
            }
        return RedirectToAction("Edit", new {id=editTagRequest.Id});
        }
    }
}
