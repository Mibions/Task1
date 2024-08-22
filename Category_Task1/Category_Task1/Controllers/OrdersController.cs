using Microsoft.EntityFrameworkCore;
using Category_Task1.Data;
using Category_Task1.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Category_Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderProduct>>> GetOrders()
        {
            // Retrieve all orders from the database, including related employee information
            var orders = await _context.OrderProducts
                .Include(o => o.Employee) // Include the related Employee entity
                .ToListAsync(); // Asynchronously convert the result to a list

            // Return the list of orders with a successful response
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderProduct>> GetOrder(int id)
        {
            // Retrieve a specific order by its ID, including related employee information
            var order = await _context.OrderProducts
                .Include(o => o.Employee) // Include the related Employee entity
                .FirstOrDefaultAsync(o => o.OrderId == id); // Find the order by ID

            if (order == null)
            {
                // Return a NotFound result if the order with the given ID does not exist
                return NotFound("Order not found");
            }

            // Return the order with a successful response
            return Ok(order);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Policy = "AdminPolicy")] // Ensure only Admin can access this
        public async Task<IActionResult> ApproveOrder(int id)
        {
            var order = await _context.OrderProducts.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            // Check if the order is already in progress
            if (order.OrderStatus != "Pending")
            {
                return BadRequest("Order is not in a pending state");
            }

            // Update the order status
            order.OrderStatus = "Progress";
            await _context.SaveChangesAsync();

            return Ok("Order status updated to Progress");
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderProduct orderModel)
        {
            try
            {
                // Check if the Employee exists using the provided EmployeeId
                var employee = await _context.Employees.FindAsync(orderModel.EmployeeId);
                if (employee == null)
                {
                    // Return a bad request response if the EmployeeId is invalid
                    return BadRequest("Invalid EmployeeId.");
                }

                // Create a new OrderProduct instance with the provided details
                var order = new OrderProduct
                {
                    OrderDate = orderModel.OrderDate,
                    OrderPaymentMethod = orderModel.OrderPaymentMethod,
                    OrderShippingAddress = orderModel.OrderShippingAddress,
                    OrderStatus = orderModel.OrderStatus,
                    OrderTotalPrice = orderModel.OrderTotalPrice,
                    EmployeeId = orderModel.EmployeeId,
                    Employee = employee // Associate the order with the employee
                };

                // Add the new order to the database context
                await _context.OrderProducts.AddAsync(order);

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Return a success response with the created order details
                return Ok(order);
            }
            catch (Exception ex)
            {
                // Return a bad request response with the exception message in case of an error
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, OrderProduct orderModel)
        {
            // Retrieve the existing order by its ID
            var order = await _context.OrderProducts.SingleOrDefaultAsync(p => p.OrderId == id);

            // Check if the order exists
            if (order != null)
            {
                // Update the order details with the provided values, excluding OrderStatus
                order.OrderDate = orderModel.OrderDate;
                order.OrderPaymentMethod = orderModel.OrderPaymentMethod;
                order.OrderShippingAddress = orderModel.OrderShippingAddress;
                order.OrderTotalPrice = orderModel.OrderTotalPrice;
                order.EmployeeId = orderModel.EmployeeId;

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Return the updated order details
                return Ok(order);
            }
            else
            {
                // Return a bad request response if the order does not exist
                return BadRequest("Order does not exist!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            // Find the order with the specified ID
            var order = await _context.OrderProducts
                .SingleOrDefaultAsync(p => p.OrderId == id);

            if (order != null)
            {
                // If the order exists, remove it from the context
                _context.OrderProducts.Remove(order);
                await _context.SaveChangesAsync(); // Save changes to the database

                // Return a success message
                return Ok("Order successfully deleted!");
            }

            // If the order does not exist, return a BadRequest response
            return BadRequest("Order not found!");
        }

        //======================================================================================================================================================
    }

}