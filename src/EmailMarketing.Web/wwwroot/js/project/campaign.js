
function loadDatatable(url,ReportUrl) {

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

    $('#campaign-table').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": url,
        "order": [[0, "asc"]],
        "columnDefs": [
            {
                "targets": [0],
                'sortable': true,
                'searchable': true,
                "orderData": [0]
            },
            {
                "targets": [1],
                'sortable': false,
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
            },
            {
                "targets": [3],
                'sortable': true,
                'searchable': false,
                "orderData": [3],
                "render": function (data, type, row, meta) {
                    var lbl = data == "Finished" ? "badge-success" : "badge-warning";
                    return '<span class="badge  ' + lbl + '">' + data + '</span>';
                }
            },
            {
                "targets": [4],
                'sortable': true,
                'searchable': false,
                "orderData": [4],
                "render": function (data, type, row, meta) {
                    var lbl = data == "Yes" ? "badge-success" : "badge-danger";
                    return '<span class="badge  ' + lbl + '">' + data + '</span>';
                }
            },
            {
                "targets": [5],
                'sortable': true,
                'searchable': false,
                "orderData": [5]
            }
            ,
            {
                "targets": [6],
                'sortable': true,
                'searchable': false,
                "orderData": [6]
            },
            {
                "targets": [7],
                'sortable': false,
                'searchable': false,
                "width": "15%",
                "className": "text-center",
                "render": function (data, type, row, meta) {
                    var reportButton = '<a class="text-primary" href="' + ReportUrl + '/' + data + '" title="Details">' +
                        '<i class="icon-info22"></i></a>';

                    var draftButton = '<a class="text-danger" data-toggle="modal" data-target="#modal-activeDraft" data-id="' + data + '" data-title="' + row[3] + '" href="#" title="Finish/Process">' +
                        '<i class="icon-blocked"></i></a>';

                    return reportButton + ' ' + draftButton;
                }
            }
         
        ]
    });
}