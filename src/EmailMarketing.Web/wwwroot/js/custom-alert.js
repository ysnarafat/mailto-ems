

    // Defaults
    var swalInit = swal.mixin({
        buttonsStyling: false,
        confirmButtonClass: 'btn btn-primary',
        cancelButtonClass: 'btn btn-light'
    });


    //
    // Basic options
    //

    function delete_confirm(url, paramData) {
        swalInit.fire({
            title: 'Are you sure want to delete?',
            type: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Confirm'
        }).then(function (result) {
            if (result.value) {
                ajaxCall(url, paramData, "renderDeleteItem");
            }
        });

    }

  
