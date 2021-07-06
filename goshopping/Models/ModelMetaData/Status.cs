using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace goshopping.Models
{
    [MetadataType(typeof(StatusMetaData))]
    public partial class Status
    {
        private partial class StatusMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "狀態編號")]
            [Required(ErrorMessage = "狀態編號不可空白")]
            public string mno { get; set; }
            [Display(Name = "狀態名稱")]
            [Required(ErrorMessage = "狀態名稱不可空白")]
            public string mname { get; set; }
            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}