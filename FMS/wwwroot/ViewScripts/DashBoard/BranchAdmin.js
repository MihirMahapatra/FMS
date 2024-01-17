
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
            var defaultOption = $('<option></option>').val('').text('--Select Option--');
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
    var BranchId = $(this).val();
    $.ajax({
        url: '/DashBoard/GetFinancialYears?BranchId=' + BranchId + '',
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            if (result.ResponseCode == 302) {
                ddlFinancialYear.empty();
                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                ddlFinancialYear.append(defaultOption);
                if (BranchId === 'All') {
                    $.each(result.FinancialYears, function (key, item) {
                        var option = $('<option></option>').val(item.FinancialYearId).text(item.Financial_Year);
                        ddlFinancialYear.append(option);
                    });
                }
                else {
                    $.each(result.BranchFinancialYears, function (key, item) {
                        var option = $('<option></option>').val(item.Fk_FinancialYearId).text(item.FinancialYear.Financial_Year);
                        ddlFinancialYear.append(option);
                    });
                }
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
