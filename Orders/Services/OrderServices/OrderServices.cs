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
            if (OrdernNameExists(orders.Number))
                return null;

            var result = new Order() { Date = orders.Date, Number = orders.Number, ProviderId = orders.ProviderId };
            _context.Order.Add(result);
            await _context.SaveChangesAsync();            
            return orders;
        }

        public async Task<OrderItemViewModel> CreateOrderItemToOrder(OrderItemViewModel orderItem)
        {
            if (OrderItemsNumberExists(orderItem.Name))
                return null;

            var result = new OrderItem() { Name = orderItem.Name, Quantity = orderItem.Quantity, OrderId = orderItem.OrderId, Unit = orderItem.Unit };
            _context.OrderItems.Add(result);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<IEnumerable<OrderTableViewModel>> GetOrders()
        {
            var result = from f in _context.OrderItems.Include(o => o.Order).ThenInclude(p => p.Provider).AsNoTracking()
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

        public async Task<OrderTableViewModel> GetOrder(int id)
        {
            var result = from f in _context.OrderItems.Include(o => o.Order).ThenInclude(p => p.Provider).Where(o => o.Order.Id == id).AsNoTracking()
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
            return await result.FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateOrder(int id, OrderUpdateViewModel orders)
        {
            if (orders.Id != id)
                return false;

            var result = new Order() { Id = orders.Id, Date = orders.Date, ProviderId = orders.ProviderId, Number =orders.Number };

            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                    return false;
                else
                    throw;
            }
        }

        public async Task<bool> UpdateOrderItem(int id, OrderItemUpdateViewModel orderItem)
        {
            if (orderItem.Id != id)
                return false;

            var result = new OrderItem() { Id = orderItem.Id, Name = orderItem.Name, Quantity = orderItem.Quantity, Unit = orderItem.Unit };

            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemsExists(id))
                    return false;
                else
                    throw;
            }
        }

        private bool OrdernNameExists(string name)
        {
            return _context.Order.Any(e => e.Number == name);
        }

        private bool OrderItemsNumberExists(string number)
        {
            return _context.OrderItems.Any(e => e.Name == number);
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }

        private bool OrderItemsExists(int id)
        {
            return _context.OrderItems.Any(e => e.Id == id);
        }
    }
}
