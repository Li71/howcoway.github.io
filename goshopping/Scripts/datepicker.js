$(function () {
    //全域性設定日期選擇外掛的引數.
    $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);
    $(".datepicker").datepicker(
        {
            //設定允許通過下拉框列表選取月份。
            changeMonth: true,
            //設定允許通過下拉框列表選取年份。
            changeYear: true,
            //是否在面板的頭部年份後面顯示月份
            showMonthAfterYear: true,
            //是否在當前面板顯示上、下兩個月的一些日期數
            showOtherMonths: true,
            //設定是否在面板上顯示相關的按鈕
            showButtonPanel: true,
            //如果設定為true，則約束當前輸入的日期格式。
            constrainInput: true,
            //設定什麼事件觸發顯示日期外掛的面板，可選值：focus, button, both
            showOn: 'focus',
            //設定日期控制元件展開動畫的顯示時間，可選是”slow”, “normal”, “fast”，”代表立刻，數字代表毫秒數。
            duration: 'fast',
            //設定顯示、隱藏日期外掛的動畫的名稱 show, fold
            showAnim: 'fold',
            //如果使用showAnim來顯示動畫效果的話，可以通過此引數來增加一些附加的引數設定
            showOptions: { direction: 'up' },
            //設定日期格式
            dateFormat: 'yy/mm/dd'
        });
    //鍵盤操作：
    //page up/down - 上一月、下一月
    //ctrl page up/down - 上一年、下一年
    //ctrl home - 當前月或最後一次開啟的日期
    //ctrl left/right - 上一天、下一天
    //ctrl up/down - 上一週、下一週
    //enter - 確定選擇日期
    //ctrl end - 關閉並清除已選擇的日期
    //escape - 關閉並取消選擇
    //----------------------------------------------
    //日期格式：
    //d - 每月的第幾天 (沒有前導零)
    //dd - 每月的第幾天(兩位數字)
    //o - 一年中的第幾天(沒有前導零)
    //oo - 一年中的第幾天(三位數字)
    //D - day name short
    //DD - day name long
    //m - 月份(沒有前導零)
    //mm - 月份(兩位數字)
    //M - month name short
    //MM - month name long
    //y - 年份(兩位數字)
    //yy - 年份(四位數字)
});