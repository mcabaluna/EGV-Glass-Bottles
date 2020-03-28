
$(document).ready(function () {
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
            $.map($(this).data('api').getSelection(), function (val) { return Number(val.RoleId); });
    });

$('#add').click(function () {
    disablefield('add', false);
    $('#ModalTitle').html('Create Role');
    $('#roles').text('Add Role').removeClass('btn btn-success').addClass('btn btn-primary');
    clearAll();
    $('#MyModalRoles').modal({ backdrop: 'static', keyboard: false });
});
$('#roles').click(function () {

    var title = $('#roles').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Role');
        $('#roles').text('Update Role').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;
        
        if ($('#rolename').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Role Name is required.', '');
            isAllValid = false;
        }
        if (isAllValid) {
            $("#MyModalRoles").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding role...' : 'Update role...';
            var messagetext = title === 'add' ? "Error adding role!" : "Error updating role!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " this role?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " role!",
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
                    var data = {};
                    if (title === 'add') {
                        data = {
                            RoleName: $('#rolename').val().toUpperCase().trim(),
                            Description: $('#description').val().toUpperCase().trim(),
                            CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                            CreatedById: '',
                            ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                            ModifiedById: ''
                        }
                    }
                    else {
                        data = {
                            RoleId: $('#roleid').val().toUpperCase().trim(),
                            RoleName: $('#rolename').val().toUpperCase().trim(),
                            Description: $('#description').val().toUpperCase().trim(),
                            CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                            CreatedById: '',
                            ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                            ModifiedById: ''
                        }
                    }
                    var requestURL = title === 'add' ? '../../../Roles/Save' : '../../../Roles/Update';
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
                                        var url = '../../../Roles/Index';
                                        window.location.href = url;
                                    }
                                }
                                else {
                                    $('#MyModalRoles').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalRoles').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalRoles').modal({ backdrop: 'static', keyboard: false });
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
function clearAll() {
    var date = new Date();
    $('#roleid').val('');
    $('#rolename').val('');
    $('#description').val('');
}
function initButtonEvent() {
    $('.edit_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../Roles/GetRoles';
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
                    $('#roleid').val(forcomplete.RoleId);
                    $('#rolename').val(forcomplete.RoleName);
                    $('#description').val(forcomplete.Description);
                    $('#ModalTitle').html('Edit Role');
                    $('#roles').text('Update Role');
                    $('#MyModalRoles').modal({ backdrop: 'static', keyboard: false });
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
                text: "You want to remove this Role?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, remove role!",
                cancelButtonText: "No, cancel remove!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: 'Removing Role...',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var requestURL = '../../../Roles/Delete';
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
                                        var url = '../../../Roles/Index';
                                        window.location.href = url;
                                    }
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: "Error removing role!",
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
            var requestURL = '../../../Roles/GetRoles';
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
                    $('#roleid').val(forcomplete.RoleId);
                    $('#rolename').val(forcomplete.RoleName);
                    $('#description').val(forcomplete.Description);
                    $('#ModalTitle').html('Role Details');
                    $('#roles').text('Edit Role').addClass('btn btn-success');
                    $('#MyModalRoles').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
}
function disablefield(ptype, pDisable) {
    switch (ptype) {
        case "edit":
        case "add":
            $('#rolename').prop('disabled', pDisable);
            break;
        default:
            $('#rolename').prop('disabled', !pDisable);
            break;
    }
    $('#description').prop('disabled', pDisable);
}