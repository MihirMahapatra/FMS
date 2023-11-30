$(function () {
    //----------------------------------------varible declaration-----------------------------------------//
    var CompanyName = $('input[name="Name"]');
    var Phone = $('input[name="ContactNo"]');
    var Email = $('input[name="Email"]');
    var State = $('input[name="State"]');
    var GSTIN = $('input[name="GstNo"]');
    var address = $('textarea[name="Address"]');
    var Branch = $('input[name="Branch"]');
    var logo = $('input[name="logo"]')
    //-------------------------------------------screen-----------------------------------------------//

    loadCompany();
    function loadCompany() {
        $.ajax({
            url: "/Admin/GetCompany",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    CompanyName.val(result.GetCompany.Name);
                    Phone.val(result.GetCompany.Phone);
                    Email.val(result.GetCompany.Email);
                    State.val(result.GetCompany.State);
                    GSTIN.val(result.GetCompany.GSTIN);
                    address.val(result.GetCompany.Adress);
                    Branch.val(result.GetCompany.BranchName);
                    logo.val(result.GetCompany.logo);
                }
                else {
                    CompanyName.val('');
                    Phone.val('');
                    Email.val('');
                    State.val('');
                    GSTIN.val('');
                    address.val('');
                    logo.val('');
                    Branch.val(result.GetCompany.BranchName);
                }
            },
            error: function (errormessage) {
                Swal.fire(
                    'Error!',
                    'An error occurred',
                    'error'
                );
            }
        });
    }

    $('#btnSave').on('click', function () {
        var Data = {
            Name: CompanyName.val(),
            Phone: Phone.val(),
            Email: Email.val(),
            State: State.val(),
            GSTIN: GSTIN.val(),
            Adress: address.val(),
            logo: logo.val(),
            Fk_BranchId: ddlBranch.val(),
        };
        $.ajax({
            type: "POST",
            url: '/Admin/CreateCompany',
            dataType: 'json',
            data: JSON.stringify(Data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                if (Response.ResponseCode == 201) {
                    toastr.success(Response.SuccessMsg);
                    CompanyName.val('');
                    Phone.val('');
                    Email.val('');
                    State.val('');
                    GSTIN.val('');
                    address.val('');
                    logo.val('');
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
});