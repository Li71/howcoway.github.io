using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevStudio;
using DevStudio.Securitys;
using goshopping.Models;

namespace goshopping.Areas.Admin.Controllers
{
    public class MemberController : Controller
    {
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Index(int id = 2)
        {
            UserAccount.UserRoleNo = "Member";
            UserAccount.UserStatus = id;
            return View();
        }

        public ActionResult GetDataList()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                if (UserAccount.UserStatus == 2)
                {
                    // 全部會員資料
                    var all_users = db.Users
                    .Where(m => m.role_no == UserAccount.UserRoleNo)
                    .OrderBy(m => m.mno).ToList();
                    return Json(new { data = all_users }, JsonRequestBehavior.AllowGet);
                }

                // 已審核/未審核會員資料
                var models = db.Users
                .Where(m => m.role_no == UserAccount.UserRoleNo)
                .Where(m => m.isvarify == UserAccount.UserStatus)
                .OrderBy(m => m.mno).ToList();
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
                    Users new_model = new Users();
                    new_model.role_no = UserAccount.UserRoleNo;
                    if (UserAccount.UserStatus == 0)
                    { new_model.isvarify = 0; new_model.bool_isvarify = false; }
                    else
                    { new_model.isvarify = 1; new_model.bool_isvarify = true; }
                    return View(new_model);
                }
                var models = db.Users.Where(m => m.rowid == id).FirstOrDefault();
                models.bool_isvarify = Shop.IntegerToBool(models.isvarify);
                return View(models);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Edit(Users models)
        {
            bool status = false;
            ModelState.Remove("Password"); //忽略 Password 驗證
            ModelState.Remove("role_no"); //忽略 role_no 驗證
            if (ModelState.IsValid)
            {
                using (goshoppingEntities db = new goshoppingEntities())
                {
                    if (models.rowid > 0)
                    {
                        //Edit 
                        var users = db.Users.Where(m => m.rowid == models.rowid).FirstOrDefault();
                        if (users != null)
                        {
                            users.isvarify = Shop.BoolToInteger(models.bool_isvarify);
                            users.mno = models.mno;
                            users.mname = models.mname;
                            users.email = models.email;
                            users.birthday = models.birthday;
                            users.remark = models.remark;
                        }
                    }
                    else
                    {
                        //Save
                        models.role_no = UserAccount.UserRoleNo;
                        models.varify_code = UserAccount.GetNewVarifyCode(); //產生驗證碼
                        if (UserAccount.UserStatus < 2)
                            models.isvarify = UserAccount.UserStatus;
                        else
                            models.isvarify = Shop.BoolToInteger(models.bool_isvarify);

                        db.Users.Add(models);
                    }
                    try
                    {
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        db.Configuration.ValidateOnSaveEnabled = true;
                        status = true;
                    }
                    catch (Exception ex)
                    {
                        string str_message = ex.Message;
                        status = false;
                    }
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
                var model = db.Users.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    model.bool_isvarify = Shop.IntegerToBool(model.isvarify);
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
                var model = db.Users.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    db.Users.Remove(model);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Reset(int id)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Users.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    model.bool_isvarify = Shop.IntegerToBool(model.isvarify);
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Reset")]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult ResetPassword(int id)
        {
            bool status = false;
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Users.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    using (Cryptographys crpy = new Cryptographys())
                    {
                        model.password = crpy.SHA256Encode(model.mno);
                        db.SaveChanges();
                    }
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult Confirm(int id)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Users.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    model.isvarify = 1;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Member", new { area = "Admin", id = UserAccount.UserStatus });
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult UnConfirm(int id = 0)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Users.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    model.isvarify = 0;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Member", new { area = "Admin", id = UserAccount.UserStatus });
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
    }
}