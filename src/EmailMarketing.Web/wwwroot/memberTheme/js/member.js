$(function() {
    //Show Hide
    $('#showHide').click(function () {
        $(this).text(function (i, old) {
            return old == 'Show' ? 'Hide' : 'Show';
        });
    });

    $('#showHideCustom').click(function () {
        $(this).text(function (i, old) {
            return old == 'Show' ? 'Hide' : 'Show';
        });
    });
    //Show Hide
});
