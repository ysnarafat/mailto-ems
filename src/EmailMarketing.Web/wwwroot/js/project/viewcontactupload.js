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
            targets: [5]
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
                'searchable': false,
                "orderData": [0]
            },
            {
                "targets": [1],
                'sortable': true,
                'searchable': false,
                "orderData": [1]
               
            }
            
        ]
    });
}