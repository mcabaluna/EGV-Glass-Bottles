var general_row_html = "";
var general_row_uom_html = "";
var ItemUomTemporary = [];
$(document).ready(function () {//decleration of controls

    $('.chosen-select').chosen({ width: "100%" });
    // Bind normal buttons
    Ladda.bind('.ladda-button', { timeout: 2000 });

    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });
    initButtonEvent();
    general_row_html = $(".general-tab-table").find("tbody").html();
    general_row_uom_html = $(".general-tab-table-uom").find("tbody").html();
    ItemRowEvent();
    ItemUoMRowEvent();
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
$('#add').click(function () {
    var requestURL = '../../../SequenceTable/CheckSequence';
    var forcomplete = {};
    var custobject = 4;
    var documentname = "Pricelist";
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { objectcode: custobject },
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
                $('#status').prop('checked', true);
                $('#ModalTitle').html('Create Pricelist');
                $('#pricelist').text('Add Pricelist').removeClass('btn btn-success').addClass('btn btn-primary');
                clearAll();
                LoadSeries();
                if ($('#tableListItem tbody tr').length == 0) {
                    LoadItems();
                }
                disablefield('add', false);
                $('#MyModalPricelist').modal({ backdrop: 'static', keyboard: false });
            }
        }
    });


    //document.getElementById("status").checked = true;
    //var series = $('#series').val();
    //var requestURL = '../../../SequenceTable/GetCurrentSequence';
    //var forcomplete = {};
    //$.ajax({
    //    type: "GET",
    //    contentType: "application/json; charset=utf-8",
    //    url: requestURL,
    //    data: { : series  },
    //    dataType: "json",
    //    success: function (data) {
    //        forcomplete = data;
    //    },
    //    complete: function () {
    //        if (forcomplete != null)
    //            $('#pricelistid').val(forcomplete);
    //        $('#status').prop('checked', true);
    //        disablefield(false);
    //        $('#ModalTitle').html('Create Pricelist');
    //        $('#pricelist').text('Add Pricelist').removeClass('btn btn-success').addClass('btn btn-primary');
    //        clearAll();
    //        if ($('#tableListItem tbody tr').length == 0) {
    //            LoadItems();
    //        }
    //        $('#MyModalPricelist').modal({ backdrop: 'static', keyboard: false });
    //    }
    //});
});
$('#pricelist').click(function () {
    var ItemList = [];
    var UoMList = [];
    var title = $('#pricelist').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Pricelist');
        $('#pricelist').text('Update Pricelist').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;

        //if ($('#pricelistid').val().trim() === '') {
        //    $('.custom-alert').fadeIn();
        //    toastr.warning('Pricelist Id is required.', '');
        //    isAllValid = false;
        //}
        if ($('#pricelistname').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Pricelist Name is required.', '');
            isAllValid = false;
        }
        //if ($('#basepricelist').val().trim() === '') {

        //    $('.custom-alert').fadeIn();
        //    toastr.warning('Base Pricelist is required.', '');
        //    isAllValid = false;
        //}
        if (Number($('#factor').val().trim()) === 0) {

            $('.custom-alert').fadeIn();
            toastr.warning('Pricelist Factor is required.', '');
            isAllValid = false;
        }
        if ($('#status').val().trim() === '') {
            $('.custom-alert').fadeIn();
            toastr.warning('Status is required.', '');
            isAllValid = false;
        }
        //Item Details
        var i = 0;
        $('#tableListItem tbody tr').each(function (index, ele) {
            if ($("#uom-" + index, this).val() != "" && $("#price-" + index, this).val() != "" && ($("#wholesaleprice-" + index, this).val() != "" || $("#retailprice-" + index, this).val() != "")) {
                var item = {
                    LineNum: i++,
                    PricelistId: $('#pricelistid').val().trim(),
                    ItemId: $("#itemcode-" + index, this).val().trim(),
                    ItemName: $("#itemname-" + index, this).val().trim(),
                    UoMCode: $("#uom-" + index, this).val().trim(),
                    Price: Number($("#price-" + index, this).val().replace(/,/g, '')),
                    WholesalePrice: Number($("#wholesaleprice-" + index, this).val().replace(/,/g, '')),
                    RetailPrice: Number($("#retailprice-" + index, this).val().replace(/,/g, '')),
                    Status: $("#itemstatus-" + index, this).prop("checked")
                }
                ItemList.push(item);
            }
        })
        if (ItemList.length === 0) {
            $('.custom-alert').fadeIn();
            toastr.warning('Item Details is required.', '');
            isAllValid = false;
        }
        //>>end
        //UoM Details
        var y = 0;
        $.map(ItemUomTemporary, function (value) {
            $.map(ItemList, function (val2) {
                if (val2.ItemId == value.uomitemcode) {
                    if (value.itemuom != "" && (value.uomprice != "" || Number(value.uomprice) > 0)) {
                        var uom = {
                            LineNum: value.uomitemline,
                            PricelistId: $('#pricelistid').val().trim(),
                            ItemId: value.uomitemcode,
                            UoMCode: value.itemuom,
                            Quantity: value.uomqty,
                            Percentage: value.uompercent,
                            Price: Number(value.uomprice),
                            Status: value.uomstatus
                        }
                        UoMList.push(uom);
                    }
                }
            });
        });
        if (UoMList.length === 0) {
            $('.custom-alert').fadeIn();
            toastr.warning('UoM Details is required.', '');
            isAllValid = false;
        }
        //>>end
        if (isAllValid) {
            $("#MyModalPricelist").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding pricelist...' : 'Update pricelist...';
            var messagetext = title === 'add' ? "Error adding pricelist!" : "Error updating pricelist!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " pricelist?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " pricelist!",
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
                        PricelistId: $('#pricelistid').val().trim(),
                        Name: $('#pricelistname').val().toUpperCase().trim(),
                        BasePricelist: $('#basepricelist').val().trim(),
                        Factor: Number($('#factor').val().trim()),
                        Status: $('#status').is(':checked'),
                        Lines: ItemList,
                        UoMs: UoMList,
                        Series: $('#series').val(),
                        ObjectType: 4,
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: ''
                    }
                    var requestURL = title === 'add' ? '../../../Pricelist/Save' : '../../../Pricelist/Update';
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
                                        var url = '../../../Pricelist/Index';
                                        window.location.href = url;
                                    }
                                }
                                else {
                                    $('#MyModalPricelist').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalPricelist').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalPricelist').modal({ backdrop: 'static', keyboard: false });
                }
            });
        };
    }
});
$('#name').change(function () {
    var name = $(this).val();
    $('#oldprname').val(name);
    //$('#basepricelist').val(name).text(name).trigger("chosen:updated");
});
$('#factor').change(function () {
    var factor = parseFloat($(this).val().replace(',', ''));
    if (isNaN(factor)) { factor = 0; }
    $('#factor').val(addThousandsSeparator(factor.toFixed(2)));
});
$('#clear').click(function () {
    clearAll();
});
$('#plclose').click(function () {
    $("#MyModalPricelist").modal('hide');
});
//Load Series and current id : MELJUN 20200312
function LoadSeries() {
    var requestURL = '../../../SequenceTable/GetSeriesBasedOnObject';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { objectcode: 4 },
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
        data: { series: series, objectcode: 4 },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null)
                $('#pricelistid').val(forcomplete[0]);
        }
    });
}
//>>end


