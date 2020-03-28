
$(document).ready(function () {//decleration of controls
    $('.chosen-select').chosen({ width: "100%" });
   
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });
    initButtonEvent();
});
$('#ApiDemoGrid').on('awerender', function () {
    $(this).find(".checkbox-toggle").bootstrapToggle();
});

$('#txtsearch').keyup(function () {
    $('#ApiDemoGrid').data('api').load();
    initButtonEvent();
});

$('#ApiDemoGrid').on('aweload', function (e, data, rd) {
    $('#log').prepend('aweload handled,\n data: ' + JSON.stringify(data) + "\n request data" + JSON.stringify(rd) + '\n\n');
    initButtonEvent();
}).on('awebeginload', function (e, data) {
    $('#log').prepend('awebeginload handled,\n request data: ' + JSON.stringify(data) + '\n\n');
});

var lastSelectedIds;
$('#ApiDemoGrid').on('aweselect',
    function () {
        lastSelectedIds =
            $.map($(this).data('api').getSelection(), function (val) { return val.Code; });
    });


function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}

$('#add').click(function () {
    var date = new Date();
    document.getElementById("status").checked = true;
    disablefield('add', false);
    $('#ModalTitle').html('Create Warehouse');
    $('#whse').text('Add Warehouse').removeClass('btn btn-success').addClass('btn btn-primary');
    clearAll();
    $('#MyModalWarehouse').modal({ backdrop: 'static', keyboard: false });
});
$('#whse').click(function () {

    var title = $('#whse').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Warehouse');
        $('#whse').text('Update Warehouse').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;

        if ($('#whseid').val().trim() === '') {
            $('.custom-alert').fadeIn();
            toastr.warning('Whse Code is required.', '');
            isAllValid = false;
        }
        if ($('#whsename').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Whse Name is required.', '');
            isAllValid = false;
        }
        if ($('#branch').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Branch is required.', '');
            isAllValid = false;
        }
        if ($('#status').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Status is required.', '');
            isAllValid = false;
        }
        if (isAllValid) {
            $("#MyModalWarehouse").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding warehouse...' : 'Update warehouse...';
            var messagetext = title === 'add' ? "Error adding warehouse!" : "Error updating warehouse!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " warehouse?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " warehouse!",
                cancelButtonText: "No, cancel " + canceltext + "!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: swaltext,
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var date = new Date();
                    var data = {
                        Code: $('#whseid').val().toUpperCase().trim(),
                        Name: $('#whsename').val().toUpperCase().trim(),
                        BranchCode: $('#branch').val().trim(),
                        Status: $('#status').is(':checked'),
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: ''
                    }
                    var requestURL = title === 'add' ? '../../../Warehouse/Save' : '../../../Warehouse/Update';
                    var type = title === 'add' ? "POST" : "PUT";
                    var forcomplete = {};
                    $.ajax({
                        type: type,
                        contentType: "application/json; charset=utf-8",
                        url: requestURL,
                        data: JSON.stringify(data),
                        dataType: "json",
                        success: function (data2) {
                            forcomplete = data2;
                        },
                        complete: function () {
                            swal.hideLoading();
                            swal({
                                type: forcomplete.HttpStatus === 200 ? 'success' : 'warning',
                                title: forcomplete.HttpStatus === 200 ? "Successful" : messagetext,
                                text: forcomplete.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (forcomplete.HttpStatus === 200) {
                                    if (result.value) {
                                        var url = '../../../Warehouse/Index';
                                        window.location.href = url;
                                    }
                                }
                                else {
                                    $('#MyModalWarehouse').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: messagetext,
                                text: data3.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (result.value) {
                                    $('#MyModalWarehouse').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalWarehouse').modal({ backdrop: 'static', keyboard: false });
                }
            });
        };
    }
});
$('#clear').click(function () {
    clearAll();
});
function setContent(o) {
    $('#ApiDemoGrid').data('api').clearpersist();
    $('#demoContent').html(o);
}
var Statustoggle = function (model, prop) {
    var checked = model[prop];
    var status = checked === true ? "ACTIVE" : "IN_ACTIVE";
    if (status === "ACTIVE") {
        return '<span class="label label-primary">' + status + '</span>';
    } else if (status === "IN_ACTIVE") {
        return '<span class="label label-danger">' + status + '</span>';
    }
};
function clearAll() {
    var date = new Date();
    $('#whseid').val('');
    $('#whsename').val('');
    $('#branch').val('').trigger('chosen:updated');
}
function initButtonEvent() {
    $('.edit_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../Warehouse/GetWarehouse';
            var forcomplete = {};
            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                url: requestURL,
                data: { id: lastSelectedIds },
                dataType: "json",
                success: function (data) {
                    forcomplete = data;
                },
                complete: function () {
                    disablefield(false);
                    $('#whseid').val(forcomplete.Code);
                    $('#whsename').val(forcomplete.Name);
                    $('#branch').val(forcomplete.BranchCode).trigger('chosen:updated');
                    $('#status').prop('cheked', forcomplete.Status);
                    $('#ModalTitle').html('Edit Warehouse');
                    $('#whse').text('Update Warehouse');
                    $('#MyModalWarehouse').modal({ backdrop: 'static', keyboard: false });
                }
            });
        } else {
            $('.custom-alert').fadeIn();
            toastr.warning('Please select row to edit.', 'System Warning');
        }
    });
    $('.cancel_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            swal({
                title: "Are you sure?",
                text: "You want to remove this warehouse?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, remove warehouse!",
                cancelButtonText: "No, cancel remove!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: 'Removing warehouse...',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var requestURL = '../../../Warehouse/Delete';
                    var forcomplete = {};
                    $.ajax({
                        type: "DELETE",
                        url: requestURL,
                        data: { "id": lastSelectedIds },
                        success: function (data2) {
                            forcomplete = data2;
                        },
                        complete: function () {
                            swal.hideLoading();
                            swal({
                                type: forcomplete.HttpStatus === 200 ? 'success' : 'warning',
                                title: forcomplete.HttpStatus === 200 ? "Successful" : "Error removing warehouse!",
                                text: forcomplete.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (forcomplete.HttpStatus === 200) {
                                    if (result.value) {
                                        var url = '../../../Warehouse/Index';
                                        window.location.href = url;
                                    }
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: "Error removing warehouse!",
                                text: data3.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            });
                        }
                    });
                }
            });
        }
        else {

            $('.custom-alert').fadeIn();
            toastr.warning('Please select row to cancel.', 'System Warning');
        }
    });
    $('.view_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../Warehouse/GetWarehouse';
            var forcomplete = {};
            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                url: requestURL,
                data: { id: lastSelectedIds },
                dataType: "json",
                success: function (data) {
                    forcomplete = data;
                },
                complete: function () {
                    disablefield('edit', true);
                    $('#whseid').val(forcomplete.Code);
                    $('#whsename').val(forcomplete.Name);
                    $('#branch').val(forcomplete.BranchCode).trigger('chosen:updated');
                    $('#status').prop('cheked', forcomplete.Status);
                    $('#ModalTitle').html('Warehouse Details');
                    $('#whse').text('Edit Warehouse').addClass('btn btn-success');
                    $('#MyModalWarehouse').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
}
function disablefield(ptype, pDisable) {
    switch (ptype) {
        case "edit":
        case "add":
            $('#whseid').prop('disabled', pDisable);
            break;
        default:
            $('#whseid').prop('disabled', !pDisable);
            break;
    }
    $('#whsename').prop('disabled', pDisable);
    $('#branch').prop('disabled', pDisable).trigger("chosen:updated");
    $('#status').prop('disabled', pDisable);
}