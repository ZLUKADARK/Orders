using Orders.ViewModels.OrderItem;
using Orders.ViewModels.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orders.Services.OrderServices
{
    interface IOrderServices
    {
        public Task<OrderViewModel> CreateOrder(OrderViewModel orders);
        public Task<OrderItemViewModel> CreateOrderItemToOrder(OrderItemViewModel orderItem);
        public Task<OrderItemViewModel> AddOrderItemToOrder(OrderItemViewModel orderItem);
        public Task<IEnumerable<OrderTableViewModel>> GetOrders();
        public Task<OrderTableViewModel> GetOrder(int id);
        public Task<bool> UpdateOrder(int id, OrderUpdateViewModel orders);
        public Task<bool> UpdateOrderItem(int id, OrderItemUpdateViewModel orderItem);
    }
}
