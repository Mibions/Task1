using System;
using System.ComponentModel.DataAnnotations;
using Category_Task1.Model;
using Category_Task1.Models;
namespace Category_Task1.Model
{
    public class OrderProductModel
    {
        public string OrderPaymentMethod { get; set; }
        public string OrderShippingAddress { get; set; }
        public int EmployeeId { get; set; }
        public List<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();


    }
}