$(function () {
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#AllBranchCustomerReportLink").addClass("active");
    $("#AllBranchCustomerReportLink").addClass("active");
    $("#AllBranchCustomerReportLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const FromDateSupplyer = $('input[name="FromDateSupplyer"]');
    FromDateSupplyer.val(todayDate);
    const ToDateSupplyer = $('input[name="ToDateSupplyer"]');
    ToDateSupplyer.val(todayDate);
    const ddlCustomer = $('select[name="ddlCustomerId"]');
    const ddlSupplyer = $('select[name="ddlSupplyerId"]');
    const ddlPartyGroup = $('select[name="ddlPartyGroupId"]');
    const ddlPartyGroupDetailed = $('select[name="ddlPartyGroupIddetailed"]');
    const FromDateCustomer = $('input[name="FromDateCustomer"]');
    FromDateCustomer.val(todayDate);
    const ToDateCustomer = $('input[name="ToDateCustomer"]');
    ToDateCustomer.val(todayDate);
    //--------------------------------Customer Report Summerized------------------------------------------------//
    //--------------------------------Customer Report Detailed------------------------------------------------//

    LoadPartyGroupsDetailed();
    function LoadPartyGroupsDetailed() {
        $.ajax({
            url: "/Master/GetPartyGruops",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                ddlPartyGroupDetailed.empty();
                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                ddlPartyGroupDetailed.append(defaultOption);
                $.each(result.PartyGruops, function (key, item) {
                    var option = $('<option></option>').val(item.PartyGroupId).text(item.PartyGruopName);
                    ddlPartyGroupDetailed.append(option);
                });
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    ddlPartyGroupDetailed.on('select2:open', function (e) {
        // Focus on the search box when the dropdown is opened
        $('.select2-search__field').focus();
    });
    ddlPartyGroupDetailed.on('change', function () {
        var fkpartygroupId = $(this).val();
        $.ajax({
            url: "/Transaction/GetSundryDebtors",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlCustomer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlCustomer.append(defaultOption);
                    $.each(result.SubLedgers, function (key, item) {
                        if (item.PartyGroupId == fkpartygroupId) {
                            var option = $('<option></option>').val(item.SubLedgerId).text(item.SubLedgerName);
                            ddlCustomer.append(option);
                        }
                    });
                }
                else {
                    ddlCustomer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlCustomer.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    })
    var data = {}
    var PrintDataDetailed = {};
    $('#btnViewCustomerDetailed').on('click', function () {
        $('#loader').show();
        $('.DetailedReportTable').empty();
        if (!ddlCustomer.val() || ddlCustomer.val() === '--Select Option--') {
            toastr.error('Customer Name Is Required.');
            return;
        }
        else if (!FromDateCustomer.val()) {
            toastr.error('FromDate Is Required.');
            return;
        } else if (!ToDateCustomer.val()) {
            toastr.error('ToDate Type Is Required.');
            return;
        }
        else {
            requestData = {
                FromDate: FromDateCustomer.val(),
                ToDate: ToDateCustomer.val(),
                PartyId: ddlCustomer.val()
            };
            data = {
                FromDate: FromDateCustomer.val(),
                ToDate: ToDateCustomer.val(),
                PartyId: ddlCustomer.val()
            };
            $.ajax({
                url: "/Reports/GetDetailedCustomerReportForAll",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 DetailedReportTable" style="width:100%">';
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        if (result.PartyDetailed !== null) {
                            var balance =
                                html += '<tr>';
                            html += '<td colspan="8">Opening Bal.</td>';
                            html += '<td >' + result.PartyDetailed.OpeningBal + ' ' + result.PartyDetailed.OpeningBalType + '</td>';
                            html += '</tr >';
                            var balance = result.PartyDetailed.OpeningBal;
                            if (result.PartyDetailed.Orders.length > 0) {
                                html += '<tr class="bg-primary">';
                                html += '<td >Trxn Date </td>';
                                html += '<td >Challan No</td>';
                                html += '<td >Site Adress</td>';
                                html += '<td >Branch</td>';
                                html += '<td colspan="5">Details</td>';
                                html += '<td></td>';
                                html += '</tr >';
                                $.each(result.PartyDetailed.Orders, function (key, item) {
                                    const ModifyedDate = item.TransactionDate;
                                    var formattedDate;
                                    if (ModifyedDate) {
                                        const dateObject = new Date(ModifyedDate);
                                        if (!isNaN(dateObject)) {
                                            const day = String(dateObject.getDate()).padStart(2, '0');
                                            const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                                            const year = dateObject.getFullYear();
                                            formattedDate = `${day}/${month}/${year}`;
                                        }
                                    }
                                    html += '<tr>';
                                    html += '<td>' + formattedDate + '</td>';
                                    html += '<td>' + item.ChallanNo + '</td>';
                                    html += '<td>' + item.SiteAdress + '</td>';
                                    html += '<td>' + item.BranchName + '</td>';
                                    html += '<td colspan="4">' + "----" + '</td>';
                                    html += '<td>-</td>';
                                    html += '</tr >';
                                    if (item.Transactions.length > 0) {
                                        html += '<tr>';
                                        html += '<td >-</td>';
                                        html += '<td >-</td>';
                                        html += '<td >-</td>';
                                        html += '<td >Product</td>';
                                        html += '<td >Alternate Qty</td>';
                                        html += '<td >Alternate Unit</td>';
                                        html += '<td >Qty</td>';
                                        html += '<td >Unit</td>';
                                        html += '<td >Rate</td>';
                                        html += '<td >Amount</td>';
                                        html += '<td >-</td>';
                                        html += '</tr >';
                                        $.each(item.Transactions, function (key, Transaction) {
                                            html += '<tr>';
                                            html += '<td >-</td>';
                                            html += '<td >-</td>';
                                            html += '<td >-</td>';
                                            html += '<td >' + Transaction.ProductName + '</td>';
                                            if (Transaction.AlternateQuantity != null) {
                                                html += '<td >' + Transaction.AlternateQuantity + '</td>';
                                            } else {
                                                html += '<td >-</td>';
                                            }
                                            if (Transaction.AlternateUnit != null) {
                                                html += '<td >' + Transaction.AlternateUnit + '</td>';
                                            } else {
                                                html += '<td >-</td>';
                                            }
                                            html += '<td >' + Transaction.Quantity + '</td>';
                                            html += '<td >' + Transaction.Unit + '</td>';

                                            html += '<td >' + Transaction.Rate + '</td>';
                                            html += '<td >' + Transaction.Amount + '</td>';
                                            html += '<td >-</td>';
                                            html += '</tr >';
                                        });
                                    }
                                    html += '<tr>';
                                    html += '<td >-</td>';
                                    html += '<td >-</td>';
                                    html += '<td >-</td>';
                                    html += '<td colspan="4">Grand Total</td>';
                                    html += '<td>' + item.GrandTotal + '</td>';
                                    balance += item.DrCr == "Dr" ? item.GrandTotal : -item.GrandTotal;
                                    var DrCr = balance > 0 ? "Dr" : "Cr";
                                    html += '<td>' + balance.toFixed(2) + ' ' + DrCr + '</td>';
                                    html += '</tr >';
                                });
                            }
                        }
                        $('#BtnPrintDetailed').show();
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="8">No Record</td>';
                        html += '</tr>';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblCustomerDetailedList').html(html);
                },
                error: function (errormessage) {
                    $('#loader').hide();
                    Swal.fire(
                        'Error!',
                        'An error occurred',
                        'error'
                    );
                }
            });
        }
    })
    $('#BtnPrintDetailed').on('click', function () {
        var queryString = $.param(data); // Serialize object to query string
        var url = '/Print/CustomerDetailedReportPrint?' + queryString; // Append query string to URL
        window.open(url, '_blank');
    });
    //----------------------------Suppleyer Report Detailed --------------------------------------------------//
    GetSundryCreditors();
    function GetSundryCreditors() {
        $.ajax({
            url: "/Transaction/GetSundryCreditors",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlSupplyer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlSupplyer.append(defaultOption);
                    $.each(result.SubLedgers, function (key, item) {
                        var option = $('<option></option>').val(item.SubLedgerId).text(item.SubLedgerName);
                        ddlSupplyer.append(option);
                    });
                }
                else {
                    ddlSupplyer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlSupplyer.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    var Data = {};
    $('#btnViewSupplyerDetailed').on('click', function () {
        $('#loader').show();
        $('.SupplyerDetailedReportTable').empty();
        if (!ddlSupplyer.val() || ddlSupplyer.val() === '--Select Option--') {
            toastr.error('Supplyer Name Is Required.');
            return;
        }
        else if (!ToDateSupplyer.val()) {
            toastr.error('ToDate Is Required.');
            return;
        } else if (!FromDateSupplyer.val()) {
            toastr.error('FromDate Is Required.');
            return;
        }
        else {
            var requestData = {
                FromDate: FromDateSupplyer.val(),
                ToDate: ToDateSupplyer.val(),
                PartyId: ddlSupplyer.val()
            };
            Data = {
                FromDate: FromDateSupplyer.val(),
                ToDate: ToDateSupplyer.val(),
                PartyId: ddlSupplyer.val()
            };
            $.ajax({
                url: "/Reports/GetDetailedSupplyerReportForAll",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 SupplyerDetailedReportTable" style="width:100%">';
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        if (result.PartyDetailed !== null) {
                            var balance =
                                html += '<tr>';
                            html += '<td colspan="7">Opening Bal.</td>';
                            html += '<td >' + result.PartyDetailed.OpeningBal + ' ' + result.PartyDetailed.OpeningBalType + '</td>';
                            html += '</tr >';
                            var balance = result.PartyDetailed.OpeningBal;
                            if (result.PartyDetailed.Orders.length > 0) {
                                html += '<tr class="bg-primary">';
                                html += '<td >Trxn Date </td>';
                                html += '<td >Mat Receive No</td>';
                                html += '<td >Branch</td>';
                                html += '<td colspan="4">Details</td>';
                                html += '<td></td>';
                                html += '</tr >';
                                $.each(result.PartyDetailed.Orders, function (key, item) {
                                    const ModifyedDate = item.TransactionDate;
                                    var formattedDate;
                                    if (ModifyedDate) {
                                        const dateObject = new Date(ModifyedDate);
                                        if (!isNaN(dateObject)) {
                                            const day = String(dateObject.getDate()).padStart(2, '0');
                                            const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                                            const year = dateObject.getFullYear();
                                            formattedDate = `${day}/${month}/${year}`;
                                        }
                                    }
                                    html += '<tr>';
                                    html += '<td >' + formattedDate + '</td>';
                                    html += '<td >' + item.MaterialReceiptNo + '</td>';
                                    html += '<td >' + item.BranchName + '</td>';
                                    html += '<td colspan="4">' + item.Naration + '</td>';
                                    html += '<td>-</td>';
                                    html += '</tr >';
                                    if (item.Transactions.length > 0) {
                                        html += '<tr>';
                                        html += '<td >-</td>';
                                        html += '<td >-</td>';
                                        html += '<td >-</td>';
                                        html += '<td >Product</td>';
                                        html += '<td >Qty</td>';
                                        html += '<td >Alt Qty</td>';
                                        html += '<td >Rate</td>';
                                        html += '<td >Amount</td>';
                                        html += '<td >-</td>';
                                        html += '</tr >';
                                        $.each(item.Transactions, function (key, Transaction) {
                                            html += '<tr>';
                                            html += '<td >-</td>';
                                            html += '<td >-</td>';
                                            html += '<td >-</td>';
                                            html += '<td >' + Transaction.ProductName + '</td>';
                                            html += '<td >' + Transaction.Quantity + '</td>';
                                            html += '<td >' + Transaction.AlternateQuantity + '</td>';
                                            html += '<td >' + Transaction.Rate + '</td>';
                                            html += '<td >' + Transaction.Amount + '</td>';
                                            html += '<td >-</td>';
                                            html += '</tr >';
                                        });
                                    }
                                    html += '<tr>';
                                    html += '<td >-</td>';
                                    html += '<td >-</td>';
                                    html += '<td >-</td>';
                                    html += '<td colspan="3">Grand Total</td>';
                                    html += '<td>' + item.GrandTotal + '</td>';
                                    balance += item.DrCr == "Dr" ? item.GrandTotal : -item.GrandTotal;
                                    var DrCr = balance > 0 ? "Dr" : "Cr";
                                    html += '<td>' + balance.toFixed(2) + ' ' + DrCr + '</td>';
                                    html += '</tr >';
                                });
                            }
                        }
                        $('#BtnPrintDetailed').show();
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="8">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblSupplyerDetailed').html(html);

                },
                error: function (errormessage) {
                    $('#loader').hide();
                    Swal.fire(
                        'Error!',
                        'An error occurred',
                        'error'
                    );
                }
            });
        }

    })
    $('#BtnPrintDetailed').on('click', function () {
        $.ajax({
            type: "POST",
            url: '/Print/SupplyerDetailsPrintData',
            dataType: 'json',
            data: JSON.stringify(Printdatadetails),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                window.open(Response.redirectTo, '_blank');
            },
        });
    });
    $('#BtnPrintDetailed').on('click', function () {
        var queryString = $.param(Data); // Serialize object to query string
        var url = '/Print/SupplyerDetailedReportPrint?' + queryString; // Append query string to URL
        window.open(url, '_blank');
    });
})