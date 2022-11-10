using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Data.Models
{
    public class Provider
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Name { get; set; }
        public List<Order> Orders { get; set; }
    }
}
