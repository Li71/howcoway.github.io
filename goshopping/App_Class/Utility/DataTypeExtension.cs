using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static partial class Extension
{
    /// <summary>
    /// 取得當週週一的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime ezBaseWeekFirstDate(this DateTime dateBase)
    {
        int int_week = (int)dateBase.DayOfWeek;
        if (int_week == 0) int_week = 7;
        return dateBase.AddDays(1 - int_week);
    }

    /// <summary>
    /// 取得當週週日的日期(週一為星期第一天,週日星期最後一天)
    /// </summary>
    /// <returns></returns>
    public static DateTime ezBaseWeekLastDate(this DateTime dateBase)
    {
        int int_week = (int)dateBase.DayOfWeek;
        if (int_week == 0) int_week = 7;
        return dateBase.AddDays(7 - int_week);
    }

    /// <summary>
    /// 取得前月月初的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime ezPriorWeekFirstDate(this DateTime dateBase)
    {
        int int_week = (int)dateBase.DayOfWeek;
        if (int_week == 0) int_week = 7;
        return dateBase.AddDays(1 - int_week - 7);
    }

    /// <summary>
    /// 取得前月月底的日期(週一為星期第一天,週日星期最後一天)
    /// </summary>
    /// <returns></returns>
    public static DateTime ezPriorWeekLastDate(this DateTime dateBase)
    {
        int int_week = (int)dateBase.DayOfWeek;
        if (int_week == 0) int_week = 7;
        return dateBase.AddDays(7 - int_week - 7);
    }

    /// <summary>
    /// 取得當月月初的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime ezBaseMonthFirstDate(this DateTime dateBase)
    { return dateBase.AddDays(1 - dateBase.Day); }

    /// <summary>
    /// 取得當月月底的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime ezBaseMonthLastDate(this DateTime dateBase)
    { return dateBase.ezBaseMonthFirstDate().AddMonths(1).AddDays(-1); }

    /// <summary>
    /// 取得前月月初的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime ezPriorMonthFirstDate(this DateTime dateBase)
    {
        DateTime dtm_date = dateBase.ezBaseMonthFirstDate().AddDays(-1);
        dtm_date = dtm_date.AddDays(1 - dtm_date.Day);
        return dtm_date;
    }

    /// <summary>
    /// 取得前月月底的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime ezPriorMonthLastDate(this DateTime dateBase)
    {
        DateTime dtm_date = dateBase.ezBaseMonthFirstDate().AddDays(-1);
        return dtm_date;
    }

    /// <summary>
    /// 取得當年年初的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime ezBaseYearFirstDate(this DateTime dateBase)
    { return DateTime.Parse(string.Format("{0}/01/01", dateBase.Year)); }

    /// <summary>
    /// 取得當年年底的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime ezBaseYearLastDate(this DateTime dateBase)
    { return DateTime.Parse(string.Format("{0}/12/31", dateBase.Year)); }

    /// <summary>
    /// 取得前年年初的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime ezPriorYearFirstDate(this DateTime dateBase)
    { return DateTime.Parse(string.Format("{0}/01/01", (dateBase.Year - 1))); }

    /// <summary>
    /// 取得前年年底的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime ezPriorYearLastDate(this DateTime dateBase)
    { return DateTime.Parse(string.Format("{0}/12/31", (dateBase.Year - 1))); }

    /// <summary>
    /// 取得日期的最小值,不含時間
    /// </summary>
    /// <param name="dateBase"></param>
    /// <returns></returns>
    public static DateTime ezMinDateTime(this DateTime dateBase)
    { return DateTime.Parse(dateBase.ToString("yyyy-MM-dd") + " 00:00:00"); }

    /// <summary>
    /// 取得日期的最小值,最時間
    /// </summary>
    /// <param name="dateBase"></param>
    /// <returns></returns>
    public static DateTime ezMaxDateTime(this DateTime dateBase)
    { return DateTime.Parse(dateBase.ToString("yyyy-MM-dd") + " 23:59:59"); }

    /// <summary>
    /// 取得月份的天數
    /// </summary>
    /// <param name="dateBase"></param>
    /// <returns></returns>
    public static int ezMonthDays(this DateTime dateBase)
    {
        return System.DateTime.DaysInMonth(dateBase.Year, dateBase.Month);
    }
}
