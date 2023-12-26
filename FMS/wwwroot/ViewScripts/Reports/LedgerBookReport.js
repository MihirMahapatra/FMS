$(function () {
    //----------------------------------------varible declaration-----------------------------------------//
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    const fromDate = $('input[name="FromDate"]');
    fromDate.val(todayDate);
    const toDate = $('input[name="ToDate"]');
    toDate.val(todayDate);
    const ClosingBal = $('input[name="ClosingBal"]');
    var Ladger = $('select[name="ddlLadgerId"]');
    //-----------------------------------------------------Stock Report Scren --------------------------------------------------//
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
    $('#btnView').on('click', function () {
        $('#loader').show();
        $('.LadgerBookReportTable').empty();

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
                LedgerId: Ladger.val()
            };
            $.ajax({
                url: "/Reports/LedgerBookReport",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 LadgerBookReportTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th>Date</th>'
                    html += '<th>Particulurs</th>'
                    html += '<th>Voucher Type</th>'
                    html += '<th>Voucher Number</th>'
                    html += '<th>Debit</th>'
                    html += '<th>Credit</th>'
                    html += '<th>Balance</th>'
                    html += '<th>Type</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $(".hddiv").show();
                        ClosingBal.val(Math.abs(result.LedgerBook.ClosingBal));
                        var totalAmount = 0;
                        function formatDate(dateString) {
                            var date = new Date(dateString);
                            var day = date.getDate().toString().padStart(2, '0');
                            var month = (date.getMonth() + 1).toString().padStart(2, '0');
                            var year = date.getFullYear();
                            return day + '/' + month + '/' + year;
                        }

                        html += '<tr>';
                        html += '<td>' + "-" + '</td>';
                        html += '<td>' + "-" + '</td>';
                        html += '<td>' + "-" + '</td>';
                        html += '<td>' + "-" + '</td>';
                        html += '<td>' + "-" + '</td>';
                        html += '<td>' + "Opening bal" + '</td>';
                        html += '<td>' + result.LedgerBook.OpeningBal + '</td>';
                        if (result.LedgerBook.OpeningBal > 0) {
                            html += '<td>' + "Cr" + '</td>';
                        } else {
                            html += '<td>' + "Dr" + '</td>';
                        }
                        html += '</tr >';

                        $.each(result.LedgerBook.Payments, function (key, item) {
                            html += '<tr>';
                            html += '<td>' + formatDate(item.VoucherDate) + '</td>';
                            html += '<td>' + item.narration + '</td>';
                            html += '<td>' + "Payment" + '</td>';
                            html += '<td>' + item.VouvherNo + '</td>';
                            html += '<td>' + item.Amount + '</td>';
                            html += '<td>' + "0.00" + '</td>';
                            totalAmount += parseFloat(item.Amount);
                            html += '<td>' + totalAmount.toFixed(2) + '</td>';
                            html += '<td>' + item.DrCr + '</td>';
                            html += '</tr >';
                        });
                        $.each(result.LedgerBook.Receipts, function (key, item) {
                            html += '<tr>';
                            html += '<td>' + formatDate(item.VoucherDate) + '</td>';
                            html += '<td>' + item.narration + '</td>';
                            html += '<td>' + "Receipts" + '</td>';
                            html += '<td>' + item.VouvherNo + '</td>';
                            html += '<td>' + "0.00" + '</td>';
                            html += '<td>' + item.Amount + '</td>';
                            totalAmount -= parseFloat(item.Amount);
                            html += '<td>' + totalAmount.toFixed(2) + '</td>';
                            html += '<td>' + item.DrCr + '</td>';
                            html += '</tr >';
                        });
                        $.each(result.LedgerBook.Journals, function (key, item) {
                            html += '<tr>';
                            html += '<td>' + formatDate(item.VoucherDate) + '</td>';
                            html += '<td>' + item.narration + '</td>';
                            html += '<td>' + "Journals" + '</td>';
                            html += '<td>' + item.VouvherNo + '</td>';
                            html += '<td>' + "0.00" + '</td>';
                            html += '<td>' + item.Amount + '</td>';
                            totalAmount -= parseFloat(item.Amount);
                            html += '<td>' + totalAmount.toFixed(2) + '</td>';
                            html += '<td>' + item.DrCr + '</td>';
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
                    $('.LadgerBookTransationsDetails').html(html);
                    if (!$.fn.DataTable.isDataTable('.LadgerBookReportTable')) {
                        var table = $('.LadgerBookReportTable').DataTable({
                            "responsive": true, "lengthChange": false, "autoWidth": false,
                            "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
                        }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');
                    }
                    PrintData = {
                        OpeningBal: parseFloat((Math.abs(result.LedgerBook.OpeningBal))),
                        ClosingBal: parseFloat((Math.abs(result.LedgerBook.ClosingBal))),
                        Receipts: result.LedgerBook.Receipts,
                        Payments: result.LedgerBook.Payments,
                        Journals: result.LedgerBook.Journals,
                        LedgerName: result.LedgerBook.LedgerName,
                        FromDate: fromDate.val(),
                        Todate: toDate.val(),
                    };
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
    $("#btnPrint").click(function () {
        $.ajax({
            type: "POST",
            url: '/Print/LedgerBookPrintData',
            dataType: 'json',
            data: JSON.stringify(PrintData),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                window.open(Response.redirectTo, '_blank');

            },
        });
    });
});