using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.DTO
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
        public string CustomerName { get; set; }
    }

    public class OrderDtoWithDetails
    {
        public HeaderProductDto Header { get; set; }
        
        public IEnumerable<DetailProductDto> Details { get; set; }
    }
    public class HeaderProductDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
        public int CustomerId { get; set; }
    }

    public class DetailProductDto
    {
        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get { return this.Quantity * this.Price; } }
    }
}
