using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace goshopping.Models
{
    [MetadataType(typeof(CartsMetaData))]
    public partial class Carts
    {
        private class CartsMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "購物車批號")]
            public string lot_no { get; set; }
            [Display(Name = "會員編號")]
            public string user_no { get; set; }
            [Display(Name = "商品編號")]
            public string product_no { get; set; }
            [Display(Name = "商品名稱")]
            public string product_name { get; set; }
            [Display(Name = "商品規格")]
            public string product_spec { get; set; }
            [Display(Name = "數量")]
            public Nullable<int> qty { get; set; }
            [Display(Name = "單價")]
            public Nullable<int> price { get; set; }
            [Display(Name = "金額")]
            public Nullable<int> amount { get; set; }
            [Display(Name = "建立時間")]
            public Nullable<System.DateTime> crete_time { get; set; }
        }
    }
}