using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace goshopping.Models
{
    public class cvmOrderDetail
    {
        [Key]
        public int rowid { get; set; }
        public Orders OrderData { get; set; }
        public List<OrdersDetail> OrderDetailList { get; set; }
    }
}