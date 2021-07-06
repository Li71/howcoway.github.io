using DevStudio;
using DevStudio.Securitys;
using goshopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace goshopping.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Index(int id = 0, int code = -1)
        {
            UserAccount.UserStatus = id;
            if (code == -1)
            {
                if (UserAccount.UserCode == -1) UserAccount.UserCode = 0;
            }
            else
                UserAccount.UserCode = code;
            return View();
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult GetDataList()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.Orders
                    .Join(db.Payments, p => p.payment_no, d => d.mno,
                    (p1, d1) => new { p1, payment_name = d1.mname })
                    .Join(db.Status, p => p.p1.order_status, d => d.mno,
                    (p2, d2) => new { p2, status_name = d2.mname })
                    .Join(db.Users , p => p.p2.p1.user_no , d => d.mno,
                    (p3 , d3) => new { p3 , user_name = d3.mname})
                    .Join(db.Shippings, p => p.p3.p2.p1.shipping_no, d => d.mno,
                    (p4, d4) => new
                    {
                        rowid = p4.p3.p2.p1.rowid,
                        order_closed = p4.p3.p2.p1.order_closed,
                        user_no = p4.p3.p2.p1.user_no,
                        user_name = p4.user_name,
                        order_no = p4.p3.p2.p1.order_no,
                        order_date = p4.p3.p2.p1.order_date,
                        status_no = p4.p3.p2.p1.order_status,
                        status_name = p4.p3.status_name,
                        shipping_name = d4.mname,
                        payment_name = p4.p3.p2.payment_name,
                        receive_name = p4.p3.p2.p1.receive_name,
                        receive_address = p4.p3.p2.p1.receive_address,
                        remark = p4.p3.p2.p1.remark
                    })
                     .Where(m => m.order_closed == UserAccount.UserCode)
                     .OrderByDescending(m => m.order_no).ToList();

                return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult ChangeStatus(int id = 0)
        {
            string str_status = "ON";
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Orders.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null) str_status = model.order_status;

                var selectList = new List<SelectListItem>();
                List<Status> lists = Shop.GetStatusList();
                foreach (var item in lists)
                {
                    SelectListItem list = new SelectListItem();
                    list.Value = item.mno;
                    list.Text = item.mname;
                    selectList.Add(list);
                }
                //預設選擇哪一筆
                selectList.Where(m => m.Value == str_status).First().Selected = true;

                ViewBag.SelectList = selectList;
                return View(model);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult ChangeStatus(Orders model)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                bool status = false;
                var orders = db.Orders.Where(m => m.order_no == model.order_no).FirstOrDefault();
                if (orders != null)
                {
                    orders.order_status = model.order_status;
                    orders.order_closed = Shop.GetOrderClosed(model.order_status);
                    orders.order_validate = Shop.GetOrderValidate(model.order_status);
                    db.SaveChanges();
                    status = true;
                }
                return new JsonResult { Data = new { status = status } };
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Details(int id = 0)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                string str_order_no = "";
                var order = db.Orders.Where(m => m.rowid == id).FirstOrDefault();
                if (order != null) str_order_no = order.order_no;
                var details = db.OrdersDetail.Where(m => m.order_no == str_order_no).ToList();
                ViewBag.OrderNo = str_order_no;
                return View(details);
            }
        }
    }
}