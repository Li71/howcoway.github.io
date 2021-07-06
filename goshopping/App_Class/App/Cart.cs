using goshopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevStudio.Securitys;

namespace DevStudio
{
    /// <summary>
    /// 購物車類別
    /// </summary>
    public static class Cart
    {
        #region 公開屬性
        /// <summary>
        /// 訂單編號
        /// </summary>
        public static string OrderNo { get; set; }

        /// <summary>
        /// 購物批號 LotNo
        /// </summary>
        public static string LotNo
        {
            get { return GetLotNo(); }
            set { HttpContext.Current.Session["CartLotNo"] = value; }
        }
        /// <summary>
        /// 購物批號建立時間
        /// </summary>
        public static DateTime LotCreateTime
        {
            get { return GetLotCreateTime(); }
            set { HttpContext.Current.Session["CartCreateTime"] = value; }
        }
        /// <summary>
        /// 購物車筆數
        /// </summary>
        public static int Counts { get { return GetCartCount(); } }

        /// <summary>
        /// 購物車合計
        /// </summary>
        public static int Totals { get { return GetCartTotals(); } }
        #endregion
        #region 公用函數
        /// <summary>
        /// 更新購物批號
        /// </summary>
        /// <returns></returns>
        public static string NewLotNo()
        {
            string str_lot_no = "";
            if (!UserAccount.IsLogin)
                str_lot_no = Guid.NewGuid().ToString().Substring(0, 15).ToUpper();
            LotNo = str_lot_no;
            LotCreateTime = DateTime.Now;
            return str_lot_no;
        }
        #endregion
        #region 公用事件
        /// <summary>
        /// 登入時將現有遊客的購物車加入客戶的購物車
        /// </summary>
        public static void LoginCart()
        {
            if (!string.IsNullOrEmpty(LotNo))
            {
                int int_qty = 0;
                using (goshoppingEntities db = new goshoppingEntities())
                {
                    var datas = db.Carts
                       .Where(m => m.lot_no == LotNo)
                       .ToList();
                    if (datas != null)
                    {
                        foreach (var item in datas)
                        {
                            int_qty = item.qty.GetValueOrDefault();
                            AddCart(item.product_no, item.product_spec, int_qty);
                            db.Carts.Remove(item);
                        }
                        db.SaveChanges();
                    }
                }
                NewLotNo();
            }
        }
        /// <summary>
        /// 加入購物車
        /// </summary>
        /// <param name="productNo">商品編號</param>
        public static void AddCart(string productNo)
        {
            AddCart(productNo, "", "", 1);
        }

        /// <summary>
        /// 加入購物車
        /// </summary>
        /// <param name="productNo">商品編號</param>
        /// <param name="prod_Spec">商品規格</param>
        /// <param name="buyQty">數量</param>
        public static void AddCart(string productNo, string prod_Spec, int buyQty)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                int int_price = Shop.GetProductPrice(productNo);
                int int_amount = (buyQty * int_price);
                var datas = db.Carts
                    .Where(m => m.lot_no == LotNo)
                    .Where(m => m.user_no == UserAccount.UserNo)
                    .Where(m => m.product_no == productNo)
                    .Where(m => m.product_spec == prod_Spec)
                    .FirstOrDefault();

                if (datas == null)
                {
                    Carts models = new Carts();
                    models.lot_no = LotNo;
                    models.user_no = UserAccount.UserNo;
                    models.crete_time = LotCreateTime;
                    models.product_no = productNo;
                    models.product_name = Shop.GetProductName(productNo);
                    models.product_spec = prod_Spec;
                    models.qty = buyQty;
                    models.amount = int_amount;
                    models.price = int_price;
                    db.Carts.Add(models);
                    db.SaveChanges();
                }
                else
                {
                    datas.qty += buyQty;
                    datas.amount = buyQty * int_price;
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 加入購物車
        /// </summary>
        /// <param name="productNo">商品編號</param>
        /// <param name="color_no">商品顏色</param>
        /// <param name="size_no">商品尺寸</param>
        /// <param name="buyQty">數量</param>
        public static void AddCart(string productNo, string color_no, string size_no, int buyQty)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                string str_spec = "";
                if (!string.IsNullOrEmpty(color_no)) str_spec += string.Format("顏色:{0}", color_no);
                if (!string.IsNullOrEmpty(size_no))
                {
                    if (!string.IsNullOrEmpty(str_spec)) str_spec += " ";
                    str_spec += string.Format("尺寸:{0}", size_no);
                }
                AddCart(productNo, str_spec, buyQty);
            }
        }

