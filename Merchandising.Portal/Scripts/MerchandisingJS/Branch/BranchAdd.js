
function validatetoemail(my_email) {
    var filter = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
    if (filter.test(my_email)) {
        return true;
    }
    else {
        return false;
    }
}
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}
$(document).ready(function () {
    $('.chosen-select').chosen({ width: "100%" });
});
$('#submit').click(function () {
    var isAllValid = true;

    if ($('#userid').val().trim() === '') {
        $('.custom-alert').fadeIn();
        toastr.warning('User ID is required.', 'System Warning');
        isAllValid = false;
    }


    if ($('#username').val().trim() === '') {

        $('.custom-alert').fadeIn();
        toastr.warning('User Name is required.', 'System Warning');
        isAllValid = false;
    }

    if ($('#password').val().trim() === '') {

        $('.custom-alert').fadeIn();
        toastr.warning('Password is required.', 'System Warning');
        isAllValid = false;
    }

    if ($('#password').val().length < 4) {
        $('.custom-alert').fadeIn();
        toastr.warning('Password must be at least 4 characters.', 'System Warning');

        isAllValid = false;
    }

    if ($('#email').val().trim() != '') {
        if (validatetoemail($('#email').val().trim())) {
        }
        else {

            $('.custom-alert').fadeIn();
            toastr.warning('Email is not valid email address.', 'System Warning');

            isAllValid = false;
        }
    }

    if ($('#email').val().trim() === '') {
        $('.custom-alert').fadeIn();
        toastr.warning('Email is required.', 'System Warning');
        isAllValid = false;
    }
    //if ($('#branch').val().trim() === '') {

    //    $('.custom-alert').fadeIn();
    //    toastr.warning('Branch is required.', 'System Warning');

    //    isAllValid = false;
    //}

    if ($('#role').val() === '') {

        $('.custom-alert').fadeIn();
        toastr.warning('Role is required.', 'System Warning');

        isAllValid = false;
    }
    if (isAllValid) {
        swal({
            title: "Are you sure?",
            text: "You want to add User!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, add user!",
            cancelButtonText: "No, cancel adding!"
        }).then((result) => {
            if (result.value) {
                swal({
                    title: 'Loading... Please Wait!',
                    text: 'Adding User...',
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    onOpen: () => {
                        swal.showLoading();
                    }
                });
                var data = {
                    UserId: $('#userid').val().trim(),
                    UserName: $('#username').val().trim(),
                    Password: $('#password').val().trim(),
                    Role: $('#role').val().trim(),
                    BranchCode: $('#branch').val().trim(),
                    ContactNo: $('#contact').val().trim(),
                    Email: $('#email').val().trim(),
                    Status: $('#status').val().trim(),
                    LastAccess: ''
                }
                var requestURL = '../../../Users/Save';
                var forcomplete = {};
                $.ajax({
                    type: "POST",
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
                            title: forcomplete.HttpStatus === 200 ? "Successful" : "Error adding user!",
                            text: forcomplete.Message,
                            allowOutsideClick: false,
                            allowEscapeKey: false
                        }).then((result) => {
                            if (forcomplete.HttpStatus === 200) {
                                if (result.value) {
                                    var url = '@Url.Action("Index")';
                                    window.location.href = url;
                                }
                            }
                        });
                    },
                    error: function (data3) {
                        swal.hideLoading();
                        swal({
                            type: 'warning',
                            title: "Error adding user!",
                            text: data3.Message,
                            allowOutsideClick: false,
                            allowEscapeKey: false
                        });
                    }
                });
            }
        });
    };
}); //end