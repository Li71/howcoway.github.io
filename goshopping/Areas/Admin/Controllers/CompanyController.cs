using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using goshopping.Models;

namespace goshopping.Areas.Admin.Controllers
{
    public class CompanyController : Controller
    {
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDataList()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.Companys.OrderBy(m => m.mno).ToList();
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
                    Companys new_model = new Companys();
                    return View(new_model);
                }
                var models = db.Companys.Where(m => m.rowid == id).FirstOrDefault();
                return View(models);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Edit(Companys models)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (goshoppingEntities db = new goshoppingEntities())
                {
                    if (models.rowid > 0)
                    {
                        //Edit 
                        var Companys = db.Companys.Where(m => m.rowid == models.rowid).FirstOrDefault();
                        if (Companys != null)
                        {
                            Companys.mno = models.mno;
                            Companys.mname = models.mname;
                            Companys.msname = models.msname;
                            Companys.name_charge = models.name_charge;
                            Companys.name_contact = models.name_contact;
                            Companys.tel_company = models.tel_company;
                            Companys.fax_company = models.fax_company;
                            Companys.date_register = models.date_register;
                            Companys.tel_contact = models.tel_contact;
                            Companys.url_company = models.url_company;
                            Companys.email_company = models.email_company;
                            Companys.remark = models.remark;
                        }
                    }
                    else
                    {
                        //Save
                        db.Companys.Add(models);
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
                var model = db.Companys.Where(m => m.rowid == id).FirstOrDefault();
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
                var model = db.Companys.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    db.Companys.Remove(model);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}