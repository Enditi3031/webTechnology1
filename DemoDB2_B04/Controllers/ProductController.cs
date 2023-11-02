using DemoDB2_B04.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2_B04.Controllers
{
    public class ProductController : Controller
    {
        DBSportStoreEntities database = new DBSportStoreEntities();
        // GET: Product
        public ActionResult Index()
        {
            return View(database.Products.ToList());
        }
        public ActionResult admin()
        {
            return View(database.Products.ToList());
        }
        public ActionResult Create()
        {
            Product pro = new Product();
            return View(pro);
        }
        public ActionResult SelectCate()
        {
            Category se_cate = new Category();
            se_cate.ListCate = database.Categories.ToList<Category>();
            return PartialView(se_cate);
        }
        [HttpPost]
        public ActionResult Create(Product pro)
        {
            try
            {
                if (pro.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(pro.UploadImage.FileName);
                    string extent = Path.GetExtension(pro.UploadImage.FileName);
                    filename = filename + extent;
                    pro.ImagePro = "~/Content/images/" + filename;
                    pro.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), filename));
                }
                database.Products.Add(pro);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            return View(database.Products.Where(s => s.ProductID == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Product product)
        {
            try
            {
                product = database.Products.Where(s => s.ProductID == id).FirstOrDefault();
                database.Products.Remove(product);
                database.SaveChanges();
                return RedirectToAction("admin");
            }
            catch
            {
                return Content("This data is using in other table, Error Deletel!");
            }
        }
        public ActionResult Edit(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, Category cate)
        {
            if (cate.IDCate == null)
            {
                TempData["ErrorMessage"] = "Không thể chỉnh sửa sản phẩm với giá trị IDCate là null.";
                return RedirectToAction("Error");
            }
            else
            {
                // Tiếp tục xử lý chỉnh sửa
                database.Entry(cate).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult Error()
        {
            // Lấy thông báo lỗi từ TempData
            string errorMessage = TempData["ErrorMessage"] as string;
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }

    }
}