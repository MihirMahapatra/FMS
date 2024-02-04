$(function () {
    //*****************Declaration Section*******************//
    const TokenValueInput = $('input[name="TokenValue"]');
    const FkTokenIdInput = $('input[name="FkTokenId"]');
    const nameInput = $('input[name="Name"]');
    const emailInput = $('input[name="Email"]');
    const phoneInput = $('input[name="PhoneNumber"]');
    const passwordInput = $('input[name="Password"]');
    const confirmPasswordInput = $('input[name="ConformPassword"]');
    const PhotoInput = $('input[name="ProfilePhoto"]');
    //Disable All Input
    nameInput.prop('disabled', true)
    emailInput.prop('disabled', true)
    phoneInput.prop('disabled', true)
    passwordInput.prop('disabled', true)
    confirmPasswordInput.prop('disabled', true)
    PhotoInput.prop('disabled', true)
    $('#btnSubmit').prop('disabled', true);
    /*******************************Validation*************************************/
    $('#btnValidate').on('click', function () {
        $.ajax({
            url: '/Account/Token',
            type: 'Post',
            dataType: 'json',
            data: { TokenValue: TokenValueInput.val() },
            success: function (result) {

                if (result.ResponseCode == "302") {
                    FkTokenIdInput.val(result.TokenId);
                    nameInput.prop('disabled', false)
                    emailInput.prop('disabled', false)
                    phoneInput.prop('disabled', false)
                    passwordInput.prop('disabled', false)
                    confirmPasswordInput.prop('disabled', false)
                    PhotoInput.prop('disabled', false)
                    $('#btnSubmit').prop('disabled', false);
                    toastr.success(result.SuccessMsg);
                }
                else {
                    toastr.error(result.ErrorMsg);
                }
            },
            error: function () {
                alert('Error!');
            }
        });
    });
    nameInput.on('keydown', function (event) {
        const keyCode = event.keyCode || event.which;
        if (keyCode >= 48 && keyCode <= 57 || (keyCode >= 96 && keyCode <= 111)) {
            event.preventDefault();
        }

    });
    emailInput.on('blur', function (event) {
        const Regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        const isValid = Regex.test(emailInput.val());
        if (!isValid) {
            toastr.error("Invalid Email");
        }
    });
    phoneInput.on('keydown', function (event) {

        // Allow: backspace, delete, tab, escape, and enter
        if (event.key === "Backspace" || event.key === "Delete" || event.key === "Tab" || event.key === "Escape" || event.key === "Enter") {
            return;
        }
        if (event.keyCode < 48 || event.keyCode > 57) {

            event.preventDefault();
        }
    });
    phoneInput.on('blur', function () {
        const Regex = /^\d{10}$/;
        const isValid = Regex.test(phoneInput.val());
        if (!isValid) {
            toastr.error("Phone Number Must Be 10 Digit Long");
        }
    });
    passwordInput.on('blur', function () {
        const Regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$/;
        const isValid = Regex.test(passwordInput.val());
        if (!isValid) {
            toastr.error("Password must contain:<br>&#8226; <b>At least one lowercase letter [a-z].</b><br>&#8226;<b>At least one uppercase letter.[A-Z]</b><br>&#8226;<b>At least one digit. [0-9]</b><br>&#8226;<b>At least one special character.[$,@,# etc.]</b><br>&#8226;<b>At least 8 character long</b>.<br>&#8226;<b>Example : User@1234</b>");
        }
    });
    confirmPasswordInput.on('blur', function () {
        if (confirmPasswordInput.val() === passwordInput.val()) {

        }
        else {
            alert('The password And Conform Password do not match');
        }
    });
})