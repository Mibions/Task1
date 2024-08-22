using Category_Task1.Model;
using System.ComponentModel.DataAnnotations;

namespace Category_Task1.Models
{
    public class OrderDetailModel
    {
        public int ProductId { get; set; }
        public int OrderDetailQuantity { get; set; }
        public decimal OrderDetailPrice { get; set; }
    }
}