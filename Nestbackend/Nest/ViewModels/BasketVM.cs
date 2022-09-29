using Nest.Models;

namespace Nest.ViewModels
{
    public class BasketVM
    {
        public List<ProductBasketItem> Products { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
