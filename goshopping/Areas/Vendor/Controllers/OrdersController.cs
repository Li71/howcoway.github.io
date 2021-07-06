using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevStudio;
using DevStudio.Securitys;
using goshopping.Models;

namespace goshopping.Areas.Vendor.Controllers
{
    public class OrdersController : Controller
    {
        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult Index(int id = 0)
        {
            UserAccount.UploadImageMode = false;
            UserAccount.UserCode = id;
            using (Sale sale = new Sale(DateTime.Today))
            {
                sale.CountAmount("M");
                ViewBag.MonthAmount = sale.AmountData;
                ViewBag.MonthAmountPercent = sale.PercentData;

                sale.CountQty("M");
                ViewBag.MonthCount = sale.AmountData; ;
                ViewBag.MonthCountPercent = sale.PercentData;

                sale.CountAmount("Y");
                ViewBag.YearAmount = sale.AmountData;
                ViewBag.YearAmountPercent = sale.PercentData;

                sale.CountQty("Y");
                ViewBag.YearCount = sale.AmountData; ;
                ViewBag.YearCountPercent = sale.PercentData;

                List<string> arrYearMonthName = sale.GetYearMonthNameList();
                List<int> arrYearMonthAmount = sale.GetYearMonthAmountList("Base");
                ViewBag.BaseYearAmountRank = sale.GetSaleRank("Base", "Amount", 20);
                ViewBag.BaseYearQtyRank = sale.GetSaleRank("Base", "Qty", 20);
                ViewBag.BaseYearMonthName = Newtonsoft.Json.JsonConvert.SerializeObject(arrYearMonthName);
                ViewBag.BaseYearMonthAmount = Newtonsoft.Json.JsonConvert.SerializeObject(arrYearMonthAmount);

                sale.CountAmount("W");
                List<string> arrPriorWeekName = sale.GetWeekNameList("Prior");
                List<int> arrPriorWeekAmount = sale.GetWeekAmountList("Prior");
                ViewBag.PriorWeekName = Newtonsoft.Json.JsonConvert.SerializeObject(arrPriorWeekName);
                ViewBag.PriorWeekAmount = Newtonsoft.Json.JsonConvert.SerializeObject(arrPriorWeekAmount);

                List<string> arrBaseWeekName = sale.GetWeekNameList("Base");
                List<int> arrBaseWeekAmount = sale.GetWeekAmountList("Base");
                ViewBag.BaseWeekName = Newtonsoft.Json.JsonConvert.SerializeObject(arrBaseWeekName);
                ViewBag.BaseWeekAmount = Newtonsoft.Json.JsonConvert.SerializeObject(arrBaseWeekAmount);

                return View();
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult GetDataList()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.OrdersDetail
                    .Join(db.Orders , p => p.order_no , d => d.order_no,
                    (p1 , d1) => new 
                    {
                       p1,
                        order_date = d1.order_date,
                        order_status = d1.order_status,
                        order_closed = d1.order_closed,
                        user_no = d1.user_no
                       })
                        .Join(db.Status , p => p.order_status , d => d.mno ,
                        (p2 , d2) => new {
                            order_no = p2.p1.order_no,
                            order_date = p2.order_date,
                            order_status = p2.order_status,
                            order_closed = p2.order_closed,
                            user_no = p2.user_no,
                            vendor_no = p2.p1.vendor_no,
                            product_no = p2.p1.product_no,
                            product_name = p2.p1.product_name,
                            product_spec = p2.p1.product_spec,
                            price = p2.p1.price,
                            qty = p2.p1.qty,
                            amount = p2.p1.amount,
                            remark = p2.p1.remark,
                            status_name = d2.mname})
                        .Where(m => m.vendor_no == UserAccount.UserNo)
                        .Where(m => m.order_closed == UserAccount.UserCode)
                        .OrderByDescending(m => m.order_no)
                        .OrderBy(m => m.product_no)
                        .ToList();

                return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}