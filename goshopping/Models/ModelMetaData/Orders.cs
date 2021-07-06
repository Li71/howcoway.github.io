using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace goshopping.Models
{
    [MetadataType(typeof(OrdersMetaData))]
    public partial class Orders
    {
        private class OrdersMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "訂單編號")]
            public string order_no { get; set; }
            [Display(Name = "訂單日期")]
            public Nullable<System.DateTime> order_date { get; set; }
            [Display(Name = "訂單狀態")]
            public string order_status { get; set; }
            [Display(Name = "會員編號")]
            public string user_no { get; set; }
            [Display(Name = "付款方式")]
            [Required(ErrorMessage = "付款方式不可空白")]
            public string payment_no { get; set; }
            [Display(Name = "運送方式")]
            [Required(ErrorMessage = "運送方式不可空白")]
            public string shipping_no { get; set; }
            [Display(Name = "收件人姓名")]
            [Required(ErrorMessage = "收件人姓名不可空白")]
            public string receive_name { get; set; }
            [Display(Name = "收件人信箱")]
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入電子信箱")]
            public string receive_email { get; set; }

            [Display(Name = "收件人電話")]
            [Required(ErrorMessage = "收件人電話不可空白")]
            public string receive_phone { get; set; }


            [Display(Name = "收件人地址")]
            [Required(ErrorMessage = "收件人地址不可空白")]
            public string receive_address { get; set; }
            [Display(Name = "未稅合計")]
            public Nullable<int> amounts { get; set; }
            [Display(Name = "營業稅")]
            public Nullable<int> taxs { get; set; }
            [Display(Name = "含稅合計")]
            public Nullable<int> totals { get; set; }
            [Display(Name = "訂單備註")]
            public string remark { get; set; }
            [Display(Name = "GUID")]
            public string order_guid { get; set; }
            [Display(Name = "已結案")]
            public Nullable<int> order_closed { get; set; }
            [Display(Name = "有效訂單")]
            public Nullable<int> order_validate { get; set; }
        }
    }
}