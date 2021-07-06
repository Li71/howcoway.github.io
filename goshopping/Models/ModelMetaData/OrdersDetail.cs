using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace goshopping.Models
{
    [MetadataType(typeof(OrdersDetailMetaData))]
    public partial class OrdersDetail
    {
        private class OrdersDetailMetaData
        {
            [Key]
            public int rowid { get; set; }
            [Display(Name = "訂單編號")]
            public string order_no { get; set; }
            [Display(Name = "廠商編號")]
            public string vendor_no { get; set; }
            [Display(Name = "商品分類")]
            public string category_name { get; set; }
            [Display(Name = "商品編號")]
            public string product_no { get; set; }
            [Display(Name = "商品名稱")]
            public string product_name { get; set; }
            [Display(Name = "商品規格")]
            public string product_spec { get; set; }
            [Display(Name = "單價")]
            public Nullable<int> price { get; set; }
            [Display(Name = "數量")]
            public Nullable<int> qty { get; set; }
            [Display(Name = "小計")]
            public Nullable<int> amount { get; set; }
            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}