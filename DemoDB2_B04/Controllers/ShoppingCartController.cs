using DemoDB2_B04.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2_B04.Controllers
{
    public class ShoppingCartController : Controller
    {
        DBSportStoreEntities2 database = new DBSportStoreEntities2();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("EmptyCart");
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        //Action tạo mới giỏ hàng
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;

            }
            return cart;
        }
        //Thêm product vào giỏ hàng
        public ActionResult AddToCart(int id)
        {
            var _pro = database.Products.SingleOrDefault(s => s.ProductID == id);//lấy pro theo id
            if (_pro != null)
            {
                GetCart().Add_Product_Cart(_pro);

            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        //Hàm cập nhật số lượng sản phẩm và tính lại tổng tiền
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["idPro"]);
            int _quantity = int.Parse(form["cartQuantity"]);
            cart.Update_quantity(id_pro, _quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        //Xóa dòng sản phẩm trong giỏ hàng
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int toltal_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                toltal_quantity_item = cart.Total_quantity();

            }
            ViewBag.QuantityCart = toltal_quantity_item;
            return PartialView("BagCart");
        }
        public ActionResult CheckOut()
        {
            int id = int.Parse(Session["ID"].ToString()); // Gán giá trị mặc định, hoặc giá trị mong muốn nếu Session không tồn tại
            var pro = database.AdminUsers.SingleOrDefault(s => s.ID == id);
            try
            {
                Cart cart = Session["Cart"] as Cart;
                OrderPro _order = new OrderPro();//bang hoa don sp
                _order.DateOrder = DateTime.Now;
                _order.AddressDeliverry = pro.RoleUser;
                _order.IDCus = pro.ID;
                database.OrderProes.Add(_order);
                foreach (var item in cart.Items)
                {
                    OrderDetail _order_detail = new OrderDetail();//luu dong san pham vao bang chi tiet hoa don
                    _order_detail.IDOrder = _order.ID;
                    _order_detail.IDProduct = item._product.ProductID;
                    _order_detail.UnitPrice = (double)item._quantity;
                    _order_detail.ImagePro = item._product.ImagePro;
                    _order_detail.Price = item._product.Price;
                    _order_detail.NamePro = item._product.NamePro;
                    database.OrderDetails.Add(_order_detail);
                }
                database.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("CheckOut_Success", "ShoppingCart");//
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message;
                return Content("Error checkout. Please check information of Customer...Thanks. Error: " + ex.Message);
            }
        }
        public ActionResult CheckOut_Success()
        {
            return View();
        }
        public ActionResult DanhsachDH()
        {
            var danhsach = database.OrderProes.ToList();
            return View(danhsach);
        }
        public ActionResult chitiet(int id)
        {
            var danhsach = database.OrderDetails.Where(s => s.IDOrder == id).ToList();
            return View(danhsach);
        }
        public ActionResult DatHang()//loc
        {
            int id = int.Parse(Session["ID"].ToString()); // Gán giá trị mặc định, hoặc giá trị mong muốn nếu Session không tồn tại
            var pro = database.AdminUsers.SingleOrDefault(s => s.ID == id);//lấy ra người dùng có id vừa đặng nhập vào
            Cart cart = Session["Cart"] as Cart;
            OrderPro order = new OrderPro();
            order.DateOrder = DateTime.Now.Date;
            order.AddressDeliverry = pro.RoleUser;
            order.IDCus = pro.ID;
            database.OrderProes.Add(order);
            database.SaveChanges();
            foreach (var item in cart.Items)
            {
                OrderDetail detail = new OrderDetail();
                detail.IDOrder = order.ID;
                detail.IDProduct = item._product.ProductID;
                detail.UnitPrice = (double)item._product.Price;
                detail.Quantity = item._quantity;
                detail.ImagePro = item._product.ImagePro;
                detail.Price = item._product.Price;
                detail.NamePro = item._product.NamePro;
                database.OrderDetails.Add(detail);
                database.SaveChanges();
            }
            cart.ClearCart();
            return RedirectToAction("Index", "Product");
        }
    }
}