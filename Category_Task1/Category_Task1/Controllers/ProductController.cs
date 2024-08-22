using Category_Task1.Data;
using Category_Task1.Entity;
using Category_Task1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Category_Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            return Ok(product);
        }

        //[HttpPost]
        //public async Task<ActionResult<Product>> CreateProduct(Product product)
        //{
        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        //}

        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductModel productModel)
        {
            try
            {
                var existingProduct = await _context.Products
                    .SingleOrDefaultAsync(p => p.ProductName == productModel.ProductName);

                if (existingProduct != null)
                {
                    return BadRequest("Đã tồn tại sản phẩm có cùng tên!");
                }

                var product = new Product
                {
                    ProductName = productModel.ProductName,
                    ProductSize = productModel.ProductSize,
                    ProductPrice = productModel.ProductPrice,
                    ProductQuantity = productModel.ProductQuantity,
                    ProductColor = productModel.ProductColor,
                    ProductDescription = productModel.ProductDescription,
                    ProductImageUrl = productModel.ProductImageUrl,
                    ProductId = productModel.ProductId,
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateProduct(int id, Product product)
        //{
        //    if (id != product.ProductId)
        //    {
        //        return BadRequest("Product ID mismatch");
        //    }

        //    _context.Entry(product).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductExists(id))
        //        {
        //            return NotFound("Product not found");
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductModel productModel)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return BadRequest("Sản phẩm không tồn tại!");
            }

            var existingProduct = await _context.Products
                .Where(p => p.ProductName == productModel.ProductName && p.ProductId != id)
                .SingleOrDefaultAsync();

            if (existingProduct != null)
            {
                return BadRequest("Đã tồn tại sản phẩm có cùng tên!");
            }

            product.ProductName = productModel.ProductName;
            product.ProductSize = productModel.ProductSize;
            product.ProductPrice = productModel.ProductPrice;
            product.ProductQuantity = productModel.ProductQuantity;
            product.ProductColor = productModel.ProductColor;
            product.ProductImageUrl = productModel.ProductImageUrl;
            product.ProductDescription = productModel.ProductDescription;
            product.ProductId = productModel.ProductId;

            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Ok("Xóa sản phẩm thành công!");
            }
            return BadRequest("Sản phẩm không tồn tại!");
        }
    }
}
