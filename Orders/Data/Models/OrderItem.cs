using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Data.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal Quantity { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Unit { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}
