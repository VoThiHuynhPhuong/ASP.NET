﻿using ChuyenDeASPNET.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WedASP.Areas.Admin.Filter;

namespace ChuyenDeASPNET.Areas.Admin.Controllers
{
    [AdminAuthorize] // Gắn bộ lọc vào toàn bộ controller

    public class BrandController : Controller
    {
        WebsiteASP_NETEntities1 objASPNETEntities = new WebsiteASP_NETEntities1();
        public ActionResult Index(string searchTerm, int? page)
        {
            // Get all products as IQueryable
            var lstBrand = objASPNETEntities.Brand.AsQueryable();

            // Search functionality
            if (!string.IsNullOrEmpty(searchTerm))
            {
                lstBrand = lstBrand.Where(p => p.Name.Contains(searchTerm));
            }

            ViewBag.CurrentFilter = searchTerm;

            // Pagination settings
            int pageSize = 10;
            int pageNumber = page ?? 1;

            // Ensure you call ToPagedList() on the IQueryable to get IPagedList
            var pagedList = lstBrand.OrderBy(p => p.Name).ToPagedList(pageNumber, pageSize);

            return View(pagedList);  // Return IPagedList<Product> to the view
        }
        public ActionResult Details(int Id)
        {
            var objBrand = objASPNETEntities.Brand.Where(n => n.Id == Id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var objBrand = objASPNETEntities.Brand.Where(n => n.Id == Id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Delete(Brand objBra)
        {
            var objBrand = objASPNETEntities.Brand.Where(n => n.Id == objBra.Id).FirstOrDefault();

            objASPNETEntities.Brand.Remove(objBrand);
            objASPNETEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Brand objBrand)
        {
            try
            {
                if (objBrand.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                    string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                    fileName = fileName + extension;
                    objBrand.Avatar = fileName;
                    objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                }

                objASPNETEntities.Brand.Add(objBrand);
                objASPNETEntities.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/Product
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();

            var brand = objASPNETEntities.Brand.Find(id);
            if (brand == null) return HttpNotFound();

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand objBrand)
        {
            try
            {
                var existingBrand = objASPNETEntities.Brand.Find(objBrand.Id);
                if (existingBrand == null)
                {
                    return HttpNotFound();
                }

                // Kiểm tra và xử lý tệp tải lên
                if (objBrand.ImageUpload != null && objBrand.ImageUpload.ContentLength > 0)
                {
                    // Xử lý ảnh
                    string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                    string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                    fileName = fileName + extension; // Thêm timestamp để tránh trùng tên
                    string filePath = Path.Combine(Server.MapPath("~/Content/images/items/"), fileName);

                    objBrand.ImageUpload.SaveAs(filePath);

                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(existingBrand.Avatar))
                    {
                        string oldFilePath = Path.Combine(Server.MapPath("~/Content/images/items/"), existingBrand.Avatar);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Cập nhật đường dẫn ảnh mới
                    existingBrand.Avatar = fileName;
                }

                // Cập nhật các trường khác
                existingBrand.Name = objBrand.Name;
                existingBrand.Slug = objBrand.Slug;

                // Đánh dấu thực thể là đã chỉnh sửa
                objASPNETEntities.Entry(existingBrand).State = EntityState.Modified;

                // Lưu thay đổi vào cơ sở dữ liệu
                objASPNETEntities.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần thiết (sử dụng thư viện log như NLog hoặc Serilog)
                TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình cập nhật sản phẩm. Vui lòng thử lại.";
                return RedirectToAction("Edit", new { id = objBrand.Id });
            }
        }

    }
}