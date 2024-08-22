using Category_Task1.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Category_Task1.Data;

namespace Category_Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, DataContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            // Tạo đối tượng Employee từ dữ liệu đăng ký
            var employee = new Employee
            {
                FullName = model.FullName,
                Position = model.Position,
                Department = model.Department,
            };

            // Lưu thông tin Employee vào cơ sở dữ liệu
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Tạo đối tượng Account từ dữ liệu đăng ký
            var account = new Account
            {
                Username = model.Username,
                Password = model.Password,
                RoleEmployee = model.RoleEmployee,
                EmployeeId = employee.EmployeeId
            };

            // Lưu thông tin Account vào cơ sở dữ liệu
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var account = await _context.Accounts
           .Include(a => a.Employee) // Load thông tin Employee liên quan
           .FirstOrDefaultAsync(a => a.Username == model.Username && a.Password == model.Password);

            if (account == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Tạo JWT token
            var token = GenerateJwtToken(account);

            return Ok(new
            {
                token = token,
                account = new
                {
                    account.AccountId,
                    account.Username,
                    account.RoleEmployee,
                    employee = new
                    {
                        account.Employee.EmployeeId,
                        account.Employee.FullName,
                        account.Employee.Position,
                        account.Employee.Department
                    }
                }
            });
        }

        private string GenerateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, account.Username),
                    new Claim(ClaimTypes.Role, account.RoleEmployee.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"], // Kiểm tra Issuer
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256Signature) // Kiểm tra Key
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [Authorize(Roles = "1")] // Chỉ dành cho Admin (RoleEmployee = 1)
        [HttpGet("admin")]
        public IActionResult AdminAction()
        {
            return Ok("Welcome Admin");
        }

        [Authorize(Roles = "0")] // Chỉ dành cho User (RoleEmployee = 0)
        [HttpGet("user")]
        public IActionResult UserAction()
        {
            return Ok("Welcome User");
        }

    }
}