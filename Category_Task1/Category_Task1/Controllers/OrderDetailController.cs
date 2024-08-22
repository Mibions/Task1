using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Category_Task1.Data;
using Category_Task1.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Category_Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderDetailController(DataContext context)
        {
            _context = context;
        }

        // HTTP GET method to retrieve all order details
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
        {
            // Retrieve a list of order details including related product and order information
            var orderDetail = await _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.OrderProduct) 
                .ThenInclude(o => o.Employee)
                .ToListAsync(); // Retrieve all order details asynchronously

            // Check if no order details are found
            if (orderDetail == null || !orderDetail.Any())
            {
                return NotFound("Order detail not found");
            }

            // Return the list of order details
            return orderDetail;
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int orderId)
        {
            // Retrieve a single order detail including related product and order information
            var orderDetail = await _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.OrderProduct)
                .ThenInclude(o => o.Employee)
                .FirstOrDefaultAsync(od => od.OrderId == orderId); // Retrieve the order detail with the specified orderId asynchronously

            // Check if the order detail is not found
            if (orderDetail == null)
            {
                return NotFound("Order detail not found");
            }

            // Return the found order detail
            return orderDetail;
        }

        //======================================================================================================================================================
        //======================================================================================================================================================
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> CreateOrderDetail(OrderDetail orderDetail)
        {
            // Find the order based on the OrderId provided in the orderDetail
            var order = await _context.OrderProducts
                .FirstOrDefaultAsync(o => o.OrderId == orderDetail.OrderId);

            // Check if the order does not exist
            if (order == null)
            {
                // Return a 404 Not Found response if the order is not found
                return NotFound("Order not found");
            }

            // Check the status of the order
            if (order.OrderStatus != "Progress")
            {
                // Return a 400 Bad Request response if the order status is not 'Progress'
                return BadRequest("Order status must be 'Progress' to add order details");
            }

            // Add the order detail to the database
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            // Return a 201 Created response with the created order detail
            // Include the URI to retrieve the details of the created order
            return CreatedAtAction(nameof(GetOrderDetail), new { orderId = orderDetail.OrderId, productId = orderDetail.ProductId }, orderDetail);
        }

        [HttpPut("{orderId}/{productId}")]
        public async Task<ActionResult<OrderDetail>> UpdateOrderDetail(int orderId, int productId, OrderDetail updatedOrderDetail)
        {
            // Find the existing order detail based on OrderId and ProductId
            var existingOrderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            // Check if the order detail exists
            if (existingOrderDetail == null)
            {
                // Return 404 Not Found if the order detail does not exist
                return NotFound("Order detail not found");
            }

            // Update the existing order detail with the provided values
            existingOrderDetail.OrderDetailQuantity = updatedOrderDetail.OrderDetailQuantity;
            existingOrderDetail.OrderDetailPrice = updatedOrderDetail.OrderDetailPrice;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Return 200 OK with the updated order detail
            return Ok(existingOrderDetail);
        }

        [HttpDelete("{orderId}/{productId}")]
        public async Task<ActionResult> DeleteOrderDetail(int orderId, int productId)
        {
            // Find the existing order detail based on OrderId and ProductId
            var orderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            // Check if the order detail exists
            if (orderDetail == null)
            {
                // Return 404 Not Found if the order detail does not exist
                return NotFound("Order detail not found");
            }

            // Remove the order detail from the database
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            // Return 204 No Content to indicate successful deletion
            return NoContent();
        }

        private bool OrderDetailExists(int orderId, int productId)
        {
            return _context.OrderDetails.Any(od => od.OrderId == orderId && od.ProductId == productId);
        }
    }
}
