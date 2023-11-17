using DemoDB2_B04.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2_B04.Controllers
{
    public class LoginUserController : Controller
    {
        DBSportStoreEntities3 database = new DBSportStoreEntities3();
        // GET: LoginUser
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAcount(AdminUser _user)//hao
        {
            if("admin@gmail.com" == _user.NameUser && "admin123456" == _user.PasswordUser)
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["ID"] = "admin";
                Session["PasswordUser"] = _user.PasswordUser;
                return RedirectToAction("admin", "Product");
            }
            var check = database.AdminUsers.Where(s => s.NameUser == _user.NameUser && s.PasswordUser == _user.PasswordUser).FirstOrDefault();
            if (check == null) //logi sai thong tin
            {
                ViewBag.ErrorInfo = "SaiInfo";
                return View("Index");
            }
            else
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["ID"] = _user.ID;
                Session["PasswordUser"] = _user.PasswordUser;
                return RedirectToAction("Index", "Product");

            }
        }
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(AdminUser _user)
        {
            var lastUserId = database.AdminUsers.OrderByDescending(u => u.ID).Select(u => u.ID).FirstOrDefault();
            _user.ID = lastUserId + 1;
            if (ModelState.IsValid)
            {
                var check_ID = database.AdminUsers.Where(s => s.NameUser == _user.NameUser).FirstOrDefault();
                if (check_ID == null) //chua co ID
                {
                    database.Configuration.ValidateOnSaveEnabled = false;
                    database.AdminUsers.Add(_user);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorRegister = "This ID is exixst";
                    return View();
                }
            }
            return View();
        }
        public ActionResult LogOutUser()
        {
            Session.Clear();
            return RedirectToAction("Index", "LoginUser");
        }
    }
}