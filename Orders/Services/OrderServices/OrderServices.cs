using Orders.Data;
using Orders.ViewModels.OrderItem;
using Orders.ViewModels.Orders;
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

        public Task<OrderItemViewModel> AddOrderItemToOrder(OrderItemViewModel orderItem)
        {
            throw new System.NotImplementedException();
        }

        public Task<OrderViewModel> CreateOrder(OrderViewModel orders)
        {
            throw new System.NotImplementedException();
        }

        public Task<OrderItemViewModel> CreateOrderItemToOrder(OrderItemViewModel orderItem)
        {
            throw new System.NotImplementedException();
        }

        public Task<OrderTableViewModel> GetOrders()
        {
            throw new System.NotImplementedException();
        }

        public Task<OrderTableViewModel> GetOrders(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateOrder(OrderUpdateViewModel orders)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateOrderItem(OrderItemUpdateViewModel orderItem)
        {
            throw new System.NotImplementedException();
        }
    }
}
