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
    general_row_html = $(".general-tab-table").find("tbody").html();
    ItemRowEvent();
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
$('#add').click(function () {
    //disablefield('add', false);
    //$('#status').val("2").trigger("chosen:updated");
    $('#ModalTitle').html('Create Payment');
    //$('#pay').text('Add Payment').removeClass('btn btn-success').addClass('btn btn-primary');
    clearAll();
    $('#MyModalPayment').modal({ backdrop: 'static', keyboard: false });
});
$('#pay').click(function () {
    var ItemList = [];
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
    //Item Details
    var i = 0;
    $('#tableListItem tbody tr').each(function (index, ele) {
        if ($("#itemcode", this).val() != "" && $("#itemname", this).val() != "") {
            var item = {
                LineNum: i++,
                ItemCode: $('#itemcode', this).val().trim(),
                ItemName: $("#itemname", this).val().trim(),
                Quantity: $("#qty", this).val().trim(),
                UoM: $('#uom', this).val().trim(),
                UnitPrice: Number($("#unitprice", this).val().replace(/,/g, '')),
                Discount: Number($("#linediscount", this).val().replace(/,/g, '')),
                PriceAfterDiscount: Number($("#priceafdiscount", this).val().replace(/,/g, '')),
                LineTotal: Number($("#linetotal", this).val().replace(/,/g, '')),
                WTax: Number($("#linewtax", this).val().replace(/,/g, '')),
                Vat: Number($("#linevat", this).val().replace(/,/g, '')),
                GrossPrice: Number($("#linegross", this).val().replace(/,/g, '')),
                GrossTotal: Number($("#linegrosstotal", this).val().replace(/,/g, ''))
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
    if (isAllValid) {
        $('#amountpaid').val($('#grosstotal').val());
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
    var title = $('#purchaseinvoice').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Purchase Invoice');
        $('#purchaseinvoice').text('Update Purchase Invoice').removeClass('btn btn-success').addClass('btn btn-primary');
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
        if ($('#termid').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Payment Term is required.', '');
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
            if ($("#itemcode", this).val() != "" && $("#itemname", this).val() != "") {
                var item = {
                    LineNum: i++,
                    ItemCode: $('#itemcode', this).val().trim(),
                    ItemName: $("#itemname", this).val().trim(),
                    Quantity: $("#qty", this).val().trim(),
                    UoM: $('#uom', this).val().trim(),
                    UnitPrice: Number($("#unitprice", this).val().replace(/,/g, '')),
                    Discount: Number($("#linediscount", this).val().replace(/,/g, '')),
                    PriceAfterDiscount: Number($("#priceafdiscount", this).val().replace(/,/g, '')),
                    LineTotal: Number($("#linetotal", this).val().replace(/,/g, '')),
                    WTax: Number($("#linewtax", this).val().replace(/,/g, '')),
                    Vat: Number($("#linevat", this).val().replace(/,/g, '')),
                    GrossPrice: Number($("#linegross", this).val().replace(/,/g, '')),
                    GrossTotal: Number($("#linegrosstotal", this).val().replace(/,/g, ''))
                }
                ItemList.push(item);
            }
        })
        if (ItemList.length === 0) {
            $('.custom-alert').fadeIn();
            toastr.warning('Item Details is required.', '');
            isAllValid = false;
        }
        //Payment Details
        var y = 0;
        if ($("#amountpaid").val() != "" && $("#modeofpayment option:selected").val() != "") {
            //lines payment
            var payline = {
                LineNum: y++,
                InvType: "PI",
                SumApplied: parseFloat($('#amountpaid').val().replace(/,/g, '')),
                DocTotal: parseFloat($('#amountpaid').val().replace(/,/g, '')),
                InvoiceNo: $('#pino').val().trim()
            };
            paymentlines.push(payline);

            //header payment
            var payment = {
                BranchCode: $('#series').val().trim(),
                Series: $('#series').val().trim(),
                CardCode: $("#cardcode").val().trim(),
                CardName: $("#cardname").val().trim(),
                InvoiceNo: $("#pino").val().trim(),
                InvoiceType: "PI",
                DueDate: $('#duedate').val().trim(),
                GrossTotal: parseFloat($('#grosstotal').val().replace(/,/g, '')),
                Collections: parseFloat($('#grosstotal').val().replace(/,/g, '')),
                Balance: parseFloat($('#grosstotal').val().replace(/,/g, '')) - parseFloat($('#amountpaid').val().replace(/,/g, '')),
                DocTotal: parseFloat($('#total').val().replace(/,/g, '')),
                AmountPaid: parseFloat($('#amountpaid').val().replace(/,/g, '')),
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
        }
        //>>end
        if (isAllValid) {
            $("#MyModalPurchaseInvoice").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding purchase invoice...' : 'Update purchase invoice...';
            var messagetext = title === 'add' ? "Error adding purchase invoice!" : "Error updating purchase invoice!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " purchaseinvoice?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " purchaseinvoice!",
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
                        PInvoice: $('#pino').val(),
                        CardCode: $('#cardcode option:selected').val().trim(),
                        CardName: $('#cardname option:selected').text().trim(),
                        Reference: $('#reference').val().trim(),
                        TermId: $('#termid').val().trim(),
                        DocTotal: parseFloat($('#total').val().replace(/,/g, '')),
                        Discount: parseFloat($('#discount').val().replace(/,/g, '')),
                        DiscountAmount: parseFloat($('#discountamount').val().replace(/,/g, '')),
                        WTaxAmount: parseFloat($('#wtaxamount').val().replace(/,/g, '')),
                        VatAmount: parseFloat($('#vatamount').val().replace(/,/g, '')),
                        GrossTotal: parseFloat($('#grosstotal').val().replace(/,/g, '')),
                        Date: $('#date').val().trim(),
                        DueDate: $('#duedate').val().trim(),
                        Status: $('#status option:selected').val(),
                        Lines: ItemList,
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: ''
                    }
                    var requestURL = title === 'add' ? '../../../purchaseinvoice/Save' : '../../../purchaseinvoice/Update';
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
                                    requestURL = title === 'add' ? '../../../Incomings/Save' : '../../../Incomings/Update';
                                    var type = title === 'add' ? "POST" : "PUT";
                                    var forcomplete2 = {};
                                    $.ajax({
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        url: requestURL,
                                        data: JSON.stringify(payment),
                                        dataType: "json",
                                        success: function (data4) {
                                            forcomplete2 = data4;
                                        },
                                        complete: function () {
                                            swal.hideLoading();
                                            swal({
                                                type: forcomplete2.HttpStatus === 200 ? 'success' : 'warning',
                                                title: forcomplete2.HttpStatus === 200 ? "Successful" : messagetext,
                                                text: forcomplete2.Message,
                                                allowOutsideClick: false,
                                                allowEscapeKey: false
                                            }).then((result) => {
                                                if (forcomplete.HttpStatus === 200) {
                                                    if (result.value) {
                                                        var url = '../../../purchaseinvoice/Index';
                                                        window.location.href = url;
                                                    }
                                                }
                                                else {
                                                    $('#MyModalPurchaseInvoice').modal({ backdrop: 'static', keyboard: false });
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
                                                    $('#MyModalPurchaseInvoice').modal({ backdrop: 'static', keyboard: false });
                                                }
                                            });
                                        }
                                    });
                                    //var url = '../../../purchaseinvoice/Index';
                                    //window.location.href = url;
                                }
                            }
                            else {
                                $('#MyModalPurchaseInvoice').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalPurchaseInvoice').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalPurchaseInvoice').modal({ backdrop: 'static', keyboard: false });
                }
            });
        };
    }
});
$('#clear').click(function () {
    clearAll();
});
$('#cardcode').change(function () {
    var code = $(this).val();
    $('#cardname').val(code).trigger("chosen:updated");
    var requestURL = '../../../incomings/GetInvoice';
    var type = "GET";
    var forcomplete = {};
    $.ajax({
        type: type,
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { value: code, branch: "PRD01" },
        dataType: "json",
        success: function (data2) {
            forcomplete = data2;
        },
        complete: function () {
            if (forcomplete.HttpStatus === 200) {
                if (result.value) {
                    //var url = '../../../purchaseinvoice/Index';
                    //window.location.href = url;
                }
            }
            else {
                $('#MyModalPayment').modal({ backdrop: 'static', keyboard: false });
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
                    $('#MyModalPayment').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
});
$('#cardname').change(function () {
    var code = $(this).val();
    $('#cardcode').val(code).trigger("chosen:updated");
});
$('#series').change(function () {
    var series = $(this).val();
    var requestURL = '../../../SequenceTable/GetCurrentSequence';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { series: series, objectcode: 9 },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null)
                //var docnum = forcomplete.split('-')[1];
            $('#docnum').val(forcomplete[1]);
            $('#pino').val(forcomplete[0]);
        }
    });
});
$('#amountpaid').change(function () {
    var amount = parseFloat($(this).val().replace(/,/g, ''));
    $('#amountpaid').val(addThousandsSeparator(amount.toFixed(2)));
    $('#status').val(CheckStatus()).trigger("chosen:updated");
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
    var status = model[prop];
    //var status = checked === true ? "ACTIVE" : "IN_ACTIVE";
    if (status === "FULLYPAID") {
        return '<span class="label label-primary">' + status + '</span>';
    } else if (status === "PARTIALLY_PAID") {
        return '<span class="label label-warning">' + status + '</span>';
    } else if (status === "UNPAID") {
        return '<span class="label label-success">' + status + '</span>';
    }
    else if (status === "CANCELED") {
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
            var requestURL = '../../../purchaseinvoice/Getpurchaseinvoice';
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
                    $('#reference').val(forcomplete.Reference);
                    $('#termid').val(forcomplete.TermId);
                    $('#series').val(forcomplete.Series).trigger("chosen:updated");
                    $('#docnum').val(forcomplete.DocNum);
                    $('#status').val(forcomplete.Status).trigger("chosen:updated");
                    $('#date').val(moment(forcomplete.Date).format("MM/D/YYYY"));
                    $('#duedate').val(moment(forcomplete.DueDate).format("MM/D/YYYY"));

                    //Display Item Details
                    //$.each(data, function (i, item) {
                    //    var $table = $('#tableListItem tbody');
                    //    $table.append('<tr>')
                    //});
                    //>>end
                    $('#ModalTitle').html('Purchase Invoice Details');
                    $('#purchaseinvoice').text('Edit Purchase Invoice').addClass('btn btn-success');
                    $('#MyModalPurchaseInvoice').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
}
function disablefield(ptype, pDisable) {
    //$('#pricelistname').prop('disabled', pDisable);
    //$('#basepricelist').prop('disabled', pDisable);
    //$('#factor').prop('disabled', pDisable);
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
    $('.qty').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
        updatetotal();
    });
    $('.unitprice').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
        updatetotal();
    });
    $('.linediscount').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
        updatetotal();
    });
    $('.priceafdiscount').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
        updatetotal();
    });
    $('.linetotal').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
        updatetotal();
    });
    $('.linewtax').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
        updatetotal();
    });
    $('.linevat').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
        updatetotal();
    });
    $('.linegross').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
        updatetotal();
    });
    $('.linegrosstotal').change(function () {
        var row = $(this).closest('tr');
        calculate_row(row);
        updatetotal();
    });
    $('#discount').change(function () {
        if ($(this).val() <= 100) {
            updatetotal("Discount");
        }
        else {
            $('.custom-alert').fadeIn();
            toastr.warning('Discount Percentage should not be greater than 100!.', '');
            $('#discount').val($('#hidedisc').val());
        }
    });
    $('#discountamount').change(function () {
        if (parseFloat($(this).val().replace(/,/g, '')) <= parseFloat($('#total').val().replace(/,/g, ''))) {
            updatetotal("DiscountAmount");
        }
        else {
            $('.custom-alert').fadeIn();
            toastr.warning('Discount Amount should not be greater than Total!.', '');
            $('#discountamount').val($('#hidediscamount').val());
        }
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
    var $this = $(row);
    var qty = parseFloat($('#qty', $this).val().replace(',', ''));
    var unitprice = parseFloat($('#unitprice', $this).val().replace(',', ''));
    var linediscount = parseFloat($('#linediscount', $this).val().replace(',', ''));
    var priceafdiscount = parseFloat($('#priceafdiscount', $this).val().replace(',', ''));
    var linetotal = parseFloat($('#linetotal', $this).val().replace(',', ''));
    var linewtax = parseFloat($('#linewtax', $this).val().replace(',', ''));
    var linevat = parseFloat($('#linevat', $this).val().replace(',', ''));
    var linegross = parseFloat($('#linegross', $this).val().replace(',', ''));
    var linegrosstotal = parseFloat($('#linegrosstotal', $this).val().replace(',', ''));

    if (isNaN(qty)) { qty = 0; }
    if (isNaN(unitprice)) { unitprice = 0; }
    if (isNaN(linediscount)) { linediscount = 0; }
    if (isNaN(priceafdiscount)) { priceafdiscount = 0; }
    if (isNaN(linetotal)) { linetotal = 0; }
    if (isNaN(linewtax)) { linewtax = 0; }
    if (isNaN(linevat)) { linevat = 0; }
    if (isNaN(linegross)) { linegross = 0; }
    if (isNaN(linegrosstotal)) { linegrosstotal = 0; }


    //computation
    //price after discount
    var newpriceafdis = unitprice * ((100 - linediscount) / 100);
    var newlinetotal = qty * newpriceafdis;
    var newgrossprice = newpriceafdis * ((100 + linevat) / 100);
    var newgrostotal = qty * newgrossprice;

    $('#qty', $this).val(addThousandsSeparator(qty.toFixed(2)));
    $('#unitprice', $this).val(addThousandsSeparator(unitprice.toFixed(2)));
    $('#linediscount', $this).val(addThousandsSeparator(linediscount.toFixed(2)));
    $('#priceafdiscount', $this).val(addThousandsSeparator(newpriceafdis.toFixed(2)));
    $('#linetotal', $this).val(addThousandsSeparator(newlinetotal.toFixed(2)));
    $('#linewtax', $this).val(addThousandsSeparator(linewtax.toFixed(2)));
    $('#linegross', $this).val(addThousandsSeparator(newgrossprice.toFixed(2)));
    $('#linegrosstotal', $this).val(addThousandsSeparator(newgrostotal.toFixed(2)));
}
function updatetotal(discounttype) {
    var total = 0;
    var discount = 0;
    var discountamount = 0;
    var wtax = 0;
    var vat = 0;
    var gross = 0;
    var newdiscountamount = 0;
    var newdiscount = 0;
    $('#tableListItem tbody tr').each(function (i, item) {
        var linevat = parseFloat($('#linevat', this).val());
        if (isNaN(linevat)) { linevat = 0; }
        total += parseFloat($('#linetotal', this).val().replace(/,/g, ''));
        wtax += parseFloat($('#linetotal', this).val().replace(/,/g, '')) * (parseFloat($('#linewtax', this).val().replace(/,/g, '')) / 100);
        vat += parseFloat($('#linetotal', this).val().replace(/,/g, '')) * (parseFloat(linevat) / 100);
    });
    discount = parseFloat($('#discount').val().replace(/,/g, ''));
    discountamount = parseFloat($('#discountamount').val().replace(/,/g, ''));
    if (isNaN(total)) { total = 0; }
    if (isNaN(wtax)) { wtax = 0; }
    if (isNaN(vat)) { vat = 0; }
    if (isNaN(discount)) { discount = 0; }
    if (isNaN(discountamount)) { discountamount = 0; }
    //getdiscountamount
    switch (discounttype) {
        case "Discount":
            newdiscountamount = (total * discount) / 100;
            $('#discountamount').val(addThousandsSeparator(newdiscountamount.toFixed(2)));
            $('#hidediscamount').val(addThousandsSeparator(newdiscountamount.toFixed(2)));
            $('#discount').val(addThousandsSeparator(discount.toFixed(2)));
            $('#hidedisc').val(addThousandsSeparator(discount.toFixed(2)));
            break;
        case "DiscountAmount":
            newdiscount = (discountamount / total) * 100;
            newdiscountamount = addThousandsSeparator(discountamount.toFixed(2));
            $('#discountamount').val(addThousandsSeparator(discountamount.toFixed(2)));
            $('#discount').val(addThousandsSeparator(newdiscount.toFixed(2)));
            break;
    }
    $('#total').val(addThousandsSeparator(total.toFixed(2)));
    $('#wtaxamount').val(addThousandsSeparator(wtax.toFixed(2)));
    $('#vatamount').val(addThousandsSeparator(vat.toFixed(2)));
    gross = addThousandsSeparator((total - newdiscountamount - wtax + vat).toFixed(2));
    $('#grosstotal').val(gross);
    $('#totaltopay').val(gross);
}
function CheckStatus() {
    var total = parseFloat($('#grosstotal').val().replace(/,/g, '')) - parseFloat($('#amountpaid').val().replace(/,/g, ''));
    var status = 0;
    if (total === 0) {
        status = 0;
    } else if (total > 0) {
        status = 1;
    } else if (total === parseFloat($('#grosstotal').val().replace(/,/g, ''))) {
        status = 2;
    }
    return status;
}
function GetCashBalance() {
    var balance = parseFloat($('#totaltopay').val().replace(/,/g, '')) - parseFloat($('#amountpaid').val().replace(/,/g, ''));
    $('#balance').val(addThousandsSeparator(balance.toFixed(2)));
}