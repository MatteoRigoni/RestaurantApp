using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace webapi.Model
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Total { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        [NotMapped]
        public int[] DeletedOrderDetailsId { get; set; }
    }
}