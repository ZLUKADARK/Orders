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
using System;

namespace Orders.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderServices _orderServices;
        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        public async Task<ActionResult> Index(DateTime past, DateTime now, string name, string unit, string number, string providerName)
        {
            var filter = new OrdersFilterViewModel { DatePast = past, DateNow = now, Name = name, Number = number, Unit = unit, ProviderName = providerName };
            var selectValues = await _orderServices.GetDistinct();
            ViewBag.SelectProductsName = new SelectList(selectValues.Name, "Name");
            ViewBag.SelectNumber = new SelectList(selectValues.Number, "Number");
            ViewBag.SelectProviderName = new SelectList(selectValues.ProviderName, "ProviderName");
            ViewBag.SelectProductunit = new SelectList(selectValues.Unit, "Unit");

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

        public async Task<ActionResult> EditOrders(int id)
        {
            var result = await _orderServices.GetProviders();
            ViewBag.Providers = new SelectList(result, "Id", "Name");
            return View(await _orderServices.GetOrder(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOrders(int id, [Bind("Id,Number,Date,ProviderId")] OrderUpdateViewModel order)
        {
            var result = await _orderServices.UpdateOrder(id, order);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> EditOrderItems(int id)
        {
            return View(await _orderServices.GetOrderItem(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOrderItems(int id, [Bind("Id,Name,Quantity,Unit")] OrderItemUpdateViewModel item)
        {
            var result = await _orderServices.UpdateOrderItem(id, item);
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
