using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace goshopping.Models
{
    [MetadataType(typeof(RolesMetaData))]
    public partial class Roles
    {
        private class RolesMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "角色編號")]
            [Required(ErrorMessage = "角色編號不可空白")]
            public string mno { get; set; }
            [Display(Name = "角色名稱")]
            [Required(ErrorMessage = "角色名稱不可空白")]
            public string mname { get; set; }
            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}