
var general_row_html = "";
var ItemRow = 0;
$(document).ready(function () {//decleration of controls
    $('.chosen-select').chosen({ width: "100%" });
    //var elem = document.querySelector('.js-switch');
    //var switchery = new Switchery(elem, { color: '#1AB394' });
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });
    initButtonEvent();
    general_row_html = $(".general-tab-table").find("tbody").html();
    ItemRowEvent();
});
function RemoveItem() {
    if ($(".general-tab-table").find("tbody").find('tr').length > 1) {
        $(".general-tab-table").find("tbody").find('.selected').parent().remove();
    }

    if ($(".general-tab-table").find("tbody").find('tr').length < 1) {
        AddNewItem();
    }
}
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
            $.map($(this).data('api').getSelection(), function (val) { return val.ItemCode; });
    });
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}
$('#add').click(function () {
    var requestURL = '../../../SequenceTable/CheckSequence';
    var forcomplete = {};
    var object = 1;
    var documentname = "Item Master Data";
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { objectcode: object },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (!forcomplete) {
                $('.custom-alert').fadeIn();
                toastr.warning('Document ' + documentname + ' is not available. Please contact your administrator.', '');
                return;
            }
            else {
                disablefield('add', false);
                $('#status').prop('checked', true);
                //$('#series').val('').trigger("chosen:updated");
                $('#ModalTitle').html('Create Item');
                $('#items').text('Add Item').removeClass('btn btn-success').addClass('btn btn-primary');
                clearAll();
                $('#tableListUoM tbody tr').remove();
                DefaultItemUOM();
                LoadSeries();
                $('#MyModalItems').modal({ backdrop: 'static', keyboard: false });
            }
        }
    });
});
$('#uomgroup').click(function (e) {
    var isAllValid = true;
    if ($('#itemcode').val() === '') {
        $('.custom-alert').fadeIn();
        toastr.warning('Item Code is required.', '');
        isAllValid = false;
    }
    if ($('#itemname').val() === '') {
        $('.custom-alert').fadeIn();
        toastr.warning('Item Name is required.', '');
        isAllValid = false;
    }
    if (isAllValid) {
        $('#MyModalUoM').modal({ backdrop: 'static', keyboard: false });

        //$('#ModalTitle').html("UoM Group - (" + $('#itemcode').val() + "-" + $('#itemname').val().toUpperCase() + ")");
    }
});
$('#series').change(function () {
    var series = $(this).val();
    var requestURL = '../../../SequenceTable/GetCurrentSequence';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { series: series, objectcode: 3 },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null)
                $('#itemcode').val(forcomplete[0]);
        }
    });
});
function LoadSeries() {
    var requestURL = '../../../SequenceTable/GetSeriesBasedOnObject';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { objectcode: 3 },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null) {
                var defaultval = 0;
                $("#series").empty().trigger('chosen:updated');;
                $("#series").append($('<option/>')).trigger('chosen:updated');
                $.each(forcomplete, function (i, val) {
                    if (val.DefaultSeries > 0)
                        defaultval = val.DefaultSeries;
                    $("#series").append($('<option/>').val(val.Series).text(val.SeriesName)).trigger('chosen:updated');
                });
                $('#series').val(defaultval).trigger("chosen:updated");
                LoadCurrentId(defaultval);
            }
        }
    });
}
function LoadCurrentId(series) {
    var requestURL = '../../../SequenceTable/GetCurrentSequence';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { series: series, objectcode: 3 },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null)
                $('#itemcode').val(forcomplete[0]);
        }
    });
}
$('#items').click(function () {
    var listUoM = [];
    var title = $('#items').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Item');
        $('#items').text('Update Item').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;
        var i = 0;
        $('#tableListUoM tbody tr').each(function (index, ele) {
            if ($('#uomcode', this).val() != '') {
                var orderItem = {
                    LineNum: $('#linenum-' + index, this).val(),
                    ItemCode: $('#itemcode').val().trim(),
                    UoM: $('#uomcode-' + index + ' option:selected', this).val(),
                    Quantity: parseFloat($('#qty-' + index, this).val().replace(/,/g, '')),
                    BaseUoM: $('#baseuom-' + index + ' option:selected', this).val(),
                    BaseQty: parseFloat($('#baseqty-' + index, this).val().replace(/,/g, '')),
                    Price: parseFloat($('#price-' + index, this).val().replace(/,/g, '')),
                    TotalPrice: parseFloat($('#totalprice-' + index, this).val().replace(/,/g, '')),
                    isSmallestUoM: index === 0 ? true : false
                }
                listUoM.push(orderItem);
            }
        });
        if ($('#itemcode').val().trim() === '') {
            $('.custom-alert').fadeIn();
            toastr.warning('Item Code is required.', '');
            isAllValid = false;
        }
        if ($('#itemname').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Item Name is required.', '');
            isAllValid = false;
        }
        if ($('#itemgroup').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Group Code is required.', '');
            isAllValid = false;
        }
        if ($('#wtax').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('WTax is required.', '');
            isAllValid = false;
        }
        if ($('#status').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Status is required.', '');
            isAllValid = false;
        }
        if (listUoM.length === 0) {
            $('.custom-alert').fadeIn();
            toastr.warning('UoM Group is required.', '');
            isAllValid = false;
        }
        if (isAllValid) {
            $("#MyModalItems").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding item...' : 'Update item...';
            var messagetext = title === 'add' ? "Error adding item!" : "Error updating item!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " item!",
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
                        ItemCode: $('#itemcode').val().trim(),
                        ItemName: $('#itemname').val().toUpperCase().trim(),
                        GroupCode: $('#itemgroup').val().trim(),
                        WtaxId: $('#wtax').val().trim(),
                        Status: $('#status').is(':checked'),
                        isSellItem: $('#sellitem').is(':checked'),
                        isPurchaseItem: $('#purchitem').is(':checked'),
                        isInvItem: $('#invitem').is(':checked'),
                        ItemUoM: listUoM,
                        ItemOnHandPerWhse: [],
                        Series: $('#series').val(),
                        ObjectType: 3,
                        WholeSaleQty: Number($('#wholesaleqty').val()),
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: ''
                    }
                    var requestURL = title === 'add' ? '../../../Items/Save' : '../../../Items/Update';
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
                                        var url = '../../../Items/Index';
                                        window.location.href = url;
                                    }
                                }
                                else {
                                    $('#MyModalItems').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalItems').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalItems').modal({ backdrop: 'static', keyboard: false });
                }
            });
        };
    }
});
//$('#clear').click(function () {
//    clearAll();
//});
$('#close').click(function () {
    $('#MyModalItems').modal('hide');
});
$('#wholesaleqty').change(function () {
    var qty = parseFloat($(this).val());
    $('#wholesaleqty').val(addThousandsSeparator(qty.toFixed(2)));
});
$('#uomgroupdetail').click(function () {
    var isAllValid = true;
    $('#tableListUoM tbody tr').each(function (index, ele) {
        if (parseFloat($('#baseqty-' + index).val()) === 0 || isNaN(parseFloat($('#baseqty-' + index).val()))) {
            $('.custom-alert').fadeIn();
            toastr.warning('Base qty in row ' + (index + 1) + ' is required.', '');
            isAllValid = false;
        }
        if (parseFloat($('#price-' + index).val()) === 0 || isNaN(parseFloat($('#price-' + index).val()))) {
            $('.custom-alert').fadeIn();
            toastr.warning('Price in row ' + (index + 1) + ' is required.', '');
            isAllValid = false;
        }
        if (parseFloat($('#totalprice-' + index).val()) === 0 || isNaN(parseFloat($('#totalprice-' + index).val()))) {
            $('.custom-alert').fadeIn();
            toastr.warning('Total Price in row ' + (index + 1) + ' is required.', '');
            isAllValid = false;
        }
        if ($('#uomcode-' + index + ' option:selected').val() === "") {
            $('.custom-alert').fadeIn();
            toastr.warning('UoM in row ' + (index + 1) + ' is required.', '');
            isAllValid = false;
        }
        if (parseFloat($('#qty-' + index).val()) === 0 || isNaN(parseFloat($('#qty-' + index).val()))) {
            $('.custom-alert').fadeIn();
            toastr.warning('Qty in row ' + (index + 1) + ' is required.', '');
            isAllValid = false;
        }
    });
    if (isAllValid) {
        $("#MyModalUoM").modal('hide');
    }
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
    $('#itemcode').val('');
    $('#itemname').val('');
    $('#groupcode').val('').trigger('chosen:updated');
    $('#wtax').val('').trigger('chosen:updated');
}
function initButtonEvent() {
    $('.edit_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../Items/GetItems';
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

                    $('#series').val(forcomplete.Series).trigger("chosen:updated");
                    $('#itemcode').val(forcomplete.ItemCode);
                    $('#itemname').val(forcomplete.ItemName);
                    $('#itemgroup').val(forcomplete.GroupCode).trigger("chosen:updated");
                    $('#wtax').val(forcomplete.WtaxId).trigger("chosen:updated");
                    $('#wholesaleqty').val(forcomplete.WholeSaleQty);
                    $('#ModalTitle').html('Edit Item');
                    $('#items').text('Update Item').removeClass('btn btn-success').addClass('btn btn-primary');
                    $('#tableListWarehouse tbody tr').remove();
                    $('#tableListUoM tbody tr').remove();
                    //Display Item On Hand
                    $.map(forcomplete.ItemOnHandPerWhse, function (val) {
                        var $item = $('#tableListWarehouse tbody');
                        //var available = parseFloat(val.OnHand.replace(/,/g, '')) - parseFloat(val.Commited.replace(/,/g, '')) + parseFloat(val.Ordered.replace(/,/g, ''));
                        var available = val.OnHand - val.Commited - val.Ordered;
                        $item.append('<tr>' +
                            '<td>' + val.WhseId + '</td>' +
                            '<td>' + addThousandsSeparator(val.OnHand.toFixed(2)) + '</td>' +
                            '<td>' + addThousandsSeparator(val.Commited.toFixed(2)) + '</td>' +
                            '<td>' + addThousandsSeparator(val.Ordered.toFixed(2)) + '</td>' +
                            '<td>' + addThousandsSeparator(available.toFixed(2)) + '</td>' +
                            '<td>' + addThousandsSeparator(val.ItemCost.toFixed(2)) + '</td>' +
                            '<tr>');
                    });
                    //>>end
                    //Display Item UoM
                    ItemRow = 0;
                    $.map(forcomplete.ItemUoM, function (val) {
                        $item = $('#tableListUoM tbody');
                        if (ItemRow === 0) {
                            $item.append(
                                '<tr>' +
                                '<td></td>' +
                                '<td><input type="text"  class="form-control" id="linenum-' + ItemRow + '" value="' + (ItemRow + 1) + '"disabled/></td>' +
                                '<td><input type="text"  class="form-control" id="qty-' + ItemRow + '" value="' + addThousandsSeparator(val.Quantity.toFixed(2)) + '"  disabled/></td>' +
                                '<td><select id="uomcode-' + ItemRow + '" class="form-control chosen-select" disabled></select></td>' +
                                '<td>=</td>' +
                                '<td><input type="text"  class="form-control" id="baseqty-' + ItemRow + '" value="' + addThousandsSeparator(val.BaseQty.toFixed(2)) + '" disabled/></td>' +
                                '<td><select id="baseuom-' + ItemRow + '" class="form-control chosen-select" onchange="DisplayDefaultUoMVal(getRowID(this));"></select></td>' +
                                '<td><input type="text"  class="form-control" id="price-' + ItemRow + '" value="' + addThousandsSeparator(val.Price.toFixed(2)) + '" onkeypress="return isNumberKey(event)" onchange="updateQtyAndPrice(getRowID(this));" /></td>' +
                                '<td><input type="text"  class="form-control" id="totalprice-' + ItemRow + '" value="' + addThousandsSeparator(val.Price.toFixed(2)) + '" onkeypress="return isNumberKey(event)" onchange="updateQtyAndPrice(getRowID(this));" disabled/></td>' +
                                '</tr>');
                        }
                        else {
                            $item.append(
                                '<tr>' +
                                '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + ItemRow + '" onclick="RemoveItemRow(this)"><span class="fa fa-remove"></span></a></td>' +
                                '<td><input type="text"  class="form-control" id="linenum-' + ItemRow + '" value="' + (ItemRow + 1) + '"disabled/></td>' +
                                '<td><input type="text"  class="form-control" id="qty-' + ItemRow + '" value="' + addThousandsSeparator(val.Quantity.toFixed(2)) + '"  onkeypress="return isNumberKey(event)" onchange="updateQtyAndPrice(getRowID(this));"  /></td>' +
                                '<td><select id="uomcode-' + ItemRow + '" class="form-control chosen-select"></select></td>' +
                                '<td>=</td>' +
                                '<td><input type="text"  class="form-control" id="baseqty-' + ItemRow + '"value="' + addThousandsSeparator(val.BaseQty.toFixed(2)) + '"  onchange="updateQtyAndPrice(getRowID(this));"/></td>' +
                                '<td><select id="baseuom-' + ItemRow + '" class="form-control chosen-select"  disabled></select></td>' +
                                '<td><input type="text"  class="form-control" id="price-' + ItemRow + '" onkeypress="return isNumberKey(event)" value="' + addThousandsSeparator(val.Price.toFixed(2)) + '" onchange="updateQtyAndPrice(getRowID(this));" /></td>' +
                                '<td><input type="text"  class="form-control" id="totalprice-' + ItemRow + '" value="' + addThousandsSeparator(val.TotalPrice.toFixed(2)) + '"onkeypress="return isNumberKey(event)" onchange="updateQtyAndPrice(getRowID(this));" disabled/></td>' +
                                '</tr>'
                            );
                        }
                        PopulateUoM(ItemRow, val.UoM, val.BaseUoM);
                        ItemRow++;
                    });
                    $('#status').prop('checked', forcomplete.Status);
                    $('#sellitem').prop('checked', forcomplete.isSellItem);
                    $('#purchitem').prop('checked', forcomplete.isPurchaseItem);
                    $('#invitem').prop('checked', forcomplete.isInvItem);
                    disablefield("", false);
                    $('#wholesaleqty').val(addThousandsSeparator(forcomplete.WholeSaleQty.toFixed(2)));
                    $('#MyModalItems').modal({ backdrop: 'static', keyboard: false });
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
                text: "You want to remove this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, remove item!",
                cancelButtonText: "No, cancel remove!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: 'Removing Item...',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var requestURL = '../../../Items/Delete';
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
                                title: forcomplete.HttpStatus === 200 ? "Successful" : "Error removing item!",
                                text: forcomplete.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (forcomplete.HttpStatus === 200) {
                                    if (result.value) {
                                        var url = '../../../Items/Index';
                                        window.location.href = url;
                                    }
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: "Error removing item!",
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
            var requestURL = '../../../Items/GetItems';
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
                    $('#series').val(forcomplete.Series).trigger("chosen:updated");
                    $('#itemcode').val(forcomplete.ItemCode);
                    $('#itemname').val(forcomplete.ItemName);
                    $('#itemgroup').val(forcomplete.GroupCode).trigger("chosen:updated");
                    $('#wtax').val(forcomplete.WtaxId).trigger("chosen:updated");
                    $('#tableListWarehouse tbody tr').remove();
                    $('#tableListUoM tbody tr').remove();
                    //Display Item On Hand
                    $.map(forcomplete.ItemOnHandPerWhse, function (val) {
                        var $item = $('#tableListWarehouse tbody');
                        //var available = parseFloat(val.OnHand.replace(/,/g, '')) - parseFloat(val.Commited.replace(/,/g, '')) + parseFloat(val.Ordered.replace(/,/g, ''));
                        var available = val.OnHand - val.Commited - val.Ordered;
                        $item.append('<tr>' +
                            '<td>' + val.WhseId + '</td>' +
                            '<td>' + addThousandsSeparator(val.OnHand.toFixed(2)) + '</td>' +
                            '<td>' + addThousandsSeparator(val.Commited.toFixed(2)) + '</td>' +
                            '<td>' + addThousandsSeparator(val.Ordered.toFixed(2)) + '</td>' +
                            '<td>' + addThousandsSeparator(available.toFixed(2)) + '</td>' +
                            '<td>' + addThousandsSeparator(val.ItemCost.toFixed(2)) + '</td>' +
                            '<tr>');
                    });
                    //>>end

                    //Display Item UoM
                    ItemRow = 0;
                    $.map(forcomplete.ItemUoM, function (val) {
                        $item = $('#tableListUoM tbody');
                        if (ItemRow === 0) {
                            $item.append(
                                '<tr>' +
                                '<td></td>' +
                                '<td><input type="text"  class="form-control" id="linenum-' + ItemRow + '" value="' + (ItemRow + 1) + '"disabled/></td>' +
                                '<td><input type="text"  class="form-control" id="qty-' + ItemRow + '" value="' + addThousandsSeparator(val.Quantity.toFixed(2)) + '"  disabled/></td>' +
                                '<td><select id="uomcode-' + ItemRow + '" class="form-control chosen-select" disabled></select></td>' +
                                '<td>=</td>' +
                                '<td><input type="text"  class="form-control" id="baseqty-' + ItemRow + '" value="' + addThousandsSeparator(val.BaseQty.toFixed(2)) + '" disabled/></td>' +
                                '<td><select id="baseuom-' + ItemRow + '" class="form-control chosen-select" onchange="DisplayDefaultUoMVal(getRowID(this));"></select></td>' +
                                '<td><input type="text"  class="form-control" id="price-' + ItemRow + '" value="' + addThousandsSeparator(val.Price.toFixed(2)) + '" onkeypress="return isNumberKey(event)" onchange="updateQtyAndPrice(getRowID(this));" /></td>' +
                                '<td><input type="text"  class="form-control" id="totalprice-' + ItemRow + '" value="' + addThousandsSeparator(val.Price.toFixed(2)) + '" onkeypress="return isNumberKey(event)" onchange="updateQtyAndPrice(getRowID(this));" disabled/></td>' +
                                '</tr>');
                        }
                        else {
                            $item.append(
                                '<tr>' +
                                '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + ItemRow + '" onclick="RemoveItemRow(this)"><span class="fa fa-remove"></span></a></td>' +
                                '<td><input type="text"  class="form-control" id="linenum-' + ItemRow + '" value="' + (ItemRow + 1) + '"disabled/></td>' +
                                '<td><input type="text"  class="form-control" id="qty-' + ItemRow + '" value="' + addThousandsSeparator(val.Quantity.toFixed(2)) + '"  onkeypress="return isNumberKey(event)" onchange="updateQtyAndPrice(getRowID(this));"  /></td>' +
                                '<td><select id="uomcode-' + ItemRow + '" class="form-control chosen-select"></select></td>' +
                                '<td>=</td>' +
                                '<td><input type="text"  class="form-control" id="baseqty-' + ItemRow + '"value="' + addThousandsSeparator(val.BaseQty.toFixed(2)) + '"  onchange="updateQtyAndPrice(getRowID(this));"/></td>' +
                                '<td><select id="baseuom-' + ItemRow + '" class="form-control chosen-select"  disabled></select></td>' +
                                '<td><input type="text"  class="form-control" id="price-' + ItemRow + '" onkeypress="return isNumberKey(event)" value="' + addThousandsSeparator(val.Price.toFixed(2)) + '" onchange="updateQtyAndPrice(getRowID(this));" /></td>' +
                                '<td><input type="text"  class="form-control" id="totalprice-' + ItemRow + '" value="' + addThousandsSeparator(val.TotalPrice.toFixed(2)) + '"onkeypress="return isNumberKey(event)" onchange="updateQtyAndPrice(getRowID(this));" disabled/></td>' +
                                '</tr>'
                            );
                        }
                        PopulateUoM(ItemRow, val.UoM, val.BaseUoM);
                        ////Check isSmallest
                        //if (val.isSmallestUoM) {
                        //    $('#smallestuom-' + ItemRow).prop("checked", true);
                        //} else {
                        //    $('#smallestuom-' + ItemRow).prop("checked", false);
                        //}
                        ItemRow++;
                    });
                    $('#status').prop('checked', forcomplete.Status);
                    $('#sellitem').prop('checked', forcomplete.isSellItem);
                    $('#purchitem').prop('checked', forcomplete.isPurchaseItem);
                    $('#invitem').prop('checked', forcomplete.isInvItem);
                    $('#wholesaleqty').val(addThousandsSeparator(forcomplete.WholeSaleQty.toFixed(2)));
                    $('#ModalTitle').html('Item Details');
                    $('#items').text('Edit Item').addClass('btn btn-success');
                    $('#MyModalItems').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
}
function disablefield(ptype, pDisable) {
    switch (ptype) {
        case "edit":
        case "add":
            $('#series').prop('disabled', pDisable).trigger("chosen:updated");
            break;
        default:
            $('#series').prop('disabled', !pDisable).trigger("chosen:updated");
            break;
    }
    $('#itemname').prop('disabled', pDisable);
    $('#itemgroup').prop('disabled', pDisable).trigger("chosen:updated");
    $('#uomgroup').prop('disabled', pDisable);
    $('#wtax').prop('disabled', pDisable).trigger("chosen:updated");
    $('#wholesaleqty').prop('disabled', pDisable);
    $('#status').prop('disabled', pDisable);
    $('#invitem').prop('disabled', pDisable);
    $('#sellitem').prop('disabled', pDisable);
    $('#purchitem').prop('disabled', pDisable);
}
function AddNewItem() {
    var row_count = $(".general-tab-table").find("tbody").find('.rowItem').length;
    var new_row_class_name = "rowItem row-" + row_count;
    $(".general-tab-table").find("tbody").append(general_row_html.replace("rowItem", new_row_class_name));
    $(".general-tab-table").find("tbody").find(".row-" + row_count).find('.chosen-container-single').remove();
    $('.chosen-select').chosen({ width: "100%" });
    ItemRowEvent();
}
function ItemRowEvent() {
    $(".general-tab-table").find("tbody").find('.rowItem').find('.general-row-linenum').off().on('click', function () {
        if ($(this).hasClass("selected")) {
            $(this).removeClass("selected");

        }
        else {
            $(this).addClass("selected");
        }
    });
    $('.qty').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
    });
    $('.price').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
    });
}
function calculate_row(row) {
    var $this = $(row);
    var quantity = parseFloat($('#qty', $this).val().replace(',', ''))
    var price = parseFloat($('#price', $this).val().replace(',', ''))

    if (isNaN(quantity)) { quantity = 0; }
    if (isNaN(price)) { price = 0; }
    $('#qty', $this).val(addThousandsSeparator(quantity.toFixed(2)));
    $('#price', $this).val(addThousandsSeparator(price.toFixed(2)));
}
function addThousandsSeparator(input) {
    var num_parts = input.toString().split(".");
    num_parts[0] = num_parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return num_parts.join(".");
}

