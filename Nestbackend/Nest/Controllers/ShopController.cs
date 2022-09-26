using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.DAL;
using Nest.Models;
using Nest.ViewModels;
using Z.EntityFramework.Plus;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nest.Controllers
{
    public class ShopController : Controller
    {
        private readonly NestContext _context;
        public ShopController(NestContext context)
        {
            _context = context;
        }

        public IActionResult Index(ShopAndFilterVM? sp)
        { ShopAndFilterVM shopAndFilterVM = new ShopAndFilterVM();
            //shopAndFilterVM.Products
            shopAndFilterVM.Products= _context.Products.IncludeOptimized(p => p.ProductImages).IncludeOptimized(p => p.Vendor)
                .IncludeOptimized(p => p.Badge).IncludeOptimized(p=>p.Category).ToList();
            shopAndFilterVM.Vendors = _context.Vendors.ToList();
            shopAndFilterVM.Categories = _context.Categories.ToList();    
            return View(shopAndFilterVM);
        }
        //public IActionResult Filter()
        //{ ShopAndFilterVM shopAndFilterVM = new ShopAndFilterVM();
            
        //        shopAndFilterVM.Products = _context.Products.IncludeOptimized(p => p.ProductImages).IncludeOptimized(p => p.Vendor)
        //   .IncludeOptimized(p => p.Badge).IncludeOptimized(p => p.Category).ToList();
        //    shopAndFilterVM.Vendors = _context.Vendors.ToList();
        //    shopAndFilterVM.Categories = _context.Categories.ToList();
        //    return PartialView(shopAndFilterVM);
        //}
        public IActionResult Product()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Compare()
        {
            return View();
        }
        public IActionResult Filter(Dictionary<string, int> a)
        {
            ShopAndFilterVM shopAndFilterVM = new ShopAndFilterVM();
            foreach (var item in a)
            {
                if (item.Key=="category")
                {
                    shopAndFilterVM.Products = _context.Products.Where(p => p.CategoryId == item.Value).IncludeOptimized(p => p.ProductImages).IncludeOptimized(p => p.Vendor)
                    .IncludeOptimized(p => p.Badge).IncludeOptimized(p => p.Category).ToList();
                }
                else
                {
                    shopAndFilterVM.Products = _context.Products.Where(p => p.VendorId == item.Value).IncludeOptimized(p => p.ProductImages).IncludeOptimized(p => p.Vendor)
                      .IncludeOptimized(p => p.Badge).IncludeOptimized(p => p.Category).ToList();
                }
           
            }
            shopAndFilterVM.Vendors = _context.Vendors.ToList();
            shopAndFilterVM.Categories = _context.Categories.ToList();
            return View("~/Views/Shop/Index.cshtml", shopAndFilterVM);
        }
    }
}

