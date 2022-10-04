using System.ComponentModel.DataAnnotations.Schema;

namespace Nest.Models
{
    public class Category:BaseEntity
    {
        
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Product> Products { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
