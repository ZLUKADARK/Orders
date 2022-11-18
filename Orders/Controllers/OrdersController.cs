﻿using Microsoft.AspNetCore.Http;
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

        public async Task<ActionResult> Index(DateTime past, DateTime now, string[] name, string[] unit, string[] number, string[] providerName)
        {
            var filter = new OrdersFilterViewModel { DatePast = past, DateNow = now, Name = name, Number = number, Unit = unit, ProviderName = providerName };
            var selectValues = await _orderServices.GetDistinct();
            ViewBag.SelectProductsName = new SelectList(selectValues.Name, "Name");
            ViewBag.SelectNumber = new SelectList(selectValues.Number, "Number");
            ViewBag.SelectProviderName = new SelectList(selectValues.ProviderName, "ProviderName");
            ViewBag.SelectProductUnit = new SelectList(selectValues.Unit, "Unit");
            ViewBag.DatePast = DateTime.Now.AddMonths(-1);
            ViewBag.DateNow = DateTime.Now;
            var result = await _orderServices.GetOrdersTable(filter);

            return View(result.OrderBy(o => o.Number));
        }

        public async Task<ActionResult> DetailsOrderItem(int id)
        {
            return View(await _orderServices.GetOrderItem(id));
        }

        public async Task<ActionResult> DetailsOrder(int id)
        {
            return View(await _orderServices.GetOrderWithItems(id));
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
            var providers = await _orderServices.GetProviders();
            ViewBag.Providers = new SelectList(providers, "Id", "Name");
            
            if (!ModelState.IsValid)
                return View();

            var result = await _orderServices.CreateOrder(order);
            if (result == null)
            {
                ViewBag.ErrorMessage = "Номер заказа уже существует в базе данных. Либо он соотвествует названию заказа, который существует в базе данных"; 
                return View();
            }
                
            try
            {
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
        public async Task<ActionResult> CreateOrderItem([Bind("Name,Quantity,Unit,OrderId")] OrderItemViewModel orderItem, bool next)
        {
            var orders = await _orderServices.GetOrders();
            ViewBag.Orders = new SelectList(orders, "Id", "Number");
            
            if (!ModelState.IsValid)
                return View();

            var result = await _orderServices.CreateOrderItemToOrder(orderItem);
            if (result == null)
            {
                ViewBag.ErrorMessage = "Название заказа соотвествует номеру заказа, который существует в базе данных";
                return View();
            }

            try
            {
                if (next)
                    return RedirectToAction("CreateOrderItem", new { orderid = result.OrderId });
                return RedirectToAction(nameof(Index));
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
             if (!ModelState.IsValid)
                return View();

            var result = await _orderServices.UpdateOrder(id, order);
            if (result == false)
            {
                var providers = await _orderServices.GetProviders();
                ViewBag.Providers = new SelectList(providers, "Id", "Name");
                ViewBag.ErrorMessage = "Номер заказа уже существует в базе данных. Либо он соотвествует названию заказа, который существует в базе данных";
                return View(await _orderServices.GetOrder(id));
            }

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
            if (!ModelState.IsValid)
                return View(item);
            
            var result = await _orderServices.UpdateOrderItem(id, item);
            if (result == false)
            {
                ViewBag.ErrorMessage = "Название заказа соотвествует номеру заказа, который существует в базе данных";
                return View(await _orderServices.GetOrderItem(id));
            }

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
