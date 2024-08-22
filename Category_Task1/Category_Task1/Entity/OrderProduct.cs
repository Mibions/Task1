using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Category_Task1.Entity
{
    public class OrderProduct
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(255)]
        public string OrderStatus { get; set; } = "Pending";

        [Required]
        public decimal OrderTotalPrice { get; set; } = 0;

        [Required]
        [MaxLength(255)]
        public string OrderPaymentMethod { get; set; } = string.Empty;

        [Required]
        public string OrderShippingAddress { get; set; } = string.Empty;

        [Required]  
        public int EmployeeId { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Employee? Employee { get; set; }
    }
}