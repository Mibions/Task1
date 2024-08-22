using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Category_Task1.Entity
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        public int? RoleEmployee { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        // Navigation properties
        public Employee Employee { get; set; }
    }
}