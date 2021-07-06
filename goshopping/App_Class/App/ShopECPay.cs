using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECPay.Payment.Integration;
using goshopping.Models;

namespace DevStudio
{
    /// <summary>
    /// 綠界科技支付
    /// </summary>
    public static class ShopECPay
    {
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public static string ErrorMessage { get; set; }
        /// <summary>
        /// 綠界科技付款
        /// </summary>
        /// <param name="orderID">訂單ID</param>
        public static void Payment(int orderID)
        {
            ErrorMessage = "";
            List<string> enErrors = new List<string>();
            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    using (goshoppingEntities db = new goshoppingEntities())
                    {
                        var order = db.Orders.Where(m => m.rowid == orderID).FirstOrDefault();
                        if (order != null)
                        {
                            string str_home_url = Shop.GetAppConfigValue("ECPayHomeURL");
                            string str_prod_url = "";
                            string str_order_no = order.order_no;
                            string str_total = db.OrdersDetail
                                .Where(m => m.order_no == str_order_no)
                                .Sum(m => m.amount).ToString();

                            /* 服務參數 */
                            //介接服務時，呼叫 API 的方法
                            oPayment.ServiceMethod = HttpMethod.HttpPOST;
                            //要呼叫介接服務的網址
                            //測試環境：https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5
                            //正式環境：https://payment.ecpay.com.tw/Cashier/AioCheckOut/V5
                            oPayment.ServiceURL = Shop.GetAppConfigValue("ECPayServiceURL");
                            //ECPay提供的Hash Key Demo = 5294y06JbISpM5x9
                            oPayment.HashKey = Shop.GetAppConfigValue("ECPayHashKey");
                            //ECPay提供的Hash IV Demo = v77hoKGq4kWxNNIS
                            oPayment.HashIV = Shop.GetAppConfigValue("ECPayHashIV");
                            //ECPay提供的特店編號 Demo = 2000132
                            oPayment.MerchantID = Shop.GetAppConfigValue("ECPayMerchantID");

                            /* 基本參數 */
                            //付款完成通知回傳的網址
                            oPayment.Send.ReturnURL = Shop.GetAppConfigValue("ECPayReturnURL");
                            //瀏覽器端返回的廠商網址
                            oPayment.Send.ClientBackURL = Shop.GetAppConfigValue("ECPayClientBackURL");
                            //瀏覽器端回傳付款結果網址
                            oPayment.Send.OrderResultURL = Shop.GetAppConfigValue("ECPayOrderResultURL");
                            //訂單編號前置碼
                            string ECPayOrderCode = Shop.GetAppConfigValue("ECPayOrderCode");
                            //廠商的交易編號
                            oPayment.Send.MerchantTradeNo = ECPayOrderCode + new Random().Next(0, 99999).ToString();
                            //廠商的交易時間
                            oPayment.Send.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            //交易總金額
                            oPayment.Send.TotalAmount = Decimal.Parse(str_total);
                            //交易描述
                            oPayment.Send.TradeDesc = "線上購物付款";
                            //使用的付款方式
                            oPayment.Send.ChoosePayment = PaymentMethod.ALL;
                            //備註欄位
                            oPayment.Send.Remark = "";
                            //使用的付款子項目
                            oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;
                            //是否需要額外的付款資訊
                            oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.Yes;
                            //來源裝置
                            oPayment.Send.DeviceSource = DeviceType.PC;
                            //不顯示的付款方式
                            oPayment.Send.IgnorePayment = "";
                            //特約合作平台商代號
                            oPayment.Send.PlatformID = "";
                            oPayment.Send.CustomField1 = "";
                            oPayment.Send.CustomField2 = "";
                            oPayment.Send.CustomField3 = "";
                            oPayment.Send.CustomField4 = "";
                            oPayment.Send.EncryptType = 1;

                            //訂單的商品資料
                            var detail = db.OrdersDetail
                                .Where(m => m.order_no == str_order_no)
                                .ToList();
                            if (detail != null && detail.Count() > 0)
                            {
                                foreach (var item in detail)
                                {
                                    str_prod_url = string.Format("{0}/Product/ProductDetail/{1}", str_home_url, item.product_no); 
                                    oPayment.Send.Items.Add(new Item()
                                    {
                                        //商品名稱
                                        Name = (item.product_name + item.product_spec),
                                        //商品單價
                                        Price = Decimal.Parse(item.price.ToString()),
                                        //幣別單位
                                        Currency = "新台幣",
                                        //購買數量
                                        Quantity = Int32.Parse(item.qty.ToString()),
                                        //商品的說明網址
                                        URL = str_prod_url
                                    });
                                }
                            }

                            /*************************非即時性付款:ATM、CVS 額外功能參數**************/

                            #region ATM 額外功能參數

                            //oPayment.SendExtend.ExpireDate = 3;//允許繳費的有效天數
                            //oPayment.SendExtend.PaymentInfoURL = "";//伺服器端回傳付款相關資訊
                            //oPayment.SendExtend.ClientRedirectURL = "";//Client 端回傳付款相關資訊

                            #endregion


                            #region CVS 額外功能參數

                            //oPayment.SendExtend.StoreExpireDate = 3; //超商繳費截止時間 CVS:以分鐘為單位 BARCODE:以天為單位
                            //oPayment.SendExtend.Desc_1 = "test1";//交易描述 1
                            //oPayment.SendExtend.Desc_2 = "test2";//交易描述 2
                            //oPayment.SendExtend.Desc_3 = "test3";//交易描述 3
                            //oPayment.SendExtend.Desc_4 = "";//交易描述 4
                            //oPayment.SendExtend.PaymentInfoURL = "";//伺服器端回傳付款相關資訊
                            //oPayment.SendExtend.ClientRedirectURL = "";///Client 端回傳付款相關資訊

                            #endregion

                            /***************************信用卡額外功能參數***************************/

                            #region Credit 功能參數

                            //oPayment.SendExtend.BindingCard = BindingCardType.No; //記憶卡號
                            //oPayment.SendExtend.MerchantMemberID = ""; //記憶卡號識別碼
                            //oPayment.SendExtend.Language = ""; //語系設定

                            #endregion Credit 功能參數

                            #region 一次付清

                            //oPayment.SendExtend.Redeem = false;   //是否使用紅利折抵
                            //oPayment.SendExtend.UnionPay = true; //是否為銀聯卡交易

                            #endregion

                            #region 分期付款

                            //oPayment.SendExtend.CreditInstallment = "3,6";//刷卡分期期數

                            #endregion 分期付款

                            #region 定期定額

                            //oPayment.SendExtend.PeriodAmount = 1000;//每次授權金額
                            //oPayment.SendExtend.PeriodType = PeriodType.Day;//週期種類
                            //oPayment.SendExtend.Frequency = 1;//執行頻率
                            //oPayment.SendExtend.ExecTimes = 2;//執行次數
                            //oPayment.SendExtend.PeriodReturnURL = "";//伺服器端回傳定期定額的執行結果網址。

                            #endregion

                            /* 產生訂單 */
                            enErrors.AddRange(oPayment.CheckOut());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
            }
            finally
            {
                // 顯示錯誤訊息。
                if (enErrors.Count() > 0)
                {
                    ErrorMessage = String.Join("\\r\\n", enErrors);
                }
            }
        }
    }
}