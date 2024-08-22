using Category_Task1.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Category_Task1.Data;
using Category_Task1.Models;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly DataContext _context;

    public EmployeesController(DataContext context)
    {
        _context = context;
    }

    // GET: api/Employees
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        return await _context.Employees.ToListAsync();
    }

    // GET: api/Employees/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return employee;
    }

    //// POST: api/Employees
    //[HttpPost]
    //public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    // Thêm Employee vào cơ sở dữ liệu
    //    _context.Employees.Add(employee);
    //    await _context.SaveChangesAsync();

    //    // Thêm tài khoản (nếu có)
    //    foreach (var account in employee.Accounts)
    //    {
    //        account.EmployeeId = employee.EmployeeId; // Thiết lập EmployeeId cho Account
    //        _context.Accounts.Add(account);
    //    }
    //    await _context.SaveChangesAsync();

    //    return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
    //}

    // PUT: api/Employees/5

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(int id, Employee employee)
    {
        if (id != employee.EmployeeId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Cập nhật thông tin Employee
        _context.Entry(employee).State = EntityState.Modified;

        // Xóa các tài khoản cũ
        var existingAccounts = _context.Accounts.Where(a => a.EmployeeId == id).ToList();
        _context.Accounts.RemoveRange(existingAccounts);

        // Thêm các tài khoản mới
        foreach (var account in employee.Accounts)
        {
            account.EmployeeId = employee.EmployeeId;
            _context.Accounts.Add(account);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Employees/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EmployeeExists(int id)
    {
        return _context.Employees.Any(e => e.EmployeeId == id);
    }
}