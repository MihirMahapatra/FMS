$(function () {
    const email = $('input[name = "Email"]');
    const password = $('input[name = "Password"]');
    const financialYear = $('select[name="FinancialYearId"]');
    $('#btnSignin').on('click', function (event) {
        event.preventDefault();
        if (!email.val()) {
            toastr.error('Email Is Required.');
            return;
        }
        if (!password.val()) {
            toastr.error('Password Is Required.');
            return;
        }
        $('#LoginForm').submit();
    });
});