using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Category_Task1.Entity
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(255)]
        public string CategoryName { get; set; } = string.Empty;

        public string? CategoryDescription { get; set; }

        // Navigation properties
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}