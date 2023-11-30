const ddlFinancialYear = $('select[name="FinancialYearId"]');
const ddlBranch = $('select[name="BranchId"]');
Branches();
function Branches() {
    $.ajax({
        url: "/DashBoard/GetAllBranch",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",

        success: function (result) {
            ddlBranch.empty();
            var defaultOption  = $('<option></option>').val('').text('--Select Option--');
            ddlBranch.append(defaultOption);
            var addAllBranch = $('<option></option>').val('All').text('All');
            ddlBranch.append(addAllBranch);
            $.each(result.Branches, function (key, item) {
                var option = $('<option></option>').val(item.BranchId).text(item.BranchName);
                ddlBranch.append(option);
            });
        },
        error: function (errormessage) {
            console.log(errormessage)
        }
    });
}
$("#BranchId").on("change", function () {
    var id = ddlBranch.val();
    if (id === 'All') {
        $.ajax({
            url: '/DashBoard/GetFinancialYears?BranchId=' + id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlFinancialYear.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlFinancialYear.append(defaultOption);
                    $.each(result.FinancialYears, function (key, item) {
                        var option = $('<option></option>').val(item.Financial_Year).text(item.Financial_Year);
                        ddlFinancialYear.append(option);
                    });
                }
                else {
                    ddlFinancialYear.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlFinancialYear.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    else
        $.ajax({
            url: '/DashBoard/GetFinancialYears?BranchId=' + id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlFinancialYear.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlFinancialYear.append(defaultOption);
                    $.each(result.FinancialYears, function (key, item) {
                        var option = $('<option></option>').val(item.FinancialYearId).text(item.Financial_Year);
                        ddlFinancialYear.append(option);
                    });
                }
                else {
                    ddlFinancialYear.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlFinancialYear.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
})