using Microsoft.EntityFrameworkCore;
using Orders.Data;
using Orders.Data.Models;
using Orders.ViewModels.OrderItem;
using Orders.ViewModels.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Services.OrderServices
{
    public class OrderServices : IOrderServices
    {
        private readonly OrdersDBContext _context;
        public OrderServices(OrdersDBContext context)
        {
            _context = context;
        }

        public async Task<OrderItemViewModel> AddOrderItemToOrder(OrderItemViewModel orderItem)
        {
            throw new System.NotImplementedException();
        }

        public async Task<OrderViewModel> CreateOrder(OrderViewModel orders)
        {
            var result = new Order() { Date = orders.Date, Number = orders.Number, ProviderId = orders.ProviderId };
            _context.Order.Add(result);
            await _context.SaveChangesAsync();
            return orders;
        }

        public async Task<OrderItemViewModel> CreateOrderItemToOrder(OrderItemViewModel orderItem)
        {
            var result = new OrderItem() { Name = orderItem.Name, Quantity = orderItem.Quantity, OrderId = orderItem.OrderId, Unit = orderItem.Unit };
            _context.OrderItems.Add(result);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<IEnumerable<OrderTableViewModel>> GetOrders()
        {
            var result = from f in _context.OrderItems.Include(o => o.Order).ThenInclude(p => p.Provider)
                         select new OrderTableViewModel 
                         { 
                             Name = f.Name, 
                             Quantity = f.Quantity, 
                             Unit = f.Unit, 
                             Number = f.Order.Number, 
                             Date = f.Order.Date, 
                             ProviderId = f.Order.Provider.Id, 
                             ProviderName = f.Order.Provider.Name 
                         };
            return await result.ToListAsync();
        }

        public async Task<OrderTableViewModel> GetOrders(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateOrder(OrderUpdateViewModel orders)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateOrderItem(OrderItemUpdateViewModel orderItem)
        {
            throw new System.NotImplementedException();
        }
    }
}
