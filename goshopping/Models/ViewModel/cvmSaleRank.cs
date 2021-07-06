using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace goshopping.Models
{
    public class cvmSaleRank
    {
        [Key]
        [Display(Name = "名次")]
        public int RankNumber { get; set; }
        [Display(Name = "商品代號")]
        public string ProductNo { get; set; }
        [Display(Name = "商品名稱")]
        public string ProductName { get; set; }
        [Display(Name = "數值")]
        public int? RankValue { get; set; }
    }
}