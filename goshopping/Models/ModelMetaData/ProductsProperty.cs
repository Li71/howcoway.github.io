using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace goshopping.Models
{
    [MetadataType(typeof(ProductsPropertyMetaData))]
    public partial class ProductsProperty
    {
        private class ProductsPropertyMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "商品編號")]
            public string product_no { get; set; }
            [Display(Name = "屬性編號")]
            public string property_no { get; set; }
            [Display(Name = "屬性值")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入屬性值")]
            public string text_value { get; set; }
            [Display(Name = "備註")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入備註")]
            public string remark { get; set; }
        }
    }
}