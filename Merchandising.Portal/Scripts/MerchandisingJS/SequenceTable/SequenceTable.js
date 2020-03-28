
$(document).ready(function () {
    //decleration of controls
    $('.chosen-select').chosen({ width: "100%" });

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
var documentname;
var doctype;
var docsubtype;
var objectcode;
var defaultseries;
var ItemRow = 0;
$('#ApiDemoGrid').on('aweselect',
    function () {
        lastSelectedIds =
            $.map($(this).data('api').getSelection(), function (val) { return val.Id; });
        documentname =
            $.map($(this).data('api').getSelection(), function (val) { return val.Document; });
        doctype =
            $.map($(this).data('api').getSelection(), function (val) { return val.DocType; });
        docsubtype =
            $.map($(this).data('api').getSelection(), function (val) { return val.DocSubType; });
        objectcode =
            $.map($(this).data('api').getSelection(), function (val) { return val.ObjectCode; });
        defaultseries =
            $.map($(this).data('api').getSelection(), function (val) { return val.Series; });
    });
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
$('#add').click(function () {  
    var requestURL = '../../../SequenceTable/GetDocuments';
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
            $("#seqdocument").empty().trigger('chosen:updated');;
            $("#seqdocument").append($('<option/>')).trigger('chosen:updated');
            $.map(forcomplete, function (val) {
                    $("#seqdocument").append($('<option/>').val(val.ObjectCode + "|" + val.DocType + "|" + val.DocSubType).text(val.DocumentName)).trigger('chosen:updated');
            });
            disablefield('add', false);
            $('#ModalTitle').html('Create Document Numbering');
            $('#sequence').text('Add Document Numbering').removeClass('btn btn-success').addClass('btn btn-primary');
            clearAll();
            $('#MyModalSequence').modal({ backdrop: 'static', keyboard: false });
        }
    });


});
$('#addseqdoc').click(function () {
    //disablefield('addsequ', false);

    document.getElementById("status").checked = true;
    $('#ModalTitle').html('Create Document');
    $('#sequencedocument').text('Add Document').removeClass('btn btn-success').addClass('btn btn-primary');
    clearAll();
    LoadDocuments();
    $('#MyModalDocument').modal({ backdrop: 'static', keyboard: false });
});
$('#sequence').click(function () {

    var title = $('#sequence').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Sequence');
        $('#sequence').text('Update Sequence').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;
        if ($('#seqdocument option:selected').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Document is required.', '');
            isAllValid = false;
        }
        if (isAllValid) {
            $("#MyModalSequence").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding document numbering...' : 'Updating document numbering...';
            var messagetext = title === 'add' ? "Error adding document numbering!" : "Error updating document numbering!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " this document numbering?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " this document numbering!",
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
                    var data = {
                        Id: title === "update" ? Number($('#seqid').val().trim()) : 0,
                        ObjectCode: Number($('#seqdocument option:selected').val().split("|")[0]),
                        DocSubType: $('#seqdocument option:selected').val().split("|")[2].toUpperCase().trim(),
                        DefaultSeries: Number($('#defaultseries option:selected').val()),

                        //Prefix: $('#seqprefix').val().toUpperCase().trim(),
                        //Suffix: $('#seqsuffix').val().toUpperCase().trim(),
                        //InitialNum: $('#seqinitialnum').val().trim(),
                        //NextNumber: $('#seqnextnum').val().trim(),
                        //LastNumber: $('#seqlastnum').val().trim(),
                        //BranchCode: $('#branchcode').val().toUpperCase().trim(),
                        Status: true,
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: ''
                    }
                    var requestURL = title === 'add' ? '../../../SequenceTable/Save' : '../../../SequenceTable/Update';
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
                                        var url = '../../../SequenceTable/Index';
                                        window.location.href = url;
                                        clearAll();
                                    }
                                }
                                else {
                                    $('#MyModalSequence').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalSequence').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalSequence').modal({ backdrop: 'static', keyboard: false });
                }
            });
        };
    }
});
$('#sequencedocument').click(function () {

    var title = $('#sequencedocument').text().toLowerCase().split(' ')[0];
    if (title === "edit") {
        disablefield('update', false);
        $('#ModalTitle').html('Edit Document');
        $('#sequencedocument').text('Update Document').removeClass('btn btn-success').addClass('btn btn-primary');
    }
    else {
        var isAllValid = true;

        if ($('#seqdocname').val().toUpperCase().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Doc Name is required.', '');
            isAllValid = false;
        }
        if ($('#seqdoctype').val().trim() === '') {

            $('.custom-alert').fadeIn();
            toastr.warning('Doc Type is required.', '');
            isAllValid = false;
        }
        if (isAllValid) {
            $("#MyModalDocument").modal('hide');
            var canceltext = title === 'add' ? 'adding' : 'updating';
            var swaltext = title === 'add' ? 'Adding document...' : 'Updating document...';
            var messagetext = title === 'add' ? "Error adding document!" : "Error updating document!";
            swal({
                title: "Are you sure?",
                text: "You want to " + title + " document?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + title + " document!",
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
                        DocumentName: $('#seqdocname').val().toUpperCase().trim(),
                        DocType: $('#seqdoctype').val().trim(),
                        DocSubType: $('#seqdocsubtype').val().toUpperCase().trim() || "",
                        Status: $('#status').is(':checked'),
                        CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                        CreatedById: '',
                        ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                        ModifiedById: ''
                    }
                    var requestURL = title === 'add' ? '../../../SequenceTable/SaveDocument' : '../../../SequenceTable/UpdateDocument';
                    var type = title === 'add' ? "POST" : "PUT";
                    var forcomplete = {};
                    var displayDocuments = {};
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
                                        $("#tableList tbody tr").remove();
                                        LoadDocuments();
                                        clearDocument();
                                        $('#MyModalDocument').modal({ backdrop: 'static', keyboard: false });
                                    }
                                }
                                else {
                                    $('#MyModalDocument').modal({ backdrop: 'static', keyboard: false });
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
                                    $('#MyModalDocument').modal({ backdrop: 'static', keyboard: false });
                                }
                            });
                        }
                    });
                }
                else {
                    $('#MyModalDocument').modal({ backdrop: 'static', keyboard: false });
                }
            });
        };
    }
});
$('#seqdocument').change(function () {
    var objectcode = $(this).val().split('|')[0];
    var requestURL = '../../../SequenceTable/GetSequenceInfo';
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
            $("#defaultseries").empty().trigger('chosen:updated');;
            $("#defaultseries").append($('<option/>')).trigger('chosen:updated');
            $.map(forcomplete, function (val) {
                if (val.ObjectCode == Number(objectcode)) {
                    $.map(val.Lines, function (val2) {
                        $("#defaultseries").append($('<option/>').val(val2.Series).text(val2.SeriesName)).trigger('chosen:updated');
                    });
                }
            });
        }
    });



    //$('#doctype').val(doc[1]).trigger("chosen:updated");
    //if (doc[0] === "PRICELIST") {
    //    $('#branchcode').prop('disabled', true).trigger("chosen:updated");
    //}
    //else {
    //    $('#branchcode').prop('disabled', false).trigger("chosen:updated");
    //}
});
$('#sequencelines').click(function () {
    var isAllValid = true;
    var SequenceLines = [];
    $('#tableListSequenceLines tbody tr').each(function (index, ele) {
        if ($('#linenum-' + index).val() != "" && $('#initialnum-' + index).val() != "" &&
            $('#seriesname-' + index).val() != "") {
            var date = new Date();
            var item = {
                Id: lastSelectedIds[0],
                ObjectCode: objectcode[0],
                Series: $('#series-' + index).val() || 0,
                BranchCode: $('#branchcode-' + index).val(),
                SeriesName: $('#seriesname-' + index).val().toUpperCase(),
                InitialNum: $('#initialnum-' + index).val().toUpperCase(),
                NextNumber: $('#nextnum-' + index).val(),
                LastNum: $('#lastnum-' + index).val(),
                BeginStr: $('#beginstr-' + index).val().toUpperCase() || "",
                LastStr: $('#laststr-' + index).val().toUpperCase() || "",
                Remarks: $('#remarks-' + index).val().toUpperCase() || "",
                NumSize: $('#numsize-' + index).val(),
                Indicator: $('#indicator-' + index).is(":checked"),
                Locked: $('#linestatus-' + index).is(":checked"),
                DocSubType: docsubtype[0],
                CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                CreatedById: '',
                ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                ModifiedById: ''
            }
            SequenceLines.push(item);
        }
    });

    if (SequenceLines.length === 0) {
        $('.custom-alert').fadeIn();
        toastr.warning('Sequence lines is required.', '');
        isAllValid = false;
    }
    if (isAllValid) {
        $("#MyModalSequenceLines").modal('hide');
        var canceltext = 'updating';
        var swaltext = 'Updating Lines...';
        var messagetext = "Error updating Lines!";
        swal({
            title: "Are you sure?",
            text: "You want to update this lines?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, update this lines!",
            cancelButtonText: "No, cancel updating!"
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
                    Id: lastSelectedIds[0],
                    DocSubType: docsubtype[0],
                    DefaultSeries: 0,
                    ObjectCode: objectcode[0],
                    Status: true,
                    Lines: SequenceLines,
                    CreatedOn: moment(date),//.format("YYYY-MM-DD"),
                    CreatedById: '',
                    ModifiedOn: moment(date),//.format("YYYY-MM-DD"),
                    ModifiedById: ''
                }
                var requestURL = '../../../SequenceTable/Update';
                var type = "PUT";
                var forcomplete = {};
                $.ajax({
                    type: type,
                    contentType: "application/json; charset=utf-8",
                    url: requestURL,
                    data: JSON.stringify(data),
                    dataType: "json",
                    success: function (data) {
                        forcomplete = data;
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
                                    var url = '../../../SequenceTable/Index';
                                    window.location.href = url;
                                    //clearAll();
                                    //$('#MyModalSequenceLines').modal({ backdrop: 'static', keyboard: false });
                                }
                            }
                            else {
                                $('#MyModalSequenceLines').modal({ backdrop: 'static', keyboard: false });
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
                                $('#MyModalSequenceLines').modal({ backdrop: 'static', keyboard: false });
                            }
                        });
                    }
                });
            }
            else {
                $('#MyModalSequenceLines').modal({ backdrop: 'static', keyboard: false });
            }
        });
    }
});
$('#lineclose').click(function () {
    $('#MyModalSequenceLines').modal('hide');
});
$('#seqclose').click(function () {
    $('#MyModalSequence').modal('hide');
});
function LoadDocuments() {
    requestURL = '../../../SequenceTable/GetSequenceDocument';
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: requestURL,
        dataType: "json",
        success: function (data) {
            displayDocuments = data;
        },
        complete: function () {
            $('#tableList tbody tr').remove();
            $.each(displayDocuments, function (i, item) {
                var $serial = $('#tableList');
                $serial.append(
                    '<tr>' +
                    //'<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + i + '" onclick="removeItem(this)"><span class="fa fa-remove"></span></a></td>' +
                    '</td><td><input type="hidden" id="av2-' + i + '" value=' + item.DocumentName + '>' + item.DocumentName +
                    '</td><td><input type="hidden" id="av3-' + i + '" value=' + item.DocType + '>' + item.DocType +
                    '</td><td><input type="hidden" id="av3-' + i + '" value=' + (item.DocSubType != null ? item.DocSubType : "") + '>' + (item.DocSubType != null ? item.DocSubType : "") +
                    '</td><td><input type="hidden" id="av4-' + i + '" value=' + item.Status + '>' + ViewDocStatus(item.Status) +
                    '</td></tr>');
            });
        }
    });
}
$('#clear').click(function () {
    clearDocument();
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
function ViewDocStatus(status) {
    var status = status === true ? "ACTVE" : "IN_ACTIVE";
    if (status === "ACTVE") {
        return '<span class="label label-primary">' + status + '</span>';
    } else if (status === "IN_ACTIVE") {
        return '<span class="label label-danger">' + status + '</span>';
    }
};
function clearAll() {
    $('#seqid').val('');
    $('#seqdocument').val('').trigger('chosen:updated');
    $('#doctype').val('').trigger('chosen:updated');
    $('#seqseriesname').val('');
    $('#seqprefix').val('');
    $('#seqsuffix').val('');
    $('#seqinitialnum').val('');
    $('#seqnextnum').val('');
    $('#seqlastnum').val('');
    $('#branchcode').val('').trigger('chosen:updated');
}
function clearDocument() {
    $('#seqdoc').val('');
    $('#seqdocname').val('');
    $('#seqdoctype').val('').trigger("chosen:updated");

}
function initButtonEvent() {
    $('.edit_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        documentname = $(this).attr('id').split('_')[2] || "";
        docsubtype = $(this).attr('id').split('_')[3] || "";
        objectcode = $(this).attr('id').split('_')[4] || 0;
        doctype = $(this).attr('id').split('_')[5] || "";
        defaultseries = $(this).attr('id').split('_')[6] || 0;
        var localobj = objectcode;
        if (lastSelectedIds != null) {
            $('#seqid').val(lastSelectedIds);
            $('#seqdocument').val(objectcode + "|" + doctype + "|" + docsubtype).trigger('chosen:updated');
            var objectcode = $(this).val().split('|')[0];
            var requestURL = '../../../SequenceTable/GetSequenceInfo';
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
                    $("#defaultseries").empty().trigger('chosen:updated');;
                    $("#defaultseries").append($('<option/>')).trigger('chosen:updated');
                    $.map(forcomplete, function (val) {
                        if (val.ObjectCode == Number(localobj)) {
                            $.map(val.Lines, function (val2) {
                                $("#defaultseries").append($('<option/>').val(val2.Series).text(val2.SeriesName)).trigger('chosen:updated');
                            });
                        }
                    });
                    $('#defaultseries').val(defaultseries).trigger("chosen:updated");
                    $('#ModalTitle').html('Edit Sequence');
                    $('#sequence').text('Update Sequence').removeClass('btn btn-success').addClass('btn btn-primary');
                    $('#MyModalSequence').modal({ backdrop: 'static', keyboard: false });
                }
            });
        } else {
            $('.custom-alert').fadeIn();
            toastr.warning('Please select row to edit.', 'System Warning');
        }
    });
    $('.cancel_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        documentname = $(this).attr('id').split('_')[2] || "";
        docsubtype = $(this).attr('id').split('_')[3] || "";
        objectcode = $(this).attr('id').split('_')[4] || 0;
        doctype = $(this).attr('id').split('_')[5] || "";
        defaultseries = $(this).attr('id').split('_')[6] || 0;
        var localobj = objectcode;
        var id = lastSelectedIds;
        if (lastSelectedIds != null) {
            swal({
                title: "Are you sure?",
                text: "You want to remove this Sequence?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, remove sequence!",
                cancelButtonText: "No, cancel remove!"
            }).then((result) => {
                if (result.value) {
                    swal({
                        title: 'Loading... Please Wait!',
                        text: 'Removing Sequence...',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    });
                    var requestURL = '../../../SequenceTable/Delete';
                    var forcomplete = {};
                    $.ajax({
                        type: "DELETE",
                        url: requestURL,
                        data: { "id": id },
                        success: function (data2) {
                            forcomplete = data2;
                        },
                        complete: function () {
                            swal.hideLoading();
                            swal({
                                type: forcomplete.HttpStatus === 200 ? 'success' : 'warning',
                                title: forcomplete.HttpStatus === 200 ? "Successful" : "Error removing sequence!",
                                text: forcomplete.Message,
                                allowOutsideClick: false,
                                allowEscapeKey: false
                            }).then((result) => {
                                if (forcomplete.HttpStatus === 200) {
                                    if (result.value) {
                                        var url = '../../../SequenceTable/Index';
                                        window.location.href = url;
                                    }
                                }
                            });
                        },
                        error: function (data3) {
                            swal.hideLoading();
                            swal({
                                type: 'warning',
                                title: "Error removing sequence!",
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
        documentname = $(this).attr('id').split('_')[2] || "";
        docsubtype = $(this).attr('id').split('_')[3] || "";
        objectcode = $(this).attr('id').split('_')[4] || 0;
        doctype = $(this).attr('id').split('_')[5] || "";
        defaultseries = $(this).attr('id').split('_')[6] || 0;
        var localobj = objectcode;
        if (lastSelectedIds != null) {
            disablefield('edit', true);
            $('#seqid').val(lastSelectedIds);
            $('#seqdocument').val(objectcode + "|" + doctype + "|" + docsubtype).trigger('chosen:updated');
            var objectcode = $(this).val().split('|')[0];
            var requestURL = '../../../SequenceTable/GetSequenceInfo';
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
                    $("#defaultseries").empty().trigger('chosen:updated');;
                    $("#defaultseries").append($('<option/>')).trigger('chosen:updated');
                    $.map(forcomplete, function (val) {
                        if (val.ObjectCode == Number(localobj)) {
                            $.map(val.Lines, function (val2) {
                                $("#defaultseries").append($('<option/>').val(val2.Series).text(val2.SeriesName)).trigger('chosen:updated');
                            });
                        }
                    });
                    $('#defaultseries').val(defaultseries).trigger("chosen:updated");
                    $('#ModalTitle').html('Sequence Details');
                    $('#sequence').text('Edit Sequence').addClass('btn btn-success');
                    $('#MyModalSequence').modal({ backdrop: 'static', keyboard: false });
                }
            });


        }
    });
    $('.view_child_doc').off().on('click', function () {
        lastSelectedIds = $(this).attr('id').split('_')[1];
        documentname = $(this).attr('id').split('_')[2] || "";
        docsubtype = $(this).attr('id').split('_')[3] || "";
        objectcode = $(this).attr('id').split('_')[4] || 0;
        doctype = $(this).attr('id').split('_')[5] || "";
        defaultseries = $(this).attr('id').split('_')[6] || 0;
        var objectcode = $(this).val().split('|')[4];
        if (lastSelectedIds != null) {
            $('#tableListSequenceLines tbody tr').remove();
            ItemRow = 0;
            var requestURL = '../../../SequenceTable/GetSequenceTable';
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
                    $.each(forcomplete.Lines, function (i, item) {
                        var $item = $('#tableListSequenceLines tbody');
                        $item.append(
                            '<tr>' +
                            '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + ItemRow + '" onclick="RemoveItemRow(this)"><span class="fa fa-remove"></span></a>&nbsp;&nbsp;' +
                            '<a href="javascript:void(0)" class="btn btn-primary btn-sm" id="setdefault-' + ItemRow + '" onclick="SetDefault(getRowID(this))"><span class="fa fa-user"></span></a></td> ' +
                            '<td><input type="text"  class="form-control" id="linenum-' + ItemRow + '" value="' + (ItemRow + 1) + '" disabled/></td>' +
                            '<td><select id="branchcode-' + ItemRow + '" class="form-control chosen-select"></select></td>' +
                            '<td><input type="text"  class="form-control  text-uppercase" id="seriesname-' + ItemRow + '" value="' + item.SeriesName + '" /><input type="hidden" id="series-' + ItemRow + '" value="' + item.Series + '"/></td>' +
                            '<td><input type="text"  class="form-control" id="initialnum-' + ItemRow + '" value="' + item.InitialNum + '"  onkeyup="NextNoOnKeyUp(getRowID(this))" onkeypress="return isNumberKey(event)"/></td>' +
                            '<td><input type="text"  class="form-control" id="nextnum-' + ItemRow + '" value="' + item.NextNumber + '"  onkeypress="return isNumberKey(event)" disabled/></td>' +
                            '<td><input type="text"  class="form-control" id="lastnum-' + ItemRow + '" value="' + item.LastNum + '" onkeypress="return isNumberKey(event)"/></td>' +
                            '<td><input type="text"  class="form-control text-uppercase" id="beginstr-' + ItemRow + '" value="' + item.BeginStr + '"/></td>' +
                            '<td><input type="text"  class="form-control text-uppercase" id="laststr-' + ItemRow + '" value="' + (item.LastStr != null ? item.LastStr : "") + '"/></td>' +
                            '<td><input type="text"  class="form-control text-uppercase" id="remarks-' + ItemRow + '" value="' + (item.Remarks != null ? item.Remarks : "") + '"/></td>' +
                            '<td><input type="text"  class="form-control" id="numsize-' + ItemRow + '" value="' + item.NumSize + '" onkeypress="return isNumberKey(event)"/></td>' +
                            '<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input disabled id = "indicator-' + ItemRow + '" type = "checkbox" > <label for="checkbox8"> </label> </div ></td>' +
                            '<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input id = "linestatus-' + ItemRow + '" type = "checkbox" > <label for="checkbox8"> </label> </div ></td>' +
                            '</tr>');
                        //Check Lock field
                        if (item.Locked) {
                            $('#linestatus-' + ItemRow).prop("checked", true);
                        } else {
                            $('#linestatus-' + ItemRow).prop("checked", false);
                        }
                        //Check Lock field
                        if (item.Indicator) {
                            $('#indicator-' + ItemRow).prop("checked", true);
                        } else {
                            $('#indicator-' + ItemRow).prop("checked", false);
                        }
                        PopulateBranch(ItemRow, item.BranchCode);
                        ItemRow++;
                    });
                    $('#ModalTitleLines').html("Series - " + documentname[0].toLowerCase() + " - Setup");
                    $('#MyModalSequenceLines').modal({ backdrop: 'static', keyboard: false });
                }
            });
        }
    });
}
function disablefield(ptype, pDisable) {
    switch (ptype) {
        case "edit":
        case "add":
            $('#seqdocument').prop('disabled', pDisable).trigger('chosen:updated');
            break;
        default:
            $('#seqdocument').prop('disabled', !pDisable);
            break;
    }
    $('#defaultseries').prop('disabled', pDisable).trigger('chosen:updated');
}
function InputToUpper(obj) {
    if (obj.value != "") {
        obj.value = obj.value.toUpperCase();
    }
}
$('#seqinitialnum').keyup(function () {
    var num = $(this).val();
    var nextnum = num.replace(/.$/, "1")
    $('#seqnextnum').val(nextnum);
    $('#seqlastnum').val(num);
});
function getAndIncrementLastNumber(str) {
    return str.replace(/\d+$/, function (s) {
        return +s + 1;
    });
}
//$('#branchcode').change(function () {
//    $('#seqseriesname').val($(this).val());
//});

// New Update on Document Numbering : MELJUN 2020-03-09
function AddNewItemRow() {
    $item = $('#tableListSequenceLines tbody');
    $item.append(
        '<tr>' +
        '<td><a href="javascript:void(0)" class="btn btn-danger btn-sm" id="removes-' + ItemRow + '" onclick="RemoveItemRow(this)"><span class="fa fa-remove"></span></a>&nbsp;&nbsp;' +
        '<a href="javascript:void(0)" class="btn btn-primary btn-sm" id="setdefault-' + ItemRow + '" onclick="SetDefault(getRowID(this))"><span class="fa fa-user"></span></a></td> ' +
        '<td><input type="text"  class="form-control" id="linenum-' + ItemRow + '" value="' + (ItemRow + 1) + '" disabled/></td>' +
        '<td><select id="branchcode-' + ItemRow + '" class="form-control chosen-select"></select></td>' +
        '<td><input type="text"  class="form-control  text-uppercase" id="seriesname-' + ItemRow + '"  maxlength ="8"  /><input type="hidden" id="series-' + ItemRow + '"/></td>' +
        '<td><input type="text"  class="form-control" id="initialnum-' + ItemRow + '" onkeyup="NextNoOnKeyUp(getRowID(this))" onkeypress="return isNumberKey(event)"/></td>' +
        '<td><input type="text"  class="form-control" id="nextnum-' + ItemRow + '" onkeypress="return isNumberKey(event)" disabled/></td>' +
        '<td><input type="text"  class="form-control" id="lastnum-' + ItemRow + '" onkeypress="return isNumberKey(event)"/></td>' +
        '<td><input type="text"  class="form-control text-uppercase" id="beginstr-' + ItemRow + '"/></td>' +
        '<td><input type="text"  class="form-control text-uppercase" id="laststr-' + ItemRow + '" /></td>' +
        '<td><input type="text"  class="form-control text-uppercase" id="remarks-' + ItemRow + '" /></td>' +
        '<td><input type="text"  class="form-control" id="numsize-' + ItemRow + '" onkeypress="return isNumberKey(event)"/></td>' +
        '<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input disabled id = "indicator-' + ItemRow + '" type = "checkbox" > <label for="checkbox8"> </label> </div ></td>' +
        '<td style="text-align:center"><div class="checkbox checkbox-success checkbox-circle"> <input id = "linestatus-' + ItemRow + '" type = "checkbox" > <label for="checkbox8"> </label> </div ></td>' +
        '</tr>');
    PopulateBranch(ItemRow);
    ItemRow++;
}
function PopulateBranch(row, branch) {
    var requestURL = '../../../Branch/GetBranchInfo';
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
            $("#branchcode-" + row).empty().trigger('chosen:updated');;
            $("#branchcode-" + row).append($('<option/>')).trigger('chosen:updated');
            $.each(forcomplete, function (i, val) {
                $("#branchcode-" + row).append($('<option/>').val(val.Code).text(val.Name)).trigger('chosen:updated');
            });
            if (branch != null) {
                $('#branchcode-' + row).val(branch).trigger("chosen:updated");
            }
            $('.chosen-select').chosen({ width: "100%" });
        }
    });
}
function NextNoOnKeyUp(row) {
    $('#nextnum-' + row).val($('#initialnum-' + row).val());
}
function getRowID(obj) {
    return $(obj).closest('tr').index();
}
function RemoveItemRow(obj) {
    $(obj).parent().parent().remove();
    updateIds();
}
function updateIds() {
    $('#tableListSequenceLines').find('tbody > tr').each(function (index) {
        $(this).find('[id^=removes]').attr('id', 'removes-' + index);
        $(this).find('[id^=setdefault]').attr('id', 'setdefault-' + index);
        $(this).find('[id^=linenum]').attr('id', 'linenum-' + index);
        $(this).find('[id^=linenum]').val(index + 1);
        $(this).find('[id^=seriesname]').attr('id', 'seriesname-' + index);
        $(this).find('[id^=initialnum]').attr('id', 'initialnum-' + index);
        $(this).find('[id^=nextnum]').attr('id', 'nextnum-' + index);
        $(this).find('[id^=lastnum]').attr('id', 'lastnum-' + index);
        $(this).find('[id^=beginstr]').attr('id', 'beginstr-' + index);
        $(this).find('[id^=laststr]').attr('id', 'laststr-' + index);
        $(this).find('[id^=remarks]').attr('id', 'remarks-' + index);
        $(this).find('[id^=numsize]').attr('id', 'numsize-' + index);
        $(this).find('[id^=indicator]').attr('id', 'indicator-' + index);
        $(this).find('[id^=linestatus]').attr('id', 'linestatus-' + index);
    });
}
function SetDefault(row) {
    $('#tableListSequenceLines').find('tbody > tr').each(function (index) {
        if (row == index && $(this).find('[id^=indicator]').is(':checked') === false) {
            $(this).find('[id^=indicator]').prop('checked', true);
            //$('#indicator-' + row).prop('checked', true);
        } else {
            $(this).find('[id^=indicator]').prop('checked', false);
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

//>>end