
$(document).ready(function () {//decleration of controls
    $('.chosen-select').chosen({ width: "100%" });
    // Bind normal buttons
    Ladda.bind('.ladda-button', { timeout: 2000 });
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
function PopulateItemUoM(row) {
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
            $("#uom-" + row).empty().trigger('chosen:updated');;
            $("#uom-" + row).append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete, function (i, val) {
                $("#uom-" + row).append($('<option/>').val(val.Code).text(val.Name)).trigger('chosen:updated');
            });
        }
    });
}
function PopulateItems(row) {
    var requestURL = '../../../Items/GetItemsInfo';
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
            $("#itemcode-" + row).empty().trigger('chosen:updated');;
            $("#itemcode-" + row).append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete, function (i, val) {
                $("#itemcode-" + row).append($('<option/>').val(val.ItemCode).text(val.ItemCode)).trigger('chosen:updated');
            });
            $("#itemname-" + row).empty().trigger('chosen:updated');;
            $("#itemname-" + row).append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete, function (i, val) {
                $("#itemname-" + row).append($('<option/>').val(val.ItemCode).text(val.ItemName)).trigger('chosen:updated');
            });
        }
    });
}
function PopulateWarehouse(row) {
    var requestURL = '../../../Warehouse/GetWarehouseInfo';
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
            $("#warehouse-" + row).empty().trigger('chosen:updated');;
            $("#warehouse-" + row).append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete, function (i, val) {
                $("#warehouse-" + row).append($('<option/>').val(val.Code).text(val.Name)).trigger('chosen:updated');
            });
        }
    });
}
$('#add').click(function () {
    var requestURL = '../../../SequenceTable/CheckSequence';
    var forcomplete = {};
    var object = 7;
    var documentname = "Inventory Adjustment";
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
                toastr.warning('Document ' + document + '-' + documentname + ' is not available. Please contact your administrator.', '');
                return;
            }
            else {
                disablefield('add', false);
                var date = new Date();
                $('#tableListItem tbody tr').remove();
                $('#invstatus').val("0").trigger("chosen:updated");
                $('#ModalTitle').html('Create Inventory Adjustment');
                disablefield("add", false);
                $('#invadjustment').text('Add Inventory Adjustment').removeClass('btn btn-success').addClass('btn btn-primary');
                clearAll();
                LoadSeries();
                $('#MyModalInvAdjustment').modal({ backdrop: 'static', keyboard: false });
            }
        }
    });
});
$('#invadjustment').click(function () {
    var ItemList = [];
    var paymentlines = [];
    var date = new Date();
    var title = $('#invadjustment').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Inventory Adjustment');
        $('#invadjustment').text('Update Inventory Adjustment').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;


        if ($('#type').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Inventory Type is required.', '');
            isAllValid = false;
        }
        if ($('#series').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Series is required.', '');
            isAllValid = false;
        }
        if (Number($('#docnum').val().trim()) === 0) {

            $('.custom-alert').fadeIn();
            toastr.warning('Document Number is required.', '');
            isAllValid = false;
        }
        //Item Details
        var i = 0;

        $('#tableListItem tbody tr').each(function (index, ele) {
            if ($("#itemcode-" + index, this).val() != "" && $("#itemname-" + index, this).val() != "") {
                var item = {
                    LineNum: i++,
                    ItemCode: $("#itemcode-" + index, this).val().trim(),
                    ItemName: $("#itemname-" + index, this).val().trim(),
                    Quantity: $("#qty-" + index, this).val().trim(),
                    UoM: $("#uom-" + index, this).val().trim(),
                    Whse: $("#warehouse-" + index, this).val().trim(),
                    UnitPrice: Number($("#unitprice-" + index, this).val().replace(/,/g, '')),
                    LineTotal: Number($("#linetotal-" + index, this).val().replace(/,/g, ''))
                }
                ItemList.push(item);
            }
        })
        if (ItemList.length === 0) {
            $('.custom-alert').fadeIn();
            toastr.warning('Item Details is required.', '');
            isAllValid = false;
        }
        if (isAllValid) {
            $("#MyModalInvAdjustment").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding inventory adjustment...' : 'Update inventory adjustment...';
            var messagetext = title === 'add' ? "Error adding inventory adjustment!" : "Error updating inventory adjustment!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " inventory adjustment?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " inventory adjustment!",
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

                    var data = {
                        DocNum: $('#docnum').val(),
                        Series: $('#series').val(),
                        InvAdjustmentNo: $('#invadjno').val(),
                        ObjectType: "7",
                        BranchCode: $('#branch').val(),
                        Type: $('#type option:selected').val().trim(),
                        Reference: $('#reference').val().trim(),
                        Reason: $('#reason').val().trim(),
                        DocTotal: parseFloat($('#total').val().replace(/,/g, '')),
                        Date: $('#date').val().trim(),
                        InvoiceStatus: $('#invstatus option:selected').val(),
                        Lines: ItemList,
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: ''
                    }
                    var requestURL = title === 'add' ? '../../../InvAdjustment/Save' : '../../../InvAdjustment/Update';
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
                            if (forcomplete.HttpStatus === 200) {
                                if (result.value) {
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
                                                var url = '../../../InvAdjustment/Index';
                                                window.location.href = url;
                                            }
                                        }
                                        else {
                                            $('#MyModalInvAdjustment').modal({ backdrop: 'static', keyboard: false });
                                        }
                                    });
                                }
                            }
                            else {
                                $('#MyModalInvAdjustment').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalInvAdjustment').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalInvAdjustment').modal({ backdrop: 'static', keyboard: false });
                }
            });
        };
    }
});
$('#clear').click(function () {
    clearAll();
});
$('#series').change(function () {
    var series = $(this).val();
    var requestURL = '../../../SequenceTable/GetCurrentSequence';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { series: series, objectcode: 7 },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null)
                //var docnum = forcomplete.split('-')[1];
                $('#docnum').val(forcomplete[1]);
            $('#invadjno').val(forcomplete[0]);
            $('#branch').val(forcomplete[2]).trigger("chosen:updated");
        }
    });
});
function getdate(noOfDays) {
    var date = new Date();
    var newdate = new Date(date);

    newdate.setDate(newdate.getDate() + noOfDays);

    var dd = newdate.getDate();
    var mm = newdate.getMonth() + 1;
    var y = newdate.getFullYear();

    var someFormattedDate = mm + '/' + dd + '/' + y;
    return someFormattedDate;
}
function setContent(o) {
    $('#ApiDemoGrid').data('api').clearpersist();
    $('#demoContent').html(o);
}
var Statustoggle = function (model, prop) {
    var status = model[prop];
    //var status = checked === true ? "ACTIVE" : "IN_ACTIVE";
    if (status === "CLOSED") {
        return '<span class="label label-primary">' + status + '</span>';
    } else if (status === "OPEN") {
        return '<span class="label label-warning">' + status + '</span>';
    }
    else if (status === "CANCELLED") {
        return '<span class="label label-danger">' + status + '</span>';
    }
};
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}
function clearAll() {
    var date = new Date();
    var defaultval = 0.00;
    $('#type').val('').trigger("chosen:updated");
    $('#reference').val('');
    $('#reason').val('');
    $('#series').val('').trigger("chosen:updated");
    $('#docnum').val('');
    $('#invstatus').val("0").trigger("chosen:updated");
    $('#date').val(moment(date.Date).format("MM/D/YYYY"));
    $('#total').val(addThousandsSeparator(defaultval.toFixed(2)));
}
function initButtonEvent() {
    $('.edit_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../InvAdjustment/GetInvAdjustment';
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
                    $('#type').val(forcomplete.Type).trigger("chosen:updated");
                    $('#reason').val(forcomplete.Reason);
                    $('#reference').val(forcomplete.Reference);
                    $('#series').val(forcomplete.Series).trigger("chosen:updated");
                    $('#docnum').val(forcomplete.DocNum);
                    $('#invstatus').val(forcomplete.InvoiceStatus);
                    $('#date').val(moment(forcomplete.Date).format('M/DD/YYYY'));
                    $('#ModalTitle').html('Edit Inventory Adjustment');
                    $('#invadjustment').text('Update Inventory Adjustmentist');
                    $('#MyModalInvAdjustment').modal({ backdrop: 'static', keyboard: false });
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
                text: "You want to cancel this inv. adjustment?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, remove inv. adjustment!",
                cancelButtonText: "No, cancel inv. adjustment!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: 'Cancelling inv. adjustment...',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var requestURL = '../../../InvAdjustment/Cancelled';
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
                                title: forcomplete.HttpStatus === 200 ? "Successful" : "Error cancelling inv. adjustment!",
                                text: forcomplete.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (forcomplete.HttpStatus === 200) {
                                    if (result.value) {
                                        var url = '../../../InvAdjustment/Index';
                                        window.location.href = url;
                                    }
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: "Error cancelling inv. adjustment!",
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
            var requestURL = '../../../InvAdjustment/GetInvAdjustment';
            var forcomplete = {};
            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                url: requestURL,
                data: { id: Number(lastSelectedIds) },
                dataType: "json",
                success: function (data) {
                    forcomplete = data;
                },
                complete: function () {
                    $('#type').val(forcomplete.Type).trigger("chosen:updated");
                    $('#reason').val(forcomplete.Reason);
                    $('#reference').val(forcomplete.Reference);
                    $('#series').val(forcomplete.Series).trigger("chosen:updated");
                    $('#docnum').val(forcomplete.DocNum);
                    $('#invstatus').val(forcomplete.InvoiceStatus);
                    $('#date').val(moment(forcomplete.Date).format('M/DD/YYYY'));
                    $('#total').val(addThousandsSeparator(forcomplete.DocTotal.toFixed(2)));
                    // View Item Details
                    $('#tableListItem tbody tr').remove();
                    $.map(forcomplete.Lines, function (val) {
                        var $item = $('#tableListItem tbody');
                        var row = $('#tableListItem').find('tbody > tr').length;
                        $item.append('<tr>' +
                            '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + row + '" onclick="removeItem(this)"><span class="fa fa-remove"></span></a></td>' +
                            '<td><select id="itemcode-' + row + '" onchange="ItemCodeOnChange(this)" value="' + val.ItemCode + '" class="form-control chosen-select"></select></td>' +
                            '<td><select id="itemname-' + row + '" onchange="ItemNameOnChange(this)" value="' + val.ItemName + '" class="form-control chosen-select"></select></td>' +
                            '<td> <input type="text" id="qty-' + row + '" value="' + addThousandsSeparator(val.Quantity.toFixed(2)) + '" class="c4 form-control text-right price" placeholder="0.00" onchange="calculate_row(this)" onkeypress="return isNumberKey(event)" /> </td>' +
                            '<td><select id="uom-' + row + '" value="' + val.UoM + '" class="form-control chosen-select"></select></td>' +
                            '<td> <input type="text" id="unitprice-' + row + '" value="' + addThousandsSeparator(val.UnitPrice.toFixed(2)) + '" class="c4 form-control text-right price" placeholder="0.00" onchange="calculate_row(this)" onkeypress="return isNumberKey(event)" /> </td>' +
                            '<td><select id="warehouse-' + row + '" value="' + val.Whse + '" class="form-control chosen-select"></select></td>' +
                            '<td> <input type="text" id="linetotal-' + row + '" value="' + addThousandsSeparator(val.LineTotal.toFixed(2)) + '" class="c4 form-control text-right price" placeholder="0.00" onchange="calculate_row(this)"  onkeypress="return isNumberKey(event)" disabled /> </td>' +
                            '</tr>');
                        PopulateItemUoM(row);
                        PopulateItems(row);
                        PopulateWarehouse(row);
                    });
                    //>>end
                    $('.chosen-select').chosen({ width: "100%" });
                    disablefield('edit', true);
                    $('#ModalTitle').html('Inventory Adjustment Details');
                    $('#invadjustment').text('Edit Inventory Adjustment').addClass('btn btn-success');
                    $('#MyModalInvAdjustment').modal({ backdrop: 'static', keyboard: false });
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
    $('#type').prop('disabled', pDisable).trigger("chosen:updated");
    $('#reason').prop('disabled', pDisable);
    $('#reference').prop('disabled', pDisable);
    $('#add-item').prop('disabled', pDisable);
    $('#tableListItem').find('tbody > tr').each(function (index) {
        $(this).find('[id^=removes]').attr('disabled', pDisable);
        $(this).find('[id^=itemcode]').attr('disabled', pDisable);
        $(this).find('[id^=itemname]').attr('disabled', pDisable);
        $(this).find('[id^=uom]').attr('disabled', pDisable);
        $(this).find('[id^=qty]').attr('disabled', pDisable);
        $(this).find('[id^=unitprice]').attr('disabled', pDisable);
        $(this).find('[id^=warehouse]').attr('disabled', pDisable);
        $(this).find('[id^=linetotal]').attr('disabled', pDisable);
    });
}
function addThousandsSeparator(input) {
    var num_parts = input.toString().split(".");
    num_parts[0] = num_parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return num_parts.join(".");
}
function LoadSeries() {
    var requestURL = '../../../SequenceTable/GetSeriesBasedOnObject';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { objectcode: 7 },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null) {
                var defaultval = 0;
                var defaultbranch = "";
                $("#series").empty().trigger('chosen:updated');;
                $("#series").append($('<option/>')).trigger('chosen:updated');
                $.each(forcomplete, function (i, val) {
                    if (val.Indicator === true) {
                        defaultval = val.DefaultSeries;
                        defaultbranch = val.BranchCode;
                    }
                    $("#series").append($('<option/>').val(val.Series).text(val.SeriesName)).trigger('chosen:updated');
                });
                $('#series').val(defaultval).trigger("chosen:updated");
                $('#branch').val(defaultbranch).trigger("chosen:updated");
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
        data: { series: series, objectcode: 7 },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null) {
                $('#docnum').val(forcomplete[1]);
                $('#invadjno').val(forcomplete[0]);
            }
        }
    });
}
//Item Details
function AddNewItem() {
    var $item = $('#tableListItem tbody');
    var row = $('#tableListItem').find('tbody > tr').length;
    $item.append('<tr>' +
        '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + row + '" onclick="removeItem(this)"><span class="fa fa-remove"></span></a></td>' +
        '<td><select id="itemcode-' + row + '" onchange="ItemCodeOnChange(this)" class="form-control chosen-select"></select></td>' +
        '<td><select id="itemname-' + row + '" onchange="ItemNameOnChange(this)" class="form-control chosen-select"></select></td>' +
        '<td> <input type="text" id="qty-' + row + '" class="c4 form-control text-right price" placeholder="0.00" onchange="calculate_row(this)" onkeypress="return isNumberKey(event)" /> </td>' +
        '<td><select id="uom-' + row + '" class="form-control chosen-select"></select></td>' +
        '<td> <input type="text" id="unitprice-' + row + '" class="c4 form-control text-right price" placeholder="0.00" onchange="calculate_row(this)" onkeypress="return isNumberKey(event)" /> </td>' +
        '<td><select id="warehouse-' + row + '" class="form-control chosen-select"></select></td>' +
        '<td> <input type="text" id="linetotal-' + row + '" class="c4 form-control text-right price" placeholder="0.00" onchange="calculate_row(this)"  onkeypress="return isNumberKey(event)" disabled /> </td>' +
        '</tr>');
    PopulateItemUoM(row);
    PopulateItems(row);
    PopulateWarehouse(row);
    $('.chosen-select').chosen({ width: "100%" });
}
function removeItem(obj) {
    var rowid = obj.id.split('-')[1];
    $(obj).parent().parent().remove();
    updateIds();
};
function updateIds() {
    $('#tableListItem').find('tbody > tr').each(function (index) {
        $(this).find('[id^=removes]').attr('id', 'removes-' + index);
        $(this).find('[id^=itemcode]').attr('id', 'itemcode-' + index);
        $(this).find('[id^=itemname]').attr('id', 'itemname-' + index);
        $(this).find('[id^=uom]').attr('id', 'uom-' + index);
        $(this).find('[id^=qty]').attr('id', 'qty-' + index);
        $(this).find('[id^=unitprice]').attr('id', 'unitprice-' + index);
        $(this).find('[id^=warehouse]').attr('id', 'warehouse-' + index);
        $(this).find('[id^=linetotal]').attr('id', 'linetotal-' + index);
    });

}
function ItemCodeOnChange(obj) {
    var code = $(obj).val();
    var index = $(obj).attr('id').split('-')[1];
    $('#itemname-' + index).val(code).trigger("chosen:updated");
}
function ItemNameOnChange(obj) {
    var code = $(obj).val();
    var index = $(obj).attr('id').split('-')[1];
    $('#itemcode-' + index).val(code).trigger("chosen:updated");
}
function calculate_row(row) {
    var index = $(row).attr('id').split('-')[1];
    var qty = parseFloat($('#qty-' + index).val().replace(',', ''));
    var unitprice = parseFloat($('#unitprice-' + index).val().replace(',', ''));
    var linetotal = parseFloat($('#linetotal-' + index).val().replace(',', ''));

    if (isNaN(qty)) { qty = 0; }
    if (isNaN(unitprice)) { unitprice = 0; }
    if (isNaN(linetotal)) { linetotal = 0; }

    var total = qty * unitprice;

    $('#qty-' + index).val(addThousandsSeparator(qty.toFixed(2)));
    $('#unitprice-' + index).val(addThousandsSeparator(unitprice.toFixed(2)));
    $('#linetotal-' + index).val(addThousandsSeparator(total.toFixed(2)));
    updatetotal();
}
function updatetotal() {
    var total = 0;
    $('#tableListItem tbody tr').each(function (i, item) {
        total += parseFloat($('#linetotal-' + i, this).val().replace(/,/g, ''));
    });

    if (isNaN(total)) { total = 0; }
    $('#total').val(addThousandsSeparator(total.toFixed(2)));
}