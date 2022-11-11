using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Orders.Services.OrderServices;
using Orders.ViewModels.OrderItem;
using Orders.ViewModels.Orders;
using System.Threading.Tasks;

namespace Orders.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderServices _orderServices;
        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }
        // GET: OrdersController
        public async Task<ActionResult> Index()
        {
            return View(await _orderServices.GetOrdersTable());
        }

        // GET: OrdersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrdersController/Create
        public async Task<ActionResult> CreateOrder()
        {
            var result = await _orderServices.GetProviders();
            ViewBag.Providers = new SelectList(result, "Id", "Name");
            return View();
        }

        public async Task<ActionResult> CreateOrderItem()
        {
            var result = await _orderServices.GetOrders();
            ViewBag.Orders = new SelectList(result, "Id", "Number");
            return View();
        }

        // POST: OrdersController/Create
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

                return RedirectToAction(nameof(CreateOrderItem));
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

        // GET: OrdersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: OrdersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrdersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
