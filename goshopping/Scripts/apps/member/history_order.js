$(document).ready(function () {
    var oTable = $('#DatatableList').DataTable({
        "ajax": {
            "url": '/Member/Orders/GetDataList',
            "type": "get",
            "datatype": "json"
        },
        "language": {
            "sProcessing": "處理中...",
            "sLengthMenu": "顯示 _MENU_ 項結果",
            "sZeroRecords": "沒有匹配結果",
            "sInfo": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
            "sInfoEmpty": "顯示第 0 至 0 項結果，共 0 項",
            "sInfoFiltered": "(從 _MAX_ 項結果過濾)",
            "sInfoPostFix": "",
            "sSearch": "查詢:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "首頁",
                "sPrevious": "上頁",
                "sNext": "下頁",
                "sLast": "尾頁"
            }
        },
        "columns": [
            {
                "data": "rowid", "width": "30px", "orderable": false, "render": function (data) {
                    return '<a class="popup"  title="訂單明細" href="/Member/Orders/Details/' + data + '"><i class="fas fa-edit fa-2x"></i></a>';
                }
            },
            { "data": "order_no", "width": "60px" },
            {
                "data": "order_date", "width": "60px", "title": "訂單日期",
                "render": function (data) { return moment(data).format("YYYY/MM/DD"); }
            },
            { "data": "status_name", "width": "60px" },
            { "data": "payment_name", "width": "60px" },
            { "data": "shipping_name", "width": "60px" },
            { "data": "receive_name", "width": "60px" },
            { "data": "receive_address", "width": "100px" },
            { "data": "remark", "autoWidth": true }
        ]
    })
    $('.tablecontainer').on('click', 'a.popup', function (e) {
        e.preventDefault();
        OpenPopup($(this).attr('href'));
    })
    function OpenPopup(pageUrl) {
        var $pageContent = $('<div/>');
        $pageContent.load(pageUrl, function () {
            $('#popupForm', $pageContent).removeData('validator');
            $('#popupForm', $pageContent).removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse('form');

        });

        $dialog = $('<div class="popupWindow" style="overflow:auto"></div>')
            .html($pageContent)
            .dialog({
                draggable: true,
                autoOpen: false,
                resizable: true,
                model: true,
                title: '查詢視窗',
                height: 650,
                width: 800,
                close: function () {
                    $dialog.dialog('destroy').remove();
                }
            })

        $('.popupWindow').on('submit', '#popupForm', function (e) {
            var url = $('#popupForm')[0].action;
            $.ajax({
                type: "POST",
                url: url,
                data: $('#popupForm').serialize(),
                success: function (data) {
                    if (data.status) {
                        $dialog.dialog('close');
                        oTable.ajax.reload();
                    }
                }
            })
            e.preventDefault();
        })
        $dialog.dialog('open');
    }
})