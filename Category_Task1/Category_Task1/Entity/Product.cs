
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Category_Task1.Entity
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProductSize { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ProductColor { get; set; } = string.Empty;

        [Required]
        public decimal ProductPrice { get; set; }

        [Required]
        public int ProductQuantity { get; set; }

        public string? ProductDescription { get; set; }
        public string? ProductImageUrl { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Category Category { get; set; }
        [JsonIgnore]
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}