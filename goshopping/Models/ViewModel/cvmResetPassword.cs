using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace goshopping.Models
{
    public class cvmResetPassword
    {
        [Key]
        [Display(Name = "登入帳號")]
        public string UserNo { get; set; }

        [Required(ErrorMessage = "請輸入目前的密碼!!")]
        [DataType(DataType.Password)]
        [Display(Name = "目前密碼")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "請輸入新的密碼!!")]
        [DataType(DataType.Password)]
        [Display(Name = "新的密碼")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "請輸入確認密碼!!")]
        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("NewPassword", ErrorMessage = "確認密碼不正確!!")]
        public string ConfirmPassword { get; set; }
    }
}