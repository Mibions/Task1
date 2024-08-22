using System.ComponentModel.DataAnnotations;

namespace Category_Task1.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string ProductSize { get; set; } = string.Empty;
        public string ProductColor { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImageUrl { get; set; }

    }
}
