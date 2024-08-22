using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Category_Task1.Model
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryDescription { get; set; }
    }
}
