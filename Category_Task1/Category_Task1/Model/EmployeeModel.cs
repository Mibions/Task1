using Category_Task1.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Category_Task1.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
    }
}