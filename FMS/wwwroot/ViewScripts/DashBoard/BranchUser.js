const ddlFinancialYear = $('select[name="FinancialYearId"]');
const ddlBranch = $('select[name="BranchId"]');
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
                    $.each(result.BranchFinancialYears, function (key, item) {
                        var option = $('<option></option>').val(item.Fk_FinancialYearId).text(item.FinancialYear.Financial_Year);
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
