using WedASP.Areas.Admin.Filter;
using ChuyenDeASPNET.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuyenDeASPNET.Areas.Admin.Controllers
{
    [AdminAuthorize] // Gắn bộ lọc vào toàn bộ controller
    public class HomeController : Controller
    {
        WebsiteASP_NETEntities1 objASPNETEntities = new WebsiteASP_NETEntities1();
        // GET: Admin/Home
        public ActionResult Index()
        {
            // Thống kê
            var totalProducts = objASPNETEntities.Product.Count();
            var totalOrders = objASPNETEntities.Order.Count();
            var totalCustomers = objASPNETEntities.User.Count();

            var totalRevenue = objASPNETEntities.OrderDetail
                .Where(od => od.Product != null && od.Product.Price.HasValue && od.Product.Price.HasValue)
                .Sum(od => od.Quantity * od.Product.Price.Value);

            // Truyền dữ liệu qua ViewBag hoặc ViewData
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalRevenue = totalRevenue;
            ViewData["Active"] = "Home";
            return View();
        }
    }
}