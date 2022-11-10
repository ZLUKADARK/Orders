using Orders.ViewModels.OrderItem;
using Orders.ViewModels.Orders;
using System.Threading.Tasks;

namespace Orders.Services.OrderServices
{
    interface IOrderServices
    {
        public Task<OrderViewModel> CreateOrder(OrderViewModel orders);
        public Task<OrderItemViewModel> CreateOrderItemToOrder(OrderItemViewModel orderItem);
        public Task<OrderItemViewModel> AddOrderItemToOrder(OrderItemViewModel orderItem);
        public Task<OrderTableViewModel> GetOrders();
        public Task<OrderTableViewModel> GetOrders(int id);
        public Task<bool> UpdateOrder(OrderUpdateViewModel orders);
        public Task<bool> UpdateOrderItem(OrderItemUpdateViewModel orderItem);
    }
}
