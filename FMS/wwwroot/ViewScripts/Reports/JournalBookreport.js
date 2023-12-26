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
    //-----------------------------------------------------Stock Report Scren --------------------------------------------------//
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
            };
            $.ajax({
                url: "/Reports/JournalBookReport",
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
                    html += '<th>VoucherNumber</th>'
                    html += '<th>Date</th>'
                    html += '<th>Ladger</th>'
                    html += '<th>SubLadger</th>'
                    html += '<th>Cr</th>'
                    html += '<th>Dr</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $(".hddiv").show();
                        var isFirstJournal = true;
                        $.each(result.GroupedJournals, function (key, item) {
                            var TotalCr = 0;
                            var TotalDr = 0;
                            $.each(item.Journals, function (key, journal) {
                                html += '<tr>';
                                if (isFirstJournal) {
                                    html += '<td rowspan="' + item.Journals.length + '">' + item.VoucherNo + '</td>';
                                    html += '<td rowspan="' + item.Journals.length + '">' + journal.VoucherDate + '</td>';
                                    isFirstJournal = false;
                                }
                                if (journal.LedgerDevName !== null) {
                                    html += '<td>' + journal.LedgerDevName + '</td>';
                                } else {
                                    html += '<td>' + journal.LedgerName + '</td>';
                                }
                                html += '<td>' + journal.SubLedgerName + '</td>';
                                if (journal.DrCr !== "CR") {
                                    html += '<td>' + journal.Amount + '</td>';
                                    TotalCr += journal.Amount;
                                    html += '<td>' + "-" + '</td>';
                                } else {
                                    html += '<td>' + "-" + '</td>';
                                    html += '<td>' + journal.Amount + '</td>';
                                    TotalDr += journal.Amount;
                                }
                                html += '</tr>';
                            })
                            html += '<tr>';
                            html += '<td colspan="2">' + "-" + '</td>';
                            html += '<td>' + "-" + '</td>';
                            html += '<td>' + "Total" + '</td>';
                            html += '<td>' + TotalCr + '</td>';
                            html += '<td>' + TotalDr + '</td>';
                            html += '</tr>';
                            isFirstJournal = true; 
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
                        FromDate: fromDate.val(),
                        Todate: toDate.val(),
                        GroupedJournals: result.GroupedJournals
                    };
                    console.log(PrintData);
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
            url: '/Print/JournalBookPrintData',
            dataType: 'json',
            data: JSON.stringify(PrintData),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                window.open(Response.redirectTo, '_blank');
            },
        });
    });
});