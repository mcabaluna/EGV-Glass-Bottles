
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
            $.map($(this).data('api').getSelection(), function (val) { return val.TermId; });
    });


$('#data_1 .input-group.date').datepicker({
    todayBtn: "linked",
    keyboardNavigation: false,
    forceParse: false,
    calendarWeeks: true,
    autoclose: true
});
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}

$('#add').click(function () {
    var date = new Date();
    disablefield('add', false);
    $('#ModalTitle').html('Create Payment Term');
    $('#status').prop('checked', true);
    $('#terms').text('Add Payment Term').removeClass('btn btn-success').addClass('btn btn-primary');
    clearAll();
    $('#MyModalPaymentTerms').modal({ backdrop: 'static', keyboard: false });
});
$('#terms').click(function () {

    var title = $('#terms').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Payment Term');
        $('#terms').text('Update Payment Term').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;

        if ($('#termid').val().trim() === '') {
            $('.custom-alert').fadeIn();
            toastr.warning('Term Id is required.', '');
            isAllValid = false;
        }
        if ($('#termname').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Term Name is required.', '');
            isAllValid = false;
        }
        if (Number($('#noofdays').val().trim()) === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Term No. of Days is required.', '');
            isAllValid = false;
        }
        if ($('#status').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Status is required.', '');
            isAllValid = false;
        }
        if (isAllValid) {
            $("#MyModalPaymentTerms").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding payment term...' : 'Update payment term...';
            var messagetext = title === 'add' ? "Error adding payment term!" : "Error updating payment term!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " payment term?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " payment term!",
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
                        TermId: $('#termid').val().toUpperCase().trim(),
                        Name: $('#termname').val().toUpperCase().trim(),
                        NoOfDays: Number($('#noofdays').val().trim()),
                        Status: $('#status').is(':checked'),
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: ''
                    }
                    var requestURL = title === 'add' ? '../../../PaymentTerms/Save' : '../../../PaymentTerms/Update';
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
                                        var url = '../../../PaymentTerms/Index';
                                        window.location.href = url;
                                    }
                                }
                                else {
                                    $('#MyModalPaymentTerms').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalPaymentTerms').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalPaymentTerms').modal({ backdrop: 'static', keyboard: false });
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
    $('#termid').val('');
    $('#termname').val('');
    $('#noofdays').val(0);
}
function initButtonEvent() {
    $('.edit_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../PaymentTerms/GetPaymentTerms';
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
                    $('#termid').val(forcomplete.TermId);
                    $('#termname').val(forcomplete.Name);
                    $('#noofdays').val(forcomplete.NoOfDays);
                    $('#status').prop('checked', forcomplete.Status);
                    $('#ModalTitle').html('Edit Payment Term');
                    $('#terms').text('Update Payment Term');
                    $('#MyModalPaymentTerms').modal({ backdrop: 'static', keyboard: false });
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
                text: "You want to remove this Payment Term?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, remove payment term!",
                cancelButtonText: "No, cancel remove!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: 'Removing Payment Term...',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var requestURL = '../../../PaymentTerms/Delete';
                    var forcomplete = {};
                    $.ajax({
                        type: "DELETE",
                        url: requestURL,
                        data: { id: lastSelectedIds },
                        success: function (data2) {
                            forcomplete = data2;
                        },
                        complete: function () {
                            swal.hideLoading();
                            swal({
                                type: forcomplete.HttpStatus === 200 ? 'success' : 'warning',
                                title: forcomplete.HttpStatus === 200 ? "Successful" : "Error removing payment term!",
                                text: forcomplete.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (forcomplete.HttpStatus === 200) {
                                    if (result.value) {
                                        var url = '../../../PaymentTerms/Index';
                                        window.location.href = url;
                                    }
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: "Error removing payment term!",
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
            var requestURL = '../../../PaymentTerms/GetPaymentTerms';
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
                    $('#termid').val(forcomplete.TermId);
                    $('#termname').val(forcomplete.Name);
                    $('#noofdays').val(forcomplete.NoOfDays);
                    $('#status').prop('checked', forcomplete.Status);
                    $('#ModalTitle').html('Payment Term Details');
                    $('#terms').text('Edit Payment Term').addClass('btn btn-success');
                    $('#MyModalPaymentTerms').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
}
function disablefield(ptype, pDisable) {
    switch (ptype) {
        case "edit":
        case "add":
            $('#termid').prop('disabled', pDisable);
            break;
        default:
            $('#termid').prop('disabled', !pDisable);
            break;
    }
    $('#termname').prop('disabled', pDisable);
    $('#noofdays').prop('disabled', pDisable);
    $('#status').prop('disabled', pDisable);
}