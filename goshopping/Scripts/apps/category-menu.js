$(function () {
    $.ajax({
        type: 'get',
        url: "/Home/GetCategoryMenuList",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            $('#tree').bstreeview({
                data: result,
                expandIcon: 'fa fa-angle-down fa-fw',
                collapseIcon: 'fa fa-angle-right fa-fw',
                indent: 1.25,
                parentsMarginLeft: '1.25rem',
                openNodeLinkOnNewTab: true
            });
        },
        error: function (error) {
            alert("樹形結構載入失敗！");
        }
    });
});