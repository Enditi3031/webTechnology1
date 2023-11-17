using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoDB2_B04.Models
{
    //Tạo lớp cartitem chứa các dòng sản phẩm trong giỏ hàng
    public class CartItem
    {
        public Product _product { get; set; }
        public int _quantity { get; set; }
    }
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        { get { return items; } }
        // Hàm lấy sản phẩm bỏ vào giỏ hàng
        public void Add_Product_Cart(Product _pro, int _quan = 1)
        {
            var item = Items.FirstOrDefault(s => s._product.ProductID == _pro.ProductID);
            if (item == null)// nếu giỏ hàng rỗng thì thêm dòng hàng mới vào giỏ hàng
                items.Add(new CartItem
                {
                    _product = _pro,
                    _quantity = _quan
                });
            else
                item._quantity += _quan;//Tổng số lượng trong giỏ hàng đc cộng dồn

        }
        //Hàm tính tổng số lượng trong giỏ hàng
        public int Total_quantity()
        {
            return items.Sum(s => s._quantity);
        }
        // Hàm tính thành tiền cho mỗi dòng sản phẩm trong giỏ hàng
        public decimal Total_money()
        {
            var total = items.Sum(s => s._quantity * s._product.Price);
            return (decimal)total;
        }
        //hàm cập nhật lại số lượng sản phẩm ở mỗi dòng sản phẩm khi khách hàng muốn đặt mua thêm
        public void Update_quantity(int id, int _new_quan)
        {
            var item = items.Find(s => s._product.ProductID == id);
            if (item != null)
            {
                // Chuyển đổi giá trị chuỗi DecriptionPro thành số nguyên
                if (int.TryParse(item._product.DecriptionPro, out int descriptionValue))
                {
                    if (descriptionValue > _new_quan)
                    {
                        item._quantity = _new_quan;
                        descriptionValue = descriptionValue - _new_quan;
                        item._product.DecriptionPro = descriptionValue.ToString();
                    }
                    else
                    {
                        item._quantity = 1;
                    }
                }
                else
                {
                    // Xử lý lỗi chuyển đổi
                    Console.WriteLine("Không thể chuyển đổi giá trị DescriptionPro thành số nguyên.");
                    // Bạn có thể thực hiện xử lý khác tùy thuộc vào yêu cầu của bạn
                }
            }}
        //hàm xóa sản phẩm trong giỏ hàng
        public void Remove_CartItem(int id)
        {
            items.RemoveAll(s => s._product.ProductID == id);
        }
        //hàm xóa giỏ hàng sau khi Khách hàng thực hiện thanh toán
        public void ClearCart()
        {
            items.Clear();
        }
    }
}