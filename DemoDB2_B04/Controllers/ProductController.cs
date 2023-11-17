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
        DBSportStoreEntities3 database = new DBSportStoreEntities3();
        // GET: Product
        public ActionResult Index(string name)// tiềm kiếm ngoc peo
        {
            if (name == null)// khi người dùng không nhập tìm kiếm 
                return View(database.Products.ToList());// thì trả về list tất cả các sản phẩm của product
            else
                return View(database.Products.Where(s => s.NamePro.Contains(name)).ToList());// chức năng tiềm kiếm
        }
        public ActionResult Index2()
        {
            return View(database.Products.ToList());
        }
        public ActionResult Admin()
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
                pro.giamgia = 0;
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
                return RedirectToAction("Admin");
            }
            catch
            {
                return Content("This data is using in other table, Error Deletel!");
            }
        }
        public ActionResult Edit(int id)
        {
            return View(database.Products.Where(s => s.ProductID > id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            database.Entry(product).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index2");
        }
        public ActionResult giamgia(int id)
        {
            return View(database.Products.Where(s => s.ProductID > id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult giamgia(Product product)
        {
            database.Entry(product).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index2");
        }
        //public ActionResult Edit(int id, Category cate)
        //{
        //    // Tiếp tục xử lý chỉnh sửa
        //    database.Entry(cate).State = System.Data.Entity.EntityState.Modified;
        //    database.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult Error()
        {
            // Lấy thông báo lỗi từ TempData
            string errorMessage = TempData["ErrorMessage"] as string;
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }
        public ActionResult Detail(int id)
        {
           
            return View(database.Products.Where(s => s.ProductID == id).FirstOrDefault());
        }
    }
}