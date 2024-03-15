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
    const ddlCustomer = $('select[name="ddlCustomerId"]');
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
                    html += '<th></th>'
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
                        $.each(result.PartySummerized, function (key, item) {
                            html += '<tr>';
                            html += '<td><button  class="btn btn-primary btn-sm toggleColumnsBtn" id="btn-info-' + item.Fk_SubledgerId + '"  data-id="' + item.Fk_SubledgerId + '" style=" border-radius: 50%;" ><i class="fa-solid fa-circle-info"></i></button></td>'
                            html += '<td>' + item.PartyName + '</td>';
                            html += '<td>' + item.OpeningBal + '</td>';
                            html += '<td>' + item.OpeningBalType + '</td>';
                            html += '<td>' + item.DrAmt + '</td>';
                            html += '<td>' + item.CrAmt + '</td>';
                            var balance = item.Balance + item.OpeningBal;
                            var balanceType = balance >= 0 ? "Dr" : "Cr";
                            html += '<td>' + Math.abs(balance) + '</td>';
                            html += '<td>' + balanceType + '</td>';
                            html += '</tr>';
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
    $(document).on('click', '.toggleColumnsBtn', (event) => {
        const value = $(event.currentTarget).data('id');
        var requestData = {
            FromDate: fromDateSummerized.val(),
            ToDate: toDateSummerized.val(),
            Fk_SubledgerId: value
        };
        $.ajax({
            url: "/Reports/GetBranchWiseCustomerInfo",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            data: JSON.stringify(requestData),
            success: function (result) {
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-2 SummerizedCustomerInfoTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th>Branch</th>'
                html += '<th>Closing Bal</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    var totalClosing = 0;
                    $.each(result.PartyInfos, function (key, item) {
                        html += '<tr>';
                        html += '<td>' + item.BranchName + '</td>';
                        closingBal = item.OpeningBalance + item.RunningBalance;
                        closingBalType = closingBal >= 0 ? "Dr" : "Cr";
                        totalClosing += closingBal
                        html += '<td>' + Math.abs(closingBal) + ' ' + closingBalType + '</td>';
                        html += '</tr >';
                    });
                    html += '<tr>';
                    html += '<td> Total Closing</td>';
                    html += '<td >' + totalClosing + ' </td>';
                    html += '</tr> ';
                }
                else {
                    html += '<tr>';
                    html += '<td colspan="3">No Record</td>';
                    html += '</tr >';
                }
                html += ' </tbody>';
                html += '</table >';
                $('.tblSummerizedInfo').html(html);
            },
            error: function (errormessage) {
                Swal.fire(
                    'Error!',
                    'An error occurred',
                    'error'
                );
            }
        });
        $('#modal-customer-info').modal('show');
    });
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
                                html += '<td >Challan No</td>';
                                html += '<td >Site Adress</td>';
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
                                        html += '<td >Unit</td>';
                                        html += '<td >Qty</td>';
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
                                            html += '<td >' + Transaction.Unit + '</td>';
                                            html += '<td >' + Transaction.Quantity +'</td>';
                                            html += '<td >' + Transaction.Rate +'</td>';
                                            html += '<td >' + Transaction.Amount +'</td>';
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
                        PrintDataDetailed = {
                            FromDate: fromDateDetailed.val(),
                            ToDate: toDateDetailed.val(),
                            PartyDetailedReports: result.PartyDetailed
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
})