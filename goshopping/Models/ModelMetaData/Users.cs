using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace goshopping.Models
{
    [MetadataType(typeof(UsersMetaData))]
    [Bind(Exclude = "varify_code,isvarify")]
    public partial class Users
    {
        [Display(Name = "驗證")]
        [NotMapped]
        public bool bool_isvarify { get; set; }
        private class UsersMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Required(ErrorMessage = "帳號不可空白!!")]
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入帳號")]
            [Display(Name = "帳號")]
            public string mno { get; set; }
            [Required(ErrorMessage = "姓名不可空白")]
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入姓名")]
            [Display(Name = "姓名")]
            public string mname { get; set; }
            [Required(ErrorMessage = "密碼不可空白")]
            [DataType(DataType.Password)]
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true)]
            [Display(Name = "密碼")]
            public string password { get; set; }
            [Required(ErrorMessage = "電子信箱不可空白")]
            [Display(Name = "電子信箱")]
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入電子信箱")]
            [EmailAddress(ErrorMessage = "請輸入電子信箱格式")]
            public string email { get; set; }
            [Required(ErrorMessage = "角色不可空白")]
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true)]
            [Display(Name = "角色")]
            public string role_no { get; set; }
            [Display(Name = "出生日期")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public Nullable<System.DateTime> birthday { get; set; }
            [Display(Name = "驗證碼")]
            public string varify_code { get; set; }
            [Display(Name = "驗證")]
            public Nullable<int> isvarify { get; set; }
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true)]
            [Display(Name = "備註")]
            public string remark { get; set; }
        }



    }
}
