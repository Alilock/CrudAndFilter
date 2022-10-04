using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nest.DAL;
using Nest.Models;
using System.Collections;
using Z.EntityFramework.Plus;

namespace Nest.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class CategoryController : Controller
    {

        public CategoryController(NestContext context)
        {
            _context = context;
        }

        public NestContext _context { get; }

        public IActionResult Index()
        {   CategoryCrViewModel categoryCrView = new CategoryCrViewModel();
            categoryCrView.categories = _context.Categories.IncludeOptimized(c => c.Products); 
            return View(categoryCrView);
        }

   
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            if (category.Name == null)
            {
                ModelState.AddModelError("category.Name", "Wntwe name idiot!");
                return RedirectToAction(nameof(Index));
            }
            var ImageFile = category.ImageFile;
            if (ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Choose Image");
                return RedirectToAction(nameof(Index));

            }
            string newFileName = Guid.NewGuid().ToString();
            using (var writer = new FileStream(Path.Combine("wwwroot","assets","imgs", newFileName), FileMode.Create))
            {
                ImageFile.CopyTo(writer);
            };
            category.ImageUrl= ImageFile.Name;

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
