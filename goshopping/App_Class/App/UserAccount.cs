using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace DevStudio.Securitys
{
    public static class UserAccount
    {
        public static bool IsLogin { get; set; } = false;
        public static string UserNo { get; set; } = "";
        public static AppEnum.enUserRole RoleNo { get; set; } = AppEnum.enUserRole.Guest;
        public static string RoleName { get { return Enum.GetName(typeof(AppEnum.enUserRole), UserAccount.RoleNo); } }
        public static string UserName { get; set; } = "未登入";
        public static string UserInfo
        {
            get
            {
                return string.Format("{0} {1}", UserNo, UserName);
            }
        }

        public static AppEnum.enUserRole GetRoleNo(string roleNo)
        {
            AppEnum.enUserRole roleUser = AppEnum.enUserRole.Guest;
            Enum.TryParse(roleNo, true, out roleUser);
            return roleUser;
        }

        public static int UserStatus
        {
            get
            {
                int int_value = 0;
                if (HttpContext.Current.Session["UserStatus"] != null)
                {
                    string str_value = HttpContext.Current.Session["UserStatus"].ToString();
                    if (!int.TryParse(str_value, out int_value)) int_value = 0;
                }
                return int_value;
            }
            set
            { HttpContext.Current.Session["UserStatus"] = value; }
        }

        public static int UserCode
        {
            get
            {
                int int_value = -1;
                if (HttpContext.Current.Session["UserCode"] != null)
                {
                    string str_value = HttpContext.Current.Session["UserCode"].ToString();
                    if (!int.TryParse(str_value, out int_value)) int_value = -1;
                }
                return int_value;
            }
            set
            { HttpContext.Current.Session["UserCode"] = value; }
        }

        public static string UserRoleNo
        {
            get { return (HttpContext.Current.Session["UserRoleNo"] == null) ? "Guest" : HttpContext.Current.Session["UserRoleNo"].ToString(); }
            set { HttpContext.Current.Session["UserRoleNo"] = value; }
        }

        public static bool UploadImageMode
        {
            get
            {
                bool bln_value = false;
                if (HttpContext.Current.Session["UploadImage"] != null)
                {
                    string str_value = HttpContext.Current.Session["UploadImage"].ToString();
                    bln_value = (str_value == "1");
                }
                return bln_value;
            }
            set
            { HttpContext.Current.Session["UploadImage"] = (value) ? "1" : "0"; }
        }

        public static string UserImageUrl
        {
            get
            {
                string str_url = "~/Images/user/guest.jpg";
                string str_file = string.Format("~/Images/user/{0}.jpg", UserNo);
                if (File.Exists(HttpContext.Current.Server.MapPath(str_file))) str_url = str_file;
                str_url += string.Format("?{0}", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                return str_url;
            }
        }

        public static void Login(string userNo, string userName, AppEnum.enUserRole roleNo)
        {
            UserNo = userNo;
            UserName = userName;
            RoleNo = roleNo;
            IsLogin = true;
            Cart.LoginCart();
        }

        public static void LogOut()
        {
            UserNo = "";
            UserName = "";
            RoleNo = AppEnum.enUserRole.Guest;
            IsLogin = false;
        }
        public static string GetNewVarifyCode()
        {
            return Guid.NewGuid().ToString().ToUpper(); //產生驗證碼
        }

        public static void Demo(AppEnum.enUserRole roleNo)
        {
            if (roleNo == AppEnum.enUserRole.Admin) { UserNo = "admin"; UserName = "王小明"; }
            if (roleNo == AppEnum.enUserRole.Member) { UserNo = "001"; UserName = "王小明"; }
            if (roleNo == AppEnum.enUserRole.Vendor) { UserNo = "V01"; UserName = "王小明"; }
            RoleNo = roleNo;
            IsLogin = true;
            Cart.LoginCart();
        }
    }
}