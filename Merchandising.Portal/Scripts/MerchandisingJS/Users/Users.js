//Global Variable declaration
var lastSelectedIds;
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
$('#ApiDemoGrid').on('aweselect',
    function () {
        lastSelectedIds =
            $.map($(this).data('api').getSelection(), function (val) { return val.Code; });
    });
$('#add').click(function () {
    clearAll();
    disablefield('add', false);
    $('#status').prop("checked", true);
    $('#ModalTitle').html('Create User');
    $('#user').text('Add User').removeClass('btn btn-success').addClass('btn btn-primary');
    $('#MyModalUser').modal({ backdrop: 'static', keyboard: false });
});
$('#user').click(function () {

    var title = $('#user').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit User');
        $('#user').text('Update User').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;

        if ($('#userid').val().trim() === '') {
            $('.custom-alert').fadeIn();
            toastr.warning('User ID is required.', '');
            isAllValid = false;
        }


        if ($('#username').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('User Name is required.', '');
            isAllValid = false;
        }

        if ($('#password').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Password is required.', '');
            isAllValid = false;
        }

        if ($('#password').val().length < 4) {
            $('.custom-alert').fadeIn();
            toastr.warning('Password must be at least 4 characters.', '');

            isAllValid = false;
        }

        if ($('#email').val().trim() != '') {
            if (validatetoemail($('#email').val().trim())) {
            }
            else {

                $('.custom-alert').fadeIn();
                toastr.warning('Email is not valid email address.', '');

                isAllValid = false;
            }
        }

        //if ($('#email').val().trim() === '') {
        //    $('.custom-alert').fadeIn();
        //    toastr.warning('Email is required.', '');
        //    isAllValid = false;
        //}
        if ($('#branch').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Branch is required.', '');

            isAllValid = false;
        }

        if ($('#role').val() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Role is required.', '');

            isAllValid = false;
        }
        if (isAllValid) {
            $("#MyModalUser").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding user...' : 'Update user...';
            var messagetext = title === 'add' ? "Error adding user!" : "Error updating user!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " user?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " user!",
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
                        UserId: $('#userid').val().toUpperCase().trim(),
                        UserName: $('#username').val().toUpperCase().trim(),
                        Password: $('#password').val().trim(),
                        Role: $('#role').val(),
                        BranchCode: $('#branch').val().trim(),
                        ContactNo: $('#contact').val().trim(),
                        Email: $('#email').val().trim(),
                        Status: $('#status').is(':checked'),
                        LastAccess: moment($('#lastaccess').val())
                    }
                    var requestURL = title === 'add' ? '../../../Users/Save' : '../../../Users/Update';
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
                                        var url = '../../../Users/Index';
                                        window.location.href = url;
                                    }
                                }
                                else {
                                    $('#MyModalUser').modal({ backdrop: 'static', keyboard: false });
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
                    $('#MyModalUser').modal({ backdrop: 'static', keyboard: false });
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
    $('#userid').val('');
    $('#username').val('');
    $('#password').val('');
    $('#email').val('');
    $('#contact').val('');
    $('#role').val('').trigger("chosen:updated");
    $('#branch').val('').trigger("chosen:updated");
    $('#lastaccess').val('');

}
function initButtonEvent() {
    $('.edit_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../Users/GetUser';
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
                    clearAll();
                    disablefield(false);
                    $('#userid').val(forcomplete.UserId);
                    $('#username').val(forcomplete.UserName);
                    $('#password').val(forcomplete.Password);
                    $('#email').val(forcomplete.Email);
                    $('#contact').val(forcomplete.ContactNo);
                    $('#role').val(forcomplete.Role).trigger('chosen:updated');
                    $('#branch').val(forcomplete.BranchCode).trigger('chosen:updated');
                    $("#status").prop("checked", forcomplete.Status);
                    $('#lastaccess').val(GetUserLastAccess(forcomplete.LastAccess));
                    $('#ModalTitle').html('Edit User');
                    $('#user').text('Update User');
                    $('#MyModalUser').modal({ backdrop: 'static', keyboard: false });
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
                text: "You want to remove this User?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, remove user!",
                cancelButtonText: "No, remove adding!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: 'Removing User...',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var requestURL = '../../../Users/Delete';
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
                                title: forcomplete.HttpStatus === 200 ? "Successful" : "Error removing user!",
                                text: forcomplete.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (forcomplete.HttpStatus === 200) {
                                    if (result.value) {
                                        var url = '@Url.Action("Index")';
                                        window.location.href = url;
                                    }
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: "Error removing user!",
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
            var requestURL = '../../../Users/GetUser';
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
                    clearAll();
                    disablefield('edit', true);
                    $('#userid').val(forcomplete.UserId);
                    $('#username').val(forcomplete.UserName);
                    $('#password').val(forcomplete.Password);
                    $('#email').val(forcomplete.Email);
                    $('#contact').val(forcomplete.ContactNo);
                    $('#role').val(forcomplete.Role).trigger("chosen:updated");
                    $('#branch').val(forcomplete.BranchCode).trigger("chosen:updated");
                    $("#status").prop("checked", forcomplete.Status);
                    $('#lastaccess').val(GetUserLastAccess(forcomplete.LastAccess));
                    $('#ModalTitle').html('User Details');
                    $('#user').text('Edit User').addClass('btn btn-success');
                    $('#MyModalUser').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
}
function disablefield(ptype, pDisable) {
    switch (ptype) {
        case "edit":
        case "add":
            $('#userid').prop('disabled', pDisable);
            $('#password').prop('disabled', pDisable);
            break;
        default:
            $('#userid').prop('disabled', !pDisable);
            $('#password').prop('disabled', !pDisable);
            break;
    }
    $('#username').prop('disabled', pDisable);
    $('#email').prop('disabled', pDisable);
    $('#contact').prop('disabled', pDisable);
    $('#role').prop('disabled', pDisable).trigger('chosen:updated');
    $('#branch').prop('disabled', pDisable).trigger('chosen:updated');
    $('#status').prop('disabled', pDisable);
}
function validatetoemail(my_email) {
    var filter = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
    if (filter.test(my_email)) {
        return true;
    }
    else {
        return false;
    }
}
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}
function GetUserLastAccess(lastaccess) {
    if (lastaccess != null && lastaccess != '') {
        var d = new Date(moment(lastaccess));
        var nmonth = d.getMonth(), ndate = d.getDate(), nyear = d.getFullYear(), day = d.getDay();
        var nhour = d.getHours(), nmin = d.getMinutes(), nsec = d.getSeconds(), ap;

        months = new Array('January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December');
        days = new Array('Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday');
        if (nhour == 0) { ap = " AM"; nhour = 12; }
        else if (nhour < 12) { ap = " AM"; }
        else if (nhour == 12) { ap = " PM"; }
        else if (nhour > 12) { ap = " PM"; nhour -= 12; }

        if (nmin <= 9) nmin = "0" + nmin;
        if (nsec <= 9) nsec = "0" + nsec;

        //var clocktext = "" + (nmonth + 1) + " " + ndate + ", " + nyear + " " + nhour + ":" + nmin + ":" + nsec + ap + "";
        //document.getElementById('clockbox').innerHTML = clocktext;
        return lastaccess = '' + days[day] + ' - ' + months[nmonth] + ' ' + ndate + ', ' + nyear + ' ' + nhour + ':' + nmin + ':' + nsec + ap;
    }
}
//function arrayToString(arr) {
//    let str = '';
//    arr.forEach(function (i, index) {
//        str += i;
//        if (index != (arr.length - 1)) {
//            str += ',';
//        };
//    });
//    return str;
//}