/// UoM Details
function DisplayUoM(row) {
    var rowindex = $(row).attr('id').split('-')[1];
    var isAllValid = true;
    var row = $(this).closest('tr');
    if ($('#itemcode-' + rowindex).val() === '') {
        $('.custom-alert').fadeIn();
        toastr.warning('Item Code is required.', '');
        isAllValid = false;
    }
    if ($('#itemname-' + rowindex).val() === '') {
        $('.custom-alert').fadeIn();
        toastr.warning('Item Name is required.', '');
        isAllValid = false;
    }
    if ($('#uom-' + rowindex).val() === '') {
        $('.custom-alert').fadeIn();
        toastr.warning('UoM is required.', '');
        isAllValid = false;
    }
    if (isAllValid) {
        //var checkItemUoM = false;
        //$.map(ItemUomTemporary, function (value) {
        //    if (value.uomitemcode == $('#itemcode-' + rowindex).val() && value.uomitemline == rowindex) {
        //        checkItemUoM = true;
        //    }
        //});
        //if (checkItemUoM == false) {// || ItemUomTemporary.length === 0) {
        //    var additemuom = {
        //        uomitemline: rowindex,
        //        uomitemcode: $('#itemcode-' + rowindex).val(),
        //        itemuom: $('#uom-' + rowindex).val(),
        //        uomqty: Number(1),
        //        uompercent: Number(0),
        //        uomprice: Number($('#price-' + rowindex).val().replace(/,/g, '')),
        //        status: $('#status-' + rowindex).prop("checked")
        //    };
        //    ItemUomTemporary.push(additemuom);

        //if (ItemUomTemporary.length > 0) {
        //Check if Item is already in the Table UoM if not then perform add
        $('#tableListUoM tbody tr').remove();
        $.map(ItemUomTemporary, function (value) {
            //$('#tableListUoM tbody tr').each(function (index, ele) {
            if ($('#itemcode-' + rowindex).val() == value.uomitemcode && rowindex == value.uomitemid) {
                var $item = $('#tableListUoM tbody');
                var uomrow = $('#tableListUoM > tbody >tr').index() + 1;
                $item.append(
                    '<tr>' +
                    '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removeuom-' + rowindex + '" onclick="removeUoM(this)"><span class="fa fa-remove"></span></a></td>' +
                    '<td><input type="hidden" id="uomitemcode-' + value.uomitemid + '" value="' + value.uomitemcode + '" /><input type="hidden" id="uomitemline-' + value.uomitemid +
                    '"value="' + value.uomitemcode + '" /><input type="hidden" id="uomlineid-' + value.uomitemid + '" /><select id="itemuom-' + value.uomitemid +
                    '" class="form-control chosen-select"></select></td>' +
                    '<td><input type="text" id="uomqty-' + value.uomitemid + '" class="c4 form-control text-right" placeholder="0.00" onkeypress="return isNumberKey(event)" /></td>' +
                    '<td><input type="text" id="uompercent-' + value.uomitemid + '" class="c4 form-control text-right" placeholder="0.00" onkeypress="return isNumberKey(event)" /></td>' +
                    '<td><input type="text" id="uomprice-' + value.uomitemid + '" class="c4 form-control text-right" placeholder="0.00" onkeypress="return isNumberKey(event)" /></td>' +
                    '<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input id = "uomstatus-' + value.uomitemid + '" type = "checkbox" checked > <label for="checkbox8"> </label> </div ></td>' +
                    '</tr>');
                PopulateItemUoM(rowindex, value.itemuom);
                $("#uomqty-" + rowindex).val(addThousandsSeparator(value.uomqty.toFixed(2)));
                $("#uompercent-" + rowindex).val(addThousandsSeparator(value.uompercent.toFixed(2)));
                $("#uomprice-" + rowindex).val(addThousandsSeparator(value.uomprice.toFixed(2)));

            }
            //})
        });
        //>>end
        //}
        //}
        $('#ModalTitleItemUoM').html("Pricelist - " + $('#itemcode-' + rowindex).val() + " - UoM");
        $('#MyModalItemUoM').modal();
    };
}
function PopulateItemUoM(row, itemuom) {
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
            $("#itemuom-" + row).empty().trigger('chosen:updated');;
            $("#itemuom-" + row).append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete, function (i, val) {
                $("#itemuom-" + row).append($('<option/>').val(val.Code).text(val.Name)).trigger('chosen:updated');
            });
            $('.chosen-select').chosen({ width: "100%" });
            $("#itemuom-" + row).val(itemuom).trigger("chosen:updated");
        }
    });
}
function PopulateItemUomDetails(items) {
    var y = 0;
    for (var i = 0; i < items.length; i++) {
        $.map(items[i].ItemUoM, function (value) {
            var additemuom = {
                uomitemid: y++,
                uomitemline: value.LineNum,
                uomitemcode: value.ItemCode,
                itemuom: value.UoM,
                uomqty: value.Quantity,
                uompercent: Number(0),
                uomprice: value.Price,
                uomstatus: true
            };
            ItemUomTemporary.push(additemuom);
        });
    }

    $('#MyModalPricelist').modal({ backdrop: 'static', keyboard: false });
}
function removeItemUoM(itemcode) {
    $.map(ItemUomTemporary, function (val) {
        if (val.ItemCode == itemcode) {
            $(this).remove(val);
        }
    });
}
function AddNewItemUoM(row) {
    $.map(ItemUomTemporary, function (value) {
        //$('#tableListUoM tbody tr').each(function (index, ele) {
        if ($('#itemcode-' + rowindex).val() == value.uomitemcode && rowindex == value.uomitemid) {
            var $item = $('#tableListUoM tbody');
            var uomrow = $('#tableListUoM > tbody >tr').index() + 1;
            $item.append(
                '<tr>' +
                '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removeuom-' + rowindex + '" onclick="removeUoM(this)"><span class="fa fa-remove"></span></a></td>' +
                '<td><input type="hidden" id="uomitemcode-' + value.uomitemid + '" value="' + value.uomitemcode + '" /><input type="hidden" id="uomitemline-' + value.uomitemid +
                '"value="' + value.uomitemcode + '" /><input type="hidden" id="uomlineid-' + value.uomitemid + '" /><select id="itemuom-' + value.uomitemid +
                '" class="form-control chosen-select"></select></td>' +
                '<td><input type="text" id="uomqty-' + value.uomitemid + '" class="c4 form-control text-right" placeholder="0.00" onkeypress="return isNumberKey(event)" /></td>' +
                '<td><input type="text" id="uompercent-' + value.uomitemid + '" class="c4 form-control text-right" placeholder="0.00" onkeypress="return isNumberKey(event)" /></td>' +
                '<td><input type="text" id="uomprice-' + value.uomitemid + '" class="c4 form-control text-right" placeholder="0.00" onkeypress="return isNumberKey(event)" /></td>' +
                '<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input id = "uomstatus-' + value.uomitemid + '" type = "checkbox" checked > <label for="checkbox8"> </label> </div ></td>' +
                '</tr>');
            PopulateItemUoM(rowindex, value.itemuom);
            $("#uomqty-" + rowindex).val(addThousandsSeparator(value.uomqty.toFixed(2)));
            $("#uompercent-" + rowindex).val(addThousandsSeparator(value.uompercent.toFixed(2)));
            $("#uomprice-" + rowindex).val(addThousandsSeparator(value.uomprice.toFixed(2)));

        }
        //})
    });
}
// Items Details
function LoadItems() {
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

            $('#tableListItem tbody tr').remove();
            $('#tableListUoM tbody tr').remove();
            ItemUomTemporary = [];
            for (var i = 0; i < forcomplete.length; i++) {
                var $item = $('#tableListItem tbody');
                $item.append(
                    '<tr> ' +
                    '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + i + '" onclick="removeItem(this)"><span class="fa fa-remove"></span></a></td>' +
                    '<td> <a href="javascript:void(0)" class="btn btn-primary btn-sm" data-toggle="tooltip" data-placement="top" title="Item UoM"><span class="fa fa-plus-circle" id="displayuom-' + i + '" onclick="DisplayUoM(this)"></span></a></td>' +
                    '<td><input type="text" class="form-control" id="itemcode-' + i + '" value="' + forcomplete[i].ItemCode + '" disabled /></td>' +
                    '<td><input type="text" class="form-control" id="itemname-' + i + '" value="' + forcomplete[i].ItemName + '" disabled /></td>' +
                    '<td><select id="uom-' + i + '" class="form-control chosen-select"></select></td>' +
                    '<td> <input type="text" id="price-' + i + '" class="c4 form-control text-right price" placeholder="0.00" onchange="calculate_row(this)" onkeypress="return isNumberKey(event)" /> </td>' +
                    '<td> <input type="text" id="wholesaleprice-' + i + '"class="c4 form-control text-right wholesaleprice" placeholder="0.00"  onchange="calculate_row(this)" onkeypress="return isNumberKey(event)" /> </td> ' +
                    '<td> <input type="text" id="retailprice-' + i + '" class="c4 form-control text-right retailprice" placeholder="0.00" onchange="calculate_row(this)" onkeypress="return isNumberKey(event)" /></td>' +
                    '<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input id = "itemstatus-' + i + '" type = "checkbox" checked > <label for="checkbox8"> </label> </div ></td>' +
                    '</tr>'
                );
            }
            PopulateUoM(forcomplete);
            PopulateItemUomDetails(forcomplete);
        }
    });
}
function PopulateUoM(items) {
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
            for (var b = 0; b < items.length; b++) {
                $("#uom-" + b).empty().trigger('chosen:updated');;
                $("#uom-" + b).append($('<option/>')).trigger('chosen:updated');
                $.each(forcomplete, function (i, val) {
                    $("#uom-" + b).append($('<option/>').val(val.Code).text(val.Name)).trigger('chosen:updated');
                });
                $.each(items[b].ItemUoM, function (i, val) {
                    if (val.UoM == "PIECE" && val.Quantity == 1) {
                        $("#uom-" + b).val(val.UoM).trigger('chosen:updated');
                        var price = parseFloat(val.Price);
                        if (isNaN(price)) { price = 0; }
                        $('#price-' + b).val(addThousandsSeparator(price.toFixed(2)));
                    }
                })
            }
            $('.chosen-select').chosen({ width: "100%" });
        }
    });
}
function getRowID(obj) {
    return $(obj).closest('tr').index();
}
function removeItem(obj) {
    var rowid = obj.id.split('-')[1];
    var itemcode = "";
    $(obj).parent().parent().remove();
    removeItemUoM(itemcode);
    updateIds();
};

