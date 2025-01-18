using ChuyenDeASPNET.Models;
using ChuyenDeASPNET.Context;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using User = ChuyenDeASPNET.Context.User;
namespace ChuyenDeASPNET.Controllers
{
    public class HomeController : Controller
    {
        WebsiteASP_NETEntities1 objWebsiteASP_NETEntities1 = new WebsiteASP_NETEntities1();

        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListCategory = objWebsiteASP_NETEntities1.Category.ToList();
            objHomeModel.ListProduct = objWebsiteASP_NETEntities1.Product.ToList();

            return View(objHomeModel);

        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _User)
        {
            if (ModelState.IsValid)
            {
                var check = objWebsiteASP_NETEntities1.User.FirstOrDefault(s => s.Email == _User.Email);
                if (check == null)
                {
                    _User.Password = GetMD5(_User.Password);
                    objWebsiteASP_NETEntities1.Configuration.ValidateOnSaveEnabled = false;
                    objWebsiteASP_NETEntities1.User.Add(_User);
                    objWebsiteASP_NETEntities1.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View();
        }
        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                // Mã hóa mật khẩu
                var f_password = GetMD5(password);
                // Kiểm tra thông tin đăng nhập
                var user = objWebsiteASP_NETEntities1.User
                            .FirstOrDefault(u => u.Email == email && u.Password == f_password);

                if (user != null)
                {
                    // Thêm thông tin vào Session
                    Session["FullName"] = user.FirstName + " " + user.LastName;
                    Session["Email"] = user.Email;
                    Session["idUser"] = user.Id;

                    // Kiểm tra quyền Admin
                    if (user.IsAdmin == true)
                    {
                        Session["IsAdmin"] = true;
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                    {
                        Session["IsAdmin"] = false;
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.error = "Login failed";
                }
            }
            return View();
        }
        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}