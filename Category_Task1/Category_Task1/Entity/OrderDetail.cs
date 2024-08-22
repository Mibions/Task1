using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Category_Task1.Entity
{
    public class OrderDetail
    {
        [Key]
        [Column(Order = 0)]
        public int OrderId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int ProductId { get; set; }

        [Required]
        public int OrderDetailQuantity { get; set; }

        [Required]
        public decimal OrderDetailPrice { get; set; }

        // Navigation properties
        public OrderProduct? OrderProduct { get; set; }
        public Product? Product { get; set; }
    }
}