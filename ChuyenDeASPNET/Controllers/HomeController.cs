using ChuyenDeASPNET.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuyenDeASPNET.Controllers
{
    public class HomeController : Controller
    {
        HuynhPhuongASPNETEntities objHuynhPhuongASPNETEntities = new HuynhPhuongASPNETEntities();
        public ActionResult Index()
        {
            var lstProduct = objHuynhPhuongASPNETEntities.Product.ToList();
            return View(lstProduct);
        }
    }
}