using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuyenDeASPNET.Context;

namespace ChuyendeASPT.NET.Controllers
{
    public class ProductController : Controller
    {
        WebsiteASP_NETEntities1 objWebsiteASP_NETEntities1 = new WebsiteASP_NETEntities1();
        public ActionResult ProductDetail(int Id)
        {
            var objProduct = objWebsiteASP_NETEntities1.Product.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }

    }
}