        /// <summary>
        /// 消費者付款
        /// </summary>
        public static int CartPayment(cvmOrders model)
        {
            int int_order_id = 0;
            OrderNo = CreateNewOrderNo(model);
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var datas = db.Carts
                   .Where(m => m.user_no == UserAccount.UserNo)
                   .ToList();
                if (datas != null)
                {
                    int int_amount = datas.Sum(m => m.amount).GetValueOrDefault();
                    decimal dec_tax = 0;
                    if (int_amount > 0)
                    {
                        dec_tax = Math.Round((decimal)(int_amount * 5 / 100), 0);
                    }
                    int int_total = int_amount + (int)dec_tax;
                    var data = db.Orders.Where(m => m.order_no == OrderNo).FirstOrDefault();
                    if (data != null)
                    {
                        data.amounts = int_amount;
                        data.taxs = (int)dec_tax;
                        data.totals = int_total ;
                        db.SaveChanges();
                    }

                    foreach (var item in datas)
                    {
                        OrdersDetail detail = new OrdersDetail();
                        detail.order_no = OrderNo;
                        detail.product_no = item.product_no;
                        detail.product_name = item.product_name;
                        detail.vendor_no = Shop.GetVendorNoByProduct(item.product_no);
                        detail.category_name = Shop.GetCategoryName(item.product_no);
                        detail.product_spec = item.product_spec;
                        detail.qty = item.qty;
                        detail.price = item.price;
                        detail.amount = item.amount;
                        detail.remark = "";
                        db.OrdersDetail.Add(detail);
                        db.SaveChanges();

                        db.Carts.Remove(item);
                        db.SaveChanges();
                    }
                }
            }
            return int_order_id;
        }

        #endregion
        #region 私有函數
        /// <summary>
        /// 取得購物批號建立時間
        /// </summary>
        /// <returns></returns>
        private static DateTime GetLotCreateTime()
        {
            object obj_time = HttpContext.Current.Session["CartCreateTime"];
            return (obj_time == null) ? DateTime.Now : DateTime.Parse(obj_time.ToString());
        }

        /// <summary>
        /// 取得購物批號
        /// </summary>
        /// <returns></returns>
        private static string GetLotNo()
        {
            return (HttpContext.Current.Session["CartLotNo"] == null) ? NewLotNo() : HttpContext.Current.Session["CartLotNo"].ToString();
        }

        /// <summary>
        /// 取得目前購物車筆數
        /// </summary>
        /// <returns></returns>
        private static int GetCartCount()
        {
            int int_count = 0;
            using (goshoppingEntities db = new goshoppingEntities())
            {
                if (UserAccount.IsLogin)
                {
                    var data1 = db.Carts
                        .Where(m => m.user_no == UserAccount.UserNo)
                        .ToList();
                    if (data1 != null) int_count = data1.Count;
                }
                else
                {
                    var data2 = db.Carts
                       .Where(m => m.lot_no == LotNo)
                       .ToList();
                    if (data2 != null) int_count = data2.Count;
                }
            }
            return int_count;
        }

        private static int GetCartTotals()
        {
            int? int_totals = 0;
            using (goshoppingEntities db = new goshoppingEntities())
            {
                if (UserAccount.IsLogin)
                {
                    var data1 = db.Carts
                        .Where(m => m.user_no == UserAccount.UserNo)
                        .ToList();
                    if (data1 != null) int_totals = data1.Sum(m => m.amount);
                }
                else
                {
                    var data2 = db.Carts
                       .Where(m => m.lot_no == LotNo)
                       .ToList();
                    if (data2 != null) int_totals = data2.Sum(m => m.amount);
                }
            }
            if (int_totals == null) int_totals = 0;
            return int_totals.GetValueOrDefault();
        }

        private static string CreateNewOrderNo(cvmOrders model)
        {
            Shop.OrderID = 0;
            Shop.OrderNo = "0";
            string str_order_no = "";
            string str_guid = Guid.NewGuid().ToString().Substring(0, 25).ToUpper();
            using (goshoppingEntities db = new goshoppingEntities())
            {
                Orders orders = new Orders();
                orders.order_closed = 0;
                orders.order_validate = 0;
                orders.order_no = "";
                orders.order_date = DateTime.Now;
                orders.user_no = UserAccount.UserNo;
                orders.order_status = "ON";
                orders.order_guid = str_guid;
                orders.payment_no = model.payment_no;
                orders.shipping_no = model.shipping_no;
                orders.receive_name = model.receive_name;
                orders.receive_email = model.receive_email;
                orders.receive_phone = model.receive_phone;
                orders.receive_address = model.receive_address;
                orders.remark = "";
                db.Orders.Add(orders);
                db.SaveChanges();

                var neword = db.Orders.Where(m => m.order_guid == str_guid).FirstOrDefault();
                if (neword != null)
                {
                    str_order_no = neword.rowid.ToString().PadLeft(8, '0');
                    neword.order_no = str_order_no;
                    Shop.OrderID = neword.rowid;
                    Shop.OrderNo = str_order_no;
                    db.SaveChanges();
                }
            }
            return str_order_no;
        }
        #endregion
    }
}