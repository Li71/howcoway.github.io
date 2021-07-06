using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using goshopping.Models;
using DevStudio.Securitys;

namespace DevStudio
{
    /// <summary>
    /// 銷售相關類別
    /// </summary>
    public class Sale : IDisposable
    {
        #region 解構子
        private bool disposed = false;
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        /// <summary>
        /// 解構子,實現IDisposable中的Dispose方法
        /// </summary>
        public void Dispose()
        {
            //必須為true
            Dispose(true);
            //通知垃圾回收機制不再調用終端子（析構器）
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 解構子
        /// </summary>
        /// <param name="disposing">disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            //解構時要執行的其它程式
            if (disposing)
            {
                handle.Dispose();
            }
            //讓類別知道自己已經被釋放
            disposed = true;
        }
        /// <summary>
        /// Sale 解構子
        /// </summary>
        ~Sale()
        {
            //必須為false
            Dispose(false);
        }
        #endregion
        #region 建構子
        /// <summary>
        /// 建構子
        /// </summary>
        public Sale()
        {
            BaseDate = DateTime.Today;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="dateBase">基準日</param>
        public Sale(DateTime dateBase)
        {
            BaseDate = dateBase;
        }
        #endregion
        #region 屬性
        private string _BasePerior = "M";
        private DateTime _BaseDate = DateTime.Now;

        /// <summary>
        /// 週期代碼
        /// </summary>
        public string BasePerior { get { return _BasePerior; } set { _BasePerior = value; DatePropertyReset(); } }
        /// <summary>
        /// 基準日
        /// </summary>
        public DateTime BaseDate { get { return _BaseDate; } set { _BaseDate = value; DatePropertyReset(); } }
        /// <summary>
        /// 基準日的當月第一天
        /// </summary>
        public DateTime BaseFirstDate { get; set; }
        /// <summary>
        /// 基準日的當月最後一天
        /// </summary>
        public DateTime BaseLastDate { get; set; }
        /// <summary>
        /// 基準日的上月第一天
        /// </summary>
        public DateTime PriorFirstDate { get; set; }
        /// <summary>
        /// 基準日的上月最後一天
        /// </summary>
        public DateTime PriorLastDate { get; set; }
        /// <summary>
        /// 基準日的當月銷售日額
        /// </summary>
        public int BaseMonthAmount { get; set; }
        /// <summary>
        /// 金額文字
        /// </summary>
        public string AmountData { get; set; }
        /// <summary>
        /// 百分比文字
        /// </summary>
        public string PercentData { get; set; }
        #endregion
        #region 函數
        /// <summary>
        /// 取得前期數值與當期數值中的當期金額增加百分比
        /// </summary>
        /// <param name="priorValue">當期數</param>
        /// <param name="baseValue">前期數</param>
        /// <param name=""></param>
        /// <returns></returns>
        public double GetPercent(int priorValue, int baseValue)
        {
            double dbl_percent = 0;
            if (priorValue > 0 || baseValue > 0)
            {
                if (priorValue == 0)
                    dbl_percent = 100;
                else if (baseValue == 0)
                    dbl_percent = 100;
                else
                    dbl_percent = ((baseValue / priorValue) * 100);
            }
            return dbl_percent;
        }

        public void CountAmount(string basePerior)
        {
            BasePerior = basePerior;
            BaseDate = DateTime.Today;
            int int_prior_amount = GetSaleAmount(PriorFirstDate, PriorLastDate);
            int int_base_amount = GetSaleAmount(BaseFirstDate, BaseLastDate);
            double dbl_percent = GetPercent(int_prior_amount, int_base_amount);
            string str_code = (int_base_amount > int_prior_amount) ? "+" : "-";
            AmountData = int_base_amount.ToString("#,##0");
            PercentData = str_code + " " + dbl_percent.ToString("#,##00") + "%";
        }

        public void CountQty(string basePerior)
        {
            BasePerior = basePerior;
            BaseDate = DateTime.Today;
            int int_prior_count = GetSaleQty(PriorFirstDate, PriorLastDate);
            int int_base_count = GetSaleQty(BaseFirstDate, BaseLastDate);
            double dbl_percent = GetPercent(int_prior_count, int_base_count);
            string str_code = (int_base_count > int_prior_count) ? "+" : "-";
            AmountData = int_base_count.ToString("#,##0");
            PercentData = str_code + " " + dbl_percent.ToString("#,##00") + "%";
        }

        public List<string> GetWeekNameList(string codeAmount)
        {
            List<string> nameList = new List<string>();
            DateTime dtm_first = DateTime.MinValue;
            DateTime dtm_end = DateTime.MinValue;
            List<string> weekList = new List<string>() { "一", "二", "三", "四", "五", "六", "日" };
            string str_name = "";
            if (codeAmount == "Base") dtm_first = BaseFirstDate;
            if (codeAmount == "Prior") dtm_first = PriorFirstDate;
            for (int i = 0; i < 7; i++)
            {
                str_name = string.Format("{0} ({1})", dtm_first.ToString("MM/dd"), weekList[i]);
                nameList.Add(str_name);
                dtm_first = dtm_first.AddDays(1);
            }
            return nameList;
        }

        public List<string> GetYearMonthNameList()
        {
            List<string> nameList = new List<string>();
            List<string> monthList = new List<string>() { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二" };
            string str_name = "";
            for (int i = 0; i < 12; i++)
            {
                str_name = string.Format("{0}月", monthList[i]);
                nameList.Add(str_name);
            }
            return nameList;
        }

        public List<int> GetWeekAmountList(string codeAmount)
        {
            List<int> amountList = new List<int>();
            DateTime dtm_first = DateTime.MinValue;
            DateTime dtm_end = DateTime.MinValue;
            int int_amount = 0;
            if (codeAmount == "Base") dtm_first = BaseFirstDate;
            if (codeAmount == "Prior") dtm_first = PriorFirstDate;
            for (int i = 0; i < 7; i++)
            {
                dtm_first = dtm_first.ezMinDateTime();
                dtm_end = dtm_first.ezMaxDateTime();
                int_amount = GetSaleAmount(dtm_first, dtm_end);
                amountList.Add(int_amount);
                dtm_first = dtm_first.AddDays(1);
            }
            return amountList;
        }

        public List<int> GetYearMonthAmountList(string codeAmount)
        {
            List<int> amountList = new List<int>();
            DateTime dtm_first = DateTime.MinValue;
            DateTime dtm_end = DateTime.MinValue;
            int int_amount = 0;
            int int_year = 0;
            string str_month = "01";
            string str_days = "31";
            if (codeAmount == "Base") int_year = BaseFirstDate.Year;
            if (codeAmount == "Prior") int_year = PriorFirstDate.Year;
            for (int i = 1; i <= 12; i++)
            {
                str_month = i.ToString().PadLeft(2, '0');
                dtm_first = DateTime.Parse(string.Format("{0}-{1}-01 00:00:00", int_year, str_month));
                str_days = dtm_first.ezMonthDays().ToString();
                dtm_end = DateTime.Parse(string.Format("{0}-{1}-{2} 23:59:59", int_year, str_month, str_days));
                int_amount = GetSaleAmount(dtm_first, dtm_end);
                amountList.Add(int_amount);
                dtm_first = dtm_first.AddDays(1);
            }
            return amountList;
        }

        /// <summary>
        /// 取得日期範圍中的銷售金額
        /// </summary>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns></returns>
        private int GetSaleAmount(DateTime startDate, DateTime endDate)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                int int_amount = 0;
                startDate = startDate.ezMinDateTime();
                endDate = endDate.ezMaxDateTime();
                if (UserAccount.RoleNo == AppEnum.enUserRole.Admin)
                {
                    object obj_value = db.OrdersDetail
                        .Join(db.Orders, p => p.order_no, d => d.order_no,
                        (p1, d1) => new { p1, d1 })
                        .Where(m => m.d1.order_date >= startDate)
                        .Where(m => m.d1.order_date <= endDate)
                        .Sum(m => m.p1.amount);
                    if (obj_value != null) int.TryParse(obj_value.ToString(), out int_amount);
                }
                if (UserAccount.RoleNo == AppEnum.enUserRole.Member)
                {
                    object obj_value = db.OrdersDetail
                        .Join(db.Orders, p => p.order_no, d => d.order_no,
                        (p1, d1) => new { p1, d1 })
                        .Where(m => m.d1.order_date >= startDate)
                        .Where(m => m.d1.order_date <= endDate)
                        .Where(m => m.d1.user_no == UserAccount.UserNo)
                        .Sum(m => m.p1.amount);
                    if (obj_value != null) int.TryParse(obj_value.ToString(), out int_amount);
                }
                if (UserAccount.RoleNo == AppEnum.enUserRole.Vendor)
                {
                    object obj_value = db.OrdersDetail
                        .Join(db.Orders, p => p.order_no, d => d.order_no,
                        (p1, d1) => new { p1, d1 })
                        .Where(m => m.d1.order_date >= startDate)
                        .Where(m => m.d1.order_date <= endDate)
                        .Where(m => m.p1.vendor_no == UserAccount.UserNo)
                        .Sum(m => m.p1.amount);
                    if (obj_value != null) int.TryParse(obj_value.ToString(), out int_amount);
                }
                return int_amount;
            }
        }

