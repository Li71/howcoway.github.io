using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace DevStudio.Securitys
{
    /// <summary>
    /// AES 演算法為字串加密解密
    /// </summary>
    public class Cryptographys : IDisposable
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
        /// ezApplication 解構子
        /// </summary>
        ~Cryptographys()
        {
            //必須為false
            Dispose(false);
        }
        #endregion
        #region 建構子
        public Cryptographys()
        {
            ErrorMessage = "";
        }
        #endregion
        #region 屬性
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }
        #endregion
        #region 函數
        /// <summary>
        /// MD5 加密法(不可逆)
        /// </summary>
        /// <param name="source">加密文字字串</param>
        /// <returns>
        /// 值:abcdefg
        /// 加密:esZsDxSN6VGbi9JkMSxNZA==
        /// </returns>
        public string MD5Encode(string source)
        {
            ErrorMessage = "";
            string str_string = "";
            try
            {
                //建立一個MD5
                MD5 md5 = MD5.Create();
                //將字串轉為Byte[]
                byte[] bsource = Encoding.UTF8.GetBytes(source);
                //進行MD5加密
                byte[] crypto = md5.ComputeHash(bsource);
                //把加密後的字串從Byte[]轉為字串
                str_string = Convert.ToBase64String(crypto);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return str_string;
        }
        /// <summary>
        /// SHA256 加密法(不可逆)
        /// </summary>
        /// <param name="source">加密文字字串</param>
        /// <returns>
        /// 值:abcdefg
        /// 加密:fRpUEnsiJQL1t5tfsIAwYRUqRPkrN+I8ZSe69mXU2po=
        /// </returns>
        public string SHA256Encode(string source)
        {
            ErrorMessage = "";
            string str_string = "";
            try
            {
                //建立一個 SHA256
                SHA256 sha = SHA256.Create();
                //將字串轉為Byte[]
                byte[] bsource = Encoding.UTF8.GetBytes(source);
                //進行 SHA256 加密
                byte[] crypto = sha.ComputeHash(bsource);
                //把加密後的字串從Byte[]轉為字串
                str_string = Convert.ToBase64String(crypto);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return str_string;
        }
        #endregion
    }
}