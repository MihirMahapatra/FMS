$(function () {
    //----------------------------------------varible declaration-----------------------------------------//
    var CompanyName = $('input[name="name"]');
    var Phone = $('input[name="Phone"]');
    var Email = $('input[name="Email"]');
    var State = $('input[name="State"]');
    var GSTIN = $('input[name="GSTIN"]');
    var address = $('input[name="Address"]');
    var ddlBranch = $('select[name="ddlBranch"]');
    //-------------------------------------------screen-----------------------------------------------//
    loadBranch();
    function loadBranch() {
        $.ajax({
            url: "/Admin/GetAllBranch",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlBranch.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlBranch.append(defaultOption);
                    $.each(result.Branches, function (key, item) {
                        var option = $('<option></option>').val(item.BranchId).text(item.BranchName);
                        ddlBranch.append(option);
                    });
                }
                else {                  
                    ddlBranch.empty();
                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlBranch.append(defaultOption);
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