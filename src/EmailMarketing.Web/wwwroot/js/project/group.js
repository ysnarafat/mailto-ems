
function loadDatatable(url) {

    if (!$().DataTable) {
        console.warn('Warning - datatables.min.js is not loaded.');
        return;
    }

    $.extend($.fn.dataTable.defaults, {
        autoWidth: false,
        columnDefs: [{
            orderable: false,
            width: 100,
            targets: [3]
        }],
        dom: '<"datatable-header"fl><"datatable-scroll"t><"datatable-footer"ip>',
        language: {
            search: '<span>Filter:</span> _INPUT_',
            searchPlaceholder: 'Type to filter...',
            lengthMenu: '<span>Show:</span> _MENU_',
            paginate: { 'first': 'First', 'last': 'Last', 'next': $('html').attr('dir') == 'rtl' ? '&larr;' : '&rarr;', 'previous': $('html').attr('dir') == 'rtl' ? '&rarr;' : '&larr;' }
        }
    });

    $('#group-table').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": url,
        "order": [[0, "asc"]],
        "columnDefs": [
            {
                "targets": [0],
                'sortable': true,
                'searchable': false,
                "orderData": [0]
            },
            {
                "targets": [1],
                'sortable': false,
                'searchable': false,
                "orderData": [1],
                "render": function (data, type, row, meta) {
                    var lbl = data == "No" ? "badge-danger" : "badge-success";
                    return '<span class="badge  ' + lbl + '">' + data + '</span>';
                }
            },
            {
                "targets": [2],
                'sortable': false,
                'searchable': false,
                "width": "15%",
                "className": "text-center",
                "render": function (data, type, row, meta) {

                    var editButton = '<a class="text-primary" onclick="add_edit_Data(' + data + ')" href="#" title="Edit">' +
                        '<i class="icon-pencil7"></i></a>';

                    var activeButton = '<a class="text-danger" data-toggle="modal" data-target="#modal-activeField" data-id="' + data + '" data-title="' + row[1] + '" href="#" title="Active/InActive">' +
                        '<i class="icon-blocked"></i></a>';

                    return editButton + "  " + activeButton;
                }
            }
        ]
    });
}


//function loadDatatable(url, editUrl) {

//    if (!$().DataTable) {
//        console.warn('Warning - datatables.min.js is not loaded.');
//        return;
//    }

//    $.extend($.fn.dataTable.defaults, {
//        autoWidth: false,
//        columnDefs: [{
//            orderable: false,
//            width: 100,
//            targets: [1]
//        }],
//        dom: '<"datatable-header"fl><"datatable-scroll"t><"datatable-footer"ip>',
//        language: {
//            search: '<span>Filter:</span> _INPUT_',
//            searchPlaceholder: 'Type to filter...',
//            lengthMenu: '<span>Show:</span> _MENU_',
//            paginate: { 'first': 'First', 'last': 'Last', 'next': $('html').attr('dir') == 'rtl' ? '&larr;' : '&rarr;', 'previous': $('html').attr('dir') == 'rtl' ? '&rarr;' : '&larr;' }
//        }
//    });

//    $('#group-table').DataTable({
//        "processing": true,
//        "serverSide": true,
//        "ajax": url,
//        "order": [[0, "asc"]],
//        "columnDefs": [
//            {
//                "targets": [0],
//                'sortable': true,
//                'searchable': true,
//                "orderData": [0]
//            },

//            {
//                "targets": [1],
//                'sortable': false,
//                'searchable': false,
//                "width": "15%",
//                "className": "text-center",
//                "render": function (data, type, row, meta) {
//                    var editButton = '<a class="text-primary" href="' + editUrl + '/'+ data +'" title="Edit">' +
//                        '<i class="icon-pencil7"></i></a>';

//                    var deleteButton = '<a class="text-danger show-bs-modal" data-id="'+ data +'" href="#" title="Delete">' +
//                        '<i class="icon-trash"></i></a>';

//                    return editButton + ' ' + deleteButton;
//                }
//            }
//        ]
//    });
//}