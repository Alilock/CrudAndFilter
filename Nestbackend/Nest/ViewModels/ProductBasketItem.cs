using Nest.Models;

namespace Nest.ViewModels
{
    public class ProductBasketItem
    {
        public Product product { get; set; }
        public int Count { get; set; }
    }
}
