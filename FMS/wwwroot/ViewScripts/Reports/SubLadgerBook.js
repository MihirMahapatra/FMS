$(function () {
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#ReportsLink").addClass("active");
    $("#SubLedgerBookLink").addClass("active");
    $("#SubLedgerBookLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const fromDateSummerized = $('input[name="FromDateSummerized"]');
    fromDateSummerized.val(todayDate);
    const toDateSummerized = $('input[name="ToDateSummerized"]');
    toDateSummerized.val(todayDate);
    var SLadger = $('select[name="ddlSummrizedLadgerId"]');
    var Ladger = $('select[name="ddlLadgerId"]');
    const ddlSubLadger = $('select[name="ddlSubLadgerId"]');
    const fromDateDetailed = $('input[name="FromDateDetailed"]');
    fromDateDetailed.val(todayDate);
    const toDateDetailed = $('input[name="ToDateDetailed"]');
    toDateDetailed.val(todayDate);
    //--------------------------------Customer Report Summerized------------------------------------------------//
    var PrintData = {}
    GetSummrizedLadgers();
    function GetSummrizedLadgers() {
        $.ajax({
            url: '/Accounting/GetLedgers',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                SLadger.empty();
                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                SLadger.append(defaultOption);
                if (result.ResponseCode == 302) {
                    $.each(result.Ledgers, function (key, item) {
                        var option = $('<option></option>').val(item.LedgerId).text(item.LedgerName);
                        SLadger.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        })
    }
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
                LedgerId: SLadger.val()
            };
            $.ajax({
                url: "/Reports/GetSummerizedSubLadgerReport",
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
                    html += '<th>Dr Amt</th>'
                    html += '<th>Cr Amt</th>'
                    html += '<th>Balance</th>'
                    html += '<th>Type</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $('#BtnPrintSummarized').show();
                        $.each(result.PartySummerized, function (key, item) {
                            html += '<tr>';
                            html += '<td>' + item.PartyName + '</td>';
                            html += '<td>' + item.OpeningBal + '</td>';
                            html += '<td>' + item.OpeningBalType + '</td>';
                            html += '<td>' + item.DrAmt + '</td>';
                            html += '<td>' + item.CrAmt + '</td>';
                            var balance = item.Balance + item.OpeningBal;
                            var balanceType = balance >= 0 ? "Dr" : "Cr";
                            html += '<td>' + Math.abs(balance) + '</td>';
                            html += '<td>' + balanceType + '</td>';
                            html += '<tr>';
                        })
                        PrintData = {
                            FromDate: fromDateSummerized.val(),
                            ToDate: toDateSummerized.val(),
                            PartyReports: result.PartySummerized
                        }
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="8">No Record</td>';
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
    });
    //--------------------------------Customer Report Detailed------------------------------------------------//
    GetLadgers();
    function GetLadgers() {
        $.ajax({
            url: '/Accounting/GetLedgers',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                Ladger.empty();
                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                Ladger.append(defaultOption);
                if (result.ResponseCode == 302) {
                    $.each(result.Ledgers, function (key, item) {
                        var option = $('<option></option>').val(item.LedgerId).text(item.LedgerName);
                        Ladger.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        })
    }
    var selectedOption = "";
    $(document).on('change', '.ledgerType', function () {
        selectedOption = $(this).val();
        if (selectedOption) {
            $.ajax({
                url: '/Accounting/GetSubLedgersById?LedgerId=' + selectedOption + '',
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        ddlSubLadger.empty();
                        var defaultOption = $('<option></option>').val('').text('--Select Option--');
                        ddlSubLadger.append(defaultOption);
                        $.each(result.SubLedgers, function (key, item) {
                            var option = $('<option></option>').val(item.SubLedgerId).text(item.SubLedgerName);
                            ddlSubLadger.append(option);
                        });
                        ddlSubLadger.prop('disabled', false);
                    }
                    else {
                        ddlSubLadger.empty();
                        var defaultOption = $('<option></option>').val('').text('--Select Option--');
                        ddlSubLadger.append(defaultOption);
                        ddlSubLadger.prop('disabled', false);
                    }

                },
                error: function (errormessage) {
                    console.log(errormessage)
                }
            });
           
        }
    });
    //function GetSubLadgersDebtors() {
    //    $.ajax({
    //        url: "/Reports/GetSubLadgers",
    //        type: "GET",
    //        contentType: "application/json;charset=utf-8",
    //        dataType: "json",
    //        success: function (result) {
    //            if (result.ResponseCode == 302) {
    //                ddlSubLadger.empty();
    //                var defaultOption = $('<option></option>').val('').text('--Select Option--');
    //                ddlSubLadger.append(defaultOption);
    //                $.each(result.SubLedgers, function (key, item) {
    //                    var option = $('<option></option>').val(item.SubLedgerId).text(item.SubLedgerName);
    //                    ddlSubLadger.append(option);
    //                });
    //            }
    //            else {
    //                ddlSubLadger.empty();
    //                var defaultOption = $('<option></option>').val('').text('--Select Option--');
    //                ddlSubLadger.append(defaultOption);
    //            }
    //        },
    //        error: function (errormessage) {
    //            console.log(errormessage)
    //        }
    //    });
    //}
    var requestData = {};
    $('#btnViewDetailed').on('click', function () {
        $('#loader').show();
        $('.DetailedReportTable').empty();
        if (!ddlSubLadger.val() || ddlSubLadger.val() === '--Select Option--') {
            toastr.error('SubLadger Name Is Required.');
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
             requestData = {
                FromDate: fromDateDetailed.val(),
                ToDate: toDateDetailed.val(),
                LedgerId: ddlSubLadger.val()
            };
            $.ajax({
                url: "/Reports/SubLedgerDetailedBookReport",
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
                                html += '<td >Trxn No</td>';
                                html += '<td colspan="4">Narration</td>';
                                html += '<td>Dr</td>';
                                html += '<td>Cr</td>';
                                html += '<td>Runnig Bal</td>';
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
                                    html += '<td >' + item.TransactionNo + '</td>';
                                    html += '<td colspan="4">' + item.Naration + '</td>';
                                   
                                    if (item.DrCr === "Dr") {
                                        html += '<td>' + item.GrandTotal + '</td>';
                                        html += '<td>'+0.00 +'</td>';
                                        balance += item.GrandTotal;
                                    } else if (item.DrCr === "Cr") {
                                        html += '<td>'+0.00 +'</td>';
                                        html += '<td>' + item.GrandTotal + '</td>';
                                        balance -= item.GrandTotal;
                                    }
                                    var DrCr = balance >= 0 ? "Dr" : "Cr";
                                    html += '<td>' + balance.toFixed(2) + ' ' + DrCr + '</td>';
                                    html += '</tr >';
                                });
                            }
                            
                            html += '<tr style="Background-color:cyan;">';
                            html += '<td colspan="8">Closing Bal.</td>';
                            var DrCr = balance > 0 ? "Dr" : "Cr";
                            html += '<td>' + balance.toFixed(2) + ' ' + DrCr + '</td>';
                            html += '</tr >';
                        }
                        //PrintDataDetailed = {
                        //    FromDate: fromDateDetailed.val(),
                        //    ToDate: toDateDetailed.val(),
                        //    PartyDetailedReports: result.PartyDetailed
                        //}
                        $('#BtnPrintDetailed').show();
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="8">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblDetailedList').html(html);
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
        console.log(requestData);
        var queryString = $.param(requestData); // Serialize object to query string
        var url = '/Print/SubLadgerDetailedReportPrint?' + queryString; // Append query string to URL
        window.open(url, '_blank');
    });
})