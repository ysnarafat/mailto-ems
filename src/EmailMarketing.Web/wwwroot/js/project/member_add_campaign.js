
$(function () {

    $(".display_editor").hide();

    $(".editor_btn").click(function () {
        $(".display_editor").show();
    });

    $('#checkAll').click(function (event) {
        if (this.checked) {
            // Iterate each checkbox
            $(':checkbox.group-select').each(function () {
                this.checked = true;
                $(this).val(this.checked);
            });
        } else {
            $(':checkbox.group-select').each(function () {
                this.checked = false;
                $(this).val(this.checked);
            });
        }
    });

    $('.group-select').change(function () {
        var val = this.checked;
        $(this).val(val);
    });


    //Count selected List
    var selectedElm = document.getElementById('selected');
    function showChecked() {
        var count = document.querySelectorAll('input.group-select:checked').length;
        if (count > 0) selectedElm.innerHTML = count;
        else selectedElm.innerHTML = "No";
    }
    document.querySelectorAll("input").forEach(i => {
        i.onclick = () => showChecked();
    });

    //$("#card_preview").click(function () {
    //    //var id = $("#template_id").val();
    //    //$("#exampleModalLong").val(id);
    //    var text = $("#template_body").html();
    //    $(".modal-body").html(text);
    //});

    $(".selected_template_preview").click(function () {
        var text = $(this).closest('.custom_card_body').find(".template_body").html();
        var title = $(this).closest('.custom_card_body').find(".template_title").text();
        $(".modal-body").html(text);
        $(".modal-title").text(title);
    });

    $(".selected_template").change(function () {
        $(".selected_template").closest(".custom_template_card").removeClass('custom_template_opacity');
        $(".selected_template").closest(".overlay").removeClass('overlay_opacity');

        if ($(this).is(':checked')) {
            $(this).closest(".custom_template_card").addClass('custom_template_opacity');
            $(this).closest(".overlay").addClass('overlay_opacity');
        }
    });

    $(".selected_template_copy").click(function () {
        var text = $(this).closest('.custom_card_body').find(".template_body").html();
        tinyMCE.activeEditor.setContent(text);
    });


    tinymce.init({
        selector: '.editor_textarea',
        height: 440,
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks code fullscreen",
            "insertdatetime media table paste imagetools wordcount"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
        toolbar_mode: 'floating',
        tinycomments_mode: 'embedded',
        tinycomments_author: 'Author name',
    });

    $("#remove_all").click(function () {
        $(".selected_template").prop('checked', false);
        $(".selected_template").closest(".custom_template_card").removeClass('custom_template_opacity');
        $(".selected_template").closest(".overlay").removeClass('overlay_opacity');
    });

    $("#check").click(function () {
        if ($(this).is(":checked")) {
            $("#sendDateTime").attr("disabled", true);
        }
        else {
            $("#sendDateTime").attr("disabled", false);
        }
    });

    //$(document).ready(function () {
    //    $('.editor_textarea').tinymce({
    //        //selector: '.editor_textarea',
    //        height: 440,
    //        plugins: [
    //            "advlist autolink lists link image charmap print preview anchor",
    //            "searchreplace visualblocks code fullscreen",
    //            "insertdatetime media table paste imagetools wordcount"
    //        ],
    //        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
    //        toolbar_mode: 'floating',
    //        tinycomments_mode: 'embedded',
    //        tinycomments_author: 'Author name',
    //    });
    //});

});
