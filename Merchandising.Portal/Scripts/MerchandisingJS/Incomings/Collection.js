var general_row_html = "";
var ItemUomTemporary = [];
$(document).ready(function () {//decleration of controls
    $('.chosen-select').chosen({ width: "100%" });

    // Bind normal buttons
    Ladda.bind('.ladda-button', { timeout: 2000 });

    //var elem = document.querySelector('.js-switch');
    //var switchery = new Switchery(elem, { color: '#1AB394' });
    //$('.i-checks').iCheck({
    //    checkboxClass: 'icheckbox_square-green',
    //    radioClass: 'iradio_square-green',
    //});
    initButtonEvent();
    general_row_html = $(".general-tab-table").find("tbody").html();
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
var lastStatus;
$('#ApiDemoGrid').on('aweselect',
    function () {
        lastSelectedIds =
            $.map($(this).data('api').getSelection(), function (val) { return val.DocEntry; });
    });
$('#data_1 .input-group.date').datepicker({
    todayBtn: "linked",
    keyboardNavigation: false,
    forceParse: false,
    calendarWeeks: true,
    autoclose: true
});
$('#add').click(function () {
    var requestURL = '../../../SequenceTable/CheckSequence';
    var forcomplete = {};
    var documentname = "Collection";
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { objectcode: 5 },
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
                $('#ModalTitle').html('Create Collection');
                $('#tableList tbody tr').remove();
                $('#incoming').text('Add Collection').removeClass('btn btn-success').addClass('btn btn-primary');
                clearAll();
                LoadSeries();
                $('#MyModalPayment').modal({ backdrop: 'static', keyboard: false });
            }
        }
    });
});
$('#close').click(function () {
    $('#MyModalPayment').modal('hide');
});
$('#collection').click(function () {
    var pay = 0;
    var isAllValid = true;

    if ($('#cardcode option:selected').val().trim() === '') {

        $('.custom-alert').fadeIn();
        toastr.warning('Card Code is required.', '');
        isAllValid = false;
    }
    if ($('#cardname option:selected').val().trim() === '') {

        $('.custom-alert').fadeIn();
        toastr.warning('Card Name is required.', '');
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
    //Payment Details
    var i = 0;
    $('#tableList tbody tr').each(function (index, ele) {
        if ($("#amountpay-" + index, this).val() != 0) {
            pay += parseFloat($("#amountpay-" + index, this).val().replace(/,/g, ''));
        }
    })
    if (pay === 0) {
        $('.custom-alert').fadeIn();
        toastr.warning('No Invoice selected!.', '');
        isAllValid = false;
    }
    //>>end
    if (isAllValid) {
        $('#amountpaid').val($('#total').val());
        $('#status').val(CheckStatus()).trigger("chosen:updated");
        GetCashBalance();
        $('#ModalTitle').html('Collections');
        $('#collection').text('Collection').removeClass('btn btn-success').addClass('btn btn-primary');
        //clearAll();
        $('#MyModalCollection').modal({ backdrop: 'static', keyboard: false });
    }
});
$('#incoming').click(function () {
    var ItemList = [];
    var paymentlines = [];
    var date = new Date();
    var title = $('#incoming').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Collection');
        $('#incoming').text('Update Collection').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;

        if ($('#cardcode option:selected').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Card Code is required.', '');
            isAllValid = false;
        }
        if ($('#cardname option:selected').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Card Name is required.', '');
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
        if ($('#modeofpayment option:selected').val() == "") {
            $('.custom-alert').fadeIn();
            toastr.warning('Mode of Payment is required.', '');
            isAllValid = false;
        }
        //Payment Details
        var i = 0;

        //$('#tableList tbody tr').each(function (index, ele) {
        //    if ($("#amountpay-" + index, this).val() != 0) {
        //        var item = {
        //            LineNum: i++,
        //            ItemCode: $('#itemcode', this).val().trim(),
        //            ItemName: $("#itemname", this).val().trim(),
        //            Quantity: $("#qty", this).val().trim(),
        //            UoM: $('#uom', this).val().trim(),
        //            UnitPrice: Number($("#unitprice", this).val().replace(/,/g, '')),
        //            Discount: Number($("#linediscount", this).val().replace(/,/g, '')),
        //            PriceAfterDiscount: Number($("#priceafdiscount", this).val().replace(/,/g, '')),
        //            LineTotal: Number($("#linetotal", this).val().replace(/,/g, '')),
        //            WTax: Number($("#linewtax", this).val().replace(/,/g, '')),
        //            Vat: Number($("#linevat", this).val().replace(/,/g, '')),
        //            GrossPrice: Number($("#linegross", this).val().replace(/,/g, '')),
        //            GrossTotal: Number($("#linegrosstotal", this).val().replace(/,/g, ''))
        //        }
        //        ItemList.push(item);
        //    }
        //})
        //if (ItemList.length === 0) {
        //    $('.custom-alert').fadeIn();
        //    toastr.warning('Item Details is required.', '');
        //    isAllValid = false;
        //}
        //Payment Details
        var y = 0;
        $('#tableList tbody tr').each(function (index, ele) {
            if ($("#amountpay-" + index, this).val() != 00) {
                //lines payment
                var payline = {
                    LineNum: y++,
                    DocNum: $('#invdocnum-' + index).val(),
                    InvType: "SI",
                    DocTotal: parseFloat($('#grosstotal-' + index).val().replace(/,/g, '')),
                    Collections: parseFloat($('#amountpay-' + index).val().replace(/,/g, '')),
                    Balance: parseFloat($('#balance-' + index).val().replace(/,/g, '')) - parseFloat($('#amountpay-' + index).val().replace(/,/g, '')),
                    SumApplied: parseFloat($('#amountpay-' + index).val().replace(/,/g, '')),
                    InvoiceNo: $('#invno-' + index).val().trim()
                };
                paymentlines.push(payline);
            }
        });
    }
    //>>end
    if (isAllValid) {
        $("#MyModalPayment").modal('hide');
        var canceltext = title === 'add' ? 'adding' : 'updating';
        var swaltext = title === 'add' ? 'Adding collection payment...' : 'Updating collection payment...';
        var messagetext = title === 'add' ? "Error adding collection payment!" : "Error updating collection payment!";
        swal({
            title: "Are you sure?",
            text: "You want to " + title + " this collection?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, " + title + " this collection!",
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
                //header payment
                var data = {
                    BranchCode: $('#series').val().split("|")[1].trim(),
                    ObjectType: "8",
                    Series: $('#series').val().split("|")[0].trim(),
                    CardCode: $('#cardcode option:selected').text().trim(),
                    CardName: $('#cardname option:selected').text().trim(),
                    PaymentNo: $('#pino').val(),
                    //InvoiceType: "SI",
                    DueDate: $('#duedate').val().trim(),
                    //GrossTotal: parseFloat($('#total').val().replace(/,/g, '')),
                    //Collections: parseFloat($('#total').val().replace(/,/g, '')),
                    //Balance: parseFloat($('#total').val().replace(/,/g, '')),// - parseFloat($('#amountpaid').val().replace(/,/g, '')),
                    //DocTotal: parseFloat($('#total').val().replace(/,/g, '')),
                    AmountPaid: parseFloat($('#total').val().replace(/,/g, '')),
                    ModeOfPayment: $('#modeofpayment').val().trim(),
                    DatePaid: $('#date').val().trim(),
                    Remarks: $('#remarks').val().trim(),
                    Status: true,
                    Lines: paymentlines,
                    CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                    CreatedById: '',
                    ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                    ModifiedById: ''
                }
                var requestURL = title === 'add' ? '../../../Incomings/Save' : '../../../Incomings/Update';
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
                                    var url = '../../../Incomings/Collection';
                                    window.location.href = url;
                                }
                            }
                            else {
                                $('#MyModalPayment').modal({ backdrop: 'static', keyboard: false });
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
                                $('#MyModalPayment').modal({ backdrop: 'static', keyboard: false });
                            }
                        });
                    }
                });
            }
            else {
                $('#MyModalPayment').modal({ backdrop: 'static', keyboard: false });
            }
        });
    };
});
$('#clear').click(function () {
    clearAll();
});
$('#cardcode').change(function () {
    var code = $(this).val();
    $('#cardname').val(code).trigger("chosen:updated");
    var branch = $('#series option:selected').val().split("|")[1];
    var requestURL = '../../../Incomings/GetInvoice';
    var type = "GET";
    var forcomplete = {};
    $.ajax({
        type: type,
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { cardcode: code, branch: branch },
        dataType: "json",
        success: function (data2) {
            forcomplete = data2;
        },
        complete: function () {
            $('#tableList tbody tr').remove();
            $.map(forcomplete, function (val) {
                var $item = $('#tableList tbody');
                var row = $('#tableList').find('tbody > tr').length;
                $item.append('<tr>' +
                    '<td><input type="hidden" id="invno-' + row + '"value="' + val.InvoiceNo + '"/><input type="hidden" id="invdocnum-' + row + '" value="' + val.DocNum + '"/>' + val.InvoiceNo + '</td>' +
                    //'<td><input id="duedate-' + val.DueDate + '"/>' + val.DueDate + '</td>' +
                    '<td><input type="hidden" id="duedate-' + row + '" value="' + moment(val.DueDate).format('MM/DD/YYYY') + '"/>' + moment(val.DueDate).format('MM/DD/YYYY') + '</td>' +
                    '<td><input type="hidden" id="grosstotal-' + row + '" value="' + addThousandsSeparator(val.GrossTotal.toFixed(2)) + '"/>' + addThousandsSeparator(val.GrossTotal.toFixed(2)) + '</td>' +
                    '<td><input type="hidden" id="collection-' + row + '" value="' + addThousandsSeparator(val.Collection.toFixed(2)) + '"/>' + addThousandsSeparator(val.Collection.toFixed(2)) + '</td>' +
                    '<td><input type="hidden" id="balance-' + row + '" value="' + addThousandsSeparator(val.Balance.toFixed(2)) + '"/>' + addThousandsSeparator(val.Balance.toFixed(2)) + '</td>' +
                    '<td><input type="text" class="form-control" id="amountpay-' + row + '" value="0.00" onchange="updatetotal(this);amountpaychange(this);" disabled/></td>' +
                    '<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input id = "status-' + row + '" type = "checkbox" onclick="enableamountpay(this)"  > <label for="checkbox8"> </label> </div ></td>' +
                    '</t>');
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
                    $('#MyModalPayment').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
});
$('#cardname').change(function () {
    var code = $(this).val();
    $('#cardcode').val(code).trigger("chosen:updated");
    var branch = $('#series option:selected').val().split("|")[1];
    var requestURL = '../../../Incomings/GetInvoice';
    var type = "GET";
    var forcomplete = {};
    $.ajax({
        type: type,
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { cardcode: code, branch: branch },
        dataType: "json",
        success: function (data2) {
            forcomplete = data2;
        },
        complete: function () {
            $('#tableList tbody tr').remove();
            $.map(forcomplete, function (val) {
                var $item = $('#tableList tbody');
                var row = $('#tableList').find('tbody > tr').length;
                $item.append('<tr>' +
                    '<td><input type="hidden" id="invno-' + row + '"value="' + val.InvoiceNo + '"/><input type="hidden" id="invdocnum-' + row + '" value="' + val.DocNum + '"/>' + val.InvoiceNo + '</td>' +
                    //'<td><input id="duedate-' + val.DueDate + '"/>' + val.DueDate + '</td>' +
                    '<td><input type="hidden" id="duedate-' + row + '" value="' + moment(val.DueDate).format('MM/DD/YYYY') + '"/>' + moment(val.DueDate).format('MM/DD/YYYY') + '</td>' +
                    '<td><input type="hidden" id="grosstotal-' + row + '" value="' + addThousandsSeparator(val.GrossTotal.toFixed(2)) + '"/>' + addThousandsSeparator(val.GrossTotal.toFixed(2)) + '</td>' +
                    '<td><input type="hidden" id="collection-' + row + '" value="' + addThousandsSeparator(val.Collection.toFixed(2)) + '"/>' + addThousandsSeparator(val.Collection.toFixed(2)) + '</td>' +
                    '<td><input type="hidden" id="balance-' + row + '" value="' + addThousandsSeparator(val.Balance.toFixed(2)) + '"/>' + addThousandsSeparator(val.Balance.toFixed(2)) + '</td>' +
                    '<td><input type="text" class="form-control" id="amountpay-' + row + '" value="0.00" onchange="updatetotal(this);amountpaychange(this);" disabled/></td>' +
                    '<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input id = "status-' + row + '" type = "checkbox" onclick="enableamountpay(this)"  > <label for="checkbox8"> </label> </div ></td>' +
                    '</t>');
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
                    $('#MyModalPayment').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
});
$('#series').change(function () {
    var series = $(this).val();
    var requestURL = '../../../SequenceTable/GetCurrentSequence';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { series: series, objectcode: 8 },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null)
                //var docnum = forcomplete.split('-')[1];
                $('#docnum').val(forcomplete[1]);
            var code = $('#cardcode').val();
            if (code != "") {
                requestURL = '../../../Incomings/GetInvoice';
                var type = "GET";
                var forcomplete2 = {};
                $.ajax({
                    type: type,
                    contentType: "application/json; charset=utf-8",
                    url: requestURL,
                    data: { cardcode: code, branch: series },
                    dataType: "json",
                    success: function (data2) {
                        forcomplete2 = data2;
                    },
                    complete: function () {
                        $('#tableList tbody tr').remove();
                        $.map(forcomplete2, function (val) {
                            var $item = $('#tableList tbody');
                            var row = $('#tableList').find('tbody > tr').length;
                            $item.append('<tr>' +
                                '<td><input type="hidden" id="invno-' + row + '"value="' + val.InvoiceNo + '"/><input type="hidden" id="invdocnum-' + row + '" value="' + val.DocNum + '"/>' + val.InvoiceNo + '</td>' +
                                //'<td><input id="duedate-' + val.DueDate + '"/>' + val.DueDate + '</td>' +
                                '<td><input type="hidden" id="duedate-' + row + '" value="' + moment(val.DueDate).format('MM/DD/YYYY') + '"/>' + moment(val.DueDate).format('MM/DD/YYYY') + '</td>' +
                                '<td><input type="hidden" id="grosstotal-' + row + '" value="' + addThousandsSeparator(val.GrossTotal.toFixed(2)) + '"/>' + addThousandsSeparator(val.GrossTotal.toFixed(2)) + '</td>' +
                                '<td><input type="hidden" id="collection-' + row + '" value="' + addThousandsSeparator(val.Collection.toFixed(2)) + '"/>' + addThousandsSeparator(val.Collection.toFixed(2)) + '</td>' +
                                '<td><input type="hidden" id="balance-' + row + '" value="' + addThousandsSeparator(val.Balance.toFixed(2)) + '"/>' + addThousandsSeparator(val.Balance.toFixed(2)) + '</td>' +
                                '<td><input type="text" class="form-control" id="amountpay-' + row + '" value="0.00" onchange="updatetotal(this);amountpaychange(this);" disabled/></td>' +
                                '<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input id = "status-' + row + '" type = "checkbox" onclick="enableamountpay(this)"  > <label for="checkbox8"> </label> </div ></td>' +
                                '</t>');
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
                                $('#MyModalPayment').modal({ backdrop: 'static', keyboard: false });
                            }
                        });
                    }
                });
            }
        }
    });
});
$('#amountpaid').change(function () {
    var amount = parseFloat($(this).val().replace(/,/g, ''));
    if (isNaN(amount)) { amount = 0; }
    $('#amountpaid').val(addThousandsSeparator(amount.toFixed(2)));
    GetCashBalance();
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
    var status = model[prop] === true ? "ACTIVE" : "INACTIVE";
    if (status === "ACTIVE") {
        return '<span class="label label-primary">' + status + '</span>';
    } else if (status === "INACTIVE") {
        return '<span class="label label-warning">' + status + '</span>';
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
    $('#cardcode').val('').trigger("chosen:updated");
    $('#cardname').val('').trigger("chosen:updated");
    $('#reference').val('');
    $('#termid').val('');
    $('#series').val('').trigger("chosen:updated");
    $('#docnum').val('');
    $('#status').val('');
    $('#date').val(moment(date.Date).format("MM/D/YYYY"));
    $('#duedate').val(moment(date.DueDate).format("MM/D/YYYY"));
    $('#total').val(addThousandsSeparator(defaultval.toFixed(2)));
    $('#discount').val(addThousandsSeparator(defaultval.toFixed(2)));
    $('#discountamount').val(addThousandsSeparator(defaultval.toFixed(2)));
    $('#vatamount').val(addThousandsSeparator(defaultval.toFixed(2)));
    $('#wtaxamount').val(addThousandsSeparator(defaultval.toFixed(2)));
    $('#grosstotal').val(addThousandsSeparator(defaultval.toFixed(2)));
    $('#totaltopay').val(addThousandsSeparator(defaultval.toFixed(2)));
    $('#amountpaid').val(addThousandsSeparator(defaultval.toFixed(2)));
    $('#balance').val(addThousandsSeparator(defaultval.toFixed(2)));
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
                    disablefield(false);
                    $('#pricelistid').val(forcomplete.PricelistId);
                    $('#pricelistname').val(forcomplete.Name);
                    $('#basepricelist').val(forcomplete.BasePricelist).trigger('chosen:updated');
                    $('#factor').val(forcomplete.Factor);
                    $('#ModalTitle').html('Edit Pricelist');
                    $('#wtax').text('Update Pricelist');
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
        lastStatus = $(this).attr('id').split('_')[2];
        if (lastSelectedIds != null) {
            if (lastStatus === "1") {
                swal({
                    title: "Are you sure?",
                    text: "You want to cancelled this payment?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, cancelled this payment!",
                    cancelButtonText: "No, Don't cancel!"
                }).then((result) => {
                    if (result.value) {
                        swal({
                            title: 'Loading... Please Wait!',
                            text: 'Cancelling payment...',
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            onOpen: () => {
                                swal.showLoading();
                            }
                        });
                        var requestURL = '../../../Incomings/Cancelled';
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
                                    title: forcomplete.HttpStatus === 200 ? "Successful" : "Error cancelling payment!",
                                    text: forcomplete.Message,
                                    allowOutsideClick: false,
                                    allowEscapeKey: false
                                }).then((result) => {
                                    if (forcomplete.HttpStatus === 200) {
                                        if (result.value) {
                                            var url = '../../../Incomings/Collection';
                                            window.location.href = url;
                                        }
                                    }
                                });
                            },
                            error: function (data3) {
                                swal.hideLoading();
                                swal({
                                    type: 'warning',
                                    title: "Error cancelling payment!",
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
                toastr.warning('Selected row already cancelled.', 'System Warning');
            }
        }
        else {

            $('.custom-alert').fadeIn();
            toastr.warning('Please select row to cancel.', 'System Warning');
        }
    });
    $('.view_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../Incomings/GetIncomings';
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
                    $('#cardcode').val(forcomplete.CardCode).trigger("chosen:updated");
                    $('#cardname').val(forcomplete.CardCode).trigger("chosen:updated");
                    $('#remarks').val(forcomplete.Reference);
                    $('#series').val(forcomplete.Series).trigger("chosen:updated");
                    $('#docnum').val(forcomplete.DocNum);
                    $('#date').val(moment(forcomplete.DatePaid).format("MM/D/YYYY"));
                    $('#duedate').val(moment(forcomplete.DueDate).format("MM/D/YYYY"));
                    $('#total').val(addThousandsSeparator(forcomplete.AmountPaid.toFixed(2)));
                    $("#modeofpayment").val(forcomplete.ModeOfPayment).trigger("chosen:updated");
                    //Display Item Details
                    $.each(forcomplete.Lines, function (i, item) {
                        var $item = $('#tableList tbody');
                        $item.append('<tr>' +
                            '<td>' + item.InvoiceNo + '</td>' +
                            '<td></td>' +
                            //'<td>' + moment(val.DueDate).format('MM/DD/YYYY') + '</td>' +
                            '<td>' + addThousandsSeparator(item.DocTotal.toFixed(2)) + '</td>' +
                            '<td>' + addThousandsSeparator(item.Collections.toFixed(2)) + '</td>' +
                            '<td>' + addThousandsSeparator(item.Balance.toFixed(2)) + '</td>' +
                            '<td>' + addThousandsSeparator(item.SumApplied.toFixed(2)) + '</td>' +
                            '<td></td>' +
                            //'<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input id = "status-' + i + '" type = "checkbox" onclick="enableamountpay(this)"  > <label for="checkbox8"> </label> </div ></td>' +
                            '</t>');
                    });
                    //>>end
                    $('#ModalTitle').html('Collection Details');
                    $('#incoming').text('Edit Collection').addClass('btn btn-success');
                    $('#MyModalPayment').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
}
function disablefield(ptype, pDisable) {
    $('#cardcode').prop('disabled', pDisable).trigger("chosen:updated");
    $('#cardname').prop('disabled', pDisable).trigger("chosen:updated");
    $('#remarks').prop('disabled', pDisable);
    $('#series').prop('disabled', pDisable).trigger("chosen:updated");
    $('#modeofpayment').prop('disabled', pDisable).trigger("chosen:updated");
    $('#incoming').prop('disabled', pDisable);
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
        data: { objectcode: 8 },
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
                        defaultval = val.DefaultSeries + "|" + val.BranchCode;
                    $("#series").append($('<option/>').val((val.Series + "|" + val.BranchCode)).text(val.SeriesName)).trigger('chosen:updated');
                });
                $('#series').val(defaultval).trigger("chosen:updated");
                LoadCurrentId(defaultval.split("|")[0]);
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
        data: { series: series, objectcode: 8 },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null)
                $('#docnum').val(forcomplete[1]);
            $('#pino').val(forcomplete[0]);
        }
    });
}
//Payment Details
function enableamountpay(obj) {
    var rowindex = $(obj).attr('id').split('-')[1];
    if ($("#status-" + rowindex).is(":checked")) {
        $("#amountpay-" + rowindex).prop('disabled', false);
        var bal = parseFloat($("#balance-" + rowindex).val().replace(/,/g, ''));
        $("#amountpay-" + rowindex).val(addThousandsSeparator(bal.toFixed(2)));
    }
    else {
        $("#amountpay-" + rowindex).prop('disabled', true);
        $("#amountpay-" + rowindex).val(0.00);
    }
    updatetotal();
}
function amountpaychange(obj) {
    var rowindex = $(obj).attr('id').split('-')[1];
    var amount = parseFloat($('#amountpay-' + rowindex).val().replace(/,/g, ''));
    if (isNaN(amount)) { amount = 0; }
    $('#amountpay-' + rowindex).val(addThousandsSeparator(amount.toFixed(2)));
}
function updatecurrentAmount(obj) {

    var rowindex = $(obj).attr('id').split('-')[1];
    var currentamount = parseFloat($("#amountpay-" + rowindex).val().replace(/,/g, ''));
    var bal = parseFloat($("#balance-" + rowindex).val().replace(/,/g, ''));
    if (currentamount > bal) {
        $('.custom-alert').fadeIn();
        toastr.warning('Amount to pay should not be greater than the balance!.', '');
        $("#amountpay-" + rowindex).val("0.00");
        return;
    }
    if (isNaN(currentamount)) { currentamount = 0; }
    $("#amountpay-" + rowindex).val(addThousandsSeparator(currentamount.toFixed(2)));
    updatetotal();
}
function updatetotal() {
    var total = 0.00;
    $('#tableList tbody tr').each(function (i, item) {
        if ($("#status-" + i, this).is(":checked")) {
            var pay = parseFloat($("#amountpay-" + i, this).val());
            if (isNaN(pay)) { pay = 0; }
            total += parseFloat($("#amountpay-" + i, this).val().replace(/,/g, ''));
        }
    });
    if (isNaN(total)) { total = 0; }
    $('#total').val(addThousandsSeparator(total.toFixed(2)));
}
function CheckStatus() {
    var total = parseFloat($('#total').val().replace(/,/g, '')) - parseFloat($('#amountpaid').val().replace(/,/g, ''));
    var status = 0;
    if (total === 0) {
        status = 0;
    } else if (total === parseFloat($('#total').val().replace(/,/g, ''))) {
        status = 2;
    } else if (total > 0) {
        status = 1;
    }
    return status;
}
function GetCashBalance() {
    var balance = parseFloat($('#totaltopay').val().replace(/,/g, '')) - parseFloat($('#amountpaid').val().replace(/,/g, ''));
    if (isNaN(balance)) { balance = 0; }
    $('#balance').val(addThousandsSeparator(balance.toFixed(2)));
}