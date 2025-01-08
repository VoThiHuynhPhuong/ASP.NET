using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuyenDeASPNET.Models; // Namespace của model (thay đổi tùy vào project của bạn)

namespace ChuyenDeASPNET.Controllers
{
    public class OrderController : Controller
    {
        // Danh sách đơn hàng giả lập (dùng để demo)
        private static List<Order> orders = new List<Order>
        {
            new Order { Id = 1, CustomerName = "John Doe", TotalAmount = 100.0m },
            new Order { Id = 2, CustomerName = "Jane Smith", TotalAmount = 200.0m }
        };

        // GET: Order
        public ActionResult Index()
        {
            return View(orders); // Trả danh sách đơn hàng về view
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            var order = orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return HttpNotFound("Order not found");
            }
            return View(order); // Trả về chi tiết đơn hàng
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        public ActionResult Create(Order newOrder)
        {
            if (ModelState.IsValid)
            {
                newOrder.Id = orders.Count > 0 ? orders.Max(o => o.Id) + 1 : 1;
                orders.Add(newOrder);
                return RedirectToAction("Index");
            }
            return View(newOrder);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            var order = orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return HttpNotFound("Order not found");
            }
            return View(order);
        }

        // POST: Order/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Order updatedOrder)
        {
            var order = orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return HttpNotFound("Order not found");
            }

            if (ModelState.IsValid)
            {
                order.CustomerName = updatedOrder.CustomerName;
                order.TotalAmount = updatedOrder.TotalAmount;
                return RedirectToAction("Index");
            }
            return View(updatedOrder);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            var order = orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return HttpNotFound("Order not found");
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                orders.Remove(order);
            }
            return RedirectToAction("Index");
        }
    }

    // Mô hình dữ liệu đơn hàng
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
