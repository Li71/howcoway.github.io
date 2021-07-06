using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using goshopping.Models;

namespace goshopping.Areas.Admin.Controllers
{
    public class PaymentController : Controller
    {
        [LoginAuthorize(RoleNo ="Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult GetDataList()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.Payments.OrderBy(m => m.mno).ToList();
                return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Edit(int id = 0)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                if (id == 0)
                {
                    Payments new_model = new Payments();
                    return View(new_model);
                }
                var models = db.Payments.Where(m => m.rowid == id).FirstOrDefault();
                return View(models);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Edit(Payments models)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (goshoppingEntities db = new goshoppingEntities())
                {
                    if (models.rowid > 0)
                    {
                        //Edit 
                        var payments = db.Payments.Where(m => m.rowid == models.rowid).FirstOrDefault();
                        if (payments != null)
                        {
                            payments.mno = models.mno;
                            payments.mname = models.mname;
                            payments.remark = models.remark;
                        }
                    }
                    else
                    {
                        //Save
                        db.Payments.Add(models);
                    }
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Delete(int id)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Payments.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult DeleteData(int id)
        {
            bool status = false;
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Payments.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    db.Payments.Remove(model);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}