using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace goshopping.Models
{
    [MetadataType(typeof(ShippingsMetaData))]
    public partial class Shippings
    {
        private class ShippingsMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "編號")]
            [Required(ErrorMessage = "編號不可空白!!")]
            public string mno { get; set; }
            [Display(Name = "名稱")]
            [Required(ErrorMessage = "名稱不可空白!!")]
            public string mname { get; set; }
            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}