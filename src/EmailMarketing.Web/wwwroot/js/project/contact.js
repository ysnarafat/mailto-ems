function loadDatatable(url, editUrl, contactDetailsUrl) {

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

    $('#contact-table').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": url,
        "order": [[1, "asc"]],
        "columnDefs": [
            {
                "targets": [0],
                'sortable': true,
                'searchable': false,
                "orderData": [0]
            },
            {
                "targets": [1],
                'sortable': true,
                'searchable': true,
                "orderData": [1]
            },
            {
                "targets": [2],
                'sortable': false,
                'searchable': false,
                "width": "15%",
                "className": "text-center",
                "render": function (data, type, row, meta) {
                    var contactDetailsButton = '<a class="text-info" href="' + contactDetailsUrl + '/' + data + '" title="Contact Details">' +
                        '<i class="icon-info22"></i></a>';

                    var editButton = '<a class="text-primary" href="' + editUrl + '/' + data + '" title="Edit">' +
                        '<i class="icon-pencil7"></i></a>';

                    var deleteButton = '<a class="text-danger" data-toggle="modal" data-target="#modal-delete" data-id="' + data + '" href="#" title="Delete">' +
                        '<i class="icon-trash"></i></a>';

                    return contactDetailsButton + ' ' + editButton + ' ' + deleteButton;
                }
            }
        ]
    });
}