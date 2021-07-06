using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace goshopping.Models
{
    public class cvmLogin
    {
        [Key]
        [Required(ErrorMessage = "請輸入登入帳號!!")]
        [Display(Name = "登入帳號")]
        public string UserNo { get; set; }
        [Required(ErrorMessage = "請輸入登入密碼!!")]
        [DataType(DataType.Password)]
        [Display(Name = "登入密碼")]
        public string Password { get; set; }
        [Display(Name = "記住我")]
        public bool Remember { get; set; }
    }
}