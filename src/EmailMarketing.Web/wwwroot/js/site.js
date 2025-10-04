// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//---Form Progress-----
$("form").on("submit", function () {
    if ($(this).valid()) {
        $.blockUI({
            message: '<i class="icon-spinner4 spinner"></i>',
            timeout: 20000, //unblock after 20 seconds
            overlayCSS: {
                backgroundColor: '#1b2024',
                opacity: 0.8,
                zIndex: 1200,
                cursor: 'wait'
            },
            css: {
                border: 0,
                color: '#fff',
                padding: 0,
                zIndex: 1201,
                backgroundColor: 'transparent'
            }
        });
    }
});
//---Form Progress-----

//-------Jgrowl-Notify---------
Noty.overrideDefaults({
    theme: 'limitless',
    layout: 'topRight',
    type: 'alert',
    timeout: 1500
});