using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest.DAL;
using Nest.Models;
using Nest.ViewModels;
using Newtonsoft.Json;
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

        public IActionResult Index()
        { 
            ShopIndexVM shopIndexVM = new ShopIndexVM();
            //shopAndFilterVM.Products
            shopIndexVM.Products= _context.Products.IncludeOptimized(p => p.ProductImages).IncludeOptimized(p => p.Vendor)
                .IncludeOptimized(p => p.Badge).IncludeOptimized(p=>p.Category).ToList();
            shopIndexVM.Vendors = _context.Vendors.ToList();
            shopIndexVM.Categories = _context.Categories.ToList();    
            return View(shopIndexVM);
        }
        [HttpPost]
        public IActionResult Index(ShopAndFilterVM items)
        {
            if (items is null) return NotFound();
            var products = _context.Products.IncludeOptimized(p => p.ProductImages).IncludeOptimized(p => p.Vendor)
                      .IncludeOptimized(p => p.Badge).IncludeOptimized(p => p.Category);
            if(items.CategoryIds != null && items.VendorIds != null)
            {
                products = products.Where(p => items.CategoryIds.Contains(p.CategoryId) || items.VendorIds.Contains(p.VendorId));
            }
            else
            {
                if (items.CategoryIds != null)
                    products = products.Where(p => items.CategoryIds.Contains(p.CategoryId));
                if (items.VendorIds != null)
                    products = products.Where(p => items.VendorIds.Contains(p.VendorId));

            }
            if(items.MinPrice>=0 && items.MinPrice < items.MaxPrice)
            {
                products = products.Where(p => (p.SellPrice * (100 - p.DiscountPercent) / 100) > items.MinPrice && (p.SellPrice * (100 - p.DiscountPercent) / 100) < items.MaxPrice);
            }
            return PartialView("_ProductsPartialView",products);
        }
        public IActionResult AddBasket(int? id)
        {
            if (id is null) return BadRequest();
            if (!_context.Products.Any(p => p.Id == id)) return NotFound();
            
            List<BasketItem> basketItems = new List<BasketItem>();
            string basketValue = HttpContext.Request.Cookies["basket"];
            if (basketValue != null)
            {
              basketItems= JsonConvert.DeserializeObject<List<BasketItem>>(basketValue);
            }
            BasketItem basketItem = new BasketItem();
            basketItem = basketItems.Find(bi => bi.ProductId == id);
            if(basketItem is null)
            {
                basketItems.Add(new BasketItem { ProductId = (int)id, Count = 1 });
            }
            else
            {   
                basketItem.Count++;
            }
            HttpContext.Response.Cookies.Append("basket",JsonConvert.SerializeObject(basketItems), new CookieOptions { MaxAge=TimeSpan.MaxValue});


            return Json(HttpContext.Request.Cookies["basket"]); 
        }
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

    }
}