        /// <summary>
        /// 取得日期範圍中的銷售訂單數
        /// </summary>
        /// <returns></returns>
        private int GetSaleQty(DateTime startDate, DateTime endDate)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                int int_amount = 0;
                startDate = startDate.ezMinDateTime();
                endDate = endDate.ezMaxDateTime();
                if (UserAccount.RoleNo == AppEnum.enUserRole.Admin)
                {
                    object obj_value = db.OrdersDetail
                        .Join(db.Orders, p => p.order_no, d => d.order_no,
                        (p1, d1) => new { p1, d1 })
                        .Where(m => m.d1.order_date >= startDate)
                        .Where(m => m.d1.order_date <= endDate)
                        .GroupBy(m => m.p1.order_no)
                        .Count();
                    if (obj_value != null) int.TryParse(obj_value.ToString(), out int_amount);
                }
                if (UserAccount.RoleNo == AppEnum.enUserRole.Member)
                {
                    object obj_value = db.OrdersDetail
                        .Join(db.Orders, p => p.order_no, d => d.order_no,
                        (p1, d1) => new { p1, d1 })
                        .Where(m => m.d1.order_date >= startDate)
                        .Where(m => m.d1.order_date <= endDate)
                        .Where(m => m.d1.user_no == UserAccount.UserNo)
                        .GroupBy(m => m.p1.order_no)
                        .Count();
                    if (obj_value != null) int.TryParse(obj_value.ToString(), out int_amount);
                }
                if (UserAccount.RoleNo == AppEnum.enUserRole.Vendor)
                {
                    object obj_value = db.OrdersDetail
                        .Join(db.Orders, p => p.order_no, d => d.order_no,
                        (p1, d1) => new { p1, d1 })
                        .Where(m => m.d1.order_date >= startDate)
                        .Where(m => m.d1.order_date <= endDate)
                        .Where(m => m.p1.vendor_no == UserAccount.UserNo)
                        .GroupBy(m => m.p1.order_no)
                        .Count();
                    if (obj_value != null) int.TryParse(obj_value.ToString(), out int_amount);
                }
                return int_amount;
            }
        }

        public List<cvmSaleRank> GetSaleRank(string codePeriod, string codeType, int numberRank)
        {
            DateTime dtm_first = DateTime.Today;
            DateTime dtm_end = DateTime.Today;
            if (codePeriod == "Base")
            {
                dtm_first = BaseFirstDate;
                dtm_end = BaseLastDate;
            }
            if (codePeriod == "Prior")
            {
                dtm_first = BaseFirstDate.AddYears(-1);
                dtm_end = BaseLastDate.AddYears(-1);
            }

            List<cvmSaleRank> dataList = new List<cvmSaleRank>();

            List<cvmSaleRank> amountList = GetSaleAmountRank(dtm_first, dtm_end);
            List<cvmSaleRank> qtyList = GetSaleQtyRank(dtm_first, dtm_end);
            for (int i = 0; i < numberRank; i++)
            {
                cvmSaleRank item = new cvmSaleRank()
                {
                    RankNumber = (i + 1),
                    ProductNo = "",
                    ProductName = "",
                    RankValue = 0
                };
                if (codeType == "Amount")
                {
                    if (amountList != null && amountList.Count > i)
                    {
                        item.ProductNo = amountList[i].ProductNo;
                        item.ProductName = amountList[i].ProductName;
                        item.RankValue = amountList[i].RankValue;
                    }
                    dataList.Add(item);
                }
                else
                {
                    if (qtyList != null && qtyList.Count > i)
                    {
                        item.ProductNo = qtyList[i].ProductNo;
                        item.ProductName = qtyList[i].ProductName;
                        item.RankValue = qtyList[i].RankValue;
                    }
                    dataList.Add(item);
                }
            }
            return dataList;
        }

        /// <summary>
        /// 取得日期範圍中的銷售金額合計前 20 名
        /// </summary>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns></returns>
        private List<cvmSaleRank> GetSaleAmountRank(DateTime startDate, DateTime endDate)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                startDate = startDate.ezMinDateTime();
                endDate = endDate.ezMaxDateTime();
                if (UserAccount.RoleNo == AppEnum.enUserRole.Admin)
                {
                    List<cvmSaleRank> model1 = db.OrdersDetail
                        .Join(db.Orders, p => p.order_no, d => d.order_no,
                        (p1, d1) => new { p1, d1 })
                        .Where(m => m.d1.order_date >= startDate)
                        .Where(m => m.d1.order_date <= endDate)
                        .Where(m => m.d1.order_validate == 1)
                        .GroupBy(m => new { m.p1.product_no, m.p1.product_name })
                        .Select(m => new cvmSaleRank
                        {
                            RankNumber = 0,
                            ProductNo = m.Key.product_no,
                            ProductName = m.Key.product_name,
                            RankValue = m.Sum(x => x.p1.amount)
                        })
                        .OrderByDescending(m => m.RankValue)
                        .ToList();
                    return model1;
                }
                if (UserAccount.RoleNo == AppEnum.enUserRole.Member)
                {
                    List<cvmSaleRank> model2 = db.OrdersDetail
                        .Join(db.Orders, p => p.order_no, d => d.order_no,
                        (p1, d1) => new { p1, d1 })
                        .Where(m => m.d1.order_date >= startDate)
                        .Where(m => m.d1.order_date <= endDate)
                        .Where(m => m.d1.order_validate == 1)
                        .Where(m => m.d1.user_no == UserAccount.UserNo)
                        .GroupBy(m => new { m.p1.product_no, m.p1.product_name })
                        .Select(m => new cvmSaleRank
                        {
                            RankNumber = 0,
                            ProductNo = m.Key.product_no,
                            ProductName = m.Key.product_name,
                            RankValue = m.Sum(x => x.p1.amount)
                        })
                        .OrderByDescending(m => m.RankValue)
                        .ToList();
                    return model2;
                }
                if (UserAccount.RoleNo == AppEnum.enUserRole.Vendor)
                {
                    List<cvmSaleRank> model3 = db.OrdersDetail
                        .Join(db.Orders, p => p.order_no, d => d.order_no,
                        (p1, d1) => new { p1, d1 })
                        .Where(m => m.d1.order_date >= startDate)
                        .Where(m => m.d1.order_date <= endDate)
                        .Where(m => m.d1.order_validate == 1)
                        .Where(m => m.p1.vendor_no == UserAccount.UserNo)
                        .GroupBy(m => new { m.p1.product_no, m.p1.product_name })
                        .Select(m => new cvmSaleRank
                        {
                            RankNumber = 0,
                            ProductNo = m.Key.product_no,
                            ProductName = m.Key.product_name,
                            RankValue = m.Sum(x => x.p1.amount)
                        })
                        .OrderByDescending(m => m.RankValue)
                        .ToList();
                    return model3;
                }
                List<cvmSaleRank> model4 = new List<cvmSaleRank>();
                return model4;
            }
        }

        /// <summary>
        /// 取得日期範圍中的銷售數量合計前 20 名
        /// </summary>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns></returns>
        private List<cvmSaleRank> GetSaleQtyRank(DateTime startDate, DateTime endDate)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                startDate = startDate.ezMinDateTime();
                endDate = endDate.ezMaxDateTime();
                if (UserAccount.RoleNo == AppEnum.enUserRole.Admin)
                {
                    List<cvmSaleRank> model1 = db.OrdersDetail
                        .Join(db.Orders, p => p.order_no, d => d.order_no,
                        (p1, d1) => new { p1, d1 })
                        .Where(m => m.d1.order_date >= startDate)
                        .Where(m => m.d1.order_date <= endDate)
                        .Where(m => m.d1.order_validate == 1)
                        .GroupBy(m => new { m.p1.product_no, m.p1.product_name })
                        .Select(m => new cvmSaleRank
                        {
                            RankNumber = 0,
                            ProductNo = m.Key.product_no,
                            ProductName = m.Key.product_name,
                            RankValue = m.Sum(x => x.p1.qty)
                        })
                        .OrderByDescending(m => m.RankValue)
                        .ToList();
                    return model1;
                }
                if (UserAccount.RoleNo == AppEnum.enUserRole.Member)
                {
                    List<cvmSaleRank> model2 = db.OrdersDetail
                        .Join(db.Orders, p => p.order_no, d => d.order_no,
                        (p1, d1) => new { p1, d1 })
                        .Where(m => m.d1.order_date >= startDate)
                        .Where(m => m.d1.order_date <= endDate)
                        .Where(m => m.d1.order_validate == 1)
                        .Where(m => m.d1.user_no == UserAccount.UserNo)
                        .GroupBy(m => new { m.p1.product_no, m.p1.product_name })
                        .Select(m => new cvmSaleRank
                        {
                            RankNumber = 0,
                            ProductNo = m.Key.product_no,
                            ProductName = m.Key.product_name,
                            RankValue = m.Sum(x => x.p1.amount)
                        })
                        .OrderByDescending(m => m.RankValue)
                        .ToList();
                    return model2;
                }
                if (UserAccount.RoleNo == AppEnum.enUserRole.Vendor)
                {
                    List<cvmSaleRank> model3 = db.OrdersDetail
                        .Join(db.Orders, p => p.order_no, d => d.order_no,
                        (p1, d1) => new { p1, d1 })
                        .Where(m => m.d1.order_date >= startDate)
                        .Where(m => m.d1.order_date <= endDate)
                        .Where(m => m.d1.order_validate == 1)
                        .Where(m => m.p1.vendor_no == UserAccount.UserNo)
                        .GroupBy(m => new { m.p1.product_no, m.p1.product_name })
                        .Select(m => new cvmSaleRank
                        {
                            RankNumber = 0,
                            ProductNo = m.Key.product_no,
                            ProductName = m.Key.product_name,
                            RankValue = m.Sum(x => x.p1.amount)
                        })
                        .OrderByDescending(m => m.RankValue)
                        .ToList();
                    return model3;
                }
                List<cvmSaleRank> model4 = new List<cvmSaleRank>();
                return model4;
            }
        }
        #endregion
        #region 事件
        /// <summary>
        /// 日期屬性重設
        /// </summary>
        private void DatePropertyReset()
        {
            if (BasePerior == "W")
            {
                BaseFirstDate = BaseDate.ezBaseWeekFirstDate();
                BaseLastDate = BaseDate.ezBaseWeekLastDate();
                PriorFirstDate = BaseDate.ezPriorWeekFirstDate();
                PriorLastDate = BaseDate.ezPriorWeekLastDate();
            }
            if (BasePerior == "M")
            {
                BaseFirstDate = BaseDate.ezBaseMonthFirstDate();
                BaseLastDate = BaseDate.ezBaseMonthLastDate();
                PriorFirstDate = BaseDate.ezPriorMonthFirstDate();
                PriorLastDate = BaseDate.ezPriorMonthLastDate();
            }
            if (BasePerior == "Y")
            {
                BaseFirstDate = BaseDate.ezBaseYearFirstDate();
                BaseLastDate = BaseDate.ezBaseYearLastDate();
                PriorFirstDate = BaseDate.ezPriorYearFirstDate();
                PriorLastDate = BaseDate.ezPriorYearLastDate();
            }
        }
        #endregion
    }
}