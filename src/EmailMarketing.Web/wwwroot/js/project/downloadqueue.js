
function loadDatatable(url, downloadUrl, isProcessing) {

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

    $('#downloadqueue-table').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": url,
        "order": [[0, "desc"]],
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
                "orderData": [1]
            },
            {
                "targets": [2],
                'sortable': false,
                'searchable': false,
                "orderData": [2],
                "render": function (data, type, row, meta) {
                    isProcessing = (data == "Processing" ? true : false);
                    var lbl = data == "Processing" ? "badge-warning" : "badge-success";
                    return '<span class="badge  ' + lbl + '">' + data + '</span>';
                }
            },
            {
                "targets": [3],
                'sortable': false,
                'searchable': true,
                "orderData": [3],
                "render": function (data, type, row, meta) {
                    var lbl = data == "No" ? "badge-danger" : "badge-success";
                    return '<span class="badge  ' + lbl + '">' + data + '</span>';
                }
            },
            {
                "targets": [4],
                'sortable': false,
                'searchable': true,
                "orderData": [4],
                "render": function (data, type, row, meta) {
                    var lbl = data == "No" ? "badge-danger" : "badge-success";
                    return '<span class="badge  ' + lbl + '">' + data + '</span>';
                }
            },
            {
                "targets": [5],
                'sortable': false,
                'searchable': false,
                "width": "15%",
                "className": "text-center",
                "render": function (data, type, row, meta) {

                    if (isProcessing) {
                        var downloadButton = '<a id="download" class="text-info" href="' + downloadUrl + '/' + data + '" title="Download">' +
                            '<i class="fa fa-download"></i></a>';
                    }
                    else {
                        var downloadButton = '<a id="download" class="text-primary" href="' + downloadUrl + '/' + data + '" title="Download">' +
                            '<i class="fa fa-download"></i></a>';
                    }

                    var style = document.createElement('style');
                    style.innerHTML = `
                            .text-info {
                            pointer-events: none;
                            opacity: 0.5;
                                }
                            `;
                    document.head.appendChild(style);

                    return downloadButton;
                }
            }
        ]
    });



}
