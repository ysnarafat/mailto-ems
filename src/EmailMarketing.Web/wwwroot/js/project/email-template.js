
$(function () {

    $(".selected_template_preview").click(function () {
        var text = $(this).closest('.custom_card_body').find(".template_body").html();
        var title = $(this).closest('.custom_card_body').find(".template_title").text();
        $(".modal-body").html(text);
        $(".modal-title").text(title);
    });

});
