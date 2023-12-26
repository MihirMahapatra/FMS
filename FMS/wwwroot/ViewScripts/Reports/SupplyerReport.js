$(function () {
    //Default Date 
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#ReportsLink").addClass("active");
    $("#SupplyerReportLink").addClass("active");
    $("#SupplyerReportLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const fromDateSummerized = $('input[name="FromDateSummerized"]');
    fromDateSummerized.val(todayDate);
    const toDateSummerized = $('input[name="ToDateSummerized"]');
    toDateSummerized.val(todayDate);
    const ddlZeroValued = $('select[name="ddlZerovalued"]');
    const ddlSupplyer = $('select[name="ddlSupplyerId"]');
    const fromDateDetailed = $('input[name="FromDateDetailed"]');
    fromDateDetailed.val(todayDate);
    const toDateDetailed = $('input[name="ToDateDetailed"]');
    toDateDetailed.val(todayDate);
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
                url: "/Reports/GetSummerizedSupplyerReport",
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
                    html += '<th>Bill Amt</th>'
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
                        })

                        PrintData = {
                            FromDate : fromDateSummerized.val(),
                            ToDate : toDateSummerized.val(),
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
            url: '/Print/SupplyerSummarizedPrintData',
            dataType: 'json',
            data: JSON.stringify(PrintData),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                window.open(Response.redirectTo, '_blank');
            },
        });
    });
    //--------------------------------Customer Report Detailed------------------------------------------------//
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

    var Printdatadetails = {}; 
    $('#btnViewDetailed').on('click', function () {
        $('#loader').show();
        $('.DetailedReportTable').empty();
        if (!ddlSupplyer.val() || ddlSupplyer.val() === '--Select Option--') {
            toastr.error('Supplyer Name Is Required.');
            return;
        }
       else if (!toDateDetailed.val()) {
            toastr.error('ToDate Is Required.');
            return;
        }else if (!fromDateDetailed.val()) {
            toastr.error('FromDate Is Required.');
            return;
        } 
        else {
            var requestData = {
                FromDate: fromDateDetailed.val(),
                ToDate: toDateDetailed.val(),
                PartyId: ddlSupplyer.val()
            };
            $.ajax({
                url: "/Reports/GetDetailedSupplyerReport",
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
                    html += '<th>Bill Amt</th>'
                    html += '<th>Paid Amt</th>'
                    html += '<th>Balance</th>'
                    html += '<th>Type</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $('#BtnPrintDetailed').show();
                        var Balance = 0;
                        if (result.Party.OpeningBalType === 'Dr') {
                            Balance = result.Party.OpeningBal;
                        }
                        else {
                            Balance = -result.Party.OpeningBal;
                        }
                        html += '<tr>';
                        html += '<td></td>';
                        html += '<td></td>';
                        html += '<td>Opeining Bal</td>';
                        html += '<td></td>';
                        html += '<td></td>';
                        html += '<td>' + result.Party.OpeningBal + '</td>';
                        html += '<td>' + result.Party.OpeningBalType + '</td>';
                        html += '</tr >';
                        if (result.Party.PurchaseOrders !== null) {
                            $.each(result.Party.PurchaseOrders, function (key, PurchaseOrder) {
                                html += '<tr>';
                                html += '<td>' + PurchaseOrder.TransactionDate + '</td>';
                                html += '<td>' + PurchaseOrder.TransactionNo + '</td>';
                                html += '<td>Credit Purchase</td>';
                                html += '<td>' + PurchaseOrder.GrandTotal + '</td>';
                                html += '<td></td>';
                                Balance += PurchaseOrder.GrandTotal
                                html += '<td>' + Math.abs(Balance) + '</td>';
                                if (Balance > 0) {
                                    html += '<td>Cr</td>';
                                }
                                else {
                                    html += '<td>Dr</td>';
                                }
                                html += '</tr >';
                            });
                            $.each(result.Party.payments, function (key, payment) {
                                html += '<tr>';
                                html += '<td>' + payment.VoucherDate + '</td>';
                                html += '<td>' + payment.VouvherNo + '</td>';
                                html += '<td>' + payment.narration + '</td>';
                                html += '<td></td>';
                                html += '<td>' + payment.Amount + '</td>';
                                Balance -= payment.Amount
                                html += '<td>' + Math.abs(Balance) + '</td>';
                                if (Balance > 0) {
                                    html += '<td>Cr</td>';
                                } else {
                                    html += '<td>Dr</td>';
                                }
                                html += '</tr >';
                            });
                        }
                        Printdatadetails = {
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
        console.log(Printdatadetails);
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
})