$(function () {
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#ReportsLink").addClass("active");
    $("#CustomerReportLink").addClass("active");
    $("#CustomerReportLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const fromDateSummerized = $('input[name="FromDateSummerized"]');
    fromDateSummerized.val(todayDate);
    const toDateSummerized = $('input[name="ToDateSummerized"]');
    toDateSummerized.val(todayDate);
    const ddlZeroValued = $('select[name="ddlZerovalued"]');
    const ddlCustomer = $('select[name="ddlCustomerId"]');
    const TddlCustomer = $('select[name="TddlCustomerId"]');
    const fromDateDetailed = $('input[name="FromDateDetailed"]');
    fromDateDetailed.val(todayDate);
    const toDateDetailed = $('input[name="ToDateDetailed"]');
    toDateDetailed.val(todayDate);
    const TfromDateDetailed = $('input[name="TFromDateDetailed"]');
    TfromDateDetailed.val(todayDate);
    const TtoDateDetailed = $('input[name="TToDateDetailed"]');
    TtoDateDetailed.val(todayDate);
    //--------------------------------Customer Report Summerized------------------------------------------------//
    var PrintData = {}
    $('#btnViewSummerized').on('click', function () {
        $('#loader').show();
        $('.SummerizedReportTable').empty();
        if (!fromDateSummerized.val()) {
            toastr.error('FromDate Is Required.');
            return;
        } else if (!toDateSummerized.val()) {
            toastr.error('ToDate Is Required.');
            return;
        } else {
            var requestData = {
                FromDate: fromDateSummerized.val(),
                ToDate: toDateSummerized.val(),
                ZeroValued: ddlZeroValued.val(),
            };
            $.ajax({
                url: "/Reports/GetSummerizedCustomerReport",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 SummerizedReportTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th>Name</th>'
                    html += '<th>Opn Bal</th>'
                    html += '<th>Opn Type</th>'
                    html += '<th>Billing Amt</th>'
                    html += '<th>Paid Amt</th>'
                    html += '<th>Balance</th>'
                    html += '<th>Type</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $('#BtnPrintSummarized').show();
                        $.each(result.PartyReports, function (key, item) {
                            html += '<tr>';
                            html += '<td>' + item.PartyName + '</td>';
                            html += '<td>' + item.OpeningBal + '</td>';
                            html += '<td>' + item.OpeningBalType + '</td>';
                            html += '<td>' + item.DrAmt + '</td>';
                            html += '<td>' + item.CrAmt + '</td>';
                            var balance = item.Balance + item.OpeningBal;
                            html += '<td>' + Math.abs(balance) + '</td>';
                            html += '<td>' + item.BalanceType + '</td>';
                            html += '<tr>';
                        })
                        PrintData = {
                            FromDate: fromDateSummerized.val(),
                            ToDate: toDateSummerized.val(),
                            PartyReports: result.PartyReports
                        }
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="6">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblSummerizedList').html(html);
                    if (!$.fn.DataTable.isDataTable('.SummerizedReportTable')) {
                        var table = $('.SummerizedReportTable').DataTable({
                            "responsive": true, "lengthChange": false, "autoWidth": false,
                            "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
                        }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');
                    }
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
    $('#BtnPrintSummarized').on('click', function () {
        console.log(PrintData);
        $.ajax({
            type: "POST",
            url: '/Print/CustomerSummarizedPrintData',
            dataType: 'json',
            data: JSON.stringify(PrintData),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                window.open(Response.redirectTo, '_blank');
            },
        });
    });
    //--------------------------------Customer Report Detailed------------------------------------------------//
    GetSundryDebtors();
    function GetSundryDebtors() {
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
                        var option = $('<option></option>').val(item.SubLedgerId).text(item.SubLedgerName);
                        ddlCustomer.append(option);
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
    }
    var PrintDataDetailed = {};
    $('#btnViewDetailed').on('click', function () {
        $('#loader').show();
        $('.DetailedReportTable').empty();
        if (!ddlCustomer.val() || ddlCustomer.val() === '--Select Option--') {
            toastr.error('Customer Name Is Required.');
            return;
        }
        else if (!fromDateDetailed.val()) {
            toastr.error('FromDate Is Required.');
            return;
        } else if (!toDateDetailed.val()) {
            toastr.error('ToDate Type Is Required.');
            return;
        }
        else {
            var requestData = {
                FromDate: fromDateDetailed.val(),
                ToDate: toDateDetailed.val(),
                PartyId: ddlCustomer.val()
            };
            $.ajax({
                url: "/Reports/GetDetailedCustomerReport",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 DetailedReportTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th>Date</th>'
                    html += '<th>Trxn N0</th>'
                    html += '<th>Particular</th>'
                    html += '<th>Billing Amt</th>'
                    html += '<th>Paid Amt</th>'
                    html += '<th>Balance</th>'
                    html += '<th>Type</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $('#BtnPrintDetailed').show();
                        var Balance = result.Party.OpeningBal;
                        html += '<tr>';
                        html += '<td></td>';
                        html += '<td></td>';
                        html += '<td>Opeining Bal</td>';
                        html += '<td></td>';
                        html += '<td></td>';
                        html += '<td>' + result.Party.OpeningBal + '</td>';
                        html += '<td>' + result.Party.OpeningBalType + '</td>';
                        html += '</tr >';
                        $.each(result.Party.SalesOrders, function (key, SalesOrder) {
                            const ModifyDate = SalesOrder.TransactionDate;
                            var formattedDate = '';
                            if (ModifyDate) {
                                const dateObject = new Date(ModifyDate);
                                if (!isNaN(dateObject)) {
                                    const day = String(dateObject.getDate()).padStart(2, '0');
                                    const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                                    const year = dateObject.getFullYear();
                                    formattedDate = `${day}/${month}/${year}`;
                                }
                            }
                            html += '<tr>';
                            html += '<td>' + formattedDate + '</td>';
                            html += '<td>' + SalesOrder.TransactionNo + '</td>';
                            html += '<td>Credit Sale</td>';
                            html += '<td>' + SalesOrder.GrandTotal + '</td>';
                            html += '<td></td>';
                            Balance += SalesOrder.GrandTotal
                            html += '<td>' + Math.abs(Balance) + '</td>';
                            if (Balance > 0) {
                                html += '<td>DR</td>';
                            }
                            else {
                                html += '<td>CR</td>';
                            }

                            html += '</tr >';
                        });
                        $.each(result.Party.SalesReturns, function (key, SalesReturn) {
                            const ModifyDate = SalesReturn.TransactionDate;
                            var formattedDate = '';
                            if (ModifyDate) {
                                const dateObject = new Date(ModifyDate);
                                if (!isNaN(dateObject)) {
                                    const day = String(dateObject.getDate()).padStart(2, '0');
                                    const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                                    const year = dateObject.getFullYear();
                                    formattedDate = `${day}/${month}/${year}`;
                                }
                            }
                            html += '<tr>';
                            html += '<td>' + formattedDate + '</td>';
                            html += '<td>' + SalesReturn.TransactionNo + '</td>';
                            html += '<td>Sales Return</td>';
                            html += '<td>' + SalesReturn.GrandTotal + '</td>';
                            html += '<td></td>';
                            Balance -= SalesReturn.GrandTotal
                            html += '<td>' + Math.abs(Balance) + '</td>';
                            if (Balance > 0) {
                                html += '<td>DR</td>';
                            }
                            else {
                                html += '<td>CR</td>';
                            }

                            html += '</tr >';
                        });
                        $.each(result.Party.Receipts, function (key, Recipt) {
                            const ModifyDate = Recipt.VoucherDate;
                            var formattedDate = '';
                            if (ModifyDate) {
                                const dateObject = new Date(ModifyDate);
                                if (!isNaN(dateObject)) {
                                    const day = String(dateObject.getDate()).padStart(2, '0');
                                    const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                                    const year = dateObject.getFullYear();
                                    formattedDate = `${day}/${month}/${year}`;
                                }
                            }
                            html += '<tr>';
                            html += '<td>' + formattedDate + '</td>';
                            html += '<td>' + Recipt.VouvherNo + '</td>';
                            html += '<td>' + Recipt.narration + '</td>';
                            html += '<td></td>';
                            html += '<td>' + Recipt.Amount + '</td>';
                            Balance -= Recipt.Amount
                            html += '<td>' + Math.abs(Balance) + '</td>';
                            if (Balance > 0) {
                                html += '<td>DR</td>';
                            } else {
                                html += '<td>CR</td>';
                            }
                            html += '</tr >';
                        });
                        PrintDataDetailed = {
                            FromDate: fromDateDetailed.val(),
                            ToDate: toDateDetailed.val(),
                            Party: [result.Party],
                            PartyName: result.Party.PartyName
                        }
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="7">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblDetailedList').html(html);
                    if (!$.fn.DataTable.isDataTable('.DetailedReportTable')) {
                        var table = $('.DetailedReportTable').DataTable({
                            "responsive": true, "lengthChange": false, "autoWidth": false,
                            "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
                        }).buttons().container().appendTo('#example2_wrapper .col-md-6:eq(0)');
                    }
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
        console.log(PrintDataDetailed);
        $.ajax({
            type: "POST",
            url: '/Print/CustomerDetailedPrintData',
            dataType: 'json',
            data: JSON.stringify(PrintDataDetailed),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                window.open(Response.redirectTo, '_blank');
            },
        });
    });

    //-----------------------------------------------------------------Transation Detailed Report --------------------------------------------//
    TGetSundryDebtors();
    function TGetSundryDebtors() {
        $.ajax({
            url: "/Transaction/GetSundryDebtors",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    TddlCustomer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    TddlCustomer.append(defaultOption);
                    $.each(result.SubLedgers, function (key, item) {
                        var option = $('<option></option>').val(item.SubLedgerId).text(item.SubLedgerName);
                        TddlCustomer.append(option);
                    });
                }
                else {
                    TddlCustomer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    TddlCustomer.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
})