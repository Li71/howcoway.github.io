using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using goshopping.Models;

namespace goshopping.Areas.Admin.Controllers
{
    public class PropertysController : Controller
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
                var models = db.Propertys.OrderBy(m => m.mno).ToList();
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
                    Propertys new_model = new Propertys();
                    return View(new_model);
                }
                var models = db.Propertys.Where(m => m.rowid == id).FirstOrDefault();
                return View(models);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Edit(Propertys models)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (goshoppingEntities db = new goshoppingEntities())
                {
                    if (models.rowid > 0)
                    {
                        //Edit 
                        var propertys = db.Propertys.Where(m => m.rowid == models.rowid).FirstOrDefault();
                        if (propertys != null)
                        {
                            propertys.mno = models.mno;
                            propertys.mname = models.mname;
                            propertys.remark = models.remark;
                        }
                    }
                    else
                    {
                        //Save
                        db.Propertys.Add(models);
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
                var model = db.Propertys.Where(m => m.rowid == id).FirstOrDefault();
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
                var model = db.Propertys.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    db.Propertys.Remove(model);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}