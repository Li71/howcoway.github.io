using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevStudio;
using DevStudio.Securitys;
using goshopping.Models;

namespace goshopping.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {

        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Index()
        {
            UserAccount.UploadImageMode = false;
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

        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult AdminProfile()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Users
                    .Where(m => m.mno == UserAccount.UserNo)
                    .FirstOrDefault();
                return View(model);
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult EditProfile()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Users
                .Where(m => m.mno == UserAccount.UserNo)
                .FirstOrDefault();
                return View(model);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult EditProfile(Users model)
        {
            if (!ModelState.IsValid) return View(model);

            using (goshoppingEntities db = new goshoppingEntities())
            {
                var user = db.Users
                .Where(m => m.rowid == model.rowid)
                .FirstOrDefault();

                if (user != null)
                {
                    user.mname = model.mname;
                    user.email = model.email;
                    user.birthday = model.birthday;
                    user.remark = model.remark;
                    db.SaveChanges();
                }

                return RedirectToAction("AdminProfile");
            }
        }

        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult UploadImage()
        {
            UserAccount.UploadImageMode = true;
            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = UserAccount.UserNo + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Images/user"), fileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    file.SaveAs(path);
                }
            }
            UserAccount.UploadImageMode = false;
            return RedirectToAction("AdminProfile");
        }

        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult UploadCancel()
        {
            UserAccount.UploadImageMode = false;
            return RedirectToAction("AdminProfile");
        }
    }
}