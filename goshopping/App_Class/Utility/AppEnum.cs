using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 枚舉類型類別
/// </summary>
public static class AppEnum
{
    /// <summary>
    /// 使用者角色枚舉類型
    /// </summary>
    public enum enUserRole
    {
        /// <summary>
        /// 會員角色
        /// </summary>
        Member = 0,
        /// <summary>
        /// 管理者角色
        /// </summary>
        Admin = 1,
        /// <summary>
        /// 廠商角色
        /// </summary>
        Vendor = 2,
        /// <summary>
        /// 訪客
        /// </summary>
        Guest = 3
    }
}


