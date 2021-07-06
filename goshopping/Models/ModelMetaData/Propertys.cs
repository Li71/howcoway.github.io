using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace goshopping.Models
{
    [MetadataType(typeof(PropertysMetaData))]
    public partial class Propertys
    {
        private class PropertysMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "屬性編號")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入屬性編號")]
            [Required(ErrorMessage = "屬性編號不可空白")]
            public string mno { get; set; }
            [Display(Name = "屬性名稱")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入屬性名稱")]
            [Required(ErrorMessage = "屬性名稱不可空白")]
            public string mname { get; set; }
            [Display(Name = "屬性值")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入屬性值")]
            [Required(ErrorMessage = "屬性值不可空白")]
            public string mvalue { get; set; }
            [Display(Name = "備註")]
            [DisplayFormat(ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入備註")]
            public string remark { get; set; }
        }
    }
}