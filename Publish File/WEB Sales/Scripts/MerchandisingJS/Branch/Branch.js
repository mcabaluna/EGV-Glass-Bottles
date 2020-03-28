$(document).ready(function () {//decleration of controls
    $('.chosen-select').chosen({ width: "100%" });
    //var elem = document.querySelector('.js-switch');
    //var switchery = new Switchery(elem, { color: '#1AB394' });

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
$('#data_1 .input-group.date').datepicker({
    todayBtn: "linked",
    keyboardNavigation: false,
    forceParse: false,
    calendarWeeks: true,
    autoclose: true
});
$('#add').click(function () {
    $('#status').prop("checked", true);
    var date = new Date();
    $('#validfrom').val(moment(date).format('M/DD/YYYY'));
    $('#validto').val(moment(date).format('M/DD/YYYY'));

    //if (Array.prototype.forEach) {
    //    var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));

    //    elems.forEach(function (html) {
    //        var switchery = new Switchery(html, { color: '#1AB394' });
    //    });
    //}
    //else {
    //    var elems = document.querySelectorAll('.js-switch');

    //    for (var i = 0; i < elems.length; i++) {
    //        var switchery = new Switchery(elems[i], { color: '#1AB394' });
    //    }
    //}

    disablefield('add', false);
    $('#ModalTitle').html('Create Branch');
    $('#branch').text('Add Branch').removeClass('btn btn-success').addClass('btn btn-primary');
    clearAll();
    $('#MyModalBranch').modal({ backdrop: 'static', keyboard: false });
});
$('#branch').click(function () {

    var title = $('#branch').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Branch');
        $('#branch').text('Update Branch').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;

        if ($('#branchid').val().trim() === '') {
            $('.custom-alert').fadeIn();
            toastr.warning('Branch Code is required.', 'System Warning');
            isAllValid = false;
        }
        if ($('#branchname').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Branch Name is required.', 'System Warning');
            isAllValid = false;
        }
        if ($('#status').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Status is required.', 'System Warning');
            isAllValid = false;
        }
        if ($('#validfrom').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Valid From is required.', 'System Warning');
            isAllValid = false;
        }
        if ($('#validto').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Valid To is required.', 'System Warning');
            isAllValid = false;
        }
        if (isAllValid) {
            $("#MyModalBranch").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding branch...' : 'Update branch...';
            var messagetext = title === 'add' ? "Error adding branch!" : "Error updating branch!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " branch?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " branch!",
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
                        Code: $('#branchid').val().toUpperCase().trim(),
                        Name: $('#branchname').val().toUpperCase().trim(),
                        Status: $('#status').is(':checked'),
                        ValidFrom: $('#validfrom').val().trim(),
                        ValidTo: $('#validto').val().trim(),
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: ''
                    }
                    var requestURL = title === 'add' ? '../../../Branch/Save' : '../../../Branch/Update';
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
                                        var url = '../../../Branch/Index';
                                        window.location.href = url;
                                    }
                                }
                                else {
                                    $('#MyModalBranch').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalBranch').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalBranch').modal({ backdrop: 'static', keyboard: false });
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
    $('#branchid').val('');
    $('#branchname').val('');
    $('#validfrom').val(moment(date).format('M/DD/YYYY'));
    $('#validto').val(moment(date).format('M/DD/YYYY'));
    $('#status').prop('checked', true);
}
function initButtonEvent() {
    $('.edit_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../Branch/GetBranch';
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
                    $('#branchid').val(forcomplete.Code);
                    $('#branchname').val(forcomplete.Name);
                    $('#validfrom').val(moment(forcomplete.ValidFrom).format('M/DD/YYYY'));
                    $('#validto').val(moment(forcomplete.ValidTo).format('M/DD/YYYY'));
                    $('#status').prop('checked', forcomplete.Status);
                    $('#ModalTitle').html('Edit Branch');
                    $('#branch').text('Update Branch');
                    $('#MyModalBranch').modal({ backdrop: 'static', keyboard: false });
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
                text: "You want to remove this Branch?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, remove branch!",
                cancelButtonText: "No, cancel remove!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: 'Removing Branch...',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var requestURL = '../../../Branch/Delete';
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
                                title: forcomplete.HttpStatus === 200 ? "Successful" : "Error removing branch!",
                                text: forcomplete.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (forcomplete.HttpStatus === 200) {
                                    if (result.value) {
                                        var url = '../../../Branch/Index';
                                        window.location.href = url;
                                    }
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: "Error removing branch!",
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
            var requestURL = '../../../Branch/GetBranch';
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
                    $('#branchid').val(forcomplete.Code);
                    $('#branchname').val(forcomplete.Name);
                    $('#validfrom').val(moment(forcomplete.ValidFrom).format('M/DD/YYYY'));
                    $('#validto').val(moment(forcomplete.ValidTo).format('M/DD/YYYY'));
                    $('#status').prop('checked', forcomplete.Status);
                    $('#ModalTitle').html('Branch Details');
                    $('#branch').text('Edit Branch').addClass('btn btn-success');
                    $('#MyModalBranch').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
}
function disablefield(ptype, pDisable) {
    switch (ptype) {
        case "edit":
        case "add":
            $('#branchid').prop('disabled', pDisable);
            break;
        default:
            $('#branchid').prop('disabled', !pDisable);
            break;
    }
    $('#branchname').prop('disabled', pDisable);
    $('#validfrom').prop('disabled', pDisable);
    $('#validto').prop('disabled', pDisable);
    $('#status').prop('disabled', pDisable);
}