using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Model;
using webapi.DTO;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly RestaurantContext _context;

        public OrdersController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrder()
        {
            var query = _context.Order
                        .Include(o => o.Customer)
                        .Select(o => new OrderDto{
                            OrderId = o.OrderId,
                            OrderNumber = o.OrderNumber,
                            CustomerName = o.Customer.Name,
                            PaymentMethod = o.PaymentMethod,
                            Total = o.Total
                        });

            return await query.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDtoWithDetails>> GetOrder(int id)
        {
            var order = await _context.Order
                        .Include(o => o.OrderDetails)
                        .ThenInclude(o => o.Product)
                        .Select(o => new OrderDtoWithDetails
                        {
                            Header = new HeaderProductDto()
                            {
                                OrderId = o.OrderId,
                                OrderNumber = o.OrderNumber,
                                CustomerId = o.CustomerId,
                                PaymentMethod = o.PaymentMethod,
                                Total = o.Total,
                            },
                            Details = o.OrderDetails.Select(d => new DetailProductDto()
                            {
                                Description = d.Product.Description,
                                OrderDetailId = d.OrderDetailId,
                                OrderId = d.OrderId,
                                Price = d.Product.Price,
                                ProductId = d.ProductId,
                                Quantity = d.Quantity
                            })
                        }).FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Orders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (order.OrderId == 0)
                _context.Order.Add(order);
            else
                _context.Entry(order).State = EntityState.Modified;

            // delete existing order details
            foreach (var orderDetail in order.OrderDetails)
            {
                if (order.DeletedOrderDetailsId != null && order.DeletedOrderDetailsId.Contains(orderDetail.OrderDetailId))
                    _context.Entry(orderDetail).State = EntityState.Deleted;
                else
                {
                    if (orderDetail.OrderDetailId > 0)
                        _context.Entry(orderDetail).State = EntityState.Modified;
                    else
                        _context.Entry(orderDetail).State = EntityState.Added;
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, null);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
