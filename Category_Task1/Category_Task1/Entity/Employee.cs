using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Category_Task1.Entity
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Position { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Department { get; set; } = string.Empty;

        [Required]
        public DateTime HireDate { get; set; }

        // Navigation properties
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<OrderProduct> Orders { get; set; } = new List<OrderProduct>();
    }
}