function updateIds() {
    ItemRow = 0;
    $('#tableListItem').find('tbody > tr').each(function (index) {
        $(this).find('[id^=removes]').attr('id', 'removes-' + index);
        $(this).find('[id^=displayuom]').attr('id', 'displayuom-' + index);
        $(this).find('[id^=itemcode]').attr('id', 'itemcode-' + index);
        $(this).find('[id^=itemname]').attr('id', 'itemname-' + index);
        $(this).find('[id^=uom]').attr('id', 'uom-' + index);
        $(this).find('[id^=price]').attr('id', 'price-' + index);
        $(this).find('[id^=wholesaleprice]').attr('id', 'wholesaleprice-' + index);
        $(this).find('[id^=retailprice]').attr('id', 'retailprice-' + index);
        $(this).find('[id^=itemstatus]').attr('id', 'itemstatus-' + index);
        ItemRow++;
    });

}
function RefreshItems() {
    LoadItems();
}

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
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}
function clearAll() {
    var date = new Date();
    //$('#pricelistid').val('');
    $('#pricelistname').val('');
    $('#basepricelist').val('').trigger('chosen:updated');
    $('#factor').val('');
}
function initButtonEvent() {
    $('.edit_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../Pricelist/GetPricelist';
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
                    $('#pricelistid').val(forcomplete.PricelistId);
                    $('#pricelistname').val(forcomplete.Name);
                    $('#basepricelist').val(forcomplete.BasePricelist).trigger('chosen:updated');
                    $('#factor').val(forcomplete.Factor);
                    $('#status').prop('checked', forcomplete.Status);

                    LoadItems();
                    ////Load Items and UoM Display
                    //$('#tableListItem tbody tr').remove();
                    //$('#tableListUoM tbody tr').remove();
                    //ItemUomTemporary = [];
                    //for (var i = 0; i < forcomplete.Lines.length; i++) {
                    //    var $item = $('#tableListItem tbody');
                    //    $item.append(
                    //        '<tr> ' +
                    //        '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + i + '" onclick="removeItem(this)"><span class="fa fa-remove"></span></a></td>' +
                    //        '<td> <a href="javascript:void(0)" class="btn btn-primary btn-sm" data-toggle="tooltip" data-placement="top" title="Item UoM"><span class="fa fa-plus-circle" id="displayuom-' + i + '" onclick="DisplayUoM(this)"></span></a></td>' +
                    //        '<td><input type="text" class="form-control" id="itemcode-' + i + '" value="' + forcomplete.Lines[i].ItemId + '" disabled /></td>' +
                    //        '<td><input type="text" class="form-control" id="itemname-' + i + '" value="' + forcomplete.Lines[i].ItemName + '" disabled /></td>' +
                    //        '<td><select id="uom-' + i + '" class="form-control chosen-select"></select></td>' +
                    //        '<td> <input type="text" id="price-' + i + '" class="c4 form-control text-right price" placeholder="0.00" onchange="calculate_row(this)" onkeypress="return isNumberKey(event)" /> </td>' +
                    //        '<td> <input type="text" id="wholesaleprice-' + i + '"class="c4 form-control text-right wholesaleprice" placeholder="0.00"  onchange="calculate_row(this)" onkeypress="return isNumberKey(event)" /> </td> ' +
                    //        '<td> <input type="text" id="retailprice-' + i + '" class="c4 form-control text-right retailprice" placeholder="0.00" onchange="calculate_row(this)" onkeypress="return isNumberKey(event)" /></td>' +
                    //        '<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input id = "itemstatus-' + i + '" type = "checkbox" checked > <label for="checkbox8"> </label> </div ></td>' +
                    //        '</tr>'
                    //    );
                    //}
                    //InitDisplayUoM(forcomplete.Lines);
                    //PopulateItemUomDetails(forcomplete.UoM);
                    disablefield("", false);
                    $('#ModalTitle').html('Edit Pricelist');
                    $('#pricelist').text('Update Pricelist');
                    $('#MyModalPricelist').modal({ backdrop: 'static', keyboard: false });
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
                text: "You want to remove this pricelist?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, remove pricelist!",
                cancelButtonText: "No, cancel remove!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: 'Removing pricelist...',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var requestURL = '../../../Pricelist/Delete';
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
                                title: forcomplete.HttpStatus === 200 ? "Successful" : "Error removing pricelist!",
                                text: forcomplete.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (forcomplete.HttpStatus === 200) {
                                    if (result.value) {
                                        var url = '../../../Pricelist/Index';
                                        window.location.href = url;
                                    }
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: "Error removing pricelist!",
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
            var requestURL = '../../../Pricelist/GetPricelist';
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
                    $('#pricelistid').val(forcomplete.PricelistId);
                    $('#pricelistname').val(forcomplete.Name);
                    $('#basepricelist').val(forcomplete.BasePricelist).trigger('chosen:updated');
                    $('#factor').val(forcomplete.Factor);
                    $('#status').prop('checked', forcomplete.Status);

                    LoadItems();
                    disablefield('edit', true);
                    $('#ModalTitle').html('Pricelist Details');
                    $('#pricelist').text('Edit Pricelist').addClass('btn btn-success');
                    $('#MyModalPricelist').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
}
function InitDisplayUoM(items) {
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
            for (var b = 0; b < items.length; b++) {
                $("#uom-" + b).empty().trigger('chosen:updated');;
                $("#uom-" + b).append($('<option/>')).trigger('chosen:updated');
                $.each(forcomplete, function (i, val) {
                    $("#uom-" + b).append($('<option/>').val(val.Code).text(val.Name)).trigger('chosen:updated');
                });
                //$.each(items[b].ItemUoM, function (i, val) {
                //    if (val.UoM == "PIECE" && val.Quantity == 1) {
                //        $("#uom-" + b).val(val.UoM).trigger('chosen:updated');
                //        var price = parseFloat(val.Price);
                //        if (isNaN(price)) { price = 0; }
                //        $('#price-' + b).val(addThousandsSeparator(price.toFixed(2)));
                //    }
                //})
            }
            $('.chosen-select').chosen({ width: "100%" });
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
    $('#pricelistname').prop('disabled', pDisable);
    $('#basepricelist').prop('disabled', pDisable).trigger("chosen:updated");
    $('#factor').prop('disabled', pDisable);
    $('#status').prop('disabled', pDisable);
}
function addThousandsSeparator(input) {
    var num_parts = input.toString().split(".");
    num_parts[0] = num_parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return num_parts.join(".");
}
//Item Details
function AddNewItem() {
    var row_count = $(".general-tab-table").find("tbody").find('.rowItem').length;
    var new_row_class_name = "rowItem row-" + row_count;
    $(".general-tab-table").find("tbody").append(general_row_html.replace("rowItem", new_row_class_name));
    $(".general-tab-table").find("tbody").find(".row-" + row_count).find('.chosen-container-single').remove();
    $('.chosen-select').chosen({ width: "100%" });
    ItemRowEvent();
}
function RemoveItem() {
    if ($(".general-tab-table").find("tbody").find('tr').length > 1) {
        $(".general-tab-table").find("tbody").find('.selected').parent().remove();
    }

    if ($(".general-tab-table").find("tbody").find('tr').length < 1) {
        AddNewItem();
    }
}
function ItemRowEvent() {
    $(".general-tab-table").find("tbody").find('.rowItem').find('.general-row-linenum-item').off().on('click', function () {
        if ($(this).hasClass("selected")) {
            $(this).removeClass("selected");

        }
        else {
            $(this).addClass("selected");
        }
    });
    $('.price').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
    });
    $('.wholesaleprice').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
    });
    $('.retailprice').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
    });
    $('.itemcode').change(function () {
        var row = $(this).closest('tr');
        $('#itemname', row).val($('#itemcode', row).val()).trigger("chosen:updated");
    });
    $('.itemname').change(function () {
        var row = $(this).closest('tr');
        $('#itemcode', row).val($('#itemname', row).val()).trigger("chosen:updated");
    });

}
function calculate_row(row) {
    var index = $(row).attr('id').split('-')[1];
    var price = parseFloat($('#price-' + index).val().replace(',', ''));
    var wholesaleprice = parseFloat($('#wholesaleprice-' + index).val().replace(',', ''));
    var retailprice = parseFloat($('#retailprice-' + index).val().replace(',', ''));
    if (isNaN(price)) { price = 0; }
    if (isNaN(wholesaleprice)) { wholesaleprice = 0; }
    if (isNaN(retailprice)) { retailprice = 0; }
    $('#price-' + index).val(addThousandsSeparator(price.toFixed(2)));
    $('#wholesaleprice-' + index).val(addThousandsSeparator(wholesaleprice.toFixed(2)));
    $('#retailprice-' + index).val(addThousandsSeparator(retailprice.toFixed(2)));
}
//UoM Details
//function AddNewItemUoM() {
//    var row_count = $(".general-tab-table-uom").find("tbody").find('.rowItemUoM').length;
//    var new_row_class_name = "rowItemUoM row-" + row_count;
//    $(".general-tab-table-uom").find("tbody").append(general_row_uom_html.replace("rowItemUoM", new_row_class_name));
//    $(".general-tab-table-uom").find("tbody").find(".row-" + row_count).find('.chosen-container-single').remove();
//    $('.chosen-select').chosen({ width: "100%" });
//    ItemUoMRowEvent();
//}
function RemoveItemUoM() {
    if ($(".general-tab-table-uom").find("tbody").find('tr').length > 1) {
        $(".general-tab-table-uom").find("tbody").find('.selected').parent().remove();
    }

    if ($(".general-tab-table-uom").find("tbody").find('tr').length < 1) {
        AddNewItemUoM();
    }
}
function ItemUoMRowEvent() {
    $(".general-tab-table-uom").find("tbody").find('.rowItemUoM').find('.general-row-linenum-uom').off().on('click', function () {
        if ($(this).hasClass("selected")) {
            $(this).removeClass("selected");

        }
        else {
            $(this).addClass("selected");
        }
    });
    $('.uomprice').change(function () {
        var row = $(this).closest('tr');
        calculate_rowuom(row);
    });
}
function calculate_rowuom(row) {
    var $this = $(row);
    var price = parseFloat($('#uomprice', $this).val().replace(',', ''));
    var percent = parseFloat($('#uompercent', $this).val().replace(',', ''));
    var total = 0;
    if (isNaN(price)) { price = 0; }
    if (isNaN(percent)) { percent = 0; }
    //if (percent > 0) {
    //    total = 
    //}
    $('#uomprice', $this).val(addThousandsSeparator(price.toFixed(2)));
}   