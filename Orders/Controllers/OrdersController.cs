using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Orders.Services.OrderServices;
using Orders.ViewModels.Order;
using Orders.ViewModels.OrderItem;
using Orders.ViewModels.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace Orders.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderServices _orderServices;
        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        public async Task<ActionResult> Index([Bind("DateNow,DatePast,Name,Unit,Number,ProviderName")] OrdersFilterViewModel filter)
        {                                                               
            return View(await _orderServices.GetOrdersTable(filter));
        }

        public async Task<ActionResult> DetailsOrderItem(int id)
        {
            return View(await _orderServices.GetOrderItem(id));
        }

        public async Task<ActionResult> DetailsOrder(int id)
        {
            return View(await _orderServices.GetOrder(id));
        }

        public async Task<ActionResult> CreateOrder()
        {
            var result = await _orderServices.GetProviders();
            ViewBag.Providers = new SelectList(result, "Id", "Name");
            return View();
        }

        public async Task<ActionResult> CreateOrderItem(int? orderid)
        {
            var result = await _orderServices.GetOrders();
            if (orderid != null)
                result.Where(a => a.Id == orderid);
            ViewBag.Orders = new SelectList(result, "Id", "Number");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrder([Bind("Number,Date,ProviderId")] OrderViewModel order)
        {
            try
            {
                if (order == null)
                    return BadRequest();
                
                var result = await _orderServices.CreateOrder(order);
                if (result == null)
                    return BadRequest();

                return RedirectToAction("CreateOrderItem", new { orderid = result.Id});
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrderItem([Bind("Name,Quantity,Unit,OrderId")] OrderItemViewModel orderItem)
        {
            try
            {
                if (orderItem == null)
                    return BadRequest();

                var result = await _orderServices.CreateOrderItemToOrder(orderItem); 
                if (result == null)
                    return BadRequest();
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrderItemAndAdd([Bind("Name,Quantity,Unit,OrderId")] OrderItemViewModel orderItem)
        {
            try
            {
                if (orderItem == null)
                    return BadRequest();

                var result = await _orderServices.CreateOrderItemToOrder(orderItem);
                if (result == null)
                    return BadRequest();

                return RedirectToAction("CreateOrderItem", new { orderid = result.OrderId });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditOrders(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrders(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditOrderItems(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrderItems(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> DeleteOrderItem(int id)
        {
            var result = await _orderServices.DeleteOrderItem(id);
            if (result == true)
                return RedirectToAction("Index");
            return BadRequest();
        }

        public async Task<ActionResult> DeleteOrder(int id)
        {
            var result = await _orderServices.DeleteOrder(id);
            if (result == true)
                return RedirectToAction("Index");
            return BadRequest();
        }

    }
}
