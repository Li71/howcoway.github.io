using DevStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using goshopping.Models;
using DevStudio.Securitys;
using System.IO;

namespace goshopping.Controllers
{
    public class ProductController : Controller
    {
        goshoppingEntities db = new goshoppingEntities();

        public ActionResult CategoryList(string id)
        {
            int int_id = 0;
            ViewData["CategoryNo"] = id;
            ViewBag.CategoryNo = id;
            ViewBag.CategoryName = Shop.GetCategoryName(id, ref int_id);
            List<Products> model = new List<Products>();

            var prod1 = db.Products.Where(m => m.categoryid == int_id).ToList();
            if (prod1 != null && prod1.Count > 0)
            {
                foreach (var item1 in prod1)
                { model.Add(item1); }
            }

            var model1 = db.Categorys.Where(m => m.parentid == int_id).ToList();
            if (model1 != null && model1.Count > 0)
            {
                foreach (var mod1 in model1)
                {
                    var prod2 = db.Products.Where(m => m.categoryid == mod1.rowid).ToList();
                    if (prod2 != null && prod2.Count > 0)
                    {
                        foreach (var item2 in prod2)
                        { model.Add(item2); }
                    }

                    var model2 = db.Categorys.Where(m => m.parentid == mod1.rowid).ToList();
                    if (model2 != null && model2.Count > 0)
                    {
                        foreach (var mod2 in model2)
                        {
                            var prod3 = db.Products.Where(m => m.categoryid == mod2.rowid).ToList();
                            if (prod3 != null && prod3.Count > 0)
                            {
                                foreach (var item3 in prod3)
                                { model.Add(item3); }
                            }
                        }
                    }
                }
            }
            return View(model);
        }

        public ActionResult ProductDetail(string id)
        {
            if (id == null) id = "";
            var model = db.Products.Where(m => m.product_no == id).FirstOrDefault();
            if (model == null) return RedirectToAction("Index", "Home");
            Shop.ProductNo = id;
            string categoty_no = "";
            string categoty_name = "";
            Shop.GetCategoryByProductNo(model.product_no, ref categoty_no, ref categoty_name);

            ViewBag.CategoryNo = categoty_no;
            ViewBag.CategoryName = categoty_name;
            return View(model);
        }

        //public ActionResult ProductDetail(string qty, string color_no, string size_no, string product_no)

        [HttpPost]
        public ActionResult ProductDetail(FormCollection collection)
        {
            int int_qty = 0;
            //string str_property_no = "";
            string str_product_spec = "";
            int.TryParse(collection["qty"].ToString(), out int_qty);
            List<SelectListItem> list = Shop.GetPropertyList(Shop.ProductNo);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (collection[item.Value] != null)
                    {
                        str_product_spec += string.Format("{0}={1}", item.Text, collection[item.Value].ToString());
                    }
                }
            }
            Cart.AddCart(Shop.ProductNo, str_product_spec, int_qty);
            return RedirectToAction("ProductDetail", "Product", new { id = Shop.ProductNo });
        }

        [LoginAuthorize(RoleNo = "Guest,Member")]
        public ActionResult CartList()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                if (UserAccount.IsLogin)
                {
                    var data1 = db.Carts
                        .Where(m => m.user_no == UserAccount.UserNo)
                        .ToList();
                    return View(data1);
                }
                var data2 = db.Carts
                   .Where(m => m.lot_no == Cart.LotNo)
                   .ToList();
                return View(data2);
            }
        }

        [LoginAuthorize(RoleNo = "Guest,Member")]
        public ActionResult AddCart(string id)
        {
            return RedirectToAction("ProductDetail", "Product", new { id = id });
        }

        [LoginAuthorize(RoleNo = "Guest,Member")]
        public ActionResult CartDelete(int id)
        {
            var data = db.Carts
                .Where(m => m.rowid == id)
                .FirstOrDefault();
            if (data != null)
            {
                db.Carts.Remove(data);
                db.SaveChanges();
            }
            return RedirectToAction("CartList");
        }

        [LoginAuthorize(RoleNo = "Guest,Member")]
        public ActionResult CartPlus(int id)
        {
            var data = db.Carts
                .Where(m => m.rowid == id)
                .FirstOrDefault();
            if (data != null)
            {
                data.qty += 1;
                data.amount = data.qty * data.price;
                db.SaveChanges();
            }
            return RedirectToAction("CartList");
        }

        [LoginAuthorize(RoleNo = "Guest,Member")]
        public ActionResult CartMinus(int id)
        {
            var data = db.Carts
                .Where(m => m.rowid == id)
                .FirstOrDefault();
            if (data != null)
            {
                if (data.qty > 1)
                {
                    data.qty -= 1;
                    data.amount = data.qty * data.price;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CartList");
        }

        [LoginAuthorize(RoleNo = "Member")]
        public ActionResult Checkout()
        {
            cvmOrders models = new cvmOrders()
            {
                receive_name = "",
                receive_email = "",
                receive_phone = "",
                receive_address = "",
                payment_no = "01",
                shipping_no = "01",
                remark = "",
                PaymentsList = db.Payments.OrderBy(m => m.mno).ToList(),
                ShippingsList = db.Shippings.OrderBy(m => m.mno).ToList()
            };

            return View(models);
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Member")]
        public ActionResult Checkout(cvmOrders model)
        {
            if (!ModelState.IsValid)
            {
                if (model.PaymentsList == null)
                {
                    model.PaymentsList = db.Payments.OrderBy(m => m.mno).ToList();
                }
                if (model.ShippingsList == null)
                {
                    model.ShippingsList = db.Shippings.OrderBy(m => m.mno).ToList();
                }
                return View(model);
            }

            Cart.CartPayment(model);

            return Redirect("~/ECPayment.aspx");
        }

        public ActionResult CheckoutReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchText)
        {
            ViewBag.SearchText = searchText;
            if (string.IsNullOrEmpty(searchText)) searchText = "XXXXXXXXX";
            var model = db.Products
                .Where(m => m.product_no.Contains(searchText) || m.product_name.Contains(searchText))
                .ToList();
            return View(model);
        }


    }
}