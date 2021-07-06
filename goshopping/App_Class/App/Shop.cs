using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using goshopping.Models;
using System.Web.Configuration;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace DevStudio
{
    /// <summary>
    /// 購物商城類別
    /// </summary>
    public static class Shop
    {
        /// <summary>
        /// 應用程式名稱
        /// </summary>
        public static string AppName { get { return GetAppConfigValue("AppName"); } }
        /// <summary>
        /// 除錯模式
        /// </summary>
        public static bool DebugMode { get { return (GetAppConfigValue("DebugMode") == "1"); } }
        /// <summary>
        /// 取得輪播圖片列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCarouselImages()
        {
            List<string> lists = new List<string>();
            string str_folder = "~/Images/carousel";
            string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath(str_folder));
            if (files.Length > 0)
            {
                foreach (string fileName in files)
                {
                    lists.Add(string.Format((str_folder + "/{0}"), Path.GetFileName(fileName)));
                }
            }
            return lists;
        }
        /// <summary>
        /// 商品名稱
        /// </summary>
        public static string ProductNo
        {
            get { return (HttpContext.Current.Session["ProductNo"] == null) ? "" : HttpContext.Current.Session["ProductNo"].ToString(); }
            set { HttpContext.Current.Session["ProductNo"] = value; }
        }
        /// <summary>
        /// 商品名稱
        /// </summary>
        public static string ProductName
        {
            get { return (HttpContext.Current.Session["ProductName"] == null) ? "" : HttpContext.Current.Session["ProductName"].ToString(); }
            set { HttpContext.Current.Session["ProductName"] = value; }
        }
        /// <summary>
        /// 訂單 ID
        /// </summary>
        public static int OrderID
        {
            get { return (HttpContext.Current.Session["OrderID"] == null) ? 0 : (int)HttpContext.Current.Session["OrderID"]; }
            set { HttpContext.Current.Session["OrderID"] = value; }
        }
        /// <summary>
        /// 訂單 編號
        /// </summary>
        public static string OrderNo
        {
            get { return (HttpContext.Current.Session["OrderNo"] == null) ? "0" : HttpContext.Current.Session["OrderNo"].ToString(); }
            set { HttpContext.Current.Session["OrderNo"] = value; }
        }
        /// <summary>
        /// 布林值轉整數
        /// </summary>
        /// <param name="boolValue">布林值</param>
        /// <returns></returns>
        public static int BoolToInteger(bool boolValue)
        {
            return (boolValue) ? 1 : 0;
        }
        /// <summary>
        /// 整數轉布林值
        /// </summary>
        /// <param name="integerValue">整數</param>
        /// <returns></returns>
        public static bool IntegerToBool(int? integerValue)
        {
            return !(integerValue == null || integerValue != 1);
        }
        /// <summary>
        /// 取得商品圖片位址
        /// </summary>
        /// <param name="productNo">商品編號</param>
        /// <returns></returns>
        public static string GetProductImage(string productNo)
        {
            string str_image = string.Format("~/Images/product/{0}/{1}.jpg", productNo, productNo);
            if (File.Exists(HttpContext.Current.Server.MapPath(str_image)))
                str_image = string.Format("../../Images/product/{0}/{1}.jpg", productNo, productNo);
            else
                str_image = "../../Images/app/product.jpg";
            return str_image;
        }
        /// <summary>
        /// 取得置頂商品列表
        /// </summary>
        /// <returns></returns>
        public static List<Products> GetTopProducts()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var lists = db.Products
                  .Where(m => m.istop == 1)
                  .Where(m => m.issale == 1)
                  .OrderBy(m => m.product_no)
                  .ToList();
                return lists;
            }
        }
        /// <summary>
        /// 取得熱門商品列表
        /// </summary>
        /// <returns></returns>
        public static List<Products> GetHotProducts()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var lists = db.Products
                  .Where(m => m.ishot == 1)
                  .Where(m => m.issale == 1)
                  .OrderBy(m => m.product_no)
                  .ToList();
                return lists;
            }
        }
        /// <summary>
        /// 取得商品子分類列表
        /// </summary>
        /// <param name="id">父分類id</param>
        /// <returns></returns>
        public static List<Categorys> GetCategorys(int id)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var lists = db.Categorys
                    .Where(m => m.parentid == id)
                    .OrderBy(m => m.category_no)
                    .ToList();
                return lists;
            }
        }
        /// <summary>
        /// 取得父分類列表
        /// </summary>
        /// <param name="catrgoryNo">分類代號</param>
        /// <param name="list_no">代號列表</param>
        /// <param name="list_name">名稱列表</param>
        /// <returns></returns>
        public static int GetParentCategoryList(string catrgoryNo, ref List<string> list_no, ref List<string> list_name)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                int int_id = 0;
                int int_count = 0;
                var model = db.Categorys.Where(m => m.category_no == catrgoryNo).FirstOrDefault();
                if (model != null)
                {
                    int_id = model.rowid;
                    while (int_id > 0)
                    {
                        var item = db.Categorys.Where(m => m.rowid == int_id).FirstOrDefault();
                        if (item == null) return int_count;
                        int_count++;
                        list_no.Add(item.category_no);
                        list_name.Add(item.category_name);
                        int_id = item.parentid.GetValueOrDefault();
                    }
                }
                return int_count;
            }
        }
        /// <summary>
        /// 取得商品子分類產品數量
        /// </summary>
        /// <param name="id">父分類id</param>
        /// <returns></returns>
        public static int GetProductCount(int id)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                return db.Products.Where(m => m.categoryid == id).Count();
            }
        }
        /// <summary>
        /// 取得類別名稱
        /// </summary>
        /// <param name="productNo">商品編號</param>
        /// <returns></returns>
        public static string GetCategoryName(string productNo)
        {
            string str_name = "";
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var prod = db.Products.Where(m => m.product_no == productNo).FirstOrDefault();
                if (prod != null)
                {
                    int int_cate_id = prod.categoryid.GetValueOrDefault();
                    while (int_cate_id > 0)
                    {
                        var cate = db.Categorys.Where(m => m.rowid == int_cate_id).FirstOrDefault();
                        if (cate == null) break;
                        str_name = cate.category_name + (string.IsNullOrEmpty(str_name) ? "" : "/") + str_name;
                        int_cate_id = cate.parentid.GetValueOrDefault();
                    }
                }
                return str_name;
            }
        }
        /// <summary>
        /// 取得類別名稱
        /// </summary>
        /// <param name="categoryID">類別ID</param>
        /// <returns></returns>
        public static string GetCategoryName(int categoryID)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                string str_cate_name = "";
                while (categoryID > 0)
                {
                    var cate = db.Categorys.Where(m => m.rowid == categoryID).FirstOrDefault();
                    if (cate == null) break;
                    str_cate_name = cate.category_name + (string.IsNullOrEmpty(str_cate_name) ? "" : "/") + str_cate_name;
                    categoryID = cate.parentid.GetValueOrDefault();
                }
                return str_cate_name;
            }
        }
        /// <summary>
        /// 取得廠商編號
        /// </summary>
        /// <param name="productNo">商品編號</param>
        /// <returns></returns>
        public static string GetVendorNoByProduct(string productNo)
        {
            string str_no = "";
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var prod = db.Products.Where(m => m.product_no == productNo).FirstOrDefault();
                if (prod != null) str_no = prod.vendor_no;
            }
            return str_no;
        }
        /// <summary>
        /// 取得分類名稱
        /// </summary>
        /// <param name="cat_no">分類代號</param>
        /// <param name="cat_id">分類 ID</param>
        /// <returns></returns>
        public static string GetCategoryName(string cat_no, ref int cat_id)
        {
            cat_id = 0;
            string str_name = "";
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Categorys.Where(m => m.category_no == cat_no).FirstOrDefault();
                if (model != null)
                {
                    str_name = model.category_name;
                    cat_id = model.rowid;
                }
                return str_name;
            }
        }
        /// <summary>
        /// 用商品編號取得分類資訊
        /// </summary>
        /// <param name="productNo">商品編號</param>
        /// <param name="categoryNo">分類編號</param>
        /// <param name="categoryName">分類名稱</param>
        public static void GetCategoryByProductNo(string productNo, ref string categoryNo, ref string categoryName)
        {
            categoryNo = "";
            categoryName = "";
            int int_category_id = 0;
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Products.Where(m => m.product_no == productNo).FirstOrDefault();
                if (model != null && model.categoryid != null)
                { int.TryParse(model.categoryid.ToString(), out int_category_id); }
                if (int_category_id > 0)
                {
                    var categotymodel = db.Categorys.Where(m => m.rowid == int_category_id).FirstOrDefault();
                    if (categotymodel != null)
                    {
                        categoryNo = categotymodel.category_no;
                        categoryName = categotymodel.category_name;
                    }
                }
            }
        }
        /// <summary>
        /// 取得狀態列表
        /// </summary>
        /// <returns></returns>
        public static List<Status> GetStatusList()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                return db.Status.OrderBy(m => m.mno).ToList();
            }
        }
        /// <summary>
        /// 取得廠商列表
        /// </summary>
        /// <returns></returns>
        public static List<Users> GetVendorsList()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                return db.Users.Where(m => m.role_no == "Vendor").OrderBy(m => m.mno).ToList();
            }
        }
        /// <summary>
        /// 取得商品名稱
        /// </summary>
        /// <param name="productNo">商品代號</param>
        /// <returns></returns>
        public static string GetProductName(string productNo)
        {
            string str_name = "";
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.Products.Where(m => m.product_no == productNo).FirstOrDefault();
                if (models != null) str_name = models.product_name;
            }
            return str_name;
        }
        /// <summary>
        /// 取得使用者名稱
        /// </summary>
        /// <param name="userNo">使用者編號</param>
        /// <returns></returns>
        public static string GetUserName(string userNo)
        {
            string str_name = "";
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.Users.Where(m => m.mno == userNo).FirstOrDefault();
                if (models != null) str_name = models.mname;
            }
            return str_name;
        }
        /// <summary>
        /// 取得商品單價
        /// </summary>
        /// <param name="productNo">商品代號</param>
        /// <returns></returns>
        public static int GetProductPrice(string productNo)
        {
            int int_price = 0;
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.Products.Where(m => m.product_no == productNo).FirstOrDefault();
                if (models != null)
                {
                    if (models.price_discont != null && models.price_discont > 0)
                        int.TryParse(models.price_discont.ToString(), out int_price);
                    else if (models.price_sale != null)
                        int.TryParse(models.price_sale.ToString(), out int_price);
                }
            }
            return int_price;
        }
        /// <summary>
        /// 檢查訂單狀態是否為未結案
        /// </summary>
        /// <param name="orderStatus">訂單狀態</param>
        /// <returns></returns>
        public static bool IsUnCloseOrder(string orderStatus)
        {
            bool bln_value = false;
            if (orderStatus == "CP") bln_value = true;
            if (orderStatus == "OR") bln_value = true;
            if (orderStatus == "RT") bln_value = true;
            return bln_value;
        }
        /// <summary>
        /// 取得訂單結案狀態
        /// </summary>
        /// <param name="orderStatus">訂單狀態代碼</param>
        /// <returns></returns>
        public static int? GetOrderClosed(string orderStatus)
        {
            int? int_value = 0;
            //已領取已付款
            if (orderStatus == "CP") int_value = 1;
            //取消訂單
            if (orderStatus == "OR") int_value = 1;
            //已退貨
            if (orderStatus == "RT") int_value = 1;
            return int_value;
        }
        /// <summary>
        /// 取得訂單已付款已領貨狀態
        /// </summary>
        /// <param name="orderStatus">訂單狀態代碼</param>
        /// <returns></returns>
        public static int? GetOrderValidate(string orderStatus)
        {
            int? int_value = 0;
            //已領取已付款
            if (orderStatus == "CP") int_value = 1;
            return int_value;
        }

        public static string GetProductPropertyValue(string productNo, string propertyNo)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                string str_value = "";
                var model = db.ProductsProperty
                    .Where(m => m.product_no == productNo)
                    .Where(m => m.property_no == propertyNo)
                    .FirstOrDefault();
                if (model != null) str_value = model.text_value;
                return str_value;
            }
        }

        public static List<SelectListItem> GetPropertyList(string productNo)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                string str_name = "";
                List<SelectListItem> plist = new List<SelectListItem>();
                var models = db.ProductsProperty.Where(m => m.product_no == productNo).ToList();
                if (models != null)
                {
                    foreach (var item in models)
                    {
                        str_name = item.property_no;
                        var datas = db.Propertys.Where(m => m.mno == item.property_no).FirstOrDefault();
                        if (datas != null) str_name = datas.mname;
                        SelectListItem data = new SelectListItem();
                        data.Text = str_name;
                        data.Value = item.property_no;
                        plist.Add(data);
                    }
                }
                return plist;
            }
        }

        /// <summary>
        /// 取得 Web.config 中的 App.Config 設定值
        /// </summary>
        /// <param name="keyName">Key 值</param>
        /// <returns></returns>
        public static string GetAppConfigValue(string keyName)
        {
            object obj_value = WebConfigurationManager.AppSettings[keyName];
            return (obj_value == null) ? "" : obj_value.ToString();
        }
    }
}