//MELJUN 03/15/2020 : Update UOM Details
function DefaultItemUOM() {
    $item = $('#tableListUoM tbody');
    $item.append(
        '<tr>' +
        '<td></td>' +
        //'<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + ItemRow + '" onclick="RemoveItemRow(this)"><span class="fa fa-remove"></span></a>&nbsp;&nbsp;' +
        //'<a href="javascript:void(0)" class="btn btn-primary btn-sm" id="setdefault-' + ItemRow + '" onclick="SetDefault(getRowID(this))"><span class="fa fa-user"></span></a></td> ' +
        '<td><input type="text"  class="form-control" id="linenum-' + ItemRow + '" value="' + (ItemRow + 1) + '"disabled/></td>' +
        //'<td><select id="uomcode-' + ItemRow + '" class="form-control chosen-select"></select></td>' +
        '<td><input type="text"  class="form-control" id="qty-' + ItemRow + '" disabled/></td>' +
        '<td><select id="uomcode-' + ItemRow + '" class="form-control chosen-select" disabled></select></td>' +
        '<td>=</td>' +
        '<td><input type="text"  class="form-control" id="baseqty-' + ItemRow + '" disabled/></td>' +
        '<td><select id="baseuom-' + ItemRow + '" class="form-control chosen-select" onchange="DisplayDefaultUoMVal(getRowID(this));"></select></td>' +
        '<td><input type="text"  class="form-control" id="price-' + ItemRow + '" onkeypress="return isNumberKey(event)" onchange="updateQtyAndPrice(getRowID(this));" /></td>' +
        '<td><input type="text"  class="form-control" id="totalprice-' + ItemRow + '" onkeypress="return isNumberKey(event)" onchange="updateQtyAndPrice(getRowID(this));" disabled/></td>' +
        //'<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input disabled id = "smallestuom-' + ItemRow + '" type = "checkbox" disabled> <label for="checkbox8"> </label> </div ></td>' +
        '</tr>'
    );
    PopulateUoM(ItemRow, "", "");
    ItemRow++;
}
function DisplayDefaultUoMVal(row) {
    var baseuom = "";
    if (row === 0) {
        baseuom = $('#baseuom-' + row + ' option:selected').val();
        $('#qty-' + row).val(1.00);
        $('#uomcode-' + row).val(baseuom).trigger("chosen:updated");
        $('#baseqty-' + row).val(1.00);
    }
    $('#tableListUoM tbody tr').each(function (index, ele) {
        if (index > 0) {
            $('#baseuom-' + index).val(baseuom).trigger("chosen:updated");
        }
    });
}
function AddNewItemUOM() {
    var defaultuom = "";
    var defaultprice = 0;
    var isAllValid = true;
    $('#tableListUoM tbody tr').each(function (index, ele) {
        if (index == 0) {
            defaultuom = $('#baseuom-' + index + ' option:selected').val();
            defaultprice = parseFloat($('#price-' + index).val().replace(/,/g, ''));
        }
    });
    if (defaultuom === "" || defaultuom === undefined) {
        $('.custom-alert').fadeIn();
        toastr.warning('Please selecte smallest uom.', '');
        isAllValid = false;
    }
    if (isAllValid) {
        $item = $('#tableListUoM tbody');
        $item.append(
            '<tr>' +
            '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + ItemRow + '" onclick="RemoveItemRow(this)"><span class="fa fa-remove"></span></a></td>' +
            //'<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + ItemRow + '" onclick="RemoveItemRow(this)"><span class="fa fa-remove"></span></a>&nbsp;&nbsp;' +
            //'<a href="javascript:void(0)" class="btn btn-primary btn-sm" id="setdefault-' + ItemRow + '" onclick="SetDefault(getRowID(this))"><span class="fa fa-user"></span></a></td> ' +
            '<td><input type="text"  class="form-control" id="linenum-' + ItemRow + '" value="' + (ItemRow + 1) + '"disabled/></td>' +
            '<td><input type="text"  class="form-control" id="qty-' + ItemRow + '" onkeypress="return isNumberKey(event)" onchange="updateQtyAndPrice(getRowID(this));"  /></td>' +
            '<td><select id="uomcode-' + ItemRow + '" class="form-control chosen-select"></select></td>' +
            '<td>=</td>' +
            '<td><input type="text"  class="form-control" id="baseqty-' + ItemRow + '" onchange="updateQtyAndPrice(getRowID(this));"/></td>' +
            '<td><select id="baseuom-' + ItemRow + '" class="form-control chosen-select"  disabled></select></td>' +
            '<td><input type="text"  class="form-control" id="price-' + ItemRow + '" onkeypress="return isNumberKey(event)" value="' + addThousandsSeparator(defaultprice.toFixed(2)) + '" onchange="updateQtyAndPrice(getRowID(this));" /></td>' +
            '<td><input type="text"  class="form-control" id="totalprice-' + ItemRow + '" onkeypress="return isNumberKey(event)" onchange="updateQtyAndPrice(getRowID(this));" disabled/></td>' +
            //'<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input disabled id = "smallestuom-' + ItemRow + '" type = "checkbox" disabled> <label for="checkbox8"> </label> </div ></td>' +
            '</tr>'
        );
        PopulateUoM(ItemRow, "", defaultuom);
        ItemRow++;
    }
}
function PopulateUoM(row, uom, baseuom) {
    var requestURL = '../../../UoM/GetUoMInfo';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            $("#uomcode-" + row).empty().trigger('chosen:updated');;
            $("#uomcode-" + row).append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete, function (i, val) {
                $("#uomcode-" + row).append($('<option/>').val(val.Code).text(val.Name)).trigger('chosen:updated');
            });
            $("#baseuom-" + row).empty().trigger('chosen:updated');;
            $("#baseuom-" + row).append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete, function (i, val) {
                $("#baseuom-" + row).append($('<option/>').val(val.Code).text(val.Name)).trigger('chosen:updated');
            });

            if (uom != "") {
                $("#uomcode-" + row).val(uom).trigger('chosen:updated');
            }
            if (baseuom != "") {
                $("#baseuom-" + row).val(baseuom).trigger('chosen:updated');
            }

            $('.chosen-select').chosen({ width: "100%" });
        }
    });
}
function getRowID(obj) {
    return $(obj).closest('tr').index();
}
function RemoveItemRow(obj) {
    $(obj).parent().parent().remove();
    updateIds();
}
function updateIds() {
    ItemRow = 0;
    $('#tableListUoM').find('tbody > tr').each(function (index) {
        $(this).find('[id^=removes]').attr('id', 'removes-' + index);
        $(this).find('[id^=setdefault]').attr('id', 'setdefault-' + index);
        $(this).find('[id^=linenum]').attr('id', 'linenum-' + index);
        $(this).find('[id^=linenum]').val(index + 1);
        $(this).find('[id^=uomcode]').attr('id', 'uomcode-' + index);
        $(this).find('[id^=qty]').attr('id', 'qty-' + index);
        $(this).find('[id^=baseuom]').attr('id', 'baseuom-' + index);
        $(this).find('[id^=price]').attr('id', 'price-' + index);
        $(this).find('[id^=smallestuom]').attr('id', 'smallestuom-' + index);
        ItemRow++;
    });
}
function SetDefault(row) {
    $('#tableListUoM').find('tbody > tr').each(function (index) {
        if (row == index && $(this).find('[id^=smallestuom]').is(':checked') === false) {
            $(this).find('[id^=smallestuom]').prop('checked', true);
            //$('#indicator-' + row).prop('checked', true);
        } else {
            $(this).find('[id^=smallestuom]').prop('checked', false);
        }
    })
    //    $(this).find('[id^=removes]').attr('id', 'removes-' + index);
    //    $(this).find('[id^=setdefault]').attr('id', 'setdefault-' + index);
    //});
    //$('#tableListSequenceLines tbody tr').each(function (index, ele) {
    //    if (row == index + 1 && $('#indicator-' + row).is(':checked') === false) {
    //        $('#indicator-' + row).prop('checked', true);
    //    } else {
    //        $('#indicator-' + row).prop('checked', false);
    //    }
    //})
}
function updateQtyAndPrice(row) {
    var qty = parseFloat($('#qty-' + row).val().replace(/,/g, ''));
    var baseqty = parseFloat($('#baseqty-' + row).val().replace(/,/g, ''));
    var price = parseFloat($('#price-' + row).val().replace(/,/g, ''));

    if (isNaN(qty)) { qty = 0; }
    if (isNaN(price)) { price = 0; }
    if (isNaN(baseqty)) { baseqty = 0; }

    var totalprice = baseqty * price;

    if (isNaN(totalprice)) { totalprice = 0; }

    $('#qty-' + row).val(addThousandsSeparator(qty.toFixed(2)));
    $('#baseqty-' + row).val(addThousandsSeparator(baseqty.toFixed(2)));
    $('#price-' + row).val(addThousandsSeparator(price.toFixed(2)));
    $('#totalprice-' + row).val(addThousandsSeparator(totalprice.toFixed(2)));
}
///>>end