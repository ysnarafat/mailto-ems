

var mapFieldDropdownData = [];
var groupDropdownData = [];
var mapFieldTableColumnHeaderData = [];
var mapFieldTableRowData = [];
var hasColumnHeader = true;

$(function () {
    $("#uploadFileDiv").on('click', function(e) {
        e.preventDefault();
        $("#uploadFile").trigger('click');
    });

    //$("#finishBtn").on('click', function(e) {
    //    //e.preventDefault();
    //    $('#submit_form').submit();
    //    $("#exampleModalCenter").modal('show');
    //});

    $("#uploadFile").on('change', function (e) {
        var file = e.target.files[0];
        var isFileSelected = false;
        var fileName = 'Recommended file formats: .xls, .xlsx';
        var exfileName = 'FileName.xlsx';

        if (file !== null && file !== undefined) {
            fileName = 'The file "' + file.name + '" has been selected.';
            exfileName = file.name;
            isFileSelected = file.size > 0;
            parseExcel(file);
        }

        $("#uploadFileName").text(fileName);
        $("#reviewFileName").text(exfileName);
        document.getElementById('nextBtn').disabled = !isFileSelected;
    });

    

    $('#hasColumnHeader').on('change', function (e) {
        var has = $(this).is(":checked");
        generateColumnHeaderDisable(has);
    });

    $('#selectAllGroups').on('change', function (e) {
        changeGroupSelect(this,true);
    });
});

function renderGetAllFieldMapsForUploadContact(response) {
    console.log(response);
    mapFieldDropdownData = response;
    console.log(mapFieldDropdownData);
    callGroupFromServer();
}

function renderGetAllGroupsForUploadContact(response) {
    console.log(response);
    groupDropdownData = response;
    console.log(groupDropdownData);
    generateGroupData();
}


//var ExcelToJSON = function () {

    this.parseExcel = function (file) {
        var reader = new FileReader();

        reader.onload = function (e) {
            var data = e.target.result;
            var workbook = XLSX.read(data, {
                type: 'binary'
            });

            workbook.SheetNames.forEach(function (sheetName, index) {
                if (index == 0) {
                    // Here is your object
                    var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
                    var json_object = JSON.stringify(XL_row_object);
                    console.log(json_object);
                    //console.log(Object.keys(json_object).length);
                    //Object.keys(json_object).forEach(function (val, index) { console.log(val) });
                    var tableColHdr = [];
                    for (var key in XL_row_object[0]) {
                        console.log(key);
                        console.log(XL_row_object[0][key]);
                        tableColHdr.push(key);
                    }

                    mapFieldTableColumnHeaderData = tableColHdr;
                    mapFieldTableRowData = XL_row_object;

                    generateMapFieldTableData();
                }
            });

        };

        reader.onerror = function (ex) {
            console.log(ex);
        };

        reader.readAsBinaryString(file);
    };
//};



function generateMapFieldTableColumnHeaderData() {
    var rowMarkup = '';

    mapFieldDropdownData.forEach(function (grpValue, grpIndex) {
        rowMarkup += '<optgroup label="' + (grpValue.isChecked ? 'Standard Fields' : 'Custom Fields') + '">';

        grpValue.values.forEach(function (value, index) {
            rowMarkup += '<option value = "' + value.value + '">' + value.text + '</option>';
        });
        rowMarkup += '</optgroup >';
    });

    $('.mapFieldDropdown').append(rowMarkup);
}

function generateMapFieldTableData() {
    var rowMarkup = '';
    $('#mapFieldTable > thead').empty();
    $('#mapFieldTable > tbody').empty();

    mapFieldTableColumnHeaderData.forEach(function (value, index) {
        rowMarkup += '<th><select class="form-control mapFieldDropdown" id="ContactUploadFieldMaps_' + index + '_Value" name="ContactUploadFieldMaps[' + index + '].Value">' +
            '<option value = "-1">---Ignore---</option>' +
            '</select></th>';
    });

    rowMarkup = '<tr class="text-center">' + rowMarkup + '</tr>';
    $('#mapFieldTable > thead').append(rowMarkup);

    rowMarkup = '<tr class="text-center hasColumnHeaderRow">';

    mapFieldTableColumnHeaderData.forEach(function (colValue, colIndex) {
        rowMarkup += '<td>' + colValue + '</td>';
    });

    rowMarkup += '</tr>'
    $('#mapFieldTable > tbody').append(rowMarkup);

    generateColumnHeaderDisable(hasColumnHeader);

    mapFieldTableRowData.slice(0, 10).forEach(function (rowValue, rowIndex) {
        rowMarkup = '<tr class="text-center">';

        mapFieldTableColumnHeaderData.forEach(function (colValue, colIndex) {
            rowMarkup += '<td>' + mapFieldTableRowData[rowIndex][colValue] + '</td>';
        });

        rowMarkup += '</tr>'
        $('#mapFieldTable > tbody').append(rowMarkup);
    });

    generateMapFieldTableColumnHeaderData();
}

function generateColumnHeaderDisable(has) {

    var rowCount = mapFieldTableRowData.length;
    var showRowCount = mapFieldTableRowData.length > 10 ? 10 : mapFieldTableRowData.length;

    if (has) {
        hasColumnHeader = true;
        //rowCount--;
        $('.hasColumnHeaderRow').addClass('bg-col-header text-muted');
    } else {
        hasColumnHeader = false;
        rowCount++;
        showRowCount++;
        $('.hasColumnHeaderRow').removeClass('bg-col-header text-muted');
    }

    $("#showRowCount").text(showRowCount);
    $("#allRowCount").text(rowCount);
    $("#reviewAllRowCount").text(rowCount);
}

