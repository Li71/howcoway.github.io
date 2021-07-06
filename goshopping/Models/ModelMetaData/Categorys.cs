using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace goshopping.Models
{
    [MetadataType(typeof(CategorysMetaData))]
    public partial class Categorys
    {
        private class CategorysMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "父記錄ID")]
            public string parentid { get; set; }
            [Display(Name = "分類編號")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入分類編號")]
            [Required(ErrorMessage = "編號不可空白!!")]
            public string category_no { get; set; }
            [Display(Name = "分類名稱")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入分類名稱")]
            [Required(ErrorMessage = "分類名稱不可空白!!")]
            public string category_name { get; set; }
        }
    }
}