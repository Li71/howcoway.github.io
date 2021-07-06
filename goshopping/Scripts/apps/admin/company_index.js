$(document).ready(function () {
    var oTable = $('#DatatableList').DataTable({
        "ajax": {
            "url": '/Admin/Company/GetDataList',
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
                    return '<a class="popup"  title="修改記錄" href="/Admin/Company/Edit/' + data + '"><i class="fas fa-edit fa-2x"></i></a>' +
                        '<a class="popup" title="刪除記錄" href="/Admin/Company/Delete/' + data + '"><i class="fas fa-trash-alt fa-2x"></i></a>';
                }
            },
            { "data": "mno", "width": "50px" },
            { "data": "msname", "width": "100px" },
            { "data": "mname", "width": "100px" },
            {
                "data": "date_register", "width": "60px", "title": "登記日期",
                "render": function (data) { return moment(data).format("YYYY/MM/DD"); }
            },
            { "data": "tel_company", "width": "100px" },
            { "data": "fax_company", "width": "100px" },
            { "data": "name_charge", "width": "100px" },
            { "data": "name_contact", "width": "100px" },
            { "data": "tel_contact", "width": "100px" },
            { "data": "adr_company", "width": "100px" },
            { "data": "email_company", "width": "100px" },
            { "data": "url_company", "width": "100px" },
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
                title: '編輯視窗',
                height: 600,
                width: 500,
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