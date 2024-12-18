using ChuyenDeASPNET.Context;
using ChuyenDeASPNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuyenDeASPNET.Controllers
{
    public class HomeController : Controller
    {
        HuynhPhuongASPNETEntities1 objHuynhPhuongASPNETEntities = new HuynhPhuongASPNETEntities1();
        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListProduct = objHuynhPhuongASPNETEntities.Product.ToList();
            objHomeModel.ListCategories = objHuynhPhuongASPNETEntities.Categories.ToList();

            return View(objHomeModel);
        }
    }
}