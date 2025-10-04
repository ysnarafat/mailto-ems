
function loadDatatable(url, editUrl, userInformationUrl) {

    if (!$().DataTable) {
        console.warn('Warning - datatables.min.js is not loaded.');
        return;
    }

    $.extend($.fn.dataTable.defaults, {
        autoWidth: false,
        columnDefs: [{
            orderable: false,
            width: 100,
            targets: [6]
        }],
        dom: '<"datatable-header"fl><"datatable-scroll"t><"datatable-footer"ip>',
        language: {
            search: '<span>Filter:</span> _INPUT_',
            searchPlaceholder: 'Type to filter...',
            lengthMenu: '<span>Show:</span> _MENU_',
            paginate: { 'first': 'First', 'last': 'Last', 'next': $('html').attr('dir') == 'rtl' ? '&larr;' : '&rarr;', 'previous': $('html').attr('dir') == 'rtl' ? '&rarr;' : '&larr;' }
        }
    });

    $('#user-table').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": url,
        "order": [[0, "asc"]],
        "columnDefs": [
            //{
            //    "targets": [0],
            //    'sortable': true,
            //    'searchable': true,
            //    "orderData": [0]
            //},
            {
                "targets": [0],
                'sortable': true,
                'searchable': false,
                "orderData": [0]
            },
            {
                "targets": [1],
                'sortable': true,
                'searchable': false,
                "orderData": [1]
            },
            {
                "targets": [2],
                'sortable': true,
                'searchable': false,
                "orderData": [2],
                "render": function (data, type, row, meta) {
                    var lbl = data == "Yes" ? "badge-success" : "badge-danger";
                    return '<span class="badge  ' + lbl + '">' + data + '</span>';
                }
            }
            ,
            {
                "targets": [3],
                'sortable': true,
                'searchable': false,
                "orderData": [3]
            }
            ,
            {
                "targets": [4],
                'sortable': true,
                'searchable': false,
                "orderData": [4],
                "render": function (data, type, row, meta) {
                    var lbl = data == "Yes" ? "badge-danger" : "badge-success";
                    return '<span class="badge  ' + lbl + '">' + data + '</span>';
                }
            }
            ,
            {
                "targets": [5],
                'sortable': false,
                'searchable': false,
                "width": "15%",
                "className": "text-center",
                "render": function (data, type, row, meta) {
                    var editButton = '<a class="text-primary" href="' + editUrl + '/' + data + '" title="Edit">' +
                        '<i class="icon-pencil7"></i></a>';

                    var deleteButton = '<a class="text-danger" data-toggle="modal" data-target="#modal-delete" data-id="' + data + '" href="#" title="Delete">' +
                        '<i class="icon-trash"></i></a>';

                    var blockButton = '<a class="text-primary" data-toggle="modal" data-target="#modal-blockUser" data-id="' + data + '" data-title="' + row[4] + '" href="#" title="Block/Unblock">' +
                        '<i class="icon-user-block"></i></a>';

                    var resetPasswordButton = '<a class="text-danger" data-toggle="modal" data-target="#modal-resetUserPassword" data-id="' + data + '" href="#" title="Reset password">' +
                        '<i class="icon-key"></i></a>';
                    var userInfoButton = '<a class="text-info" href="' + userInformationUrl + '/' + data + '" title="User Information">' +
                        '<i class="icon-info22"></i></a>';

                    return editButton + ' ' + deleteButton + ' ' + blockButton + ' ' + resetPasswordButton + ' ' + userInfoButton;
                }
            }
        ]
    });
}