using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace goshopping.Models
{
    [MetadataType(typeof(PaymentsMetaData))]
    public partial class Payments
    {
        private class PaymentsMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "編號")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入付款方式編號")]
            [Required(ErrorMessage = "付款方式編號不可空白!!")]
            public string mno { get; set; }
            [Display(Name = "名稱")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入付款方式名稱")]
            [Required(ErrorMessage = "付款方式名稱不可空白!!")]
            public string mname { get; set; }
            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}