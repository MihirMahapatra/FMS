$(function () {
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;

    $("#ReportsLink").addClass("active");
    $("#LabourReportLink").addClass("active");
    $("#LabourReportLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const fromDate = $('input[name="FromDate"]');
    fromDate.val(todayDate);
    const toDate = $('input[name="ToDate"]');
    toDate.val(todayDate);
    const fromDateDetailed = $('input[name="FromDateDetailed"]');
    fromDateDetailed.val(todayDate);
    const toDateDetailed = $('input[name="ToDateDetailed"]');
    toDateDetailed.val(todayDate);
    const ddlLabour = $('select[name="ddlLabour"]');
    //-----------------------------------------------------Stock Report Scren --------------------------------------------------//
    function getPrintHeaderFooter() {
        var headerFooter =
            '<div style="font-size: 12px; color: red; font-weight: bold;">Bhuasuni Precast</div>' +
            '<div style="font-size: 10px; color: blue; text-align: center;">Address | Phone: 123-456-7890 | Email: info@example.com</div>' +
            '<div style="font-size: 10px; text-align: right;">Printed on: ' + new Date().toLocaleString() + '</div>';
        return headerFooter;
    }
    $('#btnViewSummerized').on('click', function () {
        $('#loader').show();
        $('.SummerizedLabourReportTable').empty();

        if (!fromDate.val()) {
            toastr.error('FromDate Is Required.');
            return;
        } else if (!toDate.val()) {
            toastr.error('ToDate Is Required.');
            return;
        }
        else {
            var requestData = {
                FromDate: fromDate.val(),
                ToDate: toDate.val(),
            };
            $.ajax({
                url: "/Reports/GetSummerizedLabourReport",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 SummerizedLabourReportTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th>Labour Name</th>'
                    html += '<th>Opening Bal</th>'
                    html += '<th>Balance Type</th>'
                    html += '<th>Billing Amt</th>'
                    html += '<th>Damage Amt(-)</th>'
                    html += '<th>Payment Amt(-)</th>'
                    html += '<th>Balance</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $.each(result.LaborReports, function (key, item) {
                            html += '<tr>';
                            html += '<td>' + item.LabourName + '</td>';
                            html += '<td>' + item.OpeningBal + '</td>';
                            html += '<td>' + item.OpeningBalType + '</td>';
                            html += '<td>' + item.BillingAmt + '</td>';
                            html += '<td>' + item.DamageAmt + '</td>';
                            html += '<td>' + item.PaymentAmt + '</td>';
                            var Balance = item.OpeningBal + (item.BillingAmt - item.DamageAmt - item.PaymentAmt);
                            html += '<td>' + Balance + '</td>';
                            html += '</tr >';
                        });
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="6">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblSummerizedLabourList').html(html);
                    if (!$.fn.DataTable.isDataTable('.SummerizedLabourReportTable')) {
                        var table = $('.SummerizedLabourReportTable').DataTable({
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
    GetLabours()
    function GetLabours() {
        $.ajax({
            url: "/Master/GetAllLabourDetails",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlLabour.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlLabour.append(defaultOption);
                    $.each(result.Labours, function (key, item) {
                        var option = $('<option></option>').val(item.LabourId).text(item.LabourName);
                        ddlLabour.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage);
            }
        });
    }
    $('#btnViewDetailed').on('click', function () {
        $('#loader').show();
        $('.DetailedLabourReportTable').empty();
        if (!fromDateDetailed.val()) {
            toastr.error('FromDate Is Required.');
            return;
        } else if (!toDateDetailed.val()) {
            toastr.error('ToDate Is Required.');
            return;
        } else if (!ddlLabour.val() || ddlLabour.val() === '--Select Option--') {
            toastr.error('Labour Name Is Required.');
            return;
        }
        else {
            var requestData = {
                FromDate: fromDateDetailed.val(),
                ToDate: toDateDetailed.val(),
                LabourId: ddlLabour.val()
            };
            $.ajax({
                url: "/Reports/GetDetailedLabourReport",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    console.log(result);
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 DetailedLabourReportTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th>Date</th>'
                    html += '<th>Trxn No</th>'
                    html += '<th>Particular</th>'
                    html += '<th>Qty</th>'
                    html += '<th>Rate</th>'
                    html += '<th>Billing</th>'
                    html += '<th>Damage</th>'
                    html += '<th>Payment</th>'
                    html += '<th>Balance</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {

                        $.each(result.Labours, function (key, item) {
                            var Balance = item.OpeningBalance;
                            html += '<tr>';
                            html += '<td>-</td>';
                            html += '<td>-</td>';
                            html += '<td>Opening Bal.</td>';
                            html += '<td>-</td>';
                            html += '<td>-</td>';
                            html += '<td>-</td>';
                            html += '<td>-</td>';
                            html += '<td>-</td>';
                            html += '<td>' + item.OpeningBalance + '</td>';
                            html += '</tr >';
                            if (item.ProductionEntries !== null) {
                                $.each(item.ProductionEntries, function (key, Production) {
                                    html += '<tr>';
                                    html += '<td>' + Production.ProductionDate + '</td>';
                                    html += '<td>' + Production.ProductionNo + '</td>';
                                    html += '<td>' + Production.Product.ProductName + '</td>';
                                    html += '<td>' + Production.Quantity + '</td>';
                                    html += '<td>' + Production.Rate + '</td>';
                                    html += '<td>' + Production.Amount + '</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    Balance += Production.Amount
                                    html += '<td>' + Balance + '</td>';
                                    html += '</tr >';
                                });
                            }
                            if (item.Payment !== null) {
                                $.each(item.Payment, function (key, Payment) {
                                    html += '<tr>';
                                    html += '<td>' + Payment.VoucherDate + '</td>';
                                    html += '<td>' + Payment.VouvherNo + '</td>';
                                    html += '<td>Payment</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    html += '<td>' + Payment.Amount + '</td>';
                                    Balance -= Payment.Amount
                                    html += '<td>' + Balance + '</td>';
                                    html += '</tr >';
                                });

                            }
                            if (item.DamageOrders !== null) {
                                $.each(item.DamageOrders, function (key, Damage) {
                                    html += '<tr>';
                                    html += '<td>' + Damage.TransactionDate + '</td>';
                                    html += '<td>' + Damage.TransactionNo + '</td>';
                                    html += '<td>Damage</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    html += '<td>' + Damage.TotalAmount + '</td>';
                                    html += '<td>-</td>';
                                    Balance -= Damage.TotalAmount
                                    html += '<td>' + Balance + '</td>';
                                    html += '</tr >';
                                });
                            }
                        });
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="9">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblDetailedLabourList').html(html);
                    if (!$.fn.DataTable.isDataTable('.DetailedLabourReportTable')) {
                        var table = $('.DetailedLabourReportTable').DataTable({
                            "responsive": true,
                            "lengthChange": false,
                            "autoWidth": false,
                            "buttons": [
                                {
                                    extend: 'copy',
                                    footer: true
                                },
                                {
                                    extend: 'csv',
                                    footer: true
                                },
                                {
                                    extend: 'excel',
                                    footer: true
                                },
                                {
                                    extend: 'pdf',
                                    footer: true,
                                    title: function () {
                                        return getPrintHeaderFooter();
                                    }
                                },
                                {
                                    extend: 'print',
                                    footer: true,
                                    title: function () {
                                        return getPrintHeaderFooter();
                                    },

                                },
                                {
                                    extend: 'colvis'
                                }
                            ]
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
    });
});