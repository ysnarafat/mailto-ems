
function ajaxCall(url, paramData, callback, method, obj) {
    method = method == null ? "POST" : method;
    console.log(url, paramData, callback, method);
    $.ajax({
        type: method,
        url: url,
        data: paramData,
        dataType: "json",
        success: function (response) {
            if (callback == 'renderDeleteItem') {
                renderDeleteItem(response);
            }
            else if (callback == 'renderGetAllFieldMapsForUploadContact') {
                renderGetAllFieldMapsForUploadContact(response);
            }    
            else if (callback == 'renderGetAllGroupsForUploadContact') {
                renderGetAllGroupsForUploadContact(response);
            }  
            else if (callback == 'renderSmtpTest') {
                renderSmtpTest(response);
            }  
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + "! please try again");
        }
    });
}

// for Modal...
function ajaxCallModal(url, paramData, callback, method, obj) {
    method = method == null ? "POST" : method;
    $.ajax({
        type: method,
        url: url,
        data: paramData,
        dataType: "html",
        success: function (response) {
            if (callback == 'renderStudentAddOrEditLoad') {
                renderStudentAddOrEditLoad(response);
            }
            else if (callback == 'renderCourseAddOrEditLoad') {
                renderCourseAddOrEditLoad(response);
            } 
            else if (callback == 'renderStudentRegistrationAddOrEditLoad') {
                renderStudentRegistrationAddOrEditLoad(response);
            }   
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + "! please try again");
        }
    });
}

// render method //
function renderDeleteItem(data) {
    swalInit.fire({
        title: 'Deleted!!!',
        text: 'Your data has been deleted.',
        type: 'success'
    }).then(function (result) {
        if (result.value) {
            location.reload();
        }
    });
}


