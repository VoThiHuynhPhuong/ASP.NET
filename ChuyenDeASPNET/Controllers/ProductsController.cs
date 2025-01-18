using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using ChuyenDeASPNET.Context;

namespace ChuyendeASPT.NET.Controllers
{
    public class ProductsController : Controller
    {
        WebsiteASP_NETEntities1 objWebsiteASP_NETEntities1 = new WebsiteASP_NETEntities1();
        public ActionResult ProductDetail(int Id)
        {
            var objProduct = objWebsiteASP_NETEntities1.Product.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }
        public ActionResult AllProduct(string currentFilter, string SearchString, int? page)
        {
            var lstProduct = new List<Product>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstProduct = objWebsiteASP_NETEntities1.Product.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstProduct = objWebsiteASP_NETEntities1.Product.ToList();
            }
            ViewBag.CurentFIlter = SearchString;
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            lstProduct = lstProduct.OrderByDescending(n => n.CategoryId).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }
    }
}