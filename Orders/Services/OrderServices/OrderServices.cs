using Microsoft.EntityFrameworkCore;
using Orders.Data;
using Orders.Data.Models;
using Orders.ViewModels.Order;
using Orders.ViewModels.OrderItem;
using Orders.ViewModels.Orders;
using Orders.ViewModels.Provider;
using System;
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

        public async Task<OrderViewModel> CreateOrder(OrderViewModel orders)
        {
            if (OrderItemsNumberExists(orders.Number))
                return null;

            var result = new Order() { Date = orders.Date, Number = orders.Number, ProviderId = orders.ProviderId };
            _context.Order.Add(result);
            try
            {
                await _context.SaveChangesAsync();   
            }
            catch (DbUpdateException)
            {
                return null;
            }
            orders.Id = result.Id;
            return orders;
        }

        public async Task<OrderItemViewModel> CreateOrderItemToOrder(OrderItemViewModel orderItem)
        {
            if (OrdernNameExists(orderItem.Name))
                return null;

            var result = new OrderItem() { Name = orderItem.Name, Quantity = orderItem.Quantity, OrderId = orderItem.OrderId, Unit = orderItem.Unit };
            _context.OrderItems.Add(result);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrders()
        {
            var result = from f in await _context.Order.Include(p => p.Provider).AsNoTracking().ToListAsync()
                         select new OrderViewModel { Id = f.Id, Date = f.Date, Number = f.Number, ProviderId = f.ProviderId, ProviderName = f.Provider.Name };
            return result;
        }

        public async Task<OrderViewModel> GetOrder(int? id)
        {
            if (id == null)
                return null;

            var result = await _context.Order.Include(p => p.Provider).Where(o => o.Id == id).AsNoTracking().FirstOrDefaultAsync();
                          
            return new OrderViewModel { Id = result.Id, Date = result.Date, Number = result.Number, ProviderId = result.ProviderId, ProviderName = result.Provider.Name }; 
        }
        
        public async Task<OrderDetailWithItemsViewModel> GetOrderWithItems(int? id)
        {
            if (id == null)
                return null;

            var result = await _context.Order.Include(i => i.OrderItem).Include(p => p.Provider).Where(o => o.Id == id).AsNoTracking().FirstOrDefaultAsync();

            return new OrderDetailWithItemsViewModel
            {
                Id = result.Id,
                Date = result.Date,
                Number = result.Number,
                ProviderId = result.ProviderId,
                ProviderName = result.Provider.Name,
                OrderItem = result.OrderItem.Select(i => new OrderItemShorViewModel { Id = i.Id, Name = i.Name, Quantity = i.Quantity, Unit = i.Unit }).ToList()
            };
        }

        public async Task<OrderItemViewModel> GetOrderItem(int id)
        {
            var result = await _context.OrderItems.Include(o => o.Order).Where(o => o.Id == id).AsNoTracking().FirstOrDefaultAsync();

            return new OrderItemViewModel { Id = result.Id, Name = result.Name, OrderId = result.OrderId, Quantity = result.Quantity, Unit = result.Unit, OrderNumber = result.Order.Number };
        }

        public async Task<IEnumerable<ProviderViewModel>> GetProviders()
        {
            var result = await _context.Provider
                .Select(p => new ProviderViewModel { Id = p.Id, Name = p.Name }).AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<IEnumerable<OrderTableViewModel>> GetOrdersTable(OrdersFilterViewModel filter)
        {
            var result = from f in _context.OrderItems.Include(o => o.Order).ThenInclude(p => p.Provider).AsNoTracking()
                         .Where(f => 
                         (filter.Name.Length == 0 ? true : filter.Name[0] == null ? true : filter.Name.Contains(f.Name)) 
                         & (filter.Number.Length == 0 ? true : filter.Number[0] == null ? true : filter.Number.Contains(f.Order.Number))
                         & (filter.ProviderName.Length == 0 ? true : filter.ProviderName[0] == null ? true : filter.ProviderName.Contains(f.Order.Provider.Name))
                         & (filter.Unit.Length == 0 ? true : filter.Unit[0] == null ? true : filter.Unit.Contains(f.Unit))
                         & (filter.DateNow == DateTime.MinValue & filter.DatePast == DateTime.MinValue ? true : f.Order.Date <= filter.DateNow & f.Order.Date >= filter.DatePast))
                         select new OrderTableViewModel 
                         { 
                             OrderItemId = f.Id,
                             OrderId = f.OrderId,
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
        
        public async Task<DistinctValuesForSelect> GetDistinct()
        {
            var number = await _context.Order.Select(o => o.Number).Distinct().AsNoTracking().ToListAsync();
            var name = await _context.Provider.Select(o => o.Name).Distinct().AsNoTracking().ToListAsync();
            var itemName = await _context.OrderItems.Select(o => o.Name).Distinct().AsNoTracking().ToListAsync();
            var itemUnit = await _context.OrderItems.Select(o => o.Unit).Distinct().AsNoTracking().ToListAsync();
            DistinctValuesForSelect distinct = new DistinctValuesForSelect() { Number = number, Name = itemName, ProviderName = name, Unit = itemUnit };
            return distinct;
        }

        public async Task<OrderTableViewModel> GetOrderTable(int id)
        {
            var result = from f in _context.OrderItems.Include(o => o.Order).ThenInclude(p => p.Provider).Where(o => o.Order.Id == id).AsNoTracking()
                         select new OrderTableViewModel
                         {
                             OrderItemId = f.Id,
                             OrderId = f.OrderId,
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

            if (OrderItemsNumberExists(orders.Number))
                return false;

            var result = await _context.Order.FindAsync(id);
            result.Date = orders.Date;
            result.ProviderId = orders.ProviderId;
            result.Number = orders.Number;
                
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
            
            if (OrdernNameExists(orderItem.Name))
                return false;

            var result = await _context.OrderItems.FindAsync(id);
            result.Name = orderItem.Name;
            result.Quantity = orderItem.Quantity;
            result.Unit = orderItem.Unit;
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

        public async Task<bool> DeleteOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
                return false;

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
                return false;

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return true;
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
