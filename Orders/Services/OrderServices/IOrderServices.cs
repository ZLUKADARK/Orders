using Orders.ViewModels.OrderItem;
using Orders.ViewModels.Orders;
using Orders.ViewModels.Provider;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orders.Services.OrderServices
{
    public interface IOrderServices
    {
        public Task<OrderViewModel> CreateOrder(OrderViewModel orders);
        public Task<OrderItemViewModel> CreateOrderItemToOrder(OrderItemViewModel orderItem);
        public Task<IEnumerable<OrderTableViewModel>> GetOrdersTable();
        public Task<OrderTableViewModel> GetOrderTable(int id);
        public Task<IEnumerable<OrderViewModel>> GetOrders();
        public Task<OrderViewModel> GetOrder(int? id);
        public Task<IEnumerable<ProviderViewModel>> GetProviders();
        public Task<bool> DeleteOrderItem(int id);
        public Task<bool> DeleteOrder(int id);
        public Task<bool> UpdateOrder(int id, OrderUpdateViewModel orders);
        public Task<bool> UpdateOrderItem(int id, OrderItemUpdateViewModel orderItem);
    }
}
