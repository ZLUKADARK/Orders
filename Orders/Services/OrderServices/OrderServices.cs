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
            var result = from f in await _context.Order.AsNoTracking().ToListAsync()
                         select new OrderViewModel { Id = f.Id, Date = f.Date, Number = f.Number, ProviderId = f.ProviderId };
            return result;
        }

        public async Task<OrderViewModel> GetOrder(int? id)
        {
            if (id == null)
                return null;
            var result = await _context.Order.FindAsync(id);

            return new OrderViewModel
            {
                Id = result.Id,
                Date = result.Date,
                Number = result.Number,
                ProviderId = result.ProviderId };
        }

        public async Task<OrderItemViewModel> GetOrderItem(int id)
        {
            var result = await _context.OrderItems.FindAsync(id);

            return new OrderItemViewModel
            {
                Id = result.Id,
                Name = result.Name,
                OrderId = result.OrderId,
                Quantity = result.Quantity,
                Unit = result.Unit
            };
        }

        public async Task<IEnumerable<ProviderViewModel>> GetProviders()
        {
            var result = await _context.Provider
                .Select(p => new ProviderViewModel { Id = p.Id, Name = p.Name }).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<OrderTableViewModel>> GetOrdersTable(OrdersFilterViewModel filter)
        {
            var orders = from f in _context.OrderItems.Include(o => o.Order).ThenInclude(p => p.Provider).AsNoTracking()
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

            return await Filter(orders, filter);
        }

        public async Task<OrderTableViewModel> GetOrderTable(int id)
        {
            var result = from f in _context.OrderItems.Include(o => o.Order).ThenInclude(p => p.Provider).Where(o => o.Id == id).AsNoTracking()
                         select new OrderTableViewModel()
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

            var result = new Order() { Id = orders.Id, Date = orders.Date, ProviderId = orders.ProviderId, Number = orders.Number };

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

        private async Task<IEnumerable<OrderTableViewModel>> Filter(IQueryable<OrderTableViewModel> orders, OrdersFilterViewModel filter)
        {
            if (filter.Name == null && filter.Number == null && filter.ProviderName == null && filter.Unit == null && filter.DateNow == DateTime.MinValue && filter.DatePast == DateTime.MinValue)
                return await orders.ToListAsync();

            List<OrderTableViewModel> result = new List<OrderTableViewModel>();
            
            if (filter.DateNow != DateTime.MinValue && filter.DatePast != DateTime.MinValue)
            {
                foreach (var f in await FilterByDate(orders, filter.DatePast, filter.DateNow))
                {
                    if (result.Contains(f) != true)
                    {
                        result.Add(f);
                    }
                }
            }
            if (filter.Name != null)
            {
                foreach (var f in await FilterByName(orders, filter.Name))
                {
                    if(result.Contains(f) != true)
                    {
                        result.Add(f);
                    }
                }       
            }
            if (filter.Number != null)
            {
                foreach (var f in await FilterByNumber(orders, filter.Number))
                {
                    if (result.Contains(f) != true)
                    {
                        result.Add(f);
                    }
                }
            }
            if (filter.ProviderName != null)
            {
                foreach (var f in await FilterByProvider(orders, filter.ProviderName))
                {
                    if (result.Contains(f) != true)
                    {
                        result.Add(f);
                    }
                }
            }
            if (filter.Unit != null)
            {
                foreach (var f in await FilterByUnit(orders, filter.Unit))
                {
                    if (!result.Contains(f))
                        result.Add(f);
                }
            }
            return result;
        }

        private async Task<IEnumerable<OrderTableViewModel>> FilterByDate(IQueryable<OrderTableViewModel> orders, DateTime past, DateTime now)
        {
            if (past != DateTime.MinValue && now != DateTime.MinValue)
                return await orders.Where(o => o.Date >= past && o.Date <= now).ToListAsync();
            return null;
        }

        private async Task<IEnumerable<OrderTableViewModel>> FilterByName(IQueryable<OrderTableViewModel> orders, string name)
        {
            if (name != null)
                return await orders.Where(o => o.Name == name).ToListAsync();
            return null;
        }

        private async Task<IEnumerable<OrderTableViewModel>> FilterByNumber(IQueryable<OrderTableViewModel> orders, string number)
        {
            if (number != null)
                return await orders.Where(o => o.Number == number).ToListAsync();
            return null;
        }

        private async Task<IEnumerable<OrderTableViewModel>> FilterByUnit(IQueryable<OrderTableViewModel> orders, string unit)
        {
            if (unit != null)
                return await orders.Where(o => o.Unit == unit).ToListAsync();
            return null;
        }

        private async Task<IEnumerable<OrderTableViewModel>> FilterByProvider(IQueryable<OrderTableViewModel> orders, string providername)
        {
            if (providername != null)
                return await orders.Where(o => o.ProviderName == providername).ToListAsync();
            return null;
        }
    }
}
