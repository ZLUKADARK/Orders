using Microsoft.EntityFrameworkCore;
using Orders.Data.Models;

namespace Orders.Data
{
    public class OrdersDBContext : DbContext
    {
        public OrdersDBContext(DbContextOptions<OrdersDBContext> options)
            : base(options)
        {
        }
        public DbSet<Provider> Provider { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Order { get; set; }
    }
}
