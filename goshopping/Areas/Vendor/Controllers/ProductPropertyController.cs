using DevStudio;
using goshopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace goshopping.Areas.Vendor.Controllers
{
    public class ProductPropertyController : Controller
    {
        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult Index(int id = 0)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                if (string.IsNullOrEmpty(Shop.ProductNo))
                {
                    Shop.ProductNo = "";
                    var model = db.Products.Where(m => m.rowid == id).FirstOrDefault();
                    if (model != null)
                    {
                        Shop.ProductNo = model.product_no;
                        Shop.ProductName = model.product_name;
                    }
                }
                return View();
            }
        }

        public ActionResult GetDataList()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.ProductsProperty
                     .Join(db.Propertys, p => p.property_no, d => d.mno,
                    (p1, d1) => new
                    {
                        p1.rowid,
                        p1.product_no,
                        p1.property_no,
                        property_name = d1.mname,
                        p1.text_value,
                        p1.remark
                    })
                    .Where(m => m.product_no == Shop.ProductNo)
                    .OrderBy(m => m.property_no)
                    .ToList();
                return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult Edit(int id = 0)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.ProductsProperty.Where(m => m.rowid == id).FirstOrDefault();

                //Propertys DropDownList
                string str_pno = "";
                var propertyList = new List<SelectListItem>();
                List<Propertys> plists = db.Propertys.OrderBy(m => m.mno).ToList();
                foreach (var item in plists)
                {
                    SelectListItem list = new SelectListItem();
                    list.Value = item.mno.ToString();
                    list.Text = item.mname;
                    propertyList.Add(list);
                    if (id == 0)
                    { if (str_pno == "") str_pno = item.mno.ToString(); }
                }

                if (models != null) str_pno = models.property_no.ToString();

                //預設選擇哪一筆
                if (!string.IsNullOrEmpty(str_pno))
                    propertyList.Where(m => m.Value == str_pno).First().Selected = true;

                ViewBag.PropertyList = propertyList;

                if (id == 0)
                {
                    ProductsProperty new_model = new ProductsProperty();
                    new_model.product_no = Shop.ProductNo;
                    return View(new_model);
                }

                return View(models);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Edit(ProductsProperty models)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (goshoppingEntities db = new goshoppingEntities())
                {
                    if (models.rowid > 0)
                    {
                        //Edit 
                        var datas = db.ProductsProperty.Where(m => m.rowid == models.rowid).FirstOrDefault();
                        if (datas != null)
                        {
                            datas.property_no = models.property_no;
                            datas.text_value = models.text_value;
                            datas.remark = models.remark;
                        }
                    }
                    else
                    {
                        //Save
                        models.product_no = Shop.ProductNo;
                        if (string.IsNullOrEmpty(models.text_value))
                        {
                            var prod = db.Propertys.Where(m => m.mno == models.property_no).FirstOrDefault();
                            if (prod != null) models.text_value = prod.mvalue;
                        }
                        db.ProductsProperty.Add(models);
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
                var model = db.ProductsProperty.Where(m => m.rowid == id).FirstOrDefault();
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
                var model = db.ProductsProperty.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    db.ProductsProperty.Remove(model);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}