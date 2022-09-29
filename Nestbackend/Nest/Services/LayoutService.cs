using Microsoft.EntityFrameworkCore;
using Nest.DAL;
using Nest.Models;
using Nest.ViewModels;
using Newtonsoft.Json;

namespace Nest.Services
{
    public class LayoutService
    {

        public LayoutService(NestContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }
         NestContext _context { get; }
         IHttpContextAccessor _accessor { get; }

        public BasketVM GetBasket()
        {
            string cookie = _accessor.HttpContext.Request.Cookies["basket"];
            List < BasketItem > basketItems = new List< BasketItem >();
            if (cookie != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(cookie);

            }
            
            BasketVM basketVM = new BasketVM {Products= new(),TotalPrice=0 };
            foreach (var item in basketItems)
            {
                Product pr = _context.Products.Include(p=>p.ProductImages).Where(p=>p.Id==item.ProductId).FirstOrDefault();
                if (pr != null)
                {
                    basketVM.Products.Add(new ProductBasketItem
                    {
                        product = pr,
                        Count = item.Count
                    });
                basketVM.TotalPrice += item.Count*  pr.SellPrice * (100 - pr.DiscountPercent) / 100;
                }   
            }
            return basketVM;
        }


    }
}
