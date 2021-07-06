using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace goshopping.Models
{
    [MetadataType(typeof(ProductsMetaData))]
    public partial class Products
    {
        [NotMapped]
        [Display(Name = "首頁")]
        public bool bool_istop { get; set; }
        [NotMapped]
        [Display(Name = "熱門")]
        public bool bool_ishot { get; set; }
        [NotMapped]
        [Display(Name = "上架")]
        public bool bool_issale { get; set; }
        private class ProductsMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "分類ID")]
            public Nullable<int> categoryid { get; set; }
            [Display(Name = "商品分類")]
            public string category_name { get; set; }
            [Display(Name = "首頁顯示")]
            public Nullable<int> istop { get; set; }
            [Display(Name = "熱門商品")]
            public Nullable<int> ishot { get; set; }
            [Display(Name = "上架銷售")]
            public Nullable<int> issale { get; set; }
            [Display(Name = "瀏覽次數")]
            public Nullable<int> browse_count { get; set; }
            [Display(Name = "廠商編號")]
            [Required(ErrorMessage = "廠商編號不可空白")]
            public string vendor_no { get; set; }
            [Display(Name = "商品編號")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入商品編號")]
            [Required(ErrorMessage = "商品編號不可空白")]
            public string product_no { get; set; }
            [Display(Name = "商品名稱")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入商品名稱")]
            [Required(ErrorMessage = "商品名稱不可空白")]
            public string product_name { get; set; }
            [Display(Name = "商品規格")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入商品規格")]
            public string product_spec { get; set; }
            [Display(Name = "銷售單價")]
            public Nullable<int> price_cost { get; set; }
            [Display(Name = "銷售單價")]
            
            public Nullable<int> price_discont { get; set; }
            [Display(Name = "推薦星數")]
            public Nullable<int> start_count { get; set; }
            [Display(Name = "備註")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入備註")]
            public string remark { get; set; }
        }
    }
}