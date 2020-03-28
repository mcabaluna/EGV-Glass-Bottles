var general_row_html = "";
var ItemRow = 0;
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
    //ItemRowEvent();
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
    var object = 5;
    var documentname = "Sales Invoice";
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
                $('#tableListItem tbody tr').remove();
                var date = new Date();
                $('#duedate').val(moment(date).format("MM/DD/YYYY"));
                $('#deliverydate').val(moment(date).format("MM/DD/YYYY"));
                $('#date').val(moment(date).format("MM/DD/YYYY"));
                disablefield('add', false);
                ItemRow = 0;
                $('#status').val("2").trigger("chosen:updated");
                $('#ModalTitle').html('Create Sales Invoice');
                $('#salesinvoice').text('Add Sales Invoice').removeClass('btn btn-success').addClass('btn btn-primary');
                clearAll();
                LoadSeries();
                $('#MyModalSalesInvoice').modal({ backdrop: 'static', keyboard: false });
            }
        }
    });
});
$('#collection').click(function () {
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
    if ($('#termid option:selected').val().trim() === '') {

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
                ItemCode: $('#itemcode-' + index).val().trim(),
                ItemName: $('#itemname-' + index).val().trim(),
                Quantity: $('#qty-' + index).val().trim(),
                UoM: $('#uom-' + index).val().trim(),
                Discount: Number($('#linediscount-' + index).val().replace(/,/g, '')),
                WTax: Number($('#linewtax-' + index).val()),
                Vat: Number($('#linevat-' + index).val().replace(/,/g, '')),
                GrossPrice: Number($('#linegross-' + index).val().replace(/,/g, '')),
                GrossTotal: Number($('#linegrosstotal-' + index).val().replace(/,/g, ''))
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
        //$('#amountpaid').val($('#grosstotal').val());
        var amount = parseFloat($('#amountpaid').val().replace(/,/g, ''));

        if (isNaN(amount)) { amount = 0; }
        if (amount === 0) {
            $('#modeofpayment').val('').trigger("chosen:updated");
            $('#modeofpayment').prop('disabled', true).trigger("chosen:updated");
        }
        else {
            $('#modeofpayment').prop('disabled', false).trigger("chosen:updated");
        }
        $('#status').val(CheckStatus()).trigger("chosen:updated");
        $('#ModalTitle').html('Collections');
        $('#collection').text('Collection').removeClass('btn btn-success').addClass('btn btn-primary');
        GetCashBalance();
        //clearAll();
        $('#MyModalCollection').modal({ backdrop: 'static', keyboard: false });
    }
});
$('#amountpaid').change(function () {
    var amount = parseFloat($('#amountpaid').val().replace(/,/g, ''));
    if (isNaN(amount)) { amount = 0; }
    if (amount === 0) {
        $('#modeofpayment').val('').trigger("chosen:updated");
        $('#modeofpayment').prop('disabled', true).trigger("chosen:updated");
    }
    else {
        $('#modeofpayment').prop('disabled', false).trigger("chosen:updated");
    }
});
$('#closecollection').click(function () {
    var isAllValid = true;
    if (parseFloat($('#balance').val().replace(/,/g, '')) === 0 && $('#modeofpayment option:selected').val() === "") {
        $('.custom-alert').fadeIn();
        toastr.warning('Mode of Payment is required.', '');
        isAllValid = false;
    }
    if (isAllValid) {
        $("#MyModalCollection").modal('hide');
    }
});
$('#salesinvoice').click(function () {
    var ItemList = [];
    var paymentlines = [];
    var date = new Date();
    var title = $('#salesinvoice').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        var requestURL = '../../../Home/CheckStatus';
        var forcomplete2 = {};
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: requestURL,
            data: { transtype: "SalesInvoices", docentry: $('#docentry').val() },
            dataType: "json",
            success: function (data) {
                forcomplete2 = data;
            },
            complete: function () {
                if (forcomplete2 != null) {
                    if (forcomplete2.Status && forcomplete2.Remarks.includes('FULLYPAID')) {
                        $('.custom-alert').fadeIn();
                        toastr.warning('Selected row already fully paid.');
                    }
                    else if (forcomplete2.Remarks.includes("CANCELED")) {
                        $('.custom-alert').fadeIn();
                        toastr.warning('Selected row already canceled.');
                    }
                    else {
                        disablefield('update', false);
                        $('#ModalTitle').html('Edit Sales Invoice');
                        $('#salesinvoice').text('Update Sales Invoice').removeClass('btn btn-success').addClass('btn btn-primary');
                    }
                }
                else {
                    $('.custom-alert').fadeIn();
                    toastr.warning('Error encountered while checking status. Please contact administrator.', 'System Warning');
                }
            }
        });
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
        if ($('#termid option:selected').val().trim() === '') {

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
            if ($('#itemcode-' + index).val() != "" && $('#itemname-' + index).val() != ""
                && $('#whse-' + index + ' option:selected').val() != "") {
                var item = {
                    DocEntry: $('#docentry').val() || 0,
                    LineNum: i++,
                    ItemCode: $('#itemcode-' + index + ' option:selected').val(),
                    ItemName: $('#itemname-' + index + ' option:selected').text(),
                    Quantity: $('#qty-' + index).val().trim(),
                    UoM: $('#uom-' + index).val().trim(),
                    //UnitPrice: Number($("#unitprice", this).val().replace(/,/g, '')),
                    Discount: Number($('#linediscount-' + index).val().replace(/,/g, '')),
                    PriceAfterDiscount: 0.00,//Number($("#priceafdiscount", this).val().replace(/,/g, '')),
                    LineTotal: 0.00,//Number($("#linetotal", this).val().replace(/,/g, '')),
                    WTax: Number($('#linewtax-' + index).val()),
                    Whse: $('#whse-' + index + ' option:selected').val(),
                    Vat: Number($('#linevat-' + index + ' option:selected').val().replace(/,/g, '')),
                    GrossPrice: Number($('#linegross-' + index).val().replace(/,/g, '')),
                    GrossTotal: Number($('#linegrosstotal-' + index).val().replace(/,/g, ''))
                }
                ItemList.push(item);
            } else {
                if ($('#itemcode-' + index).val() === "" && $('#itemname-' + index).val() === "") {
                    $('.custom-alert').fadeIn();
                    toastr.warning('Item selection is required in row ' + (index + 1) + '.', '');
                    isAllValid = false;
                }
                if ($('#whse-' + index + ' option:selected').val() == "") {
                    $('.custom-alert').fadeIn();
                    toastr.warning('Warehouse selection is required in row ' + (index + 1) + '.', '');
                    isAllValid = false;
                }
            }
        })
        if (ItemList.length === 0) {
            $('.custom-alert').fadeIn();
            toastr.warning('Item Details is required.', '');
            isAllValid = false;
        }
        //Payment Details
        var y = 0;
        var payment = {};
        var checkPayment = false;
        if (Number($("#amountpaid").val()) != 0 && $("#modeofpayment option:selected").val() != "") {
            //lines payment
            var amntpaid = parseFloat($('#amountpaid').val().replace(/,/g, ''));
            if (isNaN(amntpaid)) { amntpaid = 0; }
            var payline = {
                LineNum: y++,
                DocNum: $('#docnum').val(),
                InvType: "SI",
                DocTotal: parseFloat($('#grosstotal').val().replace(/,/g, '')),
                Collections: parseFloat($('#amountpaid').val().replace(/,/g, '')),
                Balance: parseFloat($('#grosstotal').val().replace(/,/g, '')) - amntpaid,
                SumApplied: parseFloat($('#amountpaid').val().replace(/,/g, '')),
                InvoiceNo: $('#sino').val().trim()
            };
            paymentlines.push(payline);

            //header payment
            payment = {
                BranchCode: $('#branch option:selected').val(),
                Series: "",
                ObjectType: "8",
                CardCode: $('#cardcode option:selected').val().trim(),
                CardName: $('#cardname option:selected').text().trim(),
                //PaymentNo: ""),
                //InvoiceType: "SI",
                DueDate: $('#duedate').val().trim(),
                //GrossTotal: parseFloat($('#grosstotal').val().replace(/,/g, '')),
                //Collections: parseFloat($('#grosstotal').val().replace(/,/g, '')),
                //Balance: parseFloat($('#grosstotal').val().replace(/,/g, '')) - parseFloat($('#amountpaid').val().replace(/,/g, '')),
                //DocTotal: parseFloat($('#total').val().replace(/,/g, '')),
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
            checkPayment = true;
        }
        //>>end
        if (isAllValid) {
            $("#MyModalSalesInvoice").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding salesinvoice...' : 'Update salesinvoice...';
            var messagetext = title === 'add' ? "Error adding salesinvoice!" : "Error updating salesinvoice!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " salesinvoice?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " salesinvoice!",
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
                        DocEntry: $('#docentry').val() || 0,
                        DocNum: $('#docnum').val(),
                        ObjectType: "5",
                        Series: $('#series').val(),
                        BranchCode: $('#branch option:selected').val(),
                        SInvoice: $('#sino').val(),
                        CardCode: $('#cardcode option:selected').val().trim(),
                        CardName: $('#cardname option:selected').text().trim(),
                        Reference: $('#reference').val().trim(),
                        PricelistId: $('#pricelistid').val().trim(),
                        TermId: $('#termid option:selected').val().trim(),
                        DocTotal: parseFloat($('#total').val().replace(/,/g, '')),
                        Discount: parseFloat($('#discount').val().replace(/,/g, '')),
                        DiscountAmount: parseFloat($('#discountamount').val().replace(/,/g, '')),
                        WTaxAmount: parseFloat($('#wtaxamount').val().replace(/,/g, '')),
                        VatAmount: parseFloat($('#vatamount').val().replace(/,/g, '')),
                        GrossTotal: parseFloat($('#grosstotal').val().replace(/,/g, '')),
                        Date: $('#date').val().trim(),
                        Deliverydate: $('#deliverydate').val().trim(),
                        DueDate: $('#duedate').val().trim(),
                        Status: $('#status option:selected').val(),
                        Lines: ItemList,
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: '',
                        Remarks: $('#remarks').val().trim()
                    }
                    var requestURL = title === 'add' ? '../../../SalesInvoice/Save' : '../../../SalesInvoice/Update';
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
                                    if (checkPayment) {
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
                                                    text: forcomplete.Message,
                                                    allowOutsideClick: false,
                                                    allowEscapeKey: false
                                                }).then((result) => {
                                                    if (forcomplete.HttpStatus === 200) {
                                                        if (result.value) {
                                                            var url = '../../../SalesInvoice/Index';
                                                            window.location.href = url;
                                                        }
                                                    }
                                                    else {
                                                        $('#MyModalSalesInvoice').modal({ backdrop: 'static', keyboard: false });
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
                                                        $('#MyModalSalesInvoice').modal({ backdrop: 'static', keyboard: false });
                                                    }
                                                });
                                            }
                                        });
                                    } else {
                                        swal.hideLoading();
                                        swal({
                                            type: 'success',
                                            title: "Successful",
                                            text: forcomplete.Message,
                                            allowOutsideClick: false,
                                            allowEscapeKey: false
                                        }).then((result) => {
                                            if (result.value) {
                                                var url = '../../../SalesInvoice/Index';
                                                window.location.href = url;
                                            }
                                        });
                                    }
                                }
                            }
                            else {
                                $('#MyModalSalesInvoice').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalSalesInvoice').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalSalesInvoice').modal({ backdrop: 'static', keyboard: false });
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
    var requestURL = '../../../BusinessPartner/GetBusinessPartner';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { id: code },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null) {
                if (forcomplete.CardCode == code) {
                    $('#bpwtaxliable').val(forcomplete.WithWTax);
                    $('#termid').val(forcomplete.TermId).trigger("chosen:updated");
                    requestURL = '../../../PaymentTerms/GetPaymentTerms';
                    var forcomplete2 = {};
                    $.ajax({
                        type: "GET",
                        contentType: "application/json; charset=utf-8",
                        url: requestURL,
                        data: { id: $('#termid option:selected').val() },
                        dataType: "json",
                        success: function (data) {
                            forcomplete2 = data;
                        },
                        complete: function () {
                            if (forcomplete2 != null) {
                                var newDate = getdate(forcomplete2.NoOfDays);
                                $('#duedate').val(moment(newDate).format("MM/DD/YYYY"));
                                requestURL = '../../../Pricelist/GetPricelist';
                                var forcomplete3 = {};
                                $.ajax({
                                    type: "GET",
                                    contentType: "application/json; charset=utf-8",
                                    url: requestURL,
                                    data: { id: forcomplete.PricelistId },
                                    dataType: "json",
                                    success: function (data) {
                                        forcomplete3 = data;
                                    },
                                    complete: function () {
                                        if (forcomplete3 != null) {
                                            $('#pricelistid').val(forcomplete3.PricelistId);
                                            $('#pricelistname').val(forcomplete3.Name);
                                        }
                                    }
                                });
                            }
                        }
                    });
                }
            }
        }
    });
});
$('#cardname').change(function () {
    var code = $(this).val();
    $('#cardcode').val(code).trigger("chosen:updated");
    var requestURL = '../../../BusinessPartner/GetBusinessPartner';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { id: code },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null) {
                if (forcomplete.CardCode == code) {
                    $('#bpwtaxliable').val(forcomplete.WithWTax);
                    $('#termid').val(forcomplete.TermId).trigger("chosen:updated");
                    requestURL = '../../../PaymentTerms/GetPaymentTerms';
                    var forcomplete2 = {};
                    $.ajax({
                        type: "GET",
                        contentType: "application/json; charset=utf-8",
                        url: requestURL,
                        data: { id: $('#termid option:selected').val() },
                        dataType: "json",
                        success: function (data) {
                            forcomplete2 = data;
                        },
                        complete: function () {
                            if (forcomplete2 != null) {
                                var newDate = getdate(forcomplete2.NoOfDays);
                                $('#duedate').val(moment(newDate).format("MM/DD/YYYY"));
                                requestURL = '../../../Pricelist/GetPricelist';
                                var forcomplete3 = {};
                                $.ajax({
                                    type: "GET",
                                    contentType: "application/json; charset=utf-8",
                                    url: requestURL,
                                    data: { id: forcomplete.PricelistId },
                                    dataType: "json",
                                    success: function (data) {
                                        forcomplete3 = data;
                                    },
                                    complete: function () {
                                        if (forcomplete3 != null) {
                                            $('#pricelistid').val(forcomplete3.PricelistId);
                                            $('#pricelistname').val(forcomplete3.Name);
                                        }
                                    }
                                });
                            }
                        }
                    });
                }
            }
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
        data: { series: series, objectcode: 5 },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null) {
                $('#docnum').val(forcomplete[1]);
                $('#sino').val(forcomplete[0]);
                $('#branch').val(forcomplete[2]).trigger("chosen:updated");
            }
        }
    });
});
$('#amountpaid').change(function () {
    var amount = parseFloat($(this).val().replace(/,/g, ''));
    if (isNaN(amount)) { amount = 0; }
    $('#amountpaid').val(addThousandsSeparator(amount.toFixed(2)));
    $('#status').val(CheckStatus()).trigger("chosen:updated");
    GetCashBalance();
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
    var amount = parseFloat($(this).val().replace(/,/g, ''));
    if (isNaN(amount)) { amount = 0; }

    if (amount <= parseFloat($('#total').val().replace(/,/g, ''))) {
        updatetotal("DiscountAmount");
    }
    else {
        $('.custom-alert').fadeIn();
        toastr.warning('Discount Amount should not be greater than Total!.', '');
        $('#discountamount').val($('#hidediscamount').val());
    }
});
$('#close').click(function () {
    $("#MyModalSalesInvoice").modal('hide');
});
function LoadSeries() {
    var requestURL = '../../../SequenceTable/GetSeriesBasedOnObject';
    var forcomplete = {};
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
        data: { series: series, objectcode: 5 },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null) {
                $('#docnum').val(forcomplete[1]);
                $('#sino').val(forcomplete[0]);
            }
        }
    });
}
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
    $('#termid').val('').trigger("chosen:updated");
    $('#series').val('').trigger("chosen:updated");
    $('#docnum').val('');
    $('#status').val('2').trigger("chosen:updated");
    $('#date').val(moment(date.Date).format("MM/D/YYYY"));
    $('#duedate').val(moment(date.DueDate).format("MM/D/YYYY"));
    $('#deliverydate').val(moment(date.DueDate).format("MM/D/YYYY"));
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
        var lastid = lastSelectedIds;
        if (lastSelectedIds != null) {
            var requestURL = '../../../Home/CheckStatus';
            var forcomplete2 = {};
            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                url: requestURL,
                data: { transtype: "SalesInvoices", docentry: lastSelectedIds },
                dataType: "json",
                success: function (data) {
                    forcomplete2 = data;
                },
                complete: function () {
                    if (forcomplete2 != null) {
                        if (forcomplete2.Status && forcomplete2.Remarks.includes('FULLYPAID')) {
                            $('.custom-alert').fadeIn();
                            toastr.warning('Selected row already fully paid.');
                        }
                        else if (forcomplete2.Remarks.includes("CANCELED")) {
                            $('.custom-alert').fadeIn();
                            toastr.warning('Selected row already canceled.');
                        }
                        else {
                            requestURL = '../../../SalesInvoice/GetSalesInvoice';
                            var forcomplete = {};
                            $.ajax({
                                type: "GET",
                                contentType: "application/json; charset=utf-8",
                                url: requestURL,
                                data: { id: Number(lastid) },
                                dataType: "json",
                                success: function (data) {
                                    forcomplete = data;
                                },
                                complete: function () {
                                    $('#series').val(forcomplete.Series).trigger("chosen:updated");
                                    $('#docnum').val(forcomplete.DocNum);
                                    $('#docentry').val(forcomplete.DocEntry);
                                    $('#cardcode').val(forcomplete.CardCode).trigger("chosen:updated");
                                    $('#cardname').val(forcomplete.CardCode).trigger("chosen:updated");
                                    $('#reference').val(forcomplete.Reference);
                                    $('#pricelistid').val(forcomplete.PricelistId);
                                    $('#termid').val(forcomplete.TermId).trigger("chosen:updated");
                                    $('#status').val(forcomplete.Status).trigger("chosen:updated");
                                    $('#date').val(moment(forcomplete.Date).format('MM/DD/YYYY'));
                                    $('#deliverydate').val(moment(forcomplete.DeliveryDate).format('MM/DD/YYYY'));
                                    $('#duedate').val(moment(forcomplete.DueDate).format('MM/DD/YYYY'));
                                    $('#total').val(addThousandsSeparator(forcomplete.DocTotal.toFixed(2)));
                                    $('#discount').val(addThousandsSeparator(forcomplete.Discount.toFixed(2)));
                                    $('#discountamount').val(addThousandsSeparator(forcomplete.DiscountAmount.toFixed(2)));
                                    $('#wtaxamount').val(addThousandsSeparator(forcomplete.WTaxAmount.toFixed(2)));
                                    $('#vatamount').val(addThousandsSeparator(forcomplete.VatAmount.toFixed(2)));
                                    $('#grosstotal').val(addThousandsSeparator(forcomplete.GrossTotal.toFixed(2)));

                                    //Display Pricelist
                                    requestURL = '../../../Pricelist/GetPricelist';
                                    var forcomplete3 = {};
                                    $.ajax({
                                        type: "GET",
                                        contentType: "application/json; charset=utf-8",
                                        url: requestURL,
                                        data: { id: forcomplete.PricelistId },
                                        dataType: "json",
                                        success: function (data) {
                                            forcomplete3 = data;
                                        },
                                        complete: function () {
                                            if (forcomplete3 != null) {
                                                $('#pricelistid').val(forcomplete3.PricelistId);
                                                $('#pricelistname').val(forcomplete3.Name);
                                            }
                                        }
                                    });

                                    //>>end
                                    //Display Item Details
                                    $('#tableListItem tbody tr').remove();
                                    $.each(forcomplete.Lines, function (i, item) {
                                        var $item = $('#tableListItem tbody');
                                        $item.append(
                                            '<tr>' +
                                            '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + ItemRow + '" onclick="RemoveItemRow(this)"><span class="fa fa-remove"></span></a></td>' +
                                            '<td><input type="text" class="form-control" id="linenum-' + ItemRow + '" value="' + (ItemRow + 1) + '" disabled /></td>' +
                                            '<td><select id="itemcode-' + ItemRow + '" class="form-control chosen-select" onchange="ItemCodeOnChange(getRowID(this));AvailStockOnChange(getRowID(this));"></select></td>' +
                                            '<td><select id="itemname-' + ItemRow + '" class="form-control chosen-select" onchange="ItemNameOnChange(getRowID(this));AvailStockOnChange(getRowID(this));"></select></td>' +
                                            '<td><input type="text" id="availstock-' + ItemRow + '" class="c4 form-control text-right" placeholder="0.00"  onkeypress="return isNumberKey(event)" disabled/> </td>' +
                                            '<td> <input type="text" id="qty-' + ItemRow + '" value="' + addThousandsSeparator(item.Quantity.toFixed(2)) + '" class="c4 form-control text-right" placeholder="0.00" onchange="QtyOnChange(getRowID(this));"  onkeypress="return isNumberKey(event)" /> </td>' +
                                            '<td><select id="linewtax-' + ItemRow + '" class="form-control chosen-select"  onchange="calculate_row(getRowID(this));" ></select></td>' +
                                            '<td><select id="linevat-' + ItemRow + '" class="form-control chosen-select" onchange="calculate_row(getRowID(this));" ></select></td>' +
                                            '<td><select id="whse-' + ItemRow + '" class="form-control chosen-select" onchange="AvailStockOnChange(getRowID(this));"></select></td>' +
                                            '<td><input type="text" class="form-control" id="uom-' + ItemRow + '" value="PIECE" disabled /></td>' +
                                            '<td> <input type="text" id="linediscount-' + ItemRow + '" value="' + addThousandsSeparator(item.Discount.toFixed(2)) + '" class="c4 form-control text-right" placeholder="0.00" onchange="calculate_row(getRowID(this));"  onkeypress="return isNumberKey(event)" /></td>' +
                                            //'<td><input type = "hidden" id = "linetotal-' + ItemRow + '" class= "c4 form-control text-right" />' +
                                            //'<input type="text" id="linewtax-' + ItemRow + '" value="' + addThousandsSeparator(item.WTax.toFixed(2)) + '" class="c4 form-control text-right" placeholder="0.00" onchange="calculate_row(getRowID(this));updatetotal();" onkeypress="return isNumberKey(event)" /></td >' +
                                            //'<td><select id="whse-' + ItemRow + '" class="form-control chosen-select"></select></td>' +
                                            //'<td><select id="linevat-' + ItemRow + '" class="form-control chosen-select" onchange="calculate_row(getRowID(this));updatetotal();"></select></td>' +
                                            '<td> <input type="hidden" id="pricelistgross-' + ItemRow + '"> <input type="text" id="linegross-' + ItemRow + '" value="' + addThousandsSeparator(item.GrossPrice.toFixed(2)) + '" class="c4 form-control text-right" placeholder="0.00" onchange="calculate_row(getRowID(this),true);" onkeypress="return isNumberKey(event)" /></td>' +
                                            '<td> <input type="text" id="linegrosstotal-' + ItemRow + '" value="' + addThousandsSeparator(item.GrossTotal.toFixed(2)) + '" class="c4 form-control text-right" placeholder="0.00" onkeypress="return isNumberKey(event)" disabled /></td>' +
                                            '</tr>');
                                        PopulateItem(ItemRow, item.ItemCode);
                                        PoppulateWhse(ItemRow, item.Whse);
                                        PoppulateVat(ItemRow, item.Vat);
                                        PopulateWTax(ItemRow, item.WTax);
                                        ItemRow++;
                                    });
                                    //>>end
                                    disablefield("", false);

                                    $('#ModalTitle').html('Edit Sales Invoice');
                                    $('#salesinvoice').text('Update Sales Invoice').removeClass('btn btn-success').addClass('btn btn-primary');
                                    $('#MyModalSalesInvoice').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    }
                    else {
                        $('.custom-alert').fadeIn();
                        toastr.warning('Error encountered while checking status. Please contact administrator.', 'System Warning');
                    }
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
            var requestURL = '../../../Home/CheckStatus';
            var forcomplete2 = {};
            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                url: requestURL,
                data: { transtype: "SalesInvoices", docentry: lastSelectedIds },
                dataType: "json",
                success: function (data) {
                    forcomplete2 = data;
                },
                complete: function () {
                    if (forcomplete2 != null) {
                        if (forcomplete2.Status && forcomplete2.Remarks.includes('FULLYPAID')) {
                            $('.custom-alert').fadeIn();
                            toastr.warning('Selected row already fully paid.');
                        }
                        else if (forcomplete2.Remarks.includes("CANCELED")) {
                            $('.custom-alert').fadeIn();
                            toastr.warning('Selected row already canceled.');
                        }
                        else {
                            swal({
                                title: "Are you sure,",
                                text: "You want to cancelled this salesinvoice?",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "Yes, cancelled this salesinvoice!",
                                cancelButtonText: "No, Don't cancelled!"
                            }).then((result) => {
                                if (result.value) {
                                    swal({
                                        title: 'Loading... Please Wait!',
                                        text: 'Cancelling salesinvoice...',
                                        allowOutsideClick: false,
                                        allowEscapeKey: false,
                                        onOpen: () => {
                                            swal.showLoading();
                                        }
                                    });
                                    var requestURL = '../../../SalesInvoice/Cancelled';
                                    var forcomplete = {};
                                    $.ajax({
                                        type: "DELETE",
                                        url: requestURL,
                                        data: { id: lastSelectedIds },
                                        success: function (data2) {
                                            forcomplete = data2;
                                        },
                                        complete: function () {
                                            swal.hideLoading();
                                            swal({
                                                type: forcomplete.HttpStatus === 200 ? 'success' : 'warning',
                                                title: forcomplete.HttpStatus === 200 ? "Successful" : "Error cancelling salesinvoice!",
                                                text: forcomplete.Message,
                                                allowOutsideClick: false,
                                                allowEscapeKey: false
                                            }).then((result) => {
                                                if (forcomplete.HttpStatus === 200) {
                                                    if (result.value) {
                                                        var url = '../../../SalesInvoice/Index';
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
                    }
                    else {
                        $('.custom-alert').fadeIn();
                        toastr.warning('Error encountered while checking status. Please contact administrator.', 'System Warning');
                    }
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
            var requestURL = '../../../SalesInvoice/GetSalesinvoice';
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
                    $('#series').val(forcomplete.Series).trigger("chosen:updated");
                    $('#docnum').val(forcomplete.DocNum);
                    $('#docentry').val(forcomplete.DocEntry);
                    $('#cardcode').val(forcomplete.CardCode).trigger("chosen:updated");
                    $('#cardname').val(forcomplete.CardCode).trigger("chosen:updated");
                    $('#reference').val(forcomplete.Reference);
                    $('#pricelistid').val(forcomplete.PricelistId);
                    $('#termid').val(forcomplete.TermId).trigger("chosen:updated");
                    $('#status').val(forcomplete.Status).trigger("chosen:updated");
                    $('#date').val(moment(forcomplete.Date).format("MM/D/YYYY"));
                    $('#deliverydate').val(moment(forcomplete.DeliveryDate).format("MM/D/YYYY"));
                    $('#duedate').val(moment(forcomplete.DueDate).format("MM/D/YYYY"));
                    $('#total').val(addThousandsSeparator(forcomplete.DocTotal.toFixed(2)));
                    $('#discount').val(addThousandsSeparator(forcomplete.Discount.toFixed(2)));
                    $('#discountamount').val(addThousandsSeparator(forcomplete.DiscountAmount.toFixed(2)));
                    $('#wtaxamount').val(addThousandsSeparator(forcomplete.WTaxAmount.toFixed(2)));
                    $('#vatamount').val(addThousandsSeparator(forcomplete.VatAmount.toFixed(2)));
                    $('#grosstotal').val(addThousandsSeparator(forcomplete.GrossTotal.toFixed(2)));
                    $('#totaltopay').val(addThousandsSeparator(forcomplete.GrossTotal.toFixed(2)));
                    requestURL = '../../../Pricelist/GetPricelist';
                    var forcomplete3 = {};
                    $.ajax({
                        type: "GET",
                        contentType: "application/json; charset=utf-8",
                        url: requestURL,
                        data: { id: forcomplete.PricelistId },
                        dataType: "json",
                        success: function (data) {
                            forcomplete3 = data;
                        },
                        complete: function () {
                            if (forcomplete3 != null) {
                                $('#pricelistid').val(forcomplete3.PricelistId);
                                $('#pricelistname').val(forcomplete3.Name);
                            }
                        }
                    });
                    //Display Item Details
                    $('#tableListItem tbody tr').remove();
                    $.each(forcomplete.Lines, function (i, item) {
                        var $item = $('#tableListItem tbody');
                        $item.append(
                            '<tr>' +
                            '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + ItemRow + '" onclick="RemoveItemRow(this)"><span class="fa fa-remove"></span></a></td>' +
                            '<td><input type="text" class="form-control" id="linenum-' + ItemRow + '" value="' + (ItemRow + 1) + '" disabled /></td>' +
                            '<td><select id="itemcode-' + ItemRow + '" class="form-control chosen-select" onchange="ItemCodeOnChange(getRowID(this));AvailStockOnChange(getRowID(this));"></select></td>' +
                            '<td><select id="itemname-' + ItemRow + '" class="form-control chosen-select" onchange="ItemNameOnChange(getRowID(this));AvailStockOnChange(getRowID(this));"></select></td>' +
                            '<td><input type="text" id="availstock-' + ItemRow + '" class="c4 form-control text-right" placeholder="0.00"  onkeypress="return isNumberKey(event)" disabled/> </td>' +
                            '<td> <input type="text" id="qty-' + ItemRow + '" value="' + addThousandsSeparator(item.Quantity.toFixed(2)) + '" class="c4 form-control text-right" placeholder="0.00" onchange="QtyOnChange(getRowID(this));"  onkeypress="return isNumberKey(event)" /> </td>' +
                            '<td><select id="linewtax-' + ItemRow + '" class="form-control chosen-select"  onchange="calculate_row(getRowID(this));"></select></td>' +
                            '<td><select id="linevat-' + ItemRow + '" class="form-control chosen-select" onchange="calculate_row(getRowID(this));"></select></td>' +
                            '<td><select id="whse-' + ItemRow + '" class="form-control chosen-select" onchange="AvailStockOnChange(getRowID(this));"></select></td>' +
                            '<td><input type="text" class="form-control" id="uom-' + ItemRow + '" value="PIECE" disabled /></td>' +
                            '<td> <input type="text" id="linediscount-' + ItemRow + '" value="' + addThousandsSeparator(item.Discount.toFixed(2)) + '" class="c4 form-control text-right" placeholder="0.00" onchange="calculate_row(getRowID(this));"  onkeypress="return isNumberKey(event)" /></td>' +
                            //'<td><input type = "hidden" id = "linetotal-' + ItemRow + '" class= "c4 form-control text-right" />' +
                            //'<input type="text" id="linewtax-' + ItemRow + '" value="' + addThousandsSeparator(item.WTax.toFixed(2)) + '" class="c4 form-control text-right" placeholder="0.00" onchange="calculate_row(getRowID(this));updatetotal();" onkeypress="return isNumberKey(event)" /></td >' +
                            //'<td><select id="whse-' + ItemRow + '" class="form-control chosen-select"></select></td>' +
                            //'<td><select id="linevat-' + ItemRow + '" class="form-control chosen-select" onchange="calculate_row(getRowID(this));updatetotal();"></select></td>' +
                            '<td> <input type="hidden" id="pricelistgross-' + ItemRow + '"> <input type="text" id="linegross-' + ItemRow + '" value="' + addThousandsSeparator(item.GrossPrice.toFixed(2)) + '" class="c4 form-control text-right" placeholder="0.00" onchange="calculate_row(getRowID(this),true);" onkeypress="return isNumberKey(event)" /></td>' +
                            '<td> <input type="text" id="linegrosstotal-' + ItemRow + '" value="' + addThousandsSeparator(item.GrossTotal.toFixed(2)) + '" class="c4 form-control text-right" placeholder="0.00" onkeypress="return isNumberKey(event)" disabled /></td>' +
                            '</tr>');
                        PopulateItem(ItemRow, item.ItemCode);
                        PoppulateWhse(ItemRow, item.Whse);
                        PoppulateVat(ItemRow, item.Vat);
                        PopulateWTax(ItemRow, item.WTax);
                        ItemRow++;
                    });
                    //>>end

                    disablefield('edit', true);
                    $('#ModalTitle').html('Sales Invoice Details');
                    $('#salesinvoice').text('Edit Sales Invoice').addClass('btn btn-success');
                    $('#MyModalSalesInvoice').modal({ backdrop: 'static', keyboard: false });
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
            $('#cardcode').prop('disabled', pDisable).trigger("chosen:updated");
            $('#cardname').prop('disabled', pDisable).trigger("chosen:updated");
            break;
        default:
            $('#series').prop('disabled', !pDisable).trigger("chosen:updated");
            //$('#cardcode').prop('disabled', !pDisable).trigger("chosen:updated");
            //$('#cardname').prop('disabled', !pDisable).trigger("chosen:updated");
            break;
    }
    $('#reference').prop('disabled', pDisable);
    $('#termid').prop('disabled', pDisable).trigger("chosen:updated");
    $('#deliverydate').prop('disabled', pDisable);
    $('#duedate').prop('disabled', pDisable);
    $('#add-item').prop('disabled', pDisable);
    $('#collection').prop('disabled', pDisable);
    $('#discount').prop('disabled', pDisable);
    $('#discountamount').prop('disabled', pDisable);
    //disableTable(pDisable);
}
function addThousandsSeparator(input) {
    var num_parts = input.toString().split(".");
    num_parts[0] = num_parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return num_parts.join(".");
}
function CheckStatus() {
    var total = parseFloat($('#grosstotal').val().replace(/,/g, '')) - parseFloat($('#amountpaid').val().replace(/,/g, ''));
    var status = 0;
    if (total === 0) {
        status = 0;
    } else if (total === parseFloat($('#grosstotal').val().replace(/,/g, ''))) {
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

//New Functions
function AddNewItemRow() {
    var isValid = true;
    if ($('#cardcode').val() == "") {
        isValid = false;
        $('.custom-alert').fadeIn();
        toastr.warning('Customer is required!.', '');
    }
    if ($('#series').val() == "") {
        isValid = false;
        $('.custom-alert').fadeIn();
        toastr.warning('Document Series is required!.', '');
    }
    if ($('#docnum').val() == "") {
        isValid = false;
        $('.custom-alert').fadeIn();
        toastr.warning('Document Number is required!.', '');
    }
    if (isValid) {
        var $item = $('#tableListItem tbody');
        $item.append(
            '<tr> ' +
            '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + ItemRow + '" onclick="RemoveItemRow(this)"><span class="fa fa-remove"></span></a></td>' +
            '<td><input type="text" class="form-control" id="linenum-' + ItemRow + '" value="' + (ItemRow + 1) + '" disabled /></td>' +
            '<td><input type="hidden" id="statusdetails"/><select id="itemcode-' + ItemRow + '" class="form-control chosen-select" onchange="ItemCodeOnChange(getRowID(this));AvailStockOnChange(getRowID(this));"></select></td>' +
            '<td><select id="itemname-' + ItemRow + '" class="form-control chosen-select" onchange="ItemNameOnChange(getRowID(this));AvailStockOnChange(getRowID(this));"></select></td>' +
            '<td><input type="text" id="availstock-' + ItemRow + '" class="c4 form-control text-right" placeholder="0.00"  onkeypress="return isNumberKey(event)" disabled/> </td>' +
            '<td><input type="text" id="qty-' + ItemRow + '" class="c4 form-control text-right" placeholder="0.00" onchange="QtyOnChange(getRowID(this));"  onkeypress="return isNumberKey(event)" /> </td>' +
            '<td><select id="linewtax-' + ItemRow + '" class="form-control chosen-select"  onchange="calculate_row(getRowID(this));"></select></td>' +
            '<td><select id="linevat-' + ItemRow + '" class="form-control chosen-select" onchange="calculate_row(getRowID(this));"></select></td>' +
            '<td><select id="whse-' + ItemRow + '" class="form-control chosen-select" onchange="AvailStockOnChange(getRowID(this));"></select></td>' +
            '<td><input type="text" class="form-control" id="uom-' + ItemRow + '" value="PIECE" disabled /></td>' +
            '<td><input type = "hidden" id = "linetotal-' + ItemRow + '" class= "c4 form-control text-right" /> <input type="text" id="linediscount-' + ItemRow + '" class="c4 form-control text-right" placeholder="0.00" onchange="calculate_row(getRowID(this));"  onkeypress="return isNumberKey(event)" /></td>' +
            //'<td><input type = "hidden" id = "linetotal-' + ItemRow + '" class= "c4 form-control text-right" />' +
            //'<input type="text" id="linewtax-' + ItemRow + '" class="c4 form-control text-right" placeholder="0.00" onchange="calculate_row(getRowID(this));updatetotal();" onkeypress="return isNumberKey(event)" /></td >' +
            '<td> <input type="hidden" id="pricelistgross-' + ItemRow + '"> <input type="text" id="linegross-' + ItemRow + '" class="c4 form-control text-right" placeholder="0.00" onchange="calculate_row(getRowID(this),true);" onkeypress="return isNumberKey(event)" /></td>' +
            '<td> <input type="text" id="linegrosstotal-' + ItemRow + '" class="c4 form-control text-right" placeholder="0.00" onkeypress="return isNumberKey(event)" disabled /></td>' +
            '</tr>');
        PopulateItem(ItemRow);
        PoppulateWhse(ItemRow);
        PoppulateVat(ItemRow, 0);
        PopulateWTax(ItemRow, 0.01);
        ItemRow++;
    }
}
function PopulateItem(row, itemcode) {
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
                if (val.isSellItem === true) {
                    $("#itemcode-" + row).append($('<option/>').val(val.ItemCode).text(val.ItemCode)).trigger('chosen:updated');
                }
            });
            $("#itemname-" + row).empty().trigger('chosen:updated');;
            $("#itemname-" + row).append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete, function (i, val) {
                if (val.isSellItem === true) {
                    $("#itemname-" + row).append($('<option/>').val(val.ItemCode).text(val.ItemName)).trigger('chosen:updated');
                }
            });
            if (itemcode != null) {
                $("#itemcode-" + row).val(itemcode).trigger("chosen:updated");
                $("#itemname-" + row).val(itemcode).trigger("chosen:updated");
            }
            $('.chosen-select').chosen({ width: "100%" });
        }
    });
}
function PoppulateWhse(row, whse) {
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
            $("#whse-" + row).empty().trigger('chosen:updated');;
            $("#whse-" + row).append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete, function (i, val) {
                $("#whse-" + row).append($('<option/>').val(val.Code).text(val.Name)).trigger('chosen:updated');
            });
            if (whse != null) {
                $('#whse-' + row).val(whse).trigger("chosen:updated");
            }
            $('.chosen-select').chosen({ width: "100%" });
        }
    });
}
function PoppulateVat(row, vat) {
    var requestURL = '../../../Vat/GetVatInfo';
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
            $("#linevat-" + row).empty().trigger('chosen:updated');;
            $("#linevat-" + row).append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete, function (i, val) {
                if (val.Type == 1) {
                    var text = val.Code + " - " + (val.Percentage) + "%";
                    var value = val.Percentage / 100;
                    $("#linevat-" + row).append($('<option/>').val(value).text(text)).trigger('chosen:updated');
                }
            });
            if (vat != null) {
                $('#linevat-' + row).val(vat).trigger("chosen:updated");
            }
            $('.chosen-select').chosen({ width: "100%" });
        }
    });
}
function PopulateWTax(row, wtax) {
    var requestURL = '../../../WTax/GetWTaxInfo';
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
            $("#linewtax-" + row).empty().trigger('chosen:updated');;
            $("#linewtax-" + row).append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete, function (i, val) {
                if (val.Type == 0) {
                    var text = val.Code + " - " + (val.Percentage) + "%";
                    var value = val.Percentage / 100;
                    $("#linewtax-" + row).append($('<option/>').val(value).text(text)).trigger('chosen:updated');
                }
            });
            if (wtax != null) {
                $('#linewtax-' + row).val(wtax).trigger("chosen:updated");
            }
            $('.chosen-select').chosen({ width: "100%" });
        }
    });
}
function RemoveItemRow(obj) {
    $(obj).parent().parent().remove();
    updateIds();
    updatetotal("");
}
function updateIds() {
    ItemRow = 0;
    $('#tableListItem').find('tbody > tr').each(function (index) {
        $(this).find('[id^=removes]').attr('id', 'removes-' + index);
        $(this).find('[id^=linenum]').attr('id', 'linenum-' + index);
        $(this).find('[id^=linenum]').val(index + 1);
        $(this).find('[id^=itemcode]').attr('id', 'itemcode-' + index);
        $(this).find('[id^=itemname]').attr('id', 'itemname-' + index);
        $(this).find('[id^=qty]').attr('id', 'qty-' + index);
        $(this).find('[id^=price]').attr('id', 'price-' + index);
        $(this).find('[id^=uom]').attr('id', 'uom-' + index);
        $(this).find('[id^=linediscount]').attr('id', 'linediscount-' + index);
        $(this).find('[id^=linetotal]').attr('id', 'linetotal-' + index);
        $(this).find('[id^=linewtax]').attr('id', 'linewtax-' + index);
        $(this).find('[id^=whse]').attr('id', 'whse-' + index);
        $(this).find('[id^=linevat]').attr('id', 'linevat-' + index);
        $(this).find('[id^=linegross]').attr('id', 'linegross-' + index);
        $(this).find('[id^=linegrosstotal]').attr('id', 'linegrosstotal-' + index);
        ItemRow++;
    });
}
function ItemCodeOnChange(row) {
    $('#itemname-' + row).val($('#itemcode-' + row).val()).trigger("chosen:updated");
    calculate_row(row, false, "");
}
function ItemNameOnChange(row) {
    $('#itemcode-' + row).val($('#itemname-' + row).val()).trigger("chosen:updated");
    calculate_row(row, false, "");
}
function QtyOnChange(row) {
    var isAllValid = true;
    var availstock = parseFloat($('#availstock-' + row).val().replace(/,/g, ''));
    var qty = parseFloat($('#qty-' + row).val().replace(/,/g, ''));
    if (isNaN(availstock)) { availstock = 0; }
    if (availstock < 0 || availstock < qty) {
        $('.custom-alert').fadeIn();
        toastr.warning('Available Stock is less than inputted quantity. Please check Available Stock', '');
        isAllValid = false;
        $('#qty-' + row).val(addThousandsSeparator(0.00));
        $('#pricelistgross-' + row).val(addThousandsSeparator(0.00));
        $('#linegross-' + row).val(addThousandsSeparator(0.00));
    }
    if (isAllValid) {
        var requestURL = '../../../Items/CheckItemPrice';
        var forcomplete = {};
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: requestURL,
            data: { pricelistid: $('#pricelistid').val(), itemcode: $('#itemcode-' + row).val() },
            dataType: "json",
            success: function (data) {
                forcomplete = data;
            },
            complete: function () {
                if (forcomplete != null) {
                    var qty = parseFloat($('#qty-' + row).val().replace(',', ''));
                    var wholesaleprice = parseFloat(forcomplete.WholeSalePrice);
                    var retailprice = parseFloat(forcomplete.RetailPrice);
                    if (isNaN(qty)) { qty = 0; }
                    if (isNaN(wholesaleprice)) { wholesaleprice = 0; }
                    if (isNaN(retailprice)) { retailprice = 0; }

                    if (qty >= forcomplete.WholeSaleQty) {
                        $('#linegross-' + row).val(addThousandsSeparator(wholesaleprice.toFixed(2)));
                        $('#pricelistgross-' + row).val(addThousandsSeparator(wholesaleprice.toFixed(2)));
                    }
                    else {
                        $('#linegross-' + row).val(addThousandsSeparator(retailprice.toFixed(2)));
                        $('#pricelistgross-' + row).val(addThousandsSeparator(retailprice.toFixed(2)));
                    }
                    calculate_row(row, false, "");
                }
            }
        });
    }
}
function AvailStockOnChange(row) {
    var requestURL = '../../../Items/GetAvailStock';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { itemcode: $('#itemcode-' + row).val(), warehouse: $('#whse-' + row + ' option:selected').val() },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null) {
                $('#availstock-' + row).val(addThousandsSeparator(forcomplete.toFixed(2)));
            }
        }
    });
}
function getRowID(obj) {
    return $(obj).closest('tr').index();
}
function calculate_row(row, inputgross, discounttype) {
    var qty = parseFloat($('#qty-' + row).val());
    //var unitprice = parseFloat($('#unitprice', $this).val().replace(',', ''));
    var linediscount = parseFloat($('#linediscount-' + row).val());
    //var priceafdiscount = parseFloat($('#priceafdiscount', $this).val().replace(',', ''));
    //var linetotal = parseFloat($('#linetotal-'+ $this).val().replace(',', ''));
    var linewtax = parseFloat($('#linewtax-' + row + ' option:selected').val());
    var linevat = parseFloat($('#linevat-' + row + ' option:selected').val());
    var linegross = 0;
    if (inputgross === true) {
        linegross = parseFloat($('#linegross-' + row).val());
        $('#pricelistgross-' + row).val(addThousandsSeparator(linegross.toFixed(2)));
    }
    else {
        linegross = parseFloat($('#pricelistgross-' + row).val());
    }
    var linegrosstotal = parseFloat($('#linegrosstotal-' + row).val());

    if (isNaN(qty)) { qty = 0; }
    //if (isNaN(unitprice)) { unitprice = 0; }
    if (isNaN(linediscount)) { linediscount = 0; }
    //if (isNaN(priceafdiscount)) { priceafdiscount = 0; }
    //if (isNaN(linetotal)) { linetotal = 0; }
    if (isNaN(linewtax)) { linewtax = 0; }
    if (isNaN(linevat)) { linevat = 0; }
    if (isNaN(linegross)) { linegross = 0; }
    if (isNaN(linegrosstotal)) { linegrosstotal = 0; }


    //computation
    //price after discount
    //var newpriceafdis = unitprice * ((100 - linediscount) / 100);
    //var newlinetotal = qty * priceafdiscount;
    //var newgrossprice = linegross * ((100 + linevat) / 100);
    var grosswithvat = linevat != 0 ? parseFloat(linegross) * (parseFloat(linevat) + 1) : parseFloat(linegross);
    var grosswithdiscount = linediscount != 0 ? grosswithvat * ((100 - linediscount) / 100) : grosswithvat;
    //var newgrossprice = linediscount != 0 ? (linevat != 0 ? grosswithvat : 0) *  : (linevat != 0 ? parseFloat(linegross) * (parseFloat(linevat) + 1) : 0);
    var newgrossprice = grosswithdiscount;
    var newgrostotal = qty * newgrossprice;

    $('#qty-' + row).val(addThousandsSeparator(qty.toFixed(2)));
    //$('#unitprice', $this).val(addThousandsSeparator(unitprice.toFixed(2)));
    $('#linediscount-' + row).val(addThousandsSeparator(linediscount.toFixed(2)));
    //$('#priceafdiscount', $this).val(addThousandsSeparator(priceafdiscount.toFixed(2)));
    //$('#linetotal', $this).val(addThousandsSeparator(newlinetotal.toFixed(2)));
    $('#linewtax-' + row).val(addThousandsSeparator(linewtax.toFixed(2)));
    $('#linegross-' + row).val(addThousandsSeparator(newgrossprice.toFixed(2)));
    $('#linegrosstotal-' + row).val(addThousandsSeparator(newgrostotal.toFixed(2)));
    updatetotal(discounttype);
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
        var linevat = parseFloat($('#linevat-' + i).val());
        var linewtax = parseFloat($('#linewtax-' + i).val());
        var linegrosstotal = parseFloat($('#linegrosstotal-' + i).val());
        if (isNaN(linevat)) { linevat = 0; }
        if (isNaN(linewtax)) { linewtax = 0; }
        if (isNaN(linegrosstotal)) { linegrosstotal = 0; }
        total += parseFloat(linegrosstotal);
        wtax += linewtax != 0 ? parseFloat(linegrosstotal) * parseFloat(linewtax) : 0;
        //vat += parseFloat($('#linegrosstotal-' + i, this).val().replace(/,/g, '')) * (parseFloat(linevat) / 100);
        vat += linevat != 0 ? parseFloat(linegrosstotal) * (parseFloat(linevat) + 1) : 0;
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