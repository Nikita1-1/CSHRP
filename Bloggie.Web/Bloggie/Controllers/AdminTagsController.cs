using Blog.Web.Data;
using Bloggie.Models.Domain;
using Bloggie.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

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
        public IActionResult List()
        {
            //use dbContext to read the tags
            var tags = blogDBContext.Tags.ToList();

            return View(tags);
        }
    }
}
