using Nest.Models;

namespace Nest.Areas.Manage
{
    public class CategoryCrViewModel
    {
      public  IEnumerable<Category> categories { get; set; }
        public Category category { get; set; }
    }
}
