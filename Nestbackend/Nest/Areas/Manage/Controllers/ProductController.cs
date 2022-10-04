using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.DAL;
using Nest.Models;

namespace Nest.Areas.Manage.Controllers
{
    public class ProductController : Controller
    {

        public ProductController(NestContext context)
        {
            _context = context;
        }

        public NestContext _context { get; }
        [Area("Manage")]

        public IActionResult Index()
        {
            ICollection<Product> products = _context.Products.Include(p=>p.ProductImages).ToList();
            return View(products);
        }

    }
}
