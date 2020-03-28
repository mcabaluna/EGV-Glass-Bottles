
$(document).ready(function () {//decleration of controls
    $('.chosen-select').chosen({ width: "100%" });
    //var elem = document.querySelector('.js-switch');
    //var switchery = new Switchery(elem, { color: '#1AB394' });
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });
    //$(".touchspin2").TouchSpin({
    //    min: 0,
    //    max: 100,
    //    step: 0.1,
    //    decimals: 2,
    //    boostat: 5,
    //    maxboostedstep: 1,
    //    postfix: '%',
    //    buttondown_class: 'btn btn-white',
    //    buttonup_class: 'btn btn-white'
    //});
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
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}

$('#add').click(function () {
    var date = new Date();
    $('#effectivefrom').val(moment(date).format('M/DD/YYYY'));
    $('#effectiveto').val(moment(date).format('M/DD/YYYY'));
    $('#status').prop('checked', true);
    disablefield('add', false);
    $('#ModalTitle').html('Create Vat');
    $('#vat').text('Add Vat').removeClass('btn btn-success').addClass('btn btn-primary');
    clearAll();
    $('#MyModalVat').modal({ backdrop: 'static', keyboard: false });
});
$('#vat').click(function () {

    var title = $('#vat').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Vat');
        $('#vat').text('Update Vat').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;

        if ($('#vatid').val().trim() === '') {
            $('.custom-alert').fadeIn();
            toastr.warning('Vat Code is required.', '');
            isAllValid = false;
        }
        if ($('#vatname').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Vat Name is required.', '');
            isAllValid = false;
        }
        if ($('#type').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Vat Type is required.', '');
            isAllValid = false;
        }
        if (Number($('#percentage').val().trim()) < 0) {

            $('.custom-alert').fadeIn();
            toastr.warning('Vat Percentage is required.', '');
            isAllValid = false;
        }
        if ($('#status').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Status is required.', '');
            isAllValid = false;
        }
        if ($('#effectivefrom').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Effective From is required.', '');
            isAllValid = false;
        }
        if ($('#effectiveto').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Effective To is required.', '');
            isAllValid = false;
        }
        if (isAllValid) {
            $("#MyModalVat").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding vat...' : 'Update vat...';
            var messagetext = title === 'add' ? "Error adding vat!" : "Error updating vat!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " vat?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " vat!",
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
                        Code: $('#vatid').val().toUpperCase().trim(),
                        Name: $('#vatname').val().toUpperCase().trim(),
                        Type: $('#type').val().trim(),
                        Percentage: parseFloat($('#percentage').val().replace(/,/g, '')),
                        Status: $('#status').is(':checked'),
                        EffectiveFrom: $('#effectivefrom').val().trim(),
                        EffectiveTo: $('#effectiveto').val().trim(),
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: ''
                    }
                    var requestURL = title === 'add' ? '../../../Vat/Save' : '../../../Vat/Update';
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
                                        var url = '../../../Vat/Index';
                                        window.location.href = url;
                                    }
                                }
                                else {
                                    $('#MyModalVat').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalVat').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalVat').modal({ backdrop: 'static', keyboard: false });
                }
            });
        };
    }
});
$('#percentage').change(function () {
    var val = parseFloat($(this).val().replace(',', ''));
    var oldval = parseFloat($('#oldperc').val().replace(',', ''));
    if (isNaN(val)) { val = 0; }
    if (isNaN(oldval)) { oldval = 0; }
    if (val > 100) {
        $('.custom-alert').fadeIn();
        toastr.warning('Percentage should not exceed to 100%.', '');
        $('#percentage').val(addThousandsSeparator(oldval.toFixed(2)));
    }
    else {
        $('#percentage').val(addThousandsSeparator(val.toFixed(2)));
        $('#oldperc').val(addThousandsSeparator(val.toFixed(2)));
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
    $('#vatid').val('');
    $('#vatname').val('');
    $('#type').val('').trigger("chosen:updated");
    $('#percentage').val('0.00');
    $('#effectivefrom').val(moment(date).format('M/DD/YYYY'));
    $('#effectiveto').val(moment(date).format('M/DD/YYYY'));
    $('#status').prop('checked', true);
}
function initButtonEvent() {
    $('.edit_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../Vat/GetVat';
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
                    $('#vatid').val(forcomplete.Code);
                    $('#vatname').val(forcomplete.Name);
                    $('#type').val(forcomplete.Type).trigger('chosen:updated');
                    $('#status').prop('checked', forcomplete.Status);
                    $('#percentage').val(addThousandsSeparator(forcomplete.Percentage.toFixed(2)));
                    $('#effectivefrom').val(moment(forcomplete.EffectiveFrom).format('M/DD/YYYY'));
                    $('#effectiveto').val(moment(forcomplete.EffectiveTo).format('M/DD/YYYY'));
                    $('#ModalTitle').html('Edit Vat');
                    $('#vat').text('Update Vat');
                    $('#MyModalVat').modal({ backdrop: 'static', keyboard: false });
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
                text: "You want to remove this Vat?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, remove vat!",
                cancelButtonText: "No, cancel remove!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: 'Removing Vat...',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var requestURL = '../../../Vat/Delete';
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
                                title: forcomplete.HttpStatus === 200 ? "Successful" : "Error removing vat!",
                                text: forcomplete.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (forcomplete.HttpStatus === 200) {
                                    if (result.value) {
                                        var url = '../../../Vat/Index';
                                        window.location.href = url;
                                    }
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: "Error removing vat!",
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
            var requestURL = '../../../Vat/GetVat';
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
                    $('#vatid').val(forcomplete.Code);
                    $('#vatname').val(forcomplete.Name);
                    $('#type').val(forcomplete.Type).trigger('chosen:updated');
                    $('#percentage').val(addThousandsSeparator(forcomplete.Percentage.toFixed(2)));
                    $('#status').prop('checked', forcomplete.Status);
                    $('#effectivefrom').val(moment(forcomplete.EffectiveFrom).format('M/DD/YYYY'));
                    $('#effectiveto').val(moment(forcomplete.EffectiveTo).format('M/DD/YYYY'));
                    $('#ModalTitle').html('Vat Details');
                    $('#vat').text('Edit Vat').addClass('btn btn-success');
                    $('#MyModalVat').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
}
function disablefield(ptype, pDisable) {
    switch (ptype) {
        case "edit":
        case "add":
            $('#vatid').prop('disabled', pDisable);
            break;
        default:
            $('#vatid').prop('disabled', !pDisable);
            break;
    }
    $('#vatname').prop('disabled', pDisable);
    $('#type').prop('disabled', pDisable).trigger("chosen:updated");
    $('#percentage').prop('disabled', pDisable);
    $('#effectivefrom').prop('disabled', pDisable);
    $('#effectiveto').prop('disabled', pDisable);
    $('#status').prop('disabled', pDisable);
}
function addThousandsSeparator(input) {
    var num_parts = input.toString().split(".");
    num_parts[0] = num_parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return num_parts.join(".");
}