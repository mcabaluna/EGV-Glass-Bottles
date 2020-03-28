
$(document).ready(function () {
    $('.chosen-select').chosen({ width: "100%" });

    //var updateOutput = function (e) {
    //    var list = e.length ? e : $(e.target),
    //        output = list.data('output');
    //    if (window.JSON) {
    //        output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
    //    } else {
    //        output.val('JSON browser support required for this demo.');
    //    }
    //};
    //// activate Nestable for list 1
    //$('#nestable').nestable({
    //    group: 1
    //}).on('change', updateOutput);

    //// activate Nestable for list 2
    //$('#nestable2').nestable({
    //    group: 1
    //}).on('change', updateOutput);

    //// output initial serialised data
    //updateOutput($('#nestable').data('output', $('#nestable-output')));
    //updateOutput($('#nestable2').data('output', $('#nestable2-output')));

    //$('#nestable-menu').on('click', function (e) {
    //    var target = $(e.target),
    //        action = target.data('action');
    //    if (action === 'expand-all') {
    //        $('.dd').nestable('expandAll');
    //    }
    //    if (action === 'collapse-all') {
    //        $('.dd').nestable('collapseAll');
    //    }
    //});
});
function Default() {
    var isValid = true;
    if ($('#role').val() === '') {
        $('.custom-alert').fadeIn();
        toastr.warning('Please select Role.', '');
        isValid = false;
    }
    if (isValid) {
        var requestURL = '../../../RoleAuthorization/GetDefaultAuthorization';
        var forcomplete = {};
        $.ajax({
            url: requestURL,
            type: "GET",
            dataType: "json",
            data: { id: $('#role').val() },
            success: function (data) {
                forcomplete = data;
            },
            complete: function () {

                $('#tableList tbody tr').remove();
                $.each(forcomplete.ListOfRoleMenus, function (i, item) {
                    var checkedVisible = item.Visible == true ? 'checked = "checked"' : '';
                    var checkedStatus = item.Status == true ? 'checked = "checked"' : '';

                    var $table = $('#tableList tbody');
                    $table.append('<tr>' +
                        '<td><strong id = "menu"> ' + item.MenuName + '</strong >' +
                        '</td><td><strong id="submenu">' + item.SubMenuName + '</strong>' +
                        '</td><td><div class="checkbox checkbox-success checkbox-circle"><input id="chkVisible" type="checkbox" ' + checkedVisible + '><label for="checkbox8"></label></div>' +
                        '</td><td><div class="checkbox checkbox-success checkbox-circle"><input id="chkStatus" type="checkbox" ' + checkedStatus + '><label for="checkbox8"></label></div>' +
                        '</td></tr>');
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
                        // $('#MyModalVat').modal({ backdrop: 'static', keyboard: false });
                    }
                });
            }
        });
    }
};
$('#role').change(function () {
    if ($('#role').val() != '') {
        var requestURL = '../../../RoleAuthorization/GetRoleAuthorization';
        var forcomplete = {};
        $.ajax({
            url: requestURL,
            type: "GET",
            dataType: "json",
            data: { id: $('#role').val() },
            success: function (data) {
                forcomplete = data;
            },
            complete: function () {

                $('#tableList tbody tr').remove();
                $.each(forcomplete.ListOfRoleMenus, function (i, item) {
                    var checkedVisible = item.Visible == true ? 'checked = "checked"' : '';
                    var checkedStatus = item.Status == true ? 'checked = "checked"' : '';

                    var $table = $('#tableList tbody');
                    $table.append('<tr>' +
                        '<td><strong id = "menu"> ' + item.MenuName + '</strong >' +
                        '</td><td><strong id="submenu">' + item.SubMenuName + '</strong>' +
                        '</td><td><div class="checkbox checkbox-success checkbox-circle"><input id="chkVisible" type="checkbox" ' + checkedVisible + '><label for="checkbox8"></label></div>' +
                        '</td><td><div class="checkbox checkbox-success checkbox-circle"><input id="chkStatus" type="checkbox" ' + checkedStatus + '><label for="checkbox8"></label></div>' +
                        '</td></tr>');
                });
                if (forcomplete.ListOfRoleMenus.Count > 0) {
                    $('#authorization').text('Update Authorization').removeClass('btn btn-success').addClass('btn btn-primary');
                }
                else {
                    $('#authorization').text('Create Authorization').removeClass('btn btn-success').addClass('btn btn-primary');
                }
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
                        // $('#MyModalVat').modal({ backdrop: 'static', keyboard: false });
                    }
                });
            }
        });
    }
});
function checkAllVisible(ele) {
    var checkboxes = document.getElementsByTagName('input');

    if (ele.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                if (checkboxes[i].id.toString() == 'chkVisible') {
                    checkboxes[i].checked = true;
                }

            }
        }
    } else {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                if (checkboxes[i].id.toString() == 'chkVisible') {
                    checkboxes[i].checked = false;
                }
            }
        }
    }
}
function checkAllStatus(ele) {
    var checkboxes = document.getElementsByTagName('input');

    if (ele.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                if (checkboxes[i].id.toString() == 'chkStatus') {
                    checkboxes[i].checked = true;
                }

            }
        }
    } else {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                if (checkboxes[i].id.toString() == 'chkStatus') {
                    checkboxes[i].checked = false;
                }
            }
        }
    }
}
$('#authorization').click(function () {
    var title = $('#authorization').text().toLowerCase().split(' ')[0];
    var canceltext = title === 'add' ? 'adding' : 'updating';
    var swaltext = title === 'add' ? 'Adding vat...' : 'Update vat...';
    var messagetext = title === 'add' ? "Error adding vat!" : "Error updating vat!";

    //$('#tableList tbody tr').each(function (index, ele) {
    //    var item = {

    //        }
    //        ItemList.push(item);
    //})
    //if (ItemList.length === 0) {
    //    $('.custom-alert').fadeIn();
    //    toastr.warning('Item Details is required.', '');
    //    isAllValid = false;
    //}
});