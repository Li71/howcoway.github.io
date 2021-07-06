using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace goshopping.Models
{
    [MetadataType(typeof(CompanysMetaData))]
    public partial class Companys
    {
        private class CompanysMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "公司編號")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入公司編號")]
            [Required(ErrorMessage = "公司編號不可空白!!")]
            public string mno { get; set; }
            [Display(Name = "公司簡稱")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入公司簡稱")]
            [Required(ErrorMessage = "公司簡稱不可空白!!")]
            public string msname { get; set; }
            [Display(Name = "公司名稱")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入公司名稱")]
            [Required(ErrorMessage = "公司名稱不可空白!!")]
            public string mname { get; set; }
            [Display(Name = "成立日期")]
            [DisplayFormat(DataFormatString ="{0:yyyy/MM/dd}" , ApplyFormatInEditMode =true)]
            public Nullable<System.DateTime> date_register { get; set; }
            [Display(Name = "公司電話")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入公司電話")]
            public string tel_company { get; set; }
            [Display(Name = "傳真電話")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入傳真電話")]
            public string fax_company { get; set; }
            [Display(Name = "負責人")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入負責人")]
            public string name_charge { get; set; }
            [Display(Name = "連絡人")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入連絡人")]
            public string name_contact { get; set; }
            [Display(Name = "連絡電話")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入連絡電話")]
            public string tel_contact { get; set; }
            [Display(Name = "公司地址")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入公司地址")]
            public string adr_company { get; set; }
            [Display(Name = "電子信箱")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入電子信箱")]
            [EmailAddress(ErrorMessage = "電子信箱格式錯誤!!")]
            public string email_company { get; set; }
            [Display(Name = "公司網址")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入公司網址")]
            [Url(ErrorMessage = "公司網址格式錯誤!!")]
            public string url_company { get; set; }
            [Display(Name = "備註")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入備註")]
            public string remark { get; set; }
        }
    }
}