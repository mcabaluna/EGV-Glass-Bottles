
$(document).ready(function () {//decleration of controls
    $('.chosen-select').chosen({ width: "100%" });
    //var elem = document.querySelector('.js-switch');
    //var switchery = new Switchery(elem, { color: '#1AB394' });
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });
    $(".touchspin2").TouchSpin({
        min: 0,
        max: 100,
        step: 0.1,
        decimals: 2,
        boostat: 5,
        maxboostedstep: 10,
        postfix: '%',
        buttondown_class: 'btn btn-white',
        buttonup_class: 'btn btn-white'
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
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}
$('#add').click(function () {
    $('#status').prop("checked", true);
    disablefield('add', false);
    $('#ModalTitle').html('Create BpGroup');
    $('#bpgroup').text('Add BpGroup').removeClass('btn btn-success').addClass('btn btn-primary');
    clearAll();
    $('#MyModalBpGroup').modal({ backdrop: 'static', keyboard: false });
});
$('#bpgroup').click(function () {

    var title = $('#bpgroup').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit BpGroup');
        $('#bpgroup').text('Update BpGroup').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;

        if ($('#bpgroupid').val().toUpperCase().trim() === '') {
            $('.custom-alert').fadeIn();
            toastr.warning('BP Group Code is required.', '');
            isAllValid = false;
        }
        if ($('#bpgroupname').val().toUpperCase().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('BP Group Name is required.', '');
            isAllValid = false;
        }
        if ($('#bptype').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('BP Group Type is required.', '');
            isAllValid = false;
        }
        if ($('#status').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Status is required.', '');
            isAllValid = false;
        }
        if (isAllValid) {
            $("#MyModalBpGroup").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding bpgroup...' : 'Update bpgroup...';
            var messagetext = title === 'add' ? "Error adding bpgroup!" : "Error updating bpgroup!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " bpgroup?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " bpgroup!",
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
                        Code: $('#bpgroupid').val().toUpperCase().trim(),
                        Name: $('#bpgroupname').val().toUpperCase().trim(),
                        BpType: $('#bptype').val().trim(),
                        Status: $('#status').is(':checked'),
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: ''
                    }
                    var requestURL = title === 'add' ? '../../../BpGroup/Save' : '../../../BpGroup/Update';
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
                                        var url = '../../../BpGroup/Index';
                                        window.location.href = url;
                                    }
                                }
                                else {
                                    $('#MyModalBpGroup').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalBpGroup').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalBpGroup').modal({ backdrop: 'static', keyboard: false });
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
    $('#bpgroupid').val('');
    $('#bpgroupname').val('');
}
function initButtonEvent() {
    $('.edit_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../BpGroup/GetBpGroup';
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
                    $('#bpgroupid').val(forcomplete.Code);
                    $('#bpgroupname').val(forcomplete.Name);
                    $('#bptype').val(forcomplete.BpType).trigger('chosen:updated');
                    $("#status").prop("checked", forcomplete.Status);
                    $('#ModalTitle').html('Edit BpGroup');
                    $('#bpgroup').text('Update BpGroup');
                    $('#MyModalBpGroup').modal({ backdrop: 'static', keyboard: false });
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
                text: "You want to remove this BpGroup?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, remove bpgroup!",
                cancelButtonText: "No, cancel remove!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: 'Removing BpGroup...',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var requestURL = '../../../BpGroup/Delete';
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
                                title: forcomplete.HttpStatus === 200 ? "Successful" : "Error removing bpgroup!",
                                text: forcomplete.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (forcomplete.HttpStatus === 200) {
                                    if (result.value) {
                                        var url = '../../../BpGroup/Index';
                                        window.location.href = url;
                                    }
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: "Error removing bpgroup!",
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
            var requestURL = '../../../BpGroup/GetBpGroup';
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
                    $('#bpgroupid').val(forcomplete.Code);
                    $('#bpgroupname').val(forcomplete.Name);
                    $('#bptype').val(forcomplete.BpType).trigger('chosen:updated');
                    $("#status").prop("checked", forcomplete.Status);
                    $('#ModalTitle').html('BpGroup Details');
                    $('#bpgroup').text('Edit BpGroup').addClass('btn btn-success');
                    $('#MyModalBpGroup').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
}
function disablefield(ptype, pDisable) {
    switch (ptype) {
        case "edit":
        case "add":
            $('#bpgroupid').prop('disabled', pDisable);
            break;
        default:
            $('#bpgroupid').prop('disabled', !pDisable);
            break;
    }
    $('#bpgroupname').prop('disabled', pDisable);
    $('#bptype').prop('disabled', pDisable).trigger("chosen:updated");  
    $('#status').prop('disabled', pDisable);
}