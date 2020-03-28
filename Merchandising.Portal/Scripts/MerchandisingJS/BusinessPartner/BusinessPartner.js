
$(document).ready(function () {//decleration of controls
    $('.chosen-select').chosen({ width: "100%" });

    // Bind normal buttons
    Ladda.bind('.ladda-button', { timeout: 2000 });

    //var elem = document.querySelector('.js-switch');
    //var switchery = new Switchery(elem, { color: '#1AB394' });
    //var elem2 = document.querySelector('.js-switch2');
    //var switchery2 = new Switchery(elem2, { color: '#1AB394' });
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
var lastSelectedIds;
$('#ApiDemoGrid').on('aweselect',
    function () {
        lastSelectedIds =
            $.map($(this).data('api').getSelection(), function (val) { return val.Code; });
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
    var custobject = 1;
    var supobject = 2;
    var documentname = "Business Partner Master Data";
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { objectcode: custobject, objectcode2: supobject },
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
                $('#enablewtax').prop("checked", false);
                $('#status').prop("checked", true);
                $('#ModalTitle').html('Create Business Partner');
                $('#businesspartner').text('Add Business Partner').removeClass('btn btn-success').addClass('btn btn-primary');
                clearAll();
                $('#bptype').val('C_1').trigger("chosen:updated");
                LoadBPType("C");
                $('#MyModalBusinessPartner').modal({ backdrop: 'static', keyboard: false });
            }
        }
    });
});
$('#address').click(function (e) {
    //$('#MyModalBPAddress').modal({ backdrop: 'static', keyboard: false });
    $('#MyModalBPAddress').modal();
    //$('#ModalTitle').html("UoM Group - (" + $('#itemcode').val() + "-" + $('#itemname').val().toUpperCase() + ")");
});
$('#wtax').click(function (e) {
    //$('#MyModalBPWTax').modal({ backdrop: 'static', keyboard: false });
    $('#MyModalBPWTax').modal();
});
$('#viewtrans').click(function () {
    var requestURL = '../../../BusinessPartner/GetBPBalanceDetails';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { id: $('#cardcode').val() },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null) {
                $('#tableListBPBalance tbody tr').remove();
                for (var i = 0; i < forcomplete.length; i++) {
                    var $item = $('#tableListBPBalance tbody');
                    $item.append(
                        '<tr>' +
                        '<td><input type="text" class="form-control" id="linenum-' + i + '" value="' + (i + 1) + '" disabled /></td>' +
                        '<td><a id="doc_.(CardCode)" class="view_doc">(' + forcomplete[i].Document + ')</a></td>' +
                        //'<td><input type="text" class="form-control" id="document-' + i + '" value="' + forcomplete[i].Document + '" disabled /></td>' +
                        //'<td><input type="text" class="form-control" id="status-' + i + '" value="' + forcomplete[i].Status + '" disabled /></td>' +
                        '<td>' + DisplayStatus(forcomplete[i].Status) + '</td>' +
                        '<td><input type="text" class="form-control" id="total-' + i + '" value="' + addThousandsSeparator(forcomplete[i].Total.toFixed(2)) + '" disabled /></td>' +
                        '<td><input type="text" class="form-control" id="grosstotal-' + i + '" value="' + addThousandsSeparator(forcomplete[i].GrossTotal.toFixed(2)) + '" disabled /></td>' +
                        '<td><input type="text" class="form-control" id="transdate-' + i + '" value="' + moment(forcomplete[i].TransactionDate).format("YYYY-MM-DD") + '" disabled /></td>' +
                        '<td><input type="text" class="form-control" id="duedate-' + i + '" value="' + moment(forcomplete[i].DueDate).format("YYYY-MM-DD") + '" disabled /></td>' +
                        '<td><input type="text" class="form-control" id="reference-' + i + '" value="' + forcomplete[i].Reference + '" disabled /></td>' +
                        '</tr>');
                };
                $('#MyModalBPBalance').modal();
            }
        }
    });
});
$('#businesspartner').click(function () {
    var bpaddress = [];
    var bpwtax = [];
    var title = $('#businesspartner').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Business Partner');
        $('#businesspartner').text('Update Business Partner').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;


        if ($('#bptype').val().trim() === '') {
            $('.custom-alert').fadeIn();
            toastr.warning('BP Type is required.', '');
            isAllValid = false;
        }
        if ($('#cardcode').val().trim() === '') {
            $('.custom-alert').fadeIn();
            toastr.warning('Card Code is required.', '');
            isAllValid = false;
        }
        if ($('#cardname').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Card Name is required.', '');
            isAllValid = false;
        }
        if ($('#bpgroup').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('BP Group is required.', '');
            isAllValid = false;
        }
        //if ($('#tin').val().trim() === '') {

        //    $('.custom-alert').fadeIn();
        //    toastr.warning('TIN is required.', '');
        //    isAllValid = false;
        //}
        if ($('#vat').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('VAT is required.', '');
            isAllValid = false;
        }
        //if ($('#addressdisplay').val().trim() === '') {

        //    $('.custom-alert').fadeIn();
        //    toastr.warning('Address is required.', '');
        //    isAllValid = false;
        //}
        //if ($('#contactperson').val().trim() === '') {

        //    $('.custom-alert').fadeIn();
        //    toastr.warning('Contact Person is required.', '');
        //    isAllValid = false;
        //}
        //if ($('#contactnumber').val().trim() === '') {

        //    $('.custom-alert').fadeIn();
        //    toastr.warning('Contact Number is required.', '');
        //    isAllValid = false;
        //}
        if ($('#pricelist').val() === null || $('#pricelist').val()=== '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Pricelist is required.', '');
            isAllValid = false;
        }
        if ($('#paymentterm').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Payment Term is required.', '');
            isAllValid = false;
        }
        if ($('#status').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Status is required.', '');
            isAllValid = false;
        }
        //Address
        if ($('#block').val() != '' || $('#street').val() != '' ||
            $('#city').val() != '' || $('#province').val() != '') {
            var add = {
                CardCode: $('#cardcode').val(),
                Block: $('#block').val().toUpperCase(),
                Street: $('#street').val().toUpperCase(),
                CityId: $('#city option:selected').val(),
                ProvId: $('#province option:selected').val(),
                Status: true
            }
            bpaddress.push(add);
        }
        //>>end

        //WTax
        $('#tableListWTax tbody tr').each(function (index, ele) {

            if ($("#chkBpWTax", this).is(":checked").toString() == "true") {
                var wtaxitem = {
                    CardCode: $('#cardcode').val().trim(),
                    WTCode: $('#wtaxcode', this).val()
                }
                bpwtax.push(wtaxitem);
            }
        })
        //>>end
        if (isAllValid) {
            $("#MyModalBusinessPartner").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding business partner...' : 'Update business partner...';
            var messagetext = title === 'add' ? "Error adding business partner!" : "Error updating business partner!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " business partner?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " business partner!",
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
                        CardCode: $('#cardcode').val().trim(),
                        CardName: $('#cardname').val().toUpperCase().trim(),
                        BPType: $('#bptype').val().split("_")[0].toUpperCase().trim(),
                        BPCode: $('#bpgroup').val().toUpperCase().trim(),
                        Tin: $('#tin').val().toUpperCase().trim(),
                        VatCode: $('#vat').val().toUpperCase().trim(),
                        WithWTax: $('#enablewtax').prop('checked'),
                        Address: $('#addressdisplay').val().toUpperCase().trim(),
                        ContactNumber: Number($('#contactnumber').val().trim()),
                        Email: $('#email').val().trim(),
                        PricelistId: $('#pricelist').val().toUpperCase().trim(),
                        TermId: $('#paymentterm').val().toUpperCase().trim(),
                        Balance: parseFloat($('#balance').val().replace(/,/g, '')),
                        Series: $('#series').val().trim(),
                        ObjectType: $('#bptype').val().split("_")[1],
                        ContactPerson: $('#contactperson').val().toUpperCase().trim(),
                        Remarks: $('#remarks').val().toUpperCase().trim(),
                        Status: $('#status').is(':checked'),
                        BpAddresses: bpaddress,
                        BpWTax: bpwtax,
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: ''
                    }
                    var requestURL = title === 'add' ? '../../../BusinessPartner/Save' : '../../../BusinessPartner/Update';
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
                                        var url = '../../../BusinessPartner/Index';
                                        window.location.href = url;
                                    }
                                }
                                else {
                                    $('#MyModalBusinessPartner').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalBusinessPartner').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalBusinessPartner').modal({ backdrop: 'static', keyboard: false });
                }
            });
        };
    }
});
//$('#clear').click(function () {
//    clearAll();
//});
$('#close').click(function () {
    $('#MyModalBusinessPartner').modal('hide');
});
$('#series').change(function () {
    var series = $(this).val();
    LoadSeries(series);
});
$('#block').keyup(function () {
    bpaddress();
});
$('#street').keyup(function () {
    bpaddress();
});
$('#city').change(function () {
    bpaddress();
});
$('#province').change(function () {
    var provcode = $(this).val();
    var requestURL = '../../../BusinessPartner/GetCities';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { provcode: provcode },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            $("#city").empty().trigger('chosen:updated');;
            $("#city").append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete,
                function (i, val) {
                    $("#city").append($('<option/>').val(val.CityId).text(val.CityName)).trigger("chosen:updated");
                });
        }
    });
    bpaddress();
});
$('#country').keyup(function () {
    bpaddress();
});
$('#enablewtax').click(function () {
    if ($(this).prop("checked") == true) {
        $('#wtax').prop('disabled', false);
    } else {
        $('#wtax').prop('disabled', true);
    }
});
$('#bptype').change(function () {
    var type = $(this).val().split("_")[0];
    LoadBPType(type);
});
function LoadBPType(type) {
    var requestURL = '../../../SequenceTable/GetSeries';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { docsubtype: type },
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
                LoadSeries(defaultval);
            }
        }
    });
}
function LoadSeries(series) {
    var requestURL = '../../../SequenceTable/GetCurrentSequence';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { series: series, objectcode: Number($('#bptype').val().split("_")[1]) },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            if (forcomplete != null)
                $('#cardcode').val(forcomplete[0]);
        }
    });
}
function LoadDefault(type) {
    var requestURL = '../../../SequenceTable/GetSeries';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { docsubtype: type },
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
            }
        }
    });
}
function LoadCities(provcode, cityid) {
    var requestURL = '../../../BusinessPartner/GetCities';
    var forcomplete = {};
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        data: { provcode: provcode },
        dataType: "json",
        success: function (data) {
            forcomplete = data;
        },
        complete: function () {
            $("#city").empty().trigger('chosen:updated');;
            $("#city").append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete,
                function (i, val) {
                    $("#city").append($('<option/>').val(val.CityId).text(val.CityName)).trigger("chosen:updated");
                });
            if (cityid != null)
                $('#city').val(cityid).trigger("chosen:updated");
        }
    });
    bpaddress();
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
function DisplayStatus(status) {
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
function bpaddress() {
    var address = '';
    var block = $('#block').val();
    var street = $('#street').val();
    var city = $('#city option:selected').text();
    var province = $('#province option:selected').text().split(' - ')[1];
    var country = $('#country').val();
    if (block != null)
        address += block + " ";
    if (street != null)
        address += street + " ";
    if (city != null)
        address += city + " ";
    if (province != null)
        address += province + " ";
    if (country != null)
        address += country + " ";
    $('#addressdisplay').val(address);
}
function clearAll() {
    $('#bptype').val('').trigger("chosen:updated");
    $('#series').val('').trigger("chosen:updated");
    $('#cardcode').val('');
    $('#cardname').val('');
    $('#bpgroup').val('').trigger("chosen:updated");
    $('#tin').val('');
    $('#vat').val('').trigger("chosen:updated");
    $('#contactperson').val('');
    $('#contactnumber').val('');
    $('#email').val('');
    $('#pricelist').val('').trigger("chosen:updated");
    $('#paymentterm').val('').trigger("chosen:updated");
}
function initButtonEvent() {
    $('.edit_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        if (lastSelectedIds != null) {
            var requestURL = '../../../BusinessPartner/GetBusinessPartner';
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

                    if (forcomplete.BpType === "C")
                        $('#bptype').val(forcomplete.BpType + "_1").trigger("chosen:updated");
                    else
                        $('#bptype').val(forcomplete.BpType + "_2").trigger("chosen:updated");
                    LoadDefault(forcomplete.BpType);
                    $('#series').val(Number(forcomplete.Series)).trigger("chosen:updated");
                    $('#cardcode').val(forcomplete.CardCode);
                    $('#cardname').val(forcomplete.CardName);
                    $('#balance').val(addThousandsSeparator(forcomplete.Balance.toFixed(2)));
                    $('#bpgroup').val(forcomplete.BpCode).trigger("chosen:updated");
                    $('#tin').val(forcomplete.Tin);
                    $('#vat').val(forcomplete.VatCode).trigger("chosen:updated");
                    $('#contactperson').val(forcomplete.ContactPerson);
                    $('#remarks').val(forcomplete.Remarks);
                    $('#contactnumber').val(forcomplete.ContactNumber);
                    $('#email').val(forcomplete.Email);
                    $('#pricelist').val(forcomplete.PricelistId).trigger("chosen:updated");
                    $('#paymentterm').val(forcomplete.TermId).trigger("chosen:updated");
                    //Display BpWTax
                    $('#enablewtax').prop("checked", forcomplete.WithWTax);
                    $.map(forcomplete.BpWTax, function (value) {
                        $('#tableListWTax tbody tr').each(function (index, ele) {
                            if ($('#wtaxcode', this).val() == value.WTCode) {
                                $("#chkBpWTax", this).prop("checked", true);
                            }
                        })
                    });
                    //>>end
                    //Display Address
                    $('#addressdisplay').val(forcomplete.Address);
                    $.map(forcomplete.BpAddresses, function (value) {
                        if (value.Block != "")
                            $('#block').val(value.Block);
                        if (value.Street != "")
                            $('#street').val(value.Street);
                        if (value.ProvId != "") {
                            $('#province').val(value.ProvId).trigger("chosen:updated");
                            LoadCities(value.ProvId, value.CityId);
                        }
                    });

                    //>>end
                    $("#status").prop("checked", forcomplete.Status);

                    //Check BP Balance
                    if (forcomplete.Balance > 0)
                        $('#viewtrans').prop('disabled', false);
                    else
                        $('#viewtrans').prop('disabled', true);
                    disablefield("", false);
                    $('#ModalTitle').html('Edit Business Partner');
                    $('#businesspartner').text('Update Business Partner').removeClass('btn btn-success').addClass('btn btn-primary');
                    $('#MyModalBusinessPartner').modal({ backdrop: 'static', keyboard: false });
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
                text: "You want to remove this business partner?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, remove business partner!",
                cancelButtonText: "No, cancel remove!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: 'Removing Business Partner...',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var requestURL = '../../../BusinessPartner/Delete';
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
                                title: forcomplete.HttpStatus === 200 ? "Successful" : "Error removing business partner!",
                                text: forcomplete.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (forcomplete.HttpStatus === 200) {
                                    if (result.value) {
                                        var url = '../../../BusinessPartner/Index';
                                        window.location.href = url;
                                    }
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: "Error removing business partner!",
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
            var requestURL = '../../../BusinessPartner/GetBusinessPartner';
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
                    if (forcomplete.BpType === "C")
                        $('#bptype').val(forcomplete.BpType + "_1").trigger("chosen:updated");
                    else
                        $('#bptype').val(forcomplete.BpType + "_2").trigger("chosen:updated");
                    LoadDefault(forcomplete.BpType);
                    $('#series').val(Number(forcomplete.Series)).trigger("chosen:updated");
                    $('#cardcode').val(forcomplete.CardCode);
                    $('#cardname').val(forcomplete.CardName);
                    $('#balance').val(addThousandsSeparator(forcomplete.Balance.toFixed(2)));
                    $('#bpgroup').val(forcomplete.BpCode).trigger("chosen:updated");
                    $('#tin').val(forcomplete.Tin);
                    $('#vat').val(forcomplete.VatCode).trigger("chosen:updated");
                    $('#contactperson').val(forcomplete.ContactPerson);
                    $('#remarks').val(forcomplete.Remarks);
                    $('#contactnumber').val(forcomplete.ContactNumber);
                    $('#email').val(forcomplete.Email);
                    $('#pricelist').val(forcomplete.PricelistId).trigger("chosen:updated");
                    $('#paymentterm').val(forcomplete.TermId).trigger("chosen:updated");
                    //Display BpWTax
                    $('#enablewtax').prop("checked", forcomplete.WithWTax);
                    $.map(forcomplete.BpWTax, function (value) {
                        $('#tableListWTax tbody tr').each(function (index, ele) {
                            if ($('#wtaxcode', this).val() == value.WTCode) {
                                $("#chkBpWTax", this).prop("checked", true);
                            }
                        })
                    });
                    //>>end
                    //Display Address
                    $('#addressdisplay').val(forcomplete.Address);
                    $.map(forcomplete.BpAddresses, function (value) {
                        if (value.Block != "")
                            $('#block').val(value.Block);
                        if (value.Street != "")
                            $('#street').val(value.Street);
                        if (value.ProvId != "") {
                            $('#province').val(value.ProvId).trigger("chosen:updated");
                            LoadCities(value.ProvId, value.CityId);
                        }
                    });

                    //>>end
                    $("#status").prop("checked", forcomplete.Status);

                    //Check BP Balance
                    if (forcomplete.Balance > 0)
                        $('#viewtrans').prop('disabled', false);
                    else
                        $('#viewtrans').prop('disabled', true);
                    disablefield('edit', true);
                    $('#ModalTitle').html('Business Partner Details');
                    $('#businesspartner').text('Edit Business Partner').addClass('btn btn-success');
                    $('#MyModalBusinessPartner').modal({ backdrop: 'static', keyboard: false });
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
            $('#bptype').prop('disabled', pDisable).trigger("chosen:updated");
            $('#viewtrans').prop('disabled', !pDisable);
            if (ptype === "edit")
                $('#wtax').prop('disabled', pDisable);
            else
                $('#wtax').prop('disabled', !pDisable);
            break;
        default:
            $('#series').prop('disabled', !pDisable).trigger("chosen:updated");
            $('#bptype').prop('disabled', !pDisable).trigger("chosen:updated");
            if (parseFloat($('#balance').val().replace(/,/g, '')) > 0) {
                $('#viewtrans').prop('disabled', pDisable);
            }
            if ($('#enablewtax').is(':checked') === true)
                $('#wtax').prop('disabled', pDisable);
            break;
    }
    $('#cardcode').prop('disabled', pDisable);
    $('#cardname').prop('disabled', pDisable);
    $('#bpgroup').prop('disabled', pDisable).trigger("chosen:updated");
    $('#tin').prop('disabled', pDisable);
    $('#vat').prop('disabled', pDisable).trigger("chosen:updated");
    $('#enablewtax').prop('disabled', pDisable);
    $('#address').prop('disabled', pDisable);
    $('#contactperson').prop('disabled', pDisable);
    $('#contactnumber').prop('disabled', pDisable);
    $('#email').prop('disabled', pDisable);
    $('#pricelist').prop('disabled', pDisable).trigger("chosen:updated");
    $('#paymentterm').prop('disabled', pDisable).trigger("chosen:updated");
    $('#status').prop('disabled', pDisable);
    $('#remarks').prop('disabled', pDisable);
}
function checkAllBpWTax(ele) {
    var checkboxes = document.getElementsByTagName('input');

    if (ele.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                if (checkboxes[i].id.toString() == 'chkBpWTax') {
                    checkboxes[i].checked = true;
                }

            }
        }
    } else {
        for (var i = 0; i < checkboxes.length; i++) {
            console.log(i)
            if (checkboxes[i].type == 'checkbox') {
                if (checkboxes[i].id.toString() == 'chkBpWTax') {
                    checkboxes[i].checked = false;
                }
            }
        }
    }
}
function addThousandsSeparator(input) {
    var num_parts = input.toString().split(".");
    num_parts[0] = num_parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return num_parts.join(".");
}