function generateGroupData() {
    var rowMarkup = '';

    groupDropdownData.forEach(function (value, index) {
        rowMarkup += '<li class="list-group-item border-bottom">' +
                            '<input type="hidden" class="selectSingleGroupValue" name="ContactUploadGroups[' + index + '].Value" value="' + value.value + '">' +
                            '<div class="form-check">' +
                            '<input type="checkbox" class="form-check-input selectSingleGroup" id="ContactUploadGroups_' + index + '_IsChecked" ' +
                            'name="ContactUploadGroups[' + index + '].IsChecked" value="false" onchange="changeGroupSelect(this,false)">' +
                            '<label class="form-check-label" for="ContactUploadGroups_' + index + '_IsChecked"> ' + value.text +' <span class="border" style="padding: 2px 5px;">' +
                            '<span class="mr-1">' + value.count +'</span><i class="fas fa-user fa-xs" style="font-size: 10px;"></i></span></label>'+
                     '</div></li>';
    });

    $('#groupDropdown').append(rowMarkup);
}

function changeGroupSelect(obj, isAllSelectFrom) {
    var chkAll = $('#selectAllGroups').is(":checked");
    var chk = $(obj).is(":checked");
    var selectedGroup = [];
    var isAllSelect = true;
    var showSelectText = 'No Group Selected';
    var reviewSelectText = 'None';

    if (isAllSelectFrom && chkAll) {
        //$('.selectSingleGroup:checkbox').attr('checked', 'checked');
        $(".selectSingleGroup:checkbox").prop("checked", true);
    } else if (isAllSelectFrom) {
        $(".selectSingleGroup:checkbox").prop("checked", false);
    }

    $('.selectSingleGroup:checkbox').each(function (i, e) {
        if ($(this).is(":checked")) {
            $(this).val(true);
            var val = $(this).closest('li').find('.selectSingleGroupValue').val();
            var text = groupDropdownData.find(x => x.value == val).text;
            selectedGroup.push(text);
        } else {
            $(this).val(false);
            isAllSelect = false;
        }
    });

    if (selectedGroup.length == 1) {
        showSelectText = '1 Group Selected';
        reviewSelectText = selectedGroup.join(', ');
    } else if (selectedGroup.length > 1) {
        showSelectText = selectedGroup.length.toString() + ' Groups Selected';
        reviewSelectText = selectedGroup.join(', ');
    }

    $("#selectAllGroups:checkbox").prop("checked", isAllSelect);
    $("#showSelectedGroups").text(showSelectText);
    $("#reviewSelectedGroups").text(reviewSelectText);
}

//upload contact
const previousBtn = document.getElementById('previousBtn');
const nextBtn = document.getElementById('nextBtn');
const finishBtn = document.getElementById('finishBtn');
const uploadContacts = document.getElementById('uploadContacts');
const mapFields = document.getElementById('mapFields');
const chooseActions = document.getElementById('chooseActions');
const reviewConfirm = document.getElementById('reviewConfirm');
const bullets = [...document.querySelectorAll('.bullet')];

const MAX_STEPS = 4;
let currentStep = 1;

nextBtn.addEventListener('click', () => {

    const currentBullet = bullets[currentStep - 1];
    const currentBullet2 = bullets[currentStep];
    currentBullet.classList.add('completed');
    currentBullet2.classList.add('current');
    currentStep++;
    previousBtn.disabled = false;
    if (currentStep == MAX_STEPS) {
        nextBtn.disabled = true;
        finishBtn.disabled = false;
    }

    if (currentStep == 1) {
        uploadContacts.style.display = 'block';
        mapFields.style.display = 'none';
        chooseActions.style.display = 'none';
        reviewConfirm.style.display = 'none';
    } else if (currentStep == 2) {
        uploadContacts.style.display = 'none';
        mapFields.style.display = 'block';
        chooseActions.style.display = 'none';
        reviewConfirm.style.display = 'none';
    } else if (currentStep == 3) {
        uploadContacts.style.display = 'none';
        mapFields.style.display = 'none';
        chooseActions.style.display = 'block';
        reviewConfirm.style.display = 'none';
    } else if (currentStep == 4) {
        uploadContacts.style.display = 'none';
        mapFields.style.display = 'none';
        chooseActions.style.display = 'none';
        reviewConfirm.style.display = 'block';
    }
});

previousBtn.addEventListener('click', () => {

    const previousBullet2 = bullets[currentStep - 1];
    const previousBullet = bullets[currentStep - 2];
    previousBullet.classList.remove('completed');
    previousBullet2.classList.remove('current');
    currentStep--;
    //nextBtn.disabled = true;
    const file = document.getElementById("uploadFile").files[0];
    const isFileSelected = file !== null && file !== undefined ? file.size > 0 : false;
    nextBtn.disabled = !isFileSelected;
    finishBtn.disabled = true;
    if (currentStep == 1) {
        previousBtn.disabled = true;
    }
    if (currentStep == 1) {
        uploadContacts.style.display = 'block';
        mapFields.style.display = 'none';
        chooseActions.style.display = 'none';
        reviewConfirm.style.display = 'none';
    } else if (currentStep == 2) {
        uploadContacts.style.display = 'none';
        mapFields.style.display = 'block';
        chooseActions.style.display = 'none';
        reviewConfirm.style.display = 'none';
    } else if (currentStep == 3) {
        uploadContacts.style.display = 'none';
        mapFields.style.display = 'none';
        chooseActions.style.display = 'block';
        reviewConfirm.style.display = 'none';
    } else if (currentStep == 3) {
        uploadContacts.style.display = 'none';
        mapFields.style.display = 'none';
        chooseActions.style.display = 'none';
        reviewConfirm.style.display = 'block';
    }

    //content.innerText = `Step Number ${currentStep}`;
});
