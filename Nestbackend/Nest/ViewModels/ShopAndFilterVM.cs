using Nest.Models;

namespace Nest.ViewModels
{
    public class ShopAndFilterVM
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories{ get; set; }
        public List<Vendor> Vendors { get; set; }
        public int MaxPrice { get; set; }
        public int MinPrice { get; set; }


    }
}
