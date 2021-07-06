using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

public class GmailService : IDisposable
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
    /// GmailService 解構子
    /// </summary>
    ~GmailService()
    {
        //必須為false
        Dispose(false);
    }
    #endregion
    #region 建構子
    public GmailService()
    {
        MessageText = "";
        SenderName = "網站管理員";
        SenderEmail = "johnson.ezmail@gmail.com";
        AppPassword = "wrieitvreamewrbx";
        ReceiveEmail = "";
        Subject = "";
        Body = "";
        HostUrl = "smtp.gmail.com";
        HostPort = 587;
        UseSSL = true;
    }
    #endregion
    #region 屬性
    /// <summary>
    /// 訊息文字
    /// </summary>
    public string MessageText { get; set; }
    /// <summary>
    /// 寄件者姓名
    /// </summary>
    public string SenderName { get; set; }
    /// <summary>
    /// 寄件者 (Google 帳號)
    /// </summary>
    public string SenderEmail { get; set; }
    /// <summary>
    /// Google 帳號應用程式密碼
    /// </summary>
    public string AppPassword { get; set; }
    /// <summary>
    /// 收件者 Email
    /// </summary>
    public string ReceiveEmail { get; set; }
    /// <summary>
    /// 信件主旨
    /// </summary>
    public string Subject { get; set; }
    /// <summary>
    /// 信件內文
    /// </summary>
    public string Body { get; set; }
    /// <summary>
    /// 寄件伺服器位址
    /// </summary>
    public string HostUrl { get; set; }
    /// <summary>
    /// 通訊連接埠號碼
    /// </summary>
    public int HostPort { get; set; }
    /// <summary>
    /// 啟用 SSL 機制
    /// </summary>
    public bool UseSSL { get; set; }
    #endregion
    #region 事件
    /// <summary>
    /// 送出信件
    /// </summary>
    public void Send()
    {
        var fromEmail = new MailAddress(SenderEmail, SenderName);
        var toEmail = new MailAddress(ReceiveEmail);
        try
        {
            var smtp = new SmtpClient
            {
                Host = HostUrl,
                Port = HostPort,
                EnableSsl = UseSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, AppPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        catch (Exception ex)
        {
            MessageText = ex.Message;
        }
    }
    #endregion
}
